using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.SystemRoles.Queries.GetUserRoles;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetById
{
    public class GetTraineeByIdQueryDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public int Grade { get; set; }
        public DateOnly BirthDate { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public string? PhotoUrl { get; set; }
        public string CodeForceHandle { get; set; }
        public string? VjudgeHandle { get; set; }
        public List<GetUserRolesQueryDto> UserRoles { get; set; }
    }
}
