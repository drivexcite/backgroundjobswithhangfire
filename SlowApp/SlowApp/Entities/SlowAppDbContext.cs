using Microsoft.EntityFrameworkCore;
using SlowApp.Entities.Configuration;

namespace SlowApp.Entities
{
    public class SlowAppDbContext : DbContext
    {
        public SlowAppDbContext() : base() { }
        public SlowAppDbContext(DbContextOptions<SlowAppDbContext> options): base(options) { }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
        }
    }
}
