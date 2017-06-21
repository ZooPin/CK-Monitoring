using Glouton.SPA.Models.ExceptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.LogViewModel
{
    public enum LogType {
        Opengroup,
        Line,
        CloseGroup
    }

    public static class Log
    {
        static public string Exception => "Exception";
        static public string LogTime => "LogTime";
        static public string LogType => "LogType";
        static public string LogLevel => "LogLevel";
        static public string Conclusion => "Conclusion";
        static public string Tags => "Tags";
        static public string Text => "Text";
        static public string SourceFileName => "FileName";
        static public string LineNumber => "LineNumber";
        static public string InnerException => "InnerException";
        static public string Stack => "Stack";
        static public string Message => "Message";
        static public string FileName => "Filename";
        static public string Detail => "Details";
        static public string MonitorId => "MonitorId";
        static public string GroupDepth => "GroupDepth";
        static public string PreviousEntryType => "PreviousEntryType";
        static public string PreviousLogTime => "PreviousLogTime";
    } 

    public interface ILogViewModel
    {
        LogType LogType { get; }
        IExceptionViewModel Exception { get; set; }
        string LogTime { get; set; }
        string LogLevel { get; set; }
    }
}
