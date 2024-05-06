namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class MarkLogBody
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public string AppraiserFullname { get; set; }
        public string TeacherName { get; set; }
        public Guid EvaluationId { get; set; }
    }
}