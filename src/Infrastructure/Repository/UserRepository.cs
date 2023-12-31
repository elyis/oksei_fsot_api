using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using webApiTemplate.src.App.Provider;
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

        public async Task<UserModel?> AddAsync(SignUpBody body, OrganizationModel organization)
        {
            var oldUser = await GetAsync(body.Login);
            if (oldUser != null)
                return null;

            var newUser = new UserModel
            {
                Login = body.Login,
                Password = Hmac512Provider.Compute(body.Password),
                RoleName = Enum.GetName(typeof(UserRole), body.Role)!,
                Fullname = body.Fullname,
                Organization = organization,
            };

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<UserModel?> GetAsync(Guid id)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<UserModel?> GetAsync(string login)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Login == login);

        public async Task<UserModel?> GetByTokenAsync(string refreshTokenHash)
            => await _context.Users
                .FirstOrDefaultAsync(e =>
                e.Token == refreshTokenHash
            );

        public async Task<List<UserModel>> GetUsersWithMarksByRoleAndMonth(UserRole role, Guid organizationId)
        {
            var rolename = Enum.GetName(typeof(UserRole), role);
            var users = await _context.Users
                .Include(e => e.UserAppraisers)
                    .ThenInclude(e => e.Marks)
                .Where(e =>
                    e.RoleName == rolename &&
                    e.OrganizationId == organizationId
                )
                .ToListAsync();

            return users;
        }

        public async Task<List<UserModel>> GetUsersWithMarksAndReportsByRoleAndMonth(UserRole role, Guid organizationId)
        {
            var rolename = Enum.GetName(typeof(UserRole), role);
            var users = await _context.Users
                .Include(e => e.Reports)
                .Include(e => e.UserAppraisers)
                    .ThenInclude(e => e.Marks)
                .Where(e =>
                    e.RoleName == rolename &&
                    e.OrganizationId == organizationId
                )
                .ToListAsync();

            return users;
        }

        public async Task<List<TeacherRatingSummary>> GetTeacherRatingSummariesAsync(int monthIndex, Guid organizationId)
        {
            var teachers = await GetUsersWithMarksByRoleAndMonth(UserRole.Teacher, organizationId);

            var rating = teachers.Select(teacher =>
            new TeacherRatingSummary
            {
                TeacherFullname = teacher.Fullname,
                Login = teacher.Login,

                TotalRating = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e => e.Date.Month == monthIndex)
                    .Sum(e => e.Value),

                LastAssessment = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e => e.Date.Month == monthIndex)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault()?.Date,
            })
            .OrderByDescending(e => e.TotalRating)
            .ToList();

            return rating;
        }

        public async Task<IEnumerable<TeacherPerformanceSummary>> GetTeacherPerformanceSummariesAsync(DateTime startDate, DateTime? endDate, Guid organizationId)
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
                    Premium = teacher.Premium,
                })
                .ToListAsync();

            return teacherPerformanceSummaries;
        }

        public async Task<IEnumerable<TeacherPerformanceSummary>> UpdateTeacherPerformanceSummary(int monthIndex, ReportData reportData, Guid organizationId)
        {
            var teachers = await GetUsersWithMarksAndReportsByRoleAndMonth(UserRole.Teacher, organizationId);
            var currentYear = DateTime.UtcNow.Year;

            var rating = teachers.Select(teacher =>
            new TeacherRatingSummary
            {
                TeacherFullname = teacher.Fullname,
                TotalRating = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e => e.Date.Month == monthIndex)
                    .Sum(e => e.Value),
                LastAssessment = teacher.UserAppraisers
                    .SelectMany(e => e.Marks)
                    .Where(e => e.Date.Month == monthIndex)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault()?.Date,
                Login = teacher.Login
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

            var teacherReports = new List<ReportTeacherModel>();
            var newReports = teachers.Select(teacher =>
            {
                var reports = teacher.Reports.Where(e => e.Date.Month == monthIndex);
                var teacherSummary = teacherPerformanceSummaries.First(e => e.Fullname == teacher.Fullname);

                var newReport = new ReportTeacherModel
                {
                    User = teacher,
                    CountPoints = teacherSummary.CountPoints,
                    Premium = teacherSummary.Premium,
                };

                if (reports.Any())
                {
                    var existReport = reports.First();
                    teacherReports.Add(existReport);
                    existReport.Premium = teacherSummary.Premium;
                    existReport.CountPoints = teacherSummary.CountPoints;
                }

                return newReport;
            })
            .ToList();

            teacherReports.AddRange(newReports);
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
                ReportTeachers = teacherReports
            };

            if (premiumReport == null)
                await _context.PremiumReports.AddAsync(newPremiumReport);
            else
                premiumReport = newPremiumReport;

            await _context.ReportTeachers.AddRangeAsync(newReports);
            await _context.SaveChangesAsync();
            return teacherPerformanceSummaries;
        }



        public async Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            user.Image = filename;
            await _context.SaveChangesAsync();
            return user;
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

            var result = _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<UserModel?> UpdateUserAsync(UserBody body, Guid id)
        {
            var user = await GetAsync(id);
            if (user == null)
                return null;

            user.Fullname = body.Fullname;
            user.RoleName = Enum.GetName(typeof(UserRole), body.Role)!;
            await _context.SaveChangesAsync();
            return user;
        }

    }
}