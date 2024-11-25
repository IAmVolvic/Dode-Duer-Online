using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configure the User entity
    modelBuilder.Entity<User>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("users_pkey");
        entity.ToTable("users");
        entity.HasIndex(e => e.Email, "users_email_key").IsUnique();
        entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
        entity.Property(e => e.Balance).HasPrecision(10, 2).HasDefaultValueSql("0").HasColumnName("balance");
        entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
        entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
        entity.Property(e => e.Role).HasMaxLength(255).HasDefaultValueSql("'User'::character varying").HasColumnName("role");
        entity.Property(e => e.Status).HasMaxLength(255).HasDefaultValueSql("'Active'::character varying").HasColumnName("status");
    });

    // Configure the Transaction entity
    modelBuilder.Entity<Transaction>(entity =>
    {
        entity.HasKey(e => e.Id).HasName("transactions_pkey");
        entity.ToTable("transactions");
        
        // Map the properties to their respective columns
        entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
        entity.Property(e => e.TransactionPhoneNumber).HasMaxLength(255).HasColumnName("transactionphonenumber");
        entity.Property(e => e.TransactionUsername).HasMaxLength(255).HasColumnName("transactionusername");
        entity.Property(e => e.TransactionNumber).HasMaxLength(255).HasColumnName("transactionnumber");
        entity.Property(e => e.TransactionStatus).HasMaxLength(255).HasDefaultValue("Pending").HasColumnName("transactionstatus");

        // Map the foreign key relationship
        entity.Property(e => e.UserId).HasColumnName("userid");

        // Configure the one-to-many relationship with User
        entity.HasOne(d => d.User)
              .WithMany(p => p.Transactions)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("transactions_userid_fkey");
        
    });
}
}

