namespace AccountantHub.API.Features.Bids;

public class MyBidListItemDto
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal ProposedPrice { get; set; }
    public int DeliveryDays { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
