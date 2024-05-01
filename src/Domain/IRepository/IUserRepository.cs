using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel?> AddAsync(SignUpBody body);
        Task<UserModel?> GetAsync(Guid id);
        Task<UserModel?> GetAsync(string login);
        Task<List<UserModel>> GetUsersWithMarksByRoleAndMonth(UserRole role);
        Task<List<UserModel>> GetUsersWithMarksAndReportsByRoleAndMonth(UserRole role);
        Task<IEnumerable<TeacherPerformanceSummary>> UpdateTeacherPerformanceSummary(int monthIndex, ReportData reportData, int year);
        Task<IEnumerable<TeacherPerformanceSummary>> GetTeacherPerformanceSummariesAsync(DateTime startDate, DateTime? endDate);
        Task<List<TeacherRatingSummary>> GetTeacherRatingSummariesAsync(int monthIndex, int year);
        Task<string?> UpdateTokenAsync(string newRefreshToken, Guid id);
        Task<bool> RemoveAsync(string login);
        Task<UserModel?> GetByTokenAsync(string refreshTokenHash);
        Task<IEnumerable<UserModel>> GetUsers(string role);
    }
}