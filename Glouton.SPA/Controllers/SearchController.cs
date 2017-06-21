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
    }
}
