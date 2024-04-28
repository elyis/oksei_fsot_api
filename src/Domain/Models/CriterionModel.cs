using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class CriterionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }

        public List<CriterionEvaluationOption> EvaluationOptions { get; set; } = new();
        public List<MarkModel> Marks { get; set; } = new();

        public CriterionBody ToCriterionBody()
        {
            return new CriterionBody
            {
                Id = Id,
                Description = Description,
                Name = Name,
                SerialNumber = SerialNumber,
                EvaluationOptions = EvaluationOptions.OrderByDescending(e => e.CountPoints).Select(e => e.ToEvaluationOptionResult()).ToList(),
            };
        }
    }
}