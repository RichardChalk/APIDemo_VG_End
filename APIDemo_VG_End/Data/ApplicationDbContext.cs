using DeleteMe.Models;
using Microsoft.EntityFrameworkCore;

namespace DeleteMe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }

}
