using DataAccess.Models;
using DataAccess.Types.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class LotteryContext : DbContext
{
    public LotteryContext(DbContextOptions<LotteryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Chosennumber> Chosennumbers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Winner> Winners { get; set; }

    public virtual DbSet<WinningNumbers> WinningNumbers { get; set; }

    public virtual DbSet<BoardAutoplay> BoardAutoplays { get; set; }

    public virtual DbSet<ChosenNumbersAutoplay> ChosenNumbersAutoplays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("enrollment_status", new[] { "truey", "falsey" })
            .HasPostgresEnum("game_status", new[] { "active", "inactive" })
            .HasPostgresEnum("transaction_status", new[] { "pending", "approved", "rejected" })
            .HasPostgresEnum("user_roles", new[] { "user", "admin" })
            .HasPostgresEnum("user_status", new[] { "active", "inactive" });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Dateofpurchase).HasColumnName("dateofpurchase");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Priceid).HasColumnName("priceid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Game).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Gameid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_gameid_fkey");

            entity.HasOne(d => d.Price).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Priceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_priceid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_userid_fkey");
        });

        modelBuilder.Entity<Chosennumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chosennumbers_pkey");

            entity.ToTable("chosennumbers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Boardid).HasColumnName("boardid");
            entity.Property(e => e.Number)
                .HasDefaultValue(0)
                .HasColumnName("number");

            entity.HasOne(d => d.Board).WithMany(p => p.Chosennumbers)
                .HasForeignKey(d => d.Boardid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chosennumbers_boardid_fkey");
        });

        // Add BoardAutoplay entity configuration
        modelBuilder.Entity<BoardAutoplay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("boardautoplay_pkey");

            entity.ToTable("boardautoplay");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.LeftToPlay).HasColumnName("lefttoplay");

            entity.HasOne(d => d.User).WithMany(p => p.BoardAutoplays)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("boardautoplay_userid_fkey");
        });

        // Add ChosenNumbersAutoplay entity configuration
        modelBuilder.Entity<ChosenNumbersAutoplay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chosennumbersautoplay_pkey");

            entity.ToTable("chosennumbersautoplay");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BoardId).HasColumnName("boardid");
            entity.Property(e => e.Number)
                .HasDefaultValue(0)
                .HasColumnName("number");

            entity.HasOne(d => d.BoardAutoplay).WithMany(p => p.ChosenNumbersAutoplays)
                .HasForeignKey(d => d.BoardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chosennumbersautoplay_boardid_fkey");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_pkey");

            entity.ToTable("game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Prizepool)
                .HasPrecision(10, 2)
                .HasColumnName("prizepool");
            entity.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (GameStatus)Enum.Parse(typeof(GameStatus), v)
                )
                .HasColumnName("status");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prices_pkey");

            entity.ToTable("prices");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Numbers)
                .HasPrecision(10, 2)
                .HasColumnName("numbers");
            entity.Property(e => e.Price1)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.Transactionnumber)
                .HasMaxLength(255)
                .HasColumnName("transactionnumber");

            entity.Property(e => e.Transactionstatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (TransactionStatusA)Enum.Parse(typeof(TransactionStatusA), v)
                )
                .HasColumnName("transactionstatus");

            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transactions_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");

            entity.Property(e => e.Phonenumber)
                .HasMaxLength(255)
                .HasColumnName("phonenumber");

            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");

            entity.Property(e => e.Enrolled)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserEnrolled)Enum.Parse(typeof(UserEnrolled), v)
                )
                .HasColumnName("enrolled");

            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("balance");

            entity.Property(e => e.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v)
                )
                .HasColumnName("role");

            entity.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserStatus)Enum.Parse(typeof(UserStatus), v)
                )
                .HasColumnName("status");
        });

        modelBuilder.Entity<Winner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("winners_pkey");

            entity.ToTable("winners");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Wonamount)
                .HasPrecision(10, 2)
                .HasColumnName("wonamount");

            entity.HasOne(d => d.Game).WithMany(p => p.Winners)
                .HasForeignKey(d => d.Gameid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("winners_gameid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Winners)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("winners_userid_fkey");
        });

        modelBuilder.Entity<WinningNumbers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("winningnumbers_pkey");

            entity.ToTable("winningnumbers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.GameId).HasColumnName("gameid");

            entity.Property(e => e.Number).HasColumnName("number");

            entity.HasOne(d => d.Game).WithMany(p => p.WinningNumbers)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("winningnumbers_gameid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
