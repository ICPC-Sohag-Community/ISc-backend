using FluentEmail.Core;
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
        public string GetRandomPasswordString(string firstName, string nationalId)
        {
            var random = new Random();
            var chars = "!@#$%";
            StringBuilder randomId = new StringBuilder();
            Enumerable.Range(1, 3)
               .Select(x => nationalId[random.Next(13)]).ForEach(x => randomId.Append(x));

            return "ISc" + firstName + randomId + chars[random.Next(5)];
        }

        public string GetRandomUserNameString(string firstName, string nationalId)
        {
            var random = new Random();
            StringBuilder randomId = new StringBuilder();
            Enumerable.Range(1, 3)
               .Select(x => nationalId[random.Next(13)]).ForEach(x => randomId.Append(x));

            return "ISc" + firstName + randomId + firstName.Last();
        }
    }
}
