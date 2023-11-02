using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class SignUpBody
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Password { get; set; }

        [EnumDataType(typeof(UserRole), ErrorMessage = "Значение enum вне диапазона")]
        public UserRole Role { get; set; } = UserRole.Teacher;
    }
}