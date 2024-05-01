using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICriterionRepository _criterionRepository;
        private readonly IJwtService _jwtService;

        public StatsController(
            IUserRepository userRepository,
            ICriterionRepository criterionRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _criterionRepository = criterionRepository;
            _jwtService = jwtService;
        }

        [HttpGet]
        [SwaggerOperation("Получить статистику за текущий и предыдущий месяц")]
        [SwaggerResponse(200, Type = typeof(CurrentAndPreviousMonthInfo))]
        public async Task<IActionResult> GetStatsByCurrentAndPreviousMonth(
            [FromHeader(Name = "Authorization")] string token
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);

            var currentDate = DateTime.UtcNow;
            var previousMonthDate = currentDate.AddMonths(-1);
            float countPointsByCriterions = await _criterionRepository.GetCountPointsByCriterions();

            var teacherRatingCurrentMonth = await _userRepository.GetTeacherRatingSummariesAsync(currentDate.Month, currentDate.Year);
            var teacherRatingPreviousMonth = await _userRepository.GetTeacherRatingSummariesAsync(previousMonthDate.Month, previousMonthDate.Year);

            var CreateMonthStats = (IEnumerable<TeacherRatingSummary> ratings, bool isUnderway, int monthIndex) =>
            {
                var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(monthIndex);

                return new MonthStatsBody
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthName),
                    RatingTeachers = ratings.Select(e => AbbreviateName(e.TeacherFullname)).ToList(),
                    UnderWay = isUnderway,
                    LastChange = ratings.OrderByDescending(e => e.LastAssessment).FirstOrDefault()?.LastAssessment,
                    Progress = (float)Math.Floor((float)ratings.Sum(e => e.TotalRating) / (ratings.Count() * countPointsByCriterions) * 100f)
                };
            };

            var currentMonthStats = CreateMonthStats(teacherRatingCurrentMonth, true, currentDate.Month);
            var previousMonthStats = CreateMonthStats(teacherRatingPreviousMonth, false, previousMonthDate.Month);

            var monthStats = new CurrentAndPreviousMonthInfo
            {
                CurrentMonth = currentMonthStats,
                PreviousMonth = previousMonthStats
            };

            return Ok(monthStats);
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