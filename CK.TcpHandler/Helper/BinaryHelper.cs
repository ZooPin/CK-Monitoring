using System;
using System.Collections.Generic;
using CK.Monitoring;
using System.Text;
using System.Diagnostics;
using CK.Core;
using CK.Text;
using System.IO;

namespace CK.TcpHandler.Helper
{
    public static class BinaryHelper
    {
        public static byte[] AppendEntry (IMulticastLogEntry e)
        {
            StringBuilder builder = new StringBuilder();
            int nameLen = 0;
            Guid currentMonitorId = Guid.Empty;
            Dictionary<Guid, string> monitorNames = new Dictionary<Guid, string>();
            string currentMonitorName;

            Debug.Assert(DateTimeStamp.MaxValue.ToString().Length == 32,
                "DateTimeStamp FileNameUniqueTimeUtcFormat and the uniquifier: max => 32 characters long.");
            Debug.Assert(Guid.NewGuid().ToString().Length == 36,
                "Guid => 18 characters long.");

            builder.Append(' ', nameLen + 32);
            builder.Append("| ", e.Text != null ? e.GroupDepth : e.GroupDepth - 1);
            string prefix = builder.ToString();
            builder.Clear();
            // MonitorId (if needed) on one line.
            if (currentMonitorId == e.MonitorId)
            {
                builder.Append(' ', nameLen + 1);
            }
            else
            {
                currentMonitorId = e.MonitorId;
                if (!monitorNames.TryGetValue(currentMonitorId, out currentMonitorName))
                {
                    currentMonitorName = monitorNames.Count.ToString("X" + nameLen);
                    int len = currentMonitorName.Length;
                    if (nameLen < len)
                    {
                        prefix = " " + prefix;
                        nameLen = len;
                    }
                    monitorNames.Add(currentMonitorId, currentMonitorName);
                    builder.Append(currentMonitorName)
                            .Append("~~~~")
                            .Append(' ', 28)
                            .Append("~~ Monitor: ")
                            .AppendLine(currentMonitorId.ToString());
                    builder.Append(' ', nameLen + 1);
                }
                else
                {
                    builder.Append(currentMonitorName).Append('~');
                    builder.Append(' ', nameLen - currentMonitorName.Length);
                }
            }
            builder.Append(' ', 17);
            builder.Append('+');
            builder.Append(e.LogTime.TimeUtc.ToString(@"ss\.fffffff"));
            builder.Append(' ');

            // Level is one char.
            char level;
            switch (e.LogLevel & LogLevel.Mask)
            {
                case LogLevel.Debug: level = 'd'; break;
                case LogLevel.Trace: level = ' '; break;
                case LogLevel.Info: level = 'i'; break;
                case LogLevel.Warn: level = 'W'; break;
                case LogLevel.Error: level = 'E'; break;
                default: level = 'F'; break;
            }
            builder.Append(level);
            builder.Append(' ');
            builder.Append("| ", e.Text != null ? e.GroupDepth : e.GroupDepth - 1);

            if (e.Text != null)
            {
                if (e.LogType == LogEntryType.OpenGroup) builder.Append("> ");
                prefix += "  ";
                builder.AppendMultiLine(prefix, e.Text, false).AppendLine();
                if (e.Exception != null)
                {
                    e.Exception.ToStringBuilder(builder, prefix);
                }
            }
            else
            {
                Debug.Assert(e.Conclusions != null);
                builder.Append("< ");
                if (e.Conclusions.Count > 0)
                {
                    builder.Append(" | ").Append(e.Conclusions.Count).Append(" conclusion");
                    if (e.Conclusions.Count > 1) builder.Append('s');
                    builder.Append(':').AppendLine();
                    prefix += "   | ";
                    foreach (var c in e.Conclusions)
                    {
                        builder.AppendMultiLine(prefix, c.Text, true).AppendLine();
                    }
                }
                else
                {
                    builder.AppendLine();
                }
            }
            return Encoding.ASCII.GetBytes(builder.ToString());
        }

        public static byte[] IMulticastLogEntryToBinary(IMulticastLogEntry log)
        {
            using (var mem = new MemoryStream())
            using (var w = new CKBinaryWriter(mem))
            {
                log.WriteLogEntry(w);
                w.Flush();

                return mem.ToArray();
            }
        }
    }
}
