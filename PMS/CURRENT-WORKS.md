# Current works

> The live working sheet — what is on the bench right now, this week.
> Full backlog and history stay in `PROJECT.md`. Durable analysis stays in `docs/`.
> Updated 2026-08-23.

---

## Right now

**Driver:** `AI Files/AI file.xlsx`, updated by you 2026-08-16 — now 6 sheets. The new
**Instructions** sheet is a final directive: every open question answered, decisions final,
implement rather than document, Home screen first. Digest: `docs/AI-FILE-OBSERVATIONS.md` §12.

**Application:** `http://localhost:5217` · sign in `admin` / `admin`
**Database:** `PMS_Blank` on `.\MSSQLSERVER01` — blank production schema, zero rows.
**Git:** fresh **local-only** repo since 2026-08-17 evening — branch `main`, initial commit `6002fb6`. No remote, by your explicit instruction: this repository never leaves this PC.

| | |
|:--|:--|
| **Shipped 2026-08-16** | My Home tab · registry + new navigation · **the full header shell** (search, Recent/Favourites, Generate Alert, refined icons, single-vector brand) · global search with record deep-links · rebrand to N-Stack · accessibility pass · **evening polish pass** (brand lockup centered on the bar, house sized to the wordmark, search text centered, boxed user glyph instead of the avatar photo, Overview Analytics split into Financial & Operations / Inventory / Members & Users segments — `docs/WORK-LOG.md` §8) |
| **Shipped 2026-08-20** | **The monochrome theme** (`#155`) — the full black & white ERPNext-inspired redesign from `C:\Users\Adnan Ahmed\Pictures\Theme` applied everywhere: white sidebar with black icons and ink text, white header with bordered search pill, black primary buttons, light table headers and section panels, black-chip form icons, active-item bottom-line animations (short centered line on forms, full underline on modules), and the rebuilt login page (white card left, animated dark-crystal right). My Home's colorful cards and all Dashboard colors deliberately kept. UI layer only; zero functional change — `docs/WORK-LOG.md` §11 |
| **Shipped 2026-08-23** | **PMS product identity** — the user's skyline-in-frame mark rebuilt as clean geometry (roof now runs under every building; even clearances), **monochrome by rule** (black `#111111` / gray `#7E7C7D` / white on dark, always transparent), full asset set in `wwwroot/img/brand/` (logo / icon / favicon svg+png+ico), wired into the header lockup, app switcher, `/Apps` tile, login card and every PMS page's favicon. Generator + source in `docs/brand/`. Review round: tab title = Property Management System, favicon on a black tile, login headline "Real Estate Management Simplified…", version line gone, nav toggle moved to the bottom-left of the panel (LIMS-style), current-app state on the My Home Apps tile · **six modules**: Construction & Development and Utilities Management promoted out of Estate Operations with new icons (`tools/local-run/promote-construction-utilities-modules.sql`). `docs/WORK-LOG.md` Session 14 · **LIMS** (other repo, same day): My Home landing page as in PMS, Recent/Favourites in header + home + page bar, navigation parity (chevron chips, active trail, icons-only < 1200px, ⋮ More) — `LMIS/docs/WORK-LOG.md` 2026-08-23 · **App Launcher rewording** (evening): bar "Real Estate Management Solution / Applications Library", h1 "Choose the workspace you want to access.", "One login. Multiple applications." lead, "Open Application" tiles, **Recently used** pill (`erp_recent` cookie), coming-soon tiles hoverable with roadmap tooltip; switcher item "Applications Library" in both products — `docs/WORK-LOG.md` Session 15 |
| **Shipped 2026-08-21** | **Header shell brief** (`#156`) — empty box after the form description removed from 11 views · bell badge live (99+, minute refresh) · notifications open as a **panel** with search/page-size/pages, not a page · header one flex row below 1200px with ⋮ More overflow and icon-only search on phones · sidebar auto icons-only below 1200px. Needs your eyeball pass at the widths you use · **login hero vector** (`#157`) — skyline-and-homes illustration in the crystal panel, replacing the logo + name block |
| **Shipped 2026-08-17** | Login page rebrand (N-Stack Consulting name + N logomark, support line) · approval count badge on the inbox icon (new `GetPendingApprovalCount`) · custom double-chevron expand icon (right → rotates down, 220ms) · navigation grid alignment (chevron pinned right on every row) · typography hierarchy (600/500/400) · **stuck-highlight bug fixed** — one active form at a time, marked from the URL, parents stay expanded · expand/full-page controls restored into the form title strip (and now really hide the navigation pane) · favicon swap (login + app now use the N logomark) · **full brand purge** — zero DHA/HCC/consultancy marks left anywhere (66 files, old logo images deleted, letters use the N-Stack mark; rule recorded in `AGENTS.md`) · module renamed **Property Business Operations → Estate Operations** (D32, registry DisplayName only) — `docs/WORK-LOG.md` §9 |
| **Next on the bench** | **`#149` idle-logout blur overlay** (Instructions §21) · `#151` map investigation (§4) |
| **Then** | Stage C — registry-backed authorization management · Form Alias configuration form · audit strip on forms (§13) |
| **Blocked on you** | Nothing — the workbook closed every open decision |
| **Blocked on the machine** | PHP/MySQL (`I7`) — only matters at Stage I (Land Management, last) |

**Where the day ended (14 commits, `20364cf`→`4bb22e53`):** the shell now looks and behaves
like the workbook asks — registry-driven menu with icon chips, chevrons, open-state tint and
proper indentation; one professional form header on every form with breadcrumb, registry name
and favourite star; header with centered white search pill, alert sender, and the
Property Management System vector brand. All verified live; reversals in
`docs/WORK-LOG.md` §7.

**The left menu is new as of 2026-08-16:** your 5-module structure, up to 5 levels, rendered
from the `NavigationNodes` registry — renamed forms show their new names, the 10 restored
hidden forms are in place, and the old menu's dead links, wrong targets, duplicates and fake
subtrees are gone. Permission behaviour is unchanged (proven zero-difference; detail in
`docs/WORK-LOG.md` §7.5). **Sign out and back in once** so your session picks up the new
hidden-form permissions.

---

## Done 2026-08-16 — Stage A closed, Phase 1 started

| Item | Result |
|:--|:--|
| **Workbook update read** — 3 new sheets | All 10 open questions answered; gaps G-1–G-8 closed; hidden-form verdicts in; development order fixed (§30). `docs/AI-FILE-OBSERVATIONS.md` §12 |
| **`#13` git — closed, then reversed** | Was found installed with a GitHub remote 2026-08-16; on 2026-08-17 you removed the repository and asked for every GitHub connection to be stripped from the folder |
| **Registry + navigation** (`#153`) | `NavigationNodes` table, 223 nodes seeded from your workbook; `_Layout`'s 2,460-line menu replaced by the registry partial; permission parity proven; menu defects `#130`–`#132` and the hidden-form verdicts (`#133`) closed |
| **My Home workspace tab** — new default landing view | SAP Fiori-style cards per your UI mock: Overview Analytics (KPI tiles + period deltas, mirrored from the Dashboard tab, permission-gated identically) · To-Dos (alerts / pending approvals / active users, live from the new `api/Workspace/GetMyHomeSummary`) · APPS tiles from `RealEstate:Apps` configuration (PMS current, Land + HRMS coming soon) · Recent · Favourites |
| Recent-forms tracking | Every visited form page recorded (localStorage, max 10); star any recent item to pin it to Favourites. Registry replaces the URL-derived names in Phase 2 |
| Dashboard & Map tabs | Untouched, still one click away — tab toggle generalised to three views |
| Verified | Build 0 errors · authenticated probes over HTTPS: page renders all five cards, summary API returns correct counts (0/0/1 on the blank DB), 401 without a token |

Files changed: `Controllers/api/WorkspaceController.cs` (new) · `Views/Home/Index.cshtml` ·
`Views/Shared/_Layout.cshtml` (recent-forms tracker) · `appsettings.json` (`RealEstate:Apps`).
Reversal instructions: `docs/WORK-LOG.md` §7.2.

---

## Decisions locked from the workbook update (detail in `AI-FILE-OBSERVATIONS.md` §12)

- **Hidden forms:** 10 restored with placements (Charges Group Form → "Form Wise Charges"); the
  other 5 leave the scope, kept in code until the final stage. Near-duplicate pairs both stay
  until Phase 8.
- **Administration:** 5 sub-modules per the Folders Structure sheet; General Settings folds into
  System Configuration; Implementation Center under Administration; §10 BI tree out of scope.
- **"Setup - Sub Module"** is named **Setup** with 8 structured Level-3 children.
- **Recent/Favourites:** header icons top-left after the menu icon **and** the My Home cards —
  your amendment of 2026-08-16 (supersedes the §18 navigation-only reading). Shipped same day.
- **Audit:** meaningful change capture, storage kept reasonable (§14); italic audit strip on
  every applicable form (§13).
- **Working method:** implement, don't stop for review rounds — the per-item gate is retired for
  this programme (§2, §31).

---

## Next up, in order (Instructions §30)

| Phase | What | State |
|:--|:--|:--|
| **1** | **Home screen** — My Home tab ✅ · idle-logout overlay · header strip · map investigation · search foundation | ← **here** |
| 2 | Architectural foundation — form registry, stable IDs, module registry, navigation architecture, permission registry, centralized server-side authorization | |
| 3 | Navigation restructuring to the Folders Structure tree (≤5 levels), labels, dead links | |
| 4 | Restore the 10 kept hidden forms into navigation with registry entries | |
| 5 | Security — module/form/action permissions, endpoint protection, session timeout + re-login overlay, audit trail | |
| 6 | Extensible enterprise search | |
| 7 | Secure database configuration | |
| 8 | Refinement — retire what you mark, naming, error messages, UI consistency | |
| I | Land Management — last, unchanged | |

---

## Temporary scaffolding to remove later

| File | Purpose | Remove when |
|:--|:--|:--|
| `HRMS_Web/Controllers/HiddenFormsController.cs` | Serves `/HiddenForms` | The 10 kept forms are in the registry-driven navigation (Phase 4) |
| `HRMS_Web/Views/HiddenForms/Index.cshtml` | The review page | Same |

Not scaffolding — these stay until the final stage per Instructions §10:
`DemandNoteController.PurchaseRequest()` and `Operations.TransferForm()`, both marked in place.
