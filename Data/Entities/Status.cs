using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Status
    {
        public Status()
        {
            Issues = new HashSet<Issue>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public int Position { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public int? Limit { get; set; }
        public string? Description { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
