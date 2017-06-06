using System;
using System.Collections.Generic;
using System.Text;
using CK.Core;

namespace CK.TcpHandler.Configuration.Protocol
{
    public class LogBlock : ILogBlock
    {
        public LogType Type { get; set; }
        public byte[] Log { get; set; }

        public void WriteLogBlock(CKBinaryWriter w)
        {
            w.Write('L');
            w.Write((int)Type);
            w.Write(Log.Length);
            w.Write(Log);
        }

        public static ILogBlock Read (CKBinaryReader r)
        {
            ILogBlock block = new LogBlock();
            if (r.ReadChar() != 'L')
                throw new NotSupportedException();
            block.Type = (LogType)r.ReadInt32();
            int length = r.ReadInt32();
            block.Log = r.ReadBytes(length);

            return block;
        }
    }
}
