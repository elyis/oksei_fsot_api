using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class CreateMarkBody
    {
        [Required]
        public Guid EvaluationId { get; set; }
        [Required]
        public string EvaluationLogin { get; set; }
    }
}