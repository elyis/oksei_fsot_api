using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class UserOpenInfoBody
    {
        public string Fullname { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }
    }
}