using CK.Core;
using CK.Monitoring;
using CK.TcpHandler.Configuration.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CK.TcpHandler.Helper
{
    internal class TcpHelper : IDisposable
    {
        TcpClient _client;
        NetworkStream _writer;

        public NetworkStream Stream
        {
            get
            {
                return _writer;
            }
        }

        public TcpClient Client
        {
            get
            {
                return _client;
            }
        }

        public TcpHelper()
        {
            _client = new TcpClient();
        }

        public async Task<bool> ConnectAsync(IPAddress adress, int port)
        {
            try
            {
                int appId = 122;
                await _client.ConnectAsync(adress, port);
                _writer = _client.GetStream();
                await WriteAsync(BlockConstructor.Open(appId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<bool> WriteAsync(string message)
        {
            return await WriteAsync(Encoding.ASCII.GetBytes(message));
        }

        public async Task<bool> WriteAsync(IMulticastLogEntry e)
        {
            byte[] log = BinaryHelper.IMulticastLogEntryToBinary(e);
            return await WriteAsync(BlockConstructor.Log(LogType.CKMonitoring, log));
        }

        public async Task<bool> WriteAsync(Byte[] data)
        {
            if (_writer == null) throw new NullReferenceException(nameof(_writer));
            if (!_writer.CanWrite) return false;

            await _writer.WriteAsync(ConstructHeader(data.Length), 0, 4);
            await _writer.WriteAsync(data, 0, data.Length);
            await _writer.FlushAsync();

            return true;
        }

        async Task SendDisconnect()
        {
            if (_writer == null) throw new NullReferenceException(nameof(_writer));
            if (!_writer.CanWrite) return;

            await _writer.WriteAsync(ConstructHeader(0), 0, 4);
        }

        private byte[] ConstructHeader(int size)
        {
            return new byte[] { (byte)(size >> 24), (byte)(size >> 16), (byte)(size >> 8), (byte)size };
        }

        public void Dispose()
        {
            SendDisconnect().Wait();
            _client.GetStream().Flush();
            _client.GetStream().Dispose();
            _client.Dispose();
        }
    }
}
