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

        public static async Task StartServer(int port)
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

        static Func<Task> StartServerCommunication(TcpClient client) => async () => await DoServerCommunication(client);

        static async Task DoServerCommunication(TcpClient client)
        {
            using (client)
            using (StreamReader sr = new StreamReader(client.GetStream()))
            {
                while (true)
                {
                    string s = await sr.ReadLineAsync();
                    if (s == "@_close_@")
                    {
                        Console.WriteLine("Server: communication completed");
                        break;
                    }
                    Console.WriteLine("Server: message received - ${0}$", s);
                }
            }
        }
    }
}
