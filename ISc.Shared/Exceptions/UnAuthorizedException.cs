using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Shared.Exceptions
{
    public class UnAuthorizedException:Exception
    {
        public UnAuthorizedException(string message):base(message) { }   
    }
}
