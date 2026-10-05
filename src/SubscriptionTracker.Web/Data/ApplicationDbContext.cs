using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<UserBudget> UserBudgets => Set<UserBudget>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasIndex(c => c.Name).IsUnique();
        });

        builder.Entity<Subscription>(entity =>
        {
            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.Price)
                  .HasPrecision(18, 2);

            entity.Property(s => s.Description)
                  .HasMaxLength(500);

            entity.HasOne(s => s.Category)
                  .WithMany(c => c.Subscriptions)
                  .HasForeignKey(s => s.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.User)
                  .WithMany()
                  .HasForeignKey(s => s.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<UserBudget>(entity =>
        {
            entity.Property(b => b.MonthlyLimit)
                  .HasPrecision(18, 2);

            entity.HasIndex(b => b.UserId).IsUnique();

            entity.HasOne(b => b.User)
                  .WithMany()
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}