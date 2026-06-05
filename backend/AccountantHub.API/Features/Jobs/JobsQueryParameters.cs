namespace AccountantHub.API.Features.Jobs;

public class JobsQueryParameters
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public decimal? BudgetMin { get; set; }
    public decimal? BudgetMax { get; set; }
    public string Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
