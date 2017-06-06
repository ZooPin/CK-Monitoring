using CK.Core;
using CK.TcpHandler.Configuration.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CK.TcpHandler.Helper
{
    public static class BlockReader
    {
        public static IOpen Open (byte[] open)
        {
            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryReader r = new CKBinaryReader(mem))
            {
                IOpen block = OpenInfo.Read(r);
                return block;
            }
        }

        public static ILogBlock Log (byte[] log)
        {
            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryReader r = new CKBinaryReader(mem))
            {
                ILogBlock block = LogBlock.Read(r);
                return block;
            }
        }
    }
}
