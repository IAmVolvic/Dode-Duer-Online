using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class JWTBlacklistContext : DbContext
{
    public JWTBlacklistContext(DbContextOptions<JWTBlacklistContext> options) : base(options)
    {}

    public DbSet<JWTBlacklist> JWTBlacklist { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JWTBlacklist>(entity =>
        {
            entity.ToTable("jwt_blacklist_pkey");
            entity.HasKey(e => e.Id).HasName("jwt_blacklist");
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Jti).HasMaxLength(255).HasColumnName("jti");
            entity.Property(e => e.BlacklistExpiry)
                .HasColumnName("blacklisted_expiry_time")
                .IsRequired();
        });
    }
}