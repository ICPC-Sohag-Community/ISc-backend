using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class Sheet
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int MinimumPassingPrecent { get; set; }
        public int SheetOrder { get; set; }
        public int CampId { get; set; }
        public bool IsSohag { get; set; }
        public int ProblemCount { get; set; }
        public int SheetCodefroceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
