# Project Structure — Accountant Hub

**Simple modular monolith** — API + Infrastructure projects only (no separate Application/Domain layers).

---

## Repository layout

```
accountant-hub/
├── frontend/                   # Angular (project name: frontend)
│   ├── src/app/
│   │   ├── core/               # services, models, interceptors, guards
│   │   ├── shared/             # job-card, empty-state, pagination, app-header, bid-form
│   │   └── features/
│   │       ├── jobs/           # jobs-list, job-details, bid-submission, my-proposal
│   │       ├── auth/           # login + register
│   │       └── my-bids/        # dashboard
│   ├── netlify.toml
│   └── angular.json
├── backend/
│   ├── Dockerfile              # Railway monorepo build
│   ├── AccountantHub.API/      # HTTP entry, controllers, DTOs
│   │   └── Features/
│   │       ├── Jobs/
│   │       ├── Auth/
│   │       └── Bids/
│   └── AccountantHub.Infrastructure/
│       ├── Persistence/        # DbContext, entities, migrations, seed, DemoUserSeeder
│       └── Identity/           # Identity + JWT token service
├── TECH_STACK.md
├── PROJECT_STRUCTURE.md
├── global.json
└── README.md
```

---

## Responsibilities

| Part | Role |
|------|------|
| **AccountantHub.API** | Controllers, DTOs, Swagger, CORS, `/api/health` |
| **AccountantHub.API/Features/** | Jobs, Auth, Bids grouped by feature |
| **AccountantHub.Infrastructure** | EF Core, PostgreSQL, migrations, job + demo user seed |
| **Infrastructure/Identity** | ASP.NET Identity, JWT generation |
| **frontend/core** | `JobsService`, `BidsService`, `AuthService`, interceptor, guards |
| **frontend/shared** | job-card, bid-form, pagination, empty-state, app-header |
| **frontend/features** | One folder per page/flow |

---

## API Endpoints

| Method | Path | Auth | Module | Description |
|--------|------|------|--------|-------------|
| GET | `/api/health` | No | Core | Health check |
| GET | `/api/jobs` | No | Jobs | List — `search`, `category`, `budgetMin`, `budgetMax`, `sort`, `page`, `pageSize` |
| GET | `/api/jobs/{id}` | Optional JWT | Jobs | Detail; `deadline`, `expectedDeliveryDays`; `userBid` when Bearer present |
| POST | `/api/auth/register` | No | Auth | Register — returns JWT |
| POST | `/api/auth/login` | No | Auth | Login — returns JWT |
| POST | `/api/jobs/{id}/bids` | JWT | Bids | Submit bid |
| GET | `/api/my-bids` | JWT | Bids | Current user's bids — `page`, `pageSize` |

**`GET /api/jobs` response**

```json
{
  "success": true,
  "message": "OK",
  "data": [
    {
      "id", "title", "description", "companyName", "category", "categorySlug",
      "budgetMin", "budgetMax", "status", "createdAt", "deadline", "tags", "bidCount"
    }
  ],
  "meta": { "total", "page", "pageSize" }
}
```

**`GET /api/jobs/{id}` response** — includes `deadline`, `expectedDeliveryDays` (computed from deadline − createdAt); optional `userBid` when authenticated.

**`GET /api/my-bids` response**

```json
{
  "success": true,
  "message": "OK",
  "data": [
    {
      "id", "jobId", "jobTitle", "companyName", "jobStatus", "category",
      "proposedPrice", "deliveryDays", "createdAt", "status"
    }
  ],
  "meta": { "total", "page", "pageSize" }
}
```

Bid `status` is derived: `Pending` when job is Open, `Closed` when job is Closed.

**Sort values:** `newest` (default), `budget_asc`, `budget_desc`, `title_asc`

---

## Database Schema

| Table | Notes |
|-------|-------|
| `Categories` | `Id`, `Name`, `Slug` — Taxation, Audit, Consulting, Bookkeeping |
| `Jobs` | FK `CategoryId`; `BudgetMin`/`BudgetMax`, `Status`, `Tags`, `BidCount`, `Deadline`, `CreatedAt` |
| `Bids` | FK `JobId`, `UserId`; unique `(UserId, JobId)`; price, delivery, cover letter, experience |
| `AspNetUsers` | Identity — `FullName`, `Email`, etc. |
| `AspNetRoles` + related Identity tables | Standard ASP.NET Identity schema |

**Seed on startup:** 4 categories, up to 40 jobs, demo user `demo@accountanthub.com` (if not exists).

**Auth:** JWT via Railway `JWT_SECRET` (not in git). 24h access token; client uses `localStorage`.

---

## Changelog

| Phase | Changes |
|-------|---------|
| Foundation | Monorepo, health API, Angular shell, Netlify + Railway deploy |
| Jobs | PostgreSQL, EF migrations, jobs API, listing UI (filters, pagination) |
| Job details | Single job API, details page, attachment placeholders |
| Auth | Identity + JWT, login/register, guards, interceptor |
| Bids | Bids table, submit bid API, bid form, already-bid state |
| Dashboard | My Bids API + UI, mobile nav, jobs polish |
| Task compliance | Job `Deadline`, category on cards, delivery/deadline on details |
| My Proposal | Read-only proposal page, My Bids navigation links |
