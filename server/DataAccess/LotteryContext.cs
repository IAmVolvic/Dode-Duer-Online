using System;
using System.Collections.Generic;
using DataAccess.Models;
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

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Winner> Winners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Dateofpurchase).HasColumnName("dateofpurchase");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Game).WithMany(p => p.Boards)
                .HasForeignKey(d => d.Gameid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("board_gameid_fkey");

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
                .HasMaxLength(255)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Winningnumbers)
                .HasMaxLength(50)
                .HasColumnName("winningnumbers");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Mobilepayid)
                .HasMaxLength(255)
                .HasColumnName("mobilepayid");
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
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasDefaultValueSql("'User'::character varying")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasDefaultValueSql("'Active'::character varying")
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
