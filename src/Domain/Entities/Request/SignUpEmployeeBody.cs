using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class SignUpEmployeeBody
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }
    }
}