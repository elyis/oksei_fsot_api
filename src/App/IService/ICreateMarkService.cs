using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Request;

namespace oksei_fsot_api.src.App.IService
{
    public interface ICreateMarkService
    {
        Task<IActionResult> Invoke(CreateMarkBody createMarkBody, Guid criterionEvaluationId, Guid appraiserId);
    }
}