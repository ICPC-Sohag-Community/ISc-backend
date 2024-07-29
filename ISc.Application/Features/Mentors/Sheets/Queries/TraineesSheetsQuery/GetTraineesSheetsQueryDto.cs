using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Sheets.Queries.TraineesSheetsQuery
{
    public class GetTraineesSheetsQueryDto
    {
        public List<SheetDto> Sheets { get; set; }
        public List<TraineeDto> Trainees { get; set; }
    }
    public class SheetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int ProblemCount { get; set; }
    }
    public class TraineeDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public List<Tracking> Tracking { get; set; }
    }
    public class Tracking
    {
        public int SheetId { get; set; }
        public int SolvedProblems { get; set; }
    }
}
