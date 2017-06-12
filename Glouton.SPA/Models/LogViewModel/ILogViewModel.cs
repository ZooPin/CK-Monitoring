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

    static public class Log
    {
        static public string Exception => "Exception";
        static public string LogTime => "LogTime";
        static public string LogLevel => "LogLevel";
        static public string Conclusion => "Conclusion";
        static public string Tags => "Tags";
        static public string Text => "Text";
        static public string SourceFileName => "FileName";
        static public string LineNumber => "LineNumber";
    } 

    interface ILogViewModel
    {
        LogType LogType { get; }
        string Exception { get; set; }
        string LogTime { get; set; }
        string LogLevel { get; set; }
    }
}
