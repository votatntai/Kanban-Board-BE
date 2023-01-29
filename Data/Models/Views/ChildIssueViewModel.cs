namespace Data.Models.Views
{
    public class ChildIssueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsChild { get; set; }
        public UserViewModel? Assignee { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? TypeId { get; set; }
        public int Position { get; set; }
        public Guid ProjectId { get; set; }
        public UserViewModel? Reporter { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? ResolveAt { get; set; }
        public ICollection<LabelViewModel> Labels { get; set; } = null!;
        public bool IsClose { get; set; }
    }
}
