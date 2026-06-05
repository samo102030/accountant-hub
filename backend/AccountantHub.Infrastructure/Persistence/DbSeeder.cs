using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        await EnsureCategoriesAsync(db, cancellationToken);
        await EnsureJobsAsync(db, cancellationToken);
    }

    private static async Task EnsureCategoriesAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        if (await db.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        db.Categories.AddRange(
            new Category { Id = 1, Name = "Taxation", Slug = "taxation" },
            new Category { Id = 2, Name = "Audit", Slug = "audit" },
            new Category { Id = 3, Name = "Consulting", Slug = "consulting" },
            new Category { Id = 4, Name = "Bookkeeping", Slug = "bookkeeping" });

        await db.SaveChangesAsync(cancellationToken);
    }

    private static async Task EnsureJobsAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var existingCount = await db.Jobs.CountAsync(cancellationToken);
        if (existingCount >= JobCatalogSeed.TargetCount)
        {
            return;
        }

        var catalog = JobCatalogSeed.Build(DateTime.UtcNow);
        var jobsToAdd = catalog.Skip(existingCount).Take(JobCatalogSeed.TargetCount - existingCount).ToList();
        if (jobsToAdd.Count == 0)
        {
            return;
        }

        db.Jobs.AddRange(jobsToAdd);
        await db.SaveChangesAsync(cancellationToken);
    }
}
