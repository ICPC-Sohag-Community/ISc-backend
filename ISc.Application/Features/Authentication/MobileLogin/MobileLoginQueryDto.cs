using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Authentication.MobileLogin
{
    public class MobileLoginQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
