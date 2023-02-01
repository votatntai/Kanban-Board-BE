namespace Data.Models.Views
{
    public class LinkViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid IssueId { get; set; }
    }
}
