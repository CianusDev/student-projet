using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<DeliverableType> DeliverableTypes { get; set; }
        public DbSet<ProjectDeliverable> ProjectDeliverables { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<DeliverableEvaluation> DeliverableEvaluations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations et contraintes

            // User - Projects (One-to-Many)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Teacher)
                .WithMany(u => u.TeacherProjects)
                .HasForeignKey(p => p.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Groups (One-to-Many)
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Project)
                .WithMany(p => p.Groups)
                .HasForeignKey(g => g.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Group - GroupMembers (One-to-Many)
            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Student)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index unique pour éviter les doublons GroupId + StudentId
            modelBuilder.Entity<GroupMember>()
                .HasIndex(gm => new { gm.GroupId, gm.StudentId })
                .IsUnique();

            // ProjectAssignment relations
            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(pa => pa.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Student)
                .WithMany(u => u.IndividualAssignments)
                .HasForeignKey(pa => pa.StudentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Group)
                .WithMany(g => g.Assignments)
                .HasForeignKey(pa => pa.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            // ProjectDeliverable relations
            modelBuilder.Entity<ProjectDeliverable>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.Deliverables)
                .HasForeignKey(pd => pd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectDeliverable>()
                .HasOne(pd => pd.DeliverableType)
                .WithMany(dt => dt.ProjectDeliverables)
                .HasForeignKey(pd => pd.DeliverableTypeId);

            // Submission relations
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Assignment)
                .WithMany(pa => pa.Submissions)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Deliverable)
                .WithMany(pd => pd.Submissions)
                .HasForeignKey(s => s.DeliverableId);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.SubmittedByStudent)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.SubmittedByStudentId);

            // Evaluation relations
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Assignment)
                .WithMany(pa => pa.Evaluations)
                .HasForeignKey(e => e.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Teacher)
                .WithMany(u => u.Evaluations)
                .HasForeignKey(e => e.TeacherId);

            // DeliverableEvaluation relations
            modelBuilder.Entity<DeliverableEvaluation>()
                .HasOne(de => de.Evaluation)
                .WithMany(e => e.DeliverableEvaluations)
                .HasForeignKey(de => de.EvaluationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliverableEvaluation>()
                .HasOne(de => de.Submission)
                .WithMany(s => s.Evaluations)
                .HasForeignKey(de => de.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index unique pour Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index unique pour DeliverableType Name
            modelBuilder.Entity<DeliverableType>()
                .HasIndex(dt => dt.Name)
                .IsUnique();

            // Seed data - Types de livrables
            modelBuilder.Entity<DeliverableType>().HasData(
                new DeliverableType { Id = 1, Name = "Rapport", AllowedExtensions = ".pdf,.doc,.docx", MaxFileSize = 10 },
                new DeliverableType { Id = 2, Name = "Code Source", AllowedExtensions = ".zip,.rar,.tar.gz", MaxFileSize = 50 },
                new DeliverableType { Id = 3, Name = "Présentation", AllowedExtensions = ".ppt,.pptx,.pdf", MaxFileSize = 20 },
                new DeliverableType { Id = 4, Name = "Vidéo", AllowedExtensions = ".mp4,.avi,.mov", MaxFileSize = 100 },
                new DeliverableType { Id = 5, Name = "Image", AllowedExtensions = ".jpg,.jpeg,.png", MaxFileSize = 5 }
            );

            // Données de test
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Email = "prof@school.com", 
                    PasswordHash = "hashedpassword1", 
                    FirstName = "Jean", 
                    LastName = "Dupont", 
                    Role = "Teacher",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User 
                { 
                    Id = 2, 
                    Email = "student1@school.com", 
                    PasswordHash = "hashedpassword2", 
                    FirstName = "Marie", 
                    LastName = "Martin", 
                    Role = "Student",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User 
                { 
                    Id = 3, 
                    Email = "student2@school.com", 
                    PasswordHash = "hashedpassword3", 
                    FirstName = "Pierre", 
                    LastName = "Durand", 
                    Role = "Student",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                
                new User 
                { 
                    Id = 4, 
                    Email = "student3@school.com", 
                    PasswordHash = "hashedpassword4", 
                    FirstName = "Sophie", 
                    LastName = "Bernard", 
                    Role = "Student",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}