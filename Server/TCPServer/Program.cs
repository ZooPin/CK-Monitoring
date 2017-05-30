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
            Task.Factory.StartNew(() => TCPHelper.StartServer(3630));
            Console.ReadLine();
        }
    }
}