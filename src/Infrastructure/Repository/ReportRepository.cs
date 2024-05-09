using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PremiumReportModel?> GetReport(int month, int year)
        {
            return await _context.PremiumReports
                .Include(e => e.ReportTeachers)
                    .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(e => e.Date.Month == month && e.Date.Year == year);
        }
    }
}