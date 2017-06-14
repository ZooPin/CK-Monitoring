using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Glouton.SPA.Models.ExceptionViewModel
{
    public interface IInnerExceptionViewModel
    {
        string Stack { get; set; }
        string Details { get; set; }
        string FileName { get; set; }
    }
}
