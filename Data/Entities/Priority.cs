using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Priority
    {
        public Priority()
        {
            Issues = new HashSet<Issue>();
            ProjectPriorities = new HashSet<ProjectPriority>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Value { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<ProjectPriority> ProjectPriorities { get; set; }
    }
}
