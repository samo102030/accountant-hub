# Tech Stack — Accountant Hub

**Single source of truth for pinned versions.**

---

## Runtimes

| Technology | Version | Notes |
|------------|---------|-------|
| Node.js | v22.22.0 | LTS at install time |
| .NET SDK | 8.0.421 | Pinned via `global.json` |

---

## Frontend (`/frontend`)

| Package | Version |
|---------|---------|
| Angular | 21.2.0 |
| Angular CLI | 21.2.14 |
| Tailwind CSS | 4.3.0 |
| PrimeNG | 21.1.9 |
| @primeng/themes | 21.0.4 |
| Icons | Material Symbols Outlined (Google Fonts) |

---

## Backend (`/backend`)

| Package | Version |
|---------|---------|
| .NET | 8.0 LTS |
| Entity Framework Core | 8.0.11 |
| Npgsql.EntityFrameworkCore.PostgreSQL | 8.0.11 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.11 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.11 |
| System.IdentityModel.Tokens.Jwt | 8.0.2 |
| Swashbuckle.AspNetCore | 6.6.2 |

---

## Database & Hosting

| Service | Role |
|---------|------|
| PostgreSQL | Railway-managed database |
| Netlify | Angular static site |
| Railway | .NET API (Dockerfile in `backend/`) |

---

## Notes

- Angular CLI 22 requires Node ≥22.22.3; this project uses Angular 21 on Node 22.22.0.
- `@primeng/themes` shows an npm deprecation notice; kept for Angular 21 compatibility.
