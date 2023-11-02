using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class CriterionRepository : ICriterionRepository
    {
        private readonly AppDbContext _context;

        public CriterionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CriterionModel?> AddAsync(CreateCriterionBody body)
        {
            var criterion = await _context.Criterions.FirstOrDefaultAsync(e => e.Name == body.Name);
            if (criterion != null)
                return null;

            var newCriterion = new CriterionModel
            {
                Description = body.Description,
                Name = body.Name,
                LowerBound = body.LowerBound,
                UpperBound = body.UpperBound,
            };

            var result = await _context.Criterions.AddAsync(newCriterion);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<List<CriterionModel>> GetAllAsync()
            => await _context.Criterions
                .ToListAsync();

        public async Task<CriterionModel?> GetAsync(Guid id)
            => await _context.Criterions
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> RemoveAsync(Guid id)
        {
            var criterion = await GetAsync(id);
            if (criterion != null)
            {
                _ = _context.Criterions.Remove(criterion);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}