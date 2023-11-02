namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class CriterionBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LowerBound { get; set; } = 1;
        public int UpperBound { get; set; } = 5;
    }
}