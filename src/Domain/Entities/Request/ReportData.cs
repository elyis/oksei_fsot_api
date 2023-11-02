using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class ReportData
    {
        //Общая сумма премии
        public int TotalAmountPremium { get; set; }

        //Фиксированная премия
        public int FixedPremium { get; set; }

        //Общее количество баллов
        public int TotalAmountPoints { get; set; }

        //Часть полугодовой премии
        public int PartSemiannualPremium { get; set; }

        //распределяемая премия
        public int DistributablePremium { get; set; }

        //Стоимость балла
        public int CostByPoint { get; set; }

        public ReportBody ToReportBody(string? filename, IEnumerable<TeacherPerformanceSummary> teacherPerformanceSummaries)
        {
            return new ReportBody
            {
                ReportData = this,
                TeacherPerformanceSummaries = teacherPerformanceSummaries.ToList(),
                UrlReport = filename == null ? null : $"{Constants.webPathToReports}{filename}",
            };
        }
    }
}