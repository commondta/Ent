# PMS — Re-engineering Plan

Written 2026-08-03. Supersedes nothing; read `01-SYSTEM-OVERVIEW.md` and `02-ASSESSMENT.md` first.

> **Amendments 2026-08-13 → 2026-08-16** (decisions D20–D31 in `PROJECT.md`; sources:
> `AI file.xlsx` final Instructions sheet + your live direction):
>
> - **Navigation**: ~~"nothing more than two clicks deep"~~ → up to **5 levels**, per the
>   workbook's Folders Structure sheet (D20).
> - **Module structure**: ~~twelve top-level modules~~ → **five**, now **four** at top level
>   after Records & File Management moved under Estate Operations — the module named
>   "Property Business Operations" until its D32 rename, 2026-08-17 (D20, D31).
> - **Form registry**: no longer a plan — **built and live 2026-08-16** (`NavigationNodes`,
>   223 nodes; menu renders from it; `DisplayName` separated from `PermissionKey`).
> - **Working method**: the per-item review gate is retired — decisions final, implement
>   (D26); build order is the workbook's §30 (D27).
> - **Branding**: N-Stack → **N-Stack** applied 2026-08-16 (req. 12); durable branding form (N-9)
>   still due in Stage E.
> - Size metrics withdrawn as targets (D23); rename-risk acceptance (D24); Charges→Revenue
>   scope rule (D25); hidden-form verdicts applied (D28); Recent/Favourites in header + My
>   Home (D29 as amended); Administration = 5 sub-modules (D30).
>
> Everything else in this file stands.

## Decisions locked

| Decision | Choice |
|---|---|
| Environment | **Local only.** All remote connections severed. Local SQL Server, local file storage, no outbound calls to Cloudinary / Telecard / Firebase / remote SAP. |
| Strategy | **Full rewrite, reusing the domain model and approval engine.** |
| Runtime | **.NET 10 LTS** (supported to Nov 2028). |
| Front end | **Razor with a modern component structure** — ViewComponents, Tag Helpers, real asset pipeline. Server-rendered, no SPA. |
| Navigation | **Module-workspace model** — a module rail, a landing page per module, forms grouped by item type (Workspace / Transaction / Inquiry / Periodic / Report / Setup). Nothing more than two clicks deep. |
| Module structure | **Twelve top-level modules**, extensible. Setup lives inside its owning module *and* in one central Configuration index. |
| Form registry | **Menu, permission catalogue and API authorization policies all read one registry held as data.** A form is defined once, with a stable opaque permission key. |
| Product identity | **Real Estate Management System** — the umbrella over two applications: Property Management (this solution) and Land Management (`test_Land_mgt`). |
| Application boundary | **Two applications, one front door.** Land Management stays a Laravel 8 / PHP / MySQL application. It is reached from a post-login launcher **by configured URL** — never by file path, shared database, or shared code. |
| Identity authority | **PMS owns the login.** Land keeps its own until SSO is separately designed. **No credential is ever copied, synchronised or forwarded** between the two. |
| Repositories | **One repository**, `RealEstate/{property,land}`, both histories preserved. Blocked until git is installed. |

Decided 2026-08-04, recorded as D10–D14 in `PROJECT.md`. Full design, the shell, and every form
mapped to a module: `05-MODULE-ARCHITECTURE.md`.

Decided 2026-08-05, recorded as **D15–D19**. There is now a level *above* the module rail — the
app launcher — and a second application in scope. Design, ADRs and the change list:
`modules/rems-app-launcher.md`.

**What is explicitly not decided:** merging the two databases, the two user stores, or the **two
independent approval engines** the products each built. Each is its own decision, and none is
implied by the launcher.

### One risk I want on the record

A full rewrite is the highest-risk of the three options *specifically because there are
zero tests and a year of business rules is undocumented inside 275 KB controllers*. Rules
like "which charges apply when a transfer follows a repurchase inside the grace period"
exist only as code.

I'm not arguing against the decision — locally, with no production pressure, it's
defensible and the end state is far better. But the plan below makes **Phase 1 (Behaviour
Capture)** non-optional. We write down what each module does *before* we rewrite it. Skip
that phase and the rewrite becomes a guess.

---

## Target architecture

```
Pms.sln                          (.NET 10)
├── src/
│   ├── Pms.Domain/              Entities, value objects, enums, domain events,
│   │                            invariants. No EF, no ASP.NET, no external refs.
│   ├── Pms.Application/         Use cases (vertical slices), DTOs, validators,
│   │                            abstractions (IPmsDbContext, ISapGateway, IClock,
│   │                            ISmsSender, IFileStore). Depends only on Domain.
│   ├── Pms.Infrastructure/      EF Core 10, PmsDbContext, IEntityTypeConfiguration
│   │                            per entity, migrations, interceptors (audit,
│   │                            soft-delete, concurrency), repositories.
│   ├── Pms.Integration.Sap/     ★ THE ONLY project with <COMReference>.
│   │                            Implements ISapGateway. Ships with NullSapGateway
│   │                            so everything else builds and runs without SAP.
│   ├── Pms.Integration.Sms/     Telecard adapter + LocalFileSmsSender for dev.
│   ├── Pms.Integration.Storage/ Local disk store + Cloudinary adapter (disabled locally).
│   ├── Pms.Web/                 ASP.NET Core 10 MVC. Razor views, ViewComponents,
│   │                            TagHelpers, TypeScript asset pipeline, versioned
│   │                            REST API under /api/v1.
│   └── Pms.Jobs/                One hosted worker replacing all 7 uploader console
│                                apps + the AutoTriggerService stub.
└── tests/
    ├── Pms.Domain.Tests/        Pure unit tests, fast.
    ├── Pms.Application.Tests/   Use-case tests with in-memory/SQLite.
    ├── Pms.Integration.Tests/   Real SQL Server (LocalDB), migrations applied.
    └── Pms.Web.Tests/           WebApplicationFactory — auth, authz, endpoint contracts.
```

**The single most important structural change:** `Pms.Integration.Sap` is the only
assembly holding a COM reference. Today the SAP dependency is smeared across
`HRMS_Web` and makes the entire application uncompilable without SAP B1 installed. After
this split, `dotnet build` works everywhere, `dotnet test` works everywhere, and CI becomes
possible. SAP work happens behind `ISapGateway` and can be developed against a fake.

### Standards applied throughout

- `Directory.Build.props` + Central Package Management (`Directory.Packages.props`)
- `<Nullable>enable</Nullable>` **with `TreatWarningsAsErrors`** — the current 326 warnings
  get fixed, not suppressed
- `.editorconfig` with analyzer rules; `dotnet format` in CI
- `Result<T>` / `ProblemDetails` replaces `Response_Result`; **real HTTP status codes**
- Serilog structured logging, request correlation id on every log line
- FluentValidation on every command
- `IClock` abstraction, `DateTimeOffset` in UTC — `DateTime.Now` is banned by analyzer rule
- `async`/`await` all the way down; `SaveChangesAsync` only
- One `UnitOfWork` per request; explicit transactions around multi-entity operations

---

## Phase 0 — Local isolation and immediate safety

**Goal:** the repo becomes safe to work in and never touches a remote system again.
**This runs first and I can start it immediately.**

1. **Sever every remote connection.**
   - `HRMS_Web/appsettings.json` → local SQL (`Server=localhost;Integrated Security=true`),
     dropping `WIN-CM05CUDDJMV` and the `sa` credentials.
   - 7 uploader `AppDbContext.cs` files each hard-code a *different* machine:
     `WIN-CM05CUDDJMV/DHA_Live`, `DESKTOP-7OOOP01\SQLEXPRESS/UrbanQA`,
     `WASEEM-HCCLABS\SQLEXPRESS/DHA_Test`. All replaced with configuration.
   - SAP license servers are hard-coded in three places and disagree
     (`192.168.12.32:40000` in `SAPBillingDb.cs`/`SAPConnection.cs`,
     `192.168.109.6:40000` in `SAPOperationDb.cs`) → moved to config, defaulted to unset.
   - SMS, Cloudinary, Firebase → replaced with local no-op implementations that log
     instead of calling out. Nothing leaves the machine.

2. **Close the two SQL injection holes** (`DynamicQueryController.ExecuteParamQuery`,
   `SapIntegrationController.GenerateDynamicReport`). Disabled outright in the legacy app;
   replaced properly in Phase 8.

3. **Secrets out of source.** `appsettings.json` → `.gitignore`, replaced by
   `appsettings.Example.json` + .NET user-secrets. Note: **the existing credentials are in
   git history and must be rotated at the source** — I can remove them going forward, I
   cannot un-leak them.

4. **Repo hygiene.** `UrbanDev.rar` (103 MB, and a 118.7 MB pack in `.git`) removed from
   tracking; `*.rar`, `*.user`, `appsettings.json` added to `.gitignore`.

5. **Stand up the local database.** Local SQL Server is running but holds only system
   databases — there is no `DHA_Live` here. I'll run the existing 316 migrations against
   localhost to materialise the full schema, giving a working empty local DB to develop
   against until you import real data.

**Deliverable:** the legacy app builds (on a SAP-equipped box), runs against localhost,
and makes zero outbound network calls.

---

## Phase 1 — Behaviour capture *(the phase that de-risks the rewrite)*

**Goal:** get the business rules out of the 275 KB controllers and into written specs
before a single line is rewritten.

For each of ~16 modules, produce `docs/modules/<module>.md` containing:
- Every endpoint, its inputs, and its observable effects
- The state machine (which flags flip, in what order, guarded by what)
- Which `ApprovalUIId` it uses and what each stage means
- Charge/tax/date calculations written as formulas, with worked examples
- Integration points (which SPs, which SAP calls)
- Known oddities worth *not* reproducing

Also catalogued: the ~50 stored procedures (name, params, shape of the JSON they return)
and the `ApprovalUIIds` enum mapped to human-readable request types.

**This is the phase that turns "rewrite" from a gamble into engineering.** It's also the
phase where your team's knowledge matters most — I can read code, but I can't read intent.
Expect to correct me here.

---

## Phase 2 — New solution skeleton, build, and CI

- Create `Pms.sln` on .NET 10 with the project layout above.
- SAP isolated behind `ISapGateway`; `NullSapGateway` registered by default so the
  solution builds and tests run on any machine, including this one.
- GitHub Actions: restore → build → test → format check on every push.
  (`.github/` is currently empty — there has never been CI on this project.)

**Deliverable:** `dotnet build` and `dotnet test` succeed from a clean clone with no SAP,
no Visual Studio, no special machine. This is the milestone that ends the "only builds on
one PC" problem.

---

## Phase 3 — Domain and persistence

- Port all 140 entities into `Pms.Domain`, cleaned up:
  - **One** base-entity convention. Today `BaseModel` (`Id`/`IsActive`/`CreatedOn`) and
    the legacy convention (`ID`/`is_active`/`Created_at`) both exist in the same DbContext,
    which is why no cross-cutting behaviour can be applied. Unified to:
    `Id, IsActive, IsDeleted, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, RowVersion`.
  - `RowVersion` added → optimistic concurrency, ending silent last-write-wins.
  - Column names preserved via configuration so the **database schema stays compatible**.
- Split the 41 KB `DataBase_Context` into one `IEntityTypeConfiguration<T>` per entity.
- Interceptors: audit stamping, soft-delete global query filters (removing the need to
  remember `.Where(x => !x.is_deleted)` by hand), UTC enforcement.
- **Squash 316 migrations into one baseline** that produces a byte-identical schema,
  verified by schema diff against the current database before anything is discarded.

---

## Phase 4 — Platform cross-cutting concerns

- **Authorization, properly.** Permissions (`UserPermissionMapping.CanAdd/Edit/Delete/View`)
  become claims, enforced by policy handlers server-side:
  `[RequirePermission("StockCreation", Access.Edit)]`. Plus resource-based checks so a user
  can't act on a `RegistrationNo` they aren't entitled to. Today all of this is client-side
  in Razor/JS and trivially bypassed.
- **One auth mechanism.** The current split — session for MVC, JWT for API, mutually
  unaware, sign-out leaving a 12-hour token valid — collapses into one scheme with proper
  expiry, refresh, and revocation.
- Global exception handler → `ProblemDetails`, correct status codes. No more HTTP 200 on
  failure, no more `ex.Message` returned to callers.
- Serilog + correlation ids (currently: 4 `ILogger` references in 58k lines).
- API versioning + OpenAPI/Swagger.

---

## Phase 5 — Module migration (the bulk of the work)

Ordered so each module's dependencies land before it:

| # | Module | Notes |
|---|---|---|
| 1 | Master data | ~60 near-identical CRUD controllers → one generic slice + config. Big, fast win. |
| 2 | Users, roles, permissions | Foundation for Phase 4 enforcement. |
| 3 | **Approval engine** | Ported carefully — this is the crown jewel. Gets the deepest test suite. Fixes `SaveChanges()` inside `foreach`. |
| 4 | Stock / inventory | `StockCreation` is the largest entity (12.6 KB) and the hub of the model. |
| 5 | Parties (member, dealer) | |
| 6 | Sales pipeline | Lead → PreSale → Booking → Deal → BulkDeal |
| 7 | Transfers & ownership | Transfer, Amalgamation, Surrender, Repurchase, DeAllocation, COP |
| 8 | NDC / clearance / demarcation | |
| 9 | Construction & metering | |
| 10 | Billing & charges | Highest correctness risk — money. Worked examples from Phase 1 become tests. |
| 11 | Files & documents | |
| 12 | Legal / cases / soft-locks | |
| 13 | Reporting | See Phase 8 |
| 14 | SAP integration | Last — behind `ISapGateway`, developed against a fake until you have SAP installed. |

Per module: spec (Phase 1) → tests → implement → parity-check against legacy → cut over.

---

## Phase 6 — Front end

- Decompose `_Layout.cshtml` (249 KB / 2,909 lines / 35 inline `<script>` blocks) into a
  shell plus ViewComponents: navigation, permission-aware menu, alerts, user chrome.
- **The shell is registry-driven** — module rail, module landing page, breadcrumb, global
  search, favourites, recent, and a pinned My Work region for the approval inbox. None of it
  is hard-coded; adding a module or a form is a data insert. See `05-MODULE-ARCHITECTURE.md`.
- **Retire the hard-coded menu** only after the registry is seeded and the permission
  migration has passed its zero-difference check — a mis-mapped key silently changes access.
- Tag Helpers for the repeated form/grid/modal patterns that are currently copy-pasted
  across 277 views. `Approval/Inbox.cshtml` alone is 201 KB.
- A `<permission for="StockCreation" access="Edit">` tag helper backed by *the same policy*
  as the API — so the UI and the server can no longer disagree.
- Asset pipeline: TypeScript modules replacing inline JS, bundled and minified.
  **Prerequisite: Node.js is not installed on this machine** — you'll need it (LTS).
- Delete the dead weight: the entire `wwwroot/functions/` PHP stack (phpmailer, Twitter
  OAuth, `login-form.php`), `excanvas.js`, `respond.min.js`, `html5shiv.js`,
  `bootstrap-old.min.js`, and the IE7/8 conditional comments.

---

## Phase 7 — Jobs and data loaders

Replace all 7 uploader console apps with one `Pms.Jobs` worker:
- Import definitions in configuration, not `const string csvFilePath = @"C:\DataUploader\..."`
- **Correct exit codes** — today every uploader catches, prints, and exits 0, so a
  scheduled task reports success after importing nothing
- No `Console.ReadKey()` — they currently cannot run unattended
- Batched, idempotent, resumable, with a `--dry-run` mode and structured logs
- Retires the empty `AutoTriggerService` stub

---

## Phase 8 — Reporting, done safely

- The `DynamicQuery` feature is genuinely useful — admins defining their own reports — so
  it gets rebuilt, not removed: a report-definition model with an **allow-listed schema**,
  typed parameters, and real parameterisation. Never string concatenation, never
  `CommandText = userInput`.
- Existing SP-based reports keep their SPs but get wrapped in typed repository methods with
  a proper paged-result type, replacing the `Request.Form["draw"]` + JSON-column +
  `JsonConvert` pattern.

---

## Phase 9 — Hardening and cutover

Performance pass (N+1 elimination, indexes, async verification), load test, security
review against `02-ASSESSMENT.md` line by line, containerised deployment, runbook, and a
data migration rehearsal.

---

## What I need from you

| # | Item | Blocks |
|---|---|---|
| 1 | **Node.js LTS installed** | Phase 6 asset pipeline |
| 2 | **A database with real data** — restore a `DHA_Live` backup to localhost | Phase 1 behaviour capture, all parity testing. I can work from an empty schema, but I can't verify calculations without data. |
| 3 | **SAP B1 client + DI API installed** (you mentioned you'd do this) | Phase 5 module 14 only — everything else proceeds without it |
| 4 | **Your team's knowledge during Phase 1** | The rewrite's accuracy. Business rules I can't infer from code are the main failure mode here. |

None of these block Phase 0. I can start immediately.

## Honest effort picture

This is a real re-engineering of a ~58k-line system with zero existing tests. Phases 0–2
are days of work and deliver disproportionate value: the security holes close, the build
stops being machine-locked, and CI exists for the first time. Phase 1 is the long pole and
the one where your input matters most. Phases 3–5 are the bulk, and they're incremental —
every module lands complete and tested, so there is never a "half-migrated, nothing works"
state.

## Recommended start

Phase 0, today. It's self-contained, it's the only phase with genuinely urgent items, and
it makes the repo safe to work in regardless of what we decide about everything else.
