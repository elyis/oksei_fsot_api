using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class EvaluatedAppraiserRepository : IEvaluatedAppraiserRepository
    {
        private readonly AppDbContext _context;

        public EvaluatedAppraiserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EvaluatedAppraiserModel?> AddAsync(UserModel appraiser, UserModel evaluated)
        {
            var relation = await GetAsync(appraiser.Id, evaluated.Id);
            if (relation != null)
                return null;

            var newRelation = new EvaluatedAppraiserModel
            {
                Appraiser = appraiser,
                Evaluated = evaluated
            };

            var result = await _context.EvaluatedAppraisers.AddAsync(newRelation);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<EvaluatedAppraiserModel?> AddOrGetAsync(UserModel appraiser, UserModel evaluated)
        {
            var relation = await GetAsync(appraiser.Id, evaluated.Id);
            if (relation != null)
                return relation;

            var newRelation = new EvaluatedAppraiserModel
            {
                Appraiser = appraiser,
                Evaluated = evaluated
            };

            var result = await _context.EvaluatedAppraisers.AddAsync(newRelation);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<EvaluatedAppraiserModel?> GetAsync(Guid appraiserId, Guid evaluatedId)
            => await _context.EvaluatedAppraisers
                .FirstOrDefaultAsync(e =>
                    e.EvaluatedId == evaluatedId &&
                    e.AppraiserId == appraiserId
                );
    }
}