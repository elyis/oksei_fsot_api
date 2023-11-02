using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class SignInBody
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}