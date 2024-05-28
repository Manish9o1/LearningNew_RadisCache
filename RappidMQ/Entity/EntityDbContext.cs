using Microsoft.EntityFrameworkCore;
using RappidMQ.Models;

namespace RappidMQ.Entity
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
