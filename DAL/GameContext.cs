using ChairsBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChairsBackend.DAL
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> opt) : base(opt)
        {
        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
