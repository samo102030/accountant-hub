# Slice Report — SLICE-05

## Summary

Slice 5 adds bid submission: `Bids` table with unique `(UserId, JobId)`, `POST /api/jobs/{id}/bids` (JWT), validation (400) and duplicate-bid (409) handling. `GET /api/jobs/{id}` returns optional `userBid` when the caller is authenticated. Frontend: shared `bid-form`, `/jobs/:id/bid` submission page (Stitch `bid_submission`), job details **already-bid** sidebar (Stitch `job_details_already_bid_state`), success toast after submit, Apply hidden/disabled when job is Closed or user already bid.

**Branch note:** `feature/slice-5` was created from `feature/slice-4` because Slice 4 auth is not yet on `main`. Merge **Slice 4 first**, then Slice 5 (or merge both in order).

Production API: `https://accountant-hub-production-cc2a.up.railway.app` — `JWT_SECRET` assumed set from Slice 4.

## Files created

**Backend**

- `backend/AccountantHub.Infrastructure/Persistence/Entities/Bid.cs`
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/20260605041254_AddBids.cs`
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/20260605041254_AddBids.Designer.cs`
- `backend/AccountantHub.API/Features/Bids/BidsController.cs`
- `backend/AccountantHub.API/Features/Bids/CreateBidRequest.cs`
- `backend/AccountantHub.API/Features/Bids/BidDto.cs`

**Frontend**

- `frontend/src/app/core/models/bid.model.ts`
- `frontend/src/app/core/services/bids.service.ts`
- `frontend/src/app/shared/components/bid-form/bid-form.component.ts`
- `frontend/src/app/shared/components/bid-form/bid-form.component.html`
- `frontend/src/app/features/jobs/bid-submission/bid-submission.component.ts`
- `frontend/src/app/features/jobs/bid-submission/bid-submission.component.html`

**Docs**

- `docs/slice-reports/SLICE-05.md`

## Files modified

- `backend/AccountantHub.Infrastructure/Persistence/AppDbContext.cs` — `DbSet<Bid>`, unique index, FKs
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/AppDbContextModelSnapshot.cs`
- `backend/AccountantHub.API/Features/Jobs/JobsController.cs` — `userBid` on job detail for auth users
- `backend/AccountantHub.API/Features/Jobs/JobDetailDto.cs` — `UserBid` property
- `frontend/src/app/core/models/job.model.ts` — `userBid` on `JobDetail`
- `frontend/src/app/app.routes.ts` — `/jobs/:id/bid` with `authGuard`
- `frontend/src/app/features/jobs/job-details/job-details.component.ts` — Apply routing, already-bid state, toast
- `frontend/src/app/features/jobs/job-details/job-details.component.html` — Stitch already-bid sidebar / closed job CTA

## Endpoints added

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/jobs/{id}/bids` | JWT | Submit bid — body: `proposedPrice`, `deliveryDays`, `coverLetter`, `experienceSummary` |

**Extended (not new route)**

| Method | Path | Change |
|--------|------|--------|
| GET | `/api/jobs/{id}` | When `Authorization: Bearer <token>` present, `data.userBid` is populated or `null` |

**Success response (POST bid)**

```json
{
  "success": true,
  "message": "Bid submitted successfully.",
  "data": {
    "id": 1,
    "jobId": 3,
    "proposedPrice": 4500.00,
    "deliveryDays": 14,
    "coverLetter": "...",
    "experienceSummary": "...",
    "createdAt": "2026-06-05T04:00:00Z"
  },
  "meta": null
}
```

**Validation error (400)**

```json
{
  "success": false,
  "message": "Proposed price must be greater than 0.",
  "data": null,
  "meta": null
}
```

**Duplicate bid (409)**

```json
{
  "success": false,
  "message": "You have already submitted a bid for this job.",
  "data": null,
  "meta": null
}
```

**Closed job (400)**

```json
{
  "success": false,
  "message": "This job is closed and no longer accepting bids.",
  "data": null,
  "meta": null
}
```

## Database changes

Migration `AddBids` creates `Bids` table:

| Column | Type | Notes |
|--------|------|-------|
| `Id` | int PK | Identity |
| `JobId` | int FK → `Jobs` | Cascade delete |
| `UserId` | text FK → `AspNetUsers` | Cascade delete |
| `ProposedPrice` | numeric(18,2) | > 0 |
| `DeliveryDays` | int | > 0 |
| `CoverLetter` | varchar(2000) | Required |
| `ExperienceSummary` | varchar(1000) | Required |
| `CreatedAt` | timestamptz | UTC |

**Unique index:** `IX_Bids_UserId_JobId` on `(UserId, JobId)` — one bid per user per job.

On successful bid, `Jobs.BidCount` is incremented.

Applied automatically on API startup via `db.Database.Migrate()`.

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | **health 200** on current `main`; bids endpoint **pending merge** of `feature/slice-4` + `feature/slice-5` |
| Netlify (Angular) | Dashboard production URL | **pending merge** — bid UI after branch merge + rebuild |
| Local backend build | `dotnet build` | **green** |
| Local frontend build | `npm run build` → `dist/frontend/browser` | **green** (~868 kB initial) |

Railway auto-deploys from **`main`** only. Live bid endpoints require user merge of `feature/slice-4` then `feature/slice-5` (or both in sequence).

**Railway prerequisite:** `JWT_SECRET` must remain set on the API service (from Slice 4).

## Merge status

- Branch **`feature/slice-5`** pushed to `origin`
- Depends on **`feature/slice-4`** (auth) — merge Slice 4 to `main` before or with Slice 5
- Merge to `main`: **pending user approval** — Agent did not merge

## How to verify

**After merge to `main` (production)**

1. Register/login on Netlify — obtain JWT
2. Open an **Open** job → **Apply** → `/jobs/{id}/bid` form (Stitch layout)
3. Submit valid bid — success toast, redirect to job details with **Bid Already Submitted** sidebar
4. Re-submit same job — **409** from API; UI shows already-bid state
5. Open a **Closed** job (seed id 9) — **Job Closed** button, no Apply
6. `GET /api/jobs/{id}` with Bearer token — `data.userBid` populated after bid
7. Swagger — **Authorize** with Bearer, test `POST /api/jobs/1/bids`

**Local**

1. `cd backend/AccountantHub.API && dotnet run` (Postgres / `DATABASE_URL`)
2. Register: `POST http://localhost:5080/api/auth/register`
3. `POST http://localhost:5080/api/jobs/1/bids` with Bearer token and body fields
4. Repeat POST — **409**
5. `cd frontend && npm start` — login → job details → Apply → submit bid → already-bid sidebar

**Pre-merge (current `main`)**

- `POST .../api/jobs/1/bids` returns **404** (endpoint not on `main` yet)
- `GET /api/health` returns **200**

## Known issues / blockers

1. **Production bids 404 until merge:** Railway tracks `main`; bids + auth are on feature branches only.
2. **Slice 4 dependency:** `feature/slice-5` includes Slice 4 commits; merge Slice 4 first if merging incrementally.
3. **Withdraw / edit bid:** Stitch shows “Withdraw Proposal” — out of Slice 5 scope; not implemented.
4. **View My Proposal:** Opens toast with cover letter; full proposal view ships in Slice 6 (My Bids).
5. **Bundle size warning:** Initial chunk ~868 kB exceeds 800 kB budget (PrimeNG + Toast); acceptable for assessment.
6. **Netlify URL:** Not committed; confirm in dashboard after merge.

## Stitch

Layout references: local `stitch/bid_submission/code.html` (two-column form page) and `stitch/job_details_already_bid_state/code.html` (sidebar status card). Translated to Angular + Tailwind tokens; withdraw proposal omitted.

## Netlify notes

- `environment.production.ts` → `apiUrl: 'https://accountant-hub-production-cc2a.up.railway.app'` (unchanged)
- Publish: `dist/frontend/browser`
