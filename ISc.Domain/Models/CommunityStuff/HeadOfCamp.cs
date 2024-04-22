using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models.CommunityStuff
{
    public class HeadOfCamp : Account
    {
        public string? About { get; set; }
        public int CampId { get; set; }
        public Camp Camp { get; set; }
    }

}
