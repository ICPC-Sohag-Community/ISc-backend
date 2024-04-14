using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceUserDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int rating { get; set; }
        public int maxRating { get; set; }
        public string rank { get; set; }
        public string maxRank { get; set; }
        public string avater { get; set; }
        public long lastOnlineTimeSeconds { get; set; }
        public long registrationTimeSeconds { get; set; }
        public string? organization { get; set; }
        public string? email { get; set; }
    }
}
