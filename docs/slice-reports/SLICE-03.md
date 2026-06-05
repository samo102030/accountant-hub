# Slice Report — SLICE-03

## Summary

Slice 3 adds `GET /api/jobs/:id` for a single job by id (404 when missing) and a job details page at `/jobs/:id` aligned with local `stitch/job_details_page/code.html`. Job cards link to the details view. Supporting documents are static placeholders (locked, no upload). The sidebar CTA shows **Login to Apply** (disabled until Slice 4 auth). Production API base: `https://accountant-hub-production-cc2a.up.railway.app`.

## Files created

**Backend**

- `backend/AccountantHub.API/Features/Jobs/JobDetailDto.cs`

**Frontend**

- `frontend/src/app/features/jobs/job-details/job-details.component.ts`
- `frontend/src/app/features/jobs/job-details/job-details.component.html`

**Docs**

- `docs/slice-reports/SLICE-03.md`

## Files modified

- `backend/AccountantHub.API/Features/Jobs/JobsController.cs` — `GET /api/jobs/{id}`
- `frontend/src/app/core/models/job.model.ts` — `JobDetail`, `JobDetailApiResponse`
- `frontend/src/app/core/services/jobs.service.ts` — `getJob(id)`
- `frontend/src/app/app.routes.ts` — route `jobs/:id`
- `frontend/src/app/shared/components/job-card/job-card.component.ts` — `RouterLink`
- `frontend/src/app/shared/components/job-card/job-card.component.html` — chevron navigates to details

## Endpoints added

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/jobs/{id}` | No | Single job by id; **404** if not found |

**`GET /api/jobs/{id}` success response**

```json
{
  "success": true,
  "message": "OK",
  "data": {
    "id", "title", "description", "companyName", "category", "categorySlug",
    "budgetMin", "budgetMax", "status", "createdAt", "tags", "bidCount"
  },
  "meta": null
}
```

**Not found**

```json
{
  "success": false,
  "message": "Job not found",
  "data": null,
  "meta": null
}
```

## Database changes

None.

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | **green** on `main` — `/api/health` 200, `/api/jobs` 200 |
| Railway `GET /api/jobs/1` | Same domain | **404 on `main`** — expected until `feature/slice-3` is merged and Railway redeploys |
| Netlify (Angular) | Dashboard production URL | **pending merge** — job details UI ships with branch merge + Netlify rebuild |
| Local backend build | `dotnet build` | **green** |
| Local frontend build | `npm run build` → `dist/frontend/browser` | **green** (~778 kB initial) |

Railway auto-deploys from **`main`** only. Live `GET /api/jobs/:id` and Netlify details page require user merge of `feature/slice-3`.

## Merge status

- Branch **`feature/slice-3`** pushed to `origin` (commit `9923f20`)
- Merge to `main`: **pending user approval** — Agent did not merge

## How to verify

**After merge to `main` (production)**

1. `GET https://accountant-hub-production-cc2a.up.railway.app/api/jobs/1` — 200, full job object
2. `GET https://accountant-hub-production-cc2a.up.railway.app/api/jobs/999` — 404, `"Job not found"`
3. `https://accountant-hub-production-cc2a.up.railway.app/swagger` — `GET /api/Jobs/{id}`
4. Netlify production URL — open jobs list → click chevron on a card → `/jobs/1` shows details, budget sidebar, attachment placeholders, **Login to Apply**

**Local**

1. `cd backend/AccountantHub.API && dotnet run` (Postgres / `DATABASE_URL`)
2. `GET http://localhost:5080/api/jobs/1` — 200
3. `cd frontend && npm start` — `http://localhost:4200` → job card → `/jobs/1`
4. Invalid id `/jobs/999` — error message + back link

**Pre-merge (current `main`)**

- `GET .../api/jobs/1` returns **404** (endpoint not on `main` yet)
- `GET .../api/jobs?page=1&pageSize=1` returns seeded job id `1` for post-merge checks

## Known issues / blockers

1. **Production endpoint 404 until merge:** Railway tracks `main`; `GET /api/jobs/:id` is only on `feature/slice-3`.
2. **Login to Apply:** Button is disabled with no route — Slice 4 adds login/register and navigation.
3. **Attachments:** Static placeholder rows only; no file storage or download (by design this slice).
4. **Delivery / payment / client rating:** Placeholder copy where seed data has no fields; full client profile deferred.
5. **Save for Later:** Disabled placeholder — out of scope until a later slice.
6. **Netlify URL:** Not committed; confirm in dashboard after merge.

## Netlify notes

- `environment.production.ts` → `apiUrl: 'https://accountant-hub-production-cc2a.up.railway.app'` (unchanged)
- Publish: `dist/frontend/browser`

## Stitch

Layout reference: local `stitch/job_details_page/code.html` (gitignored). Translated to Angular + Tailwind tokens from Slice 2; two-column layout collapses on mobile; sticky budget sidebar on desktop.
