using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class GameContext : DbContext
{
    public GameContext(DbContextOptions<GameContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Chosennumber> Chosennumbers { get; set; }
    public DbSet<Winner> Winners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pkey");
            entity.ToTable("game");
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Prizepool).HasPrecision(10, 2).HasColumnName("prizepool");
            entity.Property(e => e.Status).HasMaxLength(255).HasDefaultValueSql("'Active'::character varying").HasColumnName("status");
            entity.Property(e => e.Winningnumbers).HasMaxLength(50).HasColumnName("winningnumbers");
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");
            entity.ToTable("board");
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Dateofpurchase).HasColumnName("dateofpurchase");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Price).HasPrecision(10, 2).HasColumnName("price");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Game)
                  .WithMany(p => p.Boards)
                  .HasForeignKey(d => d.Gameid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("board_gameid_fkey");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Boards)
                  .HasForeignKey(d => d.Userid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("board_userid_fkey");
        });

        modelBuilder.Entity<Chosennumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chosennumbers_pkey");
            entity.ToTable("chosennumbers");
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Boardid).HasColumnName("boardid");
            entity.Property(e => e.Number).HasDefaultValue(0).HasColumnName("number");

            entity.HasOne(d => d.Board)
                  .WithMany(p => p.Chosennumbers)
                  .HasForeignKey(d => d.Boardid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("chosennumbers_boardid_fkey");
        });

        modelBuilder.Entity<Winner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("winners_pkey");
            entity.ToTable("winners");
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Wonamount).HasPrecision(10, 2).HasColumnName("wonamount");

            entity.HasOne(d => d.Game)
                  .WithMany(p => p.Winners)
                  .HasForeignKey(d => d.Gameid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("winners_gameid_fkey");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Winners)
                  .HasForeignKey(d => d.Userid)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("winners_userid_fkey");
        });
    }
}