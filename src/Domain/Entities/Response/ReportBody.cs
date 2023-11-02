using oksei_fsot_api.src.Domain.Entities.Request;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class ReportBody
    {
        public ReportData ReportData { get; set; }
        public string? UrlReport { get; set; }
        public List<TeacherPerformanceSummary> TeacherPerformanceSummaries { get; set; } = new();
    }
}