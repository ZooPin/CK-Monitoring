using System;
using System.Collections.Generic;
using System.Text;
using CK.Monitoring;
using System.Net;

namespace CK.TcpHandler
{
    public class TcpSenderConfiguration : IHandlerConfiguration
    {
        public IPAddress Address { get; set; }
        public int Port { get; set; }

        public IHandlerConfiguration Clone()
        {
            return new TcpSenderConfiguration()
            {
                Address = Address,
                Port = Port
            };
        }
    }
}
