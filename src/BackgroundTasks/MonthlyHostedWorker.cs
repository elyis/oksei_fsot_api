// using System.Globalization;
// using Microsoft.EntityFrameworkCore;
// using oksei_fsot_api.src.Domain.Entities.Response;
// using oksei_fsot_api.src.Domain.IRepository;
// using oksei_fsot_api.src.Infrastructure.Data;
// using oksei_fsot_api.src.Infrastructure.Repository;
// using oksei_fsot_api.src.Utility;

// namespace oksei_fsot_api.src.BackgroundTasks
// {
//     public class MonthlyHostedWorker : BackgroundService
//     {
//         private readonly ILogger<MonthlyHostedWorker> _logger;
//         private readonly ExcelGenerator<MarkLogBody> _excelMarkLogBodyGenerator;
//         private readonly ExcelGenerator<TeacherPerformanceSummary> _excelTeacherSummaryGenerator;
//         private readonly IMarkLogRepository _markLogRepository;
//         private readonly IUserRepository _userRepository;
//         private readonly AppDbContext _context;


//         public MonthlyHostedWorker(
//             ILogger<MonthlyHostedWorker> logger,
//             ExcelGenerator<MarkLogBody> excelMarkLogBodyGenerator,
//             ExcelGenerator<TeacherPerformanceSummary> excelTeacherSummaryGenerator,
//             IConfiguration config
//         )
//         {
//             var connectionString = config.GetConnectionString("Default");
//             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connectionString);
//             var options = optionsBuilder.Options;

//             _excelMarkLogBodyGenerator = excelMarkLogBodyGenerator;
//             _excelTeacherSummaryGenerator = excelTeacherSummaryGenerator;
//             _logger = logger;
//             _context = new AppDbContext(options, config);
//             _markLogRepository = new MarkLogRepository(_context);
//             _userRepository = new UserRepository(_context);
//         }

//         protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             var previousMonthDate = DateTime.UtcNow.AddMonths(-1);
//             var previousMonthReport = await _context.PremiumReports.FirstOrDefaultAsync(e =>
//                 e.Date.Month == previousMonthDate.Month &&
//                 e.Date.Year == previousMonthDate.Year
//             );
//             var previousMonthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(previousMonthDate.Month);

//             if (!File.Exists($"{Constants.pathToMarkLogs}{previousMonthName}{previousMonthDate.Year}.xlsx"))
//             {
//                 await GenerateMarkLogReportAsync(previousMonthDate);
//                 await GeneratePremiumReportAsync(previousMonthDate);
//             }

//             var previousDate = DateTime.UtcNow.AddYears(-1);
//             var (startDate, endDate) = CalculateYearRange(previousDate.AddYears(-1));

//             if (!File.Exists($"{Constants.pathToMarkAnnualLogs}{startDate.Year}-{endDate.Year}.xlsx"))
//                 await GenerateAnnualMarkLogReport(previousDate);

//             if (!File.Exists($"{Constants.pathToTeacherAnnualReports}{startDate.Year}-{endDate.Year}.xlsx"))
//                 await GenerateAnnualPremiumReport(previousDate);


//             while (true)
//             {
//                 var currentDate = DateTime.UtcNow;
//                 await GenerateMonthlyReportAsync(currentDate, stoppingToken);
//             }
//         }

//         private async Task GenerateMonthlyReportAsync(DateTime currentDate, CancellationToken stoppingToken)
//         {
//             var countDaysByMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
//             var countDays = countDaysByMonth - currentDate.Day;
//             _logger.LogInformation($"Дней до генерации отчета ежемесячного отчета: {countDays}");
//             using var timer = new PeriodicTimer(TimeSpan.FromDays(countDays));

//             var timeIsEnd = await timer.WaitForNextTickAsync(stoppingToken);
//             if (!timeIsEnd)
//             {
//                 _logger.LogError("Montly timer end with false");
//                 return;
//             }

//             await GenerateMarkLogReportAsync(currentDate);
//             await GeneratePremiumReportAsync(currentDate);
//         }

//         private async Task GenerateMarkLogReportAsync(DateTime date)
//         {
//             var headersMarkLogs = new string[] { "Номер критерия", "Дата оценивания", "Учитель", "Оценивающий", "Оценка" };
//             var logsByMonth = await _markLogRepository.GetAllByMonthAsync(date.Month, date.Year);
//             var logs = logsByMonth.Select(e => e.ToMarkLogBody());
//             var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(date.Month);

//             var filename = $"{monthName}{date.Year}.xlsx";
//             var result = await _excelMarkLogBodyGenerator.GenerateExcelAsync(headersMarkLogs, logs, Constants.pathToMarkLogs, filename);
//             if (!result)
//             {
//                 _logger.LogInformation($"Отчет по логам оценивания за {date.ToShortDateString()} не создан");
//                 return;
//             }

//             _logger.LogInformation($"Отчет по логам оценивания за {date.ToShortDateString()} создан: {filename}");

//             if (date.Month >= 7 && date.Month <= 8)
//                 await GenerateAnnualMarkLogReport(date);
//         }

//         private async Task GenerateAnnualPremiumReport(DateTime date)
//         {
//             var (startDate, endDate) = CalculateYearRange(date);
//             var teacherPerformanceSummaries = await _userRepository.GetTeacherPerformanceSummariesAsync(startDate, endDate);
//             var headersPremiumReport = new string[] { "ФИО", "Число очков", "Размер премии" };

//             var filename = $"{startDate.Year}-{endDate.Year}.xlsx";
//             var result = await _excelTeacherSummaryGenerator.GenerateExcelAsync(headersPremiumReport, teacherPerformanceSummaries, Constants.pathToTeacherAnnualReports, filename);
//             if (!result)
//                 _logger.LogInformation($"Отчет по премиям за {startDate.ToShortDateString()}-{endDate.ToShortDateString()} не создан");
//             else
//                 _logger.LogInformation($"Отчет по премиям за {startDate.ToShortDateString()}-{endDate.ToShortDateString()} создан: {filename}");

//             if (date.Month >= 7 && date.Month <= 8)
//                 await GenerateAnnualPremiumReport(date);
//         }
//         private async Task GenerateAnnualMarkLogReport(DateTime date)
//         {
//             var (startDate, endDate) = CalculateYearRange(date);
//             var headersMarkLogs = new string[] { "Номер критерия", "Дата оценивания", "Учитель", "Оценивающий", "Оценка" };

//             var logsByYear = await _markLogRepository.GetAllByYearRangeAsync(startDate, endDate);
//             var logs = logsByYear.Select(e => e.ToMarkLogBody());

//             var filename = $"{startDate.Year}-{endDate.Year}.xlsx";
//             var result = await _excelMarkLogBodyGenerator.GenerateExcelAsync(headersMarkLogs, logs, Constants.pathToMarkAnnualLogs, filename);
//             if (!result)
//                 _logger.LogInformation($"Отчет по логам оценивания за {startDate.ToShortDateString()}-{endDate.ToShortDateString()} не создан");
//             else
//                 _logger.LogInformation($"Отчет по логам оценивания за {startDate.ToShortDateString()}-{endDate.ToShortDateString()} создан: {filename}");
//         }

//         private (DateTime startDate, DateTime endDate) CalculateYearRange(DateTime date)
//         {
//             int startYear = date.Month < 9 ? date.Year - 1 : date.Year;
//             var startDate = new DateTime(startYear, 9, 1);
//             return (startDate, startDate.AddMonths(10));
//         }


//         private async Task GeneratePremiumReportAsync(DateTime date)
//         {
//             var premiumReport = await _context.PremiumReports
//                 .FirstOrDefaultAsync(e =>
//                     e.Date.Month == date.Month &&
//                     e.Date.Year == date.Year
//             );

//             if (premiumReport == null)
//             {
//                 _logger.LogInformation($"Отсутствуют данные для отчета по премиям за {date.ToShortDateString()}");
//                 return;
//             }

//             var headersPremiumReport = new string[] { "ФИО", "Число очков", "Размер премии" };
//             var teacherPerformanceSummary = await _userRepository.GetTeacherPerformanceSummariesAsync(date, null);

//             var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetMonthName(date.Month);
//             var filename = $"{monthName}{date.Year}.xlsx";
//             var result = await _excelTeacherSummaryGenerator.GenerateExcelAsync(headersPremiumReport, teacherPerformanceSummary, Constants.pathToTeacherReports, filename);

//             if (!result)
//             {
//                 _logger.LogInformation($"Отчет по премиям за {date.ToShortDateString()} не создан");
//                 return;
//             }

//             premiumReport.FileName = filename;
//             await _context.SaveChangesAsync();
//             _logger.LogInformation($"Отчет по премиям за {date.ToShortDateString()} создан: {filename}");
//         }
//     }
// }