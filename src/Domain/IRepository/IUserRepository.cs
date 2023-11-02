using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel?> AddAsync(SignUpBody body, OrganizationModel organization);
        Task<UserModel?> GetAsync(Guid id);
        Task<UserModel?> GetAsync(string login);
        Task<List<UserModel>> GetUsersWithMarksByRoleAndMonth(UserRole role, Guid organizationId);
        Task<List<UserModel>> GetUsersWithMarksAndReportsByRoleAndMonth(UserRole role, Guid organizationId);
        Task<IEnumerable<TeacherPerformanceSummary>> UpdateTeacherPerformanceSummary(int monthIndex, ReportData reportData, Guid organizationId);
        Task<IEnumerable<TeacherPerformanceSummary>> GetTeacherPerformanceSummariesAsync(DateTime startDate, DateTime? endDate, Guid organizationId);
        Task<List<TeacherRatingSummary>> GetTeacherRatingSummariesAsync(int monthIndex, Guid organizationId);
        Task<string?> UpdateTokenAsync(string newRefreshToken, Guid id);
        Task<bool> RemoveAsync(string login);

        Task<UserModel?> GetByTokenAsync(string refreshTokenHash);
        Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename);
        Task<UserModel?> UpdateUserAsync(UserBody body, Guid id);
    }
}