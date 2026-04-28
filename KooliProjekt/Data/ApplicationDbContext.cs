using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MediaItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");

        builder.Entity<Category>()
            .HasMany(x => x.MediaItems)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Loan>()
            .HasOne(x => x.MediaItem)
            .WithMany(x => x.Loans)
            .HasForeignKey(x => x.MediaItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Loan>()
            .HasOne(x => x.Borrower)
            .WithMany(x => x.Loans)
            .HasForeignKey(x => x.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(x => x.MediaItem)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MediaItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(x => x.Reviewer)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
