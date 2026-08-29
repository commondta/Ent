# PMS Re-engineering — Project Charter and Plan

> N-Stack Property Management System · full rebuild on .NET 10
> Owner: Adnan (solo) · Started 2026-08-03 · This file is the single source of truth

**Goal** — Rebuild the system as a clean, tested, locally-runnable solution that preserves the
domain model and approval engine a year of work went into.

**Done when** — `dotnet build` and `dotnet test` pass from a clean clone with no SAP installed,
every module has a written spec and a test suite, and no P0 or P1 from the assessment remains.

**Now** — **Building. Phase 1 of the `AI file.xlsx` restructure: the Home screen.**
The workbook update of 2026-08-16 (new Instructions sheet) closed every open Stage-A decision and
ordered implementation — decisions final, no more per-item review stops (D26). The My Home
workspace tab shipped the same day (`#148`). See `CURRENT-WORKS.md` for the bench and
`docs/AI-FILE-OBSERVATIONS.md` §12 for the answer digest.

**`#140`–`#146` (REMS app launcher) stand down** to the end of the programme — Restructure!B367:
*"we have to get land management system within this solution but this is our last task, we will
bother on it after maturing PMS."* Only the APPS tile region on My Home survives into the shell
work. D15–D19 stay locked; their delivery moves.

---

## 1 · Status at a glance

| | |
|:--|:--|
| **Health** | 🟢 Building — Home screen, registry navigation, header shell, global search and the rebrand all live |
| **Tasks complete** | 30 of 151 — `#121`–`#124` `#129`–`#133` `#148` `#150` `#152`–`#154` closed 2026-08-16; `#13` re-closed 2026-08-17 as a local-only repo; `#155` monochrome theme closed 2026-08-20; `#156` header shell brief (responsive bar, notification panel, empty-box removal) closed 2026-08-21; `#157` login hero vector closed 2026-08-21 |
| **In flight** | 1 — `#149` idle-logout overlay, next on the bench |
| **Blocked** | 0 on you — the workbook closed every open decision (D26). `#103`/`#119` reviews overtaken by it |
| **Version control** | ✅ **Local-only git since 2026-08-17** — fresh repo on `main`, initial commit `6002fb6` (2,398 files). **No remote, by explicit instruction — never push or publish** (`#13` closed) |
| **Runs locally** | ✅ `http://localhost:5217` — My Home workspace is the landing view |
| **Next milestone** | Phase 2 — the form/module registry (Stage B) |

**Progress by milestone**

| | Milestone | Scope | State |
|:--|:--|:--|:--|
| M0 | **Safe** | Repo stops touching remote systems; injection holes closed | Not started |
| M1 | **Understood** | Every module's behaviour written down | 1 of ~16 done |
| M2 | **Builds anywhere** | Clean clone builds and tests without SAP | Not started |
| M3 | **Foundation** | Domain, persistence, auth, authorization in place | Not started |
| M4 | **Feature parity** | All 16 modules rebuilt and verified | Not started |
| M5 | **Operable** | Jobs, reporting, deployment, runbook | Not started |
| M6 | **Cut over** | Live on the new system | Not started |

---

## 2 · Delivery method

One form, one module, one process at a time. Nothing is built before it is understood.

| Step | Activity | Output | Gate |
|:--|:--|:--|:--|
| 1 | **Understand** — read that item's real controller, view, entity, procedures | — | |
| 2 | **Document** — fields, endpoints, state transitions, formulas with worked examples | `docs/modules/<item>.md` | |
| 3 | **Assess** — cost, dependencies, risks, alternatives, recommendation | same file | |
| 4 | **Break down** — concrete implementable tasks | this file | |
| 5 | **Review** | — | 🛑 **Stops here until you say go** |
| 6 | **Build → verify → close** | code + tests | Definition of Done, §9 |

Steps 1–3 go deep; that is where the risk is removed. Status stays short.

**Conventions** — Status `todo` `doing` `blocked` `done` · Priority `high` `med` `low` ·
`Needs` lists task numbers that must finish first · Numbers are never reused · Only one task is
`doing` at a time.

---

## 3 · Critical path

The chain that determines the finish date. Everything else has slack.

```
#13 git ────────────► #38 CI
                        ▲
#32 solution ─► #34 projects ─► #35 SAP isolated ─► #36 tests
                        │
                        ├──► #40 base entity ─► #41 entities ─► #43 config ─► #46 squash ─► #47 verify
                        │
                        └──► #49 claims ─► #50 policies ──────┐
                                                              ▼
      #20 data dictionary ─► #23 approval spec ─► #62 approval engine ─► #66 transfers ─► #69 billing
                                                                                              │
                                                                                              ▼
                                                                              #98 rehearsal ─► #99 cutover
```

**Longest chain:** data dictionary → approval spec → approval engine → transfers → billing →
migration rehearsal → cutover. Billing cannot be verified without real data, so **restoring a
database is on the critical path even though it is your task, not mine.**

**`#13` git — re-closed 2026-08-17**: a fresh **local-only** repository (branch `main`, initial
commit `6002fb6`). The old GitHub-connected repo was removed earlier the same day; the user has
ruled that this repo never gets a remote. Repo merge (`#147`) is unblocked; CI (`#38`) stays
blocked — it would need a remote the user does not want.

---

## 4 · Active item — Restructure Phase 1 · Home screen

> Ordered by Instructions §30/§32 (workbook update 2026-08-16): Home screen first, then the
> registry. The design source is the UI sheet mock; detail in `docs/WORK-LOG.md` §7.

| # | Task | Status | Pri | Needs | Est. |
|:--|:--|:--|:--|:--|:--|
| 148 | **My Home workspace tab** — Fiori-style cards: Overview Analytics, To-Dos, APPS (config-driven), Recent, Favourites; default landing view; `api/Workspace/GetMyHomeSummary` | done | high | | — |
| 153 | **Registry + navigation** — `NavigationNodes` table seeded from the workbook (223 nodes); `_Layout` menu block (2,460 lines) replaced by the registry-driven partial; permission parity proven (0 mismatches). Closes `#121`–`#124`, `#129`–`#133` — see `docs/WORK-LOG.md` §7.5 | done | high | | — |
| 150 | **Header strip delivered** — centered global search, Recent/Favourites popovers, Generate Alert box (N-5), filled bell + approval icons, single-vector Property Management System brand, one 61px centerline, accessibility pass. §7.6–§7.12 | done | high | | — |
| 152 | **Global search foundation** — `api/Workspace/GlobalSearch`: forms (new + old names, breadcrumbs) + properties/members/dealers with deep-links into their forms via the `?gs=` opener contract | done | med | | — |
| 154 | **Rebrand to N-Stack (req. 12)** — login wordmark replaced, consultancy chevron icon removed; durable branding form (N-9) still due in Stage E | done | med | | — |
| 155 | **Monochrome theme (Theme folder spec, 2026-08-20)** — black & white ERPNext-inspired redesign across shell, forms, tables, panels, tabs, login (white card + dark-crystal right, entrance animation); active-item nav line animations; icon containers black-with-white; My Home + Dashboards colors kept by instruction. UI layer only. `docs/WORK-LOG.md` §11 | done | high | | — |
| 156 | **Header shell brief (`2. Remove the Extra Box After Form.txt`, 2026-08-21)** — empty `card p-3 > menu` box removed from 11 Setup/Approval/Alert/Demand-Note views; bell shows the live unread count (99+ cap, minute refresh) and opens the notifications **panel** (DataTables search / page size / pages over the same `GetAll` API) instead of the page; header is one flex row below 1200px with progressive disclosure (name → ⋮ More → bell at ≤480), icon-only search on phones; sidebar auto icons-only below 1200px. UI layer only. `docs/WORK-LOG.md` §12 | done | high | | — |
| 157 | **Login hero vector (2026-08-21)** — original inline SVG (tower crane, glass skyline with lit windows, three gabled homes on a ground sweep) replaces the N-Stack logo + name block in the dark crystal panel of the login page; monochrome white/grey for contrast; slow float, hook sway and window glint honouring reduced-motion. `docs/WORK-LOG.md` §12 | done | normal | | — |
| 149 | Idle-logout blur overlay + in-page re-login, server-side enforced (Instructions §21, N-3) | todo | high | | 4h |
| 151 | Map tab investigation — PDF / vector / CAD plot map with plot detail panels (§4) | todo | med | | — |

---

## 4e · Parked — Real Estate Management System app launcher

> **Analysis complete, decisions locked.** `docs/modules/rems-app-launcher.md` · both codebases
> measured · 4 ADRs · all 5 questions answered 2026-08-05. **Nothing built — waiting on your go.**

Your ask: after signing into PMS, an Odoo-style panel to choose **Land Management** or **Property
Management**, the pair branded **Real Estate Management System**, and nothing else changed.

**The finding that shapes it** — `test_Land_mgt` is **not** another .NET solution. It is a
**Laravel 8 / PHP application on MySQL**: 34 controllers, 42 models, 109 views, 51 migrations, its
own login, its own permission model (~90 boolean columns on `users`) and **its own approval
engine**. It shares nothing with PMS but the domain.

| | Property Management | Land Management |
|:--|:--|:--|
| Stack | ASP.NET Core, .NET 6 | Laravel 8, PHP |
| Database | SQL Server `PMS_Local` | MySQL `test_dha_land_management` |
| Permissions | `PermissionForms` rows, keyed by menu label | ~90 boolean columns on `users` |
| Layout | `_Layout.cshtml` 249 KB | `main.blade.php` 277 KB |
| Runs here | ✅ `:5217` | ❌ **PHP, Composer, MySQL all absent** |

So "combine" cannot mean one codebase in this task — **you confirmed that in Q1.** What ships is a
**shell that makes them one product to the user**: a launcher inside HRMS_Web, the app list held as
configuration, Land reached by URL. **Seven files, two changed lines of behaviour**
(`Views/Login/Index.cshtml:690` and one anchor in `_Layout.cshtml`), fully reversible.

**Your answers, 2026-08-05** — locked as D15–D19:

| Q | Answer | Effect |
|:--|:--|:--|
| Q1 Scope | **One front door over two apps** | Option A confirmed. No port, no code or data merge |
| Q2 Sign-in | **Second sign-in accepted for now** | SSO deferred to its own gated item |
| Q3 Switch app | **Approved — one anchor** | `#146`. Two changed lines, not one |
| Q4 Repositories | **Merge into one** | `#147`, ADR-004 — **blocked on git `#13`**, independent of the launcher |
| Q5 Auto-navigate | *my call* — **no** | The panel always renders; you asked for a selection screen |

| # | Task | Status | Pri | Needs | Est. |
|:--|:--|:--|:--|:--|:--|
| 138 | Your review — the gate | done | high | | — |
| 139 | Q1–Q5 answered → D15–D19 locked | done | high | 138 | — |
| 140 | `RealEstate` config section + typed options | todo | high | 139 | 2h |
| 141 | `AppLauncherController` — session guard, permission filter, `/apps` | todo | high | 140 | 3h |
| 142 | Launcher view + stylesheet — tiles, standalone layout | todo | high | 141 | 4h |
| 143 | Redirect login to `/apps` — one changed line | todo | high | 142 | 15m |
| 144 | `Land Management` permission key seeded | todo | med | 141 | 1h |
| 146 | "Switch app" anchor in `_Layout.cshtml` — menu block untouched | todo | med | 141 | 30m |
| 145 | Manual verification pass | todo | high | 143, 146 | 1h |

**`#140`–`#146` ≈ 1.5 days.** Separate, and blocked:

| # | Task | Status | Pri | Needs | Est. |
|:--|:--|:--|:--|:--|:--|
| 147 | **Merge the two repositories** — `RealEstate/{property,land}`, both histories preserved | blocked | med | 13 | own item |

Still deferred behind their own gates: single sign-on (REQ-7), full rebrand of both shells, reverse
proxy.

**Stood down to Stage I** (D21, reconfirmed by Instructions §23). The configuration half of
`#140` shipped 2026-08-16 inside `#148` — `RealEstate:Apps` drives the My Home APPS tiles.

---

## 4c · Parked — Module and navigation architecture

> **Analysis complete**, waiting on your review. `docs/05-MODULE-ARCHITECTURE.md` · 16 navigation
> defects found · all 209 forms mapped to 12 modules · 6 open questions.

Decided with you 2026-08-04: module-workspace shell (D10), setup in-module plus a central
Configuration index (D11), 12 top-level modules whose list stays extensible (D13, D14).

| # | Task | Status | Pri | Needs | Est. |
|:--|:--|:--|:--|:--|:--|
| 117 | Current-state navigation audit | done | high | | 2h |
| 118 | Target taxonomy and full form mapping | done | high | 117 | 4h |
| 119 | ~~Your review — the gate~~ overtaken by the workbook's final answers (D26) | done | high | 118 | — |
| 120 | Resolve the 6 open questions — answered by the 2026-08-16 workbook update | done | high | 119 | — |
| 121 | Registry schema — `NavigationNodes`, 5 levels, `DisplayName` ≠ `PermissionKey` | done | high | | `#153` |
| 122 | Seed the registry with stable permission keys — 223 nodes from the workbook | done | high | 121 | `#153` |
| 123 | Permission migration + zero-difference check — 0 unmatched keys, 0 admin gaps; 9 check-key aliases recovered from git | done | high | 122 | `#153` |
| 124 | Navigation service — permission-filtered tree (`_NavigationMenu.cshtml`); claims-based version comes with `#49` | done | high | 123 | `#153` |
| 125 | App shell — rail, module page, My Work | todo | high | 124, 74 | 2d |
| 126 | Global search over the registry | todo | med | 124 | 0.5d |
| 127 | Favourites and Recent, per user | todo | med | 124 | 0.5d |
| 128 | Administration → Configuration index | todo | med | 124 | 0.5d |
| 129 | Retire the menu block in `_Layout.cshtml` — 2,938 → 423 lines; old block preserved in git (`20364cf`) | done | high | | `#153` |

**Independent legacy repairs** — small, they fix live defects in the running app now:

| # | Task | Status | Pri | Est. |
|:--|:--|:--|:--|:--|
| 130 | Fix "Transfer Set Receiving" wrong target — registry routes it to `Operations/TransferSetReceiving`; label kept per mapping | done | high | `#153` |
| 131 | Remove the two dead "Finger Uploader" links — gone with the menu swap | done | high | `#153` |
| 132 | Delete the fake `Administration → Reports` subtree — gone; reports live under Business Analytics | done | med | `#153` |
| 133 | The 15 unreachable forms — verdicts applied: 10 in navigation with permission rows, 5 out of scope (in code until final stage) | done | high | D28, `#153` |

**Decision needed from you** — `docs/05-MODULE-ARCHITECTURE.md` §11, six questions. Q5 and Q6
matter most: fifteen working forms are unreachable from the menu, and six pairs of near-duplicate
views need one of each declared live.

---

## 4b · Running locally

The application now builds and runs on this machine for the first time.
**`http://localhost:5217`** · sign in as `admin` / `admin`.

> Deliberately weak, at your request, for a throwaway local database with no real data in it.
> This credential must never exist in any environment that holds real data — it is created by
> `tools/local-run/seed-local.sql` only, and no seed script ships with the rebuilt solution.

To start it again: `dotnet run --project HRMS_Web\HRMS_Web.csproj --urls http://localhost:5217`

| # | Task | Status | Pri | Est. |
|:--|:--|:--|:--|:--|
| 134 | SAP made a build switch; app compiles without it | done | high | 1h |
| 135 | Local database from the 316 migrations, seeded | done | high | 1h |
| 136 | **Schema drift — 235 of 439 tables missing `LastModifiedUserName`** | todo | high | 1d |
| 137 | Retire the local-run workarounds once `#35` lands | todo | med | — |

**Four files changed**, all reversible, all marked in place:

| File | Change |
|:--|:--|
| `HRMS_Web.csproj` | `SapIntegration` property, default `false`. Off: the 3 SAP extension files and `SapIntegrationController` are excluded and the stub compiles instead. On: original behaviour. **Nothing deleted.** |
| `Extensions/SapIntegrationStub.cs` | New. The 13 methods the other 9 controllers call, each returning an explicit "SAP not available" result — never a fake success |
| `Controllers/api/FilterController.cs` | Two `#if !SAP_INTEGRATION` guards returning the same values its own `catch` blocks already produced when SAP was unreachable |
| `appsettings.json` | Connection string → `Server=.;Database=PMS_Local;Trusted_Connection=True`. Original saved outside the repo |

**Findings from making it run**

- **`dotnet build` cannot build this project at all** — `MSB4803`: the .NET Core MSBuild cannot
  process COM references. Only the Visual Studio MSBuild could, and only with SAP installed.
  This is a harder version of `#38`: CI was never possible, not merely unconfigured.
- **The 316 migrations do not reproduce the current model.** `LastModifiedUserName` is on
  `BaseModel`, so every entity has it, but **235 of 439 tables were created without it**. Login
  crashed on the first query that touched one. Raised as `#136` — it means the migration history
  and the model have drifted apart, which `#46` and `#47` must account for.
- Local SQL Server had no application database at all; `PMS_Local` was built from scratch.
- `PermissionForms` was seeded with the **222 distinct permission names** scraped from
  `_Layout.cshtml` — direct confirmation of D12: the menu really is the permission catalogue.

---

## 4a · Parked — Block form

> **Analysis complete**, waiting on your review. `docs/modules/block.md` · 16 defects · 1 database
> trap · 1 open question that affects 20 entities.

| # | Task | Status | Pri | Needs | Est. |
|:--|:--|:--|:--|:--|:--|
| 100 | Deep-dive analysis — Block | done | high | | 3h |
| 101 | Feasibility and recommendation | done | high | 100 | — |
| 102 | Task breakdown | done | high | 101 | — |
| 103 | **Your review — the gate** | blocked | high | 102 | awaiting you; `#119` took the active slot |
| 105 | Audit ~39 master-data entities for the same traps | todo | high | 103 | 2h |
| 106 | Decide: Block as foreign key, or free text | todo | high | 103 | — |
| 107 | Entity, configuration, unique index, temporal preserved | todo | high | 32, 106 | 2h |
| 108 | Commands, queries, validators | todo | high | 107 | 3h |
| 109 | REST controller with permission policies | todo | high | 108, 50 | 2h |
| 110 | Razor view on shared components — 40 lines replacing 454 | todo | high | 109 | 3h |
| 111 | Tests — unit, integration, authorization | todo | high | 109 | 3h |
| 112 | Write up as **the** master-data pattern | todo | high | 111 | 2h |

**Independent security fixes to the legacy app** — small, and they cover ~50 screens:

| # | Task | Status | Pri | Est. |
|:--|:--|:--|:--|:--|
| 113 | `DeleteBlock` changes from `GET` to `POST` | done | high | 15m |
| 114 | Session guard on `HomeController` — `[SessionAuthorize]`, redirects to login | done | high | 15m |

**Decision needed from you** — `docs/modules/block.md` §10. The one that matters most: should
blocks be scoped to a phase or sector? The list is global today, so Block C in Phase 5 and Block C
in Phase 6 cannot both exist. If they should, that is a live data problem now, not a rebuild
question.

---

## 5 · Module queue

Sequenced so each module's dependencies land before it. A module expands into per-form tasks only
when we reach it. Full detail in `docs/04-WORK-INVENTORY.md`.

| # | Module | Screens | Cx | Risk | Status | Why it sits here |
|:--|:--|:--|:--|:--|:--|:--|
| 60 | M01 · Master data | ~40 | L | L | doing | Proves the architecture at near-zero risk |
| 61 | M03 · Users and permissions | 11 | M | M | todo | Everything downstream needs real authorization |
| 62 | M04 · Approval engine | 5 | H | H | todo | ~30 request types route through it |
| 63 | M02 · Property and inventory | 13 | H | H | todo | The spine — the plot lifecycle |
| 64 | M05 · Members and dealers | 14 | H | M | todo | The other half of every transaction |
| 65 | M06 · Sales pipeline | 11 | H | M | todo | Needs plots and parties to exist |
| 71 | M12 · Litigation and soft-locks | 6 | L | M | todo | A lock **vetoes** transfers — must precede them |
| 66 | M07 · Transfers and ownership | 12 | H | H | todo | Hardest module in the system |
| 68 | M09 · Construction and metering | 9 | M | L | todo | Feeds billing; independent of transfers |
| 69 | M10 · Billing and charges | 24 | H | H | todo | Money — formulas and examples before code |
| 67 | M08 · NDC and file movement | 15 | M | M | todo | Sits on billing, transfers and approvals |
| 70 | M11 · Documents and letters | 15 | L | L | todo | 15 near-identical templates → one engine |
| 72 | M13 · Reporting and dashboards | 21 | M | M | todo | Reads from everything else |
| 115 | M14 · Notifications and calendar | 9 | L | L | todo | Peripheral |
| 116 | M16 · Data import jobs | 8 apps | M | L | todo | Can move earlier if bulk loading is needed |
| 73 | M15 · SAP integration | 3 | H | H | todo | Last — behind the gateway, against a fake |

`Cx` complexity · `Risk` chance of getting the business rules wrong

---

## 6 · Backlog by phase

### Phase 0 · Isolation and safety → **M0**

*Repo becomes safe to work in and stops touching remote systems. Independent of everything else.*

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 5 | ~~Disable~~ **Guarded** `ExecuteParamQuery` — `[Authorize]` + SELECT-only | done | high | |
| 6 | ~~Disable~~ **Guarded** `GenerateDynamicReport` — `[AllowAnonymous]` removed | done | high | |
| 7 | Remove the hard-coded SAP security key | todo | high | |
| 8 | `appsettings.json` points at localhost | todo | high | |
| 9 | 7 uploader connection strings → configuration | todo | high | 8 |
| 10 | 3 SAP license-server addresses → configuration | todo | high | |
| 11 | SMS, image hosting, push → local no-ops | todo | high | |
| 12 | Secrets → user-secrets and an example file | todo | high | 8 |
| 13 | Version control — **local-only repo created 2026-08-17** on `main`, initial commit `6002fb6`. No remote by explicit instruction; never push or publish | done | high | |
| 14 | Ignore rules; untrack the 103 MB archive | todo | med | |
| 15 | **Rotate the leaked credentials** | todo | high | |
| 16 | Local database built from the 316 migrations | todo | high | 8 |
| 17 | Verify zero outbound network calls | todo | med | 9, 10, 11 |

### Phase 1 · Behaviour capture → **M1**

*Business rules out of the controllers and into specs before anything is rewritten.*

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 18 | Stored-procedure catalogue (~50) | todo | high | |
| 19 | Approval request-type map | todo | high | |
| 20 | Entity data dictionary (140 entities) | todo | high | |
| 21 | Spec — master data | todo | high | 20 |
| 22 | Spec — users, roles, permissions | todo | high | 20 |
| 23 | Spec — approval engine | todo | high | 19 |
| 24 | Spec — stock and inventory | todo | high | 20 |
| 25 | Spec — members and dealers | todo | high | 20 |
| 26 | Spec — sales pipeline | todo | high | 24 |
| 27 | Spec — transfers and ownership | todo | high | 24, 25 |
| 28 | Spec — clearance, demarcation, construction | todo | high | 24 |
| 29 | Spec — billing and charges, with worked examples | todo | high | 27 |
| 30 | Spec — files, documents, legal, soft-locks | todo | med | 20 |
| 31 | Known-oddity register | todo | med | 21, 30 |

### Phase 2 · Skeleton and CI → **M2**

*Ends the "only builds on one PC" problem.*

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 32 | `Pms.sln` on .NET 10 | todo | high | |
| 33 | Build props, central packages, editorconfig | todo | high | 32 |
| 34 | 8 source projects, dependency rules enforced | todo | high | 33 |
| 35 | SAP behind one interface, plus a fake | todo | high | 34 |
| 36 | 4 test projects and a shared harness | todo | high | 34 |
| 37 | Serilog and correlation ids | todo | high | 34 |
| 38 | CI — restore, build, test, format | todo | high | 13, 36 |
| 39 | Architecture tests | todo | med | 36 |

### Phase 3 · Domain and persistence → **M3**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 40 | One base entity convention | todo | high | 34 |
| 41 | Port 140 entities | todo | high | 40, 20 |
| 42 | Value objects, enums, domain events | todo | high | 41 |
| 43 | One configuration class per entity | todo | high | 41 |
| 44 | Audit, soft-delete, UTC interceptors | todo | high | 43 |
| 45 | Optimistic concurrency | todo | high | 43 |
| 46 | Squash 316 migrations — **preserve temporal tables** | todo | med | 43 |
| 47 | Schema-diff verification | todo | high | 46 |
| 48 | Integration-test database and seed data | todo | high | 46, 36 |

### Phase 4 · Platform → **M3**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 49 | Permissions become claims | todo | high | 22, 34 |
| 50 | Permission policies enforced server-side | todo | high | 49 |
| 51 | Resource-based authorization | todo | high | 50 |
| 52 | One authentication scheme | todo | high | 34 |
| 53 | Refresh, revocation, real sign-out | todo | high | 52 |
| 54 | Two-factor and password reset rebuilt | todo | med | 52 |
| 55 | Typed results and problem details | todo | high | 34 |
| 56 | Validation pipeline | todo | high | 55 |
| 57 | Clock abstraction, UTC everywhere | todo | high | 34 |
| 58 | API versioning and OpenAPI | todo | med | 55 |
| 59 | Security headers, CSRF, rate limits, health | todo | med | 52 |

### Phase 6 · Front end → **M4**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 74 | Design tokens and base stylesheet | todo | med | 32 |
| 75 | Layout decomposed — 249 KB today | todo | high | 74 |
| 76 | Navigation and permission-aware menu | todo | high | 75, 49 |
| 77 | Form, grid and modal components | todo | high | 74 |
| 78 | Permission component sharing API policies | todo | high | 76, 50 |
| 79 | TypeScript and bundling — **needs Node.js** | todo | med | 74 |
| 80 | Approval inbox rebuilt — 197 KB today | todo | high | 77, 62 |
| 81 | Accessibility and keyboard pass | todo | med | 77 |
| 82 | Delete dead assets — PHP stack, IE shims | todo | low | 75 |

### Phase 7 · Jobs → **M5**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 83 | One worker host replacing 7 console apps | todo | med | 34, 48 |
| 84 | Import definitions in configuration | todo | med | 83 |
| 85 | Correct exit codes — all return zero today | todo | high | 83 |
| 86 | Idempotent, resumable, batched, dry-run | todo | med | 84 |
| 87 | Retire the old uploaders and trigger stub | todo | low | 85, 86 |

### Phase 8 · Reporting → **M5**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 88 | Report definitions with an allow-listed schema | todo | high | 43 |
| 89 | Typed, parameterised execution | todo | high | 88 |
| 90 | Typed repositories over the existing procedures | todo | med | 18, 43 |
| 91 | A proper grid contract | todo | med | 90, 77 |

### Phase 9 · Hardening and cutover → **M6**

| # | Task | Status | Pri | Needs |
|:--|:--|:--|:--|:--|
| 92 | Query and index pass | todo | med | 60, 73 |
| 93 | Async verified end to end | todo | med | 60, 73 |
| 94 | Load test | todo | med | 92 |
| 95 | Security review against the assessment | todo | high | 60, 73 |
| 96 | Containerised deployment | todo | med | 38 |
| 97 | Operations runbook | todo | med | 96 |
| 98 | Data migration rehearsal | todo | high | 47, 95 |
| 99 | Cutover | todo | high | 97, 98 |

### Groundwork · complete

| # | Task | Status |
|:--|:--|:--|
| 1 | Current architecture documented | done |
| 2 | Defects and risks assessed | done |
| 3 | Target architecture and plan agreed | done |
| 4 | Work tracker and status page | done |

---

## 7 · Effort model

Working days, one developer with me. Ranges, not promises — the spread narrows as Phase 1 lands.

| Phase | Scope | Days | Confidence |
|:--|:--|:--|:--|
| 0 | Isolation and safety | 3 – 4 | High |
| 1 | Behaviour capture, 16 modules | 15 – 22 | Medium |
| 2 | Skeleton and CI | 4 – 6 | High |
| 3 | Domain and persistence | 10 – 15 | Medium |
| 4 | Platform | 10 – 14 | Medium |
| 5 | Module migration, 16 modules | 55 – 85 | Low until Phase 1 lands |
| 6 | Front end | 15 – 22 | Low |
| 7 | Jobs | 4 – 6 | High |
| 8 | Reporting | 5 – 8 | Medium |
| 9 | Hardening and cutover | 10 – 15 | Low |
| | **Total** | **131 – 197** | |

**Read this honestly.** Roughly six to nine months of focused solo work. The three phases that
move the number most are 5, 6 and 9, and all three get more predictable once Phase 1 is written.
Phases 0–2 are ~10 days and deliver disproportionate value: the security holes close, the build
stops being machine-locked, and CI exists for the first time.

**Cheapest way to shrink it:** master data is ~40 of the ~190 screens and collapses into one
pattern. Documents are 15 more that collapse into one engine. That is a quarter of the surface for
a fraction of the effort.

---

## 8 · Risks, assumptions, issues, dependencies

### Risks

| # | Risk | Impact | Likely | Mitigation |
|:--|:--|:--|:--|:--|
| R1 | Business rules exist only in 275 KB controllers; the rewrite reproduces them wrongly | **High** | High | Phase 1 is mandatory; every module gated on your review before code |
| R2 | Migration squash destroys temporal-table history | **High** | Medium | `#46` preserves system-versioning; `#47` schema-diff verifies before anything is discarded |
| R3 | The foreign-key vs free-text decision is made wrongly, affecting 20 entities | **High** | Medium | `#106` — explicitly asked, not assumed |
| R4 | Billing rebuilt without real data to verify against | **High** | Medium | Blocked on a database restore; worked examples from `#29` become tests |
| R5 | SAP unavailable, integration slips | Medium | High | `#35` gateway + fake means only `#73` is affected |
| R6 | Solo developer — no second reviewer, no cover | Medium | High | Tests and CI substitute for a reviewer; every module lands complete |
| R7 | Scope fatigue across 190 screens and ~6–9 months | Medium | Medium | Milestones deliver standalone value; no half-migrated state ever exists |
| R8 | Leaked credentials exploited before rotation | **High** | Low | `#15` — needs you; cannot be fixed from the repo |

### Assumptions

| # | Assumption | If wrong |
|:--|:--|:--|
| A1 | Local-only; no production system depends on this repo today | Sequencing changes completely — cutover planning moves first |
| A2 | The database schema stays compatible; column names are preserved | Phase 3 grows a data-migration workstream |
| A3 | You review at each gate | The method fails — this is the control that removes R1 |
| A4 | The domain model is worth keeping | Strategy reverts to a decision, not an assumption |

### Issues — open now

| # | Issue | Owner | Effect |
|:--|:--|:--|:--|
| I1 | ~~No version control~~ **Resolved 2026-08-17** — local-only repo on `main` (no remote, deliberate) | — | `#14`, `#147` unblocked; `#38` CI stays blocked (needs a remote the user does not want) |
| I2 | Live credentials in git history | You | Cannot be un-leaked by deleting files |
| I3 | ~~`HRMS_Web` will not compile here~~ | — | **Resolved 2026-08-04** by the `SapIntegration` build switch (`#134`). Only the 3 SAP screens are unavailable |
| I4 | Local database has schema but **no data** | You | `PMS_Local` built and seeded with one admin. Billing and parity checks still need a real restore |
| I6 | Migrations and model have drifted — 235 tables short a column | Me | `#136`. Affects `#46` squash and `#47` schema-diff |
| I7 | **PHP, Composer and MySQL absent** — the Land app cannot run or be tested here | You | Blocks verifying the Land half of `#145`. The launcher ships regardless, with the tile disabled |
| I8 | `test_Land_mgt\.env` is committed, with `APP_KEY` and DB settings | You | Same class as I2, in the other repository. Out of scope for `#138`; recorded so it is not lost |

~~I5 — `docs/roadmap.html` layout partly broken.~~ Fixed 2026-08-03: `li` was a grid container, so
loose text after each `<b>` became a separate grid item and wrapped into a 12px column. Rebuilt
block-level and the charter content restored to the page.

### Dependencies on you

| Item | Blocks | When |
|:--|:--|:--|
| PHP 8, Composer, MySQL (XAMPP or Laragon) | Verifying the Land tile; any SSO work | Stage I |
| **Rotate the leaked credentials** | Nothing technically — they are simply live | **Now** |
| A real database restored locally | `#29`, `#69`, every parity check | Before Phase 1 ends |
| Node.js LTS | `#79` only | Before Phase 6 |
| SAP client and DI API | `#73` only | Before Phase 5 ends |
| Your knowledge of the business rules | The accuracy of the whole rebuild | Throughout Phase 1 |

---

## 9 · Definition of Done

A module is not done until every line is true.

- [ ] Behaviour spec written and reviewed by you, before any code
- [ ] Domain rules in the domain layer — not in a controller
- [ ] Every command validated server-side
- [ ] Every endpoint carries a permission policy, enforced on the server
- [ ] Correct HTTP status codes; no exception text reaches the caller
- [ ] Unit tests for rules, integration tests for persistence, endpoint tests for authorization
- [ ] Parity-checked against the legacy behaviour, with the differences listed and accepted
- [ ] No new compiler warnings — the build treats them as errors
- [ ] Legacy controller, view and routes removed in the same change
- [ ] `docs/modules/<module>.md` updated to describe the new behaviour
- [ ] `PROJECT.md` updated

---

## 10 · Metrics — baseline to target

Measured 2026-08-03. These are how we prove the rebuild worked.

| Measure | Today | Target |
|:--|:--|:--|
| Automated tests | **0** | Every module covered |
| Continuous integration | **None** | On every push |
| Machines that can build it | **1** | Any |
| Unauthenticated SQL endpoints | ~~2~~ **0** since 2026-08-14 | 0 |
| Controllers | 152 | ~60 |
| Largest controller | 268 KB | < 10 KB |
| Largest view | 197 KB | < 200 lines |
| Endpoints with no permission check | ~all | 0 |
| `DateTime.Now` calls | 739 | 0 |
| Synchronous `SaveChanges` | 667 | 0 |
| Exception messages returned to clients | 804 | 0 |
| Structured logging | 4 references | Throughout |
| Navigation defined in | 243 KB Razor | a registry table |
| Menu depth | 4 levels | 2 |
| Forms unreachable from the menu | 15 | 0 |
| Duplicated menu links | 22 | 0 |
| Wrong or dead menu links | 4 | 0 |
| Migrations | 316 | 1 baseline |
| Repository size | 147 MB `.git` | < 20 MB |

---

## 11 · Decision log

| # | Decision | Date | Rationale |
|:--|:--|:--|:--|
| D1 | Full rewrite, reusing the domain model and approval engine | 2026-08-03 | The model is sound; the delivery around it is not |
| D2 | .NET 10 LTS | 2026-08-03 | .NET 6 went end-of-life 2024-11-12 |
| D3 | Razor with a modern component structure — no SPA | 2026-08-03 | Server-rendered suits the forms; avoids a second stack |
| D4 | Local only; every remote connection severed | 2026-08-03 | Your instruction; also removes all outbound risk |
| D5 | SAP isolated behind one interface in one assembly | 2026-08-03 | The only way the solution builds and tests without SAP |
| D6 | One base entity convention, column names preserved | 2026-08-03 | Two conventions is why no cross-cutting behaviour is possible |
| D7 | `Result<T>` and `ProblemDetails` replace the universal envelope | 2026-08-03 | HTTP 200 on failure defeats monitoring and clients |
| D8 | Block is the first slice, and the master-data pattern | 2026-08-03 | Smallest complete slice; validates the approach cheaply |
| D9 | **Block becomes a foreign key, not free text**, and is **scoped to its parent** — uniqueness on *(parent, name)* | 2026-08-13 | **Closed by you** in `AI file.xlsx` Points 6.2 ("do what is standard practice") and 6.3 ("Do the needful"). Affects 20 entities; `#106` decided, `#105` still to run. The existing duplicate-block data is a live problem, not a rebuild question |
| D26 | **Decisions are final; implement without per-item review stops** — the one-at-a-time gate is retired for the restructuring programme. Emerging requirements extend the architecture, they do not restart analysis | 2026-08-16 | Instructions sheet §1, §2, §31 — "Do the Work, Do Not Just Document It" |
| D27 | **Development order is the workbook's §30**: Home screen → registry/architecture → navigation → restore forms → security → search → database configuration → refinement | 2026-08-16 | Instructions §30, §32 |
| D28 | **Hidden forms settled**: 10 restored with placements (Charges Group Form renamed "Form Wise Charges"); the other 5 leave scope, kept in code until the final stage. Near-duplicate pairs both stay until Phase 8 | 2026-08-16 | HIdden Forms sheet + Instructions §8–§10 |
| D29 | **Recent and Favourites: header icons (top-left, after the menu icon) AND the My Home cards** — one shared store behind both | 2026-08-16 | Instructions §18 first said navigation-only; **amended by you the same day**: "these shall be included in top header left side after menu icon… want them on both at header and my home" |
| D30 | **Administration = 5 sub-modules** (System Configuration, Setup, Implementation Center, Organizational Governance, Analytics Development); General Settings folds into System Configuration; Implementation Center under Administration; the §10 BI tree is out of scope; "Setup - Sub Module" is **Setup** with 8 structured children | 2026-08-16 | Folders Structure sheet; closes G-1, G-2, G-3, G-6 |
| D31 | **Records & File Management sits under Property Business Operations** as-is, second-last (before Biometric Management) — **4 top-level modules** remain | 2026-08-16 | Your instruction: "move records and file management to Property business operations as it is … second last menu item". Amends the D20/D30 5-module top level |
| D32 | **"Property Business Operations" is renamed "Estate Operations"** — DisplayName-only change in the `NavigationNodes` registry (seed json + live rows); `FormKey`, `PermissionKey` and the seed `path` linkage stay untouched, so permissions and hierarchy are unaffected | 2026-08-17 | Your instruction: "rename module name: Property Business Operations to Estate Operations". First real use of the D22 rename-follows-the-registry mechanism |
| D24 | **The rename-revokes-permission risk is accepted** — sole user is admin with full access during the rephasing. Renames before the registry exists must update the matching `PermissionForms.Name` rows in the same change; the registry (D22) stays the durable mechanism | 2026-08-14 | `New Instructions.xlsx` row 1, your words. Softens D22's hard sequencing without withdrawing it |
| D25 | **Charges→Revenue renames only the word "Charge"** — `Surcharge Setup` and `Fixed Charges Bill Generation` are excluded by name; words merely containing "charge" are never touched. All other mapping-sheet renames required | 2026-08-14 | `New Instructions.xlsx` row 2 |
| D20 | **Five top-level modules**, up to **5 menu levels** — your structure in `AI file.xlsx`, PMS-Modules sheet | 2026-08-13 | **Supersedes D14** (12 modules) and withdraws D10's "nothing above two clicks". Points 3, 4, 7 and 12 |
| D21 | Land Management integration is the **last** task; only the APPS tile region ships early | 2026-08-13 | Restructure!B367. Stands `#140`–`#146` down without unlocking D15–D19 |
| D22 | **The registry is built before anything is renamed** — `DisplayName` separated from `PermissionKey` | 2026-08-13 | The menu label *is* the permission key (§1.4 of `05-MODULE-ARCHITECTURE.md`). Renaming 58 forms first would silently revoke them for every role. Your requirement 8.2 (Form Alias configuration) is the mechanism |
| D23 | Size is no longer a target — "largest view < 200 lines" and "largest controller < 10 KB" are withdrawn | 2026-08-13 | Points 14: *"Our object shall not be to lower file sizes and compromize on quality."* The real targets are no business logic in controllers and no duplicated markup |
| D10 | Module-workspace navigation — rail, module page, forms grouped by item type | 2026-08-04 | Two clicks to anything; kills the current 4-level tree |
| D11 | Setup lives in its owning module, plus a central Configuration index | 2026-08-04 | Configure in context; an admin does not walk twelve modules |
| D12 | Menu, permission catalogue and API policies read one form registry | 2026-08-04 | Today the menu label *is* the permission key, matched by string |
| D13 | Modules and sub-areas are data — added and renamed without code | 2026-08-04 | Your instruction: the module set grows as features are added |
| D14 | Twelve top-level modules; sub-areas carry the detail | 2026-08-04 | Fits the rail without scrolling |
| D15 | REMS launcher as an additive shim in HRMS_Web; app list as configuration | 2026-08-05 | The two products share no technology. A front door is buildable now; a merge is not. **Q1: one front door, not one codebase** |
| D16 | PMS is the identity authority; Land account linking deferred, no credential ever forwarded | 2026-08-05 | `PMSUser` and Laravel `users` are unrelated tables in different engines. **Q2: second sign-in accepted for now**; SSO gets its own gate |
| D17 | Rebrand on the launcher only; both 250 KB layouts otherwise left alone | 2026-08-05 | `#125` replaces the PMS layout anyway; renaming it twice is waste |
| D18 | One "Switch app" anchor may be added to `_Layout.cshtml` — the menu block stays untouched | 2026-08-05 | **Q3.** Without it the launcher is a screen you pass once per login, not a place you return to |
| D19 | The two repositories merge into one, `RealEstate/{property,land}` — **blocked on `#13`** | 2026-08-05 | **Q4.** Matches the single-product framing. Independent of the launcher, which reaches Land by URL, not file path |

**Changing a locked decision means updating `docs/03-REENGINEERING-PLAN.md`, not working around it.**

---

## 12 · Documents

| File | Holds |
|:--|:--|
| `docs/01-SYSTEM-OVERVIEW.md` | Current architecture, domains, scale |
| `docs/02-ASSESSMENT.md` | Verified defects, worst first |
| `docs/03-REENGINEERING-PLAN.md` | Target architecture, locked decisions |
| `docs/04-WORK-INVENTORY.md` | 16 modules, ~190 screens, ~60 processes — the pick-list |
| `CURRENT-WORKS.md` | **The bench — what is active this week and what is waiting on you** |
| `docs/AI-FILE-OBSERVATIONS.md` | **`AI file.xlsx` analysed — requirements, superseded decisions, gaps, the hidden-form register, the staged plan** |
| `docs/05-MODULE-ARCHITECTURE.md` | Module taxonomy, the shell, every form mapped. **§5's 12-module taxonomy is superseded by D20** — being rewritten to your 5 modules |
| `docs/modules/block.md` | First deep-dive — also waiting on your review |
| `docs/modules/rems-app-launcher.md` | **REMS app launcher — requirements, C4, 3 ADRs, tasks, 5 questions. Ready for your review** |
| `docs/WORK-LOG.md` | **Every change, finding and decision, session by session — and how to reverse each one.** Until git exists, this is the only change history |
| `docs/roadmap.html` | Status page; open locally in a browser |
| `AGENTS.md` | The repository's binding work contract |

---

## 13 · Log

**2026-08-16 (evening, parcels 3–9)** — **The header shell is delivered and refined live with
you** (`#150`, `#152`, `#154` closed; 14 commits `a66811a5`→`4bb22e53`). Shipped: Recent &
Favourites header icons + popovers with a working favourite-marking mechanism (star on every
form's title strip); the merged single form header replacing the triple-stacked bars on all
forms; Generate Alert (N-5) with `api/Workspace` endpoints; the centered global search with
form + record groups and deep-links into Reference No. Profile / Member Profiling / Dealer
Profiling; registry depth/tint/indent fixes; the accessibility pass from the
web-design-guidelines audit; the rebrand to N-Stack; and the single-vector Property
Management System brand mark. Records & File Management moved under Property Business
Operations (D31). Full narrative: `docs/WORK-LOG.md` §7.6–§7.12.

**2026-08-16 (second parcel)** — **The registry exists and the navigation runs on it** (`#153`,
closing `#121`–`#124` and `#129`–`#133`). `NavigationNodes` seeded with 223 nodes from the
workbook (5 modules · 51 groups · 172 forms — the mapping sheet, the Folders Structure tree,
and the 10 restored hidden forms). The 2,460-line hard-coded menu in `_Layout.cshtml` is
replaced by a registry-driven partial; renames now display everywhere while permissions stay on
the original check keys — 9 label-vs-key aliases were recovered from the old layout in git and
parity is proven by query (0 unmatched, 0 admin gaps). Legacy menu defects `#130`–`#132` died
with the swap. Detail and reversal: `docs/WORK-LOG.md` §7.5.

**2026-08-16** — **`AI file.xlsx` updated to 6 sheets; every open decision closed; building
ordered and begun.** The new Instructions sheet is a final directive (D26, D27): Home screen
first. Stage A closed via D28–D30. Git found installed — `#13` done, I1 resolved. **`#148`
shipped**: the My Home SAP Fiori-style workspace tab is the default landing view — Overview
Analytics KPI cards mirroring the Dashboard tab, live To-Dos counts from the new
`api/Workspace/GetMyHomeSummary`, APPS tiles from `RealEstate:Apps` configuration (the config
half of D15), Recent and Favourites backed by a layout-level tracker. Build 0 errors; verified
by authenticated HTTPS probes. Detail and reversal: `docs/WORK-LOG.md` §7.

**2026-08-14** — **First build work of the programme, at your instruction: front end first,
security first, nothing that can crash the solution.** `#5` and `#6` closed — the two
arbitrary-SQL endpoints now require a signed-in user, and `ExecuteParamQuery` accepts only a
single SELECT. `#113` closed — Block deletion answers POST only. `#114` closed — every
HomeController screen (~50) redirects anonymous visitors to the login page via the new
`SessionAuthorizeAttribute`. Sign-out clears the whole session; baseline security headers on every
response. Verified live on a throwaway `:5218` instance — detail and reversal in
`docs/WORK-LOG.md` §6. Also read `New Instructions.xlsx` → D24, D25 locked.

**2026-08-03** — Decisions D1–D8 locked. Charter, assessment, plan, inventory and the first
module deep-dive written. No solution code changed.

**2026-08-03** — Block found to be a **SQL Server temporal table**. Change history is already
captured by the database. `#46` must preserve system-versioning or that history is destroyed.
`#105` checks how many other entities are affected.

**2026-08-03** — Correction to an earlier note: Block has **no** parent relationship to Sector. It
references nothing, and 20 entities store the block *name* as free text. In `StockCreation` — the
hub of the model — the foreign key was written and then commented out. Raised as D9.

**2026-08-03** — Working method changed at your request: markdown only, one item at a time,
depth over speed, hard stop at your review before anything is built. Recorded in `AGENTS.md` and
in the project-management skill so it survives into future sessions.

**2026-08-04** — Session closed. `docs/WORK-LOG.md` created: every file added, changed and
discovered across both sessions, what each change bought, and how to reverse it. With no git on
this machine that file is the repository's only change history — it gets appended at the end of
every session until `#13` lands.

**2026-08-04** — **The application runs on this machine for the first time**, at
`http://localhost:5217`. Three things had to be true and none of them were: it had to compile
without SAP, a database had to exist, and a user had to exist. All three are now done and
recorded in §4b. Two findings came out of it that matter more than the run itself — `dotnet
build` could never have built this project (`MSB4803`), and the 316 migrations are missing a
`BaseModel` column on 235 of 439 tables.

**2026-08-04** — Navigation and module structure taken through the gate. Measured: 209 real forms
across 32 meaningless folders, a 243 KB hard-coded menu with 22 top-level groups four levels deep,
200 links to 178 targets. Sixteen defects verified, including a fake reports subtree, four wrong or
dead links, and a form named *Test* in the live menu. Fifteen working forms are unreachable.

**2026-08-04** — The structural finding: `Permissions.FormName` is a string and the layout checks
`Html.UserHavePermission("<menu label>")`, so **the menu label is the permission key**, and
`PermissionForms` is flat with no module concept. Renaming a menu item revokes access. D12 makes a
registry the single definition for menu, permissions and API policy.

**2026-08-04** — Shell decided with you: module-workspace model, setup in-module plus a central
Configuration index, twelve extensible top-level modules. `#103` (Block review) moves to `blocked`
so `#119` holds the single active slot; it is still waiting on you.

**2026-08-05** — You asked for an Odoo-style app chooser after login, joining Land Management and
Property Management as **Real Estate Management System**. Discovery changed the shape of the task:
`test_Land_mgt` is a **Laravel 8 / PHP / MySQL** application, not a .NET solution — different stack,
different database, different login, different permission model, and a **second approval engine**.
The two cannot be merged in this task. Planned instead as a launcher shim inside HRMS_Web: six
files, one changed line, fully reversible. `docs/modules/rems-app-launcher.md`. Nothing built —
`#138` is the gate, and Q1 asks you directly whether "combine" meant one codebase.

**2026-08-05** — Also found while measuring: PHP, Composer and MySQL are **not installed on this
machine** (I7), so the Land app cannot run here at all; and `test_Land_mgt\.env` is **committed**
with its `APP_KEY` (I8) — the same leak class as I2, in the other repository. Neither is in scope
for `#138`.

**2026-08-03** — You reported `docs/roadmap.html` renders correctly in part and broken in part.
Logged as I5. It is a status mirror only — this file stays the source of truth, so the break costs
nothing while it waits. Repair it before the next status refresh; do not rebuild it from scratch.
