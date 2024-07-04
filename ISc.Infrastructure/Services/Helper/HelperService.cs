using ISc.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Infrastructure.Services.Helper
{
    internal class HelperService : IHelperService
    {
        public string GetRandomString(string firstName, string nationalId)
        {
            var random = new Random();
            var chars = "!@#$%";

            return "ISc" + firstName + Enumerable.Range(1, 3)
                .Select(x => nationalId[random.Next(13)]) + chars[random.Next(5)];
        }
    }
}
