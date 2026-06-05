namespace AccountantHub.Infrastructure.Persistence.Entities;

public class Bid
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public decimal ProposedPrice { get; set; }
    public int DeliveryDays { get; set; }
    public string CoverLetter { get; set; } = string.Empty;
    public string ExperienceSummary { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
