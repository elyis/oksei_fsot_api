using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface ICriterionRepository
    {
        Task<CriterionModel?> AddAsync(CreateCriterionBody body);
        Task<CriterionModel?> GetAsync(Guid id);
        Task<CriterionModel?> UpdateAsync(UpdateCriterionBody body, Guid id);
        Task<bool> RemoveCriterionEvaluation(Guid id);
        Task<bool> RemoveAsync(Guid id);
        Task<CriterionEvaluationOption?> AddCriterionEvaluationOption(EvaluationOption evaluationOption, Guid criterionId);
        Task<List<CriterionModel>> GetAllAsync(int count, int offset);
        Task<float> GetCountPointsByCriterions();
    }
}