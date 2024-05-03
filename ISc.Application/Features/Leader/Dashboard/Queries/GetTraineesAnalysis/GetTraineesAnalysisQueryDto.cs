using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis
{
	public class GetTraineesAnalysisQueryDto
	{
        public int NumberOfTrainees { get; set; }
        public int NumberOfMaleTrainees { get; set; }
        public int NumberOfFemaleTrainees { get; set; }
        public List<CollegeAnalisisDto> CollegesAnalisis { get; set; }
    }
    public class CollegeAnalisisDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
