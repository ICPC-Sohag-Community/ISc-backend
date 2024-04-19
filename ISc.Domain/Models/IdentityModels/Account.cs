using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models.IdentityModels
{
    public class Account:IdentityUser,IAuditable
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public int Grade { get; set; }
        public Gender Gender { get; set; }
        public string? PhotoUrl { get; set; }
        public string CodeForceHadle { get; set; }
        public string? VjudgeHandle { get; set; }

    }
}
