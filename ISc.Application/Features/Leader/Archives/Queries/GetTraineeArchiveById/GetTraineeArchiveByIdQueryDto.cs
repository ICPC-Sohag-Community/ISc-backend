using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Archives.Queries.GetTraineeArchiveById
{
    public class GetTraineeArchiveByIdQueryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Grade { get; set; }
        public string College { get; set; }
        public Gender Gender { get; set; }
        public string CodeForceHandle { get; set; }
        public string? FacebookHandle { get; set; }
        public string? VjudgeHandle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CampName { get; set; }
        public bool IsComplete { get; set; }
        public int Year { get; set; }
    }
}
