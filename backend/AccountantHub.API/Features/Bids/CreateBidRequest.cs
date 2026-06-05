using System.ComponentModel.DataAnnotations;

namespace AccountantHub.API.Features.Bids;

public class CreateBidRequest
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Proposed price must be greater than 0.")]
    public decimal ProposedPrice { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Delivery days must be greater than 0.")]
    public int DeliveryDays { get; set; }

    [Required]
    [MaxLength(2000, ErrorMessage = "Cover letter must be 2000 characters or fewer.")]
    public string CoverLetter { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000, ErrorMessage = "Experience summary must be 1000 characters or fewer.")]
    public string ExperienceSummary { get; set; } = string.Empty;
}
