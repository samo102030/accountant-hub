namespace AccountantHub.API.Features.Bids;

public class BidDto
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public decimal ProposedPrice { get; set; }
    public int DeliveryDays { get; set; }
    public string CoverLetter { get; set; } = string.Empty;
    public string ExperienceSummary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
