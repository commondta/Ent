# DOX framework

- DOX is highly performant AGENTS.md hierarchy installed here
- Agent must follow DOX instructions across any edits

## Core Contract

- AGENTS.md files are binding work contracts for their subtrees
- Work products, source materials, instructions, records, assets, and durable docs must stay understandable from the nearest applicable AGENTS.md plus every parent AGENTS.md above it

## Read Before Editing

1. Read the root AGENTS.md
2. Identify every file or folder you expect to touch
3. Walk from the repository root to each target path
4. Read every AGENTS.md found along each route
5. If a parent AGENTS.md lists a child AGENTS.md whose scope contains the path, read that child and continue from there
6. Use the nearest AGENTS.md as the local contract and parent docs for repo-wide rules
7. If docs conflict, the closer doc controls local work details, but no child doc may weaken DOX

Do not rely on memory. Re-read the applicable DOX chain in the current session before editing.

## Update After Editing

Every meaningful change requires a DOX pass before the task is done.

Update the closest owning AGENTS.md when a change affects:

- purpose, scope, ownership, or responsibilities
- durable structure, contracts, workflows, or operating rules
- required inputs, outputs, permissions, constraints, side effects, or artifacts
- user preferences about behavior, communication, process, organization, or quality
- AGENTS.md creation, deletion, move, rename, or index contents

Update parent docs when parent-level structure, ownership, workflow, or child index changes. Update child docs when parent changes alter local rules. Remove stale or contradictory text immediately. Small edits that do not change behavior or contracts may leave docs unchanged, but the DOX pass still must happen.

## Hierarchy

- Root AGENTS.md is the DOX rail: project-wide instructions, global preferences, durable workflow rules, and the top-level Child DOX Index
- Child AGENTS.md files own domain-specific instructions and their own Child DOX Index
- Each parent explains what its direct children cover and what stays owned by the parent
- The closer a doc is to the work, the more specific and practical it must be

## Child Doc Shape

- Create a child AGENTS.md when a folder becomes a durable boundary with its own purpose, rules, responsibilities, workflow, materials, or quality standards
- Work Guidance must reflect the current standards of the project or user instructions; if there are no specific standards or instructions yet, leave it empty
- Verification must reflect an existing check; if no verification framework exists yet, leave it empty and update it when one exists

Default section order:
- Purpose
- Ownership
- Local Contracts
- Work Guidance
- Verification
- Child DOX Index

## Style

- Keep docs concise, current, and operational
- Document stable contracts, not diary entries
- Put broad rules in parent docs and concrete details in child docs
- Prefer direct bullets with explicit names
- Do not duplicate rules across many files unless each scope needs a local version
- Delete stale notes instead of explaining history
- Trim obvious statements, repeated rules, misplaced detail, and warnings for risks that no longer exist

## Closeout

1. Re-check changed paths against the DOX chain
2. Update nearest owning docs and any affected parents or children
3. Refresh every affected Child DOX Index
4. Remove stale or contradictory text
5. Run existing verification when relevant
6. Report any docs intentionally left unchanged and why

## User Preferences

When the user requests a durable behavior change, record it here or in the relevant child AGENTS.md

- **Branding is N-Stack only** (set 2026-08-17): the marks "DHA", "Defence Housing Authority",
  "HCC" and "Hussain Chaudhry/Chaudhary Consulting" must not appear anywhere — code, UI,
  document templates, assets or docs. Replace on sight with **N-Stack** (logo:
  `HRMS_Web/wwwroot/img/n-stack-logo.png`). Personal names that merely contain "Hussain" are
  not marks and stay.
- **PMS product identity** (set 2026-08-23, monochrome rule added the same day): the PMS product
  mark is the skyline-in-frame with roof supplied by the user (`docs/brand/pms-mark-source.svg`),
  regenerated as clean geometry by `docs/brand/pms-mark-generator.py` into
  `HRMS_Web/wwwroot/img/brand/` — `pms-logo.svg` / `-white` / `-current` (framed mark),
  `pms-icon.svg` + `pms-icon-plain(-white|-current).svg` (frameless mark for header, switcher,
  Apps tile, login card) and `favicon.svg` + PNG 16–512 + `.ico` (also copied to
  `wwwroot/favicon.ico|png`). **Only black, white or gray, background transparent** — with one
  exception the user asked for: the **favicon** sits on a black rounded tile (white mark) so it
  reads in a browser tab; every other asset is the bare mark (ink `#111111`, roof `#7E7C7D`, white
  on dark grounds; no colour fills, no artboards). Every PMS page uses these for `<link rel="icon">`
  and the brand; the browser **tab title is "Property Management System"** (pages may prefix
  `ViewData["Title"]`), never the company name; N-Stack stays the **company** mark ("Powered by",
  `/Apps` bar). Change the mark by editing the generator and re-running it, never by hand-editing
  the SVGs.
- **Payroll mark** (set 2026-08-28): Payroll Management uses the "group of people" silhouette generated by
  `C:\Users\Adnan Ahmed\Pictures\Payroll\Payroll_HCC2\brand\payroll-mark-generator.py` (logo: ink on
  transparent; icon/favicon: white on charcoal `#242729`); PMS draws it inline in `_ErpAppSwitcher.cshtml` and
  `Views/Apps/Index.cshtml` (`.erp-logo-people`). The skyline mark is PMS-only.
- **Local-only git** (set 2026-08-17, reaffirmed 2026-08-21): a fresh local repository exists
  on branch `main`. It must never leave this PC — do not add a remote, push, publish, mirror,
  or upload the solution to **any** repository or hosting service, private or public (GitHub,
  Azure DevOps, GitLab, Bitbucket, or anything else), unless the user explicitly asks. Guards:
  `.git/hooks/pre-push` refuses every push and `push.default=nothing` is set locally; leave
  both in place. `docs/WORK-LOG.md` remains the narrative record.
- **No "github" strings in the tree** (set 2026-08-21): the word must not appear in code, config
  or vendor assets — `.gitignore` template links were deleted, vendor license/URL comments were
  rewritten to the neutral token `[upstream]`, and the unused `github` icon glyphs were dropped
  from font-awesome, entypo and raphael. New vendor files must be scrubbed the same way before
  they land. Project docs (`docs/`, `PROJECT.md`, `CURRENT-WORKS.md`, this file) may still name
  GitHub when recording the rule or the history.
- **Shell UI rules** (set 2026-08-21 by the brief `C:\Users\Adnan Ahmed\Pictures\2. Remove the
  Extra Box After Form.txt`): no empty boxes — a `card p-3` exists only when it holds content (an
  empty `menu` div is not content); notifications open in the header **panel**, never as a full
  page from the bell; the header stays one clean row at every width — progressive disclosure
  into the ⋮ More menu, with the brand and user always visible; the navigation runs icons-only
  below 1200px. **Amended 2026-08-23:** the navigation-size toggle is the "Collapsed View"
  button pinned at the **bottom-left of the navigation panel** (as in LIMS), not a header
  hamburger — `#menu_m` is hidden. Reuse the existing handlers/APIs; never a second system. **Amended 2026-08-28:** the tree starts with a
  Home entry and a "Modules" label (as Payroll); My Home's Apps card shows each application's mark, not initials.
- **Login page copy** (set 2026-08-23): hero headline "Real Estate Management Simplified.
  Operations Streamlined."; no version line in the crystal panel.
- **ERP solution mark** (set 2026-08-23): the user's gray-tower / blue-roof / two-houses mark
  (`HRMS_Web/wwwroot/img/brand/erp/erp-mark.svg`, favicon set beside it) is the identity of the
  solution-wide surfaces — the single login page (card logo, `erp-mark-mono.svg`, no box) and the
  App Launcher (`/Apps`: bar logo `erp-mark-white.svg` on the dark bar, title "Real Estate
  Management Solution", subtitle "Applications Library"); their favicon is the simple home icon
  (`erp/home-favicon.*`). Application pages keep their own marks (PMS: `img/brand/`). The header
  switcher's first item is "Applications Library" (→ `/Apps`) — same label in the LIMS switcher.
- **App Launcher copy & behaviour** (set 2026-08-23): h1 "Choose the workspace you want to
  access."; lead "You can switch applications anytime from the workspace menu." (one line — the
  "One login. Multiple applications." block was removed 2026-08-23); tile action "Open Application"; live
  tiles the browser opened before carry a "Recently used" pill (`erp_recent` cookie, max 5 codes,
  90 days, written by `/Apps/Go`); coming-soon tiles stay hoverable roadmap tiles with a tooltip
  ("Planned — {Name} arrives in a later phase of the rollout."), never `pointer-events:none`.
- **SSO entry** (set 2026-08-23): a live central session (`erp_sso`) must never see the PMS login
  form — `Login.Index` signs the PMS session in from it (`SignInPmsFromCentral`); other
  applications link to PMS through `/Apps/Go?code=PMS`. The `erp_sso` cookie is a browser-session
  cookie unless the user ticks Remember me (then `Erp:SessionHours`) — closing the browser must end
  the signed-in state.
- **My Home Apps card** (set 2026-08-23, both products): the application currently open carries
  the `is-current` state — ink outline + inset bar, a live dot before "Current" and a slow
  breathing ring (monochrome; reduced-motion safe).
- **No browser-extension automation** (reconfirmed 2026-08-20 after a one-off user-requested
  session): verify changes with builds and HTTP probes, not the Chrome extension.
- **Monochrome theme** (set 2026-08-20): the UI follows the black & white ERPNext-inspired spec
  in `C:\Users\Adnan Ahmed\Pictures\Theme` — white first, black for focus, gray for structure;
  tokens in the `:root` block of `HRMS_Web/wwwroot/css/customStyling.css`. No brown or strong
  accent colors anywhere. Exceptions by explicit instruction: **My Home's colorful cards and all
  Dashboard-tab colors stay as they are**; semantic red (errors, badges) and the amber favourite
  star stay. Icon containers on form pages are black chips with white icons; navigation-pane
  icons are black. New screens must use the tokens, not literals.

## Child DOX Index

- `tools/AGENTS.md` — local development environment scripts (`tools/local-run/`): database
  creation, schema-drift patch, seed data, and reversal artifacts. Throwaway and regenerated,
  never hand-edited, and never holding a credential.

Create a further child when another folder becomes a durable boundary with its own rules.

Solution: `HRMS.sln`

Projects under this root, currently owned by this doc:

- `HRMS_Web` — web application
- `B_DB_Context` — database context layer
- `B_DB_Model` — data models
- `B_Utility` — shared utilities
- `AutoTriggerService` — background/trigger service
- `MemberUploader`, `NewMemberUploader`, `UpdateMemberProfile` — member data import and profile updates
- `StockDataUploader`, `NewStockUploader`, `UpdateStockUploader`, `DeleteStockUploader` — stock data import and maintenance

Root-owned files: `HRMS.sln`, `.gitattributes`, `.gitignore`, `CLAUDE.md`, `PROJECT.md`, `CURRENT-WORKS.md`, and root-level project documentation.

`AI Files/` holds the requirement workbooks supplied by the user. They are **source material, not
work products** — never edited from here. Analysis of a workbook goes into `docs/`, not back into
the spreadsheet.

`tools/` is owned by `tools/AGENTS.md`.

## Related system, outside this tree

`C:\Users\Adnan Ahmed\Pictures\test_Land_mgt` — the **Land Management** application. A separate
repository and a separate stack: **Laravel 8 / PHP / MySQL**, with its own login, its own permission
model and its own approval engine. It is **not owned by this doc and must not be edited from here.**

The two products are being presented as one, **Real Estate Management System**, through a launcher
inside `HRMS_Web` — see `docs/modules/rems-app-launcher.md`. That launcher is the only sanctioned
coupling: PMS reaches Land by configured URL and nothing more. Merging code, databases, users or
the two approval engines is out of scope until a decision says otherwise.

**Sequencing, set 2026-08-13 by `AI file.xlsx`:** Land Management integration is the **last** task,
after PMS matures. Only the APPS tile region on My Home ships early, as part of the shell.

`PROJECT.md` is the live work tracker: one task table, statuses `todo doing blocked done`. Read it
before starting work and update it in the same pass as the work itself. It records current state;
`docs/` records durable engineering knowledge.

`CURRENT-WORKS.md` is the near-term working sheet at the repository root: what is on the bench this
week, what was finished today, and what is waiting on the user. It points at `PROJECT.md` for the
full backlog and never duplicates it. Refresh it in the same pass as the work.

## Project Documentation

`docs/` holds the durable engineering record for the re-engineering effort. Read it before
structural work rather than re-deriving the codebase:

- `docs/01-SYSTEM-OVERVIEW.md` — current-state architecture, domains, scale, build reality
- `docs/02-ASSESSMENT.md` — verified defects and risks, ordered by severity
- `docs/03-REENGINEERING-PLAN.md` — agreed target architecture and phased plan
- `docs/04-WORK-INVENTORY.md` — every screen, controller and process, grouped into 16 modules; the pick-list for one-at-a-time work
- `docs/05-MODULE-ARCHITECTURE.md` — historical baseline of the old navigation. **Superseded 2026-08-16**: the live structure is the `NavigationNodes` registry seeded from `HRMS_Web/App_Data/navigation-seed.json`; read `AI-FILE-OBSERVATIONS.md` §12, not this file, for the current tree
- `docs/06-ENTERPRISE-READINESS.md` — the enterprise-level verdict: scorecard of every dimension today vs after the plan, judgement on the locked bets, and the five process risks (git, single machine, gate erosion, schema drift, unrotated credentials) that can stop the rebuild when no code defect can
- `docs/AI-FILE-OBSERVATIONS.md` — analysis of `AI Files/AI file.xlsx`: the restructuring requirements, which locked decisions they overturn, the gaps needing your answer, the hidden-form register, and the staged plan the current work follows
- `docs/modules/` — per-item deep-dive specs, one file per form or module, written before that item is rebuilt
- `docs/brand/` — PMS product mark source (`pms-mark-source.svg`) and its generator (`pms-mark-generator.py`, needs `pip install shapely`); the outputs live in `HRMS_Web/wwwroot/img/brand/`
- `docs/WORK-LOG.md` — session-by-session record of everything added, changed, discovered and decided, with reversal instructions. **Append to it at the end of every working session.** Until git is installed it is the repository's only change history
- `docs/roadmap.html` — status page mirroring `PROJECT.md`; refresh it when the module track changes

Markdown is the working format. Do not add further HTML deliverables unless asked for by name.
Work proceeds one form, module or process at a time through the gate documented in `PROJECT.md`:
understand, document, assess feasibility, break into tasks, **stop for review**, then build.

Keep these current as the plan advances. `03-REENGINEERING-PLAN.md` records locked
decisions; changing one requires updating that file, not working around it.
