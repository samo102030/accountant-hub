using AccountantHub.Infrastructure.Persistence;
using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.API.Features.Jobs;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _db;

    public JobsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs([FromQuery] JobsQueryParameters query, CancellationToken cancellationToken)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 50 ? 10 : query.PageSize;

        var jobsQuery = _db.Jobs
            .AsNoTracking()
            .Include(j => j.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLower();
            jobsQuery = jobsQuery.Where(j =>
                j.Title.ToLower().Contains(term) ||
                j.Description.ToLower().Contains(term) ||
                j.CompanyName.ToLower().Contains(term) ||
                j.Tags.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(query.Category))
        {
            var category = query.Category.Trim().ToLower();
            jobsQuery = jobsQuery.Where(j =>
                j.Category.Slug == category ||
                j.Category.Name.ToLower() == category);
        }

        if (query.BudgetMin.HasValue)
        {
            jobsQuery = jobsQuery.Where(j => j.BudgetMax >= query.BudgetMin.Value);
        }

        if (query.BudgetMax.HasValue)
        {
            jobsQuery = jobsQuery.Where(j => j.BudgetMin <= query.BudgetMax.Value);
        }

        jobsQuery = query.Sort?.ToLower() switch
        {
            "budget_asc" => jobsQuery.OrderBy(j => j.BudgetMin).ThenByDescending(j => j.CreatedAt),
            "budget_desc" => jobsQuery.OrderByDescending(j => j.BudgetMax).ThenByDescending(j => j.CreatedAt),
            "title_asc" => jobsQuery.OrderBy(j => j.Title).ThenByDescending(j => j.CreatedAt),
            _ => jobsQuery.OrderByDescending(j => j.CreatedAt)
        };

        var total = await jobsQuery.CountAsync(cancellationToken);
        var items = await jobsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(j => new JobListItemDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                CompanyName = j.CompanyName,
                Category = j.Category.Name,
                CategorySlug = j.Category.Slug,
                BudgetMin = j.BudgetMin,
                BudgetMax = j.BudgetMax,
                Status = j.Status == JobStatus.Open ? "Open" : "Closed",
                CreatedAt = j.CreatedAt,
                Tags = j.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                BidCount = j.BidCount
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            success = true,
            message = "OK",
            data = items,
            meta = new { total, page, pageSize }
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetJob(int id, CancellationToken cancellationToken)
    {
        var job = await _db.Jobs
            .AsNoTracking()
            .Include(j => j.Category)
            .Where(j => j.Id == id)
            .Select(j => new JobDetailDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                CompanyName = j.CompanyName,
                Category = j.Category.Name,
                CategorySlug = j.Category.Slug,
                BudgetMin = j.BudgetMin,
                BudgetMax = j.BudgetMax,
                Status = j.Status == JobStatus.Open ? "Open" : "Closed",
                CreatedAt = j.CreatedAt,
                Tags = j.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                BidCount = j.BidCount
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Job not found",
                data = (object?)null,
                meta = (object?)null
            });
        }

        return Ok(new
        {
            success = true,
            message = "OK",
            data = job,
            meta = (object?)null
        });
    }
}
