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
            if (query == "*") return GetAllLog(25);
            LuceneSearcher searcher;
            List<ILogViewModel> result = new List<ILogViewModel>();
            searcher = new LuceneSearcher(new string[] { Log.LogLevel, Log.Exception });
            TopDocs hits = searcher.Search(query);
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = searcher.GetDocument(scoreDoc);
                switch(doc.Get(Log.LogType))
                {
                    case "OpenGroup": result.Add(OpenGroupViewModel.Get(searcher, doc));
                        break;
                    case "Line": result.Add(LineViewModel.Get(searcher, doc));
                        break;
                    case "CloseGroup": result.Add(CloseGroupViewModel.Get(searcher, doc));
                        break;
                    default: throw new ArgumentException(nameof(doc));
                }
            }
            return result;
        }
        
        static public List<ILogViewModel> GetAllLog(int maxLogtoReturn)
        {
            List<ILogViewModel> result = new List<ILogViewModel>();
            LuceneSearcher searcher;
            searcher = new LuceneSearcher(new string[] { Log.LogLevel });
            TopDocs hits = searcher.GetAllLog(maxLogtoReturn);
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = searcher.GetDocument(scoreDoc);
                switch (doc.Get(Log.LogType))
                {
                    case "OpenGroup":
                        result.Add(OpenGroupViewModel.Get(searcher, doc));
                        break;
                    case "Line":
                        result.Add(LineViewModel.Get(searcher, doc));
                        break;
                    case "CloseGroup":
                        result.Add(CloseGroupViewModel.Get(searcher, doc));
                        break;
                    default: throw new ArgumentException(nameof(doc));
                }
            }
            return result;
        }
        static public List<ILogViewModel> GetLogWithFilters(string monitorId, string appId, DateTime dateStart, DateTime dateEnd, string[] fields, string[] logLevel, string keyword)
        {
            List<ILogViewModel> result = new List<ILogViewModel>();
            LuceneSearcher searcher;
            searcher = new LuceneSearcher(new string[] { Log.LogLevel });
            TopDocs hits = searcher.Search(searcher.CreateQuery(monitorId, appId, fields, logLevel, dateStart, dateEnd, keyword));
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = searcher.GetDocument(scoreDoc);
                switch (doc.Get(Log.LogType))
                {
                    case "OpenGroup":
                        result.Add(OpenGroupViewModel.Get(searcher, doc));
                        break;
                    case "Line":
                        result.Add(LineViewModel.Get(searcher, doc));
                        break;
                    case "CloseGroup":
                        result.Add(CloseGroupViewModel.Get(searcher, doc));
                        break;
                    default: throw new ArgumentException(nameof(doc));
                }
            }
            return result;
        }
    }
}
