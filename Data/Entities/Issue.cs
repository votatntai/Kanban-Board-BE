using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Issue
    {
        public Issue()
        {
            Attachments = new HashSet<Attachment>();
            ChildIssueIssues = new HashSet<ChildIssue>();
            Comments = new HashSet<Comment>();
            Labels = new HashSet<Label>();
            Links = new HashSet<Link>();
            LogWorks = new HashSet<LogWork>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? AssigneeId { get; set; }
        public int? EstimateTime { get; set; }
        public Guid PriorityId { get; set; }
        public Guid StatusId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public int Position { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ReporterId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? ResolveAt { get; set; }
        public bool IsClose { get; set; }

        public virtual User? Assignee { get; set; }
        public virtual Priority Priority { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
        public virtual User Reporter { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual Type Type { get; set; }
        public virtual ChildIssue? ChildIssueChild { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<ChildIssue> ChildIssueIssues { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<LogWork> LogWorks { get; set; }
    }
}
