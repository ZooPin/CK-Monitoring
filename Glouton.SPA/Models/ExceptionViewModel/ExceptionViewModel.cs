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
    public class ExceptionViewModel : IExceptionViewModel
    {
        public IInnerExceptionViewModel InnerException { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }

        public static IExceptionViewModel Get(LuceneSearcher searcher, Document doc)
        {
            if (doc.GetField(Log.Exception) == null) return null;
            TermQuery query = new TermQuery(new Term("IndexTS", doc.Get(Log.Exception)));
            TopDocs exceptionDoc = searcher.Search(query);
            Document exception = searcher.GetDocument(exceptionDoc.ScoreDocs[0]);

            return new ExceptionViewModel()
            {
                Message = exception.Get(Log.Message),
                Stack = exception.Get(Log.Stack),
                InnerException = InnerExceptionViewModel.Get(searcher, exception)
            };
        }
    }
}
