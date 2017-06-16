using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using GloutonLucene;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace LuceneSearchConsole
{
    class Searcher
    {
        static string _indexDir = "C:\\Indexer";
        LuceneSearcher _searcher;

        internal bool Search(string searchQuery)
        {
            _searcher = new LuceneSearcher(_indexDir, new string[] {"LogLevel", "IndexTS" });
            TopDocs hits = _searcher.Search(searchQuery);
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = _searcher.GetDocument(scoreDoc);

                Console.WriteLine("Monitor Id : " + doc.Get("MonitorId"));

                if (doc.Get("LogType") == "Line")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nText : " + doc.Get("Text")
                        + "\nTags : " + doc.Get("Tags")
                        + "\nSource file name : " + doc.Get("FileName")
                        + "\nLine number : " + doc.Get("LineNumber"));
                    GetExceptions(doc);
                }
                else if (doc.Get("LogType") == "OpenGroup")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nText : " + doc.Get("Text")
                        + "\nTags : " + doc.Get("Tags")
                        + "\nSource file name : " + doc.Get("FileName")
                        + "\nLine number : " + doc.Get("LineNumber"));
                    GetExceptions(doc);
                }
                else if (doc.Get("LogType") == "CloseGroup")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nConclusion : " + doc.Get("Conclusions"));
                }
                Console.WriteLine("\nTimeStampID" + doc.Get("IndexTS"));
                Console.WriteLine("\n");
            }
            return hits.TotalHits == 0 ? false : true;
        }

        private void GetExceptions(Document doc)
        {
            if (doc.GetField("Exception") != null)
            {
                TermQuery query = new TermQuery(new Term("IndexTS", doc.Get("Exception")));
                TopDocs exception = _searcher.Search(query);
                Document exceptionDoc = _searcher.GetDocument(exception.ScoreDocs[0]);

                if (exceptionDoc.Get("InnerException") != null) GetExceptions(exceptionDoc);

                Console.WriteLine("-----------------\nExceptions "
                + "\n message : "
                + exceptionDoc.Get("Message") 
                + "\n stack : "
                + exceptionDoc.Get("Stack")
                + "\n-----------------");
            }
            else if (doc.GetField("InnerException") != null)
            {
                TermQuery query = new TermQuery(new Term("IndexTS", doc.Get("InnerException")));
                TopDocs exception = _searcher.Search(query);
                Document exceptionDoc = _searcher.GetDocument(exception.ScoreDocs[0]);

                Console.WriteLine("       -----------------\nInner exceptions "
                + "\n       stack : "
                + exceptionDoc.Get("Stack"));
                if (exceptionDoc.Get("Details") != null) Console.WriteLine("\n       Details : " + exceptionDoc.Get("Details"));
                if (exceptionDoc.Get("Filename") != null) Console.WriteLine("\n       Filename : " + exceptionDoc.Get("Filename"));
                Console.WriteLine("\n       -----------------");
            }
        }
    }
}
