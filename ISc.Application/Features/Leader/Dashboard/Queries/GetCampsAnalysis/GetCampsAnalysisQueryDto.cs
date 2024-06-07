using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis
{
    public class GetCampsAnalysisQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly DueDate { get; set; }
        public int Progress { get; set; }
    }
}
