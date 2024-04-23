namespace ISc.Domain.Models.CommunityStuff
{
    public class Mentor : Stuff
    {
        public int? SessionId { get; set; }
        public virtual Session? Session { get; set; }
        public string? About { get; set; }
        public virtual ICollection<Trainee> Trainees { get; set; }
        public virtual ICollection<MentorsOfCamp> Camps { get; set; }
    }
}
