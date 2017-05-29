using CK.Core;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CK.Monitoring.Tests
{
    [TestFixture]
    public class TcpHandlerTests
    {

        [Test]
        public void dumping_text_file()
        {
            Random r = new Random();
            IPAddress adress = IPAddress.Parse("127.0.0.1");
            GrandOutputConfiguration config = new GrandOutputConfiguration()
                                                    .AddHandler(new TcpHandler.TcpSenderConfiguation { Adress = adress, Port = 3630 });
            using (GrandOutput g = new GrandOutput(config))
            {
                DumpSampleLogs1(r, g);
                DumpSampleLogs2(r, g);
            }
        }

        static void DumpSampleLogs1(Random r, GrandOutput g)
        {
            var m = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(m);
            m.SetTopic("First Activity...");
            if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
            using (m.OpenTrace().Send("Opening trace"))
            {
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Trace().Send("A trace in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Info().Send("An info in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Warn().Send("A warning in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Error().Send("An error in group.");
                if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
                m.Fatal().Send("A fatal in group.");
            }
            if (r.Next(3) == 0) System.Threading.Thread.Sleep(100 + r.Next(2500));
            m.Trace().Send("End of first activity.");
        }

        static void DumpSampleLogs2(Random r, GrandOutput g)
        {
            var m = new ActivityMonitor(false);
            g.EnsureGrandOutputClient(m);

            m.Fatal().Send(ThrowExceptionWithInner(false), "An error occured");
            m.SetTopic("This is a topic...");
            m.Trace().Send("a trace");
            m.Trace().Send("another one");
            m.SetTopic("Please, show this topic!");
            m.Trace().Send("Anotther trace.");
            using (m.OpenTrace().Send("A group trace."))
            {
                m.Trace().Send("A trace in group.");
                m.Info().Send("An info...");
                using (m.OpenInfo().Send(@"A group information... with a 
multi
-line
message. 
This MUST be correctly indented!"))
                {
                    m.Info().Send("Info in info group.");
                    m.Info().Send("Another info in info group.");
                    m.Error().Send(ThrowExceptionWithInner(true), "An error.");
                    m.Warn().Send("A warning.");
                    m.Trace().Send("Something must be said.");
                    m.CloseGroup("Everything is in place.");
                }
            }
            m.SetTopic(null);
            using (m.OpenTrace().Send("A group with multiple conclusions."))
            {
                using (m.OpenTrace().Send("A group with no conclusion."))
                {
                    m.Trace().Send("Something must be said.");
                }
                m.CloseGroup(new[] {
                        new ActivityLogGroupConclusion( "My very first conclusion." ),
                        new ActivityLogGroupConclusion( "My second conclusion." ),
                        new ActivityLogGroupConclusion( @"My very last conclusion
is a multi line one.
and this is fine!" )
                    });
            }
            m.Trace().Send("This is the final trace.");
        }

        private static string ThrowExceptionWithInner(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
