using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayOnCustomerFilter
{
    public class GetOnCustomerFilterQueryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Grade { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public string CodeForceHandle { get; set; }
        public string? FacebookHandle { get; set; }
        public string? VjudgeHandle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Comment { get; set; }
        public bool HasLaptop { get; set; }
    }

    public enum GetOnCustomerFilterQueryDtoColumn
    {
        College,
        Year,
        Gender,
        HasLaptop
    }
}
