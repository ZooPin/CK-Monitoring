using CK.Core;
using CK.TcpHandler.Configuration.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CK.TcpHandler.Helper
{
    public static class BlockReader
    {
        public static async Task<IOpen> Open (byte[] open)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                await mem.WriteAsync(open, 0, open.Length);
                mem.Seek(0, SeekOrigin.Begin);
                using (CKBinaryReader r = new CKBinaryReader(mem))
                {
                    IOpen block = OpenInfo.Read(r);
                    return block;
                }
            }
        }

        public static async Task<ILogBlock> Log (byte[] log)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                await mem.WriteAsync(log, 0, log.Length);
                mem.Seek(0, SeekOrigin.Begin);
                using (CKBinaryReader r = new CKBinaryReader(mem))
                {
                    ILogBlock block = LogBlock.Read(r);
                    return block;
                }
            }
        }
    }
}
