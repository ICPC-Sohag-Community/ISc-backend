using ISc.Domain.Comman.Enums;

namespace ISc.Domain.Models
{
    public abstract class Archive : Auditable
    {
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public int Grade { get; set; }
        public string College { get; set; }
        public Gender Gender { get; set; }
        public string? PhotoUrl { get; set; }
        public string CodeForceHandle { get; set; }
        public string? FacebookHandle { get; set; }
        public string? VjudgeHandle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }
    }
}
