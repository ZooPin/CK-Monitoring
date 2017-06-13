using Glouton.SPA.Models.LogViewModel;
using GloutonLucene;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Services
{
    public class LuceneSearcherService
    {
        static public List<ILogViewModel> Search(string query)
        {
            string path = "C:\\Indexer";
            LuceneSearcher searcher;

            List<ILogViewModel> result = new List<ILogViewModel>();
            searcher = new LuceneSearcher(path, new string[] { Log.LogLevel, Log.Exception });
            TopDocs hits = searcher.Search(query);
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = searcher.GetDocument(scoreDoc);
                switch(doc.Get(Log.LogType))
                {
                    case "OpenGroup": result.Add(OpenGroupViewModel.Get(doc));
                        break;
                    case "Line": result.Add(LineViewModel.Get(doc));
                        break;
                    case "CloseGroup": result.Add(CloseGroupViewModel.Get(doc));
                        break;
                    default: throw new ArgumentException(nameof(doc));
                }
            }
            return result;
        }
    }
}
