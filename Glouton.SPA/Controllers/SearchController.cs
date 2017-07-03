using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Glouton.SPA.Services;
using Glouton.SPA.Models.LogViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Glouton.SPA.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        [HttpGet("query/{query}")]
        public List<ILogViewModel> Search(string query)
        {
            List<ILogViewModel> list = LuceneSearcherService.Search(query);
            return list;
        }

        [HttpGet("all/{maxLogToReturn}")]
        public List<ILogViewModel> GetAllLog(int maxLogToReturn)
        {
            return LuceneSearcherService.GetAllLog(maxLogToReturn);
        }

        [HttpGet("{dateStart}/{dateEnd}/{monitorId}/{appId}/{fields}/{logLevel}/{keyword}")]
        [HttpGet("{dateStart}/{dateEnd}/")]
        public List<ILogViewModel> SearchByDate(string monitorId, string appId, DateTime dateStart, DateTime dateEnd, string[] fields, string[] logLevel, string keyword)
        {
            if (monitorId == null || monitorId == "*") monitorId = "All";
            if (appId == null || appId == "*") appId = "All";
            if (fields.Length == 0 || fields[0] == "*") fields = new string[] { "Tags", "FileName", "Text" };
            if (logLevel.Length == 0 || logLevel[0] == "*") logLevel = new string[] { "Debug", "Trace", "Info", "Warn", "Error", "Fatal" };
            if (keyword == null) keyword = "*";
            return LuceneSearcherService.GetLogWithFilters(monitorId, appId, dateStart, dateEnd, fields, logLevel, keyword);
        }
    }
}
