namespace Data.Models.Views
{
    public class IssueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public UserViewModel? Assignee { get; set; }
        public int? EstimateTime { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? TypeId { get; set; } 
        public int Position { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public UserViewModel? Reporter { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsClose { get; set; }
    }
}
