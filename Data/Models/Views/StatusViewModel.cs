namespace Data.Models.Views
{
    public class StatusViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public int? Limit { get; set; }
        public int Position { get; set; }
        public ICollection<IssueViewModel> Issues { get; set; } = null!;
    }
}
