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

            var criterionEvaluationOptions = body.EvaluationOptions
                .Select(e => new CriterionEvaluationOption
                {
                    CountPoints = e.CountPoints,
                    Description = e.Description,
                })
                .ToList();

            var newCriterion = new CriterionModel
            {
                Description = body.Description,
                Name = body.Name,
                SerialNumber = body.SerialNumber,
                EvaluationOptions = criterionEvaluationOptions
            };

            var result = await _context.Criterions.AddAsync(newCriterion);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<List<CriterionModel>> GetAllAsync(int count, int offset)
            => await _context.Criterions
                .Include(e => e.EvaluationOptions)
                .Skip(offset)
                .Take(count)
                .ToListAsync();

        public async Task<CriterionModel?> GetAsync(Guid id)
            => await _context.Criterions
                .Include(e => e.EvaluationOptions)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<CriterionEvaluationOption?> GetCriterionEvaluationOptionAsync(Guid id)
        {
            return await _context.CriterionEvaluationOptions
                .FirstOrDefaultAsync(e => e.Id == id);
        }

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

        public async Task<bool> RemoveCriterionEvaluation(Guid id)
        {
            var criterionEvaluation = await _context.CriterionEvaluationOptions.FirstOrDefaultAsync(e => e.Id == id);
            if (criterionEvaluation == null)
                return true;

            _context.CriterionEvaluationOptions.Remove(criterionEvaluation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CriterionEvaluationOption?> AddCriterionEvaluationOption(EvaluationOption evaluationOption, Guid criterionId)
        {
            var criterion = await GetAsync(criterionId);
            if (criterion == null)
                return null;

            var criterionEvaluationOption = new CriterionEvaluationOption
            {
                Description = evaluationOption.Description,
                CountPoints = evaluationOption.CountPoints,
                Criterion = criterion,
            };

            await _context.CriterionEvaluationOptions.AddAsync(criterionEvaluationOption);
            await _context.SaveChangesAsync();

            return criterionEvaluationOption;
        }

        public async Task<CriterionModel?> UpdateAsync(UpdateCriterionBody body, Guid id)
        {
            var criterion = await GetAsync(id);
            if (criterion == null)
                return null;

            criterion.Name = body.Name;
            criterion.Description = body.Description;
            await _context.SaveChangesAsync();

            return criterion;
        }

        public async Task<float> GetCountPointsByCriterions()
        {
            var countPoints = await _context.CriterionEvaluationOptions.SumAsync(e => e.CountPoints);
            return countPoints;
        }
    }
}