﻿using CK.Core;
using CK.Monitoring;
using CK.TcpHandler.Configuration.Protocol;
using CK.TcpHandler.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class TCPHelper
    {
        byte[] _buffer = new byte[4096];
        public IOpen OpenInfo { get; set; }

        public async Task StartServer(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server: Launch on port {port}");
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Server: client accepted");
                await Task.Factory.StartNew(StartServerCommunication(client));
            }
        }

        Func<Task> StartServerCommunication(TcpClient client) => async () => await DoServerCommunication(client);

        async Task DoServerCommunication(TcpClient client)
        {
            List<byte[]> t = new List<byte[]>();
            using (client)
            using (NetworkStream networkStream = client.GetStream())
            {
                OpenBlockReader(await ReadBlock(networkStream));
                while(true)
                {
                    IMulticastLogEntry log = LoggerConverter(await ReadBlock(networkStream));
                }
            }
        }

        IMulticastLogEntry LoggerConverter(byte[] data)
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

        void OpenBlockReader (byte[] block)
        {
            OpenInfo = BlockReader.Open(block);
        }

        void LogBlockReader (byte[] block)
        {
            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryReader r = new CKBinaryReader(mem))
            {
                ILogBlock logBlock = LogBlock.Read(r);
                if (logBlock.Type == LogType.CKMonitoring)
                    LoggerConverter(logBlock.Log);
            }
        }

        async Task<byte[]> ReadBlock(Stream s)
        {
            int length;
            await FillBuffer(s, 4);
            length = (_buffer[0] << 24 | _buffer[1] << 16 | _buffer[2] << 8 | _buffer[3]);
            if (length == 0) return new byte[0];
            await FillBuffer(s, length);
            byte[] data = new byte[length];
            Array.Copy(_buffer, data, length);

            return data;
        }

        async Task FillBuffer(Stream stream, int size)
        {
            if (_buffer.Length < size) _buffer = new byte[size];
            int totalReceived = 0;
            int readByte = 0;
            while(totalReceived < size && (readByte = await stream.ReadAsync(_buffer, totalReceived, size - totalReceived)) > 0)
            {
                totalReceived += readByte;
            }
        }
    }
}
