using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> AddAsync(SignUpBody body)
        {
            var oldUser = await GetAsync(body.Login);
            if (oldUser != null)
                return null;

            var newUser = new UserModel
            {
                Login = body.Login,
                Password = body.Password,
                RoleName = body.Role.ToString(),
                Fullname = body.Fullname,
            };

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<UserModel?> GetAsync(Guid id, bool IsRemoved = false)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Id == id && e.IsRemoved == IsRemoved);

        public async Task<UserModel?> GetAsync(string login, bool IsRemoved = false)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Login == login && e.IsRemoved == IsRemoved);

        public async Task<UserModel?> GetByTokenAsync(string refreshTokenHash, bool IsRemoved = false)
            => await _context.Users
                .FirstOrDefaultAsync(e =>
                e.Token == refreshTokenHash &&
                e.IsRemoved == IsRemoved
            );

        public async Task<List<UserModel>> GetUsersWithMarksByRoleAndMonth(UserRole role, bool IsRemoved = false)
        {
            var rolename = Enum.GetName(typeof(UserRole), role);
            var users = await _context.Users
                .Include(e => e.UserAppraisers)
                    .ThenInclude(e => e.Marks)
                        .ThenInclude(e => e.EvaluationOption)
                .Where(e =>
                    e.RoleName == rolename &&
                    e.IsRemoved == IsRemoved
                )
                .ToListAsync();

            return users;
        }

        public async Task<List<UserModel>> GetUsersWithMarksAndReportsByRoleAndMonth(UserRole role, bool IsRemoved = false)
        {
            var rolename = Enum.GetName(typeof(UserRole), role);
            var users = await _context.Users
                .Include(e => e.Reports)
                .Include(e => e.UserAppraisers)
                    .ThenInclude(e => e.Marks)
                        .ThenInclude(e => e.EvaluationOption)
                .Where(e =>
                    e.RoleName == rolename &&
                    e.IsRemoved == IsRemoved
                )
                .ToListAsync();

            return users;
        }

        public async Task<List<TeacherRatingSummary>> GetTeacherRatingSummariesAsync(int monthIndex, int year)
        {
            var teachers = await GetUsersWithMarksByRoleAndMonth(UserRole.Teacher);
            var rating = teachers.Select(teacher =>
            new TeacherRatingSummary
            {
                TeacherFullname = teacher.Fullname,
                Login = teacher.Login,

                TotalRating = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e =>
                        e.CreatedAt.Month == monthIndex &&
                        e.CreatedAt.Year == year)
                    .Sum(e => e.EvaluationOption.CountPoints),

                LastAssessment = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e =>
                        e.CreatedAt.Month == monthIndex &&
                        e.CreatedAt.Year == year)
                    .OrderByDescending(e => e.CreatedAt)
                    .FirstOrDefault()?.CreatedAt
                    .ToShortDateString(),
            })
            .OrderByDescending(e => e.TotalRating)
            .ToList();

            return rating;
        }

        public async Task<IEnumerable<TeacherPerformanceSummary>> GetTeacherPerformanceSummariesAsync(DateTime startDate, DateTime? endDate)
        {
            var teacherPerformanceSummaries = await _context.PremiumReports
                .Include(e => e.ReportTeachers)
                    .ThenInclude(e => e.User)
                .Where(e =>
                    e.Date.Month >= startDate.Month &&
                    e.Date.Year == startDate.Year &&
                    (endDate == null ||
                        (e.Date.Month <= endDate.Value.Month && e.Date.Year <= endDate.Value.Year)
                    )
                )
                .SelectMany(e => e.ReportTeachers, (report, teacher) => new TeacherPerformanceSummary
                {
                    Fullname = teacher.User.Fullname,
                    CountPoints = teacher.CountPoints,
                    Premium = (int)teacher.Premium,
                })
                .ToListAsync();

            return teacherPerformanceSummaries;
        }

        public async Task<IEnumerable<TeacherPerformanceSummary>> UpdateTeacherPerformanceSummary(int monthIndex, ReportData reportData, int year, string filename)
        {
            var teachers = await GetUsersWithMarksAndReportsByRoleAndMonth(UserRole.Teacher);
            var currentYear = DateTime.UtcNow.Year;

            var rating = teachers.Select(teacher =>
            new TeacherRatingSummary
            {
                TeacherFullname = teacher.Fullname,
                TotalRating = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e =>
                        e.CreatedAt.Month == monthIndex &&
                        e.CreatedAt.Year == year)
                    .Sum(e => e.EvaluationOption.CountPoints),
                LastAssessment = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e =>
                        e.CreatedAt.Month == monthIndex &&
                        e.CreatedAt.Year == year)
                    .OrderByDescending(e => e.CreatedAt)
                    .FirstOrDefault()?.CreatedAt
                    .ToShortDateString(),
                Login = teacher.Login,
            })
            .OrderByDescending(e => e.TotalRating)
            .ToList();

            var teacherPerformanceSummaries = rating.Select(teacher =>
                new TeacherPerformanceSummary
                {
                    Fullname = teacher.TeacherFullname,
                    CountPoints = (int)teacher.TotalRating,
                    Premium = (int)teacher.TotalRating * reportData.CostByPoint + reportData.FixedPremium
                });

            var premiumReport = await _context.PremiumReports
                        .FirstOrDefaultAsync(e =>
                            e.Date.Month == monthIndex &&
                            e.Date.Year == currentYear
            );

            var newPremiumReport = new PremiumReportModel
            {
                CostByPoint = reportData.CostByPoint,
                DistributablePremium = reportData.DistributablePremium,
                FixedPremium = reportData.FixedPremium,
                PartSemiannualPremium = reportData.PartSemiannualPremium,
                TotalAmountPoints = reportData.TotalAmountPoints,
                TotalAmountPremium = reportData.TotalAmountPremium,
                FileName = filename
            };

            if (premiumReport != null)
                _context.PremiumReports.Remove(premiumReport);

            premiumReport = (await _context.PremiumReports.AddAsync(newPremiumReport)).Entity;

            var teacherReports = new List<ReportTeacherModel>();
            var newReports = teachers.Select(teacher =>
            {
                var reports = teacher.Reports.Where(e => e.Date.Month == monthIndex && e.Date.Year == year);
                var teacherSummary = teacherPerformanceSummaries.First(e => e.Fullname == teacher.Fullname);

                var newReport = new ReportTeacherModel
                {
                    User = teacher,
                    CountPoints = teacherSummary.CountPoints,
                    Premium = teacherSummary.Premium,
                    PremiumReport = premiumReport,
                };

                if (reports.Any())
                    _context.ReportTeachers.RemoveRange(reports);

                return newReport;
            })
            .ToList();

            teacherReports.AddRange(newReports);
            await _context.ReportTeachers.AddRangeAsync(newReports);
            await _context.SaveChangesAsync();
            return teacherPerformanceSummaries;
        }

        public async Task<string?> UpdateTokenAsync(string newRefreshToken, Guid id)
        {
            var user = await GetAsync(id);
            if (user == null)
                return null;

            var currentDate = DateTime.UtcNow;
            if (user.TokenValidBefore > currentDate)
                return user.Token;

            user.Token = newRefreshToken;
            await _context.SaveChangesAsync();
            return newRefreshToken;
        }
        public async Task<bool> RemoveAsync(string login)
        {
            var user = await GetAsync(login);
            if (user == null)
                return true;

            user.IsRemoved = true;
            await _context.SaveChangesAsync();

            return user.IsRemoved;
        }

        public async Task<IEnumerable<UserModel>> GetUsers(IEnumerable<UserRole> roles, bool IsRemoved = false)
        {
            var roleString = roles.Select(e => e.ToString());
            return await _context.Users
                .Where(e => e.IsRemoved == IsRemoved && roleString.Contains(e.RoleName))
                .ToListAsync();
        }
    }
}