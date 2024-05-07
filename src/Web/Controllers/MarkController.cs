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

        [HttpPost("mark"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Оценить по критерию")]
        [SwaggerResponse(200, Type = typeof(MarkBody))]
        [SwaggerResponse(400, Description = "Неверный идентификатор оцениваемого")]
        [SwaggerResponse(404, Description = "Неверный идентификатор критерия")]

        public async Task<IActionResult> CreateMark(
            [FromHeader(Name = "Authorization")] string token,
            CreateMarkBody markBody
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);
            var responseStatusCode = await _createMarkService.Invoke(markBody, tokenInfo.UserId);
            return responseStatusCode;
        }


        [HttpGet("marks/{loginTeacher}")]
        [SwaggerOperation("Получить список оценок учителя за месяц")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<MarkBody>))]
        [SwaggerResponse(400, Description = "Неверный индекс месяца, должен быть от 1 до 11")]
        [SwaggerResponse(404, Description = "Несуществующий логин")]
        [SwaggerResponse(409, Description = "Оценивание уже было")]


        public async Task<IActionResult> GetMarksByMonth(
            [Range(1, 12), FromQuery, Required] int monthIndex,
            string loginTeacher
        )
        {
            var teacher = await _userRepository.GetAsync(loginTeacher);
            if (teacher == null)
                return NotFound();

            var date = DateTime.UtcNow;
            var marksByMonth = await _markRepository.GetMarksByMonth(teacher.Id, monthIndex, date.Year);
            var result = marksByMonth.Select(e => e.ToMarkBody());
            return Ok(result);
        }
    }
}