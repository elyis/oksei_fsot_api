using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Models
{
    public class MarkModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid EvaluationOptionId { get; set; }
        public CriterionEvaluationOption EvaluationOption { get; set; }
        public Guid EvaluatedAppraiserId { get; set; }
        public EvaluatedAppraiserModel EvaluatedAppraiser { get; set; }
        public MarkLogModel MarkLogs { get; set; }

        public MarkBody ToMarkBody()
        {
            return new MarkBody
            {
                Id = Id,
                Date = CreatedAt.ToShortDateString(),
                EvaluationId = EvaluationOptionId,
                AppraiserName = EvaluatedAppraiser.Appraiser.Fullname
            };
        }
    }
}