# Payroll ERP (Payroll_HCC2)

ASP.NET MVC 5 payroll application — the production line, re-branded **Payroll ERP** (platform mark: **N-Stack**).
The strategic successor is the **HCM** solution in `..\HCM` (see `..\HCM\PROJECT.md`).

## Stack
- .NET Framework **4.8**, ASP.NET MVC 5.2.3, Razor views
- 3 projects: `Payroll-HCC` (PresentationLayer) → `BusinessLayer` → `DataLayer` (models)
- UI: Bootstrap 3 + AdminLTE layout mechanics + Material inputs, fully re-skinned by `Content\theme.css`
  (monochrome charcoal/gray/white design system, Inter font, inline SVG icons from `Infrastructure\Icons.cs`)
- SQL Server `.\MSSQLSERVER01`, Windows auth
  - `Payroll_HCC` — admin DB: `Account`, `Company`, **`Role`, `RolePermission`, `ActivityLog`, `ApprovalRequest`, `ApprovalHistory`**
  - `Payroll_Company<id>` — one DB per company (legacy design)

## Build / run
```powershell
& "D:\Programs\VS Applications\MSBuild\Current\Bin\MSBuild.exe" Payroll-HCC.sln /p:Configuration=Debug
..\Start-PayrollHCC.ps1        # idempotent start; app at http://localhost:7637/
```
Sign in: `admin` (change it under *user menu → Change password*).

## Application structure
`Infrastructure\FormRegistry.cs` is the single source of truth for navigation, permissions, search and
breadcrumbs: **module → form (key, title, controller/action, icon)**. Add a form there and it appears in the
sidebar, home launcher, permission matrix and global search.

Modules: Employee Management · Compensation & Payroll Setup · Leave & Benefits Setup · Time & Attendance ·
Payroll Transactions · Overtime Management · Leave Management · Loan & Advances · Employee Reports ·
Security & Administration (Users, Roles & Permissions, Approvals, Activity Log).

Controllers keep their legacy names/routes (`Master`, `Transaction`, `Reports`) so existing JS and links work;
only the presentation is regrouped.

## Security model
- Passwords PBKDF2 (`BusinessLayer\PasswordHasher.cs`); policy: ≥8 chars, letters + digits; forced change at first sign-in / after reset.
- Session user (`DataLayer.SessionUser`) holds the role's resolved permissions; `[AdminAuthorize]` (all business controllers)
  redirects anonymous users, enforces **View** on every registered form for GET requests (URL typing gives *Access denied*),
  routes `MustChangePassword` users to the change screen, and sends `Cache-Control: no-store` so Back after sign-out shows nothing.
- `[RequirePermission(formKey, action)]` guards POST handlers (Create/Edit/Delete/Approve) server-side; AJAX gets 401/403.
- Roles: `Administrator` (system, full access) plus editable roles. Permissions per form: View, Create, Edit, Delete, Approve, Export, Print.
- Sign-out is a POST with anti-forgery token; session abandoned and cookie expired.
- Every significant action is written to `ActivityLog` (`Infrastructure\App.Log`). Header "Recent activity" and the home page read it.

## Approval framework (`BusinessLayer\Approvals.cs`)
Generic queue: `Submit(type, referenceKey, title, detail, requestedBy)` → Pending → **Approved / Rejected / Returned**
(+ Resubmit), with `ApprovalHistory`. Side effects per request type live in `SecurityController.ApplyDecision` and run
only when the whole process is complete (`ApprovalDecisionResult.Final`).
**Approval Setup** (`Security.ApprovalSetup`, form under Security & Administration, 2026-08-28): every request type is an
*approval process* row in `ApprovalProcess` with a switch (`IsEnabled` — off = the action applies at once, nothing queued;
callers ask `RequiresApproval(type)`) and ordered **stages** (`ApprovalStage` / `ApprovalStageUser`): name, approver role
(or *Any approver* = anyone holding Approve on Approvals), optional named users, and the number of approvals needed.
`CanDecide(request, user)` enforces the stage rule in the queue; `Decide` counts approvals per stage, advances
`ApprovalRequest.CurrentStage`, and finishes on the last stage. Rejected/Returned end the request at any stage; a
resubmitted request restarts at stage 1. Seeded processes: `UserAccount`, `PayrollRun` (one stage, any approver).
**Form gate — "select the form that needs approval"** (Approval Setup → *Add form*): any registered form can be made a
process (`ApprovalProcess.FormKey` + `Actions` = Create/Edit/Delete). `RequirePermissionAttribute` (on every write
handler) then *holds* the write for users without the Approve right on that form: the request (URL, content type,
JSON body or form fields) is captured into `ApprovalRequest.Payload`, the caller gets `{approvalPending:true}` (the
shell shows it as a toast) and nothing is written. On final approval the approver's browser replays the captured
request with the `X-Approval-Replay: <id>` header; the filter validates it (approved, same form, not yet applied,
caller is an approver), runs the action and marks the request applied (`AppliedAt/By`, history "Applied"). An
approved-but-unapplied request keeps an **Apply change** button in the queue. Uploads cannot be held (refused with a
message); Home and Security screens cannot be gated.
Add a new built-in type by calling `Submit` (guarded by `RequiresApproval`), seeding its `ApprovalProcess` row in
`SchemaUpgrade`, and extending `ApplyDecision`.

## Schema changes
Applied automatically at start-up (`BusinessLayer\SchemaUpgrade.cs`, idempotent, additive only) and also
provided as `..\Payroll scripts\security_upgrade.sql`. Nothing is dropped; the pre-existing `admin` account became Administrator.

## ERP suite integration (Enterprise Solution)
Payroll Management is an application of the **Real Estate Management Solution** (`..\..\Enterprise Solution\PMS`):
- **Hosting**: IIS Express serves this site under the virtual path **`/payroll`** (`Payroll_HCC2\iisexpress\applicationhost.config`,
  launched by `..\Start-PayrollHCC.ps1`); the PMS host reverse-proxies `/payroll` → `http://localhost:7637/payroll`
  (`HRMS_Web\Extensions\PayrollProxyMiddleware.cs`, settings `Erp:PayrollUpstream` / `Erp:PayrollPrefix`), so the whole
  suite lives on one origin and one cookie jar. Entry point for users: `http://localhost:5217/Apps` → Payroll tile.
- **Registry**: `ERP_Platform.dbo.Applications` row `PAYROLL` + role `PAYROLL_USER` (`..\Payroll scripts\erp_register_payroll.sql`;
  seed also updated in `LMIS\database\erp_platform.sql`). ERP_ADMIN has every app.
- **Single sign-on** (`Infrastructure\ErpSso.cs`, `Filters\AdminAuthorizeAttribute`): the `erp_sso` cookie issued by the central
  login is validated against `dbo.Sessions`; the user must hold the PAYROLL application; the local `Account` is matched by
  username (auto-provisioned on first visit: ERP administrators → Administrator, others → Viewer, then raised locally under
  User Management). Central session is re-validated every `Erp:RevalidateSeconds`. No central session → redirect to
  `Erp:BaseUrl/Login/Index`; sign-out revokes the central session (global). `?local=1` on the login page still allows a
  Payroll-native account (fallback). Endpoints: `GET /erp/touch` (prewarm from the launcher), `POST /erp/verify` (shared secret).
- **Home**: the right column starts with an **Apps** card (same tiles as PMS / LIMS My Home: marks, Current / Open /
  Coming soon, "All applications" → host `/Apps`); the navigation ends with a "Collapsed View" button (PMS / LIMS glyph).
- **Header**: the top-left brand is the ERP application switcher (`Views\Shared\_ErpAppSwitcher.cshtml`, `Content\erp-platform.css/js`
  copied from PMS) — first item "Applications Library", then PMS / LIMS / Payroll. Wordmark "Payroll / MANAGEMENT";
  tab title "Payroll Management".
- **Brand mark** (2026-08-28): the owner's "group of people" icon (`..\brand\payroll-mark-source.svg`) as solid silhouettes,
  generated by `..\brand\payroll-mark-generator.py` into `Content\brand\` — `payroll-mark.svg` (ink, for white grounds),
  `payroll-mark-white.svg` (for charcoal grounds), `favicon.svg` + PNG 16–512 + `favicon.ico` (a single white avatar on a charcoal
  `#242729` tile — the three-figure mark blurs at 16 px; also copied to the site root; `<link rel=icon>` carries `?v=`). `_BrandMark.cshtml` and the header lockup draw the same geometry inline;
  PMS and LIMS reference these files for their switcher rows and app tiles. Change the mark by editing the generator.
- Web.config keys: `Erp:Enabled, Erp:BaseUrl, Erp:AppCode, Erp:CookieName, Erp:SharedSecret, Erp:RevalidateSeconds`;
  connection string `ErpPlatform`.

## Known limitations / next candidates
1. Company switch rewrites Web.config → app-domain recycle → all users signed out (rare op).
2. Per-company database design — HCM replaces this with CompanyId scoping.
3. Legacy form views keep their original jQuery/DataTables code; they are restyled by the design system, not rewritten.
4. Approval routing is per process (stages × approvers × required count) but not per amount/department; there is no delegation or escalation.
5. No automated tests; `compilation debug="true"` — switch to Release for production.
6. No source control by owner's decision (2026-08-25).
