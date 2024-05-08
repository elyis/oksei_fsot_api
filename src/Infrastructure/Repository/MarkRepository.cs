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
            CriterionEvaluationOption evaluationOption,
            EvaluatedAppraiserModel evaluatedAppraiser,
            CreateMarkBody markBody
        )
        {
            var mark = new MarkModel
            {
                EvaluationOption = evaluationOption,
                EvaluatedAppraiser = evaluatedAppraiser,
            };

            var result = await _context.Marks.AddAsync(mark);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<MarkModel?> GetByCriterionIdAsync(Guid criterionId)
            => await _context.Marks
                .Include(e => e.EvaluatedAppraiser)
                    .ThenInclude(e => e.Appraiser)
                .Include(e => e.EvaluationOption)
                .FirstOrDefaultAsync(e => e.EvaluationOption.CriterionId == criterionId);

        public async Task<bool> RemoveAsync(Guid id)
        {
            var mark = await _context.Marks.FirstOrDefaultAsync(e => e.Id == id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<MarkModel?> GetAsync(Guid criterionId, Guid teacherId, int monthIndex, int year)
        {
            return await _context.Marks
                .Include(e => e.EvaluatedAppraiser)
                .Include(e => e.EvaluationOption)
                .FirstOrDefaultAsync(e =>
                    e.EvaluationOption.CriterionId == criterionId &&
                    e.EvaluatedAppraiser.EvaluatedId == teacherId &&
                    e.CreatedAt.Month == monthIndex &&
                    e.CreatedAt.Year == year
            );
        }

        public async Task<List<MarkModel>> GetMarksByMonth(Guid userId, int monthIndex, int year)
        {
            var marks = await _context.Marks
                .Include(e => e.EvaluatedAppraiser.Evaluated)
                .Include(e => e.EvaluatedAppraiser.Appraiser)
                .Include(e => e.EvaluationOption)
                .Where(e =>
                    e.CreatedAt.Year == year &&
                    e.CreatedAt.Month == monthIndex &&
                    e.EvaluatedAppraiser.Evaluated.Id == userId)
                .ToListAsync();
            return marks;
        }

        public async Task<List<MarkModel>> GetMarksByMonth(int monthIndex, int year)
        {
            var marks = await _context.Marks
                .Include(e => e.EvaluatedAppraiser.Evaluated)
                .Include(e => e.EvaluatedAppraiser.Appraiser)
                .Include(e => e.EvaluationOption)
                .Where(e =>
                    e.CreatedAt.Month == monthIndex &&
                    e.CreatedAt.Year == year
                )
                .ToListAsync();
            return marks;
        }
    }
}