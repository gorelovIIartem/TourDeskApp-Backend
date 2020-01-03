using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> QUsers { get; set; }
        public DbSet<Feedback> FeedBacks { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Feedback settings
            modelBuilder.Entity<Feedback>().HasKey(p => p.Id);
            modelBuilder.Entity<Feedback>().ToTable("Feedbacks");
            modelBuilder.Entity<Feedback>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<Feedback>().HasOne(p => p.Tour).WithMany(p => p.FeedBacks).HasForeignKey(p => p.TourId).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<FeedBack>().HasOne(p => p.User).WithMany(p => p.FeedBacks).HasForeignKey(p => p.UserId);
            #endregion

            #region User setting
            //modelBuilder.Entity<User>().HasKey(p => p.Id);

            #endregion

            #region Tour settings
            modelBuilder.Entity<Tour>().HasKey(p => p.Id);
            modelBuilder.Entity<Tour>().ToTable("Tours");
            //modelBuilder.Entity<Tour>().Property(p => p.Id).ValueGeneratedNever();
            #endregion

            #region Ticket settings
            modelBuilder.Entity<Ticket>().HasKey(p => new { p.UserId, p.TourId });
            modelBuilder.Entity<Ticket>().ToTable("Tickets");
            #endregion

            #region User setting
            modelBuilder.Entity<User>().HasKey(x => x.UserId);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(p => p.UserId).ValueGeneratedNever();
            modelBuilder.Entity<ApplicationUser>().HasOne(p => p.User).WithOne(p => p.ApplicationUser).HasForeignKey<User>(p => p.UserId);
            #endregion

            modelBuilder.Entity<UserProfile>().HasKey(p => p.UserId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
