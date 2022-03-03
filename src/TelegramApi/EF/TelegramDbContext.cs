using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.EF
{
    public class TelegramDbContext : DbContext
    {
        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }

        public DbSet<UserState> UserStates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserState>().Property(p => p.Propositions)
                .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v));
        }
    }
}
