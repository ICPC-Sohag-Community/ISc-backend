using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class BaseEntity:Auditable
    {
        public int Id { get; set; }
    }
}
