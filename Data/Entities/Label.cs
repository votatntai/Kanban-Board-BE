using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Label
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid? IssueId { get; set; }

        public virtual Issue? Issue { get; set; }
        public virtual Project Project { get; set; } = null!;
    }
}
