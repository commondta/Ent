# Work Log

Chronological record of sessions. Newest first.

---

## 2026-08-28 — Collapsed-nav fly-out opens instantly

- Reported: in Collapsed View, clicking a module opened its forms panel "very slowly".
- Measured (headless Edge, opacity sampled every 25 ms after the click): the click handler and style
  recalc are instant, but the panel's `lmFlyIn` entrance animation (`.16s` fade + slide) held it at
  opacity 0 for the whole 325 ms window. PMS shows its fly-out with no animation.
- `public/assets/css/lmis-theme.css`: `animation: lmFlyIn` removed from the open panel rule, the
  `@keyframes lmFlyIn` and its reduced-motion override deleted. After the change the panel is at
  opacity 1 on the first sample. `layouts/main.blade.php` links the stylesheet with `?v=20260828a`.
- Still late after that: the forms list itself. Sampling the inner `ul.parent` every 30 ms showed Bootstrap's
  Collapse running on it anyway (`.collapsing`, height 14 px, ~350 ms before `collapse show` at 246 px) although
  the fly-out handler stops propagation. `lmis-theme.js` §6(c) now also calls `stopImmediatePropagation()` and
  removes `data-bs-toggle` from the module link for the duration of the click; `lmis-theme.css` adds a safety net
  (`.lm-flyout-open … .parent.collapsing { height:auto; transition:none }`). Result: full list (246 px) on the
  first sample. Scripts/styles now `?v=20260828b`.
- Header lockup brought to the PMS proportions: mark 48 → 36 px, first line "Land Information" 15 px title case
  (was 12.5 px all caps), second line 7.6 px small caps, ~20 px from the corner (Phoenix brand margin dropped,
  bar padding 24 → 16 px).
- Payroll's new mark (people silhouette, served by the host at `/payroll/Content/brand/payroll-mark[-white].svg`)
  in `partials/app-switcher.blade.php` and the My Home Apps tile.

## 2026-08-23 (evening) — switcher label rename

- `partials/app-switcher.blade.php`: first item renamed **"Applications Library"** (was "Apps
  Library") to match the PMS launcher rewording (PMS Session 15: bar "Real Estate Management
  Solution / Applications Library", new copy, Recently-used pill, hoverable coming-soon tiles).
  Verified live on `/lims/home`. Reversal: rename the span back.

---

## 2026-08-23 — My Home landing page, Recent/Favourites, navigation parity with PMS

User brief: "create a home page for LIMS — as in PMS", "Recent and Favourites both in home and header",
"all the navigation bar/menu features in PMS shall be implemented in LIMS, including chevron icon
styles and designs".

- **My Home** (`GET /home`, name `home`, `App\Http\Controllers\HomeController`, `resources/views/home.blade.php`):
  the landing workspace with the same five cards as PMS's My Home — **Overview Analytics** (live document
  counts per module, permission-gated by the same `*_list` flags as the sidebar, each tile links to its
  form; ▲/▼ compares this month's new documents with last month's), **To-Dos** (Approvals waiting for me =
  the inbox predicate on `document_approvals`; Pending = my documents with `status = 1`; Active Users),
  **Apps** (the ERP applications from the `ErpSso` session, current = LIMS, PMS opens on the host; "All
  applications →" = `/Apps`), **Recent**, **Favourites**. `/` and `/dashboard` now land on `/home`
  (the `land_provider` route name is kept as an alias); `<meta name="lm-home-url">`, the brand link and
  the breadcrumb "Home" point at it; a **My Home** item heads the sidebar.
- **Recent & Favourites — one store, four surfaces** (`lmis-theme.js` §5, `window.limsRecentFav`,
  localStorage `lims_recent_forms` / `lims_fav_forms`): (1) every form page from the menu is recorded
  (URL, title, module; max 10); (2) two header quick buttons (clock / star, `#lmRecentBtn`, `#lmFavBtn`)
  open popovers with the lists and a star per row; (3) the page bar carries an **Add to favourites** star
  for the open form; (4) the My Home cards. Every surface re-renders on change.
- **Navigation parity with PMS** (`lmis-theme.css` §4 rewritten, `lmis-theme.js` §1b/§6): rows are one
  flex line [icon][text][chevron]; PMS's hand-drawn solid **chevron chips** (double on modules, single on
  sub-folders; 22px token, black chip + white glyph while open, glyph rotates 90°; Font Awesome's caret
  removed); depth indentation 20/36/52px; open folder = tinted row + 3px inset bar + 600 label; active
  form = tinted row + inset bar + 500 label (+ the short centred line); typography hierarchy
  600/500/400; hover = lift + ink, no weight change; **active form from the URL** (longest menu-path
  prefix) with its ancestors pre-expanded — works for routes `sidebar-active.js` never knew
  (approval_*); icons-only mode **automatically below 1200px** (the user's Collapsed View choice still
  wins above it); one fly-out cascade at a time in icons-only mode; header **⋮ More** below 768px
  folding Recent / Favourites / the four approval links so the bar stays one clean row.
- PMS side (same session): the PMS product mark became monochrome (black / gray, transparent) — LIMS's
  Apps card loads `…/img/brand/pms-icon-plain-white.svg` from the host for the PMS tile.
- **Review round (same day):** (1) **short view fly-outs as in PMS / NetSuite** — in icons-only mode
  clicking a module icon opens its list in a panel to the right (`.nav-item-wrapper.lm-flyout-open >
  .parent-wrapper.label-1`, 250px, black top edge, module title header, slide-in); one open at a
  time; outside click / Esc / the Collapsed View toggle close it; Phoenix's hover panels and its
  `pointer-events: none` lock on module icons are switched off; the bar's overflow is released so the
  panel can escape it. (2) **Search on phones**: below 768px a search icon (`#lmSearchBtn`) joins
  the header and drops the command pill under the bar (`body.lm-cmd-mobile-open`), focused, with
  results. (3) **Apps card**: the current application tile carries `is-current` (ink outline, inset
  bar, live dot, breathing ring; reduced-motion safe) — PMS's My Home got the same.
  **Fix (user review):** the fly-out now sits flush against the bar (left 100%, top 0, radius 0 8 8 8,
  no left border) so icons + list read as one bar, as PMS; and the short bar is `position: fixed`
  (Phoenix made it absolute in collapsed mode, so it scrolled with the page) — it stays still now.
  **Round 3:** (1) **application switcher = PMS's** (`partials/app-switcher.blade.php`, CSS §15):
  My Home · "Applications" · a 32px logo tile per row (PMS mark from the host, LIMS mark, HRMS
  glyph; current = black tile + check; inactive = "soon"); `ErpSso::applicationsFor` now returns
  inactive applications too (with `IsActive`) so they list as "soon", while LIMS access still
  requires an active LIMS row. (2) **No second login LIMS → PMS**: switcher rows open other
  applications through the host's `/Apps/Go?code=…` (PMS's `Login/Index` now signs the PMS session
  in from the central session). (3) Short bar: Phoenix's **hover** panel (module names beside the
  bar) outranked the disable rule — forced off with matching specificity; only the click fly-out
  remains, flush with the bar. **Round 4:** Overview Analytics shows only Land Acquisition (Land
  Providers, Land Owners, Land Offer Forms) and Legal Documents (Conveyance Deeds, Agreements,
  Affidavits, Undertakings, Indemnity Bonds) — `HomeController::ANALYTICS`; the short bar now marks
  the module holding the active form (and an open fly-out) with the selected tint + inset bar, and the
  active form inside the fly-out keeps its tint. **Round 5:** page-bar favourite is one real pill
  button (glyph + label share the hit area, whole pill lifts on hover, children `pointer-events:
  none`); DataTables footer row (info + Previous/pages/Next) separated from the records by a top
  rule + 14px; switcher's first item "Apps Library" (grid icon). **Round 6:** fly-out panel and its
  labels forced over Phoenix's collapsed-bar `:hover` rules (panel stayed solid, labels plain — no
  transparent panel / label bubbles on hover); switcher chevron is a 24px chip with closed / hover /
  pressed / open states (same as PMS).
- Verified: `/lims/` → 302 `/lims/home`; `/lims/home` 200 with all cards (admin: 22 KPI tiles); headless
  renders of `/home`, `/land_provider`, `/approval_tree` at 1400 / 1100 / 700px — chevrons, active
  trail, page-bar star, header popovers, ⋮ More, icons-only band; no script errors from the theme layer.
- Reversal: delete `HomeController.php`, `home.blade.php`; restore the three `routes/web.php` lines, the
  `main.blade.php` sidebar item / header buttons / ⋮ More / `route('home')` links, `lmis-theme.css` §4
  and the trailing Recent/Favourites + header-overflow blocks, `lmis-theme.js` §1b/§5/§6,
  `sidebar-active.js` home rule.

---

## 2026-08-22 (night) — ERP platform milestone 1: PMS + LIMS behind one login and one URL

Per `Downloads\Application-Based ERP Structure.txt`. Full design in [08-ERP-PLATFORM.md](08-ERP-PLATFORM.md).

- **Central DB `ERP_Platform`** created and seeded (`database/erp_platform.sql`): Users/Roles/UserRoles, Applications (PMS, LIMS, HRMS-inactive), Modules/Forms (LIMS nav), Actions, RoleApplication, RoleFormPermission, Sessions, AuditLogs. PMS `admin` = ERP_ADMIN; all LIMS users imported as LIMS_USER.
- **PMS (.NET)**: `ErpPlatformService`, SSO cookie + central session on login, `/Apps` selection page, header application switcher, `/lims` reverse proxy, sign-out revokes centrally. Build OK; runs on `http://localhost:5217`.
- **LIMS**: `ErpSso` middleware (cookie → central session → app access → local user/provisioning → login), `erp` DB connection, `config/erp.php`, login redirects to the PMS login, logout hands off to PMS sign-out, `APP_URL=http://localhost:5217/lims` + forced root URL, app switcher in the brand, breadcrumb Home → `/Apps`.
- Verified end to end with curl (login → /Apps → /lims pages 200 with prefixed links → cross-app sign-out). **Heads-up:** LIMS is now meant to be opened as `http://localhost:5217/lims/…`; opening `127.0.0.1:8000` directly still works but links point at the proxy.
- **1b (user review: "selecting Land opened a login page")** — LIMS `/` rendered its own login even when SSO had signed the user in; now `/` → dashboard (signed in) or the ERP login, and LIMS has no login page at all. The single login now accepts **every** solution's credentials (PMS account → central HMAC credential → LIMS-native account verified over `POST /erp/verify` with a shared secret, then stored centrally); `/Apps` pre-authenticates LIMS via `GET /lims/erp/touch`; users with one application go straight into it; `/Apps` authorises on the central cookie so LIMS-only users can use it. Verified with the LIMS administrator account through the PMS login. See 08 §5b.
- Not yet: central per-form/action authorisation (phase 2), nav from the central registry, My Home cards in PMS, HRMS, production proxy/HTTPS.

---
## 2026-08-22 (late, round 4) — Module names, form relocation, product name and a redrawn vector logo

| # | Change | Detail |
|---|---|---|
| 1 | Module names (all navbar variants in `layouts/main.blade.php`) | Purchasing of Land → **Land Acquisition**; Registry Documents → **Legal Documents**; Exemption Documents → **Exemption**; Customization → **Administration** (Intimation Documents title-cased). Permission classes/ids unchanged. |
| 2 | Form relocation | The Exemption module's only live item, **Exemption Inventory Approval**, now sits in Land Acquisition as the second-last form (before Possession Certificate); the now-empty Exemption module wrapper is wrapped in a Blade comment (its other items were already commented out) — remove the comment to restore. `sidebar-active.js` gained the missing `exemption_inventory` mapping (parent = Land Acquisition). |
| 3 | Product name | `APP_NAME="Land Information Management System"` — used for the page title, brand alt text and the footer; the header wordmark reads "Land Information / Management System" (no abbreviation). Note: changing the app name rotates Laravel's session cookie name, so everyone is signed out once. |
| 4 | Logo | **Final identity supplied by the user as an SVG** (rounded box, land, tower crane, crawler crane — a mask over a filled rect): `public/assets/img/lmis-logo.svg` (white mark → top bar, login card), `lmis-logo-dark.svg` (same mask, `#111111` fill → light surfaces; rendered to `lmis-logo.png` and `icons/logo.png` for print letterheads), `lmis-icon.svg` (**black background + white mark, for favicons**; rendered to `lmis-icon.png` and `favicons/*.png` 16/32/150/180, plus an SVG favicon link). Earlier raster extractions were deleted. The N-Stack mark remains only in the footer credit. |

Verified: authenticated fetch shows the new menu order and labels; headless render of the list page with Land Acquisition open; title and footer carry the full name.

**Live review pass (Chrome, same day):** list/form/edit/approval/users pages in light and dark. Adjustments from it: the theme-toggle's Bootstrap tooltip (mis-positioned at the top-left in dark mode) removed from all navbar variants; approval-tab counters (`.count-indicator`, were red floating dots) are now ink pills at the tab corner and the header-pill rules are scoped to the top bar; the generic attachment placeholder (`assets/file.png`, shown at 240px) is a 56px bordered thumbnail; broken upload thumbnails (missing files) are hidden by `lmis-theme.js` instead of showing the browser's broken-image glyph; a little more air between the glowing mark and the wordmark.

---
## 2026-08-22 (late, round 3) — De-branding: no former-client or former-vendor names anywhere

**Instruction:** no "DHA", "DHA Bahawalpur", "DHAB", "Defence Housing (Authority)", "HCC" / "Hussain Chaudhry … Consulting" (any spelling) anywhere in the solution.
**Result:** zero occurrences in code, views, config, assets, docs and file names (verified by a case-insensitive sweep excluding `vendor/`, `node_modules/`, `.git/`). Only the legacy SQL dumps still carry the strings **inside data rows** (see "Left as data" below).

| # | Where | What changed |
|---|---|---|
| 1 | `config/app.php`, `.env`, `.env.example` | `APP_NAME=LMIS`; new neutral organisation tokens `ORG_NAME` ("The Society"), `ORG_SHORT` ("the Society"), `ORG_LABEL` ("Society"), `ORG_LEGAL` ("the Society, having its principal office at its Head Office") — printable legal templates and labels read these, so a future client name is a `.env` change |
| 2 | 81 Blade files (126 replacements) | page `<title>` and brand text → app name; footer `LMIS · …` → `LMIS`; legal print templates (affidavit, undertaking, agreement, possession, purchase layouts) now use `config('app.org_*')` instead of the former authority's name/addresses; "Is land provider not …?" notes; "… Rate/Amount" labels → `ORG_LABEL` Rate/Amount |
| 3 | `purchase_of_lands` table | columns renamed **`dhab_rate` → `society_rate`, `dhab_amount` → `society_amount`** on the live DB (sp_rename via a one-off Laravel migration that was run by path and then removed — the migrations table is back to 57 rows; the legacy dumps were updated to the new names so a fresh import needs no migration); controller `Purchs_L`, the three purchase-of-land views and their JS (`calculateSocietyAmount`) follow |
| 4 | `Conveyance_d.php` | comment + warning message use `config('app.org_label')` |
| 5 | Brand assets | header brand = N-Stack mark + "LMIS / Land Management" wordmark; login card logo = N-Stack mark (`auth-card.blade.php`); `public/assets/img/icons/logo.png` **overwritten** with the N-Stack mark (print letterheads that reference it now show that mark); favicons/apple-touch/tile → N-Stack mark png; `manifest.json` name "LMIS" |
| 6 | Files | `database/test_…_land_management (1).sql` → `legacy_land_management_mysql.sql`, `…_mssql.sql` → `legacy_land_management_mssql.sql` (database name inside → `legacy_land_management`); `Documents/… changes1.docx` → `Documents/Client changes1.docx` |
| 7 | Docs / README / CSS comments | every mention reworded ("the client organisation", "brand logo", "third-party credit"); `docs/02`, `docs/06` no longer quote the duplicated login e-mail that contained the name |

**Left as data (not changed):** 9 rows in each legacy dump and the live `land_providers`/address data contain the former authority's name as *record values* (addresses such as "… Authority Lahore … Chapter"), and the `Documents/*.docx` files' contents. Say the word to delete the legacy dumps from the repo or to edit those data values. Geographic defaults ("Bahawalpur" as tehsil/district) were kept — they are place names, not branding.

Also this round (from review): button states reworked (rest `#111` → hover `#2B2B2B`, pressed `#000` + inset, keyboard-only focus ring, no glow; secondary with `#D4D4D4` border and pressed tint; danger tints on hover, fills only when pressed); long menu names show their full name on hover (native tooltip, set only when the text is clipped).

Verified: `view:cache` compiles; authenticated 200 on purchase-of-land list/create/show/edit, agreement/create, conveyance/create, land_provider; live Chrome check of header brand, labels ("Society Rate/Amount"), menu tooltip; headless render of `/login`.

---
## 2026-08-22 (late, round 2) — Alignment & polish from the live review

User review in Chrome flagged: button misalignments in forms, module chevrons (wanted right-side), the open-module black chip, the search pill flipping white on focus, icon/title alignment in form headers, the stock-photo user avatar, and the "Add New Approval Setup" label.

| # | Finding | Fix |
|---|---|---|
| 1 | Every label sat ~16px right of its input — Phoenix gives `.form-label` `padding-left: 1rem` | `padding-left: 0` in the theme layer (all forms at once) |
| 2 | Submit buttons hugged the card edge outside the padded area; inner section headers (`h5` float-left + `.btn` float-right) collapsed under flex | `lmis-theme.js` moves each form's single submit (+ sibling cancel/back) into a right-aligned `.lm-form-actions` bar at the end of the form (still inside `<form>`; validation/onsubmit untouched; forms with several submits or modal/table submits are left alone); section headers are flex with the title left and buttons pushed right; header buttons never wrap or keep inline fixed widths |
| 3 | Chevron chip (left, black when open) | plain thin chevron on the **right** of the module row, rotates down when open; no chip |
| 4 | Search pill turned white on focus, kbd changed colour | pill stays charcoal — only the border brightens; icon/kbd constant; 34px, max 460px |
| 5 | Form-header icon chip vs title | header is a 56px flex row, chip and title vertically centred, title 15px/600 |
| 6 | Stock photo avatar (`team/…/57.webp`, 16 places across the navbar variants) | plain outline **user glyph** (feather `user`, no circle — user asked for hollow head/body shapes, not initials, not in a circle), white in the header like the other icons, 40px ink in the profile dropdown; the template's "Privacy policy • Terms • Cookies" lines removed from all 7 dropdown variants |
| 6b | Search input turned white while typing (global `input:focus` rule won over the pill) | `.lm-cmd-input` excluded from the focus rule; pill/input stay charcoal while active |
| 7 | "Add New Approval Setup" | "New Approval Setup" (`approvals/setup/show`, `inbox`) — other lists still say "Add New Record" |

Verified live: Land Offer Form, Land Provider, Land Owner forms (top/bottom), list pages, Approval Setup, search results, dropdown.

---
## 2026-08-22 (late) — Monochrome theme, rebrand and UI features (login page & nav items excluded)

**Goal:** bring LMIS onto the same black & white ERPNext-inspired theme as PMS (brief in `Pictures\Theme`, PMS as it stands today), fix the unprofessional form colour combinations, keep the light/dark contrast toggle, add the professional touches (vectors, breadcrumb, command search), rebrand the footer.
**Result:** every authenticated screen renders through the new layer; 48 GET routes + 7 deep pages verified 200 while logged in; all Blade views compile. Full design notes in [07-UI-THEME.md](07-UI-THEME.md).

| # | Change | Where |
|---|---|---|
| 1 | New theme layer: `--lm-*` tokens (light + dark), Phoenix variables re-tokened to neutral grays, Inter self-hosted | `public/assets/css/lmis-theme.css`, `inter.css`, `fonts/inter/` |
| 2 | Top bar charcoal with white outline icons (PMS decision), brand logo on a white tile, white count pills, command-search pill; light/dark toggle kept | `layouts/main.blade.php`, theme CSS |
| 3 | Sidebar white/ink, chevron chips instead of carets, selected = gray + black inset bar, white fly-outs | theme CSS (menu items untouched) |
| 4 | Breadcrumb + black icon chip in page titles, Ctrl+K / `/` command search over all forms, required-field asterisks, table empty-state vector | `public/assets/js/lmis-theme.js` |
| 5 | Forms/tables/tabs/badges/cards/alerts/modals/DataTables/Select2 restyled; yellow `#FFD966` headers and blue multi-select in the layout → tokens | layout inline styles + theme CSS |
| 6 | 782 off-palette literals (`#ffd966`, `#06a3d7`, `#80bdff`, `#ed2000`, black/#333/#999 borders, blue focus rings …) → `var(--lm-*)` in 78 views | `resources/views/pages/**` |
| 7 | Footer rebrand in 71 views: `LMIS | © year | Powered by N-Stack` with the N-Stack mark | views, `public/assets/img/n-stack-logo.png` |
| 8 | `custom-premium.css`: sidebar/top-bar pink-blue rules removed, logo glow scoped to the login page (login page itself unchanged) | `public/assets/css/custom-premium.css` |

Verified visually with headless-Edge renders of the Land Provider list (light and dark), and the Purchase of Land form — breadcrumb, title chip, charcoal bar, white sidebar, black buttons, asterisks and the N-Stack footer all present; the Chrome extension was unreachable this session. Not done / left: login page (excluded), navigation item structure (excluded), `layouts/app|guest` (unused Breeze shells). Follow-up fixes from the renders: breadcrumb now waits for the jQuery ready chain (sidebar-active.js marks the active item asynchronously), labels forced to title case, 43 remaining 2–4px black/#333 borders in 17 views → tokens, DataTables length select chevron fixed in dark mode. **Live Chrome pass afterwards** (extension reconnected): list, form, approvals (tabs + empty state), registry list, command-search results, row-action menu, dark toggle — three more fixes from it: footer was hidden behind the sidebar on add/edit views (their footer sits under `<main>`, now offset by the sidebar width), search icon vanished on focus (feather replaces the span with the svg — selector widened), long form names made the sidebar scroll horizontally (ellipsis now).

**To reverse:** remove the two `<link>`s and the `<script>` for `lmis-theme.*` from `layouts/main.blade.php`; `git checkout` the views listed by `git status`. All other changes are presentation-only.

---
## 2026-08-22 (night) — Git history replaced with a single fresh root commit

**Goal:** no trace of the former code-hosting service or the previous team anywhere in the repository, including history.
**Result:** one commit, authored by the current owner, containing the scrubbed tree; all old objects purged.

- Old history: 32 commits, all by the previous team (one committed via the hosting service with a `noreply` address); every old tree still carried host references in 31–44 files, so rewriting a single message would not have been enough.
- Action: orphan branch → `git add -A` → root commit `LMIS — offline baseline (self-contained assets, no external services)` → old `master` deleted, new branch renamed `master` → reflogs expired, `.git/logs` / `ORIG_HEAD` removed → `git gc --prune=now`.
- `.gitignore` additions: `/.vs`, `~$*`, `~WRL*.tmp`, `*.bak` (DB backup stays out of the repo).
- Verification: `git log --all` shows 1 commit; `git fsck --unreachable` → 0; no reachable blob and no `.git` text file contains the host name; working tree clean.
- A compressed copy of the old `.git` was left in the session scratchpad (outside the project) as a rollback safety net; delete it once satisfied.

---
## 2026-08-22 (evening) — Re-pointed the app at SQL Server 2022 (`MSSQLSERVER01`)

**Goal:** SQL Server 2019 (default instance, port 1433) was uninstalled; connect the app to the new instance using `Land Management Solution.bak`.
**Result:** app queries `[Land Management]` on `DESKTOP-NRLL5A6\MSSQLSERVER01`; login page and auth POST work.

### Findings

- New instance: `MSSQLSERVER01`, SQL Server 16.0.1000.6 (2022 Enterprise Evaluation), data dir `D:\Programs\SQL Server 2022\MSSQL16.MSSQLSERVER01\MSSQL\DATA\`.
- The `.bak` (taken 2026-08-15 22:23 from the old 2019 instance, DB `legacy_land_management`) had **already been restored** on the new instance as database **`Land Management`** (created 2026-08-15 23:40; its files are `legacy_land_management.mdf/.ldf`). A second restore under the old name failed on exactly that file conflict, so the existing DB is the restored copy. Verified: 44 tables, 57 migrations, 12 users, 8 purchase_of_lands.
- Instance settings: **TCP/IP disabled**, Named Pipes disabled, Shared Memory enabled; **Windows-authentication only** (`IsIntegratedSecurityOnly = 1`). Hence no `lmis_app` SQL login, no port.
- PHP side: `pdo_sqlsrv` 5.13.2, ODBC Driver 17 + 18 present.

### Changes

| File | Change |
|---|---|
| `.env`, `.env.example` | `DB_HOST=lpc:localhost\MSSQLSERVER01` (shared-memory), `DB_PORT=` (blank), `DB_DATABASE="Land Management"`, `DB_USERNAME=` / `DB_PASSWORD=` blank → Windows auth as the process user, `DB_TRUST_SERVER_CERTIFICATE=true` |
| `Land Management Solution.bak` (file ACL) | granted read to `NT Service\MSSQL$MSSQLSERVER01` so the engine can read it (needed for `RESTORE HEADERONLY`) |

### Verification

- `php artisan migrate:status` lists the 57 ran migrations.
- `DB::selectOne("select DB_NAME()")` → `Land Management` on `DESKTOP-NRLL5A6\MSSQLSERVER01`; `users` count 12.
- `GET /` → 200; `POST /login` with a wrong password → 302 back to `/login`, no exception in `storage/logs/laravel.log`.

### If the app is later served by IIS / another account

Windows auth means the web process identity needs a login on the instance (`CREATE LOGIN [IIS APPPOOL\…] FROM WINDOWS; CREATE USER … ; ALTER ROLE db_owner ADD MEMBER …`). Alternatively switch the instance to mixed mode (registry `LoginMode=2` + service restart, needs an admin shell) and recreate `lmis_app`.

---
## 2026-08-22 (follow-up) — Footer links and all repository-host references removed

**Goal:** no hyperlinks to outside sites anywhere in the UI, and no references to the former code-hosting service anywhere in the solution.
**Result:** done for everything tracked in the repo and everything served from `public/`.

| # | Item | Action |
|---|---|---|
| 1 | "third-party credit" footer anchor (71 views) and "template vendor" credit (`pages/show`) | removed |
| 2 | `.gitattributes` export-ignore line, `.claude/settings.local.json` fetch permission | removed |
| 3 | `.git/FETCH_HEAD`, stale `fsmonitor-watchman.sample` hook | deleted |
| 4 | `README.md` (stock Laravel README with hosted badges/logo) | replaced with a short offline README |
| 5 | Saved third-party error pages (`public/assets/css/generic/59.html`, two `.html` uploads) | deleted / blanked (upload filenames kept so DB rows stay valid) |
| 6 | Library licence-header URLs in `public/vendors/*` and `public/assets/js/*` (bootstrap, echarts, feather, fontawesome, prism, rater-js, select2, simplebar, sortablejs, tile colour-filter) | URL text stripped from comments/strings; code untouched |
| 7 | Brand-icon entries for the former code-hosting service in feather / fontawesome / unicons, the matching `--phoenix-*` colour tokens and `.bg-*/.text-*/.border-*` rules in theme CSS (and the 8 `.txt` test uploads that are copies of it) | removed — nothing in `resources/` referenced them |
| 8 | `composer.lock` package metadata (`source`, `dist`, `homepage`, `support`, `funding` URLs — 696 strings) | stripped; lock is still valid JSON. Consequence: `composer install` can no longer download packages — keep the existing `vendor/` folder with the project. Original lock backed up outside the repo. |

### Not changed (out of scope or needs a decision)

- `vendor/` (ignored by git, third-party library source) still contains host URLs in package metadata and comments — these are inert strings; scrubbing them means editing thousands of library files.
- One historical commit message (`577280f`, a merge) names the old remote URL. Removing it requires rewriting history (every later commit ID changes) — not done without an explicit go-ahead.

---
## 2026-08-22 — Offline hardening: all external connections removed

**Goal:** the solution must make no outbound connection at runtime or from the repo (no CDNs, no Google Fonts, no SMTP, no remote repository).
**Result:** every runtime dependency is now served from `public/`; git has no remotes.

### What was cut

| # | External endpoint | Where it was used | Replacement |
|---|---|---|---|
| 1 | `code.jquery.com` (jQuery 3.6.0) | `layouts/main`, `land_form/add`, `land_form/edit`, `land_provider/print` | `public/vendors/jquery/jquery-3.6.0.min.js` |
| 2 | `cdn.datatables.net` (1.13.7 + responsive 2.5.0) | `layouts/main` | `public/vendors/datatables/*` |
| 3 | `cdn.jsdelivr.net` (Select2 4.1.0-rc.0) | `purchase_of_land/add`, `purchase_of_land/edit` | `public/vendors/select2/*` |
| 4 | `maxcdn.bootstrapcdn.com` (Bootstrap 4.1.1), `cdnjs.cloudflare.com` (jQuery 3.2.1) | `land_provider/print` | `public/vendors/bootstrap4/*`, local jQuery 3.6.0 |
| 5 | `fonts.googleapis.com` / `fonts.gstatic.com` (Nunito Sans, preconnects) | `layouts/main`, `layouts/app`, `layouts/guest`, `auth/login` | `public/assets/css/nunito-sans.css` + `public/assets/fonts/nunito-sans/*.woff2` (weights 300–900, latin + latin-ext) |
| 6 | `unicons.iconscout.com` (line icons CSS) | `layouts/main`, `auth/login` | `public/assets/css/line.css` (trimmed to woff2) + `public/assets/fonts/line/unicons-0..20.woff2` |
| 7 | `ajax.googleapis.com` WebFont loader, `michaeltruong.ca` images | `land_form/test`, `welcome` (unused views) | loader removed; backgrounds set to `none` |
| 8 | SMTP mailer (`MAIL_MAILER=smtp`, host `mailhog`) | `.env`, `.env.example` | `MAIL_MAILER=log` — mail is written to the log, never sent |
| 9 | Ignition / Flare "share" button | dev error page | `config/ignition.php` disables share button & runnable solutions |
| 10 | Remote repository `origin` | `.git/config` | removed, along with `branch.master` upstream tracking |

### Left in place (not connections the app opens)

- Footer credit hyperlinks to `a third-party site` (71 views) and `themewagon.com` (`pages/show`) — plain `<a href>` the user would have to click.
- Three saved HTML pages under `public/assets/uploads/` and `public/assets/css/generic/59.html` contained third-party status-page links; they are stored files, not loaded by any view (neutralised in the follow-up session below).
- `guzzlehttp/guzzle` stays in `composer.json` as a Laravel framework dependency; nothing in `app/` or `routes/` calls `Http::`/Guzzle.

### Verification

- `grep` sweep of `resources/`, `app/`, `routes/`, `config/`, `public/assets` for `src=`/`href=`/`url()`/`@import` pointing at `http(s)://` or `//` returns only the hyperlinks listed above.
- Vendored CSS (DataTables, Select2, Bootstrap 4) contains no `url()` to external hosts.
- `php artisan view:clear` + `config:clear` run; `route:list` boots cleanly.

---
## 2026-08-12 — Local bring-up and MySQL → SQL Server migration

**Goal:** run LMIS in a browser on this workstation.
**Result:** running and verified at `http://127.0.0.1:8000`.

### Sequence

| # | Action | Outcome |
|---|---|---|
| 1 | Inspected repo; found Laravel 8.83.27, `vendor/` present | — |
| 2 | `php -v` → 8.5.9 ZTS x64 | Framework/runtime mismatch noted |
| 3 | `php artisan --version` | Ran, but flooded stderr with deprecations |
| 4 | Checked port 3306 and services | **No MySQL/MariaDB installed** |
| 5 | `php --ini` | **No `php.ini` at all**; 30 core modules only |
| 6 | Listed `C:\PHP\ext` | All 36 DLLs present — only configuration missing |
| 7 | Found `database/legacy_land_management (1).sql` | phpMyAdmin dump, MariaDB 10.4, 44 tables / 345 rows |
| 8 | Wrote `C:\PHP\php.ini`, enabled 13 extensions | First attempt failed silently — CRLF broke `$` anchors |
| 9 | Rewrote with `\r?$` anchors, no BOM | All 13 extensions load |
| 10 | Asked which database engine to install | User: *"convert mysql to microsoft sql and go with it"* |
| 11 | Probed for SQL Server | **Already installed** — 2019 Standard, TCP 1433, Mixed Mode, ODBC 17/18 |
| 12 | Searched `app/` for raw SQL | **Zero matches** — pure Eloquent, so the port is code-free |
| 13 | Confirmed PHP 8.5 driver support | Drivers 5.13.0+ support PHP 8.5; 5.13.2 current |
| 14 | Downloaded driver package | Served as `.exe`, actually a ZIP (`50 4B 03 04`) — extracted instead |
| 15 | Installed `php_sqlsrv_85_ts_x64` + `php_pdo_sqlsrv_85_ts_x64` | `PDO::getAvailableDrivers()` gains `sqlsrv` |
| 16 | Created database and `lmis_app` login | Mixed Mode made a dedicated login possible |
| 17 | Analysed dump: types, reserved words, escapes, defaults | Clean data; 4 reserved-word column names |
| 18 | Wrote `database/mysql2mssql.php` | 44 tables, 345 rows, no unmapped types |
| 19 | Imported via PDO in GO batches | **84 batches, 0 failures** |
| 20 | Repointed `.env`; wired `trust_server_certificate` | `SUSER_NAME()` = `lmis_app`, 12 users |
| 21 | Started `artisan serve` | **HTTP 500, empty body** |
| 22 | Handled a request through the kernel directly | 200 / 20 KB — so the app was fine, the server was not |
| 23 | Found root `index.php` is router content; `server.php` absent | Document root is the *project root*, not `public/` |
| 24 | Added stock `server.php` | 500 fixed; **all assets now 404** |
| 25 | Read `ServeCommand` | `chdir(public_path())` — hence `/public/...` cannot resolve |
| 26 | Rewrote `server.php` for both layouts + path blocklist | Assets 200; `/.env` and `/config/database.php` 404 |
| 27 | Verified all 28 assets referenced by `/login` | 28/28 → 200 |
| 28 | Smoke test: 48 authenticated GET routes | **48/48 pass** |
| 29 | Smoke test: 58 edit/show routes with real ids | 2 apparent failures |
| 30 | Investigated the 2 failures | Test's wrong table mapping, not an app bug |
| 31 | Corrected mapping, re-ran | **58/58 pass** |
| 32 | Confirmed cleanup | 12 users, 0 test residue |

### Follow-up in the same session

| # | Action | Outcome |
|---|---|---|
| 33 | Reset `admin@gmail.com` password on request | `Admin@12345`; verified by full HTTP login |
| 34 | Authored `docs/01`–`06` and this log | Applied solution-architect and project-planner skills |

### Decisions taken

Recorded as ADR-001 … ADR-007 in [05-DECISIONS.md](05-DECISIONS.md).

### Files changed

**Tracked, modified**
- `config/database.php` — one line: `trust_server_certificate`

**Tracked, new**
- `server.php`
- `database/mysql2mssql.php`
- `database/legacy_land_management_mssql.sql`
- `docs/01-SYSTEM-OVERVIEW.md`, `02-ASSESSMENT.md`, `03-MIGRATION-RECORD.md`,
  `04-MODULE-ARCHITECTURE.md`, `05-DECISIONS.md`, `06-WORK-INVENTORY.md`,
  `WORK-LOG.md`

**Untracked**
- `.env` — connection block repointed to `sqlsrv`

**Outside the repository**
- `C:\PHP\php.ini` (created)
- `C:\PHP\ext\php_sqlsrv_85_ts_x64.dll`, `php_pdo_sqlsrv_85_ts_x64.dll` (added)
- SQL Server: database `legacy_land_management`, login `lmis_app`

**Not modified:** any controller, model, view, or migration.

### Lessons worth keeping

1. **Check what is already installed before installing anything.** SQL Server
   2019 was already running; the initial plan to install a database server
   would have been redundant.
2. **Absence of raw SQL is what made this migration cheap.** The grep in step 12
   determined the entire risk profile — it should be the first check in any
   database port.
3. **An empty 500 means the failure is upstream of the framework.** Exercising
   the kernel directly (step 22) separated "app is broken" from "server is
   broken" in one move.
4. **CRLF silently defeats `$`-anchored regexes** when editing Windows config
   files from PowerShell.
5. **Verify the test before believing the failure.** Both smoke-test failures
   were defects in the test, not the application.

### Open items

Highest priority is **task 2.1** — the three MySQL-dialect migrations, which
leave `migrate:fresh` broken. Full list in
[06-WORK-INVENTORY.md](06-WORK-INVENTORY.md).
