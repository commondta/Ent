# 07 — UI Theme & Design System

How the application looks, why, and where to change it. Applied 2026-08-22 from the
brief in `Pictures\Theme` (ERPNext-inspired black & white) and the PMS theme as it
stands today, so both products read as one family.

**Rule:** white first, black for focus, gray for structure. Colour appears only where it
carries meaning (errors, approval states).

---

## 1. Files

| File | Role |
|---|---|
| `public/assets/css/lmis-theme.css` | The theme layer. Loaded last in `layouts/main.blade.php`. Defines the `--lm-*` tokens, re-tokens Phoenix's `--phoenix-*` variables for light and dark mode, and restyles every shared component. |
| `public/assets/css/inter.css` + `public/assets/fonts/inter/` | Self-hosted Inter (400/500/600/700, latin + latin-ext). Offline. |
| `public/assets/js/lmis-theme.js` | Behaviours: breadcrumb + title chip + page-bar favourite star, Ctrl+K command search, required-field asterisks, table empty-state vector, **Recent/Favourites store + header popovers** (`window.limsRecentFav`), **navigation** (chevron chips, active form from the URL + pre-expanded trail, icons-only below 1200px, ⋮ More). |
| `resources/views/home.blade.php` + `app/Http/Controllers/HomeController.php` | **My Home** — the landing workspace (Overview Analytics · To-Dos · Apps · Recent · Favourites), the same shape as PMS's My Home. `/` and `/dashboard` redirect to it. |
| `public/assets/css/custom-premium.css` | Login page only now (its sidebar/top-bar rules were removed; the logo glow is scoped to `.login-page`). |
| `public/assets/img/lmis-logo.svg`, `lmis-logo-dark.svg`, `lmis-icon.svg` (+ `lmis-logo.png`, `icons/logo.png`, `lmis-icon.png`, `favicons/*.png` rasters) | The final product mark (SVG from the user: rounded box, land, tower crane, crawler crane). White version in the top bar and login card; dark (`#111111`) version for print letterheads; black-background icon for favicons. Change colour by editing the final `<rect fill>`; re-export rasters with headless Edge + PHP-GD as in WORK-LOG round 4. |
| `public/assets/img/n-stack-logo.png` | Footer "Powered by N-Stack" mark. |
| `layouts/main.blade.php` | Links the files; its inline style blocks use the tokens; carries the command-search pill in the top bar. |

The login page (`auth/login.blade.php`) and the navigation **items** were deliberately
left alone.

## 2. Tokens (`:root` in `lmis-theme.css`)

| Token | Light | Dark (`html.dark`) | Use |
|---|---|---|---|
| `--lm-ink` | `#111111` | `#F5F5F5` | primary text, primary buttons, selected bars |
| `--lm-text-2` | `#4D4D4D` | `#C8C8C8` | labels, secondary text |
| `--lm-muted` | `#777777` | `#9A9A9A` | hints, breadcrumbs, sidebar labels |
| `--lm-border` / `--lm-border-light` / `--lm-border-strong` | `#E5E5E5` / `#EEEEEE` / `#D4D4D4` | `#2A2A2A` / `#222222` / `#3A3A3A` | all dividers |
| `--lm-surface` | `#F7F7F7` | `#161616` | table heads, section strips, chips |
| `--lm-hover` / `--lm-selected` | `#F3F3F3` / `#F0F0F0` | `#1E1E1E` / `#232323` | hover rows, active menu |
| `--lm-bg` / `--lm-card` | `#FFFFFF` | `#0D0D0D` / `#111111` | page / cards |
| `--lm-disabled-bg` / `--lm-disabled-text` | `#F5F5F5` / `#999999` | `#1A1A1A` / `#6E6E6E` | read-only fields |
| `--lm-header-bg` / `--lm-header-fg` | `#242729` / `#FFFFFF` | `#0B0B0B` / `#FFFFFF` | top bar (charcoal, as in PMS) |
| `--lm-btn-bg` / `--lm-btn-hover` / `--lm-btn-active` | `#111111` / `#3A3A3A` / `#000000` | `#F5F5F5` / `#D9D9D9` / `#FFFFFF` | primary button |
| `--lm-danger` / `--lm-success` / `--lm-warning` (+ `-soft`) | `#B42318` / `#1F6F47` / `#8A5A00` | lighter variants | semantic only |
| `--lm-focus-ring` | `rgba(17,17,17,.14)` | `rgba(255,255,255,.18)` | focus halo |

Phoenix's bluish gray scale (`--phoenix-gray-*`, `--phoenix-100…1100`, `--phoenix-primary`,
body/input/navbar colours) is overridden with neutral values in both modes, so Phoenix
utilities such as `text-900`, `bg-soft`, `border-300` automatically land on the palette.

## 3. Components

- **Top bar** — charcoal, white outline icons, the vector brand mark + full-name wordmark, white count pills,
  the light/dark toggle (kept — it is the "black & white contrast switching" feature), and the
  command search pill.
- **Sidebar (PMS parity, 2026-08-23)** — white, ink text, 16px outline icons. Every row is one
  flex line [icon][text][chevron]; the chevron is PMS's hand-drawn solid chevron in a 22px chip
  (double on main modules, single on sub-folders) pinned at the same right edge at every depth —
  gray token → brighter on hover (scale 1.06) → black chip with white glyph while open, glyph
  rotated 90°. Depth indentation 20 / 36 / 52px, no guide line. Open folder = `--lm-selected`
  row + 3px inset ink bar + 600 label; active form (exactly one, marked from the URL; its ancestors
  render expanded) = same tint + bar + 500 label and the short centred line; typography 600 module ·
  500 sub-folder · 400 form (12.5px); hover = background lift + ink, never a weight change.
  Icons-only mode below 1200px (Collapsed View still wins above it). In icons-only mode **clicking**
  a module icon opens its list in a fly-out panel to the right (PMS / NetSuite style): white
  bordered panel, black top edge, module title header, one open at a time, outside click / Esc
  close it — Phoenix's hover panels are off. `My Home` heads the list; the "Collapsed View" toggle
  stays pinned at the bottom-left of the panel.
- **Page bar** — `lmis-theme.js` builds `Home / Module / Page` from the active menu item and
  puts a black icon chip (the page's own feather icon) in front of the card title. Titles are
  15px, 600, uppercase, `.04em` tracking. On form pages the bar ends with the **Add to
  favourites** star (amber when on). The home page (`.lm-no-crumbs`) has its own title row.
- **Recent & Favourites** — one browser-side store (`lims_recent_forms`, `lims_fav_forms`) read by
  the header clock/star popovers (`.lm-pop`), the page-bar star and the My Home cards; any star
  toggles the favourite everywhere. Amber `#ca8a04` is the one accent, as in PMS.
- **Header overflow** — below 768px the Recent / Favourites / approval icons fold into a ⋮ More
  dropdown and the search pill folds into a search icon that drops the pill under the bar; brand
  and user never leave the bar.
- **My Home Apps card** — the current application tile carries `is-current`: ink outline, inset
  bar, live dot before "Current", slow breathing ring (monochrome, reduced-motion safe).
- **Cards** — white, 1px `#E5E5E5`, 8px radius, no shadow. Inner section strips `#F7F7F7`.
- **Buttons** — primary family (`btn-primary`, `btn-phoenix-primary`, `btn-info`,
  `btn-success`, `btn-warning`, submit buttons) = black, hover `#3A3A3A` + soft shadow, press
  `#000`; secondary = white + ink + gray border; danger = outline red that fills on hover
  (semantic, quiet). 6px radius everywhere.
- **Forms** — white inputs, 1px `#E5E5E5`, 6px radius, 38px; focus = black border + ring;
  read-only = `#F5F5F5` / `#999`; labels 12.5px/500 `#4D4D4D`; `*` added to labels of
  `required` fields by script; Select2 and the layout's multi-select follow the same rules.
- **Tables** — `#F7F7F7` heads, 11.5px uppercase 600, light borders, no stripes, hover
  `#F3F3F3`; DataTables controls, pagination (active page black) and the empty-state inbox
  vector restyled.
- **Tabs** — transparent, active = black text + 2px black indicator.
- **Badges** — outlined pill with a status dot; success/warning/danger keep a muted hue.
- **Footer** — `Land Information Management System | © year | Powered by [N-Stack]`, 12px muted (all 71 views).

## 4. Command search

`Ctrl+K` (or `/` outside an input) focuses the pill. It indexes every link in the sidebar
(module → form), filters as you type, supports ↑ ↓ Enter Esc, and navigates to the chosen
form. No server call; the index is the rendered menu, so it respects the user's
permissions automatically.

## 5. Changing things

- New colour needed? Add a token in `:root` **and** `html.dark`, then use `var(--lm-…)`.
- New component? Style it in `lmis-theme.css` section 6–11 with the tokens; avoid hex
  literals in views. Per-view `<style>` blocks that used blue/yellow/black literals were
  remapped to tokens on 2026-08-22 (782 replacements across 78 views).
- The top bar can be made white (spec §6 default) by setting `--lm-header-bg: #FFFFFF`,
  `--lm-header-fg: #111111`, `--lm-header-muted: #777777`, `--lm-header-hover: #F3F3F3`,
  `--lm-header-border: #E5E5E5` — everything else follows the tokens.
