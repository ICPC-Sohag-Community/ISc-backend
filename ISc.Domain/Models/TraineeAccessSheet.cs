using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
	public class TraineeAccessSheet:ISoftEntity
	{
        public string TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
        public int SheetId { get; set; }
        public virtual Sheet Sheet { get; set; }
        public DateOnly AccessDate { get; set; }
        public int SolvedProblems { get; set; }
    }
}
