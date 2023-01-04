using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual Issue Issue { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
