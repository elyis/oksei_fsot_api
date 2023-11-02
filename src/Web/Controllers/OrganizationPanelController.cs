using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Entities.Response;
using oksei_fsot_api.src.Domain.Enums;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Utility;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/organizationPanel")]
    public class OrganizationPanelController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly LoginGenerator _loginGenerator;
        private readonly PasswordGenerator _passwordGenerator;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public OrganizationPanelController(
            IAuthService authService,
            IJwtService jwtService,
            LoginGenerator loginGenerator,
            PasswordGenerator passwordGenerator,
            IOrganizationRepository organizationRepository
        )
        {
            _organizationRepository = organizationRepository;
            _passwordGenerator = passwordGenerator;
            _loginGenerator = loginGenerator;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [SwaggerOperation("")]

        public async Task<IActionResult> CreateOrganization(CreateOrganizationBody body)
        {
            if (string.IsNullOrWhiteSpace(body.Fullname))
                return BadRequest();

            var nameParts = body.Fullname.Split(" ");
            if (nameParts.Length == 0)
                return BadRequest();

            var firstName = nameParts.First();
            var login = _loginGenerator.Invoke(firstName);
            if (login == null)
                return BadRequest("Transliteration error");

            var organization = await _organizationRepository.AddAsync(body);
            if (organization == null)
                return Conflict();


            var signUpBody = new SignUpBody
            {
                Fullname = body.Fullname,
                Login = login,
                Password = _passwordGenerator.Invoke(),
                Role = UserRole.Organization
            };

            var createAccountResult = await _authService.SignUp(signUpBody, organization.Id);
            if (createAccountResult is OkObjectResult)
            {
                var loginBody = new LoginBody
                {
                    Login = login,
                    Password = signUpBody.Password
                };
                return Ok(loginBody);
            }

            return createAccountResult;
        }

        [HttpPost("signup/employee")]
        [SwaggerOperation("Зарегистрировать аккаунт сотрудника")]
        [SwaggerResponse(200, "Успешно", Type = typeof(LoginBody))]
        [SwaggerResponse(400, Description = "Неверный формат имени или в нем есть символы, кроме русских")]
        [SwaggerResponse(409, "Логин уже существует")]

        public async Task<IActionResult> SignUpEmployee(
            [FromHeader(Name = "Authorization")] string token,
            SignUpEmployeeBody body
        )
        {
            var tokenInfo = _jwtService.GetTokenInfo(token);

            if (string.IsNullOrWhiteSpace(body.Fullname))
                return BadRequest("Имя не может быть пустым");

            var nameParts = body.Fullname.Split(" ");
            if (nameParts.Length == 0)
                return BadRequest("Неверный формат имени");

            var firstName = nameParts.First();
            var login = _loginGenerator.Invoke(firstName);
            if (login == null)
                return BadRequest("Транслитератор говорит нет..., там есть что-то кроме русских букв");

            var password = _passwordGenerator.Invoke();
            var signUpBody = new SignUpBody
            {
                Fullname = body.Fullname,
                Login = login,
                Password = password,
                Role = body.Role
            };

            var result = await _authService.SignUp(signUpBody, tokenInfo.OrganizationId);
            if (result is OkObjectResult okObjectResult)
            {
                var userCredentialsWithLogin = new UserCredentialsWithLogin
                {
                    UserCredentials = (UserCredentials)okObjectResult.Value!,
                    LoginBody = {
                        Login = login,
                        Password = password
                    },
                };

                return Ok(userCredentialsWithLogin);
            }

            return BadRequest("Ошибка при регистрации");
        }
    }
}