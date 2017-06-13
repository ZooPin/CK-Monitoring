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
            _searcher = new LuceneSearcher(_indexDir, "LogLevel");
            TopDocs hits = _searcher.search(searchQuery);
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = _searcher.getDocument(scoreDoc);
                if(doc.GetValues("LogType")[0] == "Line")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nText : " + doc.Get("Text")
                        + "\nTags : " + doc.Get("Tags")
                        + "\nSource file name : " + doc.Get("FileName")
                        + "\nLine number : " + doc.Get("LineNumber"));
                    GetExceptions(doc);
                }
                else if (doc.GetValues("LogType")[0] == "OpenGroup")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nText : " + doc.Get("Text")
                        + "\nTags : " + doc.Get("Tags")
                        + "\nSource file name : " + doc.Get("FileName")
                        + "\nLine number : " + doc.Get("LineNumber"));
                    GetExceptions(doc);
                }
                else if (doc.GetValues("LogType")[0] == "CloseGroup")
                {
                    Console.WriteLine("Log time : " + doc.Get("LogTime")
                        + "\nLog level : " + doc.Get("LogLevel")
                        + "\nConclusion : " + doc.Get("Conclusions"));
                }
                Console.WriteLine("\nTimeStampID" + doc.Get("TimestampID"));
                Console.WriteLine("\n");
            }
            return hits.TotalHits == 0 ? false : true;
        }

        private void GetExceptions(Document doc)
        {
            if (doc.GetField("Exception") != null) Console.WriteLine("Exceptions : " + doc.Get("Exception"));
        }
    }
}
