using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class NewRegisteration : BaseEntity, ISoftEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Grade { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public string CodeForceHandle { get; set; }
        public string? FacebookLink { get; set; }
        public string? VjudgeHandle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Comment { get; set; }
        public bool HasLaptop { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
    }
}
