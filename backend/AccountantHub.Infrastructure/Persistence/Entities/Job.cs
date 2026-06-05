namespace AccountantHub.Infrastructure.Persistence.Entities;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Open;
    public DateTime CreatedAt { get; set; }
    public string Tags { get; set; } = string.Empty;
    public int BidCount { get; set; }
}
