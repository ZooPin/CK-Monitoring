using System;
using System.Collections.Generic;
using System.Text;
using CK.Core;

namespace CK.TcpHandler.Configuration.Protocol
{
    public class OpenInfo : IOpen
    {
        public string BaseDirectory { get; set; }
        public int AppId { get; set; }
        public int StreamVersion { get; set; }
        public Dictionary<string, string> Info { get; set; }

        public void WriteOpen(CKBinaryWriter w)
        {
            w.Write(BaseDirectory);
            w.Write(AppId);
            w.Write(StreamVersion);
            w.Write(Info.Count);
            foreach(var i in Info)
            {
                w.Write(i.Key);
                w.Write(i.Value);
            }
        }

        public static IOpen Read(CKBinaryReader r)
        {
            IOpen t = new OpenInfo();
            int nValue;
            t.Info = new Dictionary<string, string>();
            t.BaseDirectory = r.ReadString();
            t.AppId = r.ReadInt32();
            t.StreamVersion = r.ReadInt32();
            nValue = r.ReadInt32();
            for (int i = 0; i < nValue; i++)
            {
                t.Info.Add(r.ReadString(), r.ReadString());
            }

            return t;
        }
    }
}
