using CK.Core;
using CK.Monitoring;
using CK.TcpHandler.Configuration.Protocol;
using CK.TcpHandler.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GloutonLucene;

namespace Glouton.TCPServer
{
    public static class BlockReceiver
    {
        public static async Task<bool> LogBlockReader(TCPHelper h, byte[] block, LuceneIndexer indexer)
        {
            if (block.Length == 0) return false;
            ILogBlock logBlock = await BlockReader.Log(block);
            if (logBlock.Type == LogType.CKMonitoring)
                LoggerConverter(logBlock.Log);

            switch (logBlock.Type)
            {
                case LogType.CKMonitoring:
                    indexer.IndexLog(LoggerConverter(logBlock.Log), h.OpenInfo.AppId);
                    Console.WriteLine("Server: [Received][Log.CKMonitoring]");
                    break;
                case LogType.Critical:
                    ReadCritical(logBlock); // string with critical log
                    Console.WriteLine($"Server: [Received][Log.Critical]");
                    break;
                default: throw new NotImplementedException();
            }
            return true;
        }

        public static async Task OpenBlockReader(TCPHelper h, byte[] block)
        {
            h.OpenInfo =  await BlockReader.Open(block);
        }

        public static IMulticastLogEntry LoggerConverter(byte[] data)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                mem.Write(data, 0, data.Length);
                mem.Seek(0, SeekOrigin.Begin);
                using (var reader = new LogReader(mem, LogReader.CurrentStreamVersion, 4))
                {
                    reader.MoveNext();
                    return reader.CurrentMulticast;
                }
            }
        }

        public static string ReadCritical (ILogBlock log)
        {
            return Encoding.Unicode.GetString(log.Log);
        }
    }
}
