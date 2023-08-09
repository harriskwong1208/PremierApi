using Microsoft.EntityFrameworkCore;
using SoccerApi.Models;

namespace SoccerApi.Data
{
    public class SoccerDbContext : DbContext
    {
        public SoccerDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {

        }

        public  DbSet<Player>  Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
