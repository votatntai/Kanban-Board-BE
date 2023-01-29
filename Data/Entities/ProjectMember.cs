using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinAt { get; set; }
        public bool IsOwner { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
