using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class MarkController : ControllerBase
    {
        private readonly IMarkRepository _markRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICreateMarkService _createMarkService;

        private readonly IJwtService _jwtService;

        public MarkController(
            IMarkRepository markRepository,
            IUserRepository userRepository,
            ICreateMarkService createMarkService,
            IJwtService jwtService
            )
        {
            _markRepository = markRepository;
            _userRepository = userRepository;
            _createMarkService = createMarkService;
            _jwtService = jwtService;
        }

        [HttpPost("mark/{criterionId}"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Оценить по критерию")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, Description = "Неверный идентификатор оцениваемого")]
        [SwaggerResponse(404, Description = "Неверный идентификатор критерия")]

        public async Task<IActionResult> CreateMark(
            [FromHeader(Name = "Authorization")] string token,
            CreateMarkBody markBody,
            [FromQuery, Required] Guid criterionId
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);
            var responseStatusCode = await _createMarkService.Invoke(markBody, criterionId, tokenInfo.UserId);
            return responseStatusCode;
        }


        [HttpGet("marks/{loginTeacher}/{monthIndex}")]
        [SwaggerOperation("Получить список оценок учителя за месяц")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<MarkBody>))]
        [SwaggerResponse(400, Description = "Неверный индекс месяца, должен быть от 1 до 11")]
        [SwaggerResponse(404, Description = "Несуществующий логин")]


        public async Task<IActionResult> GetMarksByMonth(
            [Range(1, 12), FromQuery, Required] int monthIndex,
            [FromQuery, Required] string loginTeacher,
            [FromQuery, Required] int year
        )
        {
            var teacher = await _userRepository.GetAsync(loginTeacher);
            if (teacher == null)
                return NotFound();

            var marksByMonth = await _markRepository.GetMarksByMonth(teacher.Id, monthIndex, year);
            var result = marksByMonth.Select(e => e.ToMarkBody());
            return Ok(result);
        }
    }
}