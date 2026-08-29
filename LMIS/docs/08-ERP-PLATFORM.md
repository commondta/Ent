# 08 — ERP Platform: one solution, one login, one URL

How PMS (.NET 6) and LIMS (Laravel 8) are joined into a single ERP per the brief
`Application-Based ERP Structure.txt`. Milestone 1 (2026-08-22) delivers the single
login, the central identity/permission database, the application selector and the
common URL; phase 2 (below) moves form/action authorisation to the centre.

## 1. Shape

```
browser ──► http://localhost:5217  (PMS host = the ERP host)
              ├── /Login/Index          .NET PMS login  = the ONLY login
              ├── /Apps                 application selection ("My Home" entry)
              ├── /Home/…, /Operations… PMS itself
              └── /lims/…               reverse-proxied to LIMS (Laravel on 127.0.0.1:8000); /lims/ lands on LIMS's My Home (/lims/home)

ERP_Platform (SQL Server, MSSQLSERVER01) ── identity, applications, roles, permissions, SSO sessions
PMS_Blank ── PMS business data        [Land Management] ── LIMS business data
```

One host → one cookie jar. PMS's `Login/Index` signs the PMS session in from a live central
session (no second login when arriving from LIMS); LIMS's switcher opens other applications via
`/Apps/Go?code=…`. The PMS login writes a row to `ERP_Platform.dbo.Sessions` and
sets the **`erp_sso`** cookie (HttpOnly, SameSite=Lax, Path=/). LIMS never shows its own
login: its `ErpSso` middleware validates the cookie against the central DB on every request,
maps the central user to a local account and signs it in. Signing out in either application
revokes the central session, so everything signs out together.

## 2. Central database — `ERP_Platform`

Script: `database/erp_platform.sql` (idempotent; creates + seeds). Tables:
`Users` (Username, Email, FullName, IsActive, PmsUserId, LimsUserId) · `Roles` · `UserRoles` ·
`Applications` (PMS, LIMS, HRMS-inactive; BaseUrl/RoutePrefix) · `Modules` · `Forms` (LIMS
navigation seeded: 4 modules, 18 forms) · `Actions` (VIEW CREATE EDIT DELETE APPROVE PRINT
EXPORT) · `RoleApplication` · `RoleFormPermission` · `Sessions` (Token, UserId, ExpiresAt,
RevokedAt, LastSeenAt, IP, UA, SourceApp) · `AuditLogs`.

Seed: roles `ERP_ADMIN` (all apps), `PMS_USER`, `LIMS_USER`, `HRMS_USER`; every PMS user
(by Username) and every LIMS user (by e-mail) became a central user; PMS `admin` is
`ERP_ADMIN`. A PMS user unknown to the centre is created on first login with `PMS_USER`.

## 3. PMS (.NET) side — `HRMS_Web`

| File | Role |
|---|---|
| `Services/ErpPlatform/ErpPlatformService.cs` | ADO.NET gateway: `CreateSession`, `Validate`, `Revoke`, `ApplicationsFor(Token)` |
| `Controllers/Login.cs` | after password check: central session + `erp_sso` cookie + `erp_apps` JSON in session; `SignOut` revokes + deletes the cookie |
| `Controllers/AppsController.cs` + `Views/Apps/Index.cshtml` | `/Apps` application selection tiles (monochrome, logo/name/description, hover, "Coming soon" for inactive); `/Apps/Go/{code}` |
| `Views/Shared/_ErpAppSwitcher.cshtml` + `wwwroot/css|js/erp-platform.*` | top-left brand is now the application switcher: My Home · ✓ PMS · LIMS · HRMS (soon) |
| `Extensions/LimsProxyMiddleware.cs` | `/lims/*` → `Erp:LimsUpstream` (streams body, forwards cookies/headers, adds `X-Forwarded-*`, rewrites internal `Location`s, 502 page when LIMS is down) |
| `appsettings.json` | `ConnectionStrings:ErpPlatform`; `Erp:{Enabled, CookieName, SessionHours, LimsUpstream, LimsPrefix}` |
| `Views/Login/Index.cshtml` | after login → `/Apps` (was `/Home/Index`) |
| `Program.cs` | `AddScoped<ErpPlatformService>()`; `app.UseLimsProxy(...)` right after the security-header middleware |

Run: `dotnet run --project HRMS_Web\HRMS_Web.csproj --urls http://localhost:5217` (LIMS: `php artisan serve --host=127.0.0.1 --port=8000`).

## 4. LIMS (Laravel) side

| File | Role |
|---|---|
| `config/erp.php`, `.env` (`ERP_ENABLED`, `ERP_BASE_URL`, `ERP_APP_CODE`, `DB_ERP_DATABASE`, `APP_URL=http://localhost:5217/lims`) | platform settings |
| `config/database.php` → `erp` connection | sqlsrv to `ERP_Platform` (same instance, Windows auth) |
| `app/Http/Middleware/ErpSso.php` (web group, after `StartSession`) | cookie → central session → application access check (403 if none) → local user by `LimsUserId`/e-mail, else provisioned (ERP admins get `is_admin=1` and every `*_list/add/edit/delete/print` column) → `Auth::login`; re-validated against the DB every 60 s; shares `erpApps` with views |
| `EncryptCookies` | `erp_sso` left unencrypted (set by .NET) |
| `Auth\AuthenticatedSessionController` | `GET /login` → redirect to the PMS login; `logout` → revoke central session, drop cookie, redirect to PMS `SignOut` |
| `AppServiceProvider` | `URL::forceRootUrl(APP_URL)` so every link/redirect carries `/lims`; `TrustProxies = *` |
| `layouts/main.blade.php` + `partials/app-switcher.blade.php` | brand → application switcher dropdown; `meta lm-home-url` points the breadcrumb "Home" at `/Apps` |

## 5. Verified (2026-08-22)

`POST /Login/LoginToPortal` (admin) → `erp_sso` set · `/Apps` 200 with PMS/LIMS/HRMS(soon) ·
`/lims/land_provider` 200 through the proxy with links on `/lims/...`, switcher present,
`admin` provisioned as LIMS admin (LimsUserId stored centrally) · `/lims/login` →
`/lims/dashboard` (no second login) · LIMS logout → PMS `SignOut` → both apps signed out ·
central `Sessions` revoked, `AuditLogs` LOGIN/LOGOUT rows.

## 5b. Milestone 1b — same credentials everywhere, no second login, instant switch (same day)

User review: selecting LIMS opened its login page. Cause: `/lims/` hit LIMS's `/` route, which
always rendered the Laravel login view even though the SSO had signed the user in. Fixed and
extended:

- **No LIMS login page at all** — `/` now sends a signed-in user to the dashboard and anyone else
  to the ERP login; `GET /login` already redirected there.
- **One credential set for every solution** — the PMS login (`Login.LoginToPortal`) now tries, in
  order: the PMS account → the **central credential** (`ERP_Platform.dbo.Users.PasswordHash/Key`,
  HMAC-SHA512, same scheme as PMS) → the **LIMS-native account** (LIMS verifies it over
  `POST /erp/verify` with the shared secret `Erp:SharedSecret` / `ERP_SHARED_SECRET`, throttled,
  CSRF-exempt, never returns a hash). A verified LIMS-native account is created centrally
  (`LIMS_USER`) and its credential stored, so from then on the centre answers for it; a PMS login
  syncs the PMS hash/key into the centre. A user with no PMS account gets the `erp_sso` cookie and
  `/Apps` but no PMS session (`Session["ID"]`), so PMS pages stay closed to them. Tested with the
  LIMS administrator (`admin@gmail.com`) — signs in at the single login and lands in LIMS.
- **Pre-authentication** — `/Apps` calls `GET /lims/erp/touch` (same-origin fetch with the cookie)
  as soon as it renders, so LIMS is already signed in before the tile is clicked.
- **Straight in** — a user with exactly one active application skips the selection and opens it
  (`/Apps?stay=1` shows the page anyway); `/Apps` and `/Apps/Go` authorise on the central session
  (cookie), not the PMS session.

## 6. Phase 2 (not done yet)

1. **Central endpoint authorisation** (brief §7): resolve each request to a
   `Form` + `Action` and check `RoleFormPermission` in the centre — a Laravel middleware
   keyed on route names, and a .NET filter keyed on controller/action; import PMS's
   `UserPermissionMapping` and LIMS's `users.*_list/add/…` columns into `RoleFormPermission`.
2. **Application-scoped navigation driven by `Modules`/`Forms`** instead of hard-coded menus.
3. **My Home workspace** inside PMS (`Home/Index`) gets the application cards + frequent items
   (today My Home links to `/Apps`, the dedicated selection page).
4. HRMS application (activate the row in `Applications`, add its proxy/prefix).
5. Production: a real reverse proxy (IIS/ARR or nginx) in front of both apps instead of the in-app
   proxy; HTTPS; `Secure` cookies.
