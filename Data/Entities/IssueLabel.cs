using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class IssueLabel
    {
        public Guid IssueId { get; set; }
        public Guid LabelId { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Issue Issue { get; set; } = null!;
        public virtual Label Label { get; set; } = null!;
    }
}
