using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Project
    {
        public Project()
        {
            Issues = new HashSet<Issue>();
            Labels = new HashSet<Label>();
            ProjectMembers = new HashSet<ProjectMember>();
            ProjectPriorities = new HashSet<ProjectPriority>();
            Statuses = new HashSet<Status>();
            Types = new HashSet<Type>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid LeaderId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? LastActivity { get; set; }
        public bool IsClose { get; set; }

        public virtual User? DefaultAssignee { get; set; }
        public virtual User Leader { get; set; } = null!;
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<ProjectPriority> ProjectPriorities { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
        public virtual ICollection<Type> Types { get; set; }
    }
}
