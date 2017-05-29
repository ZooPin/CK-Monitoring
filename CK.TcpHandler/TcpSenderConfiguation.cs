using System;
using System.Collections.Generic;
using System.Text;
using CK.Monitoring;
using System.Net;

namespace CK.TcpHandler
{
    public class TcpSenderConfiguation : IHandlerConfiguration
    {
        public IPAddress Adress { get; set; }
        public int Port { get; set; }

        public IHandlerConfiguration Clone()
        {
            return this;
        }
    }
}
