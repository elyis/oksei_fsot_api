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
        public int LowerBound { get; set; } = 1;
        public int UpperBound { get; set; } = 5;
        public int SerialNumber { get; set; }

        public ICollection<MarkModel> Marks { get; set; }

        public CriterionBody ToCriterionBody()
        {
            return new CriterionBody
            {
                Id = Id,
                Description = Description,
                Name = Name,
                LowerBound = LowerBound,
                UpperBound = UpperBound,
            };
        }
    }
}