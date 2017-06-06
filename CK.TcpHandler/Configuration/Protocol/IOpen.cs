using CK.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CK.TcpHandler.Configuration.Protocol
{
    public interface IOpen 
    {
        string BaseDirectory { get; set; }
        int AppId { get; set; }
        int StreamVersion { get; set; } 
        Dictionary<string, string> Info { get; set; }
        void WriteOpenBlock(CKBinaryWriter w);
    }
}
