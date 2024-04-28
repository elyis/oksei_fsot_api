using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class CreateCriterionBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public List<EvaluationOption> EvaluationOptions { get; set; } = new();
    }

    public class EvaluationOption
    {
        public string Description { get; set; }
        public int CountPoints { get; set; } = 0;
    }

    public class EvaluationOptionResult
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int CountPoints { get; set; } = 0;
    }
}