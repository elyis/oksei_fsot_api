namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class TeacherRatingSummary
    {
        public string TeacherFullname { get; set; }
        public decimal TotalRating { get; set; }
        public DateOnly? LastAssessment { get; set; }
        public string Login { get; set; }
    }
}