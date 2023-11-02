using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.IRepository;
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

        [HttpGet("teachers/{monthIndex}")]
        public async Task<IActionResult> GetAllTeacher(
            [FromHeader(Name = "Authorization")] string token,
            int monthIndex)
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);

            var teacherRating = await _userRepository.GetTeacherRatingSummariesAsync(monthIndex, tokenInfo.OrganizationId);

            var teachers = teacherRating.Select(e =>
            new TeacherBody
            {
                Fullname = e.TeacherFullname,
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
    }
}