using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class MarkLogRepository : IMarkLogRepository
    {
        private readonly AppDbContext _context;

        public MarkLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MarkLogModel>> GetAllByMonthAsync(int monthIndex, int year)
            => await _context.MarkLogs
                .Include(e => e.Mark)
                    .ThenInclude(e => e.EvaluatedAppraiser.Appraiser)
                .Include(e => e.Mark)
                    .ThenInclude(e => e.EvaluatedAppraiser.Evaluated)
                .Where(e =>
                    e.Date.Month == monthIndex &&
                    e.Date.Year == year)
                .ToListAsync();

        public async Task<IEnumerable<MarkLogModel>> GetAllByYearRangeAsync(DateTime startDate, DateTime endDate)
            => await _context.MarkLogs
                .Include(e => e.Mark)
                    .ThenInclude(e => e.EvaluatedAppraiser.Appraiser)
                .Include(e => e.Mark)
                    .ThenInclude(e => e.EvaluatedAppraiser.Evaluated)
                .Where(e =>
                    e.Date.Month >= startDate.Month &&
                    e.Date.Month <= endDate.Month &&
                    e.Date.Year >= startDate.Year &&
                    e.Date.Year <= endDate.Year)
                .ToListAsync();

        public async Task<MarkLogModel?> AddAsync(MarkModel mark)
        {
            var logModel = new MarkLogModel
            {
                Mark = mark
            };

            var result = await _context.MarkLogs.AddAsync(logModel);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }
    }
}