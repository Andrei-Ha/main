using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.TelegramApi.EF
{
    public class TelegramDbContext : DbContext
    {
        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options)
        {
        }

        public DbSet<UserState> UsersStates { get; set; } = null!;
    }
}
