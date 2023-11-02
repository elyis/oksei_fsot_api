using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface ICriterionRepository
    {
        Task<CriterionModel?> AddAsync(CreateCriterionBody body);
        Task<CriterionModel?> GetAsync(Guid id);
        Task<bool> RemoveAsync(Guid id);
        Task<List<CriterionModel>> GetAllAsync();
    }
}