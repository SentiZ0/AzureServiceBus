using Microsoft.EntityFrameworkCore;

namespace AzureServiceBus.Models
{
    public class BusDBContext : DbContext
    {
        public BusDBContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
