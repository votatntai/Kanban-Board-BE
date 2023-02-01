namespace Data.Models.Views
{
    public class LogWorkViewModel
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public UserViewModel User { get; set; } = null!;
        public int SpentTime { get; set; }
        public string? Description { get; set; }
        public int RemainingTime { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
