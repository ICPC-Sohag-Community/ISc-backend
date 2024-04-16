using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
	public class Material:BaseEntity,ISoftEntity
	{
        public string Link { get; set; }
        public string? Description { get; set; }
        public int SheetId { get; set; }
        public virtual Sheet Sheet { get; set; }
    }
}
