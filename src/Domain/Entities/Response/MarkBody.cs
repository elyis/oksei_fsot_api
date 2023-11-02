namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class MarkBody
    {
        public Guid Id { get; set; }
        public string AppraiserName { get; set; }
        public DateOnly Date { get; set; }
        public int Mark { get; set; }
    }
}