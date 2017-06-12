using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.LogViewModel
{
    public class LineViewModel : ILogViewModel
    {
        public LogType LogType => LogType.Line;
        public string LogTime { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public string SourceFileName { get; set; }
        public string Exception { get; set; }
        public string LogLevel { get; set; }

        public static LineViewModel Get (Document doc)
        {
            LineViewModel obj = new LineViewModel()
            {
                LogLevel = doc.Get(Log.LogLevel),
                Text = doc.Get(Log.Text),
                Tags = doc.Get(Log.Tags),
                SourceFileName = doc.Get(Log.SourceFileName),
                LogTime = doc.Get(Log.LogTime)
            };

            if (doc.GetField(Log.Exception) != null) obj.Exception = doc.Get(Log.Exception);
            return obj;
        }
    }
}
