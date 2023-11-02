namespace oksei_fsot_api.src.Domain.Models
{
    public class PremiumReportModel
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int TotalAmountPremium { get; set; }
        public int FixedPremium { get; set; }
        public int TotalAmountPoints { get; set; }
        public int PartSemiannualPremium { get; set; }
        public int DistributablePremium { get; set; }
        public int CostByPoint { get; set; }
        public string? FileName { get; set; }

        public List<ReportTeacherModel> ReportTeachers { get; set; } = new();
    }
}