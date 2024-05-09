using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;

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

        public ReportData ToReportData()
        {
            return new ReportData
            {
                TotalAmountPremium = TotalAmountPremium,
                CostByPoint = CostByPoint,
                DistributablePremium = DistributablePremium,
                FixedPremium = FixedPremium,
                PartSemiannualPremium = PartSemiannualPremium,
                TotalAmountPoints = TotalAmountPoints,
            };
        }

        public ReportBody ToReportBody()
        {
            return new ReportBody
            {
                ReportData = ToReportData(),
                TeacherPerformanceSummaries = ReportTeachers.Select(e => new TeacherPerformanceSummary
                {
                    Fullname = e.User.Fullname,
                    CountPoints = e.CountPoints,
                    Premium = e.Premium,
                })
                .ToList(),
                UrlReport = FileName == null ? null : $"{Constants.webPathToReports}{FileName}",
            };
        }
    }
}