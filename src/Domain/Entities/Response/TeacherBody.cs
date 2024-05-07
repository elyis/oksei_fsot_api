namespace oksei_fsot_api.src.Domain.Entities.Response
{
    public class TeacherBody
    {
        public string Fullname { get; set; }
        public string Login { get; set; }
        public int SumMarks { get; set; }
        public string? LastChange { get; set; }
        public bool IsKing { get; set; }
    }
}