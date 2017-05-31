using CK.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using CK.Core;
using System.IO;
using System.Threading.Tasks;
using CK.TcpHandler.Helper;

namespace CK.TcpHandler
{
    public class TcpSender : IGrandOutputHandler
    {
        TcpSenderConfiguration _config;
        TcpHelper _tcp;

        public TcpSender (TcpSenderConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
            _tcp = new TcpHelper();
        }

        public bool Activate(IActivityMonitor m)
        {
            using (m.OpenGroup(LogLevel.Trace, $"Initializing Tcp handler (Address = {_config.Address}, Port {_config.Port}).", null))
            {
                return _tcp.ConnectAsync(_config.Address, _config.Port).Result;
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
            _tcp.Dispose();
        }

        public void Handle(GrandOutputEventInfo logEvent)
        {
            _tcp.WriteAsync(logEvent.Entry).Wait();
        }

        public void OnTimer(TimeSpan timerSpan)
        {
        }
    }
}
