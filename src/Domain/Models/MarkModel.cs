using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Models
{
    public class MarkModel
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public Guid EvaluationId { get; set; }
        public CriterionEvaluationOption Evaluation { get; set; }
        public EvaluatedAppraiserModel EvaluatedAppraiser { get; set; }
        public MarkLogModel MarkLogs { get; set; }

        public MarkBody ToMarkBody()
        {
            return new MarkBody
            {
                Id = Id,
                Date = Date,
                EvaluationId = EvaluationId,
                AppraiserName = EvaluatedAppraiser.Appraiser.Fullname
            };
        }
    }
}