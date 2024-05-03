using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis
{
	public class GetTraineesAnalysisQueryDto
	{
        public int TraineesCount => FemalesCount + MalesCount;
        public int MalesCount { get; set; }
        public int FemalesCount { get; set; }
        public List<CollegeAnalisisDto> CollegesAnalisis { get; set; }
    }
    public class CollegeAnalisisDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
