using AccountantHub.Infrastructure.Persistence.Entities;

namespace AccountantHub.Infrastructure.Persistence;

internal static class JobCatalogSeed
{
    public const int TargetCount = 40;

    public static IReadOnlyList<Job> Build(DateTime now)
    {
        return new[]
        {
            Job("Senior Tax Consultant", "Seeking an experienced tax consultant for Q4 corporate filings. Must have proficiency in multi-state tax compliance and SEC reporting standards.", "Green Financials", 1, 1200, 2500, JobStatus.Open, now.AddHours(-2), now.AddDays(21), "TAXATION,CORPORATE,Q4 FILING", 12),
            Job("External Audit Associate", "Assisting in the execution of financial statement audits for mid-sized tech companies. Expertise in GAAP and risk assessment required.", "Global Ledger Group", 2, 800, 1500, JobStatus.Open, now.AddHours(-5), now.AddDays(14), "AUDIT,GAAP", 5),
            Job("Strategic Financial Advisor", "Long-term consulting project for a Series B startup. Help define revenue recognition policies and prepare for upcoming Series C due diligence.", "Blue Chip Partners", 3, 3000, 5500, JobStatus.Open, now.AddHours(-8), now.AddDays(45), "CONSULTING,STRATEGY", 18),
            Job("Part-time Bookkeeping", "Monthly reconciliation and bookkeeping for a remote SaaS company. Proficiency in QuickBooks Online and Xero is mandatory.", "Cloud Solutions Inc", 4, 400, 600, JobStatus.Open, now.AddDays(-1), now.AddDays(30), "BOOKKEEPING,RECURRING", 24),
            Job("Individual Tax Return Specialist", "High-volume individual tax preparation during peak season. Experience with complex itemized deductions and rental property schedules.", "Summit Tax Services", 1, 500, 900, JobStatus.Open, now.AddDays(-2), now.AddDays(10), "TAXATION,INDIVIDUAL", 8),
            Job("Internal Controls Auditor", "Evaluate and document internal control frameworks for a manufacturing client preparing for IPO readiness.", "Precision Audit Co", 2, 2000, 3500, JobStatus.Open, now.AddDays(-3), now.AddDays(28), "AUDIT,CONTROLS", 7),
            Job("CFO Advisory — Growth Stage", "Part-time CFO support for a D2C brand scaling internationally. Cash flow modeling and board reporting experience required.", "Northstar Ventures", 3, 4000, 7000, JobStatus.Open, now.AddDays(-4), now.AddDays(60), "CONSULTING,CFO", 11),
            Job("Accounts Payable Reconciliation", "Weekly AP reconciliation and vendor statement matching for a retail group with multiple entities.", "Retail Ledger LLC", 4, 300, 500, JobStatus.Open, now.AddDays(-5), now.AddDays(7), "BOOKKEEPING,AP", 15),
            Job("Sales Tax Compliance Review", "Review nexus and sales tax filings across 12 states for an e-commerce operator. Prior Big Four experience preferred.", "Commerce Tax Advisors", 1, 1500, 2800, JobStatus.Closed, now.AddDays(-10), now.AddDays(-2), "TAXATION,SALES TAX", 9),
            Job("Nonprofit Financial Review", "Annual financial statement review and grant compliance reporting for a regional nonprofit foundation.", "Community Impact Fund", 2, 900, 1600, JobStatus.Open, now.AddDays(-6), now.AddDays(20), "AUDIT,NONPROFIT", 4),
            Job("R&D Tax Credit Analyst", "Identify and document qualified research expenses for a software development firm seeking federal and state R&D credits.", "Innovate Tax Partners", 1, 1800, 3200, JobStatus.Open, now.AddDays(-7), now.AddDays(25), "TAXATION,R&D", 6),
            Job("Forensic Accounting Review", "Support litigation counsel with financial tracing and damages analysis for a commercial dispute.", "Harbor Forensics LLP", 2, 3500, 6000, JobStatus.Open, now.AddDays(-8), now.AddDays(35), "AUDIT,FORENSIC", 3),
            Job("ERP Implementation Consultant", "Lead finance module configuration during NetSuite rollout for a multi-entity holding company.", "Systems Finance Group", 3, 5000, 9000, JobStatus.Open, now.AddDays(-9), now.AddDays(90), "CONSULTING,ERP", 14),
            Job("Payroll Processing Specialist", "Bi-weekly payroll runs and quarterly filings for a 120-employee professional services firm.", "People Ledger Co", 4, 450, 750, JobStatus.Open, now.AddDays(-10), now.AddDays(14), "BOOKKEEPING,PAYROLL", 19),
            Job("Transfer Pricing Documentation", "Prepare OECD-aligned transfer pricing study for a cross-border manufacturing group.", "Atlas Tax Advisory", 1, 4000, 7500, JobStatus.Open, now.AddDays(-11), now.AddDays(40), "TAXATION,TRANSFER PRICING", 5),
            Job("SOC 2 Readiness Audit", "Assess control environment and evidence collection for a fintech preparing SOC 2 Type II examination.", "Trust Audit Partners", 2, 2500, 4200, JobStatus.Open, now.AddDays(-12), now.AddDays(30), "AUDIT,SOC2", 10),
            Job("Pricing Strategy Consultant", "Build unit economics models and pricing tiers for a subscription marketplace entering enterprise segment.", "Scale Advisory", 3, 2800, 4800, JobStatus.Open, now.AddDays(-13), now.AddDays(50), "CONSULTING,PRICING", 8),
            Job("Inventory Reconciliation Lead", "Monthly inventory counts reconciliation and COGS variance analysis for a wholesale distributor.", "StockLine Books", 4, 600, 1100, JobStatus.Open, now.AddDays(-14), now.AddDays(18), "BOOKKEEPING,INVENTORY", 12),
            Job("Estate Tax Planning Support", "Assist with estate tax projections and trust return preparation for high-net-worth client portfolio.", "Legacy Tax Counsel", 1, 2200, 4000, JobStatus.Open, now.AddDays(-15), now.AddDays(22), "TAXATION,ESTATE", 4),
            Job("Revenue Recognition Audit", "Test ASC 606 policies and contract modifications for a SaaS company with usage-based billing.", "SaaS Audit Desk", 2, 1900, 3400, JobStatus.Open, now.AddDays(-16), now.AddDays(26), "AUDIT,REVENUE", 7),
            Job("Working Capital Optimization", "Analyze AR/AP cycles and recommend cash conversion improvements for a logistics operator.", "Flow Capital Advisors", 3, 3200, 5800, JobStatus.Open, now.AddDays(-17), now.AddDays(55), "CONSULTING,WORKING CAPITAL", 9),
            Job("Franchise Unit Bookkeeping", "Standardized monthly close templates and reporting for a 15-unit quick-service franchise.", "Franchise Ledger", 4, 700, 1200, JobStatus.Open, now.AddDays(-18), now.AddDays(16), "BOOKKEEPING,FRANCHISE", 21),
            Job("International Tax Compliance", "Coordinate foreign subsidiary filings and GILTI calculations for a US parent with EU operations.", "CrossBorder Tax Co", 1, 3500, 6500, JobStatus.Open, now.AddDays(-19), now.AddDays(38), "TAXATION,INTERNATIONAL", 6),
            Job("Operational Audit — Retail", "Fieldwork and walkthroughs for shrinkage and cash handling controls across 8 store locations.", "Retail Assurance", 2, 1600, 2900, JobStatus.Open, now.AddDays(-20), now.AddDays(24), "AUDIT,RETAIL", 5),
            Job("Fundraising Financial Model", "Build three-statement model and investor metrics pack for a seed-stage healthtech startup.", "Venture Model Studio", 3, 1500, 2800, JobStatus.Open, now.AddDays(-21), now.AddDays(32), "CONSULTING,MODELING", 16),
            Job("1099 Contractor Cleanup", "Reconcile contractor payments and issue corrected 1099-NEC filings before IRS deadline.", "Contractor Books", 4, 350, 550, JobStatus.Open, now.AddDays(-22), now.AddDays(8), "BOOKKEEPING,1099", 13),
            Job("Property Tax Appeal Support", "Compile assessment comparables and appeal packages for a commercial real estate portfolio.", "Metro Tax Appeals", 1, 1100, 2000, JobStatus.Open, now.AddDays(-23), now.AddDays(19), "TAXATION,PROPERTY", 3),
            Job("IT General Controls Testing", "Execute ITGC test scripts for change management and access provisioning ahead of year-end audit.", "Digital Audit Group", 2, 2100, 3800, JobStatus.Open, now.AddDays(-24), now.AddDays(27), "AUDIT,ITGC", 8),
            Job("Post-Merger Integration Finance", "Harmonize chart of accounts and reporting calendars after acquisition of a regional competitor.", "MergePath Consulting", 3, 4500, 8000, JobStatus.Open, now.AddDays(-25), now.AddDays(70), "CONSULTING,M&A", 11),
            Job("Bank Feed Reconciliation", "Daily transaction categorization and month-end bank reconciliations for a property management firm.", "Property Books Pro", 4, 500, 850, JobStatus.Open, now.AddDays(-26), now.AddDays(12), "BOOKKEEPING,BANK REC", 17),
            Job("Partnership K-1 Preparation", "Prepare complex multi-tier partnership K-1s and state composite filings for real estate fund.", "Fund Tax Partners", 1, 2800, 5200, JobStatus.Open, now.AddDays(-27), now.AddDays(33), "TAXATION,PARTNERSHIP", 5),
            Job("Grant Compliance Audit", "Verify allowable costs and time-and-effort documentation for federal grant recipients.", "Public Sector Audit", 2, 1300, 2400, JobStatus.Open, now.AddDays(-28), now.AddDays(21), "AUDIT,GRANTS", 4),
            Job("Board Reporting Package", "Design monthly board deck with KPI dashboards and variance commentary for PE-backed portfolio company.", "Boardroom Finance", 3, 2400, 4300, JobStatus.Open, now.AddDays(-29), now.AddDays(42), "CONSULTING,BOARD", 10),
            Job("Multi-Currency Bookkeeping", "Record FX gains/losses and intercompany eliminations for a UK-US dual-entity SaaS business.", "Global Books Ltd", 4, 800, 1400, JobStatus.Open, now.AddDays(-30), now.AddDays(15), "BOOKKEEPING,FX", 14),
            Job("Payroll Tax Audit Defense", "Respond to state payroll tax notices and reconcile historical withholding deposits.", "Payroll Tax Defense", 1, 1700, 3100, JobStatus.Open, now.AddDays(-31), now.AddDays(17), "TAXATION,PAYROLL TAX", 6),
            Job("Fixed Asset Verification", "Physical verification and depreciation schedule update for a hospital system capital assets.", "Capital Audit Services", 2, 2000, 3600, JobStatus.Closed, now.AddDays(-35), now.AddDays(-5), "AUDIT,FIXED ASSETS", 2),
            Job("Sustainability Reporting Advisor", "Map ESG metrics to financial disclosures and draft first-year sustainability appendix.", "Green Metrics Advisory", 3, 2600, 4600, JobStatus.Open, now.AddDays(-32), now.AddDays(48), "CONSULTING,ESG", 7),
            Job("Construction Job Costing", "Track WIP schedules and percent-complete revenue for a commercial general contractor.", "BuildCost Ledger", 4, 900, 1600, JobStatus.Open, now.AddDays(-33), now.AddDays(23), "BOOKKEEPING,CONSTRUCTION", 9),
            Job("State Nexus Study", "Determine economic nexus exposure and recommend registration roadmap for expanding DTC brand.", "Nexus Tax Lab", 1, 1400, 2600, JobStatus.Open, now.AddDays(-34), now.AddDays(20), "TAXATION,NEXUS", 8),
            Job("Year-End Close Support", "Accelerate close calendar with flux analysis and audit PBC list preparation for public filer.", "CloseCycle Audit", 2, 3000, 5500, JobStatus.Open, now.AddDays(-36), now.AddDays(29), "AUDIT,YEAR END", 12)
        };
    }

    private static Job Job(
        string title,
        string description,
        string companyName,
        int categoryId,
        decimal budgetMin,
        decimal budgetMax,
        JobStatus status,
        DateTime createdAt,
        DateTime deadline,
        string tags,
        int bidCount) =>
        new()
        {
            Title = title,
            Description = description,
            CompanyName = companyName,
            CategoryId = categoryId,
            BudgetMin = budgetMin,
            BudgetMax = budgetMax,
            Status = status,
            CreatedAt = createdAt,
            Deadline = deadline,
            Tags = tags,
            BidCount = bidCount
        };
}
