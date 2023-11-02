using oksei_fsot_api.src.Domain.Entities.Request;
using Microsoft.AspNetCore.Mvc;
using oksei_fsot_api.src.Domain.Models;
namespace oksei_fsot_api.src.App.IService
{
    public interface IAuthService
    {
        Task<IActionResult> SignUp(SignUpBody body, Guid organizationId);
        Task<IActionResult> SignIn(SignInBody body);
        Task<IActionResult> RestoreToken(string refreshToken);
    }
}