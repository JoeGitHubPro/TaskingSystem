using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskingSystem.Models;

namespace TaskingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AssignedTask> AssignedTasks { get; set; }
        public DbSet<AssignmentHeadLine> AssignmentHeadLines { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentsCourses> StudentsCourses { get; set; }
        public DbSet<Theme> Themes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            builder.Entity<AssignedTask>()
                .Property(a => a.AssignedTaskId)
                .ValueGeneratedOnAdd()
            .UseIdentityColumn(1, 1);


            builder.Entity<AssignedTask>()
          .HasOne(a => a.AssignmentHeadLine)
          .WithMany()
          .HasForeignKey(a => a.TaskId)
          .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AssignedTask>()
                .HasOne(a => a.AssignedTaskStudent)
                .WithMany()
                .HasForeignKey(a => a.AssignedTaskStudentId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<AssignmentHeadLine>()
                .HasKey(a => a.AssignmentId);


            builder.Entity<Course>()
                .HasKey(a => a.CourseCode);

            builder.Entity<Course>()
          .HasOne(c => c.Professor)
          .WithMany(u => u.Courses)
          .HasForeignKey(c => c.ProfessorId)
          .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AssignmentHeadLine>()
                .HasOne(a => a.Course)
                .WithMany(c => c.AssignmentHeadLines)
                .HasForeignKey(a => a.CourseCode)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AssignmentHeadLine>()
                .HasOne(a => a.Professor)
                .WithMany()
                .HasForeignKey(a => a.ProfessorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentsCourses>()
                .HasKey(a => new { a.StudentId, a.CourseCode });

            builder.Entity<StudentsCourses>()
                .HasOne(sc => sc.Student)
                .WithMany()
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentsCourses>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.CourseCode)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
