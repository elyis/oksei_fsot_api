using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IMarkRepository
    {
        Task<MarkModel?> AddAsync(CriterionEvaluationOption evaluationOption, EvaluatedAppraiserModel evaluatedAppraiser, CreateMarkBody markBody);
        Task<MarkModel?> GetAsync(Guid id);
        Task<List<MarkModel>> GetMarksByMonth(Guid userId, int monthIndex, int year);
        Task<List<MarkModel>> GetMarksByMonth(int monthIndex, int year);
        Task<MarkModel?> GetAsync(Guid criterionId, Guid teacherId, int monthIndex, int year);
    }
}