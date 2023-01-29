using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Label
    {
        public Label()
        {
            IssueLabels = new HashSet<IssueLabel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<IssueLabel> IssueLabels { get; set; }
    }
}
