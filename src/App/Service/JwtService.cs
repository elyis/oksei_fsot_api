using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using oksei_fsot_api.src.Domain.Entities.Shared;
using Microsoft.IdentityModel.Tokens;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.Domain.Entities.Config;
using oksei_fsot_api.src.Domain.Enums;

namespace webApiTemplate.src.App.Service
{
    public class JwtService : IJwtService
    {
        private readonly SigningCredentials _signingCredentials;

        public JwtService(JwtSettings jwtSettings)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateAccessToken(Dictionary<string, string> claims, TimeSpan timeSpan)
        {
            var tokenClaims = claims.Select(claim => new Claim(claim.Key, claim.Value));

            var token = new JwtSecurityToken(
                claims: tokenClaims,
                expires: DateTime.UtcNow.Add(timeSpan),
                signingCredentials: _signingCredentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken() => Guid.NewGuid().ToString();

        public TokenPair GenerateDefaultTokenPair(TokenInfo tokenInfo)
        {
            var claims = new Dictionary<string, string>{
                { nameof(TokenInfo.UserId), tokenInfo.UserId.ToString() },
                { ClaimTypes.Role, Enum.GetName(typeof(UserRole), tokenInfo.Role)!},
            };
            var timeSpan = new TimeSpan(2, 0, 0, 0);

            return GenerateTokenPair(claims, timeSpan);
        }

        public TokenPair GenerateTokenPair(Dictionary<string, string> claims, TimeSpan timeSpan) =>
            new TokenPair(
                    GenerateAccessToken(claims, timeSpan),
                    GenerateRefreshToken()
                );

        public TokenInfo GetTokenInfo(string token)
        {
            var claims = new JwtSecurityTokenHandler()
                .ReadJwtToken(token.Replace("Bearer ", ""))
                .Claims
                .ToList();

            var tokenInfo = new TokenInfo
            {
                Role = Enum.Parse<UserRole>(claims.First(e => e.Type == ClaimTypes.Role).Value),
                UserId = Guid.Parse(claims.First(e => e.Type == nameof(TokenInfo.UserId)).Value)
            };
            return tokenInfo;
        }
    }
}