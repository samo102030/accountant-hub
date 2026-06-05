using AccountantHub.API.Features.Bids;

namespace AccountantHub.API.Features.Jobs;

public class JobDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();
    public int BidCount { get; set; }
    public BidDto? UserBid { get; set; }
}
