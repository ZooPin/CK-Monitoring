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
using CK.Core;

namespace GloutonLucene
{
    public class LuceneIndexer : IDisposable
    {
        private IndexWriter _writer;
        private DateTimeStamp _lastDateTimeStamp;

        public LuceneIndexer (string indexDirectoryPath)
        {
            Lucene.Net.Store.Directory indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectoryPath));

            _writer = new IndexWriter(indexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48,
                new StandardAnalyzer(LuceneVersion.LUCENE_48)));
            _lastDateTimeStamp = new DateTimeStamp(DateTime.UtcNow, 0);
        }

        private Document GetLogDocument(ILogEntry log)
        {
            Document document = new Document();

            if (log.LogType == LogEntryType.Line || log.LogType == LogEntryType.OpenGroup)
            {
                Field logLevel = new TextField("LogLevel", log.LogLevel.ToString(), Field.Store.YES);
                Field text = new TextField("Text", log.Text, Field.Store.YES);
                Field tags = new StringField("Tags", log.Tags.ToString(), Field.Store.YES);
                Field logTime = new StringField("LogTime", log.LogTime.TimeUtc.ToString(), Field.Store.YES);
                Field fileName = new TextField("FileName", log.FileName, Field.Store.YES);
                Field lineNumber = new TextField("LineNumber", log.LineNumber.ToString(), Field.Store.YES);
                if (log.Exception != null)
                {
                    Document exDoc = GetExceptionDocuments(log.Exception);
                    Console.WriteLine("exception " + exDoc.Get("indexTS").ToString());
                    Field exception = new TextField("Exception", exDoc.Get("indexTS").ToString(), Field.Store.YES);
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
                StringBuilder builder = new StringBuilder();
                foreach(ActivityLogGroupConclusion conclusion in log.Conclusions)
                {
                    builder.Append(conclusion.Text + "\n");
                }
                Field logLevel = new TextField("LogLevel", log.LogLevel.ToString(), Field.Store.YES);
                Field conclusions = new TextField("Conclusions", builder.ToString(), Field.Store.YES);
                Field logTime = new TextField("LogTime", log.LogTime.TimeUtc.ToString(), Field.Store.YES);

                document.Add(logLevel);
                document.Add(logTime);
                document.Add(conclusions);
            }

            Field logType = new TextField("LogType", log.LogType.ToString(), Field.Store.YES);
            Field indexTS = new StringField("indexTS", CreateIndexTS().ToString(), Field.Store.YES);

            document.Add(logType);
            document.Add(indexTS);

            return document;
        }

        Document GetExceptionDocuments(CKExceptionData exception)
        {
            Document document = new Document();

            Field message = new TextField("Message", exception.Message, Field.Store.YES);
            Field stack = new TextField("stack", exception.StackTrace, Field.Store.YES);
            Field indexTS = new StringField("indexTS", CreateIndexTS().ToString(), Field.Store.YES);
            
            if (exception.InnerException != null)
            {
                Document exDoc = GetExceptionDocuments(exception.InnerException);
                Console.WriteLine("inner exception " + exDoc.Get("indexTS").ToString());
                Field innerException = new TextField("innerException", exDoc.Get("indexTS").ToString(), Field.Store.YES);
                document.Add(innerException);
            }

            document.Add(message);
            document.Add(stack);
            document.Add(indexTS);

            _writer.AddDocument(document);

            return document;
        }

        private DateTimeStamp CreateIndexTS()
        {
            DateTimeStamp indexTS = new DateTimeStamp(_lastDateTimeStamp, DateTime.UtcNow);
            _lastDateTimeStamp = indexTS;
            return indexTS;
        }

        public void IndexLog(ILogEntry log)
        {
            Document document = GetLogDocument(log);
            _writer.AddDocument(document);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
