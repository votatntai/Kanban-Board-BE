namespace Data.Models.Views
{
    public class AttachmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public byte[] File { get; set; } = null!;
        public Guid IssueId { get; set; }
    }
}
