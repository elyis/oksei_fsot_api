using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace oksei_fsot_api.src.Domain.Models
{
    [Index(nameof(Login), IsUnique = true)]
    [Index(nameof(Token))]
    public class UserModel
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Login { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } = Enum.GetName(typeof(UserRole), UserRole.Teacher)!;
        public string? Token { get; set; }
        public DateTime? TokenValidBefore { get; set; }
        public DateOnly? LastEvaluationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<EvaluatedAppraiserModel> UserAppraisers { get; set; } = new();
        public List<ReportTeacherModel> Reports { get; set; } = new();

    }
}