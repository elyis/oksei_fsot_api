using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class UserCredentialsWithLogin
    {
        public LoginBody LoginBody { get; set; }
        public UserCredentials UserCredentials { get; set; }
    }
}