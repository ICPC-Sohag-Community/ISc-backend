using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class Camp : BaseEntity,ISoftEntity
    {
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Term? Term { get; set; }
        public int DurationInWeeks { get; set; }
        public bool OpenForRegister { get; set; }
        public virtual ICollection<MentorsOfCamp> Mentors { get; set; }
        public virtual ICollection<NewRegisteration> RegisterationRequests { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Sheet> Sheets { get; set; }
        public virtual ICollection<Trainee> Trainees { get; set; }
    }
}
