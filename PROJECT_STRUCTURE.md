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
│   │   ├── core/               # auth, interceptor, guards, api config
│   │   ├── shared/             # job-card, bid-form, pagination, empty-state
│   │   └── features/
│   │       ├── jobs/           # list + details
│   │       ├── auth/           # login + register
│   │       └── my-bids/
│   ├── netlify.toml            # publish path must match actual dist/ after build
│   └── angular.json
├── backend/
│   ├── AccountantHub.API/      # HTTP entry, controllers, DTOs, middleware
│   │   └── Features/
│   │       ├── Jobs/
│   │       ├── Auth/
│   │       └── Bids/
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
| **AccountantHub.Infrastructure** | EF Core, PostgreSQL, repositories or direct DbContext, Identity, seed data |
| **frontend/core** | `AuthService`, HTTP interceptor, guards, `environment.apiUrl` |
| **frontend/shared** | Reusable UI components |
| **frontend/features** | One folder per page/flow |

---

## API Endpoints

_Updated end of Slice 2, 4, 6._

| Method | Path | Auth | Slice | Description |
|--------|------|------|-------|-------------|
| GET | `/api/health` | No | 1 | Health check |

---

## Database Schema

_Updated end of Slice 2, 4, 6._

| Table | Slice | Notes |
|-------|-------|-------|
| — | — | — |

---

## Changelog

| Slice | Changes |
|-------|---------|
| — | Initial structure (pragmatic 2-project backend) |
