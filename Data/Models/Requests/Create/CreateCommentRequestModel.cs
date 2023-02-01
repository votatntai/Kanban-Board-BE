namespace Data.Models.Requests.Create
{
    public class CreateCommentRequestModel
    {
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;
    }
}
