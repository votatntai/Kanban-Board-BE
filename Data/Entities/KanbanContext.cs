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
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
                    .HasConstraintName("FK__Attachmen__Issue__5DCAEF64");
            });

            modelBuilder.Entity<ChildIssue>(entity =>
            {
                entity.HasKey(e => new { e.IssueId, e.ChildId })
                    .HasName("PK__ChildIss__1769B675DC893449");

                entity.ToTable("ChildIssue");

                entity.HasIndex(e => e.ChildId, "UQ__ChildIss__BEFA0717A5B251DC")
                    .IsUnique();

                entity.HasOne(d => d.Child)
                    .WithOne(p => p.ChildIssueChild)
                    .HasForeignKey<ChildIssue>(d => d.ChildId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChildIssu__Child__5AEE82B9");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.ChildIssueIssues)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChildIssu__Issue__59FA5E80");
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
                    .HasConstraintName("FK__Comment__IssueId__66603565");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__UserId__6754599E");
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

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Assignee)
                    .WithMany(p => p.IssueAssignees)
                    .HasForeignKey(d => d.AssigneeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__AssigneeI__4F7CD00D");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.PriorityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__PriorityI__5165187F");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.IssueReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__ReporterI__5441852A");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__StatusId__52593CB8");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Issue__TypeId__534D60F1");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Label__IssueId__6383C8BA");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Link__IssueId__60A75C0F");
            });

            modelBuilder.Entity<LogWork>(entity =>
            {
                entity.ToTable("LogWork");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__IssueId__6B24EA82");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogWorks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LogWork__UserId__6C190EBB");
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
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "UQ__User__536C85E484AA43BC")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534074CD6FC")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.Username).HasMaxLength(256);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__UserRole__AF2760AD412E0391");

                entity.ToTable("UserRole");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRole__RoleId__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserRole__UserId__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
