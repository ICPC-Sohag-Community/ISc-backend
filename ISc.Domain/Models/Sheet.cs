using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class Sheet : BaseEntity, ISoftEntity
    {
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int MinimumPassingPrecent { get; set; }
        public int SheetOrder { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public int SheetCodefroceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SheetType Type { get; set; }
        public SheetStatus Status { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public virtual ICollection<TraineeAccessSheet> TraineesAccess { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
