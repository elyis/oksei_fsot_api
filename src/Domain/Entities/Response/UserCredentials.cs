using oksei_fsot_api.src.Domain.Entities.Shared;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class UserCredentials
    {
        public TokenPair TokenPair { get; set; }
        public UserRole Role { get; set; }
    }
}