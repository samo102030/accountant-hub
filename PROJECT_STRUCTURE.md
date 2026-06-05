# Project Structure — Accountant Hub

**Simple modular monolith** — not full Clean Architecture. Enough structure for a clear assessment, no extra layers.

Update **API Endpoints** and **Database Schema** at end of **Slice 2, 4, and 6** only.

---

## Repository layout

```
accountant-hub/
├── .github/workflows/          # optional CI checks
├── frontend/                   # Angular (project name: frontend)
│   ├── src/app/
│   │   ├── core/               # services, models, interceptors (Slice 4+)
│   │   ├── shared/             # job-card, empty-state, pagination, app-header
│   │   └── features/
│   │       ├── jobs/           # jobs-list (Slice 2), details (Slice 3)
│   │       ├── auth/           # login + register (Slice 4)
│   │       └── my-bids/        # dashboard (Slice 6)
│   ├── netlify.toml            # publish path must match actual dist/ after build
│   └── angular.json
├── backend/
│   ├── AccountantHub.API/      # HTTP entry, controllers, DTOs, middleware
│   │   └── Features/
│   │       ├── Jobs/           # JobsController, DTOs
│   │       ├── Auth/           # Slice 4+
│   │       └── Bids/           # Slice 5+
│   └── AccountantHub.Infrastructure/
│       ├── Persistence/        # DbContext, entities, migrations, seed
│       └── Identity/           # Identity + JWT setup (Slice 4+)
├── docs/
│   └── slice-reports/          # SLICE-01.md … SLICE-06.md (one per completed slice)
├── TECH_STACK.md
├── PROJECT_STRUCTURE.md
├── accountant-hub-prompt.md
└── README.md
```

---

## Responsibilities

| Part | Role |
|------|------|
| **AccountantHub.API** | Controllers, request/response DTOs, Swagger, CORS, exception middleware, `/api/health` |
| **AccountantHub.API/Features/** | Group code by feature (Jobs, Auth, Bids) — not separate Application/Domain projects |
| **AccountantHub.Infrastructure** | EF Core, PostgreSQL, DbContext, migrations, seed data |
| **frontend/core** | API services, models; `AuthService`, interceptor, guards in Slice 4+ |
| **frontend/shared** | Reusable UI: job-card, empty-state, pagination, app-header |
| **frontend/features** | One folder per page/flow |

---

## API Endpoints

_Updated end of Slice 2, 4, 6._

| Method | Path | Auth | Slice | Description |
|--------|------|------|-------|-------------|
| GET | `/api/health` | No | 1 | Health check |
| GET | `/api/jobs` | No | 2 | List jobs — query: `search`, `category`, `budgetMin`, `budgetMax`, `sort`, `page`, `pageSize` |

**`GET /api/jobs` response**

```json
{
  "success": true,
  "message": "OK",
  "data": [ { "id", "title", "description", "companyName", "category", "categorySlug", "budgetMin", "budgetMax", "status", "createdAt", "tags", "bidCount" } ],
  "meta": { "total", "page", "pageSize" }
}
```

**Sort values:** `newest` (default), `budget_asc`, `budget_desc`, `title_asc`

---

## Database Schema

_Updated end of Slice 2, 4, 6._

| Table | Slice | Notes |
|-------|-------|-------|
| `Categories` | 2 | `Id`, `Name`, `Slug` — seeded: Taxation, Audit, Consulting, Bookkeeping |
| `Jobs` | 2 | FK `CategoryId`; budget range, status, tags, `BidCount` (display until Slice 5) |

**Seed (Slice 2):** 4 categories, 10 jobs. Migrations + seed run on API startup when `DATABASE_URL` is set (Railway).

---

## Changelog

| Slice | Changes |
|-------|---------|
| 1 | Initial structure (pragmatic 2-project backend), health + deploy |
| 2 | PostgreSQL + EF, jobs API, jobs listing UI (PrimeNG + Tailwind) |
