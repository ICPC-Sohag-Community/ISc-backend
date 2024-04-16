using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
	public class Notification:BaseEntity,ISoftEntity
	{
        public string AccountId { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
    }
}
