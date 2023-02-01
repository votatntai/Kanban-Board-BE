namespace Data.Models.Views
{
    public class IssueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsChild { get; set; }
        public UserViewModel? Assignee { get; set; }
        public int? EstimateTime { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? StatusId { get; set; }
        public TypeViewModel Type { get; set; } = null!;
        public int Position { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public ICollection<ChildIssueViewModel> ChildIssues { get; set; } = null!;
        public UserViewModel? Reporter { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? ResolveAt { get; set; }
        public ICollection<LabelViewModel> Labels { get; set; } = null!;
        public ICollection<CommentViewModel> Comments { get; set; } = null!;
        public ICollection<LinkViewModel> Links { get; set; } = null!;
        public ICollection<LogWorkViewModel> LogWorks { get; set; } = null!;
        public bool IsClose { get; set; }
    }
}
