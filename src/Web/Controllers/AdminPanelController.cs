using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Utility;
using Swashbuckle.AspNetCore.Annotations;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ExcelGenerator<TeacherPerformanceSummary> _excelTeacherSummaryGenerator;


        public AdminPanelController(
            IUserRepository userRepository,
            ExcelGenerator<TeacherPerformanceSummary> excelTeacherSummaryGenerator
        )
        {
            _userRepository = userRepository;
            _excelTeacherSummaryGenerator = excelTeacherSummaryGenerator;
        }


        [HttpDelete("user/{login}"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Удалить пользователя")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveUser([Required] string login)
        {
            var result = await _userRepository.RemoveAsync(login);
            return result == false ? BadRequest() : NoContent();
        }

        [HttpPost("report/{monthIndex}"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Сгенерировать отчет за месяц")]
        [SwaggerResponse(200, Type = typeof(ReportBody))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> CreateReport(
            ReportData reportData,
            [Range(1, 12)] int monthIndex
        )
        {
            var year = DateTime.UtcNow.Year;
            var headers = new string[] { "ФИО", "Число очков", "Размер премии" };

            var teacherPerformanceSummaries = await _userRepository.UpdateTeacherPerformanceSummary(monthIndex, reportData, year);

            var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(monthIndex);
            var filename = $"{monthName}{year}.xlsx";

            var result = await _excelTeacherSummaryGenerator.GenerateExcelAsync(headers, teacherPerformanceSummaries, Constants.pathToTeacherReports, filename);
            var report = reportData.ToReportBody(filename, teacherPerformanceSummaries);
            return Ok(report);
        }
    }
}