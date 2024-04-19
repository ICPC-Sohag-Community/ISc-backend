using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Shared.Exceptions
{
    public class SerivceErrorException:Exception
    {
        public SerivceErrorException(string message) : base(message) { }
    }
}
