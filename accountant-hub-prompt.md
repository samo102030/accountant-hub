You are building "Accountant Hub" - a job marketplace for accountants.

This is a **time-boxed assessment task** (≈48 hours), not a full production product.
Prioritize: working features, live deploy, clean readable code.
Avoid over-engineering — no extra layers, libraries, or patterns beyond what is listed here.

STRICT RULES — follow in every slice:
- Only implement what the current slice requests
- Do not refactor unrelated files
- Do not rename existing folders or files
- Do not modify APIs, database schema, or deployment config
  from previous slices unless the current slice explicitly requires it
- Keep code simple and production-ready
- If an architecture decision is needed, ask before implementing
- Do not add Application/Domain projects or abstractions "for cleanliness"

After completing ANY slice — STOP rules (mandatory):
- STOP completely. Do not start the next slice.
- Do not suggest or implement future slices.
- Do not continue automatically.
- Wait for explicit user approval (e.g. "continue Slice 2" or "start Slice 3").
- Write slice report (see below) before stopping.

Git / merge (not the Agent's job to merge):
- Push branch feature/slice-N to GitHub after verification
- Prepare branch for merge (clean commits, slice report complete)
- Do NOT merge to main automatically
- Wait for explicit user approval before merging to main

Deployment:
- Use platform-recommended settings (Railway .NET 8 for API, Netlify from netlify.toml)
- Do not hardcode obsolete Railway publish/start commands — follow Railway dashboard for .NET 8
- Record actual deploy settings and URLs in the slice report

If deployment fails (Netlify or Railway):
- STOP immediately
- Report build logs, error messages, and what was attempted
- Do NOT attempt large refactors or architecture changes
- Propose at most 1–2 minimal fixes; wait for user approval before applying
- Do not start the next slice

For each slice:
- Create branch feature/slice-N from main
- Complete work on that branch, push, verify live URLs; user merges when ready
- Create `docs/slice-reports/SLICE-0N.md` (one file per slice, do not overwrite previous slice reports)
- Update PROJECT_STRUCTURE.md only at end of Slice 2, Slice 4, and Slice 6

Slice report (`docs/slice-reports/SLICE-0N.md`) must include:
- What was added (short summary)
- Files created
- Files modified
- Endpoints added (if any)
- Database changes (if any)
- Deployment status (Netlify URL, Railway URL, build/deploy green or not)
- Merge status (branch pushed; merge to main pending user approval — Agent does not merge)
- Live URLs and how to verify
- Known issues / blockers
- For Slice 1 only: copy the mandatory checklist below with [x] or [ ] filled in

Documentation:
- Versions: TECH_STACK.md only — install latest stable / LTS, then pin exact numbers there
- Layout & API list: PROJECT_STRUCTURE.md

Tech stack (versions in TECH_STACK.md):
- Frontend: Angular (standalone), PrimeNG + Tailwind (Slice 2+)
- Backend: **.NET 8 LTS** Web API, simple modular monolith (2 projects), EF Core, PostgreSQL
- Auth: JWT + ASP.NET Identity (Slice 4+)
- API docs: **Swashbuckle** on .NET 8 → Swagger UI at `/swagger`
- Deploy: Netlify (frontend) + Railway (API + Postgres)

Backend structure (required — keep simple):
- `AccountantHub.API` — controllers, DTOs, middleware, Features/Jobs|Auth|Bids
- `AccountantHub.Infrastructure` — Persistence (DbContext, entities, migrations, seed), Identity (Slice 4+)
- Do NOT create separate Application or Domain class library projects

Frontend structure:
- `core/` — services, interceptors, guards
- `shared/` — reusable components
- `features/` — jobs, auth, my-bids

Product scope (assessment task):
- **Accountant-facing only:** browse jobs, view details, register/login/logout, submit bids, My Bids dashboard
- **Not required:** client/company auth, Post Job / Create Job UI, admin panel — jobs come from **database seed** (Slice 2)
- Narrative mentions companies posting jobs; implement as seeded data only

Design reference — Google Stitch (local only, NOT in Git):
- HTML guides live in `/stitch/` on the developer machine — **never commit** (listed in `.gitignore`)
- Do not deploy Stitch HTML; use only as visual/layout reference when implementing Angular UI
- Full tokens and rules: read local `stitch/DESIGN.md` (typography, spacing, components)
- In Agent sessions with UI work: attach relevant `stitch/<screen>/code.html` via `@` (e.g. `@stitch/jobs_listing_page/code.html`)
- Translate Stitch layout to Angular + PrimeNG + Tailwind — do not copy inline HTML blindly
- If Stitch layout and responsive rules below conflict, **responsive rules in this prompt win** (Stitch guides are mostly desktop-oriented)

Stitch screen mapping (local paths):

| Screen | Local file | Slice |
|--------|------------|-------|
| Jobs listing | `stitch/jobs_listing_page/code.html` | 2 |
| Jobs listing empty | `stitch/jobs_listing_empty_state/code.html` | 2 / 6 |
| Job details | `stitch/job_details_page/code.html` | 3 |
| Job details — already bid | `stitch/job_details_already_bid_state/code.html` | 5 |
| Bid submission | `stitch/bid_submission/code.html` | 5 |
| Login | `stitch/login_page/code.html` | 4 |
| Register | `stitch/register_page/code.html` | 4 |
| My Bids dashboard | `stitch/my_bids_dashboard/code.html` | 6 |
| My Bids empty | `stitch/my_bids_empty_state/code.html` | 6 |

Colors and theme (single source in code — from `stitch/DESIGN.md`):
- Do **not** use legacy task colors (#019a51, #000000) — removed in favor of Stitch system
- At Slice 2 (when Tailwind is added), define centralized tokens in `frontend` (e.g. `tailwind.config` and/or `src/styles/_tokens.scss`) — change colors in **one place** only
- Core brand tokens to implement:
  - `primary` / primary actions: **#14a800**
  - `primary-dark` / headings emphasis: **#0a6e00** (where Stitch uses primary token)
  - `secondary` / strong text: **#001e00**
  - `background` / canvas: **#ffffff**
  - `surface`: **#f9f9f9**
  - `on-surface` / body text: **#1a1c1c**
  - `border`: **#e4ebe4**
  - `error`: **#ba1a1a**
- Typography: **Hanken Grotesk** headings, **Inter** body/UI (load via Google Fonts or project assets in Slice 2+)
- Slice 1: no Stitch implementation, no theme setup required beyond optional placeholder

Netlify (`/frontend/netlify.toml`):
  [build]
    command = "npm run build"
    publish = "dist/frontend/browser"   # default — VERIFY after first local build

  [[redirects]]
    from = "/*"
    to = "/index.html"
    status = 200

After `npm run build`, check actual output under `dist/`. Update `publish` in netlify.toml to match exactly (may be `dist/frontend` or `dist/frontend/browser` depending on Angular version).

Railway:
- .NET API auto deploy from GitHub — use Railway-recommended .NET 8 deployment (do not assume legacy publish/start commands)
- Point service at `backend/` / API project per dashboard; record settings in slice report
- PostgreSQL in Slice 2+ — parse `DATABASE_URL` in Program.cs (see Slice 2), never commit it

GitHub Actions: optional (Netlify + Railway deploy on push is enough)

UI (Slice 2+): PrimeNG for controls, Tailwind for layout and design tokens above

Icons (Slice 2+ — one library only):
- Use **Google Material Symbols Outlined** (matches local Stitch HTML guides)
- Load via Google Fonts in `index.html` or project styles — do not mix PrimeIcons unless Material Symbols is unavailable
- Use `<span class="material-symbols-outlined">icon_name</span>` or equivalent; pick names close to Stitch (search, filter, menu, etc.)
- Slice 1: no icons required

UI / Responsive (mandatory — not defined in Stitch guides; always apply):
- Mobile < 768px | Tablet 768–1024px | Desktop > 1024px
- Mobile-first from Slice 2; polish in Slice 6

Localization: English only in templates — no ngx-translate required

API documentation:
- Swashbuckle + Swagger UI at `/swagger` (Slice 1: health; Slice 4+: JWT Authorize)

Security:
- Secrets only in Railway env vars or dotnet user-secrets (gitignored)
- JWT key in Railway (Slice 4+), 24h access token, localStorage on client
- CORS: Netlify origin + localhost:4200 (finalize Slice 2)

Validation:
- Data Annotations on DTOs
- Register: valid email, unique email, password min 8 chars
- Bid: proposedPrice > 0, deliveryDays > 0, coverLetter ≤ 2000, experienceSummary ≤ 1000
- 400 validation errors, 409 duplicate bid, global handler hides stack traces in production
- Frontend Reactive forms match API rules; English error messages

API response format:
{ success: bool, message: string, data: any, meta: { total, page, pageSize } | null }

Errors: { success: false, message: string, data: null, meta: null }

---

Slices — stop after each for approval. See STOP rules above.

SLICE 1 — Skeleton + live deploy (minimal)
- Monorepo: `/frontend`, `/backend` with **AccountantHub.API** + **AccountantHub.Infrastructure** (Infrastructure can be empty stub until Slice 2)
- Run installs using **latest stable Angular** and **.NET 8 LTS** — record exact versions in TECH_STACK.md
- Angular bare app → Netlify (page must load — not 404)
- API: `GET /api/health` → Railway
- Swashbuckle → `/swagger` on Railway
- Root `.gitignore` (secrets, bin, obj, node_modules, dist, **`stitch/`**)
- Verify `angular.json` project name `frontend` and **actual** dist publish path → update netlify.toml
- No PostgreSQL, no EF, no JWT, no PrimeNG/Tailwind yet
- GitHub Actions: optional
- SLICE 1 mandatory checklist (all must be [x] in SLICE-01.md before stopping):
  - [ ] Health endpoint `GET /api/health` returns **200** on Railway
  - [ ] Swagger UI at `/swagger` is **working** on Railway
  - [ ] Angular app on Netlify URL is **working** (not 404/blank)
  - [ ] Railway deployment status = **green** / successful
  - [ ] Netlify deployment status = **green** / published
  - [ ] TECH_STACK.md updated with **real pinned versions** (not placeholders)

SLICE 2 — Jobs + database
- Connect Railway PostgreSQL; parse DATABASE_URL:
  var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
  if (databaseUrl != null) {
    var uri = new Uri(databaseUrl);
    var connectionString = $"Host={uri.Host};Port={uri.Port};" +
      $"Username={Uri.UnescapeDataString(uri.UserInfo.Split(':')[0])};" +
      $"Password={Uri.UnescapeDataString(uri.UserInfo.Split(':')[1])};" +
      $"Database={uri.AbsolutePath.TrimStart('/')};" +
      $"SSL Mode=Require;Trust Server Certificate=true";
  }
- EF migrations, seed 10 jobs / 4 categories
- `GET /api/jobs` with filters, sort, pagination
- Install PrimeNG + Tailwind; wire Material Symbols Outlined (see Icons)
- Centralize color tokens (see Colors and theme)
- Jobs listing UI per `stitch/jobs_listing_page/code.html`; use `jobs_listing_empty_state` when no results
- Mobile-first responsive (see UI / Responsive)
- CORS: production Netlify URL
- Update PROJECT_STRUCTURE.md

SLICE 3 — Job details
- `GET /api/jobs/:id`
- Details page per `stitch/job_details_page/code.html`
- Attachments placeholder only (no upload), "Login to apply" when logged out

SLICE 4 — Auth
- Identity + `POST /api/auth/register`, `POST /api/auth/login` (JWT)
- Swagger Bearer auth
- Login/Register UI per `stitch/login_page/code.html` and `stitch/register_page/code.html`
- Interceptor, guard; client-side logout only (no logout page)
- Update PROJECT_STRUCTURE.md

SLICE 5 — Bids
- Bids table, unique (userId, jobId)
- `POST /api/jobs/:id/bids` (JWT), 409/400 handling
- Bid form per `stitch/bid_submission/code.html`
- If user already bid: show `stitch/job_details_already_bid_state/code.html` pattern (not the form)
- Success message after submit (inline/toast — no separate screen required)
- One bid per user per job; hide/disable Apply when job Closed

SLICE 6 — Polish + README
- `GET /api/my-bids`, My Bids per `stitch/my_bids_dashboard/code.html`, empty per `stitch/my_bids_empty_state/code.html`
- Pagination on jobs list; `jobs_listing_empty_state` for filtered empty results
- Navbar + mobile hamburger; responsive polish all pages
- README: overview, live URLs, Swagger, test credentials, API list, setup, security note
- **Why this stack:** Angular (UI), **.NET 8 LTS** (stable API), PostgreSQL/Railway, PrimeNG+Tailwind, Stitch guides (local), simple 2-project backend for 48h scope
- Note in README: UI follows local Stitch design system; jobs seeded (no client Post Job flow)
- Final PROJECT_STRUCTURE.md update

---

Rules for every slice:
- Commit, push, deploy, verify live
- Write `docs/slice-reports/SLICE-0N.md` then STOP — wait for explicit approval
- Do not merge to main automatically — user merges after review
- On deploy failure: STOP, report errors, no large refactors (see Deployment rules above)
- Do not start the next slice or suggest implementing it until user approves
- Shared components: job-card, bid-form, pagination, empty-state
- Slice not done until it works on live URLs and slice report is written
