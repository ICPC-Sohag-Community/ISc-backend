using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Mentors.Queries.GetMentor
{
    public class GetMentorQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string College { get; set; }
        public string? PhotoUrl { get; set; }
        public string? FacebookLink { get; set; }
        public string CodeForceHandle { get; set; }
        public string? VjudgeHandle { get; set; }
    }
}
