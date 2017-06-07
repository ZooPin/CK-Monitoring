using CK.Core;
using CK.Monitoring;
using CK.TcpHandler.Configuration.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CK.TcpHandler.Helper
{
    public static class BlockConstructor
    {
        public static byte[] Open (int appId)
        {
            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryWriter writer = new CKBinaryWriter(mem))
            {
                OpenInfo info = new OpenInfo() { AppId = appId, BaseDirectory = AppContext.BaseDirectory, StreamVersion = LogReader.CurrentStreamVersion, Info = new Dictionary<string, string>() };
                info.WriteOpenBlock(writer);
                return mem.ToArray();
            }
        }

        public static byte[] Log(LogType type, byte[] logArray)
        {
            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryWriter writer = new CKBinaryWriter(mem))
            {
                LogBlock log = new LogBlock() { Log = logArray, Type = type };
                log.WriteLogBlock(writer);
                return mem.ToArray();
            }
        }

        public static byte[] Log(LogType type, string t)
        {
            return Log(type, Encoding.Unicode.GetBytes(t));
        }
    }
}
