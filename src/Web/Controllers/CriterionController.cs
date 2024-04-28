using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class CriterionController : ControllerBase
    {
        private readonly ICriterionRepository _criterionRepository;

        public CriterionController(ICriterionRepository criterionRepository)
        {
            _criterionRepository = criterionRepository;
        }

        [HttpPost("criterion"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Создать критерий")]
        [SwaggerResponse(200, Type = typeof(CriterionBody))]
        [SwaggerResponse(409, Description = "Критерий с таким именем существует")]

        public async Task<IActionResult> CreateCriterion(CreateCriterionBody body)
        {
            var result = await _criterionRepository.AddAsync(body);
            return result == null ? Conflict() : Ok(result.ToCriterionBody());
        }

        [HttpGet("criterions")]
        [SwaggerOperation("Получить все критерии")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CriterionBody>))]

        public async Task<IActionResult> GetAllCriterions(
            [FromQuery] int count = 1,
            [FromQuery] int offset = 0
        )
        {
            var result = await _criterionRepository.GetAllAsync(count, offset);
            return Ok(result.Select(e => e.ToCriterionBody()));
        }

        [HttpPut("criterion"), Authorize(Roles = nameof(UserRole.Appraiser))]
        public async Task<IActionResult> UpdateCriterion(
            UpdateCriterionBody criterionBody,
            [FromQuery, Required] Guid criterionId)
        {
            var result = await _criterionRepository.UpdateAsync(criterionBody, criterionId);
            return result == null ? NotFound() : Ok(result.ToCriterionBody());
        }

        [HttpPost("evaluation-option"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Добавить вариант оценивания критерия")]
        [SwaggerResponse(200, Type = typeof(EvaluationOptionResult))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> AddEvaluationOption(
            EvaluationOption evaluationOption,
            [FromQuery, Required] Guid criterionId)
        {
            var result = await _criterionRepository.AddCriterionEvaluationOption(evaluationOption, criterionId);
            return result != null ? Ok(result.ToEvaluationOptionResult()) : BadRequest();
        }

        [HttpDelete("evaluation-option"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Удалить вариант оценивания")]
        [SwaggerResponse(204)]

        public async Task<IActionResult> DeleteEvaluationOption([FromQuery, Required] Guid evaluationOptionId)
        {
            var result = await _criterionRepository.RemoveCriterionEvaluation(evaluationOptionId);
            return NoContent();
        }

        [HttpGet("criterion/{criterionId}")]
        [SwaggerOperation("Получить критерий по id")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, Description = "Неверный идентификатор")]

        public async Task<IActionResult> GetById(Guid criterionId)
        {
            var result = await _criterionRepository.GetAsync(criterionId);
            return result == null ? NotFound() : Ok();
        }

        [HttpDelete("criterion/{criterionId}"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Удалить критерий по id")]
        [SwaggerResponse(204)]

        public async Task<IActionResult> RemoveById(Guid criterionId)
        {
            _ = await _criterionRepository.RemoveAsync(criterionId);
            return NoContent();
        }
    }
}