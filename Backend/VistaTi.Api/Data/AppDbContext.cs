using Microsoft.EntityFrameworkCore;
using VistaTi.Api.Models;

namespace VistaTi.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.ExternalId })
            .IsUnique();
    }
}
