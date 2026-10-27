using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Models;

namespace RCS_Beadando.Data
{
    public class PokerContext : DbContext
    {
        // The options (which database, connection string) are passed in from Program.cs
        public PokerContext(DbContextOptions<PokerContext> options) : base(options) { }

        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Hand> Hands => Set<Hand>();
        public DbSet<Player> Players => Set<Player>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Deleting a session deletes its hands too
            modelBuilder.Entity<Hand>()
                .HasOne(h => h.Session)
                .WithMany(s => s.Hands)
                .HasForeignKey(h => h.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many: a hand has several opponents, a player appears in many hands
            modelBuilder.Entity<Hand>()
                .HasMany(h => h.Opponents)
                .WithMany(p => p.Hands);
        }
    }
}
