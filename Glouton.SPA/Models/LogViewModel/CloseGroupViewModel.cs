using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.LogViewModel
{
    public class CloseGroupViewModel : ILogViewModel
    {
        public string LogLevel { get; set; }
        public string Conclusion { get; set; }
        public LogType LogType { get => LogType.CloseGroup; }
        public string Exception { get; set; }
        public string LogTime { get; set; }

        public static CloseGroupViewModel Get (Document doc)
        {
            CloseGroupViewModel obj = new CloseGroupViewModel
            {
                LogLevel = doc.Get(Log.LogLevel),
                LogTime = doc.Get(Log.LogTime),
                Conclusion = doc.Get(Log.Conclusion),
            };

            return obj;
        }
    }
}
