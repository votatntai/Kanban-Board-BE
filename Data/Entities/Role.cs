using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
