namespace Data.Models.Views
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public UserViewModel User { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
