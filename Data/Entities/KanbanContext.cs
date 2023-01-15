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
        public virtual DbSet<ChildIssue> ChildIssues { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<Link> Links { get; set; } = null!;
        public virtual DbSet<LogWork> LogWorks { get; set; } = null!;
        public virtual DbSet<Priority> Priorities { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

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
                    .HasConstraintName("FK__Attachmen__Issue__66603565");
            });

            modelBuilder.Entity<ChildIssue>(entity =>
            {
                entity.HasKey(e => new { e.IssueId, e.ChildId })
                    .HasName("PK__ChildIss__1769B67500098233");

                entity.ToTable("ChildIssue");

                entity.HasIndex(e => e.ChildId, "UQ__ChildIss__BEFA071711F4074C")
                    .IsUnique();

                entity.HasOne(d => d.Child)
                    .WithOne(p => p.ChildIssueChild)
                    .HasForeignKey<ChildIssue>(d => d.ChildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChildIssu__Child__6383C8BA");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.ChildIssueIssues)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChildIssu__Issue__628FA481");
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
                    .HasConstraintName("FK__Comment__IssueId__6FE99F9F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__UserId__70DDC3D8");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("Issue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ResolveAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Assignee)
                    .WithMany(p => p.IssueAssignees)
                    .HasForeignKey(d => d.AssigneeId)
                    .HasConstraintName("FK__Issue__AssigneeI__571DF1D5");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__PriorityI__59063A47");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__ProjectId__5BE2A6F2");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.IssueReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__ReporterI__5CD6CB2B");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__StatusId__59FA5E80");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Issue__TypeId__5AEE82B9");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.IssueId)
                    .HasConstraintName("FK__Label__IssueId__6D0D32F4");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Label__ProjectId__6C190EBB");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Link__IssueId__693CA210");
            });

            modelBuilder.Entity<LogWork>(entity =>
            {
                entity.ToTable("LogWork");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__IssueId__74AE54BC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__UserId__75A278F5");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.ToTable("Priority");

                entity.HasIndex(e => e.Value, "UQ__Priority__07D9BBC229B8A6CB")
                    .IsUnique();

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

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Projects)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProjectMember",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__ProjectMe__UserI__47DBAE45"),
                        r => r.HasOne<Project>().WithMany().HasForeignKey("ProjectId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__ProjectMe__Proje__46E78A0C"),
                        j =>
                        {
                            j.HasKey("ProjectId", "UserId").HasName("PK__ProjectM__A76232346138532A");

                            j.ToTable("ProjectMember");
                        });
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

                entity.HasIndex(e => e.Position, "UQ__Status__5A8B58B865D811E6")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Statuses)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Status__ProjectI__4E88ABD4");
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

                entity.HasIndex(e => e.Username, "UQ__User__536C85E4EF64FE56")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534687FCE34")
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
                            j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760ADE67B7ECD");

                            j.ToTable("UserRole");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
