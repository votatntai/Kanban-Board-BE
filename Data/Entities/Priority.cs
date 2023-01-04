﻿using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Priority
    {
        public Priority()
        {
            Issues = new HashSet<Issue>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDefault { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }
    }
}
