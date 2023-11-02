using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class UserBody
    {
        public string Fullname { get; set; }

        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }
    }
}