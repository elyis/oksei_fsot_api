using oksei_fsot_api.src.Domain.Entities.Request;

namespace oksei_fsot_api.src.Domain.Models
{
    public class CriterionEvaluationOption
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int CountPoints { get; set; }

        public CriterionModel Criterion { get; set; }
        public Guid CriterionId { get; set; }

        public EvaluationOptionResult ToEvaluationOptionResult()
        {
            return new EvaluationOptionResult
            {
                Id = Id,
                CountPoints = CountPoints,
                Description = Description
            };
        }
    }
}