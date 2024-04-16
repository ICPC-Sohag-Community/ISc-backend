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
        public string TaineeId { get; set; }
        public int SheetId { get; set; }
        public DateOnly Date { get; set; }
        public int SolvedProblems { get; set; }
    }
}
