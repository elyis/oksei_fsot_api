using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class CreateOrganizationBody
    {
        [Required]
        public string Name { get; set; }

        [StringLength(256, MinimumLength = 3)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }
    }
}