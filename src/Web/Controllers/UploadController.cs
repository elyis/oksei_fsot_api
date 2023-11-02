using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace oksei_fsot_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UploadController(
            IUserRepository userRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("profileIcon"), Authorize]
        [SwaggerOperation("Загрузить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(string))]

        public async Task<IActionResult> UploadProfileIcon(
            [FromHeader(Name = "Authorization")] string token
        )
        {
            if (!Request.Form.Files.Any())
                return BadRequest();

            var file = Request.Form.Files[0];
            var tokenInfo = _jwtService.GetTokenInfo(token);

            var filename = await FileUploader.UploadImageAsync(Constants.localPathToProfileIcons, file.OpenReadStream());
            await _userRepository.UpdateProfileIconAsync(tokenInfo.UserId, filename);
            return Ok(new { filename });
        }

        [HttpGet("profileIcon/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProfileIcon(string filename)
        {
            var bytes = await FileUploader.GetStreamImageAsync(Constants.localPathToProfileIcons, filename);
            if (bytes == null)
                return NotFound();

            return File(bytes, $"image/jpeg", filename);
        }


    }
}