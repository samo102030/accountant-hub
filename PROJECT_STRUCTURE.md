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
│   │   ├── core/               # services, models, interceptors, guards
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
│   │       ├── Auth/           # AuthController, login/register DTOs (Slice 4)
│   │       └── Bids/           # Slice 5+
│   └── AccountantHub.Infrastructure/
│       ├── Persistence/        # DbContext, entities, migrations, seed
│       └── Identity/           # Identity + JWT token service (Slice 4)
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
| **Infrastructure/Identity** | ASP.NET Identity registration, JWT token generation |
| **frontend/core** | API services, models; `AuthService`, `authInterceptor`, `authGuard`, `guestGuard` |
| **frontend/shared** | Reusable UI: job-card, empty-state, pagination, app-header |
| **frontend/features** | One folder per page/flow |

---

## API Endpoints

_Updated end of Slice 2, 4, 6._

| Method | Path | Auth | Slice | Description |
|--------|------|------|-------|-------------|
| GET | `/api/health` | No | 1 | Health check |
| GET | `/api/jobs` | No | 2 | List jobs — query: `search`, `category`, `budgetMin`, `budgetMax`, `sort`, `page`, `pageSize` |
| GET | `/api/jobs/{id}` | No | 3 | Single job by id; **404** if not found |
| POST | `/api/auth/register` | No | 4 | Register accountant — returns JWT |
| POST | `/api/auth/login` | No | 4 | Login — returns JWT |

**`GET /api/jobs` response**

```json
{
  "success": true,
  "message": "OK",
  "data": [ { "id", "title", "description", "companyName", "category", "categorySlug", "budgetMin", "budgetMax", "status", "createdAt", "tags", "bidCount" } ],
  "meta": { "total", "page", "pageSize" }
}
```

**`GET /api/jobs/{id}` response** — same job fields as list item in `data`; `meta` is `null`.

**`POST /api/auth/register` / `POST /api/auth/login` success**

```json
{
  "success": true,
  "message": "OK",
  "data": { "token", "email", "fullName" },
  "meta": null
}
```

**Auth errors:** **400** validation, **401** invalid login, **409** duplicate email on register.

**Sort values:** `newest` (default), `budget_asc`, `budget_desc`, `title_asc`

**Swagger:** Bearer JWT — use **Authorize** with `Bearer <token>` for protected endpoints (Slice 5+).

---

## Database Schema

_Updated end of Slice 2, 4, 6._

| Table | Slice | Notes |
|-------|-------|-------|
| `Categories` | 2 | `Id`, `Name`, `Slug` — seeded: Taxation, Audit, Consulting, Bookkeeping |
| `Jobs` | 2 | FK `CategoryId`; budget range, status, tags, `BidCount` (display until Slice 5) |
| `AspNetUsers` | 4 | Identity user — `FullName`, `Email`, `PasswordHash`, etc. |
| `AspNetRoles` | 4 | Identity roles (default schema) |
| `AspNetUserRoles` | 4 | User–role mapping |
| `AspNetUserClaims` | 4 | Per-user claims |
| `AspNetRoleClaims` | 4 | Per-role claims |
| `AspNetUserLogins` | 4 | External logins (unused) |
| `AspNetUserTokens` | 4 | Identity tokens |

**Seed (Slice 2):** 4 categories, 10 jobs. Migrations + seed run on API startup when `DATABASE_URL` is set (Railway).

**Auth (Slice 4):** JWT secret via Railway env `JWT_SECRET` (not in git). Access token lifetime **24 hours**. Client stores token in `localStorage`.

---

## Changelog

| Slice | Changes |
|-------|---------|
| 1 | Initial structure (pragmatic 2-project backend), health + deploy |
| 2 | PostgreSQL + EF, jobs API, jobs listing UI (PrimeNG + Tailwind) |
| 3 | Job details API + page |
| 4 | ASP.NET Identity + JWT auth, login/register UI, interceptor, guards, header logout |
