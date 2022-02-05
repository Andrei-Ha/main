namespace Exadel.OfficeBooking.EF
{
    using Exadel.OfficeBooking.Domain.Bookings;
    using Exadel.OfficeBooking.Domain.Notifications;
    using Exadel.OfficeBooking.Domain.OfficePlan;
    using Exadel.OfficeBooking.Domain.Person;
    using Exadel.OfficeBooking.Domain.Reports;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Allows the application to interact with the database through the EntityFramework.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        public DbSet<Booking> Bookings { get; set;} = null!;
        public DbSet<BookingNotification> Notifications { get; set; } = null!;
        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<Workplace> Workplaces { get; set; } = null!;
        public DbSet<ParkingPlace> ParkingPlaces { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vacation> Vacations { get; set; } = null!;
        public DbSet<DailyReport> DailyReports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookingNotification>()
                .Property(o => o.EmailAdress)
                .HasMaxLength(100);

            modelBuilder.Entity<BookingNotification>()
                .Property(o => o.EmailSubject)
                .HasMaxLength(100);

            modelBuilder.Entity<BookingNotification>()
                .Property(o => o.MessageBody)
                .HasMaxLength(500);

            modelBuilder.Entity<Office>()
                .Property(o => o.Country)
                .HasMaxLength(100);

            modelBuilder.Entity<Office>()
                .Property(o => o.City)
                .HasMaxLength(100);

            modelBuilder.Entity<Office>()
                .Property(o => o.Adress)
                .HasMaxLength(150);

            modelBuilder.Entity<Office>()
                .Property(o => o.Name)
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .Property(o => o.FirstName)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(o => o.LastName)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(o => o.Email)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(o => o.Role)
                .HasDefaultValue(UserRole.CommonUser);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    TelegramId = 123465,
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Email = "iivanov@gmail.com",
                    EmploymentStart = DateTime.Now,
                    //Role = UserRole.CommonUser,
                });
        }
    }
}