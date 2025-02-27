using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Word> Words { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<ConnectionWord> ConnectionWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure uniqueness of the word value in DB
        modelBuilder.Entity<Word>()
            .HasIndex(w => w.Value)
            .IsUnique();

        // Ensure uniqueness of the connection in DB
        modelBuilder.Entity<Connection>()
            .HasIndex(c => c.ConnectionType)
            .IsUnique();

        // Many-to-many relation configuration for Connection and Word
        modelBuilder.Entity<ConnectionWord>()
            .HasKey(cw => new { cw.ConnectionId, cw.WordId });

        modelBuilder.Entity<ConnectionWord>()
            .HasOne(cw => cw.Connection)
            .WithMany(c => c.ConnectionWords)
            .HasForeignKey(cw => cw.ConnectionId);

        modelBuilder.Entity<ConnectionWord>()
            .HasOne(cw => cw.Word)
            .WithMany(w => w.ConnectionWords)
            .HasForeignKey(cw => cw.WordId);
    }
}
