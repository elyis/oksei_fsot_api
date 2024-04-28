using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class MarkRepository : IMarkRepository
    {
        private readonly AppDbContext _context;

        public MarkRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MarkModel?> AddAsync(
            CriterionModel criterion,
            EvaluatedAppraiserModel evaluatedAppraiser,
            CreateMarkBody markBody
            )
        {
            var mark = new MarkModel
            {
                Criterion = criterion,
                Date = markBody.Date,
                EvaluatedAppraiser = evaluatedAppraiser,
                Value = markBody.Mark,
            };

            var result = await _context.Marks.AddAsync(mark);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<MarkModel?> GetAsync(Guid id)
            => await _context.Marks
                .Include(e => e.EvaluatedAppraiser)
                    .ThenInclude(e => e.Appraiser)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<MarkModel?> GetAsync(Guid criterionId, Guid teacherId, int monthIndex, int year)
        {
            return await _context.Marks
                .Include(e => e.EvaluatedAppraiser)
                .FirstOrDefaultAsync(e =>
                    e.CriterionId == criterionId &&
                    e.EvaluatedAppraiser.EvaluatedId == teacherId &&
                    e.Date.Month == monthIndex &&
                    e.Date.Year == year
            );
        }

        public async Task<List<MarkModel>> GetMarksByMonth(Guid userId, int monthIndex, int year)
        {
            var marks = await _context.Marks
                .Include(e => e.EvaluatedAppraiser.Evaluated)
                .Include(e => e.EvaluatedAppraiser.Appraiser)
                .Where(e =>
                    e.Date.Year == year &&
                    e.Date.Month == monthIndex &&
                    e.EvaluatedAppraiser.Evaluated.Id == userId)
                .ToListAsync();
            return marks;
        }

        public async Task<List<MarkModel>> GetMarksByMonth(int monthIndex, int year)
        {
            var marks = await _context.Marks
                .Include(e => e.EvaluatedAppraiser.Evaluated)
                .Include(e => e.EvaluatedAppraiser.Appraiser)
                .Where(e =>
                    e.Date.Month == monthIndex &&
                    e.Date.Year == year
                )
                .ToListAsync();
            return marks;
        }
    }
}