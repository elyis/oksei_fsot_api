using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
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

        [HttpPost("criterion")]
        [SwaggerOperation("Создать критерий")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, Description = "Критерий с таким именем существует")]

        public async Task<IActionResult> CreateCriterion(CreateCriterionBody body)
        {
            var result = await _criterionRepository.AddAsync(body);
            return result == null ? Conflict() : Ok();
        }

        [HttpGet("criterions")]
        [SwaggerOperation("Получить все критерии")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<CriterionBody>))]

        public async Task<IActionResult> GetAllCriterions()
        {
            var result = await _criterionRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToCriterionBody()));
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

        [HttpDelete("criterion/{criterionId}")]
        [SwaggerOperation("Удалить критерий по id")]
        [SwaggerResponse(204)]

        public async Task<IActionResult> RemoveById(Guid criterionId)
        {
            _ = await _criterionRepository.RemoveAsync(criterionId);
            return NoContent();
        }
    }
}