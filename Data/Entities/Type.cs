using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Type
    {
        public Type()
        {
            Issues = new HashSet<Issue>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public string? Description { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
