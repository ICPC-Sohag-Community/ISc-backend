using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateOnly CreationDate { get; set; }
        public int Grade { get; set; }
        public Gender Gender {get;set;}
        public DateTime? LastLoginDate { get; set; }
        public string? PhotoUrl { get; set; }
        public string CodeForceHadle { get; set; }
        public string? VjudgeHandle { get; set; }
        public string PhoneNumber { get; set; }

    }
}
