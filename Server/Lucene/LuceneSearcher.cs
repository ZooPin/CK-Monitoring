using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Lucene.Net.Store;

namespace GloutonLucene
{
    public class LuceneSearcher
    {
        IndexSearcher _indexSearcher;
        QueryParser _queryParser;
        Query _query;

        public LuceneSearcher(string indexDirPath)
        {
            Lucene.Net.Store.Directory indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirPath));
            _indexSearcher = new IndexSearcher(DirectoryReader.Open(indexDirectory));
            _queryParser = new QueryParser(LuceneVersion.LUCENE_48,
                LuceneConstant.Content,
                new StandardAnalyzer(LuceneVersion.LUCENE_48));
        }

        public TopDocs search (string searchQuery)
        {
            _query = _queryParser.Parse(searchQuery);
            return _indexSearcher.Search(_query, LuceneConstant.MaxSearch);
        }

        public Document getDocument(ScoreDoc scoreDoc)
        {
            return _indexSearcher.Doc(scoreDoc.Doc);
        }
    }
}
