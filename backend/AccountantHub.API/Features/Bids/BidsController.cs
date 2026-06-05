using System.Security.Claims;
using AccountantHub.Infrastructure.Persistence;
using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountantHub.API.Features.Bids;

[ApiController]
[Route("api/jobs/{jobId:int}/bids")]
[Authorize]
public class BidsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BidsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBid(int jobId, [FromBody] CreateBidRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ValidationErrorResponse());
        }

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

        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);
        if (job is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Job not found.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        if (job.Status != JobStatus.Open)
        {
            return BadRequest(new
            {
                success = false,
                message = "This job is closed and no longer accepting bids.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var existingBid = await _db.Bids
            .AsNoTracking()
            .AnyAsync(b => b.JobId == jobId && b.UserId == userId, cancellationToken);

        if (existingBid)
        {
            return Conflict(new
            {
                success = false,
                message = "You have already submitted a bid for this job.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var bid = new Bid
        {
            JobId = jobId,
            UserId = userId,
            ProposedPrice = request.ProposedPrice,
            DeliveryDays = request.DeliveryDays,
            CoverLetter = request.CoverLetter.Trim(),
            ExperienceSummary = request.ExperienceSummary.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _db.Bids.Add(bid);
        job.BidCount++;
        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Bid submitted successfully.",
            data = new BidDto
            {
                Id = bid.Id,
                JobId = bid.JobId,
                ProposedPrice = bid.ProposedPrice,
                DeliveryDays = bid.DeliveryDays,
                CoverLetter = bid.CoverLetter,
                ExperienceSummary = bid.ExperienceSummary,
                CreatedAt = bid.CreatedAt
            },
            meta = (object?)null
        });
    }

    private object ValidationErrorResponse()
    {
        var message = string.Join(
            " ",
            ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

        return new
        {
            success = false,
            message,
            data = (object?)null,
            meta = (object?)null
        };
    }
}
