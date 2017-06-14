using Glouton.SPA.Models.ExceptionViewModel;
using GloutonLucene;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.LogViewModel
{
    public class OpenGroupViewModel : ILogViewModel
    {
        public LogType LogType => LogType.Opengroup;
        public string LogLevel { get; set; }
        public string LogTime { get; set; }
        public string Text { get; set; }
        public string SourceFileName { get; set; }
        public IExceptionViewModel Exception { get; set; }

        public static OpenGroupViewModel Get (LuceneSearcher searcher, Document doc)
        {
            OpenGroupViewModel obj = new OpenGroupViewModel()
            {
                LogLevel = doc.Get(Log.LogLevel),
                LogTime = doc.Get(Log.LogTime),
                Text = doc.Get(Log.Text),
                SourceFileName = doc.Get(Log.SourceFileName),
                Exception = ExceptionViewModel.ExceptionViewModel.Get(searcher, doc)
            };

            return obj;
        }
    }
}
