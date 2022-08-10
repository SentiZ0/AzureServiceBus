using Microsoft.EntityFrameworkCore;

namespace BusServiceReceiver.Models
{
    public class BusDBContext : DbContext
    {
        public BusDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
