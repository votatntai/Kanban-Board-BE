using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Link
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        public Guid IssueId { get; set; }

        public virtual Issue Issue { get; set; } = null!;
    }
}
