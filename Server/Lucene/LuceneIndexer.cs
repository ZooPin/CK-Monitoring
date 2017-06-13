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
    class LuceneIndexer
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

        private Document getDocument(FileInfo file)
        {
            Document document = new Document();
            //index file contents and name
            Field contentField = new TextField(LuceneConstant.Content, new StreamReader(file.Open(FileMode.Open)));
            Field fileNameField = new TextField(LuceneConstant.FileName, file.Name, Field.Store.YES);
            //index file path
            Field filePahtField = new TextField(LuceneConstant.FilePath, file.FullName, Field.Store.YES);

            document.Add(contentField);
            document.Add(fileNameField);
            document.Add(filePahtField);

            return document;
        }

        //private Document getLogDocument(ILogEntry log)
        //{
        //    Document document = new Document();

        //    Field logType = new TextField("LogType", log.LogType, Field.Store.YES);
        //    Field logLevel = new TextField("LogLevel", log.LogLevel, Field.Store.YES);
        //    Field text = new TextField("Text", log.Text, Field.Store.YES);
        //    Field tags = new TextField("Tags", log.Tags, Field.Store.YES);
        //    Field logTime = new TextField("LogTime", log.LogTime, Field.Store.YES);
        //    Field exception = new TextField("Exception", log.Exception, Field.Store.YES);
        //    Field fileName = new TextField("FileName", log.FileName, Field.Store.YES);
        //    Field lineNumber = new TextField("LineNumber", log.LineNumber, Field.Store.YES);
        //    Field conclusions = new TextField("Conclusions", log.Conclusions, Field.Store.YES);

        //    document.Add(logType);
        //    document.Add(logLevel);
        //    document.Add(text);
        //    document.Add(tags);
        //    document.Add(logTime);
        //    document.Add(exception);
        //    document.Add(fileName);
        //    document.Add(lineNumber);
        //    document.Add(conclusions);

        //    return document;
        //}

        private void indexFile(FileInfo file)
        {
            Console.WriteLine("Indexing : " + file.FullName);
            Document document = getDocument(file);
            _writer.AddDocument(document);
        }

        public int CreateIndex (string dataDirPath)
        {
            FileInfo[] files = new DirectoryInfo(dataDirPath).GetFiles();

            foreach(FileInfo file in files)
            {
                if (file.Extension == ".txt") indexFile(file);
            }

            return _writer.NumDocs; 
        }
    }
}
