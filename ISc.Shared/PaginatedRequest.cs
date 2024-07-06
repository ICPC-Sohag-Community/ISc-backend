using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Shared
{
    public record PaginatedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public string? KeyWord { get; set; }
    }
}
