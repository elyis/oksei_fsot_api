using System.ComponentModel.DataAnnotations;
using oksei_fsot_api.src.Domain.Enums;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class ProfileBody
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public string? UrlIcon { get; set; }
        public string? LastEvaluationDate { get; set; }
    }
}