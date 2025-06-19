using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.Models;

namespace StudentProjectAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<DeliverableEvaluation> DeliverableEvaluations { get; set; }
        public DbSet<ProjectDeliverable> ProjectDeliverables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des propriétés de l'utilisateur
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Specialite).IsRequired();
                entity.Property(u => u.NiveauEtude).IsRequired();
                entity.Property(u => u.Departement).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();
            });

            // Configuration des relations
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Teacher)
                .WithMany(u => u.TeacherProjects)
                .HasForeignKey(p => p.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Project)
                .WithMany(p => p.Groups)
                .HasForeignKey(g => g.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Group)
                .WithMany(g => g.Assignments)
                .HasForeignKey(pa => pa.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProjectAssignment>()
                .HasOne(pa => pa.Student)
                .WithMany(u => u.IndividualAssignments)
                .HasForeignKey(pa => pa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration des rôles par défaut avec des valeurs statiques
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole 
                { 
                    Id = "1", 
                    Name = "Teacher", 
                    NormalizedName = "TEACHER", 
                    ConcurrencyStamp = "64d8b44a-cdbb-40c7-99c4-8a6e576d0db2"
                },
                new IdentityRole 
                { 
                    Id = "2", 
                    Name = "Student", 
                    NormalizedName = "STUDENT", 
                    ConcurrencyStamp = "7a7c67c5-24b0-4334-a55b-0ac32a70512a"
                }
            );

            // Données de test pour les utilisateurs avec des valeurs statiques
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "prof@school.com",
                    Email = "prof@school.com",
                    EmailConfirmed = true,
                    FirstName = "Jean",
                    LastName = "Dupont",
                    Specialite = "Informatique",
                    NiveauEtude = "Master",
                    Departement = "Informatique",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEPAr1rWjJPEyI0IP1uT7o7np6PWVz1bB4iVLaggbi6QNfeGGJBhQHYz3H2nprmMBWA==",
                    SecurityStamp = "27baea37-f970-4bb5-a413-d3e5e5078e13",
                    ConcurrencyStamp = "fef42b2f-1418-45c9-83c7-89356a7e3e32"
                },
                new User
                {
                    Id = "2",
                    UserName = "student1@school.com",
                    Email = "student1@school.com",
                    EmailConfirmed = true,
                    FirstName = "Marie",
                    LastName = "Martin",
                    Specialite = "Informatique",
                    NiveauEtude = "Licence",
                    Departement = "Informatique",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEPAr1rWjJPEyI0IP1uT7o7np6PWVz1bB4iVLaggbi6QNfeGGJBhQHYz3H2nprmMBWA==",
                    SecurityStamp = "27baea37-f970-4bb5-a413-d3e5e5078e13",
                    ConcurrencyStamp = "fef42b2f-1418-45c9-83c7-89356a7e3e32"
                }
            );

            // Attribution des rôles aux utilisateurs
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" },
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" }
            );
        }
    }
} 