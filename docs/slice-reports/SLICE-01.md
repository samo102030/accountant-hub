# Slice Report — SLICE-01

## Summary

Slice 1 skeleton: monorepo with `backend/` (.NET 8 API + Infrastructure stub) and `frontend/` (Angular 21 bare shell), `GET /api/health`, Swashbuckle at `/swagger`, `global.json`, `netlify.toml` with verified publish path `dist/frontend/browser`, and pinned versions in `TECH_STACK.md`. Live deploy verified on Netlify (Angular) and Railway (API).

## Files created

- `global.json`
- `README.md` (stub)
- `backend/AccountantHub.sln`
- `backend/AccountantHub.API/` (Program.cs, HealthController, Features placeholders, Swashbuckle)
- `backend/AccountantHub.Infrastructure/` (stub)
- `frontend/` (Angular 21 app, `netlify.toml`, environments, `core/` / `shared/` / `features/` placeholders)
- `docs/slice-reports/SLICE-01.md`

## Files modified

- `.gitignore` — removed erroneous `docs/slice-reports/` ignore (reports must be committed)
- `TECH_STACK.md` — resolved versions pinned
- `frontend/src/environments/environment.production.ts` — Railway API URL

## Endpoints added

| Method | Path | Auth |
|--------|------|------|
| GET | `/api/health` | No |

## Database changes

None (Slice 1).

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Netlify (Angular) | Production site (Netlify dashboard) | green |
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | green |

## Merge status

- Branch: `feature/slice-1` (pushed to `origin`)
- Merge to `main`: **pending user approval** — Agent did not merge

## How to verify

**Local**

1. `cd backend && dotnet build` — 0 errors  
2. `cd backend/AccountantHub.API && dotnet run` → open `http://localhost:5080/api/health` (200, JSON wrapper)  
3. `http://localhost:5080/swagger` — Swagger UI loads  
4. `cd frontend && npm run build` — output under `frontend/dist/frontend/browser`  
5. `cd frontend && npm start` — “Accountant Hub” shell visible  

**Live**

1. Netlify production URL — Accountant Hub shell (not 404/blank)  
2. https://accountant-hub-production-cc2a.up.railway.app/api/health — HTTP 200, JSON `{ success: true, ... }`  
3. https://accountant-hub-production-cc2a.up.railway.app/swagger — Swagger UI with `GET /api/Health`  
4. Root `/` on Railway returns 404 — expected (no root route in Slice 1)

## Slice 1 checklist (Slice 1 only)

- [x] Health endpoint `GET /api/health` returns **200** on Railway
- [x] Swagger UI at `/swagger` is **working** on Railway
- [x] Angular app on Netlify URL is **working** (not 404/blank)
- [x] Railway deployment status = **green** / successful
- [x] Netlify deployment status = **green** / published
- [x] TECH_STACK.md updated with real pinned versions

## Known issues

1. **Angular version:** Using **21.2.x** (not 22) for Node **22.22.0** compatibility (CLI 22 requires ≥22.22.3).
2. **Railway root `/`:** Returns 404 — only `/api/health` and `/swagger` are exposed in Slice 1.
3. **Railway deploy:** Root directory `backend/AccountantHub.API`; Railpack .NET build (no Dockerfile).

## Railway / Netlify notes

- **Netlify:** Base `frontend` (or root `netlify.toml` with `base = "frontend"`); build `npm run build`; publish `dist/frontend/browser`; Runtime **Not set** (disable Angular plugin for monorepo).
- **Railway:** Root `backend/AccountantHub.API`; .NET 8; public port **8080**; no Postgres / `DATABASE_URL` in Slice 1.
- **Swagger:** Enabled in all environments in `Program.cs`.
