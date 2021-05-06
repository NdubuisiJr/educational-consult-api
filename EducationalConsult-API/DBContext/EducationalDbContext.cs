using Microsoft.EntityFrameworkCore;

namespace EducationalConsultAPI.DBContext {
    public class EducationalDbContext : DbContext{
        public EducationalDbContext() {
        }

        public EducationalDbContext(DbContextOptions<EducationalDbContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //modelBuilder.Entity<ContactMessagesReceived>().HasKey(cm => new { cm.ContactId, cm.MessageId });

            //modelBuilder.Entity<User>().HasOne(a => a.Sender)
            //                           .WithOne(b => b.User)
            //                           .HasForeignKey<Sender>(b => b.UserId);

            base.OnModelCreating(modelBuilder);
        }

        //public virtual DbSet<User> Users { get; set; }
    }
}
