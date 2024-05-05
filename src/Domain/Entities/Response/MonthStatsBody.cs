namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class MonthStatsBody
    {
        public string Name { get; set; }
        public bool UnderWay { get; set; }
        public int Month { get; set; }
        public DateOnly? LastChange { get; set; }
        public float Progress { get; set; }
        public List<string> RatingTeachers { get; set; } = new();
    }

    public class CurrentAndPreviousMonthInfo
    {
        public MonthStatsBody CurrentMonth { get; set; }
        public MonthStatsBody PreviousMonth { get; set; }
    }
}