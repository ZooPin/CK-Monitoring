using Glouton.SPA.Models.LogViewModel;
using GloutonLucene;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.ExceptionViewModel
{
    public class InnerExceptionViewModel : IInnerExceptionViewModel
    {
        public string Stack { get; set; }
        public string Details { get; set; }
        public string FileName { get; set; }

        static public IInnerExceptionViewModel Get (LuceneSearcher searcher, Document doc)
        {
            if (doc.GetField(Log.InnerException) == null) return null;

            TermQuery query = new TermQuery(new Term("IndexTS", doc.Get(Log.InnerException)));
            TopDocs exceptionDoc = searcher.Search(query);
            Document exception = searcher.GetDocument(exceptionDoc.ScoreDocs[0]);

            return new InnerExceptionViewModel()
            {
                Stack = exception.Get(Log.Stack),
                Details = exception.Get(Log.Detail),
                FileName = exception.Get(Log.FileName)
            };
        }
    }
}
