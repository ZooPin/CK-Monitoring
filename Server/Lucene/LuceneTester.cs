using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.Documents;

namespace GloutonLucene
{
    public class LuceneTester
    {
        static string _indexDir = "C:\\Dev\\CK-Monitoring-Bis\\Glouton.Server\\Index";
        static string _dataDir = "C:\\Dev\\CK-Monitoring-Bis\\Glouton.Server\\Data";
        LuceneIndexer _indexer;
        LuceneSearcher _searcher;

        public static void Test ()
        {
            LuceneTester tester = new LuceneTester();
            tester.CreateIndex();
            tester.Search("abcd");
        }

        private void CreateIndex()
        {
            _indexer = new LuceneIndexer(_indexDir);
            int numIndexed;
            numIndexed = _indexer.CreateIndex(_dataDir);
            _indexer.close();
            Console.WriteLine("File indexed ");
        }

        private void Search (string searchQuery)
        {
            _searcher = new LuceneSearcher(_indexDir);
            TopDocs hits = _searcher.search(searchQuery);
            Console.WriteLine("Document found ");
            foreach (ScoreDoc scoreDoc in hits.ScoreDocs)
            {
                Document doc = _searcher.getDocument(scoreDoc);
                Console.WriteLine("File : " + doc.Get(LuceneConstant.FilePath));
            }
        }
    }
}
