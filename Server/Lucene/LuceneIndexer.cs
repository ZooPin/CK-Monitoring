using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Standard;
using CK.Monitoring;

namespace GloutonLucene
{
    public class LuceneIndexer
    {
        private IndexWriter _writer;

        public LuceneIndexer (string indexDirectoryPath)
        {
            Lucene.Net.Store.Directory indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectoryPath));

            _writer = new IndexWriter(indexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48,
                new StandardAnalyzer(LuceneVersion.LUCENE_48)));
        }

        public void close()
        {
            _writer.Dispose();
        }

        private Document GetLogDocument(ILogEntry log)
        {
            Document document = new Document();

            if (log.LogType == LogEntryType.Line || log.LogType == LogEntryType.OpenGroup)
            {
                Field logLevel = new TextField("LogLevel", log.LogLevel.ToString(), Field.Store.YES);
                Field text = new TextField("Text", log.Text, Field.Store.YES);
                Field tags = new TextField("Tags", log.Tags.ToString(), Field.Store.YES);
                Field logTime = new TextField("LogTime", log.LogTime.TimeUtc.ToString(), Field.Store.YES);
                Field fileName = new TextField("FileName", log.FileName, Field.Store.YES);
                Field lineNumber = new TextField("LineNumber", log.LineNumber.ToString(), Field.Store.YES);
                if (log.Exception != null)
                {
                    Field exception = new TextField("Exception", log.Exception.Message, Field.Store.YES);
                    document.Add(exception);
                }
                document.Add(logLevel);
                document.Add(text);
                document.Add(tags);
                document.Add(logTime);
                document.Add(fileName);
                document.Add(lineNumber);
            }

            else if (log.LogType == LogEntryType.CloseGroup)
            {
                Field logLevel = new TextField("LogLevel", log.LogLevel.ToString(), Field.Store.YES);
                Field conclusions = new TextField("Conclusions", log.Conclusions.ToString(), Field.Store.YES);
                Field logTime = new TextField("LogTime", log.LogTime.TimeUtc.ToString(), Field.Store.YES);

                document.Add(logLevel);
                document.Add(logTime);
            }

            Field logType = new TextField("LogType", log.LogType.ToString(), Field.Store.YES);

            document.Add(logType);

            return document;
        }

        public void IndexLog(ILogEntry log)
        {
            Document document = GetLogDocument(log);
            _writer.AddDocument(document);
        }
    }
}
