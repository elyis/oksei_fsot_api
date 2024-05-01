using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class TeacherController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public TeacherController(
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet("teachers")]
        [SwaggerOperation("Получить рейтинг преподавателей за месяц")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TeacherBody>))]
        public async Task<IActionResult> GetAllTeacher(
            [FromHeader(Name = "Authorization")] string token,
            [FromQuery, Range(1, 12)] int monthIndex,
            [FromQuery] int year
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);

            var teacherRating = await _userRepository.GetTeacherRatingSummariesAsync(monthIndex, year);
            var teachers = teacherRating.Select(e =>
            new TeacherBody
            {
                Fullname = AbbreviateName(e.TeacherFullname),
                IsKing = false,
                LastChange = e.LastAssessment,
                Login = e.Login,
                SumMarks = (int)e.TotalRating,
            })
            .OrderByDescending(e => e.SumMarks);

            if (teachers.Any())
                teachers.First().IsKing = true;

            return Ok(teachers);
        }


        private static string AbbreviateName(string name)
        {
            var names = name.Split(' ');
            if (names.Length == 2)
                return names[0] + " " + names[1][0] + ".";

            return names[0] + " " + names[1][..1] + ". " + names[^1][..1] + ".";
        }
    }
}