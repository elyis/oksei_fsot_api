using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class MarkBody
    {
        public Guid CriterionId { get; set; }
        public string AppraiserName { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }
        public Guid EvaluationId { get; set; }
    }
}