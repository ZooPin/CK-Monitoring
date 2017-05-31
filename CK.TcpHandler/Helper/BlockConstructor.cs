using CK.Core;
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
            {
                OpenInfo info = new OpenInfo() { AppId = appId, BaseDirectory = AppContext.BaseDirectory, StreamVersion = 7, Info = new Dictionary<string, string>() };
                CKBinaryWriter writer = new CKBinaryWriter(mem);
            }
            throw new NotImplementedException();
        }

    }
}
