using Microsoft.EntityFrameworkCore;
using Virtue.UserService.Models;

namespace Virtue.UserService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Address as an independent entity with a primary key
            modelBuilder.Entity<Address>().HasKey(a => a.Id);  
        }
    }
}
