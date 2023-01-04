using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid LeaderId { get; set; }
        public Guid? DefaultAssigneeId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsClose { get; set; }

        public virtual User? DefaultAssignee { get; set; }
        public virtual User Leader { get; set; } = null!;
    }
}
