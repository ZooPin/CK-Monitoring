using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glouton.LogGenForm
{
    public static class ExceptionGenerator
    {
        public static Exception ThrowExceptionWithInner(bool loaderException = false)
        {
            Exception e;
            try { throw new Exception("Outer", loaderException ? ThrowLoaderException() : ThrowSimpleException("Inner")); }
            catch (Exception ex) { e = ex; }
            return e;
        }

        static Exception ThrowSimpleException(string message)
        {
            Exception e;
            try { throw new Exception(message); }
            catch (Exception ex) { e = ex; }
            return e;
        }
        static Exception ThrowLoaderException()
        {
            Exception e = null;
            try { Type.GetType("A.Type, An.Unexisting.Assembly", true); }
            catch (Exception ex) { e = ex; }
            return e;
        }
    }
}
