# Tech Stack — Accountant Hub

**Single source of truth for versions.** This is a **48-hour assessment task** — prefer stable LTS and **latest stable at install**, then pin exact numbers here.

**Before Slice 1:** run `node -v`, `dotnet --version`, `npm view @angular/core version` and record results below. Do not install fictional version numbers.

---

## Runtimes

| Technology | Target | Notes |
|------------|--------|-------|
| Node.js | Latest LTS at install | Pin exact version after install |
| .NET SDK | **8.0.x LTS** | Use `global.json` to pin; stable on Railway + EF |

---

## Frontend (`/frontend`)

| Package | Target | When |
|---------|--------|------|
| Angular | **Latest stable** at `ng new` | Pin exact version after install — do not assume a specific major |
| Angular CLI | Matches Angular | Slice 1 |
| Tailwind CSS | 4.x (or latest stable 4) | Slice 2 |
| PrimeNG | Latest compatible with installed Angular | Slice 2 |
| Icons | **Material Symbols Outlined** (Google Fonts) | Slice 2 — not PrimeIcons |

---

## Backend (`/backend`)

| Package | Target | When |
|---------|--------|------|
| .NET | **8.0 LTS** | Slice 1 |
| Entity Framework Core | 8.x | Slice 2 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.x | Slice 2 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.x | Slice 4 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.x | Slice 4 |
| Swashbuckle.AspNetCore | Latest stable for .NET 8 | Slice 1 — Swagger UI at `/swagger` |

---

## Database & Hosting

| Service | Notes |
|---------|-------|
| PostgreSQL | Railway-managed — connect in **Slice 2** |
| Netlify | Angular static site |
| Railway | .NET API |

---

## Resolved versions (fill after install — Slice 1 / 2)

```
Node.js:          v22.22.0
Angular:          21.2.0
Angular CLI:      21.2.14
.NET SDK:         8.0.421
Swashbuckle:      6.6.2
PrimeNG:          (Slice 2)
Tailwind CSS:     (Slice 2)
Material Symbols: (Slice 2)
PostgreSQL:       (Slice 2 — from Railway dashboard)
```

Note: Angular CLI 22 requires Node ≥22.22.3; this repo uses Angular 21 on Node 22.22.0.
