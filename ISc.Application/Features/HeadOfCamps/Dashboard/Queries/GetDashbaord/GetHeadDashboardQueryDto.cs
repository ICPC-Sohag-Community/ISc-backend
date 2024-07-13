namespace ISc.Application.Features.HeadOfCamps.Dashboard.Queries.GetDashbaord
{
    public class GetHeadDashboardQueryDto
    {
        public GetHeadDashboardQueryDto()
        {
            SheetsAnalysis = [];
            ContestsAnalysis = [];
        }

        public int TrineesMaleCount { get; set; }
        public int TraineesFemaleCount { get; set; }
        public int MentorsMaleCount { get; set; }
        public int MentorsFemaleCount { get; set; }
        public int ProgressPrecent => (int)(SheetsAnalysis.Sum(x => x.Precent) / SheetsAnalysis.Count()) * 100;
        public int RemainDays { get; set; }
        public List<SheetAverageSolvingDto> SheetsAnalysis { get; set; }
        public List<ContestAverageSolvingDto> ContestsAnalysis { get; set; }
    }

    public class SheetAverageSolvingDto
    {
        public string Name { get; set; }
        public double Precent { get; set; }
    }
    public class ContestAverageSolvingDto
    {
        public string Name { get; set; }
        public double Precent { get; set; }
    }
}
