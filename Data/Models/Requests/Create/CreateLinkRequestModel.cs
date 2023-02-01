namespace Data.Models.Requests.Create
{
    public class CreateLinkRequestModel
    {
        public string Url { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid IssueId { get; set; }
    }
}
