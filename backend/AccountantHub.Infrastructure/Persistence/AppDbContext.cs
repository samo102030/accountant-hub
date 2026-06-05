using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Bid> Bids => Set<Bid>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FullName).HasMaxLength(200).IsRequired();
        });

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

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.ProposedPrice).HasPrecision(18, 2);
            entity.Property(b => b.CoverLetter).HasMaxLength(2000).IsRequired();
            entity.Property(b => b.ExperienceSummary).HasMaxLength(1000).IsRequired();
            entity.HasIndex(b => new { b.UserId, b.JobId }).IsUnique();
            entity.HasOne(b => b.Job)
                .WithMany()
                .HasForeignKey(b => b.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
