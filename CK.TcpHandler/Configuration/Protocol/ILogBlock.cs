using CK.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CK.TcpHandler.Configuration.Protocol
{
    public enum LogType
    {
        Critical,
        CKMonitoring,
        DotNetLogger
    }

    public interface ILogBlock
    {
        LogType Type { get; set; }

        byte[] Log { get; set; }

        void WriteLogBlock(CKBinaryWriter w);
    }
}
