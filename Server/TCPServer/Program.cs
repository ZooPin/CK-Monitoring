using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() => { TCPHelper helper = new TCPHelper();
                helper.StartServer(3630).Wait();
                });
            Console.ReadLine();
        }
    }
}