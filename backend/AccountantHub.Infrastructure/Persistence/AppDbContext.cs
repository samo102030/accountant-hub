using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Job> Jobs => Set<Job>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Slug).HasMaxLength(100).IsRequired();
            entity.HasIndex(c => c.Slug).IsUnique();
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Title).HasMaxLength(200).IsRequired();
            entity.Property(j => j.Description).HasMaxLength(4000).IsRequired();
            entity.Property(j => j.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(j => j.BudgetMin).HasPrecision(18, 2);
            entity.Property(j => j.BudgetMax).HasPrecision(18, 2);
            entity.Property(j => j.Tags).HasMaxLength(500);
            entity.HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
