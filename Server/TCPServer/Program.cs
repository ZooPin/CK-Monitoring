using System;
using System.Threading.Tasks;

namespace Glouton.TCPServer
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