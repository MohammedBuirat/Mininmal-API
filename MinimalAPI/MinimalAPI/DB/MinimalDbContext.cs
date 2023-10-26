using Microsoft.EntityFrameworkCore;
using MinimalAPI.Model;

namespace MinimalAPI.DB
{
    public class MinimalDbContext : DbContext
    {
        public MinimalDbContext(DbContextOptions<MinimalDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}