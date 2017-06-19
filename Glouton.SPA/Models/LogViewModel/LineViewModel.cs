using Glouton.SPA.Models.ExceptionViewModel;
using Glouton.SPA.Models.LogViewModel;
using GloutonLucene;
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
        public IExceptionViewModel Exception { get; set; }
        public string LogLevel { get; set; }
        public string MonitorId { get; set; }
        public string GroupDepth { get; set; }
        public string PreviousEntryType { get; set; }
        public string PreviousLogTime { get; set; }

        public static LineViewModel Get(LuceneSearcher searcher, Document doc)
        {
            LineViewModel obj = new LineViewModel()
            {
                MonitorId = doc.Get(Log.MonitorId),
                GroupDepth = doc.Get(Log.GroupDepth),
                PreviousEntryType = doc.Get(Log.PreviousEntryType),
                PreviousLogTime = doc.Get(Log.PreviousLogTime),
                LogLevel = doc.Get(Log.LogLevel),
                Text = doc.Get(Log.Text),
                Tags = doc.Get(Log.Tags),
                SourceFileName = doc.Get(Log.SourceFileName),
                LogTime = doc.Get(Log.LogTime),
                Exception = ExceptionViewModel.ExceptionViewModel.Get(searcher, doc)
            };
            return obj;
        }
    }
}
