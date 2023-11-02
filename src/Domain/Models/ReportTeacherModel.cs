namespace oksei_fsot_api.src.Domain.Models
{
    public class ReportTeacherModel
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int CountPoints { get; set; }
        public float Premium { get; set; }

        public Guid UserId { get; set; }
        public UserModel User { get; set; }

        public PremiumReportModel PremiumReport { get; set; }
        public Guid PremiumReportId { get; set; }
    }
}