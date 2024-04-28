using oksei_fsot_api.src.Domain.Entities.Shared;

namespace webApiTemplate.src.App.IService
{
    public interface IJwtService
    {
        string GenerateAccessToken(Dictionary<string, string> claims, TimeSpan timeSpan);
        string GenerateRefreshToken() => Guid.NewGuid().ToString();
        TokenPair GenerateDefaultTokenPair(TokenInfo tokenInfo);
        TokenPair GenerateTokenPair(Dictionary<string, string> claims, TimeSpan timeSpan);
        TokenInfo GetTokenInfo(string token);
    }
}