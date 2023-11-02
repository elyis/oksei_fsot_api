using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace oksei_fsot_api.src.Domain.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class OrganizationModel
    {
        public Guid Id { get; set; }
        [StringLength(256, MinimumLength = 3)]
        public string Email { get; set; }
        public string Name { get; set; }

        public List<UserModel> Staff { get; set; } = new();
    }
}