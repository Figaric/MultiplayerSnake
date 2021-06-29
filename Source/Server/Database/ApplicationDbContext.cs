using Microsoft.EntityFrameworkCore;
using MultiplayerSnake.Server.Entities;

namespace MultiplayerSnake.Server
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }
}
