# Accountant Hub

A job marketplace for accountants — browse seeded accounting jobs, view details, register/login, submit bids, and track proposals on a **My Bids** dashboard.

Built as a **48-hour assessment** project: working features, live deploy, clean readable code — not a full production product.

## Live URLs

| Service | URL |
|---------|-----|
| **Frontend (Netlify)** | Deploy from `main` — confirm URL in your Netlify dashboard |
| **API (Railway)** | https://accountant-hub-production-cc2a.up.railway.app |
| **Swagger UI** | https://accountant-hub-production-cc2a.up.railway.app/swagger |

## Test credentials

No pre-seeded users. Register an accountant on the live site or via API:

```bash
curl -X POST https://accountant-hub-production-cc2a.up.railway.app/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"fullName":"Demo Accountant","email":"demo@firm.com","password":"DemoPass123"}'
```

Then login with the same email/password. Use the returned JWT for protected endpoints (bid submit, My Bids).

**Sample seeded jobs:** 10 jobs across 4 categories (Taxation, Audit, Consulting, Bookkeeping). Job id **9** is **Closed** (cannot bid).

## Why this stack

| Layer | Choice | Rationale |
|-------|--------|-----------|
| **Frontend** | Angular 21 (standalone) | Structured SPA, strong forms/guards for auth flows |
| **UI** | PrimeNG + Tailwind | Accessible controls + layout tokens; Stitch HTML guides (local) for visual reference |
| **Backend** | **.NET 8 LTS** Web API | Stable LTS on Railway, Swashbuckle/Swagger out of the box |
| **Database** | PostgreSQL (Railway) | Relational fit for jobs, categories, bids, unique (user, job) |
| **Auth** | ASP.NET Identity + JWT | Standard for .NET; 24h bearer tokens, no cookie redirect issues |
| **Deploy** | Netlify + Railway | Zero-config static + API hosting from GitHub |

**Scope note:** UI follows the local **Stitch** design system (not committed). Jobs are **database-seeded** — there is no client “Post Job” flow; companies posting jobs is narrative only.

## Features

- **Jobs listing** — search, category/budget filters, sort, pagination, empty state when no matches
- **Job details** — full description, skills, budget, bid count; Login to Apply when logged out
- **Bid submission** — one bid per user per job; already-bid and closed-job states
- **Auth** — register, login, logout (client-side token clear)
- **My Bids** — dashboard of submitted proposals with stats (JWT required)

## API endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/health` | No | Health check |
| GET | `/api/jobs` | No | List jobs — `search`, `category`, `budgetMin`, `budgetMax`, `sort`, `page`, `pageSize` |
| GET | `/api/jobs/{id}` | Optional JWT | Job detail; `data.userBid` when authenticated |
| POST | `/api/auth/register` | No | Register — `fullName`, `email`, `password` (min 8) |
| POST | `/api/auth/login` | No | Login — `email`, `password` |
| POST | `/api/jobs/{id}/bids` | JWT | Submit bid |
| GET | `/api/my-bids` | JWT | Current user's bids — `page`, `pageSize` |

**Response envelope:** `{ success, message, data, meta }` — list endpoints include `meta: { total, page, pageSize }`.

**Sort values:** `newest` (default), `budget_asc`, `budget_desc`, `title_asc`

Authorize in Swagger with `Bearer <token>` for protected routes.

## Local setup

### Prerequisites

- Node.js 22 LTS
- .NET 8 SDK
- PostgreSQL (local or Railway `DATABASE_URL`)

### Backend

```bash
cd backend/AccountantHub.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=accountant_hub;Username=postgres;Password=yourpassword"
dotnet user-secrets set "Jwt:Key" "your-local-dev-secret-at-least-32-chars-long"
dotnet run
```

API: http://localhost:5080 — Swagger: http://localhost:5080/swagger

Migrations and seed run on startup when the database is reachable.

### Frontend

```bash
cd frontend
npm install
npm start
```

App: http://localhost:4200 — `environment.ts` points to `http://localhost:5080`.

### Production build

```bash
cd frontend && npm run build    # output: dist/frontend/browser
cd backend && dotnet build
```

## Security

- **Secrets** (`JWT_SECRET`, `DATABASE_URL`) live only in Railway env vars or `dotnet user-secrets` — never in git
- JWT access tokens: **24 hours**, stored in `localStorage` on the client
- CORS: `localhost:4200` and `*.netlify.app`
- Global exception handler hides stack traces in production
- Validation via Data Annotations; duplicate bid returns **409**

## Project layout

See [PROJECT_STRUCTURE.md](./PROJECT_STRUCTURE.md) for folder map, schema, and slice changelog.

Slice completion reports: [docs/slice-reports/](./docs/slice-reports/).

## Assumptions

- **Accountant-facing only** — no company/admin auth or job posting UI
- **Bid status** on My Bids is derived from job state (`Pending` / `Closed`); no accept/reject workflow in scope
- **Attachments** on job details are placeholder only
- **English only** in UI templates
