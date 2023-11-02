using oksei_fsot_api.src.App.IService;
using oksei_fsot_api.src.Domain.Entities.Request;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using oksei_fsot_api.src.Utility;
using oksei_fsot_api.src.Domain.Entities.Response;
using webApiTemplate.src.App.IService;


namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly PasswordGenerator _passwordGenerator;
        private readonly LoginGenerator _loginGenerator;
        private readonly IJwtService _jwtService;


        public AuthController(
            IAuthService authService,
            IJwtService jwtService,
            PasswordGenerator passwordGenerator,
            LoginGenerator loginGenerator
        )
        {
            _authService = authService;
            _jwtService = jwtService;
            _passwordGenerator = passwordGenerator;
            _loginGenerator = loginGenerator;
        }

        [HttpPost("signup")]
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
                return BadRequest();

            var nameParts = body.Fullname.Split(" ");
            if (nameParts.Length == 0)
                return BadRequest();

            var firstName = nameParts.First();
            var login = _loginGenerator.Invoke(firstName);
            if (login == null)
                return BadRequest("Translitaration error");

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

            return BadRequest();
        }



        [SwaggerOperation("Авторизация")]
        [SwaggerResponse(200, "Успешно", Type = typeof(UserCredentials))]
        [SwaggerResponse(400, "Пароли не совпадают")]
        [SwaggerResponse(404, "Login не зарегистрирован")]

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInBody signInBody)
        {
            var result = await _authService.SignIn(signInBody);
            return result;
        }

        [SwaggerOperation("Восстановление токена")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(UserCredentials))]
        [SwaggerResponse(404, "Токен не используется")]

        [HttpPost("token")]
        public async Task<IActionResult> RestoreTokenAsync(TokenBody body)
        {
            var result = await _authService.RestoreToken(body.Value);
            return result;
        }
    }
}