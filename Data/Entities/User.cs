using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            IssueAssignees = new HashSet<Issue>();
            IssueReporters = new HashSet<Issue>();
            LogWorks = new HashSet<LogWork>();
            ProjectDefaultAssignees = new HashSet<Project>();
            ProjectLeaders = new HashSet<Project>();
            Projects = new HashSet<Project>();
            Roles = new HashSet<Role>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Status { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Issue> IssueAssignees { get; set; }
        public virtual ICollection<Issue> IssueReporters { get; set; }
        public virtual ICollection<LogWork> LogWorks { get; set; }
        public virtual ICollection<Project> ProjectDefaultAssignees { get; set; }
        public virtual ICollection<Project> ProjectLeaders { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
