using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class Notification
	{
        public int Id { get; set; }
        public string AccountId { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
    }
}
