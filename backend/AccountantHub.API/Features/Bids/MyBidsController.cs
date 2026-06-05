using System.Security.Claims;
using AccountantHub.Infrastructure.Persistence;
using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.API.Features.Bids;

[ApiController]
[Route("api/my-bids")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MyBidsController : ControllerBase
{
    private readonly AppDbContext _db;

    public MyBidsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyBids(
        [FromQuery] MyBidsQueryParameters query,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "Unauthorized.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 50 ? 10 : query.PageSize;

        var bidsQuery = _db.Bids
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt);

        var total = await bidsQuery.CountAsync(cancellationToken);
        var items = await bidsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new MyBidListItemDto
            {
                Id = b.Id,
                JobId = b.JobId,
                JobTitle = b.Job.Title,
                CompanyName = b.Job.CompanyName,
                JobStatus = b.Job.Status == JobStatus.Open ? "Open" : "Closed",
                Category = b.Job.Category.Name,
                ProposedPrice = b.ProposedPrice,
                DeliveryDays = b.DeliveryDays,
                CreatedAt = b.CreatedAt,
                Status = b.Job.Status == JobStatus.Open ? "Pending" : "Closed"
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
}
