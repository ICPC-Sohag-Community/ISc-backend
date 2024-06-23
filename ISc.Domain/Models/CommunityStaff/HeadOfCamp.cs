using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models.CommunityStaff
{
    public class HeadOfCamp : Staff
    {
        public string? About { get; set; }
        public virtual Account Account { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
    }

}
