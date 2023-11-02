using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IEvaluatedAppraiserRepository
    {
        Task<EvaluatedAppraiserModel?> AddAsync(UserModel appraiser, UserModel evaluated);
        Task<EvaluatedAppraiserModel?> AddOrGetAsync(UserModel appraiser, UserModel evaluated);
        Task<EvaluatedAppraiserModel?> GetAsync(Guid appraiserId, Guid evaluatedId);
    }
}