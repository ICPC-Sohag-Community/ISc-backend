using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models
{
	public class Notification:BaseEntity,ISoftEntity
	{
        public bool IsRead { get; set; }
        public string Message { get; set; }
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
