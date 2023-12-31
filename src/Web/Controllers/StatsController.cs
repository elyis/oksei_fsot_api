using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.IRepository;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public StatsController(
            IUserRepository userRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatsByCurrentAndPreviousMonth(
            [FromHeader(Name = "Authorization")] string token
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);

            var currentDate = DateTime.UtcNow;
            var previousMonthDate = currentDate.AddMonths(-1);
            const float maxMarkValue = 54.0f;

            var teacherRatingCurrentMonth = await _userRepository.GetTeacherRatingSummariesAsync(currentDate.Month, tokenInfo.OrganizationId);
            var teacherRatingPreviousMonth = await _userRepository.GetTeacherRatingSummariesAsync(previousMonthDate.Month, tokenInfo.OrganizationId);

            var CreateMonthStats = (IEnumerable<TeacherRatingSummary> ratings, bool isUnderway) =>
            {
                var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(currentDate.Month);

                return new MonthStatsBody
                {
                    Name = monthName,
                    RatingTeachers = ratings.Select(e => e.TeacherFullname).ToList(),
                    UnderWay = isUnderway,
                    LastChange = ratings.OrderByDescending(e => e.LastAssessment).FirstOrDefault()?.LastAssessment,
                    Progress = (float)Math.Floor((float)ratings.Sum(e => e.TotalRating) / ((float)ratings.Count() * maxMarkValue) * 100f)
                };
            };

            var currentMonthStats = CreateMonthStats(teacherRatingCurrentMonth, true);
            var previousMonthStats = CreateMonthStats(teacherRatingPreviousMonth, false);

            var monthStats = new CurrentAndPreviousMonthInfo
            {
                CurrentMonth = currentMonthStats,
                PreviousMonth = previousMonthStats
            };

            return Ok(monthStats);
        }
    }
}