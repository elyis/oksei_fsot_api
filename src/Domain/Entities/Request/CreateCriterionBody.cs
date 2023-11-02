using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class CreateCriterionBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int LowerBound { get; set; } = 1;

        [DefaultValue(5)]
        [Range(1, int.MaxValue)]
        public int UpperBound { get; set; } = 5;

        public int SerialNumber { get; set; }
    }
}