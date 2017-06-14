using Glouton.SPA.Models.LogViewModel;
using GloutonLucene;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.ExceptionViewModel
{
    public interface IExceptionViewModel
    {
        IInnerExceptionViewModel InnerException { get; set; }
        string Message { get; set; }
        string Stack { get; set; }

        
    }
}
