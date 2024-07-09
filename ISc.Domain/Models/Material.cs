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
        public string Title { get; set; }
        public string Link { get; set; }
        public int MaterialOrder { get; set; }
        public int SheetId { get; set; }
        public virtual Sheet Sheet { get; set; }
    }
}
