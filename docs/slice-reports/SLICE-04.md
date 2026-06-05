# Slice Report — SLICE-04

## Summary

Slice 4 adds ASP.NET Identity with JWT authentication: `POST /api/auth/register` and `POST /api/auth/login` (24h access token). Swagger supports Bearer auth. Frontend login and register pages follow local Stitch guides; `AuthService`, HTTP interceptor, `authGuard` / `guestGuard`, and client-side logout in the header. Job details **Login to Apply** navigates to login with `returnUrl`. Production API: `https://accountant-hub-production-cc2a.up.railway.app`. JWT secret must be set as `JWT_SECRET` on the Railway API service (not in git).

## Files created

**Backend**

- `backend/AccountantHub.Infrastructure/Persistence/Entities/ApplicationUser.cs`
- `backend/AccountantHub.Infrastructure/Identity/IJwtTokenService.cs`
- `backend/AccountantHub.Infrastructure/Identity/JwtTokenService.cs`
- `backend/AccountantHub.Infrastructure/Identity/IdentityServiceExtensions.cs`
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/20260605032622_AddIdentity.cs`
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/20260605032622_AddIdentity.Designer.cs`
- `backend/AccountantHub.API/Features/Auth/AuthController.cs`
- `backend/AccountantHub.API/Features/Auth/LoginRequest.cs`
- `backend/AccountantHub.API/Features/Auth/RegisterRequest.cs`
- `backend/AccountantHub.API/Features/Auth/AuthResponseDto.cs`

**Frontend**

- `frontend/src/app/core/models/auth.model.ts`
- `frontend/src/app/core/services/auth.service.ts`
- `frontend/src/app/core/interceptors/auth.interceptor.ts`
- `frontend/src/app/core/guards/auth.guard.ts`
- `frontend/src/app/core/guards/guest.guard.ts`
- `frontend/src/app/features/auth/login/login.component.ts`
- `frontend/src/app/features/auth/login/login.component.html`
- `frontend/src/app/features/auth/register/register.component.ts`
- `frontend/src/app/features/auth/register/register.component.html`

**Docs**

- `docs/slice-reports/SLICE-04.md`

## Files modified

- `backend/AccountantHub.API/Program.cs` — JWT Bearer, Swagger security, `UseAuthentication`
- `backend/AccountantHub.API/appsettings.json` — dev-only `Jwt` section (overridden by `JWT_SECRET` on Railway)
- `backend/AccountantHub.API/AccountantHub.API.csproj` — `Microsoft.AspNetCore.Authentication.JwtBearer`
- `backend/AccountantHub.Infrastructure/AccountantHub.Infrastructure.csproj` — Identity EF, JWT tokens, `Microsoft.AspNetCore.App` framework ref
- `backend/AccountantHub.Infrastructure/DependencyInjection.cs` — `AddIdentityServices`
- `backend/AccountantHub.Infrastructure/Persistence/AppDbContext.cs` — `IdentityDbContext<ApplicationUser>`
- `backend/AccountantHub.Infrastructure/Persistence/Migrations/AppDbContextModelSnapshot.cs` — Identity tables
- `frontend/src/app/app.config.ts` — `authInterceptor`
- `frontend/src/app/app.routes.ts` — `/login`, `/register` with `guestGuard`
- `frontend/src/app/shared/components/app-header/app-header.component.ts` — auth state + logout
- `frontend/src/app/shared/components/app-header/app-header.component.html` — Login/Register links or user + Logout
- `frontend/src/app/features/jobs/job-details/job-details.component.ts` — `loginToApply()`, auth awareness
- `frontend/src/app/features/jobs/job-details/job-details.component.html` — Login to Apply CTA / Apply placeholder
- `PROJECT_STRUCTURE.md` — API endpoints + database schema (Slice 4)
- `TECH_STACK.md` — pinned Identity/JWT package versions

## Endpoints added

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/register` | No | Register — body: `fullName`, `email`, `password` (min 8); **409** if email exists |
| POST | `/api/auth/login` | No | Login — body: `email`, `password`; **401** if invalid |

**Success response (register / login)**

```json
{
  "success": true,
  "message": "OK",
  "data": { "token": "<jwt>", "email": "user@firm.com", "fullName": "John Doe" },
  "meta": null
}
```

**Validation error (400)**

```json
{
  "success": false,
  "message": "The Password field must be at least 8 characters.",
  "data": null,
  "meta": null
}
```

**Duplicate email (409)**

```json
{
  "success": false,
  "message": "Email is already registered.",
  "data": null,
  "meta": null
}
```

## Database changes

Migration `AddIdentity` adds ASP.NET Identity tables:

- `AspNetUsers` (with `FullName` column)
- `AspNetRoles`, `AspNetUserRoles`, `AspNetUserClaims`, `AspNetRoleClaims`, `AspNetUserLogins`, `AspNetUserTokens`

Applied automatically on API startup via `db.Database.Migrate()`.

## Deployment status

| Target | URL | Status |
|--------|-----|--------|
| Railway (API) | https://accountant-hub-production-cc2a.up.railway.app | **pending merge** — auth endpoints deploy after `feature/slice-4` merge + redeploy |
| Netlify (Angular) | Dashboard production URL | **pending merge** — login/register UI after branch merge + rebuild |
| Local backend build | `dotnet build` | **green** |
| Local frontend build | `npm run build` → `dist/frontend/browser` | **green** (~813 kB initial) |

**Railway prerequisite:** Set `JWT_SECRET` env var on the API service (min 32 characters recommended). Without it, the API will fail to start after merge.

Railway auto-deploys from **`main`** only. Live auth endpoints require user merge of `feature/slice-4`.

## Merge status

- Branch **`feature/slice-4`** pushed to `origin`
- Merge to `main`: **pending user approval** — Agent did not merge

## How to verify

**After merge to `main` (production)**

1. Confirm `JWT_SECRET` is set on Railway API service
2. `POST https://accountant-hub-production-cc2a.up.railway.app/api/auth/register` with `{ "fullName", "email", "password" }` — **200**, JWT in `data.token`
3. Repeat register with same email — **409**
4. `POST .../api/auth/login` with credentials — **200**, JWT
5. `https://accountant-hub-production-cc2a.up.railway.app/swagger` — **Authorize** with `Bearer <token>`
6. Netlify — `/login`, `/register`; register → redirected home with name in header; logout clears session
7. Job details while logged out — **Login to Apply** → `/login?returnUrl=...` → after login returns to job
8. Job details while logged in — **Apply** shown (disabled until Slice 5)

**Local**

1. `cd backend/AccountantHub.API && dotnet run` (Postgres / `DATABASE_URL`; uses dev `Jwt:Key` from appsettings)
2. `POST http://localhost:5080/api/auth/register` and `/login`
3. `cd frontend && npm start` — `http://localhost:4200/login`, `/register`
4. Header logout clears `localStorage` token

**Pre-merge (current `main`)**

- `POST .../api/auth/register` returns **404** (endpoint not on `main` yet)

## Known issues / blockers

1. **Production auth 404 until merge:** Railway tracks `main`; auth is only on `feature/slice-4`.
2. **JWT_SECRET required on Railway:** Must be configured before deploy; not committed to git.
3. **Apply button:** Enabled visually when logged in but disabled — bidding ships in Slice 5.
4. **Social login (Google/SSO):** Omitted from Stitch reference — out of assessment scope.
5. **Remember me:** UI checkbox only; token always stored in `localStorage` (no separate persistence policy).
6. **Mobile nav:** Hamburger still non-functional — responsive polish in Slice 6.
7. **Netlify URL:** Not committed; confirm in dashboard after merge.

## Stitch

Layout references: local `stitch/login_page/code.html` (centered card, no top nav) and `stitch/register_page/code.html` (header, decorative background, icon inputs). Translated to Angular + Tailwind tokens; social login blocks omitted.

## Netlify notes

- `environment.production.ts` → `apiUrl: 'https://accountant-hub-production-cc2a.up.railway.app'` (unchanged)
- Publish: `dist/frontend/browser`
