using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        if (await db.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        var categories = new[]
        {
            new Category { Id = 1, Name = "Taxation", Slug = "taxation" },
            new Category { Id = 2, Name = "Audit", Slug = "audit" },
            new Category { Id = 3, Name = "Consulting", Slug = "consulting" },
            new Category { Id = 4, Name = "Bookkeeping", Slug = "bookkeeping" }
        };

        var now = DateTime.UtcNow;
        var jobs = new[]
        {
            new Job
            {
                Title = "Senior Tax Consultant",
                Description = "Seeking an experienced tax consultant for Q4 corporate filings. Must have proficiency in multi-state tax compliance and SEC reporting standards.",
                CompanyName = "Green Financials",
                CategoryId = 1,
                BudgetMin = 1200,
                BudgetMax = 2500,
                Status = JobStatus.Open,
                CreatedAt = now.AddHours(-2),
                Tags = "TAXATION,CORPORATE,Q4 FILING",
                BidCount = 12
            },
            new Job
            {
                Title = "External Audit Associate",
                Description = "Assisting in the execution of financial statement audits for mid-sized tech companies. Expertise in GAAP and risk assessment required.",
                CompanyName = "Global Ledger Group",
                CategoryId = 2,
                BudgetMin = 800,
                BudgetMax = 1500,
                Status = JobStatus.Open,
                CreatedAt = now.AddHours(-5),
                Tags = "AUDIT,GAAP",
                BidCount = 5
            },
            new Job
            {
                Title = "Strategic Financial Advisor",
                Description = "Long-term consulting project for a Series B startup. Help define revenue recognition policies and prepare for upcoming Series C due diligence.",
                CompanyName = "Blue Chip Partners",
                CategoryId = 3,
                BudgetMin = 3000,
                BudgetMax = 5500,
                Status = JobStatus.Open,
                CreatedAt = now.AddHours(-8),
                Tags = "CONSULTING,STRATEGY",
                BidCount = 18
            },
            new Job
            {
                Title = "Part-time Bookkeeping",
                Description = "Monthly reconciliation and bookkeeping for a remote SaaS company. Proficiency in QuickBooks Online and Xero is mandatory.",
                CompanyName = "Cloud Solutions Inc",
                CategoryId = 4,
                BudgetMin = 400,
                BudgetMax = 600,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-1),
                Tags = "BOOKKEEPING,RECURRING",
                BidCount = 24
            },
            new Job
            {
                Title = "Individual Tax Return Specialist",
                Description = "High-volume individual tax preparation during peak season. Experience with complex itemized deductions and rental property schedules.",
                CompanyName = "Summit Tax Services",
                CategoryId = 1,
                BudgetMin = 500,
                BudgetMax = 900,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-2),
                Tags = "TAXATION,INDIVIDUAL",
                BidCount = 8
            },
            new Job
            {
                Title = "Internal Controls Auditor",
                Description = "Evaluate and document internal control frameworks for a manufacturing client preparing for IPO readiness.",
                CompanyName = "Precision Audit Co",
                CategoryId = 2,
                BudgetMin = 2000,
                BudgetMax = 3500,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-3),
                Tags = "AUDIT,CONTROLS",
                BidCount = 7
            },
            new Job
            {
                Title = "CFO Advisory — Growth Stage",
                Description = "Part-time CFO support for a D2C brand scaling internationally. Cash flow modeling and board reporting experience required.",
                CompanyName = "Northstar Ventures",
                CategoryId = 3,
                BudgetMin = 4000,
                BudgetMax = 7000,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-4),
                Tags = "CONSULTING,CFO",
                BidCount = 11
            },
            new Job
            {
                Title = "Accounts Payable Reconciliation",
                Description = "Weekly AP reconciliation and vendor statement matching for a retail group with multiple entities.",
                CompanyName = "Retail Ledger LLC",
                CategoryId = 4,
                BudgetMin = 300,
                BudgetMax = 500,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-5),
                Tags = "BOOKKEEPING,AP",
                BidCount = 15
            },
            new Job
            {
                Title = "Sales Tax Compliance Review",
                Description = "Review nexus and sales tax filings across 12 states for an e-commerce operator. Prior Big Four experience preferred.",
                CompanyName = "Commerce Tax Advisors",
                CategoryId = 1,
                BudgetMin = 1500,
                BudgetMax = 2800,
                Status = JobStatus.Closed,
                CreatedAt = now.AddDays(-10),
                Tags = "TAXATION,SALES TAX",
                BidCount = 9
            },
            new Job
            {
                Title = "Nonprofit Financial Review",
                Description = "Annual financial statement review and grant compliance reporting for a regional nonprofit foundation.",
                CompanyName = "Community Impact Fund",
                CategoryId = 2,
                BudgetMin = 900,
                BudgetMax = 1600,
                Status = JobStatus.Open,
                CreatedAt = now.AddDays(-6),
                Tags = "AUDIT,NONPROFIT",
                BidCount = 4
            }
        };

        db.Categories.AddRange(categories);
        db.Jobs.AddRange(jobs);
        await db.SaveChangesAsync(cancellationToken);
    }
}
