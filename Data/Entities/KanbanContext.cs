using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Entities
{
    public partial class KanbanContext : DbContext
    {
        public KanbanContext()
        {
        }

        public KanbanContext(DbContextOptions<KanbanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<IssueLabel> IssueLabels { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<Link> Links { get; set; } = null!;
        public virtual DbSet<LogWork> LogWorks { get; set; } = null!;
        public virtual DbSet<Priority> Priorities { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
        public virtual DbSet<ProjectPriority> ProjectPriorities { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=Janglee\\JANGLEE;Database=Kanban;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("Attachment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Attachmen__Issue__6477ECF3");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasMaxLength(256);

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__IssueId__70DDC3D8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__UserId__71D1E811");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("Issue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.EstimateTime).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ResolveAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Assignee)
                    .WithMany(p => p.IssueAssignees)
                    .HasForeignKey(d => d.AssigneeId)
                    .HasConstraintName("FK__Issue__AssigneeI__59FA5E80");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__Issue__ParentId__59063A47");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__PriorityI__5BE2A6F2");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__ProjectId__5EBF139D");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.IssueReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__ReporterI__5FB337D6");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__StatusId__5CD6CB2B");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__TypeId__5DCAEF64");
            });

            modelBuilder.Entity<IssueLabel>(entity =>
            {
                entity.HasKey(e => new { e.IssueId, e.LabelId })
                    .HasName("PK__IssueLab__4F11F4B8F18E1CEC");

                entity.ToTable("IssueLabel");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueLabels)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IssueLabe__Issue__6D0D32F4");

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.IssueLabels)
                    .HasForeignKey(d => d.LabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__IssueLabe__Label__6E01572D");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Label__ProjectId__6A30C649");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Link__IssueId__6754599E");
            });

            modelBuilder.Entity<LogWork>(entity =>
            {
                entity.ToTable("LogWork");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__IssueId__75A278F5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__UserId__76969D2E");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.ToTable("Priority");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastActivity).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.DefaultAssignee)
                    .WithMany(p => p.ProjectDefaultAssignees)
                    .HasForeignKey(d => d.DefaultAssigneeId)
                    .HasConstraintName("FK__Project__Default__4222D4EF");

                entity.HasOne(d => d.Leader)
                    .WithMany(p => p.ProjectLeaders)
                    .HasForeignKey(d => d.LeaderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Project__LeaderI__412EB0B6");
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.UserId })
                    .HasName("PK__ProjectM__A7623234CC569E5E");

                entity.ToTable("ProjectMember");

                entity.Property(e => e.JoinAt).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectMe__Proje__46E78A0C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectMe__UserI__47DBAE45");
            });

            modelBuilder.Entity<ProjectPriority>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.PriorityId })
                    .HasName("PK__ProjectP__8B1083FB99D7CFC3");

                entity.ToTable("ProjectPriority");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.ProjectPriorities)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectPr__Prior__5535A963");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectPriorities)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectPr__Proje__5441852A");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Statuses)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Status__ProjectI__4D94879B");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Types)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Type__ProjectId__4AB81AF0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "UQ__User__536C85E405C69F5C")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534A1736D38")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.Username).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__RoleId__3E52440B"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UserRole__UserId__3D5E1FD2"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760ADD8B315FD");

                            j.ToTable("UserRole");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
