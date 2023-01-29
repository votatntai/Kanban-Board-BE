using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class ProjectPriority
    {
        public Guid ProjectId { get; set; }
        public Guid PriorityId { get; set; }
        public string? Description { get; set; }

        public virtual Priority Priority { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}
