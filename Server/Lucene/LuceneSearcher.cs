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
        MultiFieldQueryParser _queryParser;
        QueryParser _exceptionParser;
        Query _query;

        public LuceneSearcher(string[] fields)
        {
            Lucene.Net.Store.Directory indexDirectory = FSDirectory.Open(new DirectoryInfo(LuceneConstant.GetPath()));
            _indexSearcher = new IndexSearcher(DirectoryReader.Open(indexDirectory));
            _queryParser =  new MultiFieldQueryParser(LuceneVersion.LUCENE_48,
                fields,
                new StandardAnalyzer(LuceneVersion.LUCENE_48));
            _exceptionParser = new QueryParser(LuceneVersion.LUCENE_48,
                "Message",
                new StandardAnalyzer(LuceneVersion.LUCENE_48));
        }

        public TopDocs Search (string searchQuery)
        {
            _query = _queryParser.Parse(searchQuery);
            return _indexSearcher.Search(_query, LuceneConstant.MaxSearch);
        }

        public TopDocs Search(Query searchQuery)
        {
            return _indexSearcher.Search(searchQuery, LuceneConstant.MaxSearch);
        }

        public Document GetDocument(ScoreDoc scoreDoc)
        {
            return _indexSearcher.Doc(scoreDoc.Doc);
        }

        public TopDocs GetAllLog(int numberDocsToReturn)
        {
            return _indexSearcher.Search(new WildcardQuery(new Term("LogLevel", "*")), numberDocsToReturn);
            return null;
        }

        public TopDocs GetAllExceptions(int numberDocsToReturn)
        {
            return _indexSearcher.Search(_exceptionParser.Parse("Outer"), numberDocsToReturn);
            return null;
        }
    }
}
