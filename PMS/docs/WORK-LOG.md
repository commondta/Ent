# PMS — Work Log

Complete record of what was added, changed, discovered and decided, session by session.
Newest first.

`PROJECT.md` says where the work stands *now*. This file says how it got there, and is the place
to look when you need to know why a file changed or how to undo it.

**Git removed 2026-08-17** — the repository (briefly live on `Dhafeature/dev` with a GitHub
remote, 2026-08-16) was deleted at the user's request along with every GitHub trace. This file
is once again the repository's only change history.

---

## Session 15 — 2026-08-23 (evening)

**App Launcher copy & behaviour** (user-directed rewording plus two behaviours). `Views/Apps/Index.cshtml`:
bar title **"Real Estate Management Solution"** (was "ERP Workspace"), subtitle **"Applications
Library"** (was "Apps Library"; tab title follows: "Applications Library · Real Estate MIS"); h1
**"Choose the workspace you want to access."**; lead is the single line "You can switch
applications anytime from the workspace menu." (a two-line version with "One login. Multiple
applications." shipped first; the user trimmed it the same evening); tile action
**"Open Application"** (chevron kept).
**Recently used:** `AppsController` keeps an `erp_recent` cookie (codes most-recent-first, max 5,
90 days, HttpOnly, Lax, written by `Apps/Go` after the app is validated); `Apps/Index` passes it as
`ViewBag.Recent` and live tiles in the list carry a "Recently used" pill (ink text, white ground —
same pill geometry as "Coming soon"). **Coming-soon tiles are no longer dead UI:** rendered as a
focusable `<div class="tile is-off">` (not a link), `pointer-events:none` removed (opacity .65,
subtle border hover, no lift), with a charcoal tooltip on hover/focus — "Planned — {Name} arrives
in a later phase of the rollout." (today that is the HRMS tile). The tile body is one Razor
templated delegate (`tileBody`) shared by both shapes; the LIMS glyph src is root-absolute
(`/img/lims-logo-white.svg`) because `~/` is not resolved inside the delegate. The switcher's
first item was renamed to match in **both products**: `_ErpAppSwitcher.cshtml` (PMS) and
`resources/views/partials/app-switcher.blade.php` (LIMS) now say "Applications Library".
No schema, API or permission change. Verified: `dotnet build` 0 errors; run on `:5217` — curl
session shows the new bar/h1/lead/"Open Application", `Go?code=LIMS` sets `erp_recent=LIMS` and the
next `/Apps?stay=1` carries one "Recently used" pill, `/lims/home` renders "Applications Library"
live; headless Edge render of the saved page confirms the layout. Reversal: revert
`Views/Apps/Index.cshtml`, `Controllers/AppsController.cs` (the `RecentCookie` block and its two
call sites), the two switcher labels, and the AGENTS.md launcher bullets.

## Session 14 — 2026-08-23

**PMS product identity — logo, icon, favicon** (user supplied `gemini-svg.svg` and the cleaner
`logo.svg`, asking for it as the PMS identity with one correction: the buildings must not cross the
house roof at the bottom). Source kept as `docs/brand/pms-mark-source.svg`. The mark was rebuilt as
boolean geometry by `docs/brand/pms-mark-generator.py` (shapely → SVG paths), which fixes the
source: the roof chevron now runs to a baseline *below* every building (source roof ended at y=813
while the building corners reached y=817 and poked out beneath it), every tower/building/roof
junction has the same clearance (the left building touched the tower at x=277), all "light"
regions are real empty space so the mark works on any background, and a frameless heavier-gap
variant exists for small sizes. Outputs in `HRMS_Web/wwwroot/img/brand/`: `pms-logo.svg`
(framed, tight viewBox), `pms-logo-white.svg`, `pms-logo-cream.svg` (the 500-style artboard),
`pms-logo-current.svg` (currentColor), `pms-icon.svg` (rounded ink tile, white mark),
`pms-icon-plain.svg` / `-white.svg`, `favicon.svg`, `favicon-16/32/48/180/192/512.png` (rendered
at native size, transparent corners), `favicon.ico` (16/32/48/64). `wwwroot/favicon.ico` and
`favicon.png` replaced. Wired in: `_Layout`, `Login/Index`, `Login/Forget`, `Login/ChangePassword`,
`Apps/Index` — `<link rel="icon">` set (svg + 32/16 png + ico + 180 apple-touch; the four dead
`img/ico/apple-touch-icon-*-precomposed.png` links are gone), `_ErpAppSwitcher` header lockup icon
(25px mark before "Property / MANAGEMENT SYSTEM", white on the charcoal bar) and the PMS row in the
switcher menu, the PMS tile glyph on `/Apps`, the login card mark (36px, `.login-mark`), the
Forget/ChangePassword card logos (the unrelated `img/avatar/logo.png` monogram is no longer
referenced; file left in place). N-Stack remains the company mark (Powered by, `/Apps` bar). No
controller, API, schema or permission change. Verified: `dotnet build` 0 errors; app run on
`:5217` — login page carries the new links, every `img/brand/*` and `/favicon.ico` returns 200 with
the right content type, Forget page references the brand, headless render of the login page shows
the mark in the card. Reversal: delete `wwwroot/img/brand/`, restore the five views' icon links and
the `_ErpAppSwitcher` house path from git, `git checkout -- HRMS_Web/wwwroot/favicon.ico
HRMS_Web/wwwroot/favicon.png`.

**Same day — identity made monochrome** (user: "only black, white or gray shall prevail and
background must be transparent"): the generator now emits ink `#111111` + roof `#7E7C7D` on a
transparent ground; the cream artboard and the filled favicon tile are gone (`pms-logo-cream.svg`
deleted; `pms-icon.svg`/`favicon.svg` are the bare mark on a transparent square canvas; PNGs/ICO
re-rasterised with transparent corners). Header lockup stays white on charcoal; black chips (Apps
tile, switcher current row) keep the white mark; LIMS's My Home Apps card loads
`pms-icon-plain-white.svg` from this host. Rule recorded in `AGENTS.md` (PMS product identity).

**Same day — review round:** (1) browser tab title is now **Property Management System** everywhere
(`_Layout` `<title>` = optional `ViewData["Title"]` + product name; login / forgot / reset titles
renamed; "N-Stack" / "Urban Developers" titles gone); (2) **favicon on a black tile** — the only
identity asset with a ground, so it reads in light and dark tabs (`favicon.svg` + PNG/ICO
re-rasterised; `pms-icon.svg` stays bare); (3) login hero headline → **"Real Estate Management
Simplified. Operations Streamlined."**, the "version 1.0.1" footer removed; (4) **navigation
toggle moved** from the header hamburger to a LIMS-style **"Collapsed View" button pinned at the
bottom-left of the navigation panel** (`.nav-footer` inside `.vd_navbar-left`, same
`data-action="nav-left-medium"` / theme handler; the panel is now a flex column whose menu
scrolls; `#menu_m` hidden — shell rule amended in `AGENTS.md`); (5) **My Home Apps card**: the
current application tile carries `is-current` (ink outline, inset bar, live dot, breathing ring).
Build 0 errors; verified on `:5217` (titles, login copy, `nav-footer-btn` and `is-current` in the
rendered home; headless render full + collapsed modes). **Fix (user review):** the new
sidebar flex column gave the menu a private scroll region that clipped the short-bar fly-outs —
released (`overflow: visible`) on `.vd_navbar-left` and `.navbar-menu` while `nav-left-medium` /
`nav-left-small` is active, as customStyling.css already does for `.vd_navbar`; click fly-outs work again.

**Same day — round 3:** (1) **No second login when switching LIMS → PMS**: `Login.Index` now
signs the PMS session in from a live central session (`erp_sso` cookie → `ErpPlatformService.Validate`
→ `FindUserById` → `Users.PmsUserId` → the same session keys LoginToPortal sets, JWT included:
`SignInPmsFromCentral`) and continues to Home — or to `/Apps?stay=1` for a central user without a PMS
account; the login form renders only when no central session exists. `Apps/Go?code=PMS` with no PMS
session now goes through that path too. Verified with curl: same session `/` → Home; fresh jar with
the `erp_sso` cookie alone `/Login/Index` → 302 Home → 200. (2) **ERP solution mark** (user's
`svg (1).svg`: gray tower + blue roof + two houses) cleaned into `wwwroot/img/brand/erp/erp-mark.svg`
(tight viewBox) + `erp-favicon.svg` + PNG 16/32/48/180/192 + `.ico`; it is the login-card logo and
the favicon of the login page and the **App Launcher**, and the launcher bar's logo — the single
login and the launcher carry the solution identity, the PMS mark stays on PMS pages. **Follow-up:**
the launcher bar uses `erp-mark-mono.svg` (blue → black, gray, white) for contrast; the colour mark
remains the favicon by the user's choice. **Round 5:** launcher bar = `erp-mark-white.svg` (white /
light-gray, no tile) with the subtitle **"Apps Library"**; the LIMS tile shows the LIMS mark
(`img/lims-logo-white.svg`), not a line glyph; login card = `erp-mark-mono.svg` without the bordered
box; the switcher's first item is **"Apps Library"** (grid icon) instead of "My Home". **Round 6:**
switcher chevron (PMS and LIMS alike) is a 24px chip with closed / hover (lift) / pressed / open (white
chip, ink chevron rotated up) states; login + App Launcher favicon is a **simple home icon**
(`img/brand/erp/home-favicon.svg|png|ico`, black tile, white house) — the colour ERP favicon set stays
in the folder unused.

**Round 7 — two new top-level modules (user instruction 2026-08-23):** `Construction & Development`
(node 20, 10 descendants) and `Utilities Management` (node 24: Utility Billing · Meter Operations)
moved out of Estate Operations to the root of the `NavigationNodes` registry (`NodeType = Module`,
`ParentId = NULL`, depths recomputed from the parent chain, Estate Operations' group order closed
up); module order is now Estate Operations 1 · Construction & Development 2 · Utilities Management 3 ·
Financial Operations 4 · Business Analytics 5 · Administration 6. Script (idempotent):
`tools/local-run/promote-construction-utilities-modules.sql` (run against `PMS_Blank`); the seed
`App_Data/navigation-seed.json` carries the same structure for fresh installs. Both modules got
hand-drawn icons in `_NavigationMenu.cshtml` (`ModuleIcon`: tower crane over a building; meter
gauge with needle). Permissions untouched (the registry only moves nodes). Reversal: set the two
nodes back to `ParentId = @estate`, `NodeType = Group`, seq 3/4, re-run the depth CTE; revert the
seed and the two `ModuleIcon` cases.

**Round 8 — SSO cookie lifetime (user concern: reopening localhost landed straight in PMS):** the
`erp_sso` cookie had a 9-hour `Expires`, so it outlived the browser and the new SSO entry signed the
user in silently. It is now a **browser-session cookie** (no Expires) unless **Remember me** is
ticked on the login form (`LoginViewModel.RememberMe`, sent by the login script; `Login.SsoCookieOptions`)
— closing the browser ends the signed-in state; the central session row still expires server-side
after `Erp:SessionHours`. Verified: `Set-Cookie: erp_sso=…; path=/; samesite=lax; httponly` without
`expires` by default, with `expires=+9h` when RememberMe=true. Tab titles: login page **"Real Estate MIS"**, launcher "Apps Library · Real Estate MIS".

---

## Session 13 — 2026-08-22

**ERP platform milestone 1 — PMS is the ERP host** (brief: `Application-Based ERP Structure.txt`; design doc lives in the LIMS repo: `docs/08-ERP-PLATFORM.md`). New central DB `ERP_Platform` on MSSQLSERVER01. Changes in `HRMS_Web`: `Services/ErpPlatform/ErpPlatformService.cs` (central users/sessions/applications, ADO.NET); `Controllers/Login.cs` — after a successful password check a central session is opened and the `erp_sso` cookie set (HttpOnly, Lax, Path=/), apps cached in session; `SignOut` revokes it; `Controllers/AppsController.cs` + `Views/Apps/Index.cshtml` — `/Apps` application selection (tiles); `Views/Shared/_ErpAppSwitcher.cshtml` + `wwwroot/css|js/erp-platform.*` — the header brand is now the application switcher (My Home · ✓ PMS · LIMS · HRMS soon); `Extensions/LimsProxyMiddleware.cs` — `/lims/*` reverse-proxied to the Laravel LIMS (`Erp:LimsUpstream`); `Program.cs` DI + `UseLimsProxy`; `appsettings.json` `ConnectionStrings:ErpPlatform` + `Erp` section; login page redirects to `/Apps`. No PMS business logic, permission or schema change. Verified with curl: login → `/Apps` → `/lims/...` 200 → sign-out signs both apps out. **Same day, 1b:** the single login now also signs in non-PMS accounts — central HMAC credential in `ERP_Platform.Users` (PMS logins sync their hash/key there), then a LIMS-native check over `POST {LimsUpstream}/erp/verify` with `Erp:SharedSecret` (`Login.CentralLogin`); such users get the `erp_sso` cookie and `/Apps` but no PMS `Session["ID"]`. `AppsController` authorises on the central cookie, opens the only app directly, and `/Apps` pre-authenticates LIMS (`/lims/erp/touch`). `ErpPlatformService` gained `FindUser/VerifyHmac/StoreCredential/EnsureCentralUser/CreateSessionForUser/VerifyWithLims`. **Switcher polish (user review):** the legacy header CSS forced the menu rows to inline-block (wrapped names, floating icons) — `erp-platform.css` now carries `#erpAppSwitch`-scoped, `!important` layout rules: flex rows 40px, nowrap, 360px menu, "Applications" label, a 32px logo tile per application (PMS house mark, the LIMS mark from its SVG, HRMS people glyph; current app = black tile + check), hover/pressed/focus states, animated caret. My Home's **Apps** card now lists the central applications (Land opens via `/Apps/Go?code=LIMS`), falling back to `RealEstate:Apps` only when no central list exists. Reversal: remove the `Erp` section / connection string and `app.UseLimsProxy`, restore the brand anchor in `_Layout`, `url = "/Home/Index"` in the login script.

---
## Session 12 — 2026-08-21

**Repository-exposure audit** (user asked that nothing expose the solution to any private or public
repository). Verified: no git remote, no upstream branch, no `.github/`, no workflow files, no git
hooks, no `core.hooksPath`, no GitHub/remote settings in `.vs`, no `gh` CLI, no GitHub URL in local,
global or system git config (global has only `credential.helper=manager` and an Azure DevOps
`usehttppath` flag — generic Windows Git defaults, not a link to this repo). The only "github" text
in the tree is `.gitignore` template comments, vendor package metadata under `wwwroot/`, and the
history already recorded in these docs.

Added guards so a push cannot happen by accident:

1. `.git/hooks/pre-push` — exits 1 with a message on every push. Local file, not versioned.
2. `git config --local push.default nothing`.

Fixed `docs/roadmap.html` stat tile, which still read "Git + GitHub remote" — now "Local · Git, no
remote — never published". Strengthened the **Local-only git** preference in `AGENTS.md`.

**"github" string scrub**, same session, at the user's request. Deleted the three template-comment URLs from
`.gitignore`. In 63 vendor files under `HRMS_Web/wwwroot/` every GitHub URL/host (license comments,
source links, moment.js deprecation strings, the tmhOAuth user-agent, the jQuery-File-Upload demo
hostname) was rewritten to the neutral token `[upstream]`; the unused `github` icon rules were
removed from `font-awesome.min.css`, `font-awesome-ie7.min.css`, `font-entypo.css` and the two
glyph paths from `raphael/icons.js` (no view, script or stylesheet referenced them); the demo
variable `isOnGitHub` became `isOnDemoHost`; CKEditor `CHANGES.md` lost the word. Final sweep:
zero matches outside project docs. App verified on `localhost:5217` (root 200, touched CSS/JS 200).

Reversal: `rm .git/hooks/pre-push`, `git config --local --unset push.default`, revert the two files.

**Header shell brief** (`C:\Users\Adnan Ahmed\Pictures\2. Remove the Extra Box After Form.txt`,
2026-08-21) — task `#156`, UI layer only; no controller, API, schema or permission change.

1. **Empty box after the form description removed** from 11 views: the `container-fluid > card
   p-3 > menu` wrapper whose menu held nothing is gone from Approval/ApprovalSetup, Inbox, Index,
   Permission, ViewApproval, Commission/Index, DemandNote/DemandNoteForm, PurchaseRequest and
   Notification/FormAlerts; in DemandNote/DNCustodian and DNHOD only the empty menu div and its
   two `<br/>` went — their card holds the table. Cards whose menu carries a button stay.
2. **Notification badge**: `GetNotificationCount` is asynchronous, caps at `99+`, mirrors into
   the ⋮ menu (item count + dot on the button), refreshes every minute and on tab focus.
3. **Notification panel** (`#npOverlay`, `_Layout.cshtml`): the bell (`#hdrBellLink`) opens a
   panel over the current page — same `api/Notification/GetAll` the page calls (it marks the
   alerts viewed, so the badge is re-read after load); DataTables inside the panel supplies
   search, page size (5/10/25/50), page numbers and the info line; sticky column header, own
   scroll area; closes on ×, outside click, Escape or the bell again; "Open page" keeps
   `/Notification/Index` one click away; full width under the bar ≤600px. The anchor's href
   remains the no-script fallback.
4. **Responsive header**: ≥1200px unchanged (search capped so it never reaches the icon
   groups); <1200px the bar is one flex row [menu][brand][quick icons]…[search]…[bell][inbox]
   [⋮][user] — the theme's absolute geometry is overridden in the layout style block; ≤767px the
   user's name, the Recent/Favourites/Generate-alert icons and the inbox fold into a ⋮ More menu
   (`#hdrMoreBtn` / `#hdrMorePop`; items run the original handlers, Recent/Favourites popovers
   anchor to ⋮) and search becomes an icon that drops the same field below the bar; ≤480px the
   bell folds into ⋮ too (item carries the count). Menu button, brand and user never move.
   The inbox `<li>` id is now `top-menu-inbox` (was a duplicate `top-menu-3`).
5. **Coordinated sidebar**: below 1200px the navigation auto-enters icons-only
   (`nav-left-medium`); the theme's "tap the content to hide/show the bar" toggle at ≤975px is
   retired (`$('.vd_container').off('click')`); the hamburger still toggles icons-only ↔ full.
   Content offset beside the 60px bar pinned below 992px.

Verified: `dotnet build` 0 errors; the 11 views plus Home and Notification/Index return 200
with zero empty cards; alert flow over the API (SendAlert → count 1 → GetAll → 0); style and
script blocks brace-balanced. No browser run (no-extension rule) — the breakpoints 375–1920 are
covered by the CSS above and want an eyeball pass on your side.

Reversal: revert the `#156` commit — everything lives in `_Layout.cshtml` plus the 11 views.

**Login hero vector** (`#157`, 2026-08-21, your request with two reference images) — the
`crystal-brand` block (N-Stack logo chip + name) in the dark crystal panel of
`Views/Login/Index.cshtml` is replaced by an original inline SVG (`.crystal-hero`, viewBox
640×350): tower crane with lattice mast and swaying hook, seven glass towers with a window-grid
pattern and a few lit windows, three gabled homes (centre one in front, chimney on the slope)
on a ground sweep, a soft halo behind. Monochrome on purpose — white and greys against the
near-black panel, no colour — so it sits inside the theme. Motion is subtle (14s float, 9s hook
sway, glint pulse) and disabled under `prefers-reduced-motion`. The unused brand CSS was
removed; the left card still carries the N-Stack logo and "Powered by N-Stack". Verified:
build 0 errors, login page 200 with the SVG present, and a headless-Edge render of the page
checked visually (not the Chrome extension). Reversal: revert the `#157` commit.

Follow-up the same day (you had to scroll): the crystal panel now fits the screen height on any
device — `max-height: 100vh`, viewport-scaled padding, the hero capped at `clamp(120px, 30vh,
240px)` and left-aligned (`xMinYMid`), headline size scaled with the viewport, tighter rhythm
≤760px tall and the copy line dropped ≤640px tall. Hero width equals the text column. Checked
with headless renders at 1920×1080, 1536×864, 1366×768, 1280×720, 1024×768 and 1280×600 — no
scrollbar from the visual at any of them.

---


## Session 11 — 2026-08-20

**The monochrome theme shipped** (`#155`). The full black & white ERPNext-inspired redesign from
`C:\Users\Adnan Ahmed\Pictures\Theme` (two instruction files + two mockups), applied to the whole
application. UI layer only — no controller, route, permission, validation or markup-structure
change anywhere except the login page's presentation layer.

**The rule: white first, black for focus, gray for structure.** Tokens now live in a `:root`
block at the top of the new theme layer in `customStyling.css` (`--ink #111111`, `--border
#E5E5E5`, `--surface #F7F7F7`, `--hover-bg #F3F3F3`, `--selected-bg #F0F0F0`, …).

What changed, by file:

1. **`wwwroot/css/customStyling.css`** — every brown literal remapped (`#5C3713`→`#111111`,
   `#3E2609`/`#4f2e1c`→`#000000`, `#8B5A2B`→`#111111`, `#6B4419`→`#E5E5E5`, `#e8c9a0`→`#111111`,
   `#451d08`→`#F7F7F7`); dark→light inversions done by hand: table headers now `#F7F7F7` with
   ink text (`th.text-white` forced to ink), `.tp-panel-head` and `.panel-heading.vd_bg-grey`
   now light bars with black titles and a bottom border, cards now `1px #E5E5E5` border +
   8px radius + near-no shadow (were borderless 16px + heavy shadow), inputs `#E5E5E5` borders,
   6px radius, black focus ring; disabled = `#F5F5F5`/`#999999`. New appended
   **"N-Stack monochrome theme layer"**: tokens, Inter font stack, white sidebar rules,
   black nav icons, chevron chips (gray bordered token → black chip with white glyph when open),
   ERP tabs (active = black text + black bottom indicator), icons-only flyouts as white bordered
   panels, and the **active-item line animations** — sub-items grow a 34px line from the bottom
   center; main modules grow a full-width line under their name (`navLineGrow` /
   `navLineGrowFull`, active/selected only, reduced-motion guarded).
2. **`Views/Shared/_Layout.cshtml`** — header inverted to white-on-white: icons `#4D4D4D` with
   `#F3F3F3` hover, badge ring re-inked to white, brand SVG house + wordmark now `#111111`
   (subtitle `#4D4D4D`), search pill bordered `#E5E5E5` with black focus and the spec's
   "Search or type a command..." placeholder, chevron chips, nav-active/open states as
   `#F0F0F0` + black inset bar, Generate Alert button black, focus outlines black.
   Inter `<link>` added (falls back to local Open Sans offline).
3. **`wwwroot/css/theme.min.css`** — the hand-edited vendor theme: header
   `#242729`→`#FFFFFF` + `1px #E5E5E5` bottom border, shadow removed; `.vd_navbar`
   `#005A7F`→white/ink; `.vd_menu` text/separator/hover/active rules inverted to the light
   palette; child/sub-menu backgrounds transparent; **every remaining `#005A7F` Urban-blue
   literal → `#111111`** (the `.vd_green`/`.vd_bg-grey` lies now resolve to ink, not brown).
4. **`wwwroot/css/customStyle.css`** — `.dha_theme_btn`, `.vd_brown`, `.dha_table`, swal
   confirm, select2 highlight, `.sub-menu`, `.proprty_btn` and the legacy login gradients all
   monochrome.
5. **`bootstrap.min.css`, `bootstrap-old.min.css`, `theme.css`, `bootstrap-switch.css`,
   `Digital Persona` css, `less/element.less`** — same hex mapping (these carried hand-edited
   brand browns).
6. **17 view files** (Operations ×13, Sales ×2, GovtTaxes, Document) — per-view brown accents
   (tab highlights, divider pills) → ink.
7. **`Views/Login/Index.cshtml` — rebuilt presentation** per `Login Page Instructions.txt`:
   LEFT 45% white with a floating login card (logo, "Welcome back", staggered fade+rise
   entrance, white inputs with `#DCDCDC` borders and black focus, black "Log In" button with
   lock icon, Powered by N-Stack); RIGHT 55% **dark crystal** — `#0A0A0A` base, four CSS
   clip-path glass polygons + three light seams + a roaming glow on 12–26s cycles, N-Stack
   identity and three glass feature chips. Crystal hidden ≤900px. **All auth wiring untouched**
   (`LogIn()`, validation, toaster, eye toggle, remember-me, ids).
8. **Kept, deliberately**: My Home's colorful KPI palette (user instruction — the curated
   navy/teal/orange/violet set stays), all three Dashboard views' colors, red semantic badges,
   the amber favourite star, red required-asterisks/error states. Icon containers on form pages
   are black chips with white icons (`.fts-icon`, `.tp-head-icon`).

**Verified**: `dotnet build` 0 errors · app run on `:5219` — login 200 with card+crystal markup,
authenticated `/Home/Index` 200 with white sidebar/header markers and nav rendered (58 chevron
chips), `/Home/Block` 200 · zero brown-palette hexes left anywhere in `HRMS_Web`.

**To reverse**: every change is a stylesheet/markup-presentation edit in the files above —
`git revert` the session's commit, or `git checkout 5b3d28e -- HRMS_Web/wwwroot/css
HRMS_Web/Views/Shared/_Layout.cshtml HRMS_Web/Views/Login/Index.cshtml` plus the 17 listed views.

**Refinement round 5 (same day):** login crystal visuals brightened per user feedback (facet
fills ~2.5×, borders `rgba(255,255,255,.28)`, seam peaks `.55`, glow doubled) — no longer a
dull black; "Contact Us" → "Reach Us". Header: user name forced white (was gray), and the
**active/selected state of every header icon is now a white container with a black icon**
(hover keeps the translucent tint) — bell/inbox `.open`, Recent/Favourites/Alert `.open`, and
the mode toggles (hamburger while the short bar is on, view-mode buttons).

**Refinement round 4 (same day):** the randomly missing chevrons had one root cause — a legacy
hamburger handler in `_Layout.cshtml` (`$('.menu-text').toggle(); $('.menu-badge').toggle()`)
blind-flipped inline display on every chevron and label with each click, desyncing from the
actual nav mode. Removed; the mode CSS owns visibility now (full modes show every chevron,
icons-only hides top-level ones, fly-out panels keep theirs and their labels). Fly-out panels
also **scroll** when a forms list outgrows the screen (`max-height` + `overflow-y:auto`),
releasing the scrollbar only while a nested panel is open (`:not(:has(...))`) so the cascade
can still escape sideways.

**Refinement round 3 (same day):** the icons-only bar now works as **click-opened cascading
fly-outs** (NetSuite-style, per user suggestion). Root cause of "no items on click": the
sidebar's private scroll region (`overflow-y:auto; overflow-x:hidden`) clipped the panels —
released while `nav-left-medium`/`nav-left-small` is active. Child menus become right-opening
252px white panels (top border black), one per folder level; tree indentation and the
expanded-mode negative row margins are neutralized inside panels. Toggling the bar collapses
all open folders both ways (the active trail's server-rendered `display:block` otherwise hung
as stale panels), and opening one module's fly-out closes the others. Extension use ended at
the user's instruction — AGENTS.md no-extension rule restored; verification back to builds +
authenticated probes.

**Refinement round 2 (same day, live browser review — the user enabled the Claude Chrome
extension; AGENTS.md preference amended):**

1. **Header stays charcoal `#242729` by user decision** — the whitening is reverted in
   `theme.min.css`; icons, hamburger group, and the brand signature returned to **full white**
   (they had been re-inked dark for the white header and were invisible/dull on charcoal).
   Badge ring re-inked to `#242729`; header hovers are white tints again; focus outlines
   `#BBBBBB` (visible on both grounds).
2. **The "still brown" Submit/Add buttons were a stale-cache artifact** — the server served
   black CSS while browsers held the old copies. Durable fix: `asp-append-version="true"` on
   every local core stylesheet link in `_Layout.cshtml` and `Login/Index.cshtml`, so changed
   CSS always busts the cache. No more Ctrl+F5.
3. **Chevron-disappears-after-toggle bug fixed** — theme.js leaves an inline `display:none` on
   `.menu-badge` when the sidebar returns from icons-only mode; a
   `body:not(.nav-left-medium):not(.nav-left-small)` rule forces it back. Verified live.
4. Dashboard-tab domain icons → black chips (inline accent backgrounds removed).
5. Login copy: "Your Workspace Awaits!", label "User", crystal sub-line removed,
   "stakeholders" wording (paragraph + feature chip).

**Refinement round (same day, user review):** login heading → "Your Workspace Awaits!", email
label → "User" (ids/names untouched), crystal side drops the "Property Management System"
sub-line, "members" → "stakeholders" in the crystal text. Dashboard-tab domain icons →
black chips with white icons (inline accent backgrounds removed; the KPI tile colors stay).
Bootstrap's stock orange `.btn-warning` and light-blue `.btn-info` (e.g. "Property Detail" in
form heads) joined the dark family globally. **Hover contract fixed**: dark buttons now lighten
to grey `#3A3A3A` with a soft shadow on hover and press to `#000` (the earlier `#111`→`#000`
hover was imperceptible); secondary buttons wash `#F3F3F3` with a darker border; all buttons
transition. The relocated title-strip action buttons (`#ftsActions`) got one consistent size
and rhythm.

---

## Session 10 — 2026-08-17 (evening)

**GitHub disconnection**, at the user's request. The `.git` folder was already gone (removed by
the user); this pass stripped everything that still referenced GitHub:

1. Deleted `.github/` (contained only an empty `workflows/` folder — no CI ever existed).
2. Deleted `.mailmap` (git identity mapping).
3. Deleted 13 `*.sourcelink.json` build artifacts in `obj/` — they embedded
   `raw.githubusercontent.com/waseemsadiq9512/Urban/...` URLs.
4. Deleted every compiled `.pdb` in `bin/`/`obj/` that had that repo URL baked in via
   SourceLink; they regenerate clean on the next `dotnet build` (no `.git` → no SourceLink).
5. Deleted VS caches that had indexed those strings (`.vs/HRMS/v17/HierarchyCache.v1.txt`,
   `.vs/ProjectEvaluation/hrms.strings.v10.bin`, `.vs/slnx.sqlite`) — VS rebuilds them.

Kept: `.gitignore` and `.gitattributes` (local git config, no GitHub linkage, useful if git
returns) and the template-comment GitHub URLs inside `.gitignore`. Third-party NuGet DLLs
mention github.com in their own package metadata — vendor URLs, not a connection.

**Later the same evening — fresh local-only repository.** The user first asked for a repo on
the `adnan-janjua` GitHub account, then reversed: nothing goes online. Final state:

1. `git init -b main`; initial commit `6002fb6` — all 2,398 tracked files (bin/obj/.vs
   excluded by the standard VS `.gitignore`).
2. A remote (`https://github.com/adnan-janjua/PMS.git`) was briefly configured but **removed
   before any push** — nothing was ever transmitted. Verified: no repo exists at that URL,
   and no GitHub repo was ever created (the creation attempt was permission-blocked).
3. Standing rule recorded in `AGENTS.md`: **never add a remote, push, or publish this repo.**
4. Verified along the way: old repo `waseemsadiq9512/Urban` is not publicly visible; the
   account's three public repos are unrelated Lovable/TypeScript prototypes; the only GitHub
   credential on this machine is GitHub Desktop's entry for `adnan-janjua`.

---

## Session 9 — 2026-08-17

Login rebrand + navigation interaction pass, from your redesign brief (suggestions of another AI
tool, applied against the existing shell). Build verified: 0 errors.

1. **Login page brand** (`Views/Login/Index.cshtml`): the left-panel block now shows the
   **N-Stack Consulting** name and the supplied N logomark — copied from your Downloads to
   `wwwroot/img/n-stack-logo.png`; the `.login-logo` tile is solid white so the black-on-white
   mark reads as one clean tile. Favicon on the login page also points at the new mark.
   Support line reworded: "Need Support? Contact Us".
2. **Approval count badge** (`_Layout.cshtml` + `ApprovalsController`): the inbox header icon now
   carries the same red count chip the alerts bell already had. New endpoint
   `api/Approvals/GetPendingApprovalCount?userId=` — one query over `TestApproval`
   (`UserId + Is_Assigned + ApprovalStatus == "Pending"`), i.e. requests waiting on the user in
   their current stage. Alerts badge (`GetNotificationCount`) was already live and is unchanged.
3. **Custom chevron expand icons, hand-drawn** (`_NavigationMenu.cshtml`): bold SOLID chevron
   bands (filled paths, per the supplied reference image — not strokes, not a font glyph),
   `currentColor`, inside the rounded chip. **Main modules carry a double chevron
   (`ChevronDouble`), sub-folders a single one (`ChevronSingle`)** — your instruction during live
   review. Points **right** when collapsed, rotates **90° to point down** when open (the brief
   said 180°, but 180° from right points left — 90° gives the downward-open state the brief
   describes), 220ms ease-in-out, subtle hover opacity/scale on the chip, accent fill while open.
   Only the icon animates; text never moves.
4. **Navigation grid** (`_Layout.cshtml` styles): every row is one flex line
   `[icon][text][chevron]` — `menu-text` flexes with ellipsis. Verified live in Chrome, which
   surfaced three buried conflicts, all fixed:
   - `theme.min.css` (hand-tweaked, diverges from theme.css) carries
     `.menu-badge { position:relative; top:-23px }` — a lift hack for the old float layout that
     shoved the chip out of its row; the chip is now `position:absolute; right:14px; top:50%`
     inside its (position:relative) anchor, immune to it.
   - the theme pads each nested `.child-menu` 16px but pulls rows out −20px, so rows grew 4px
     wider per level and chevrons drifted; padding and margins are pinned to ±16px at every
     depth, so **every chevron sits at the identical right offset at any level** (measured
     239.6px at all three depths).
   - expandable rows (`a.nav-link` / `a.nav-group`) reserve 46px right padding so long names
     ellipsize instead of running under the chip. Medium (icons-only) mode keeps block layout.
5. **Typography hierarchy**: module 600 · expanded folder 600 · sub-folder 500 · form 400. Hover
   emphasis is **color only** (text brightens to white, 180ms) so text never shifts width.
6. **One active form at a time** (fixes your stuck-highlight bug — it had TWO sources): the old
   `active1` click handler in `_Layout` (added the class to any clicked anchor, never cleaned it
   off child items) is replaced, and the sidebar block in `custom/custom.js` (sessionStorage
   path replay + URL-matched `li.active` marking, which the theme paints dark and which
   replayed the last-clicked folder even on other pages) is retired — server-side rendering is
   now the single source of truth. The server marks the current form from the URL (`nav-active`
   in `_NavigationMenu.cshtml`, matched on `NavigationNode.Route`), and a small click handler
   moves the highlight instantly before navigation. Folders never take the active state; hover
   is temporary, active is persistent (stronger tint + warm left bar).
7. **Parents of the active form stay expanded**: the active form's ancestor chain renders
   pre-expanded server-side (`open nav-trail-trigger` on triggers, `nav-trail` + `display:block`
   on their child menus). `js/theme.js` (three global-collapse spots in the click-trigger and
   outside-click handlers) now excludes `.nav-trail` / `.nav-trail-trigger`, so clicking in the
   page body or opening another module no longer collapses the trail. Clicking the trail folder
   itself still collapses it manually — the pin only blocks *automatic* collapse.
8. **Expand / full-page controls restored** (`_FormTitleStrip.cshtml`): the legacy per-form
   `vd_panel-menu` buttons (hide navigation bar · hide top menu · full page) are back as three
   28px icon buttons in the form title strip, right of the form actions. Same `data-action`s
   (`remove-navbar` / `remove-header` / `fullscreen`) — theme.js's existing handlers drive them;
   the active mode shows a pressed brown state. The strip lives in the content area, so the
   restore button stays reachable in every mode.

9. **Full brand purge — N-Stack only** (your instruction, second pass of the day): every
   occurrence of "DHA", "DHA Bahawalpur/BWL", "Defence/Defense Housing Authority",
   "HCC" and "Hussain Chaudhry/Chaudhary" replaced with **N-Stack** across 66 files by a
   scripted, ordered sweep (longest phrases first, case preserved, encodings kept) — letter and
   print templates in `Views/PartialPage/`, form views, `MemberProfileController`,
   `B_Utility/BLL/CommonBLL.cs`, custom CSS (`.dha-possession-page`→`.ns-possession-page`),
   `wwwroot/Sql/IntialQuery.js`, seed SQL, and the docs/trackers. The old authority logos
   (`img/d_logo.jpg`, `img/dha_bwp_logo.png`) are deleted; all 16 templates that embedded them
   now use `img/n-stack-logo.png`. Personal names containing "Hussain" (e.g. the finance
   officer, seed employee rows) are people, not marks — untouched. Left as-is: the git branch
   name `Dhafeature/dev` (a git ref; renaming it is a git operation for you to decide).
   Rule recorded in `AGENTS.md` → User Preferences. Verified: repo-wide grep finds zero
   remaining marks; build 0 errors; served login HTML clean (the one grep hit was the
   substring "dHa" inside JavaScript's `invalidHandler` — a false positive).
10. **Expand controls actually hide the navigation now**: the theme's `remove-navbar` /
   `fullscreen` body classes only ever hid the header — its CSS never had a rule hiding the
   left navbar (that lives under the separate `nav-left-hide` class). `_Layout` now mirrors the
   `nav-left-hide` mechanics for both modes (`margin-left:-260px` on `.vd_navbar-left`,
   container margins zeroed), so "hide navigation" and "full page" behave as labelled.
11. **Module renamed: "Property Business Operations" → "Estate Operations"** (D32) — the first
   real use of the D22 rename-follows-the-registry mechanism. DisplayName-only, three spots:
   the module node's `display` in `App_Data/navigation-seed.json` (its `path` and every child's
   `parentPath` linkage untouched, so `FormKey` stays stable), the live row in `PMS_Blank`
   (`UPDATE NavigationNodes SET DisplayName=N'Estate Operations' WHERE NodeType='Module' AND
   DisplayName=N'Property Business Operations'` — 1 row), and the `ModuleIcon` switch key in
   `_NavigationMenu.cshtml` (keys on DisplayName). Breadcrumbs and search inherit the name from
   the registry automatically. Verified by authenticated HTTP probe: the menu renders
   "Estate Operations", the old name is gone. **`PMS_Local` note**: its SQL instance (default
   `.`) was offline during the change — if that fallback DB is ever used again, run the same
   one-line UPDATE against it.

**Verified live** (Chrome on `http://localhost:5217`): login rebrand renders; menu rows are one
flex line with all chevrons on one vertical line at three depths; double/single chevrons rotate
down on expand; opening `/Home/PropertyProfile` pre-expands its three-folder trail and
highlights the form; clicking inside the form body no longer collapses the trail; the title
strip shows breadcrumb + expand controls + star.

**Reversal:** git holds the diff — files touched: `Views/Login/Index.cshtml`,
`Views/Shared/_NavigationMenu.cshtml`, `Views/Shared/_Layout.cshtml`,
`Views/Shared/_FormTitleStrip.cshtml`, `wwwroot/js/theme.js` (the `.not('.nav-trail')` guards),
`wwwroot/custom/custom.js` (sidebar replay block retired),
`Controllers/api/ApprovalsController.cs` (new endpoint at the top),
`wwwroot/img/n-stack-logo.png` (new file).

---

## Session 8 — 2026-08-16 (second)

Header polish in `HRMS_Web/Views/Shared/_Layout.cshtml`, all verified in the browser against
the running app on `https://localhost:7217`:

1. **Brand lockup centered on the bar** — it sat ~4.5px high. Two causes, two fixes: (a) the
   inline-level anchor left ~7px of baseline dead space under it inside `.logo`, so flex centered
   a 41px box instead of the 34px SVG — `.logo` in the header is now itself `display:flex;
   align-items:center`; (b) the artwork ink spanned y 1.9–29.6 inside the 34px viewBox — the SVG
   children are wrapped in `<g transform="translate(0,1.25)">`. SVG midline now exactly on the
   61px bar centerline (30.5px). The house was later enlarged to match the word block — scale
   1.375 → 1.675 (≈23px ink, cap of "Property" to base of "MANAGEMENT SYSTEM"), per your
   instruction.
2. **Search text centered in the pill** — a theme `input[type=text]` rule outranked `.gs-input`
   and re-added 6/7px vertical padding with a 23px line-height, floating the placeholder high.
   New `#gsWrap .gs-input` rule pins `padding-top/bottom: 0; line-height: 30px`.
3. **Profile: round avatar image replaced with a plain glyph** (your instruction this session) —
   the 36px round `avatar.jpg` is gone; an 18px white geometric person SVG (`.user-glyph` — a
   square head over a rectangular body, two lightly rounded `rx=1.5` rects, per your instruction) now sits inline
   before the session name. The override selectors mirror the theme's own
   (`.vd_mega-menu-wrapper .vd_mega-menu > .mega-ul > li.profile …`) so the later cascade
   position wins — a first attempt with shorter selectors lost on specificity and stacked the
   icon above the name.

4. **My Home — Overview Analytics segmented** (`HRMS_Web/Views/Home/Index.cshtml`): the flat
   9-tile KPI grid is now three labeled horizontal segments in the same card — Financial &
   Operations (Sales, Transfers, NDC — merged on your follow-up), Inventory Overview (Stock,
   Sold/Registered, Available), Members & Users (Members, Dealers, System Users) — side by side with vertical
   dividers, wrapping when the card runs out of width. Permission gates unchanged, computed once
   into `kpi*` booleans; tile ids unchanged so the KPI-fill script needs no edits. New CSS:
   `.mh-kpi-segments/.mh-kpi-seg/.mh-kpi-seg-title` beside the `.mh-kpi-row` rule.

**Reversal:** items 1–3 are confined to `_Layout.cshtml` — the inline `<style>` block
(brand/profile/search comments mark each rule) and the two markup spots (brand `<svg>` wrapper
group; profile `<a class="mega-link">` block). Git holds the exact diff.

---

## Session 7 — 2026-08-16

**Driver:** `AI Files/AI file.xlsx` updated by you — grown from 4 to 6 sheets. The new
**Instructions** sheet (§1–§32) is a final directive: every open question answered, decisions
final, stop documenting and start building, Home screen first. Full digest:
`docs/AI-FILE-OBSERVATIONS.md` §12.

**Net result:** Stage A is closed — all 10 open decisions answered from the workbook. Phase 1
(Home screen) has begun: the **My Home** SAP Fiori-style workspace tab is built, verified and
live as the default landing view. Git found installed and working — `#13` closed.

### 7.1 What the updated workbook decided

1. **Hidden forms** (HIdden Forms sheet): 10 restored with Module/Sub-Module placements — KYC,
   Deal Merger, Dealer Reservation, Booking Backlog, Map Design, Re-Design, Purchase Request,
   Demarcation Charges, Charges Group Form (**rename to "Form Wise Charges"**), Unit of Measure.
   "Remove all other hidden forms" — the rest (Registration NPD, Demarcation Charges I, Admin
   Dashboard, Operations/Propertybinding, Operations/TransferForm) leave the scope; they stay in
   code until the final review stage (Instructions §10 keeps near-duplicates present until then).
2. **Folders Structure sheet** — the definitive 4-level tree with sequence numbers. Administration
   has **5** sub-modules (mapping-sheet reading wins over Restructure §8); Implementation Center
   sits **under Administration**; "Setup - Sub Module" is now a structured **Setup** node with 8
   Level-3 children; the §10 BI tree stays out of scope (G-1, G-2, G-3, G-6 all closed).
3. **Recent / Favourites live in the navigation system only** (Instructions §18) — supersedes my
   earlier header-icon reading of G-8.
4. **Audit trail**: meaningful field-level change capture without inflating the database (§14) —
   the split strategy stands. Audit strip in italics at the bottom of every applicable form (§13).
5. **Block**: contextual scoping confirmed again (§15) — Block C may exist in Phase 5 and Phase 6.
6. **Development order fixed** (§30): Home screen → registry/architecture → navigation → restore
   forms → security → search → database configuration → refinement.
7. **Git installed** (§24) — verified: repo live on `Dhafeature/dev`, remote on GitHub.
8. **Working method changed** (§2, §31): implement, don't stop for clarification rounds; the
   decisions provided are final. The one-item-at-a-time review gate is retired for this
   restructuring programme.

### 7.2 Code changed — file by file

| File | Change | Reverse by |
|:--|:--|:--|
| `Controllers/api/WorkspaceController.cs` | **New.** `GET api/Workspace/GetMyHomeSummary?userId=` → unseen alerts, pending approvals (TestApproval `Pending`, not cancelled), active system users. `[Authorize]`, same pattern as NotificationController | Delete the file |
| `Views/Home/Index.cshtml` | **My Home tab added** as the default landing view (UI sheet mock): Fiori-style cards — Overview Analytics (KPI tiles mirrored from the Dashboard tab's fetch, permission-gated identically), To-Dos (alerts / approvals / active users), APPS tiles from configuration, Recent, Favourites. Dashboard and Map tabs untouched; toggle generalised to three views | Remove the `myhomeView` section, the `mh-*` styles, the My Home JS block, and the My Home tab button; restore `active` on the Dashboard button and remove `hide` from `dashboardView` |
| `Views/Shared/_Layout.cshtml` | Recent-forms tracker: a small self-contained script records each visited form page (URL + name derived from the URL) into `localStorage` `pms_recent_forms`, max 10, wrapped in try/catch so it can never break a page. The registry later replaces the derived names | Delete the marked script block |
| `appsettings.json` | New `RealEstate:Apps` section — the APPS tile list as configuration (PMS current; Land Management and HRMS disabled/coming soon). Delivers the config half of `#140` / D15 | Remove the section |

### 7.3 Verified

- `dotnet build` — 0 errors.
- Logged in as admin over HTTPS; `Home/Index` 200 with all five My Home cards present,
  Dashboard tab hidden by default, My Home tab active.
- `GET api/Workspace/GetMyHomeSummary` → code 100, `alerts=0, pendingApprovals=0, activeUsers=1`
  — correct against the blank database. Without a token → 401.
- Note for future probes: scripted calls must target `https://localhost:7217` directly — the
  HTTP→HTTPS redirect drops the `Authorization` header (browsers never hit this: the app runs
  same-origin on the HTTPS port after login).

### 7.4 Not done / next

- Idle-logout blur overlay (Instructions §21) — next Phase 1 item.
- Header strip per the UI mock (search, alerts, approval box) — with the shell work.
- Map tab investigation (PDF/vector/CAD plot maps, §4) — parked until after Phase 1 basics.
- `docs/05-MODULE-ARCHITECTURE.md` §5 rewrite to the Folders Structure tree — due with Stage B
  registry seeding, which now has its authoritative source.

### 7.5 Second parcel, same day — the registry and the new navigation

**Driver:** your note *"navigation menu is still same."* The registry (Instructions §5) and the
registry-driven menu were pulled forward and shipped.

**What exists now**

- **`NavigationNodes` table** — the form/navigation registry: 223 rows (5 modules, 51 groups,
  172 forms), each form carrying `DisplayName` (presentation), `PermissionKey` (stable
  authorization identity — never changes on rename), `FormKey`, `Route`, `SequenceNo`, `Depth`.
- **Seed** generated from your workbook: PMS-Modules mapping (162 forms incl. renames) +
  Folders Structure (hierarchy and sequence numbers) + HIdden Forms placements (10 restored
  forms) joined to routes from the legacy menu. Checked in as
  `HRMS_Web/App_Data/navigation-seed.json`.
- **`NavigationRegistrySeeder`** — at startup: creates the table if missing, seeds once if
  empty, and backfills `PermissionForms` + per-user grants for forms that never had a
  permission row (the 10 restored hidden forms). Idempotent; a failure leaves the app running.
- **`Views/Shared/_NavigationMenu.cshtml`** — renders the tree from the registry with the
  existing menu CSS; forms show only with `CanView` on their `PermissionKey`; groups show only
  with a visible descendant. **The 2,460-line hard-coded menu block in `_Layout.cshtml` is
  replaced by this partial** (layout went 2,938 → 423 lines; the old block is in git at
  `20364cf`).

**Permission parity, proven**

- 9 mapping-sheet names used a different string than the original `UserHavePermission()` check
  (e.g. `Assets/Media` → checked as `Banners`; `Drawing Scrutiny Charges` → `Demarcation
  Request Form`; `Dynamic Query` → `Dynamic Report Builder`; `W.Holding` → `WithHolding`…).
  All 9 were recovered from the pre-swap layout in git and the registry now carries the
  original keys; the 9 accidental duplicate permission rows were removed.
- Verified by query: **0** registry forms without a matching `PermissionForms` row; **0** form
  keys the admin lacks `CanView` on.
- 4 legacy label typos are preserved as permission keys on purpose (`Advance Applictaion On
  Plot`, `Meterial Testing Form`, `Role Permisison`, `User Permisison`) — displays are the
  corrected names; the keys must match the seeded permission rows until the key migration.

**Legacy menu defects closed by the swap:** `#130` (Transfer Set Receiving now routes to
`Operations/TransferSetReceiving`; the Drawing Scrutiny Charges label kept per the mapping),
`#131` (both dead Finger Uploader links gone), `#132` (fake Administration→Reports subtree and
empty groups gone — reports live under Business Analytics), 22 duplicate links collapsed, and
the 15 formerly hidden forms resolved per your verdicts (`#133`).

**Verified live:** build 0 errors · seeder log "223 nodes / 19 permission backfills" (then
corrected to 10) · authenticated Home page renders all 5 modules, 225 menu entries, renamed
displays (Property Generation, Revenue Group, Reports Builder…), restored forms (Know Your
Customer, Form Wise Charges) · five representative form links probed, all HTTP 200.

**Reverse by:** restoring `_Layout.cshtml` from `20364cf`, deleting `_NavigationMenu.cshtml`,
`NavigationRegistrySeeder.cs`, `NavigationNode.cs`, the `NavigationNodes` DbSet and the
`Program.cs` seeder call, then `DROP TABLE NavigationNodes` (and optionally the 10 backfilled
`PermissionForms` rows and their grants).

### 7.11 Eighth parcel — accessibility & interaction audit (web-interface-guidelines)

Ran the `web-design-guidelines` skill against the shell components (header, navigation, My
Home, form title strip) and fixed every finding in the code we own:

- Header icon triggers were `<span onclick>` — now real `<button type="button">` with
  `aria-label` + `aria-haspopup`, keyboard-operable, with a visible `:focus-visible` ring
  (warm accent) across all header/star/search controls.
- Bell & approval links: `aria-label`; decorative SVGs `aria-hidden` (menu icons and chevrons
  too).
- Search input: `role="searchbox"` + `aria-label`; results container `aria-live="polite"`.
- Generate Alert box: `role="dialog" aria-modal aria-labelledby`, Esc closes,
  `overscroll-behavior: contain`; toast is `role="status" aria-live="polite"`.
- Menu folders expose `aria-expanded`, kept in sync with the visible state (51 nodes).
- Home view tabs: `role="tab"` + `aria-selected` toggling.
- Form title strip renders the page `<h1>` (the legacy h1 is hidden with the banner, so the
  page kept its heading); breadcrumb is `<nav aria-label="Breadcrumb">`; long titles truncate.
- All favourite stars: `aria-label` naming the form + `aria-pressed` state.
- `prefers-reduced-motion` honoured across every shell transition; KPI/badge/count numbers use
  `tabular-nums`; badge hidden when empty.

Known-but-deferred (legacy theme, Phase 6 territory): `transition: all` in theme.min.css,
missing image dimensions, un-virtualised DataTables, per-form input labelling.

### 7.12 Ninth parcel — design refinement rounds (your redesign brief + live feedback)

Iterated live with you through the evening; commits `98301631` → `4bb22e53`:

- **Header redesign** (from `I want you to professionally redesi.txt`): standalone filled
  bell + matching approval icon (circles removed), crisp count badge hidden at zero, one flat
  hover treatment across all header icons, profile typography tightened.
- **Search**: white enterprise pill clearly reading "Search" when idle; the magnifier slides
  in on the left only while typing.
- **Navigation**: chevrons became rounded icon chips (accent-filled while open), indentation
  normalised (20px baseline + 16px/level), open folders tinted with accent bar.
- **Rebrand (req. 12)**: the only live N-Stack branding — the POWERED BY chevron logomark on the
  login screen — replaced with an N-STACK wordmark; consultancy icon removed. Non-branding
  hits (person names, the WASEEM-HCCLABS dev machine name in NewMemberUploader) left alone.
- **Header brand**: the N-Stack logo image replaced by a brand lockup, which after two order/color
  iterations landed as **one single SVG vector** (172×34): white house glyph left, "Property /
  MANAGEMENT SYSTEM" right — one graphic, immune to the theme's 120px `.logo a` constraint
  that had been wrapping the earlier span-based lockup ("shuffling").
- **Alignment**: whole header on the 61px centerline; the lifted-vector bug fixed by making
  every icon container a flex-centred box with block SVGs (glyphs were riding the text
  baseline). One regression (50%-transform centering threw the left icons off-screen) was
  fixed the same hour with a fixed offset.
- **Accessibility parcel** (§7.11) ran between these rounds via the web-design-guidelines
  audit.

**Bench for next session:** `#149` idle-logout blur overlay (Instructions §21) · `#151` map
investigation (§4) · favicon still the old d_logo (swap with the Stage E branding form) ·
then Stage C — registry-backed authorization management + Form Alias configuration form.

### 7.10 Seventh parcel — global search in the header

- **Global search box, top center of the header** (N-6 first delivery): debounced type-ahead
  against new `api/Workspace/GlobalSearch` — searches **forms** (registry DisplayName *and*
  old names, results show the full breadcrumb) plus **properties** (PropertyNo/RegNo),
  **members** (name/CNIC) and **dealers** (principal owner/estate/CNIC). Each record hit also
  names the form it opens in, resolved from the registry so renames follow ("in Reference No.
  Profile" / "Member Profiling" / "Dealer Profiling"). Enter opens the first hit; Esc closes;
  every group is independently guarded. Record deep-linking (open the form *on* that record)
  comes with the per-form search contract later.
- **Alert box: self-send allowed** — the sender now appears in the user list marked "(me)".
- **Short bar hardened** — icons-only rules now `!important` so no theme rule can re-show
  names in the collapsed bar.

Verified: 'block' → Block Definition with full path · 'revenue' → the 5 renamed revenue forms ·
record groups return their registry form names. Record queries return empty on the blank DB by
design.

### 7.9 Sixth parcel — Generate Alert, depth bug, lingering tint, float overlap

1. **Slash-name depth bug fixed** — the seeder computed Depth by counting '/' in the tree
   path, so forms with slashes in their names (Assets/Media, File Doc/Dup Request,
   Repurchase/Refund/Cancellation, Access Notification/Form Watcher) indented one or two
   levels too deep. Depth now derives from the parent chain — fixed in
   `NavigationRegistrySeeder` and corrected in the live table by iterative SQL.
2. **Lingering open-folder tint fixed** — the theme leaves `open` on anchors whose submenu
   actually closed; a sync pass after each menu click settles classes to what is visible.
3. **Generate Alert shipped (N-5)** — megaphone icon in the top-left header group opens a
   small box over the current screen (the open form stays): user picker, Critical /
   Non-Critical type, message, Send → toast "Alert is sent". New endpoints
   `api/Workspace/GetActiveUsers` and `api/Workspace/SendAlert` write a real `Notification`,
   so it lands in the receiver's bell. **The existing bell (received alerts) is untouched.**
   Proven end-to-end: send returned id 1 and the receiver's unseen count rose to 1.
4. **Floated add-buttons no longer overlapped** — many forms float an action link right and
   the following card overlapped it (Form Watcher's "Add User"). Global CSS keeps the float
   clickable (z-index) and clears the card below it.

### 7.8 Fifth parcel — one professional form header (from `Pre_Booking_Same_Design.xlsx`)

Your screenshot showed three stacked bars on every form: the new registry strip, the legacy
"Home / …" breadcrumb row (`.vd_head-section`), and the legacy title banner
(`.vd_title-section`). Fixed for **all** forms at once — the theme markup is uniform:

- **The strip is now THE form header.** When present it adds `fts-active` on `<body>`, which
  hides the two legacy bars; script then absorbs the legacy banner's icon (into a rounded
  chip) and its action buttons (e.g. *Property Detail*, *Show Approval History*) into the
  strip's actions area, ahead of the favourite star. Buttons are moved, not cloned — their
  event handlers survive.
- **Responsive**: the strip is relocated inside the form's own `.vd_content` wrapper, so it
  resizes and shifts with the navigation pane like the rest of the form, and shares the form's
  left/right edges (the indentation complaint).
- **Menu indentation normalised**: level 1 at the theme's 20px baseline, +16px per level.
- **Open folders are visibly distinct**: tinted background, warm accent bar, white bold label —
  plus the rotating chevron from §7.7.
- All changes are additive in `_Layout.cshtml` / `_FormTitleStrip.cshtml` — no per-form edits;
  a form without the standard theme markup simply keeps its own header.

Verified on `Sales/PreSaleApproval` (strip title "Property Reservation", hide/absorb/relocate
scripts all present) — build 0 errors.

### 7.7 Fourth parcel — UI polish batch + Records & File Management move

Your feedback, same day, all shipped:

1. **Short bar is icons-only** — the hamburger's medium mode now hides module names and badges
   (the theme only did this in its small mode); module anchors carry tooltips; child menus
   still fly out.
2. **Tree indentation** — every menu level steps in 14px (`Indent()` in `_NavigationMenu`),
   groups render semibold, so the hierarchy reads at a glance.
3. **Hover effects** — Home view tabs, all navigation items (modules + child links), My Home
   cards (lift + shadow) and KPI tiles, and form tabs (`.nav-tabs` both flavours).
4. **Form titles aligned with menu names** — new `_FormTitleStrip.cshtml` rendered above every
   form: registry breadcrumb + the registry `DisplayName` as the page title, and
   `document.title` set to match. Never breaks a page (guarded); absent on Home.
5. **Favourite marking mechanism** — the strip carries an Add-to-Favourite star for the open
   form (Restructure req. 4: present on forms, absent on Home). Shared
   `window.pmsFavToggle`/`pmsFavHas` API now drives the strip star, the header popover stars
   and the My Home cards; the recent-forms tracker records the registry display name.
6. **Expand chevrons** — `fa-angle-down` replaced with a chevron SVG that rotates 180° while
   its folder is open (keyed off the theme's `open` class on the trigger anchor).
7. **Records & File Management moved under Property Business Operations** as-is, second-last
   (before Biometric Management) — your instruction; **4 top-level modules** now. Applied to
   the live registry by SQL and to the seed generator + `navigation-seed.json` for fresh
   databases. Recorded as **D31**.

Verified: build 0 errors · 4 top-level modules render · R&FM group under PBO with its 2 forms ·
strip shows "Block Definition" with full breadcrumb on `/Home/Block` · chevron, short-bar,
indentation and hover CSS all live.

**Reverse by:** git-revert the commit; for the module move alone, re-run the two `UPDATE`s in
reverse (restore `NodeType='Module'`, `ParentId=NULL`, `Depth=0`, `SequenceNo=3`; Biometric
back to 8).

### 7.6 Third parcel — Recent & Favourites header icons

Your instruction mid-session: *"recent and favourites icon … shall be included in top header
left side after menu icon … want them on both at header and my home."* **D29 amended.**

- Two icons added to `_Layout.cshtml` immediately after the hamburger: **Recent** (clock) and
  **Favourites** (star). Each opens a popover — recent list (max 10) with star toggles, and the
  favourites list. Popovers are body-appended and fixed-positioned so no header container can
  clip them; outside click / scroll / resize closes them.
- Header and the My Home cards read the **same** localStorage stores (`pms_recent_forms`,
  `pms_fav_forms`); starring in the header refreshes the My Home cards live when open.
- On every page (layout-level), verified on `Home/Index` and `Home/Block`; build 0 errors.
- **Reverse by:** deleting the marked `hdr-quick` markup block, the `hdr-pop`/`hq-*` styles,
  and the marked popover script in `_Layout.cshtml`.

---

## Session 6 — 2026-08-14 (second)

**Driver:** your instruction to start building — front end first, security tasks first, nothing
that can break the running solution — plus `AI Files/New Instructions.xlsx`, read this session.

**Net result:** the two unauthenticated arbitrary-SQL endpoints are closed (`#5`, `#6`), Block
deletion no longer answers GET (`#113`), every HomeController screen now requires a signed-in
session (`#114`), sign-out clears the whole session, and baseline security headers ship on every
response. Build clean; every change verified live on a throwaway instance at `:5218`.

### 6.1 `New Instructions.xlsx` — two answers from you

1. **The rename-revokes-permission risk is accepted** — "we are rephasing the whole solution and
   currently I am the only user (admin), and I shall have all the access." Renaming is no longer
   hard-blocked; the registry (Stage B) remains the mechanism that makes it durable, but renames
   done before it must update the matching `PermissionForms.Name` rows in the same change.
   Recorded as **D24** in `PROJECT.md`.
2. **Charges → Revenue applies to the word "Charge" only.** `Surcharge Setup` and
   `Fixed Charges Bill Generation` are **not** renamed; words containing "charge" (Surcharge) are
   never touched. All other renaming from the mapping sheet is required. Recorded as **D25**.

### 6.2 Code changed — file by file

| File | Change | Reverse by |
|:--|:--|:--|
| `Controllers/api/DynamicQueryController.cs` | `[Authorize]` on the class (**`#5`** — Create/Update/ExecuteParamQuery stored and ran arbitrary SQL with no login). `ExecuteParamQuery` additionally rejects anything that is not a single SELECT/WITH statement | Remove the attribute and the guard block, both marked `SECURITY (#5)` |
| `Controllers/api/SapIntegrationController.cs` | `[AllowAnonymous]` removed from `GenerateDynamicReport` (**`#6`**) — the controller's `[Authorize]` now applies. File is excluded from local builds (`SapIntegration=false`), so this lands when SAP builds are next made | Re-add `[AllowAnonymous]` above the action |
| `Views/DynamicQuery/Index.cshtml` · `DynamicReport.cshtml` | Page-scoped `$.ajaxSetup` attaching the Bearer token, so the now-guarded endpoints keep working — same idiom as every other view | Delete the marked `$.ajaxSetup` lines |
| `Controllers/api/BlockController.cs` | `DeleteBlock` `[HttpGet]` → `[HttpPost]` (**`#113`**) | Swap the attribute back |
| `Views/Home/Block.cshtml` | The `DeleteBlock()` caller sends POST with `?id=` on the URL | Restore `type: "GET"` and the `data` field |
| `Extensions/SessionAuthorizeAttribute.cs` | **New** — authorization filter: no `ID` in session → redirect to the login page. Never returns 401 to a browser | Delete the file |
| `Controllers/HomeController.cs` | `[SessionAuthorize]` on the class (**`#114`** — ~50 screens); `[AllowAnonymous]` on `Error()` so the exception handler path stays public | Remove the two attributes and the two usings |
| `Controllers/Login.cs` | `SignOut` now calls `Session.Clear()` — the old key-by-key removal left `token` and `FullName` alive after sign-out | Restore the six `Session.Remove` lines |
| `Program.cs` | Response-header middleware at the top of the pipeline: `X-Content-Type-Options: nosniff`, `X-Frame-Options: SAMEORIGIN`, `Referrer-Policy: same-origin` — attachments included. **No CSP** — the legacy views are full of inline scripts and a CSP would break them | Delete the marked `app.Use` block |

### 6.3 Verified

- `dotnet build` — **0 errors** (warning count unchanged).
- Live probes against a second instance on `:5218`, then stopped:
  anonymous `/Home/Block` → 302 to login · `/Home/Error` → 200 ·
  anonymous `ExecuteParamQuery` and `GetAll` → 401 · `DeleteBlock` by GET → 405 ·
  `admin` login → guarded page 200 · SELECT with token → rows · DELETE statement with token → 400 ·
  all three headers present.
- The instance you run on `:5217` still has the old binary — **restart it to pick these up.**

### 6.5 Incident — "stuck in login": `PMS_Blank` lost its drive; both databases moved to D:

After a reboot the removable E: and F: drives were absent, `PMS_Blank` (all three files on
`E:\SQLWork`) went `RECOVERY_PENDING`, and every login died with SQL error 4060 — nothing to do
with the security changes. Once the drives were reconnected, at your instruction both blank
databases were **moved to the permanent internal drive** via backup → restore-with-MOVE (the SQL
service account owns the file ACLs, so a plain file copy is denied):

| Database | Now lives at | Was |
|:--|:--|:--|
| `PMS_Blank` | `D:\SQLWork\PMS_Blank.{mdf,ndf,ldf}` | all on `E:\SQLWork` |
| `DHA_Blank` | `D:\SQLWork\DHAB_Test.mdf` · `AdditionalFile1.ndf` · `DHAB_Test_log.ldf` | spread across E:, F: and D: |

Verified after the move: both `ONLINE`, 439 tables, the admin user present, full login flow
HTTP 200. `appsettings.json` briefly pointed at the `PMS_Local` fallback mid-incident and was
**put back to `PMS_Blank` on `.\MSSQLSERVER01`** — its committed state is unchanged.

Left behind, safe to delete whenever: the move backups `D:\SQLWork\PMS_Blank_move.bak` and
`DHA_Blank_move.bak` (~80 MB each), and any stray database files still under `E:\SQLWork` /
`F:\SQLWork` — SQL Server no longer references them. A reboot can no longer take the databases
down with the removable drives.

### 6.4 Not done, deliberately

- CSRF anti-forgery tokens across the ~600 state-changing AJAX calls — needs its own pass.
- The idle-logout blur overlay (N-3) — next front-end security item, Stage D scope.
- `[SessionAuthorize]` on the other 30 page controllers — proven pattern now; applying it wholesale
  needs a sweep for legitimately-public actions first.

---

## Session 5 — 2026-08-14

**Driver:** the laptop was restarted part-way through the structure-only backup work begun on
2026-08-13, and the run was lost. You asked me to carry on: blank the `F:\DHA_live_20260423_1424`
backup and produce both the script and a `.bak`.

**Net result:** `F:\DHA_Blank_Structure.bak` exists and verifies — **12.9 MB, from a 26.5 GB
source**, holding the complete `DHA_Live` schema with **zero rows**. Getting there exposed four
defects in the two scripts written on 08-13; none had ever been exercised, because every previous
attempt died in the restore before reaching them. All four are fixed.

### 5.1 What the restart had left behind

`DHA_Blank` on `.\MSSQLSERVER01` was stuck in `RESTORING` with its three files fully allocated on
disk — 310 GB across E:, F: and D:. `msdb.dbo.restorehistory` had no row for it, which is how the
interrupted restore was distinguished from a completed one. Nothing was salvageable; the database
had to be restored again.

### 5.2 The four defects, in the order they surfaced

| # | Defect | Fix |
|:--|:--|:--|
| 1 | **Space check refused to re-run.** `RESTORE` does *not* reuse the files of the database it replaces — it demands the full size free *again*, on top of what the old files occupy, and fails its own planning check. With 310 GB that shortfall is unfixable on this machine | `restore-and-blank.ps1` now drops any existing target **before** measuring free space |
| 2 | **`PRINT` with a subquery.** `PRINT 'x' + CAST((SELECT COUNT(*) …))` is invalid — msg 1046, *subqueries are not allowed in this context*. Three occurrences | counts assigned to variables first |
| 3 | **Bookkeeping in `##` temp tables.** They die with the session. The failed run left 205 temporal tables with versioning **off** and their history-table pairings **unrecoverable** — once versioning is off, `sys.tables.history_table_id` is `NULL` and the pairing is gone from metadata for good | state moved into two real tables inside the target database, captured once and reused on re-run |
| 4 | **`QUOTED_IDENTIFIER`.** sqlcmd defaults it **OFF**, unlike SSMS. This database has indexed views and filtered indexes, so every `INSERT`/`DELETE` failed with msg 1934 | `sqlcmd -I` in the pipeline, plus explicit `SET` statements in the SQL |

Defect 3 is the one that mattered. It is silent: the script would have reported success and
produced a backup whose 205 audit tables had quietly lost their system-versioning.

### 5.3 What changed in the scripts

- `tools/local-run/restore-and-blank.ps1`
  - drops the target unconditionally before the space check (the protected-name gate is what
    makes that safe)
  - carries the same protected-name gate as `blank-database.sql` — it did not have one, and it
    runs `RESTORE … REPLACE`, so a mistyped `-TargetDb` could have overwritten a real database.
    `tools/AGENTS.md` had claimed both scripts carried the gate; that is now true
  - passes `sqlcmd -I`
  - **re-saved as UTF-8 with BOM.** PowerShell 5.1 reads a BOM-less file as ANSI, which turned an
    em-dash inside a double-quoted string into a smart closing quote and broke parsing outright
- `tools/local-run/blank-database.sql` — reworked to be resumable, and rewritten **ASCII-only**
  so no encoding path can mangle it. It now refuses to finish quietly: step 8 compares foreign-key
  and temporal counts against what it captured and raises an error unless both match and every
  table is empty, keeping the bookkeeping tables for the retry

### 5.4 The result

| | |
|:--|:--|
| Source | `F:\DHA_live_20260423_1424` — 26.5 GB, **read-only throughout** |
| Output | `F:\DHA_Blank_Structure.bak` — 12.9 MB, `RESTORE VERIFYONLY` passes |
| Restore | 7.3 min · 310.4 GB of files · blanking took 4 s |
| Kept | 439 tables · 205 temporal · 193 foreign keys · 632 indexes · 48 procedures · 469 defaults |
| Rows | **0** |

`DHA_Blank` is still on `.\MSSQLSERVER01`, shrunk from 310 GB to 200 MB, so it costs almost
nothing to leave in place.

### 5.5 How to reverse it

Nothing in the repository or the application was touched, and the source backup was only ever
read. To undo completely:

```
sqlcmd -S .\MSSQLSERVER01 -E -Q "ALTER DATABASE DHA_Blank SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE DHA_Blank;"
del F:\DHA_Blank_Structure.bak
```

To rebuild it from scratch:

```
powershell -ExecutionPolicy Bypass -File tools\local-run\restore-and-blank.ps1 -SqlInstance ".\MSSQLSERVER01"
```

Add `-DropWorkingDbWhenDone` to release the 310 GB automatically. The run needs roughly 123 GB
free on E:, 98 GB on F: and 89 GB on D: — the drop in §5.2 defect 1 is what makes those numbers
achievable on a re-run.

### 5.6 The application moved onto the blank schema

You asked for the blank database to be restored and the local PMS instance pointed at it, with the
old connection dropped.

| | |
|:--|:--|
| New database | `PMS_Blank` on **`.\MSSQLSERVER01`**, restored from `F:\DHA_Blank_Structure.bak` |
| Files | `E:\SQLWork\PMS_Blank.mdf` · `.ndf` · `_log.ldf` — about 200 MB |
| Connection | `HRMS_Web/appsettings.json` line 41, `DefaultConnection` |
| Was | `Server=.;Database=PMS_Local` |
| Now | `Server=.\MSSQLSERVER01;Database=PMS_Blank` |

**The instance had to change, not just the database name.** The default instance `.` is SQL Server
**2019 (15.0)**; the backup came from **2022 (16.0)**. Backups restore forwards only, so the blank
database cannot live on the default instance at all. Any connection string that keeps `Server=.`
will fail.

**A blank database has no users.** Zero rows means no row in `PMSUser`, so nothing could sign in.
`tools/local-run/seed-local.sql` was run against `PMS_Blank` to put `admin` / `admin` back —
1 user, 222 permission forms, 222 mappings. It was safe to reuse: the three tables it touches
(`PMSUser`, `PermissionForms`, `UserPermissionMapping`) were diffed column-by-column between
`PMS_Local` and `PMS_Blank` first and are **identical**, 56 columns across the three. The script
only deletes from those three tables and inserts into them; it touches nothing else.

Verified end to end, not just at startup: `POST /Login/LoginToPortal` with `admin`/`admin` returns
`code 0` and a JWT, `sys.dm_exec_sessions` shows the connection on `PMS_Blank`, and `Home/Index`
returns 200 with 252 KB rendering the seeded user's name.

**`PMS_Local` was deliberately left in place** on the default instance. "Drop the older connection"
was read as the connection string, not the database — it is the only fallback if the production
schema turns out to disagree with what the EF model expects, and it costs nothing to keep. Say the
word and it goes.

To go back:

```
"DefaultConnection": "Server=.;Database=PMS_Local;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;"
```

To remove the new database:

```
sqlcmd -S .\MSSQLSERVER01 -E -Q "ALTER DATABASE PMS_Blank SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE PMS_Blank;"
```

**Known consequence of the move:** every screen backed by real data is now empty. The schema is
production-faithful but holds nothing, so lists, dropdowns and dashboards render with no rows.
That is the point of a blank database, but it means `PMS_Blank` is not a like-for-like replacement
for `PMS_Local` when exercising a form that needs lookup data behind it.

### 5.7 Enterprise-readiness verdict written

You asked whether the rebuild can reach enterprise level. All nine documents in `docs/` were
read end to end and the answer synthesised into **`docs/06-ENTERPRISE-READINESS.md`** — a
judgement document, not a summary. Its conclusion in one line: **yes, conditionally — every
technical dimension lands at B+ or better under the existing plan, and both dimensions that
stay red (delivery process, continuity) are process problems the plan never claims to cover:
no git, one machine, one person.** The document grades nine dimensions, judges the five locked
bets (none needs reversing), lists what "enterprise" means as a checklist split into
plan-guaranteed vs owned-by-no-document, and ends with a sequenced recommendation whose first
two items cost hours: install git, and copy the repo + `F:\DHA_Blank_Structure.bak` off this
machine. Reverse by deleting the file and its index lines in `AGENTS.md` and `CURRENT-WORKS.md`.

---

## Session 4 — 2026-08-13

**Driver:** you supplied `AI Files/AI file.xlsx` — the restructuring requirements workbook — and
asked for observations, a step-wise plan, a separate current-works file, and for the hidden forms
to be unhidden and located.

**Net result:** the workbook is analysed, four documents are written or updated, and **all fifteen
previously-unreachable forms are live on the local instance** behind one review page. Two small
controller changes were needed; everything else was documentation. The gate holds — nothing from
the restructure itself was built.

### 4.1 What the workbook contains

Four sheets, read via the OpenXML parts (no Excel on this machine, no Python):

| Sheet | Holds |
|:--|:--|
| Restructure | 13 numbered requirements, an "Others" list, and the Land sequencing instruction |
| PMS-Modules | **5 modules, 19 sub-modules, 163 forms mapped, 58 renamed** |
| Points | 17 answers — including the two Block questions I had been waiting on |
| UI | Home-screen mock, header strip, 7 mandatory navigation requirements, D365 F&O direction |

### 4.2 Decisions this forced

| Decision | Effect |
|:--|:--|
| **D20** — 5 modules, up to 5 menu levels | **Supersedes D14** (12 modules); withdraws D10's two-click principle. `docs/05-MODULE-ARCHITECTURE.md` §5 must be rewritten |
| **D21** — Land Management is the last task | **Stands `#140`–`#146` down.** D15–D19 remain locked; only their timing moves. The APPS tile region survives into the shell work |
| **D22** — registry before renaming | The governing constraint. `Permissions.FormName` is a string checked as a literal in `_Layout.cshtml`, so the menu label *is* the permission key. Seven live permission keys contain "Charge"; the workbook renames 58 forms. Renaming first would silently revoke those forms for every role |
| **D23** — size is not a target | Points 14. "Largest view < 200 lines" and "largest controller < 10 KB" withdrawn from the §10 metrics |
| **D9 closed** | Points 6.2 and 6.3: Block becomes a **foreign key**, **scoped to its parent**, uniqueness on *(parent, name)*. `#106` decided. Open sub-question: parent = Sector or Phase |

### 4.3 The hidden forms — located, and two restored in code

The audit had recorded 15 unreachable forms as a count (N15). This session located each one:
controller, action, view file, line count, and its live URL. **Thirteen were already reachable by
direct URL** and simply absent from the 243 KB menu. Two were genuinely dead:

| File | Change | Reverse by |
|:--|:--|:--|
| `HRMS_Web/Controllers/DemandNoteController.cs` | `PurchaseRequest()` was **commented out**, so a 719-line view could never render. Uncommented, marked in place with the task number | Comment the method out again |
| `HRMS_Web/Controllers/Operations.cs` | `TransferForm()` **did not exist at all** — `Views/Operations/TransferForm.cshtml` (386 lines) had no route. Added, marked in place | Delete the marked method |
| `HRMS_Web/Controllers/HiddenFormsController.cs` | **New.** Temporary scaffolding — serves one page at `/HiddenForms` | Delete the file |
| `HRMS_Web/Views/HiddenForms/Index.cshtml` | **New.** The review page: all 15 forms with routes, view paths and notes, plus the 6 near-duplicate pairs side by side. `Layout = null` so it opens even if the main layout or session misbehaves | Delete the file |

**Verified:** `dotnet build` → 0 errors. All 21 routes (15 hidden + 6 duplicate partners) probed
over HTTP → **200 each**. The application is left running at `http://localhost:5217`.

### 4.4 What the location work found that the count did not

- **The menu ships a form named *Test*** — `GlobalSetup/ChargesGroupFormTest` (215 lines) is the
  linked one, while `ChargesGroupForm` (405 lines) is unreachable. Almost certainly backwards.
- **Neither demarcation-charges form is in the menu.** `Home/DemarcationCharges` (217 ln) and
  `Home/DemarcChargesI` (326 ln) are both orphans; the menu's "Drawing Scrutiny Charges" points at
  `Home/DemarcationRequest`, a third form.
- **`Home/UOMDef` (358 ln) and `Home/Unitofmeasure` (356 ln) are two lines apart** — the one pair
  where size gives no signal at all. Needs the user's knowledge.
- **The menu links `Home/FingurePrint` three times**, and that is the 104-line version, while
  `FingerPrint/FingerPrint` (328 lines) is the substantial one.
- **Eleven of the fifteen have no home in the new 5-module structure.** Whichever survive the
  restore/retire verdict need a slot added to the mapping sheet before the registry is seeded, or
  they become unreachable again — this time by design.

### 4.5 Branding measured for requirement 12

`N-Stack` / `the old consultancy` → `N-Stack` / `N-Stack Consulting`. Measured: **2 live
occurrences**, both in `HRMS_Web/Views/Login/Index.cshtml` (lines 515 and 518 — an inline SVG
wordmark). Every other match is a build artefact under `obj/` or `bin/`. The real work is the
~25 logo image files under `wwwroot/img/`, which is why the Company Profile & Branding form
(workbook §9.2) should carry branding as data rather than a second hard-coded wordmark.

### 4.6 Documents

| File | Change |
|:--|:--|
| `docs/AI-FILE-OBSERVATIONS.md` | **New.** The workbook analysed: what it contains, the one constraint that governs its ordering, 8 decisions it overturns, 11 items of new scope, 8 internal gaps needing answers, the hidden-form register, the 9-stage plan, and 10 questions back |
| `CURRENT-WORKS.md` | **New.** Root-level bench: active work, what finished today, the 10 open questions, the stage order. Points at `PROJECT.md` for the backlog and does not duplicate it |
| `PROJECT.md` | "Now" rewritten; D9 closed; D20–D23 added; `#133` re-prioritised to blocked-on-you; `#140`–`#146` stood down; §12 document list extended |
| `AGENTS.md` | `AI Files/` declared source material, never edited. `CURRENT-WORKS.md` added as root-owned with its contract. Land sequencing recorded. `05-MODULE-ARCHITECTURE.md` marked as partly superseded |

Left unchanged deliberately: `docs/05-MODULE-ARCHITECTURE.md` itself (§5's rewrite to the
5-module structure is Stage A work and needs the gap answers first — flagged in place rather than
half-rewritten), `docs/roadmap.html` (refresh once the stage plan is agreed, not before),
`docs/02-ASSESSMENT.md` and `docs/03-REENGINEERING-PLAN.md` (D20–D23 belong to them once Stage A
closes; recording them twice now would create the contradiction DOX forbids).

---

## Session 3 — 2026-08-05

**One piece of work, no code.** You asked for an Odoo-style app chooser after login, joining the
Land Management application to PMS under the name **Real Estate Management System**, and nothing
else changed. Planning only — the gate holds.

**Net result:** the task turned out to be a different shape than it looked. Both codebases were
measured, the integration options were weighed, and a minimal reversible design is written and
waiting on your review.

### 3.1 The finding

`C:\Users\Adnan Ahmed\Pictures\test_Land_mgt` is **not another .NET solution.** It is a **Laravel 8
/ PHP application on MySQL**, measured from source:

| | Property Management | Land Management |
|:--|:--|:--|
| Stack | ASP.NET Core MVC, .NET 6 | Laravel 8, PHP 7.3/8.0 |
| Database | SQL Server `PMS_Local` | MySQL `test_dha_land_management` |
| Identity | `PMSUser`, HMACSHA512, session + 3 JWT schemes | `users`, Laravel Breeze session |
| Permissions | `PermissionForms` + `UserPermissionMapping`; key **is** the menu label | **~90 boolean columns on `users`**, one per form×action |
| Approval | Approval module, 5 screens | `approval_tree` / `approval_stage` / `approval_setup` — **a second, independent engine** |
| Layout | `_Layout.cshtml` **249 KB** | `main.blade.php` **277 KB** |
| Scale | 152 controllers · 209 forms · 439 tables · 316 migrations | 34 controllers · 42 models · 109 views · 51 migrations · 23 route resources |

Two things worth carrying forward beyond this task. **Both products have the same disease** — a
quarter-megabyte hard-coded layout carrying the entire menu. And **both built their own approval
engine.** If the two ever genuinely merge, that duplication is the real cost, not the menu.

So "combine" cannot mean one codebase here. A port is 34 controllers, 42 models and 109 views — a
second re-engineering programme. What was planned instead is a shell that makes them one product
*to the user*.

### 3.2 Added

| File | Size | What it is |
|:--|:--|:--|
| `docs/modules/rems-app-launcher.md` | 16 KB | The deliverable. Both systems measured side by side, C4 context and container diagrams, 7 requirements with acceptance criteria, 4 options weighed, **ADR-001/002/003**, component and configuration design, file-by-file change list with reversals, scope boundaries, 5 risks, 8 tasks, a manual verification script, and **5 open questions** |

### 3.3 Changed

| File | Change | Reversal |
|:--|:--|:--|
| `PROJECT.md` | New `§4` active item (`#138`–`#147`); old `§4` demoted to `§4c` parked; `#119` `doing`→`blocked`; **D15–D19 locked**; issues I7, I8; I1 now gates `#147` too; dependency rows; documents row; log entries | Restore `#119` to `doing`, delete the added rows |
| `AGENTS.md` | New section **Related system, outside this tree** — names the Land repository, its stack, and states it is not owned by this doc and must not be edited from here | Delete the section |
| `docs/WORK-LOG.md` | This entry | Delete it |

**No solution code was changed this session.** The four files listed in `PROJECT.md` §4b remain the
only code changes in the repository.

### 3.4 Discovered

- **PHP, Composer and MySQL are not installed on this machine**, and there is no XAMPP, WAMP or
  Laragon. The Land application **cannot run here at all**. Raised as issue **I7**. The launcher is
  designed so this does not block it: an app with no configured URL renders as a disabled tile with
  the reason shown, rather than a dead link.
- **`test_Land_mgt\.env` is committed** to that repository, carrying `APP_KEY` and database
  settings. The same leak class as **I2**, in the other repository. Raised as **I8**; out of scope
  for this task, recorded so it is not lost.
- **Git is still not installed** (`#13`, I1) — confirmed again this session, in both folders.
- The post-login redirect is a **single line of JavaScript**: `Views/Login/Index.cshtml:690`,
  `url = "/Home/Index"`. That one line is the entire insertion point for the launcher.

### 3.5 Decided — D15–D19, locked the same day

You answered all five questions in session, so the gate opened and closed within the session.

| # | Decision | Why |
|:--|:--|:--|
| D15 | Launcher as an additive shim inside `HRMS_Web`; the app list held as **configuration**, Land reached by URL | The stacks share nothing. A front door is buildable now; a merge is not. **Q1 confirmed one front door, not one codebase.** Config-as-data also honours locked **D13** and feeds `#125` rather than being thrown away by it |
| D16 | **PMS is the identity authority.** Land account linking deferred; **no credential is ever copied, synchronised or forwarded** | `PMSUser` and Laravel `users` are unrelated tables in different engines with different hashes. **Q2 accepted the second sign-in for now.** SSO is a real design task, not a lookup — a weak handoff would be an auth bypass, so it gets its own gate |
| D17 | Rebrand on the **launcher page only**; both 250 KB layouts otherwise untouched | `#125` replaces the PMS layout anyway; renaming it twice is waste |
| D18 | One **"Switch app" anchor** may be added to `_Layout.cshtml`; the 249 KB menu block stays untouched | **Q3.** Without it the launcher is a screen you pass once per login, not a place you return to. Takes the task from one changed line to two |
| D19 | The two repositories **merge into one**, `RealEstate/{property,land}` — **blocked on `#13`** | **Q4.** Matches the single-product framing. Raised as `#147` with **ADR-004**. Independent of the launcher, which reaches Land by URL, not by file path, so the merge changes nothing about how it works |

**Q5** — whether to skip the launcher for users who can see only one app — was not put to you; I
took it. **No.** You asked for a selection panel; auto-skipping undermines the feature and hides the
second app from anyone whose permissions later change.

### 3.6 Where it stopped

At the gate, as the method requires — but the gate moved. `#138` (your review) and `#139` (the five
questions) both closed this session. `#140`–`#146` are specified, sequenced and **not started.**
Nothing will be built until you say go.

Two consequences of your answers, recorded so they are not lost:

- **`#146` was added** — the switch-app anchor. The task is now **seven files and two changed lines
  of behaviour**, not six and one.
- **`#147` was added and is blocked.** The repository merge you asked for cannot begin until git is
  installed (`#13`, I1). That issue now gates three tasks, not two.

The honest summary of what was planned: **it makes them look like one product; it does not make
them one product.** ~1.5 days, fully reversible.

---

## Session 2 — 2026-08-04

**Two pieces of work.** First, the module and navigation architecture: every form in the system
located, measured and assigned a home. Second, at your request, getting the application to
actually run on this machine — which it had never done.

**Net result:** the structure of the whole front end is designed and documented, and the
application is live at `http://localhost:5217` for the first time.

### 2.1 Added

| File | Size | What it is |
|:--|:--|:--|
| `docs/05-MODULE-ARCHITECTURE.md` | 42 KB | The main deliverable. Current-state navigation audit, the target 12-module taxonomy, the shell design, the registry data model, naming standard, extensibility rules, and **all 209 forms mapped** to a module, sub-area and item type |
| `HRMS_Web/Extensions/SapIntegrationStub.cs` | 3 KB | Local stand-in for `SapIntegrationController`. The 13 methods the other nine controllers call, each returning an explicit "SAP not available" result — never a fake success. Compiled only when `SapIntegration != true` |
| `docs/WORK-LOG.md` | — | This file |
| `tools/AGENTS.md` | 2 KB | DOX contract for the new `tools/` boundary — first child AGENTS.md in the repository |

**Also created — `tools/local-run/`, kept in the repository so the local environment is
reproducible:**

| File | Size | Purpose |
|:--|:--|:--|
| `seed-local.sql` | 88 KB | Creates the `admin` / `admin` user and all 222 permission rows. Re-runnable — it clears the three tables first. **Local database only** |
| `patch-schema-drift.sql` | 9 KB | The 118 `ALTER TABLE` statements that added `LastModifiedUserName` to 235 tables |
| `HRMS_Web.csproj.original` | 6 KB | Untouched copy of the project file as it was before today |
| `legacy-menu-tree.txt` | 11 KB | The full 251-line menu tree parsed out of `_Layout.cshtml` — the evidence behind the N1–N16 defects |

**Not in the repository, deliberately:** `appsettings.original.json` holds the original
connection string *including the live `sa` password*, so it stays in the session scratchpad and
will disappear with it. Copy it somewhere private if you want it. For reference, the original
pointed at server `WIN-CM05CUDDJMV`, database `DHA_Live`, user `sa` — the password is unchanged
and still needs rotating (`#15`).

**Also created, outside the repository:** the `PMS_Local` database on your local SQL Server —
439 tables built from the 316 migrations, then patched and seeded by the two scripts above.

**Installed on this machine:** `dotnet-ef` 6.0.10 as a global tool. Required to build the
database from migrations. Remove with `dotnet tool uninstall --global dotnet-ef`.

### 2.2 Changed

**Documentation**

| File | Change |
|:--|:--|
| `PROJECT.md` | `Now` → `#119`. Counts → 7 of 133. §4 replaced with the architecture workstream; Block moved to §4a *Parked*; new §4b *Running locally*. Decisions **D10–D14** added. Five navigation metrics added. Issue I3 closed, I4 rewritten, **I6 opened**. Three log entries. Tasks `#117`–`#137` added |
| `AGENTS.md` | `docs/05-MODULE-ARCHITECTURE.md` added to the project documentation list |
| `docs/01-SYSTEM-OVERVIEW.md` | New §3 subsection: *Navigation and the permission catalogue are the same string* |
| `docs/02-ASSESSMENT.md` | **B7** — the permission key is the menu label. **D5b** — the migrations do not reproduce the model. **D5c** — the project cannot be built by `dotnet build` |
| `docs/03-REENGINEERING-PLAN.md` | Three rows added to *Decisions locked* (navigation model, module structure, form registry). Phase 6 now specifies a registry-driven shell and forbids retiring the old menu before the permission migration verifies |
| `docs/04-WORK-INVENTORY.md` | New section *Two groupings, and why they differ* — 16 rebuild modules vs 12 shell modules. Totals recounted: 277 `.cshtml` → **209 real forms** |
| `docs/roadmap.html` | Masthead, stat strip, a new *Active item* section with the shell diagram and the twelve-module table, Block section retitled *Parked*, D10–D14, *Waiting on you*, footer, five metric rows. Tag balance verified |

**Solution code** — four files, the first code changes in this project. All reversible, all
marked in place.

| File | Change | How to reverse |
|:--|:--|:--|
| `HRMS_Web/HRMS_Web.csproj` | Added a `SapIntegration` property, default `false`. When off, the three SAP extension files and `SapIntegrationController` are excluded from compilation and the stub compiles instead; the two `<COMReference>` items are skipped. When `true`, original behaviour exactly | `msbuild /p:SapIntegration=true`, or restore `HRMS_Web.csproj.original` |
| `HRMS_Web/Extensions/SapIntegrationStub.cs` | New file (see 2.1) | Delete it and build with `SapIntegration=true` |
| `HRMS_Web/Controllers/api/FilterController.cs` | Two `#if !SAP_INTEGRATION` guards in `GetFixedArrearsAndAdvanceByRegistrationNo` and `GetFixedHistoryByRegistrationNo`. Each returns exactly what that method's own `catch` block already returned when SAP was unreachable | Build with `SapIntegration=true` — the original code path is still in the file, untouched |
| `HRMS_Web/appsettings.json` | `DefaultConnection` → `Server=.;Database=PMS_Local;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;` | Restore `appsettings.original.json` |

**Local database, changed outside the repo:** 118 `ALTER TABLE` statements added
`LastModifiedUserName` to 235 tables that the migrations had created without it. Reverse by
dropping `PMS_Local` and re-running `dotnet ef database update`.

### 2.3 Removed

**Nothing.** No file was deleted, no code was removed, no menu item was taken away. Because
there is no git here, everything that would normally be a deletion was made a build switch or a
`#if` instead. Every original line is still on disk.

### 2.4 Discovered

**Navigation — 16 defects, all verified against the running code**

| # | Defect |
|:--|:--|
| N1 | Master data buried under a transaction menu — Phase, Block, Force, Rank, Category, UOM, Prefix, Postfix, Quota, Almt all live under `Transfer & Records` |
| N2 | `Transfer & Records` contains a child menu called `Transfer & Record` |
| N3 | `Operation Forms` is 30 flat items spanning five domains |
| N4 | `Administration → Reports → MemberReports` is fake — it lists Floor, Features, Finishes, Sector: the *setup* items pasted in. Two sibling branches are the same paste |
| N5 | "Transfer Set Receiving" opens `Home/SitePlan` — wrong target shipped |
| N6 | "Drawing Scrutiny Charges" opens `Home/DemarcationRequest` — label/target mismatch |
| N7 | Two "Finger Uploader" links call `Uploader/FingerUploader`; `UploaderController` has only `Index()`. Both are 404s |
| N8 | `Calendar Setup` contains SAP Billing and GL Determination |
| N9 | Two groups render with no children |
| N10 | Six single-item top-level menus |
| N11 | The dealer domain is spread across six different menus |
| N12 | Meter Type / Phase / Status / Reading Officer sit beside the meter bill generation runs |
| N13 | `Globalsetup/ChargesGroupFormTest` — a form named *Test* — is in the live menu as "Charges Incorporation Setup" |
| N14 | 22 duplicated links; `Home/FingurePrint` ×3, `Sales/LeadGeneration` ×3 |
| N15 | 15 working forms are unreachable from the menu |
| N16 | No naming standard; 12 typos shipped in visible labels |

**Structural — the finding that shaped the design**

`Permissions.FormName` is a `string`, and `_Layout.cshtml` checks
`Html.UserHavePermission("<menu label>")`. `PermissionForms` is a flat table with no parent, no
module, no hierarchy. **The menu label is the permission key.** Renaming a menu item silently
revokes access for every role, and no module concept exists anywhere in the data model.

Confirmed by experiment later the same day: seeding `PermissionForms` with the 222 distinct
permission strings scraped out of `_Layout.cshtml` produced a working, fully-permissioned
application. The menu really is the permission catalogue.

**Build**

`dotnet build` fails with `MSB4803` — the .NET Core MSBuild cannot process COM references at
all, before compiling a single file. Only the Visual Studio MSBuild can, and only where the SAP
client is installed and its type libraries registered (both GUIDs checked, both missing here).
The build was never merely machine-locked; it was toolchain-locked, and no CI runner could have
built it under any configuration.

**Schema — the most serious finding of the day**

The 316 migrations apply cleanly and produce 439 tables, but the result does not match the entity
model. `LastModifiedUserName` is declared on `BaseModel`, so every entity has it — and **235 of
the 439 tables were created without it**. Signing in crashed with
`SqlException 207 — Invalid column name 'LastModifiedUserName'` from `AlertService.GetNDC()`.

The live database presumably has these columns, added by hand outside the migration history. So
the migration history is not a reproducible description of the schema. This changes how `#46`
must be done: the squash has to be verified by diffing the **live** database, not a rebuilt one,
or every hand-added column gets silently dropped.

**Counts measured**

| Measure | Value |
|:--|:--|
| Real forms (excluding 64 partials and 4 shared) | 209 |
| Reachable from the menu | 178 |
| Unreachable | 15 |
| Menu leaf links | 200 |
| Top-level menu groups | 22 |
| Maximum menu depth | 4 levels |
| Distinct permission names | 222 |
| Tables created by the migrations | 439 |
| Tables missing a `BaseModel` column | 235 |

### 2.5 Decided

| # | Decision |
|:--|:--|
| D10 | Module-workspace navigation — a module rail, a landing page per module, forms grouped by item type. Nothing more than two clicks deep |
| D11 | Setup lives inside its owning module, **and** in one central Configuration index under Administration. One definition, two views |
| D12 | The menu, the permission catalogue and the API authorization policies all read one form registry held as data, keyed by a stable opaque `PermissionKey` |
| D13 | Modules and sub-areas are data — added, renamed and re-ordered without touching shell code |
| D14 | Twelve top-level modules; sub-areas carry the detail |

Alternatives considered and rejected are recorded in `docs/05-MODULE-ARCHITECTURE.md` §2.1 —
Oracle Fusion's work/setup split and SAP Fiori's role launchpad. Ideas from both were kept.

### 2.6 What this bought

**Realised today**

- **The application runs.** First time on this machine, and the first time it has been possible
  to see a screen without the SAP client installed. `Home/Index`, `Home/Block`, `Home/PhaseDef`
  and `Approval/Inbox` all verified rendering.
- **The build is no longer toolchain-locked.** `dotnet build` succeeds. That is the precondition
  for CI (`#38`) that nobody knew was missing.
- **A working local environment exists** — database, admin user, full permission catalogue — so
  every future change can be checked in a browser instead of by inspection. That closes I3, which
  had been "accepted" as a permanent limitation.
- **Every form in the system has a name, a location and an owner.** Before today no list of them
  existed; `Home` held 50 unrelated forms and nothing said where anything belonged.
- **Fifteen lost forms found.** KYC, Deal Merger, Dealer Reservation, Booking Backlog, Purchase
  Request and ten others exist and work but nothing links to them.
- **Two migration traps caught before they cost anything** — the schema drift, which would have
  made `#46` silently destructive, and the fact that no clean environment can be built from the
  repository.

**Set up for later**

- The registry (D12) collapses three disagreeing definitions of "a form" into one, and makes a
  hidden menu item and a rejected API call impossible to disagree.
- Stable permission keys mean forms can be renamed and moved between modules without anyone
  losing access — which is what makes the whole restructure safe to do at all.
- Every module rebuilt from here drops into a structure that already exists, instead of each one
  re-deciding where its screens live.
- Adding a form, a sub-area or a whole module becomes a data insert. That was your explicit
  requirement, and it is what makes the twelve modules a starting point rather than a ceiling.

### 2.7 Tasks added

`#117`–`#119` architecture analysis and the gate · `#120`–`#129` registry, navigation service,
shell, search, favourites, configuration index, retiring the old menu · `#130`–`#133` small
legacy repairs to the running app · `#134`–`#137` local run, and the schema drift it exposed.

### 2.8 Open at close of day

| What | Who | Blocks |
|:--|:--|:--|
| Review `docs/05-MODULE-ARCHITECTURE.md` and answer its six questions | You | `#119` and the whole shell workstream |
| Review `docs/modules/block.md` | You | `#103` and everything after it |
| Install git and put it on PATH | You | `#14`, `#38`, and this file being unnecessary |
| Rotate the leaked credentials | You | Nothing technically — they are simply live |
| Restore a real database | You | Billing parity, `#29`, `#69` |
| Schema drift, 235 tables | Me | `#136`, and how `#46` must be done |

---

## Session 1 — 2026-08-03

Reconstructed from the `PROJECT.md` log and the documents produced that day.

### 1.1 Added

| File | What it is |
|:--|:--|
| `PROJECT.md` | The work tracker and charter — single source of truth |
| `AGENTS.md` / `CLAUDE.md` | The DOX working contract for the repository |
| `docs/01-SYSTEM-OVERVIEW.md` | Current-state architecture, domains, scale, build reality |
| `docs/02-ASSESSMENT.md` | Verified defects and risks, worst first, P0–P3 |
| `docs/03-REENGINEERING-PLAN.md` | Target architecture and the phased plan |
| `docs/04-WORK-INVENTORY.md` | Every screen, controller and process, grouped into 16 modules |
| `docs/modules/block.md` | First deep-dive — the Block form |
| `docs/roadmap.html` | Status page mirroring `PROJECT.md` |

### 1.2 Discovered

- Two unauthenticated arbitrary-SQL endpoints, and production secrets committed to the
  repository including the `sa` password.
- Authorization is authentication-only: per-form rights exist but are enforced client-side.
- `Block` is a SQL Server **temporal table** — change history is already captured by the
  database, and the migration squash destroys it unless system-versioning is preserved.
- Block has **no** parent relationship to Sector. It references nothing, and 20 entities store
  the block *name* as free text. In `StockCreation` the foreign key was written, then commented
  out. Raised as **D9**, still open.
- 16 defects in the simplest screen in the application.

### 1.3 Decided

**D1–D8** — full rewrite reusing the domain model and approval engine · .NET 10 LTS · Razor with
a modern component structure, no SPA · local only, every remote connection severed · SAP behind
one interface in one assembly · one base entity convention with column names preserved ·
`Result<T>` and problem details replacing the universal envelope · Block as the first slice and
the master-data pattern.

**D9 opened** and still open — Block as a foreign key, or free text. Affects 20 entities.

### 1.4 Method agreed

Markdown only, one item at a time, depth over speed, and a hard stop at your review before
anything is built. Recorded in `AGENTS.md` so it survives into future sessions.

---

## How to reverse this session's code changes

```
copy tools\local-run\HRMS_Web.csproj.original  HRMS_Web\HRMS_Web.csproj
del  HRMS_Web\Extensions\SapIntegrationStub.cs
```

`FilterController.cs` needs no reversal — building with `/p:SapIntegration=true` takes the
original path. `appsettings.json` needs the original connection string put back by hand; see the
note in §2.1 for why that file is not in the repository.

To drop and rebuild the local database from scratch:

```
sqlcmd -S . -E -Q "ALTER DATABASE PMS_Local SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE PMS_Local;"
dotnet ef database update --project B_DB_Context --startup-project HRMS_Web
sqlcmd -S . -E -d PMS_Local -i tools\local-run\patch-schema-drift.sql
sqlcmd -S . -E -d PMS_Local -i tools\local-run\seed-local.sql
```

---

## Running the application

```
dotnet run --project HRMS_Web\HRMS_Web.csproj --urls http://localhost:5217
```

`http://localhost:5217` · `admin` / `admin`

The application currently runs against **`PMS_Blank` on `.\MSSQLSERVER01`** — the blank production
schema — not `PMS_Local`. See §5.6. `PMS_Local` is still on the default instance as a fallback.

That credential is deliberately weak, at your request. It is valid only in the two throwaway local
databases seeded by `tools/local-run/seed-local.sql` — `PMS_Local` and `PMS_Blank` — neither of
which holds real data. Nothing seeds a user in the rebuilt solution, and this account must never be
created anywhere holding real data.
To change it, edit the `$pw` value where the seed script was generated and re-run it — the
password is stored as an HMACSHA512 hash plus a per-user key, never as text.

Unavailable in this build: SAP Operations, SAP Billing, GL Determination, and two meter-billing
grid endpoints — all report SAP unavailable rather than failing silently. Everything else works.

## 2026-08-26 - Payroll Management joins the suite
- New application **PAYROLL** ("Payroll Management", `C:\Users\Adnan Ahmed\Pictures\Payroll\Payroll_HCC2`, ASP.NET MVC 5 on IIS Express :7637 under virtual path `/payroll`).
- `Extensions/PayrollProxyMiddleware.cs` (clone of the LIMS proxy) serves `/payroll` on this host; settings `Erp:PayrollUpstream`, `Erp:PayrollPrefix`; registered in `Program.cs` after `UseLimsProxy`.
- Registry: the unused HRMS placeholder row/role in `ERP_Platform` became `PAYROLL` / `PAYROLL_USER` (`LMIS/database/erp_platform.sql` seed updated; applied to the local DB).
- `_ErpAppSwitcher.cshtml` and `Views/Apps/Index.cshtml`: PAYROLL branch uses the PMS-family skyline mark; launcher prewarms `/payroll/erp/touch`.
- Payroll side implements the LIMS `ErpSso` contract (cookie validation against `dbo.Sessions`, `/erp/touch`, `/erp/verify`, central login/sign-out redirects) and carries the same header switcher ("Applications Library" first).

## 2026-08-28 - Payroll shell audit (switcher / navigation "stuck")
- Symptom after the 2026-08-26 integration: in Payroll Management the application switcher and the
  navigation did not respond. Full-chain audit run with curl + headless Edge (login -> /Apps -> /payroll
  through the proxy -> LIMS -> sign-out): proxy, `erp_sso` validation, registry rows, provisioning,
  revalidation and global sign-out all correct; PMS shell unaffected.
- Cause 1 (Payroll `Content/theme.js` line 166): `$.getJSON(ERP.url('/Security/RecentActivity').done(`
  - missing `)` -> `SyntaxError` -> the whole shell script never ran (nav collapse, tree-view, global
  search, recent activity, toasts, `ERP.post/confirm` used by 4 views). Fixed.
- Cause 2 (Payroll `Content/theme.css` `.main-header .logo`): AdminLTE's `overflow:hidden` was never
  overridden, so the switcher menu opened inside the 56px header strip and was clipped. Added `overflow: visible`.
- `Views/Shared/_Layout.cshtml` cache-buster bumped `v=20260826i` -> `v=20260828a`. No rebuild needed
  (static + view). Payroll has no git; PMS/LIMS untouched.
- Noted, not changed: `AppsController` never sets `ViewBag.PayrollPrefix` (view falls back to `/payroll`);
  nothing auto-starts PMS/LIMS/Payroll after a reboot (`PayrollHCC-AutoStart.vbs` is not in shell:startup).
- Follow-up the same day: `AppsController.Index` now sets `ViewBag.PayrollPrefix` from `Erp:PayrollPrefix`.
  Suite auto-start added at workspace root: `Start-EnterpriseSolution.ps1` (idempotent: SQL service, LIMS
  `php artisan serve`, PMS `dotnet run`, then `Start-PayrollHCC.ps1`; log `Start-EnterpriseSolution.log`) and
  `EnterpriseSolution-AutoStart.vbs`, a copy of which sits in `shell:startup` so all three apps come up at logon.

## 2026-08-28 (later) - Payroll brand mark
- Payroll Management now has its own mark: the user's "group of people" icon (`Payroll_HCC2\brand\payroll-mark-source.svg`)
  redrawn as solid silhouettes by `Payroll_HCC2\brand\payroll-mark-generator.py` - ink on transparent for logos,
  white on a charcoal tile for the favicon/app tiles (same monochrome rule as the PMS mark).
- PMS side: `_ErpAppSwitcher.cshtml` PAYROLL row and the `/Apps` PAYROLL tile draw that mark (inline SVG, mask for the
  gap between figures); `erp-platform.css` gained `.erp-logo-people` (22x14, ink / white when current). The PMS skyline
  is PMS-only again.

## 2026-08-28 (evening) - Shell parity round
- Navigation: a **Home** entry (house icon, active on the landing page) and a "MODULES" section label now head
  the tree (`_NavigationMenu.cshtml`; label CSS in `_Layout.cshtml`, hidden in icons-only mode) - Payroll parity.
- My Home Apps card: tiles show the application marks (PMS skyline, LIMS mark, Payroll people mark) instead of
  initials, as LIMS and Payroll do.
- Payroll (no git): "Collapsed View" button bottom-left of its navigation with the same sidebar glyph as PMS/LIMS
  (was "Collapse navigation" + chevron); Apps card on its home (`Views/Home/Index.cshtml`, styles in `theme.css`):
  marks, Current/Open/Coming-soon states, "All applications" link to `/Apps`; favicon regenerated as ONE avatar on the
  charcoal tile (the three-figure mark blurs at 16 px), icon links versioned `?v=20260828c`.


## 2026-08-28 (night) - Payroll: Approval Setup
- Payroll (no git) gained the approval configuration PMS has (`Approval/ApprovalSetup`): form **Security.ApprovalSetup**
  "Approval Setup" under Security & Administration. Per request type (process): approval required on/off, ordered stages
  with name, approver role or *Any approver*, named users, approvals needed. Engine (`BusinessLayer\Approvals.cs`) now
  routes stage by stage (`CanDecide`, `Decide` -> `ApprovalDecisionResult.Final`), queue shows "stage x of y" and only
  offers decisions to eligible approvers; side effects run at completion; callers honour `RequiresApproval`.
  Tables `ApprovalProcess`, `ApprovalStage`, `ApprovalStageUser`, columns `ApprovalRequest.CurrentStage`,
  `ApprovalHistory.Stage` (idempotent `SchemaUpgrade`). Verified with a two-stage run through the HTTP endpoints.
- Follow-up: **any form can be selected for approval** (Approval Setup → Add form: form + Create/Edit/Delete). The
  `RequirePermission` filter holds the write for non-approvers (captured request in `ApprovalRequest.Payload`),
  the approver's browser replays it with `X-Approval-Replay` on final approval, and the filter marks it applied.
  Verified end-to-end with a temporary HR Officer account (held → approved → replayed → refused twice → cleaned up).

