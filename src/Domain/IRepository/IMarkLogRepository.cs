using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IMarkLogRepository
    {
        Task<IEnumerable<MarkLogModel>> GetAllByMonthAsync(int monthIndex, int year);
        Task<MarkLogModel?> AddAsync(MarkModel mark);
        Task<IEnumerable<MarkLogModel>> GetAllByYearRangeAsync(DateTime startDate, DateTime endDate);
    }
}