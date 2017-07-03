using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Standard;
using CK.Monitoring;
using CK.Core;
using CK.TcpHandler.Configuration.Protocol;

namespace GloutonLucene
{
    public class LuceneIndexer : IDisposable
    {
        private IndexWriter _writer;
        private DateTimeStamp _lastDateTimeStamp;
        private DateTime _lastCommit;
        private int _numberOfFileToCommit;
        private int _exceptionDepth;
        private LuceneSearcher _searcher;
        private ISet<string> _monitorIdList;
        private ISet<string> _appIdList;

        public LuceneIndexer (string indexDirectoryPath)
        {
            Lucene.Net.Store.Directory indexDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectoryPath));

            _writer = new IndexWriter(indexDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48,
                new StandardAnalyzer(LuceneVersion.LUCENE_48)));
            _lastDateTimeStamp = new DateTimeStamp(DateTime.UtcNow, 0);
            _numberOfFileToCommit = 0;
            _exceptionDepth = 0;
            InitializeSearcher();
            InitializeIdList();
        }

        public void InitializeSearcher()
        {
            try
            {
                _searcher = new LuceneSearcher(new string[] { "MonitorIdList", "AppIdList" });
            }
            catch (Exception e)
            {

            }
        }

        private Document GetLogDocument(IMulticastLogEntry log, int appId)
        {
            Document document = new Document();
            
            Field monitorId = new StringField("MonitorId", log.MonitorId.ToString(), Field.Store.YES);
            Field groupDepth = new StringField("GroupDepth", log.GroupDepth.ToString(), Field.Store.YES);
            Field previousEntryType = new StringField("PreviousEntryType", log.PreviousEntryType.ToString(), Field.Store.YES);
            Field previousLogTime = new StringField("PreviousLogTime", log.PreviousLogTime.ToString(), Field.Store.YES);
            
            document.Add(monitorId);
            document.Add(groupDepth);
            document.Add(previousEntryType);
            document.Add(previousLogTime);

            if (log.LogType == LogEntryType.Line || log.LogType == LogEntryType.OpenGroup)
            {
                Field logLevel = new TextField("LogLevel", log.LogLevel.ToString(), Field.Store.YES);
                Field text = new TextField("Text", log.Text, Field.Store.YES);
                Field tags = new StringField("Tags", log.Tags.ToString(), Field.Store.YES);
                Field logTime = new StringField("LogTime", DateTools.DateToString(log.LogTime.TimeUtc, DateTools.Resolution.MILLISECOND), Field.Store.YES);
                Field fileName = new TextField("FileName", log.FileName, Field.Store.YES);
                Field lineNumber = new TextField("LineNumber", log.LineNumber.ToString(), Field.Store.YES);
                if (log.Exception != null)
                {
                    Document exDoc = GetExceptionDocuments(log.Exception);
                    Field exception = new TextField("Exception", exDoc.Get("IndexTS").ToString(), Field.Store.YES);
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
                Field logTime = new TextField("LogTime", DateTools.DateToString(log.LogTime.TimeUtc, DateTools.Resolution.MILLISECOND), Field.Store.YES);

                document.Add(logLevel);
                document.Add(logTime);
                document.Add(conclusions);
            }

            Field logType = new TextField("LogType", log.LogType.ToString(), Field.Store.YES);
            Field indexTS = new StringField("IndexTS", CreateIndexTS().ToString(), Field.Store.YES);
            Field AppId = new Int32Field("AppId", appId, Field.Store.YES);

            document.Add(logType);
            document.Add(indexTS);
            document.Add(AppId);

            return document;
        }

        Document GetExceptionDocuments(CKExceptionData exception)
        {
            Document document = new Document();

            Field message = new TextField("Message", exception.Message, Field.Store.YES);
            if(exception.StackTrace != null)
            {
                Field stack = new TextField("Stack", exception.StackTrace, Field.Store.YES);
                document.Add(stack);
            }
            Field indexTS = new StringField("IndexTS", CreateIndexTS().ToString(), Field.Store.YES);

            if (exception.AggregatedExceptions != null)
            {
                Field exceptionDepth = new Int32Field("ExceptionDepth", _exceptionDepth, Field.Store.YES);
                document.Add(exceptionDepth);
                if (_exceptionDepth == 0)
                {
                    StringBuilder exList = new StringBuilder();
                    foreach (CKExceptionData ex in exception.AggregatedExceptions)
                    {
                        exList.Append(GetExceptionDocuments(ex).Get("IndexTS"));
                        exList.AppendLine();
                        _exceptionDepth++;
                    }
                    Field aggregatedException = new TextField("AggregatedException", exList.ToString(), Field.Store.YES);
                    document.Add(aggregatedException);
                    _exceptionDepth = 0;
                }
            }

            if (exception.InnerException != null && exception.AggregatedExceptions == null)
            {
                Document exDoc = GetExceptionDocuments(exception.InnerException);
                Field innerException = new StringField("InnerException", exDoc.Get("IndexTS").ToString(), Field.Store.YES);
                document.Add(innerException);
            }

            if (exception.DetailedInfo != null)
            {
                Field details = new TextField("Details", exception.DetailedInfo, Field.Store.YES);
                document.Add(details);
            }
            if (exception.FileName != null)
            {
                Field filename = new StringField("Filename", exception.FileName, Field.Store.YES);
                document.Add(filename);
            }

            document.Add(message);
            document.Add(indexTS);

            _numberOfFileToCommit++;
            _writer.AddDocument(document);
            CommitIfNeeded();

            return document;
        }

        private Document GetOpenBlockDocument(IOpen openBlock)
        {
            Document document = new Document();

            Field openBlockAppId = new StringField("OpenAppId", openBlock.AppId.ToString(), Field.Store.YES);
            Field openStreamVersion = new StringField("OpenStreamVersion", openBlock.StreamVersion.ToString(), Field.Store.YES);
            Field openBlockBaseDir = new StringField("OpenBaseDirectory", openBlock.BaseDirectory, Field.Store.YES);
            Field openBlockInfos = new StringField("OpenInfos", openBlock.Info.ToString(), Field.Store.YES);

            document.Add(openBlockAppId);
            document.Add(openBlockBaseDir);
            document.Add(openBlockInfos);
            document.Add(openStreamVersion);

            return document;
        }

        private DateTimeStamp CreateIndexTS()
        {
            DateTimeStamp IndexTS = new DateTimeStamp(_lastDateTimeStamp, DateTime.UtcNow);
            _lastDateTimeStamp = IndexTS;
            return IndexTS;
        }

        private void CheckIds(IMulticastLogEntry log, int appId)
        {
            if (!_monitorIdList.Contains(log.MonitorId.ToString())) _monitorIdList.Add(log.MonitorId.ToString());
            if (!_appIdList.Contains(appId.ToString())) _appIdList.Add(appId.ToString());
        }

        private void InitializeIdList()
        {
            _monitorIdList = new HashSet<string>();
            _appIdList = new HashSet<string>();
            if (_searcher == null) return;
            TopDocs hits = _searcher.Search(new WildcardQuery(new Term("MonitorIdList", "*")));
            foreach (ScoreDoc doc in hits.ScoreDocs)
            {
                Document document = _searcher.GetDocument(doc);
                string[] monitorIds = document.Get("MonitorIdList").Split(' ');
                foreach (string id in monitorIds)
                {
                    if (!_monitorIdList.Contains(id)) _monitorIdList.Add(id);
                }
                string[] appIds = document.Get("AppIdList").Split(' ');
                foreach (string id in appIds)
                {
                    if (!_appIdList.Contains(id)) _appIdList.Add(id);
                }
            }
            if (hits.TotalHits == 0) CreateIdListDoc();
        }

        public void IndexLog(IMulticastLogEntry log, int appId)
        {
            CheckIds(log, appId);
            Document document = GetLogDocument(log, appId);
            _writer.AddDocument(document);
            _numberOfFileToCommit++;
            CommitIfNeeded();
        }

        public void IndexOpenBlock(IOpen openBlock)
        {
            Document document = GetOpenBlockDocument(openBlock);
            _writer.AddDocument(document);
            _numberOfFileToCommit++;
            CommitIfNeeded();
        }

        private string GetMonitorIdList ()
        {
            StringBuilder builder = new StringBuilder();
            foreach(string id in _monitorIdList)
            {
                builder.Append(id);
                builder.Append(" ");
            }
            return builder.ToString();
        }

        private string GetAppIdList()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string id in _appIdList)
            {
                builder.Append(id);
                builder.Append(" ");
            }
            return builder.ToString();
        }
        public void CreateIdListDoc()
        {
            Document doc = new Document();

            Field monitorIdList = new TextField("MonitorIdList", GetMonitorIdList(), Field.Store.YES);
            Field appIdList = new TextField("AppIdList", GetAppIdList(), Field.Store.YES);

            doc.Add(monitorIdList);
            doc.Add(appIdList);

            _writer.AddDocument(doc);
        }

        public void UpdateIdListDoc()
        {
            Document doc = new Document();

            Field monitorIdList = new TextField("MonitorIdList", GetMonitorIdList(), Field.Store.YES);
            Field appIdList = new TextField("AppIdList", GetAppIdList(), Field.Store.YES);

            doc.Add(monitorIdList);
            doc.Add(appIdList);

            _writer.UpdateDocument(new Term("MonitorIdList"), doc);
        }

        public void CommitIfNeeded()
        {
            if (_numberOfFileToCommit <= 0) return;
            if ((DateTime.UtcNow - _lastCommit).TotalSeconds >= 1 || _lastCommit == null || _numberOfFileToCommit >= 100)
            {
                _writer.Commit();
                _lastCommit = DateTime.UtcNow;
                _numberOfFileToCommit = 0;
            }
        }

        public void ForceCommit()
        {
            _writer.Commit();
        }

        public void Dispose()
        {
            UpdateIdListDoc();
            _writer.Dispose();
        }
    }
}
