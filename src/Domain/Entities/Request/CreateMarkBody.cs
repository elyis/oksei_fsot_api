using System.ComponentModel.DataAnnotations;

namespace oksei_fsot_api.src.Domain.Entities.Request
{
    public class CreateMarkBody
    {
        public int Mark { get; set; }
        public Guid EvaluatedId { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    }
}