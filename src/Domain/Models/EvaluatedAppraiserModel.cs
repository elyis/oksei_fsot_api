namespace oksei_fsot_api.src.Domain.Models
{
    public class EvaluatedAppraiserModel
    {
        public Guid EvaluatedId { get; set; }
        public UserModel Evaluated { get; set; }

        public Guid AppraiserId { get; set; }
        public UserModel Appraiser { get; set; }

        public List<MarkModel> Marks { get; set; }
    }
}