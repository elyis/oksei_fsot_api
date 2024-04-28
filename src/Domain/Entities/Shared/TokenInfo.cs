using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Shared
{
    public class TokenInfo
    {
        public Guid UserId { get; set; }
        public UserRole Role { get; set; }
    }
}