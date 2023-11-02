using oksei_fsot_api.src.Domain.Entities.Response;

namespace oksei_fsot_api.src.Domain.Models
{
    public class MarkLogModel
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public Guid MarkId { get; set; }
        public MarkModel Mark { get; set; }

        public MarkLogBody ToMarkLogBody()
        {
            return new MarkLogBody
            {
                Id = Id,
                Date = Mark.Date,
                AppraiserFullname = Mark.EvaluatedAppraiser.Appraiser.Fullname,
                Mark = Mark.Value,
                TeacherName = Mark.EvaluatedAppraiser.Evaluated.Fullname
            };
        }
    }
}