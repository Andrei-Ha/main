using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;
using Microsoft.EntityFrameworkCore;
using System;
using Exadel.OfficeBooking.EF.DbTestData;

namespace Exadel.OfficeBooking.EF
{
    /// <summary>
    /// Allows the application to interact with the database through the EntityFramework.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set;} = null!;
        public DbSet<Office> Offices { get; set; } = null!;
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<Workplace> Workplaces { get; set; } = null!;
        public DbSet<ParkingPlace> ParkingPlaces { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vacation> Vacations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingPlace>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.ParkingPlace)
                .HasForeignKey<Booking>(b => b.ParkingPlaceId);

            modelBuilder.Entity<Office>()
                .Property(o => o.Country)
                .HasMaxLength(100);

            modelBuilder.Entity<Office>()
                .Property(o => o.City)
                .HasMaxLength(100);

            modelBuilder.Entity<Office>()
                .Property(o => o.Address)
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

            modelBuilder.ApplyConfiguration(new OfficeTestData());
            modelBuilder.ApplyConfiguration(new UserTestData());
        }
    }
}
