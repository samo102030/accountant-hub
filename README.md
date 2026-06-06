# Accountant Hub

A job marketplace where accountants browse accounting jobs, view details, submit bids, and track proposals on a **My Bids** dashboard.

Companies post jobs via seeded data only — there is no client “Post Job” flow in this build.

## Deliverables

| Item | URL |
|------|-----|
| **GitHub** | https://github.com/samo102030/accountant-hub |
| **Live demo (Netlify)** | https://accountant-hub.netlify.app/ |
| **API / Swagger** | https://accountant-hub-production-cc2a.up.railway.app/swagger |

**Test login:** `demo@accountanthub.com` / `DemoPass123!`

The demo accountant is created automatically on API startup if it does not already exist.

## Project overview

Accountant Hub is a full-stack web app for accountants to:

- Browse and filter open accounting jobs (search, category, budget, sort, pagination)
- View job details (description, skills/tags, budget, deadline, expected delivery, bid count)
- Register, login, and logout
- Submit one bid per job (proposed price, delivery days, cover letter, experience summary)
- View a read-only **My Proposal** page after bidding (`/jobs/:id/my-proposal`)
- Track all submissions on **My Bids**

**Seeded data:** 40 jobs across 4 categories (Taxation, Audit, Consulting, Bookkeeping). Some jobs are **Closed** and do not accept new bids.

## Tech stack

| Layer | Choice |
|-------|--------|
| **Frontend** | Angular 21 (standalone), PrimeNG, Tailwind CSS 4, Material Symbols Outlined |
| **Backend** | .NET 8 LTS Web API, EF Core, PostgreSQL |
| **Auth** | ASP.NET Identity + JWT (24h bearer token, `localStorage` on client) |
| **Deploy** | Netlify (frontend) + Railway (API + PostgreSQL) |

Pinned package versions: [TECH_STACK.md](./TECH_STACK.md).

## Technology Stack Decision

The assessment allowed the use of an alternative technology stack when justified.
For this submission, I chose **Angular 21**, **ASP.NET Core (.NET 8)**, and
**PostgreSQL** — my primary professional stack and the technologies I have the most
production experience with.

Working within the limited timeframe, this let me focus on delivering a complete,
well-tested, and deployed solution — with proper documentation, deployment pipelines,
and full business-requirements coverage — rather than spending time adapting to an
unfamiliar stack. Beyond familiarity, the stack also fits the problem well:

- **Angular** — structured SPA with strong forms, routing guards, and JWT interceptors
- **.NET 8 LTS** — long-term support, first-class Swagger, and built-in Identity + JWT
- **PostgreSQL** — relational model fits jobs, categories, bids, and the unique (user, job) constraint

## Features

- **Jobs listing** — category and deadline on each card, posted date, bid count, Open/Closed status, filters, sort, pagination, empty state
- **Job details** — full description, company, category, skills (tags), budget, expected delivery days, deadline, attachment placeholders, bid count
- **Bid submission** — authenticated form; duplicate bid blocked (409); closed jobs blocked
- **My Proposal** — read-only view of submitted bid (no edit/withdraw)
- **My Bids** — dashboard with stats; links to job details and my proposal
- **Auth** — register, login, logout (client-side token clear)

## API endpoints

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/health` | No | Health check |
| GET | `/api/jobs` | No | List jobs — `search`, `category`, `budgetMin`, `budgetMax`, `sort`, `page`, `pageSize` |
| GET | `/api/jobs/{id}` | Optional JWT | Job detail; includes `deadline`, `expectedDeliveryDays`; `data.userBid` when authenticated |
| POST | `/api/auth/register` | No | Register — `fullName`, `email`, `password` (min 8) |
| POST | `/api/auth/login` | No | Login — `email`, `password` |
| POST | `/api/jobs/{id}/bids` | JWT | Submit bid |
| GET | `/api/my-bids` | JWT | Current user's bids — `page`, `pageSize` |

**Response envelope:** `{ success, message, data, meta }` — list endpoints include `meta: { total, page, pageSize }`.

**Sort values:** `newest` (default), `budget_asc`, `budget_desc`, `title_asc`

**Frontend routes (selected):** `/`, `/jobs/:id`, `/jobs/:id/bid`, `/jobs/:id/my-proposal`, `/my-bids`, `/login`, `/register`

Authorize in Swagger with `Bearer <token>` for protected routes.

## Setup instructions

### Prerequisites

- Node.js 22 LTS
- .NET 8 SDK
- PostgreSQL (local instance or Railway `DATABASE_URL`)

### Backend

```bash
cd backend/AccountantHub.API
```

**Option A — local Development config** (gitignored `appsettings.Development.json` with Postgres + JWT for dev):

```bash
dotnet run
```

**Option B — user secrets** (if not using Development config):

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=accountant_hub;Username=postgres;Password=yourpassword"
dotnet user-secrets set "Jwt:Key" "your-local-dev-secret-at-least-32-chars-long"
dotnet run
```

**Option C — Railway database locally:**

```bash
# set DATABASE_URL to your Railway public Postgres URL
dotnet run
```

API: http://localhost:5080 — Swagger: http://localhost:5080/swagger

On startup the API runs migrations, seeds categories/jobs (up to 40), and creates the demo user if missing.

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

### Production environment (Railway)

Set on the API service:

- `DATABASE_URL` — PostgreSQL connection (from Railway plugin)
- `JWT_SECRET` — at least 32 characters

After deploy, restart the API once so migrations, job seed, and demo user seed run.

## Security

- **Secrets** (`JWT_SECRET`, `DATABASE_URL`) live only in Railway env vars, local `appsettings.Development.json` (gitignored), or `dotnet user-secrets` — never committed to git
- JWT access tokens: **24 hours**, stored in `localStorage` on the client
- CORS: `localhost:4200` and `*.netlify.app`
- Global exception handler hides stack traces in production
- Validation via Data Annotations; duplicate bid returns **409**

## Project layout

See [PROJECT_STRUCTURE.md](./PROJECT_STRUCTURE.md) for folder map, schema, and API details.

## Assumptions

- **Accountant-facing only** — no company/admin auth or job posting UI; jobs are database-seeded
- **One bid per user per job** — enforced by unique index and API (409 on duplicate)
- **Bid status** on My Bids is derived from job state (`Pending` when job is Open, `Closed` when job is Closed); no accept/reject workflow
- **My Proposal** is read-only — no edit or withdraw after submit
- **Attachments** on job details are UI placeholders only (no file upload or storage)
- **`expectedDeliveryDays`** on job detail is computed from `deadline` and `createdAt` in the API (not a separate database column)
- **English only** in UI templates
