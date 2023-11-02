using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Entities.Response;
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
        public string? Image { get; set; }
        public DateOnly? LastEvaluationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid OrganizationId { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<EvaluatedAppraiserModel> UserAppraisers { get; set; } = new();
        public List<ReportTeacherModel> Reports { get; set; } = new();



        public ProfileBody ToProfileBody()
        {
            return new ProfileBody
            {
                Login = Login,
                Role = Enum.Parse<UserRole>(RoleName),
                UrlIcon = string.IsNullOrEmpty(Image) ? null : $"{Constants.webPathToProfileIcons}{Image}",
                LastEvaluationDate = LastEvaluationDate?.ToShortDateString()
            };
        }
    }
}