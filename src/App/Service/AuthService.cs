using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.App.Provider;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.Entities.Shared;


namespace oksei_fsot_api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IOrganizationRepository organizationRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var oldUser = await _userRepository.GetByTokenAsync(refreshToken);
            if (oldUser == null)
                return new NotFoundResult();

            var userCredentials = await GetUserCredentials(oldUser.Id, oldUser.RoleName, oldUser.OrganizationId);
            return new OkObjectResult(userCredentials);
        }

        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var oldUser = await _userRepository.GetAsync(body.Login);
            if (oldUser == null)
                return new NotFoundResult();

            var inputPasswordHash = Hmac512Provider.Compute(body.Password);
            if (oldUser.Password != inputPasswordHash)
                return new BadRequestResult();

            var userCredentials = await GetUserCredentials(oldUser.Id, oldUser.RoleName, oldUser.OrganizationId);
            return new OkObjectResult(userCredentials);
        }



        public async Task<IActionResult> SignUp(SignUpBody body, Guid organizationId)
        {
            var organization = await _organizationRepository.GetAsync(organizationId);
            if (organization == null)
                return new BadRequestResult();

            var oldUser = await _userRepository.AddAsync(body, organization);
            if (oldUser == null)
                return new ConflictResult();

            var roleName = Enum.GetName(typeof(UserRole), body.Role);
            var userCredentials = await GetUserCredentials(oldUser.Id, roleName, oldUser.OrganizationId);
            return new OkObjectResult(userCredentials);
        }

        private async Task<UserCredentials?> GetUserCredentials(Guid userId, string role, Guid organizationId)
        {
            var tokenInfo = new TokenInfo
            {
                OrganizationId = organizationId,
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