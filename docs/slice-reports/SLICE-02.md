# Slice Report — SLICE-02

## Summary

Slice 2 adds Railway PostgreSQL via `DATABASE_URL`, EF Core migrations with startup migrate + seed (4 categories, 10 jobs), `GET /api/jobs` with filters, sort, and pagination, CORS for localhost and `*.netlify.app`, and the jobs listing UI (PrimeNG + Tailwind + Material Symbols) aligned with local Stitch guides. Production API: `https://accountant-hub-production-cc2a.up.railway.app`.

Post-merge deploy fixes on `main`: removed invalid `backend/railway.toml` (`deploy.runtime = ["dotnet"]`), added `backend/Dockerfile` for monorepo build when Railway root is `backend/`.

## Files created

**Backend**

- `backend/AccountantHub.Infrastructure/DatabaseConnection.cs`
- `backend/AccountantHub.Infrastructure/DependencyInjection.cs`
- `backend/AccountantHub.Infrastructure/Persistence/` — `AppDbContext`, `DbSeeder`, entities, `Migrations/InitialCreate`
- `backend/AccountantHub.API/Features/Jobs/` — `JobsController`, DTOs, query model
- `backend/Dockerfile`, `backend/.dockerignore`

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

## Files removed (deploy fix)

- `backend/railway.toml` — invalid `deploy.runtime = ["dotnet"]` blocked GitHub/Railway deploys

## Endpoints added

| Method | Path | Auth | Query params |
|--------|------|------|----------------|
| GET | `/api/jobs` | No | `search`, `category`, `budgetMin`, `budgetMax`, `sort` (`newest`, `budget_asc`, `budget_desc`, `title_asc`), `page`, `pageSize` |

**Not exposed (by design):** `GET /` — no root route; API-only host. Use `/api/health`, `/api/jobs`, `/swagger`.

## Database changes

- Tables: `Categories`, `Jobs`
- Migration: `20260605012634_InitialCreate`
- Seed: 4 categories (Taxation, Audit, Consulting, Bookkeeping), 10 jobs
- Applied automatically on API startup: `Database.Migrate()` + `DbSeeder.SeedAsync()` when the app starts with a valid connection string

## Railway — final settings (API service)

| Setting | Value |
|---------|--------|
| **Root Directory** | `backend` |
| **Build** | Dockerfile (`backend/Dockerfile`) — `dotnet publish AccountantHub.API/AccountantHub.API.csproj` |
| **Start** | `dotnet AccountantHub.API.dll` (via Docker `ENTRYPOINT`) |
| **Port** | Railway `PORT` (typically **8080**); `ASPNETCORE_URLS=http://0.0.0.0:8080` in Dockerfile |
| **Branch** | `main` (auto-deploy on push) |
| **`DATABASE_URL`** | `${{Postgres.DATABASE_PUBLIC_URL}}` on API service (not `postgres.railway.internal` for local dev) |
| **Postgres** | Railway plugin linked; migrate + seed on API boot |

**Do not** restore `backend/railway.toml` with `runtime = ["dotnet"]` — Railway expects `UNSPECIFIED` \| `LEGACY` \| `V2`.

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | **green** — `/api/health` 200, `/api/jobs` 200 (seeded jobs), Swagger includes **Jobs** |
| Railway root `/` | Same domain | **404 expected** — no homepage route; not required for Slice 2 |
| Netlify (Angular) | Dashboard production URL | User-verified **green** — jobs UI + data from production API |

## Merge status

- `feature/slice-2` merged to `main` (PR #6)
- Follow-up on `main`: remove invalid `railway.toml`; add `Dockerfile` for deploy
- Merge to `main` for assessment: **done** — user merged; Agent did not merge Slice 2 initially

## How to verify

**Live (production)**

1. `GET https://accountant-hub-production-cc2a.up.railway.app/api/health` — 200, `{ success: true, ... }`
2. `GET https://accountant-hub-production-cc2a.up.railway.app/api/jobs?page=1&pageSize=10` — 200, `meta.total` = 10
3. `https://accountant-hub-production-cc2a.up.railway.app/swagger` — `GET /api/Health`, `GET /api/Jobs`
4. `GET https://accountant-hub-production-cc2a.up.railway.app/` — **404** (normal for API-only)
5. Netlify production URL — jobs listing with cards, filters, empty state when no matches

**Local**

1. `DATABASE_URL` (public Railway URL) or local Postgres + `appsettings.json`
2. `cd backend/AccountantHub.API && dotnet run` — migrate + seed
3. `http://localhost:5080/api/jobs` — JSON with 10 jobs
4. `cd frontend && npm start` — UI at `http://localhost:4200`

## Known issues / blockers

1. **Root URL 404:** Not a bug — no `MapGet("/")` in `Program.cs`. Frontend is on Netlify; API paths are under `/api/*` and `/swagger`.
2. **Netlify URL:** Not committed; confirm in dashboard. Custom domains need explicit CORS if not `*.netlify.app`.
3. **Bundle size:** Initial chunk ~754 kB (PrimeNG); budget cap 800 kB / 1.5 MB in `angular.json`.
4. **`@primeng/themes`:** npm deprecation notice; kept for Angular 21 compatibility this slice.
5. **Job card chevron:** No navigation until Slice 3 (`GET /api/jobs/:id` + details page).

## Netlify notes

- Base directory: `frontend`
- Build: `npm run build`
- Publish: `dist/frontend/browser`
- `environment.production.ts` → `apiUrl: 'https://accountant-hub-production-cc2a.up.railway.app'`

## Stitch

Layout reference: local `stitch/jobs_listing_page`, `jobs_listing_empty_state` (gitignored).
