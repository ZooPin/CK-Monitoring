using CK.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using CK.Core;
using System.IO;
using System.Threading.Tasks;
using CK.TcpHandler.Helper;
using System.Collections.Concurrent;

namespace CK.TcpHandler
{
    public class TcpSender : IGrandOutputHandler
    {
        readonly BlockingCollection<object> _criticalAndExternals;
        TcpSenderConfiguration _config;
        TcpHelper _tcp;

        public TcpSender (TcpSenderConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
            _tcp = new TcpHelper();
            _criticalAndExternals = new BlockingCollection<object>();
        }

        public bool Activate(IActivityMonitor m)
        {
            using (m.OpenGroup(LogLevel.Trace, $"Initializing Tcp handler (Address = {_config.Address}, Port {_config.Port}).", null))
            {
                if(_config.HandleCriticalErrors)
                {
                    SystemActivityMonitor.OnError += SystemActivityMonitor_OnError;
                }
                return _tcp.ConnectAsync(_config.Address, _config.Port).Result;
            }
        }

        private void SystemActivityMonitor_OnError(object sender, SystemActivityMonitor.LowLevelErrorEventArgs e)
        {
            _criticalAndExternals.Add(e);
        }

        private void DumpCriticalAndExternals()
        {
            foreach(object e in _criticalAndExternals)
            {
                var critical = e as SystemActivityMonitor.LowLevelErrorEventArgs;
                if(critical != null)
                {
                    byte[] t = BlockConstructor.Log(Configuration.Protocol.LogType.Critical, critical.ErrorMessage);
                    _tcp.WriteAsync(t).Wait();
                }
                //else
                //{
                //    /* Handdle external */
                //}
            }
        }

        public bool ApplyConfiguration(IActivityMonitor m, IHandlerConfiguration c)
        {
            TcpSenderConfiguration cF = c as TcpSenderConfiguration;
            if (cF == null || cF.Address != _config.Address) return false;
            _config = cF;
            _tcp.Dispose();
            _tcp.ConnectAsync(_config.Address, _config.Port).Wait();

            return true;
        }

        public void Deactivate(IActivityMonitor m)
        {
            if (_config.HandleCriticalErrors)
            {
                SystemActivityMonitor.OnError -= SystemActivityMonitor_OnError;
            }
            _tcp.Dispose();
        }

        public void Handle(GrandOutputEventInfo logEvent)
        {
            DumpCriticalAndExternals();
            _tcp.WriteAsync(logEvent.Entry).Wait();
        }

        public void OnTimer(TimeSpan timerSpan)
        {
            DumpCriticalAndExternals();
        }
    }
}
