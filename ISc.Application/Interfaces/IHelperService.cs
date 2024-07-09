using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Interfaces
{
    public interface IHelperService
    {
        string GetRandomPasswordString(string firstName,string nationalId);
        string GetRandomUserNameString(string firstName,string nationalId);
    }
}
