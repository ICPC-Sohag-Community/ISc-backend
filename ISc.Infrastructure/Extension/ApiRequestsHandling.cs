using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ISc.Infrastructure.Extension
{
    public static class ApiRequestsHandling
    {
        public static string CreateUri(this Dictionary<string,string> queryParams,string controller)
        {
            return $"{controller}?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}
