using oksei_fsot_api.src.Domain.Entities.Request;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class CriterionBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }

        public List<EvaluationOptionResult> EvaluationOptions { get; set; } = new List<EvaluationOptionResult>();
    }
}