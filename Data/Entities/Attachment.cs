﻿using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Attachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public Guid IssueId { get; set; }

        public virtual Issue Issue { get; set; } = null!;
    }
}
