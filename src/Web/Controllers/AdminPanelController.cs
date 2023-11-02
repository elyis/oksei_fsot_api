using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Utility;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ExcelGenerator<TeacherPerformanceSummary> _excelTeacherSummaryGenerator;


        public AdminPanelController(
            IUserRepository userRepository,
            IJwtService jwtService,
            ExcelGenerator<TeacherPerformanceSummary> excelTeacherSummaryGenerator
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _excelTeacherSummaryGenerator = excelTeacherSummaryGenerator;
        }


        [HttpDelete("user/{login}")]
        public async Task<IActionResult> RemoveUser(string login)
        {
            var result = await _userRepository.RemoveAsync(login);
            return result == false ? BadRequest() : NoContent();
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser(
            [FromHeader(Name = "Authorizaton")] string token,
            UserBody body
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);
            var result = await _userRepository.UpdateUserAsync(body, tokenInfo.UserId);
            return result == null ? NotFound() : Ok();
        }

        [HttpPost("report/{monthIndex}")]
        public async Task<IActionResult> CreateReport(
            [FromHeader(Name = "Authorization")] string token,
            ReportData reportData, int monthIndex
        )
        {
            if (monthIndex < 1 || monthIndex > 12)
                return BadRequest();

            var tokenInfo = _jwtService.GetTokenInfo(token);

            var year = DateTime.UtcNow.Year;
            var teacherPerformanceSummaries = await _userRepository.UpdateTeacherPerformanceSummary(monthIndex, reportData, tokenInfo.OrganizationId);
            var headers = new string[] { "ФИО", "Число очков", "Размер премии" };

            var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(monthIndex);
            var filename = $"{monthName}{year}.xlsx";
            var result = await _excelTeacherSummaryGenerator.GenerateExcelAsync(headers, teacherPerformanceSummaries, Constants.pathToTeacherReports, filename);
            var report = reportData.ToReportBody(filename, teacherPerformanceSummaries);
            return Ok(report);
        }
    }
}