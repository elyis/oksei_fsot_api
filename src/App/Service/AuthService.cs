using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using webApiTemplate.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Entities.Shared;


namespace oksei_fsot_api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var oldUser = await _userRepository.GetByTokenAsync(refreshToken);
            if (oldUser == null)
                return new NotFoundResult();

            var userCredentials = await GetUserCredentials(oldUser.Id, oldUser.RoleName);
            return new OkObjectResult(userCredentials);
        }

        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var oldUser = await _userRepository.GetAsync(body.Login);
            if (oldUser == null)
                return new NotFoundResult();

            var userCredentials = await GetUserCredentials(oldUser.Id, oldUser.RoleName);
            return new OkObjectResult(userCredentials);
        }



        public async Task<IActionResult> SignUp(SignUpBody body)
        {
            var oldUser = await _userRepository.AddAsync(body);
            if (oldUser == null)
                return new ConflictResult();

            var roleName = Enum.GetName(typeof(UserRole), body.Role);
            var userCredentials = await GetUserCredentials(oldUser.Id, roleName);
            return new OkObjectResult(userCredentials);
        }

        private async Task<UserCredentials?> GetUserCredentials(Guid userId, string role)
        {
            var tokenInfo = new TokenInfo
            {
                Role = Enum.Parse<UserRole>(role),
                UserId = userId
            };

            var tokenPair = _jwtService.GenerateDefaultTokenPair(tokenInfo);
            var actualRefreshToken = await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, userId);
            if (actualRefreshToken == null)
                return null;

            tokenPair.RefreshToken = actualRefreshToken;
            return new UserCredentials
            {
                Role = Enum.Parse<UserRole>(role),
                TokenPair = tokenPair
            };
        }
    }
}