using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http.Headers;
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
        private readonly IReportRepository _reportRepository;
        private readonly ExcelGenerator<TeacherPerformanceSummary> _excelTeacherSummaryGenerator;

        public AdminPanelController(
            IUserRepository userRepository,
            ExcelGenerator<TeacherPerformanceSummary> excelTeacherSummaryGenerator,
            IReportRepository reportRepository
        )
        {
            _userRepository = userRepository;
            _reportRepository = reportRepository;
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

        [HttpGet("report/{monthIndex}"), Authorize(Roles = nameof(UserRole.Appraiser))]
        [SwaggerOperation("Получить отчет за месяц")]
        [SwaggerResponse(200, Type = typeof(ReportBody))]
        [SwaggerResponse(404)]

        public async Task<IActionResult> GetReport(
            [FromHeader(Name = nameof(HttpRequestHeaders.Authorization))] string token,
            int monthIndex
        )
        {
            var date = DateTime.UtcNow;
            var report = await _reportRepository.GetReport(monthIndex, date.Year);
            return report == null ? NotFound() : Ok(report.ToReportBody());
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
            var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(monthIndex);
            var filename = $"{monthName}{year}.xlsx";

            var teacherPerformanceSummaries = await _userRepository.UpdateTeacherPerformanceSummary(monthIndex, reportData, year, filename);

            var result = await _excelTeacherSummaryGenerator.GenerateExcelAsync(headers, teacherPerformanceSummaries, Constants.pathToTeacherReports, filename);
            var report = reportData.ToReportBody(filename, teacherPerformanceSummaries);
            return Ok(report);
        }

        [HttpGet("report/file/{filename}")]
        [SwaggerOperation("Получить файл отчета за месяц")]
        [SwaggerResponse(200, Type = typeof(File))]
        [SwaggerResponse(400)]
        public async Task<IActionResult> GetReport(
            string filename
        )
        {
            var file = await _excelTeacherSummaryGenerator.GetExcelAsync(Constants.pathToTeacherReports, filename);
            if (file == null)
                return NotFound();

            return File(file, "application/octet-stream", filename);
        }
    }
}