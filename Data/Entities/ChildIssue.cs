using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class ChildIssue
    {
        public Guid IssueId { get; set; }
        public Guid ChildId { get; set; }

        public virtual Issue Child { get; set; } = null!;
        public virtual Issue Issue { get; set; } = null!;
    }
}
