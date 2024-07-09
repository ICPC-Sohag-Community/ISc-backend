using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetAllWithPagination
{
    public class GetAllTraineeWithPaginationQueryDto
    {
        public string Id { get; set; }
        public string FullName {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CodeforceHandle { get; set; }
    }
}
