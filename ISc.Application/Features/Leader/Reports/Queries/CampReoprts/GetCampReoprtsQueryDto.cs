using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Reports.Queries.CampReoprts
{
    public class GetCampReoprtsQueryDto
    {
        public GetCampReoprtsQueryDto()
        {
            SheetsRates = new();
            ContestRates = new();
            TraineesColleges = new();
            TraineesGrades = new();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MaleCount {  get; set; }
        public int FemaleCount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int ProgressPrecent { get; set; }
        public List<SheetAverageRateDto> SheetsRates { get; set; }
        public List<ContestAverageRateDto> ContestRates { get; set; }
        public List<TraineesInCollegeDto> TraineesColleges { get; set; }
        public List<TraineesInGradeDto> TraineesGrades { get; set; }
    }

    public class SheetAverageRateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
    public class ContestAverageRateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
    public class TraineesInGradeDto
    {
        public int Grade { get; set; }
        public int Count { get; set; }
    }
    public class TraineesInCollegeDto
    {
        public College College { get; set; }
        public int Count { get; set; }
    }


}
