using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class TraineeArchive : Archive, ISoftEntity
    {
        public string CampName { get; set; }
        public bool IsComplete { get; set; }
        public int Year { get; set; }
    }
}
