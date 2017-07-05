using CK.Monitoring;
using CK.TcpHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glouton.CrashApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            GrandOutputConfiguration config = new GrandOutputConfiguration()
                                                    .AddHandler(new TcpSenderConfiguration() { Address = address, Port = 3630, HandleCriticalErrors = true });
            using (GrandOutput g = new GrandOutput(config))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(g));
            }
        }
    }
}
