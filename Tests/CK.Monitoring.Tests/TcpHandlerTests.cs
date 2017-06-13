using CK.Core;
using CK.TcpHandler.Configuration.Protocol;
using FluentAssertions;
using NUnit.Framework;
using Glouton.TCPServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CK.Monitoring.Tests
{
    [TestFixture]
    public class TcpHandlerTests
    {

        [Test]
        public void dumping_text_file()
        {
            Random r = new Random();
            IPAddress address = IPAddress.Parse("127.0.0.1");
            GrandOutputConfiguration config = new GrandOutputConfiguration()
                                                    .AddHandler(new TcpHandler.TcpSenderConfiguration() { Address = address, Port = 3630, HandleCriticalErrors = true });
            using (GrandOutput g = new GrandOutput(config))
            {
                TestHelper.DumpSampleLogs1(r, g);
                TestHelper.DumpSampleLogs2(r, g);
            }
        }

        [Test]
        public void test_critical_errors()
        {
            StartServer();
            Random r = new Random();
            IPAddress address = IPAddress.Parse("127.0.0.1");
            GrandOutputConfiguration config = new GrandOutputConfiguration()
                                                    .AddHandler(new TcpHandler.TcpSenderConfiguration() { Address = address, Port = 3630, HandleCriticalErrors = true });
            using (GrandOutput g = new GrandOutput(config))
            {
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError1");
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError2");
                Thread.Sleep(200);
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError3");
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError4");
                Thread.Sleep(200);
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError5");
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError6");
                Thread.Sleep(200);
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError7");
                ActivityMonitor.CriticalErrorCollector.Add(TestHelper.ThrowExceptionWithInner(false), "CriticalError8");
                Thread.Sleep(200);
            }
        }

        [Test]
        public void protocol_block_open_serialized_write_read()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "on", "one" },
                { "ref", "two" },
                { "assemblies", "three" }
            };
            IOpen block = new OpenInfo() { AppId = 1, BaseDirectory = AppContext.BaseDirectory, StreamVersion = LogReader.CurrentStreamVersion, Info = dic };

            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryWriter writer = new CKBinaryWriter(mem))
            {
                block.WriteOpenBlock(writer);
                mem.Seek(0, SeekOrigin.Begin);
                using (CKBinaryReader reader = new CKBinaryReader(mem))
                {
                    IOpen blockReceived = OpenInfo.Read(reader);
                    blockReceived.AppId.Should().Be(block.AppId);
                    blockReceived.BaseDirectory.Should().Contain(block.BaseDirectory);
                    blockReceived.Info.ShouldBeEquivalentTo(block.Info);
                    blockReceived.StreamVersion.Should().Be(block.StreamVersion);
                }
            }
        }

        [Test]
        public void protocol_block_log_serialized_write_read()
        {
            byte[] arrByte = new byte[10] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
            ILogBlock block = new LogBlock() { Type = LogType.CKMonitoring, Log = arrByte };

            using (MemoryStream mem = new MemoryStream())
            using (CKBinaryWriter w = new CKBinaryWriter(mem))
            {
                block.WriteLogBlock(w);
                mem.Seek(0, SeekOrigin.Begin);
                using (CKBinaryReader r = new CKBinaryReader(mem))
                {
                    ILogBlock blockReceived = LogBlock.Read(r);
                    blockReceived.Type.Should().Be(block.Type);
                    blockReceived.Log.ShouldAllBeEquivalentTo(block.Log);
                }
            }
        }

        static void StartServer ()
        {
            Task.Factory.StartNew(() => {
                TCPHelper helper = new TCPHelper();
                helper.StartServer(3630).Wait();
            });
        }
    }
}
