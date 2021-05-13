using EducationalConsultAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalConsultAPI.DBContext {
    public class EducationalDbContext : DbContext{
        public EducationalDbContext() {
        }

        public EducationalDbContext(DbContextOptions<EducationalDbContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.GroupId, ug.UserId });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<DailyReport> DailyReports { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<InvitedUser> InvitedUsers { get; set; }
    }
}
