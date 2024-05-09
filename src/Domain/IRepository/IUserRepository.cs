using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel?> AddAsync(SignUpBody body);
        Task<UserModel?> GetAsync(Guid id, bool IsRemoved = false);
        Task<UserModel?> GetAsync(string login, bool IsRemoved = false);
        Task<List<UserModel>> GetUsersWithMarksByRoleAndMonth(UserRole role, bool IsRemoved = false);
        Task<List<UserModel>> GetUsersWithMarksAndReportsByRoleAndMonth(UserRole role, bool IsRemoved = false);
        Task<IEnumerable<TeacherPerformanceSummary>> UpdateTeacherPerformanceSummary(int monthIndex, ReportData reportData, int year, string filename);
        Task<IEnumerable<TeacherPerformanceSummary>> GetTeacherPerformanceSummariesAsync(DateTime startDate, DateTime? endDate);
        Task<List<TeacherRatingSummary>> GetTeacherRatingSummariesAsync(int monthIndex, int year);
        Task<string?> UpdateTokenAsync(string newRefreshToken, Guid id);
        Task<bool> RemoveAsync(string login);
        Task<UserModel?> GetByTokenAsync(string refreshTokenHash, bool IsRemoved = false);
        Task<IEnumerable<UserModel>> GetUsers(IEnumerable<UserRole> roles, bool IsRemoved = false);
    }
}