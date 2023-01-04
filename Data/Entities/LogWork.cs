using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class LogWork
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public Guid UserId { get; set; }
        public int SpentTime { get; set; }
        public int RemainingTime { get; set; }

        public virtual Issue Issue { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
