# PMS — Engineering Assessment

Findings from a full read of the solution on 2026-08-03. Ordered by severity.
Every item below was verified against the source, not inferred.

> **Closures as of 2026-08-16** — beyond A1/A2 below: the whole navigation defect family is
> gone with the registry swap (hard-coded 243 KB menu, label-as-permission-key renaming trap,
> wrong/dead/duplicate links, fake Reports subtree, 15 unreachable forms — resolved per the
> hidden-form verdicts); git is installed so the no-version-control process risk is closed.
> Still open and live: the committed credentials (rotation is yours), string-concatenated SQL
> in DynamicQuery (guarded, eliminated properly in the rebuild), and everything not listed
> here. Detail: `WORK-LOG.md` §7.

---

## P0 — Exploitable now

> **A1 and A2 closed 2026-08-14** (`#5`, `#6`): `[Authorize]` on `DynamicQueryController` plus a
> SELECT-only guard on `ExecuteParamQuery`; `[AllowAnonymous]` removed from
> `GenerateDynamicReport`. Both now require a signed-in user. A2's string-concatenation weakness
> remains for the rebuild to eliminate properly. Detail: `docs/WORK-LOG.md` §6.

### A1. Unauthenticated arbitrary SQL execution
`HRMS_Web/Controllers/api/DynamicQueryController.cs:80-107`

```csharp
[HttpPost("ExecuteParamQuery")]
public async Task<IActionResult> ExecuteParamQuery([FromBody] SqlRequest req) {
    cmd.CommandText = req.Sql;          // caller-supplied string, executed verbatim
    ...
}
```

The controller carries **no `[Authorize]`**. Any client that can reach the host can POST
`{"Sql":"..."}` and run any statement the `sa` account can run — read every member,
dealer and financial record, modify balances, `DROP` tables, or escalate to OS command
execution via `xp_cmdshell` (the connection string uses `sa`).

**Impact:** total compromise of `DHA_Live`. This is the single most urgent item in the
codebase.

### A2. `[AllowAnonymous]` SQL/SAP report execution with string-concatenated parameters
`HRMS_Web/Controllers/api/SapIntegrationController.cs:34-130`

```csharp
[AllowAnonymous]
[HttpPost] [Route("GenerateDynamicReport")]
public IActionResult GenerateDynamicReport([FromBody] QueryRequest request) {
    string rawQuery = queryTemplate.SqlQuery;
    foreach (var param in request.Parameters) {
        var safeValue = param.Value?.Replace("'", "''");        // only defence
        rawQuery = rawQuery.Replace("{" + param.Key + "}", $"'{safeValue}'");
    }
    ... cmd.CommandText = rawQuery; cmd.CommandTimeout = 0; ...   // or orecord.DoQuery(rawQuery) on SAP
}
```

Quote-doubling is not sufficient escaping: any template that interpolates a placeholder
outside a string literal (numeric comparison, `TOP {n}`, an identifier, a `LIKE` pattern)
is directly injectable, and the same payload is also forwarded to **SAP B1** via
`DoQuery`. `CommandTimeout = 0` means an injected heavy query runs forever — a
single-request denial of service.

### A3. Committed production secrets
`HRMS_Web/appsettings.json` (tracked; `.gitignore` has no rule for it)

- `Server=WIN-CM05CUDDJMV; Database=DHA_Live; User Id=sa; Password=s@dm24`
- `CloudinarySettings.APISecret = ItBpOSO9m7pFq_EP5IZxNfXNaGQ`
- `SmsApiSettings` UserId/Password for the Telecard gateway
- `AppSettings:Key = "This is my top secret, user you own secret"` — the JWT signing key,
  a guessable English sentence. Anyone who reads this file can forge a valid login token
  for any user id.
- `ResetJwtSettings:Key`, `TwoFactorJwtSettings:Key` — same exposure for the password-reset
  and 2FA flows.
- `SapIntegrationController.cs:25`: `private const string SAP_SECURITY_KEY = "s3cR#T-…"` —
  a secret compiled into the binary.

These are in git history, so rotating the files is not enough; **the credentials themselves
must be rotated**.

### A4. Application runs as SQL `sa`
The connection string authenticates as `sa`. Every injection, every logic bug, and every
compromised session inherits full sysadmin rights over the instance.

---

## P1 — Systemic security and correctness gaps

### B1. Authorization is authentication-only
`[Authorize]` is used bare on ~96 of 152 controllers. There are no roles, no policies, no
resource-based checks. The JWT carries only `Name` and `NameIdentifier`.

Per-form rights (`UserPermissionMapping.CanAdd/CanEdit/CanDelete/CanView`) are loaded into
session at login and rendered into Razor/JS — **enforcement is client-side only**. A
logged-in clerk can call any endpoint of any module directly, including approvals,
transfers, and charge setup. There is also no object-level check: nothing verifies that
the caller is entitled to the specific `RegistrationNo`/`MemberProfile` being acted on.

### B2. ~56 controllers with no `[Authorize]` at all
Includes `SPController` (46 report endpoints across every domain) and
`DynamicQueryController`. Reachable anonymously.

### B3. Two auth systems that don't agree
Session-based auth (MVC views) and JWT (API) are independent. Signing out clears session
keys but the 12-hour JWT stays valid. There is no refresh, no revocation, no `jti`, and
no server-side token store.

### B4. Raw exception text returned to clients
The universal `catch (Exception ex) { response.message = ex.Message; }` pattern (804
occurrences) leaks SQL error text, table and column names, and stack context to any
caller.

### B5. Every response is HTTP 200
`Response_Result.code` carries the outcome. Failures, conflicts, and unhandled exceptions
all return `200 OK`. Consequences: proxies and monitors see a healthy service, clients
must parse the body to detect failure, and nothing surfaces in infrastructure metrics.

### B6. No logging
`ILogger` appears 4 times in ~58k lines. Swallowed exceptions are not recorded anywhere.
When something fails in production there is no trail — no request id, no user id, no
stack trace.

---

### B7. The permission key is the menu label

`_Layout.cshtml` gates every menu item with `Html.UserHavePermission("Transfer & Records")` and
`Permissions.FormName` is a plain `string`. There is no identifier behind the label.

Consequences, all live today:

- Renaming a menu item **silently revokes access** for every role that had it.
- Two menu items with the same label share one permission, whether or not that was intended.
- Menu visibility is the *only* enforcement — the endpoint behind a hidden item still answers,
  because per-form rights are checked client-side (B1).
- 15 working forms have no menu entry at all, so they have no permission gate and no way in;
  their endpoints are still reachable directly.

Navigation itself is also measurably broken: a menu item pointing at the wrong action, two
pointing at an action that does not exist, a fake reports subtree built by copy-paste, 22
duplicated links, and a form named `ChargesGroupFormTest` wired into the live menu. Sixteen
defects catalogued in `05-MODULE-ARCHITECTURE.md` §1.3.

**Fix.** A module/form registry with a stable opaque `PermissionKey` (`property.block`, never
`"Block Definition"`), from which the menu, the permission catalogue and the API authorization
policies are all derived. Titles then change freely; access does not move.

---

## P2 — Data integrity and correctness

### C1. No transactions across multi-step business operations
`SaveChanges()` is called 667 times, frequently **inside `foreach` loops**
(`B_Utility/BLL/ApprovalBLL.cs:34-40, 55-71, 84-92` — one round-trip per approval user).
Multi-entity operations (a transfer touching stock + registration + charges + approval)
are not wrapped in a transaction. A mid-sequence failure leaves partially-applied state
with no rollback and no compensating action.

### C2. `DateTime.Now` everywhere (739 uses)
Server-local time, no time zone, no UTC. Audit timestamps, grace-period and surcharge
calculations, and bill dates are all tied to the machine's clock and DST. Moving the app
to a different host or to Azure silently shifts financial dates.

### C3. Async in name only
667 `SaveChanges()` vs 11 `SaveChangesAsync()`. Methods are declared `async Task<T>` but
contain no `await`, so they run synchronously while consuming a thread. Under load, the
thread pool starves. Combined with `CommandTimeout(180)` globally and `CommandTimeout = 0`
in `GenerateDynamicReport`, one slow query blocks a request thread for minutes.

### C4. Nullable reference types enabled but not honoured
`<Nullable>enable</Nullable>` on every project, 326 build warnings, most of them `CS8618`.
Meanwhile code does `_db.Blocks.Where(...).FirstOrDefault()` and immediately dereferences
the result (`BlockController.cs:135-136`) — a `NullReferenceException` on any bad id,
returned to the client as a generic exception message.

### C5. Silent no-op on concurrent edits
Update paths reload the entity and copy fields one by one. There is no concurrency token,
no `RowVersion`. Two users editing the same record: last write wins, silently.

### C6. Deleted records still visible
Soft delete is by convention (`is_deleted`/`IsDeleted` flags) with no global query filter.
Every query must remember `.Where(x => !x.is_deleted)` by hand. Several read paths filter
only on `is_active`, so the two flags can disagree.

### C7. Two incompatible base-entity conventions
`B_DB_Model/BaseModel.cs` defines `Id / IsActive / IsDeleted / CreatedOn / CreatedBy /
LastModified / ModifiedBy`. But many entities (`Block`, `StockCreation`, …) use
`ID / is_active / is_deleted / Created_at / Created_By / Updated_at / Updated_By`. Both
conventions are live in the same DbContext, so no cross-cutting behaviour (auditing,
soft-delete filters, optimistic concurrency) can be applied uniformly.

---

## P3 — Architecture and maintainability

### D1. No layering — controllers are the application
`DataBase_Context` is injected directly into every controller. Business rules, validation,
persistence, workflow transitions, and response shaping all live in the action method.
The `Services/` folder covers only Dealer/DealerCategory/DealerProfile/Features; the
generic `Repository<T>` in `B_DB_Context/Repository/` is essentially unused.

Result: files of 275 KB (`SapIntegrationController`), 175 KB (`SPController`), 169 KB
(`FilterController`), 98 KB (`ApprovalsController`). These cannot be reviewed, tested, or
safely modified.

### D2. Zero tests
No test project exists. A property transfer touches stock status, registration profile,
charge generation, approval stages, SAP posting and file movement — and there is no
executable check that any of it is correct. This is the main reason the codebase is
frightening to change.

### D3. Build is machine-locked; no CI
`COMReference` to `SAPbobsCOM` forces full-framework MSBuild and requires SAP B1 installed
locally. `dotnet build` fails with MSB4803. `.github/` is empty. There is no automated
build, no artifact, no repeatable deployment.

### D4. .NET 6 is out of support
End of life was 2024-11-12. No security patches. The only SDK installed here is .NET 10.
EF Core 6, `Microsoft.AspNetCore.Http 2.2.2` (a 2019 package, referenced in a class
library), `iTextSharp 5.5.13.3` (AGPL, unmaintained since 2016), and
`Microsoft.AspNet.WebApi.Core 5.2.9` (full-framework Web API pulled into a Core project)
are all obsolete or misplaced.

### D5. 316 migrations, unsquashed
Names like `possession_field_added`, `stock_table_update`. `Migrations/` is ~2.2 M
generated lines and dominates the repository. Applying from scratch takes minutes;
reasoning about schema history is impractical.

### D5b. The migrations do not reproduce the model

Verified 2026-08-04 by building a database from scratch: `dotnet ef database update` applied all
316 migrations successfully and produced **439 tables** — but the result does not match the
entity model.

`LastModifiedUserName` is declared on `BaseModel`, so every entity has it. **235 of the 439
tables were created without that column.** The application crashes on the first query that
touches one: signing in fails with `SqlException 207 — Invalid column name
'LastModifiedUserName'`, thrown from `AlertService.GetNDC()`.

The live database presumably has these columns, added by hand outside the migration history.
That means the migration history is not a reproducible description of the schema, and no clean
environment can be built from it.

Consequences:

- No developer, and no CI job, can create a working database from the repository.
- The migration squash cannot be verified against migration output alone — a schema diff has to
  run against the **live** database, not against a rebuilt one.
- Any column added by hand in production is invisible to the model and will be silently dropped
  by a squash that trusts the migrations.

**Fix.** Diff the live schema against both the model and the migration output before `#46`
touches anything. Whatever the diff finds becomes a corrective migration, and only then is the
history squashed. Tracked as `#136`.

### D5c. The project cannot be built by `dotnet build`

`HRMS_Web.csproj` carried two `<COMReference>` items for the SAP DI API. `dotnet build` fails on
them with `MSB4803 — the task "ResolveComReference" is not supported on the .NET Core version of
MSBuild`, before compiling a single file. Only the .NET Framework MSBuild shipped with Visual
Studio can resolve them, and only on a machine where the SAP client is installed and its type
libraries registered.

So the build was never merely "machine-locked" — it was locked to one *toolchain* as well, and
no CI runner could have built it under any configuration.

Mitigated 2026-08-04 by a `SapIntegration` build property, default off (`#134`). Replaced
properly by `#35`.

### D6. Views are unmaintainable
`_Layout.cshtml` is 249 KB / 2,909 lines with 35 inline `<script>` blocks. Feature views
run 100–200 KB each (`Approval/Inbox.cshtml` 201 KB, `Operations/Transfer.cshtml` 183 KB)
with business logic in inline JavaScript. There is no bundling, no minification pipeline,
no module system, no component reuse.

### D7. Dead and hostile static content
`wwwroot/functions/` contains a full PHP mailer stack (`class.phpmailer.php`,
`class.smtp.php`), Twitter OAuth libraries (`tmhOAuth.php`), cached tweet JSON, and
`login-form.php` / `register-form.php` — leftovers from the purchased "Venmond" HTML
template. Also present: `bootstrap-old.min.js`, `excanvas.js` (IE6 canvas shim),
`respond.min.js`, `html5shiv.js`, and IE7/IE8 conditional comments in `_Layout`.

### D8. Seven copy-pasted uploader console apps
`NewStockUploader`, `UpdateStockUploader`, `DeleteStockUploader`, `NewMemberUploader`,
`UpdateMemberProfile`, plus `MemberUploader` and `StockDataUploader` which exist on disk
but are **not in `HRMS.sln`** (dead or orphaned). Each is ~230 lines of the same
CSV→EF loop with a hard-coded path (`C:\DataUploader\StockDataUploader.csv`), its own
`AppDbContext`, `Console.ReadKey()` at the end (so it cannot run unattended), and
`catch { Console.WriteLine(ex.Message); }` with **exit code 0 on failure** — a scheduled
task would report success after importing nothing.

`AutoTriggerService` is an empty 2-line stub still carried in the tree.

### D9. Repository hygiene
`UrbanDev.rar` — 103 MB — is committed at the repo root (`.gitignore` has no `*.rar`
rule), and a 118.7 MB pack file dominates `.git/`. `HRMS_Web.csproj.user` is present.
`HRMS.sln` is misnamed (HRMS = HR system; this is a PMS) and omits two projects that
exist on disk.

### D10. Naming and spelling defects baked into the API surface
`ResponseCode.succcess`, `TransferHistery`, `ApprovalHistery`, `SurrenderHistery`,
`COPHistery`, `Inovice`, `GenralAdjustment`, `LeadGenration`, `Demarcation`/`Demarkation`
mixed, `Clearnce`, `ConstracutionStatus`, `GrancePeriodForBillGenration`,
`PropertyBindingControll`, `WithHoldingTax .cs` (trailing space in filename),
`MyBackgroundService .cs` (trailing space), `TestApproval` (a production table).
These are in URLs, JSON payloads and column names, so fixing them is a breaking change
that needs planning, not a rename.

---

## What is actually good here

Worth stating plainly, because it shapes the plan:

- **The domain model is real and complete.** 140 entities covering the full property
  lifecycle, including hard parts most systems skip: amalgamation, re-numbering,
  repurchase, soft-locks/caution, joint members, historical transfer data.
- **The generic approval engine** (`ApprovalSetup` → stages → users → `TestApproval` →
  `ApprovalHistery`) is a genuinely good design. Configurable multi-stage approval with
  per-stage quorum (`NumberOfApprovalRequired`) applied uniformly across ~30 request types
  is more than most systems of this size have.
- **Report SPs are correctly parameterized** (`SPController`, `FilterController` use
  `SqlParameter`) — the SQL injection is confined to the two dynamic-query paths.
- **Passwords are properly hashed** — HMACSHA512 with a per-user salt key, and the
  comparison is done byte-by-byte.
- **The SAP B1 integration works**, which is usually the hardest thing to get right.

The value in this codebase is the domain knowledge and the workflow engine. The problems
are structural — layering, security, testing, build — and structural problems are fixable
without throwing away the domain work.
