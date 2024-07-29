using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class Auditable:IAuditable
    {
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set ; }
    }
}
