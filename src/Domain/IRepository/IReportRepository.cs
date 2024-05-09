using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IReportRepository
    {
        Task<PremiumReportModel?> GetReport(int month, int year);
    }
}