# Slice Report — SLICE-02

## Summary

Slice 2 adds Railway PostgreSQL via `DATABASE_URL`, EF Core migrations with startup migrate + seed (4 categories, 10 jobs), `GET /api/jobs` with search/category/budget filters, sort, and pagination, CORS for localhost and `*.netlify.app`, and the jobs listing UI (PrimeNG + Tailwind + Material Symbols) aligned with local Stitch guides. `environment.production.ts` keeps `apiUrl` = `https://accountant-hub-production-cc2a.up.railway.app`.

## Files created

**Backend**

- `backend/AccountantHub.Infrastructure/DatabaseConnection.cs`
- `backend/AccountantHub.Infrastructure/DependencyInjection.cs`
- `backend/AccountantHub.Infrastructure/Persistence/` — `AppDbContext`, `DbSeeder`, entities, `Migrations/InitialCreate`
- `backend/AccountantHub.API/Features/Jobs/` — `JobsController`, DTOs, query model

**Frontend**

- `frontend/postcss.config.json`
- `frontend/src/app/core/models/job.model.ts`
- `frontend/src/app/core/services/jobs.service.ts`
- `frontend/src/app/shared/components/` — `app-header`, `job-card`, `empty-state`, `pagination`
- `frontend/src/app/features/jobs/jobs-list/`

**Docs**

- `docs/slice-reports/SLICE-02.md`

## Files modified

- `backend/AccountantHub.API/Program.cs` — DB wiring, migrate/seed, CORS
- `backend/AccountantHub.API/appsettings.json` — local connection string + CORS origins
- `backend/AccountantHub.API/AccountantHub.API.csproj` — EF Design package
- `backend/AccountantHub.Infrastructure/AccountantHub.Infrastructure.csproj` — EF + Npgsql
- `frontend/package.json`, `package-lock.json` — PrimeNG, Tailwind 4, animations
- `frontend/angular.json` — production `fileReplacements`, bundle budget
- `frontend/src/styles.scss`, `index.html`, `app.config.ts`, routes, shell
- `frontend/src/environments/environment.ts` — local API `http://localhost:5080`
- `PROJECT_STRUCTURE.md`, `TECH_STACK.md`

## Endpoints added

| Method | Path | Auth | Query params |
|--------|------|------|----------------|
| GET | `/api/jobs` | No | `search`, `category`, `budgetMin`, `budgetMax`, `sort` (`newest`, `budget_asc`, `budget_desc`, `title_asc`), `page`, `pageSize` |

## Database changes

- Tables: `Categories`, `Jobs`
- Migration: `20260605012634_InitialCreate`
- Seed: 4 categories (Taxation, Audit, Consulting, Bookkeeping), 10 jobs
- Applied automatically on API startup: `Database.Migrate()` + `DbSeeder.SeedAsync()` when the app starts with a valid connection string

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | **Health green** on current production deploy (main). **`GET /api/jobs` returns 404** until production deploy includes this branch (merge `feature/slice-2` or point Railway at the branch). |
| Netlify (Angular) | Set in Netlify dashboard (e.g. `*.netlify.app`) | **Pending** preview/production build from `feature/slice-2` — jobs UI needs this branch deployed |

**Railway requirements (already stated by user):** `DATABASE_URL` on API service; Postgres plugin linked. No secrets committed.

**CORS:** Allows `http://localhost:4200`, configured `Cors:AllowedOrigins`, and any origin ending with `.netlify.app`.

## Merge status

- Branch: `feature/slice-2` pushed to `origin`
- Merge to `main`: **pending user approval** — Agent did not merge

## How to verify

**Local**

1. PostgreSQL running (or use Railway `DATABASE_URL` locally via user-secrets / env).
2. `cd backend/AccountantHub.API && dotnet run` — migrations + seed on startup.
3. `http://localhost:5080/api/jobs?page=1&pageSize=10` — JSON with `success: true`, `data` array, `meta.total` = 10 (first run).
4. `http://localhost:5080/swagger` — `GET /api/Jobs` documented.
5. `cd frontend && npm run build` — output under `frontend/dist/frontend/browser`.
6. `cd frontend && npm start` — jobs listing, filters, empty state (e.g. `search=zzznomatch`), pagination.

**Live (after Railway deploys Slice 2 code + `DATABASE_URL`)**

1. `GET https://accountant-hub-production-cc2a.up.railway.app/api/health` — 200 (verified during slice work).
2. `GET https://accountant-hub-production-cc2a.up.railway.app/api/jobs` — 200 with seeded jobs.
3. Netlify URL — jobs grid loads from production API (`environment.production.ts`).

## Known issues / blockers

1. **Production `/api/jobs`:** Railway currently serves **main** (Slice 1 only); jobs endpoint is **404** until `feature/slice-2` is deployed to production.
2. **Netlify URL:** Not stored in repo; confirm in Netlify dashboard and add to `Cors:AllowedOrigins` if using a custom domain (non-`*.netlify.app`).
3. **Bundle size:** Initial chunk ~754 kB (PrimeNG + Aura); budget warning raised to 800 kB / 1.5 MB cap in `angular.json`.
4. **`@primeng/themes`:** Package shows npm deprecation notice in favor of `@primeuix/themes`; left on 21.x for Angular 21 compatibility this slice.
5. **Job detail navigation:** Card chevron is visual only until Slice 3 (`GET /api/jobs/:id` + details route).

## Railway / Netlify notes

- **Railway:** Root `backend/AccountantHub.API`; .NET 8; `DATABASE_URL` parsed per prompt; migrate + seed on boot.
- **Netlify:** Base `frontend`; build `npm run build`; publish `dist/frontend/browser`; production `apiUrl` unchanged.
- **Stitch:** Layout reference from local `stitch/jobs_listing_page` and `jobs_listing_empty_state` (not committed).
