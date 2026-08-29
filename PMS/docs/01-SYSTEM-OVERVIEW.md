# PMS — System Overview (Current State)

Snapshot taken 2026-08-03 on branch `Dhafeature/dev`. This document describes what the
system **is today**, not what it should become. See `02-ASSESSMENT.md` for defects and
`03-REENGINEERING-PLAN.md` for the forward plan.

> **Shell superseded 2026-08-16** — this snapshot predates the restructuring build. Since
> then: navigation renders from the `NavigationNodes` registry (the hard-coded `_Layout` menu
> is gone), the landing page is the My Home workspace, every form carries a registry-driven
> title header, the top bar holds global search / Recent / Favourites / Generate Alert, git is
> live, and branding is N-Stack with a Property Management System vector mark. The domain
> description below is unchanged and still accurate. Running record: `WORK-LOG.md` §7.

## 1. What this system does

A **Property Management System** for a N-Stack-style housing authority (`Database=DHA_Live`,
branding "N-Stack", SMS mask `DHAB`). It is the system of record for the full lifecycle of a
plot/property and its owner, integrated with **SAP Business One** for finance.

Business domains present in the code, grouped:

| Domain | Representative modules |
|---|---|
| **Inventory / Stock** | `StockCreation`, `StockCreationSetup`, `PropertyList`, `PropertyNo`, `RegistrationNo`, `RegistrationNoProfile`, `Renumber`, `SitePlan`, `LDAPlotNo` |
| **Parties** | `MemberProfile`, `Dealer`, `DealerProfile`, `DealerCategory`, `DealerDesignation`, `LawyerData`, `PMSUser` |
| **Sales pipeline** | `LeadGenration`, `PreSale`, `Booking`, `Deal`, `BulkDeal`, `AdvanceApplication`, `Promotion`, `Quota` |
| **Transfers & ownership** | `TransferHistery`, `TransferType`, `TransferReceiptProcessing`, `TransferSetReceiving`, `Amalgamation`, `Surrender`, `RePurchase`, `DeAllocation`, `COPHistery` |
| **Clearances / NDC** | `NDC1`, `NDCRequestForMember`, `NDCRequestForDealer`, `NDCRequestType`, `Clearance`, `Demarcation`, `DemarcationRequest`, `MapApproval`, `MapDesign` |
| **Construction** | `ConstructionMonitoring`, `ConstructionSecurity`, `ConstructionStage`, `MeterialTesting`, `Finishes` |
| **Utilities & metering** | `MeterInstallation`, `MeterReading`, `MeterType`, `MeterPhase`, `MeterPhaseWiseRate`, `MeterStatus`, `MeterBillGeneration`, `ReadingOfficer` |
| **Billing & charges** | `GlobalChargeSetup`, `GlobalChargeGroup`, `ChargeGroupType`, `PropertyFixedChargesSetup`, `FixedChargeBill`, `IndividualBill`, `DemandNote`, `SurchargeSetup`, `GracePeriodSetup`, `SaleTax`, `WithHoldingTax`, `GenralAdjustment`, `Inovice`, `VoucherSeries` |
| **Files & documents** | `ClientFileVerification`, `FileVerificationRequest`, `FileVerificationNDC1`, `FileDocDupRequest`, `FileReceivingRegister`, `FileLocationAssigment`, `StoreRoomFileMoving`, `PossessionAttachment` |
| **Legal / cases** | `CaseProfile`, `CaseType`, `CaseCategory`, `ViolationGroup`, `ViolationGroupType`, `SoftLockName` (caution/soft-lock on properties) |
| **Workflow engine** | `ApprovalSetup`, `ApprovalUI`, `ApprovalHistery`, `TestApproval`, `ApprovalUsers` — a generic multi-stage approval engine driving nearly every request type |
| **Platform** | `PMSUser`, `RolesPermissions`, `PermissionForms`, `UserPermissionMapping`, `Notification`, `AlertName`, `FormAlert`, `Forum`, `Calendar`/`WeekSchedule`, `DynamicQuery` (user-defined SQL reports), `MemberBioMetric` (fingerprint) |
| **SAP B1 bridge** | `SAPOperations`, `SAPBilling`, `SAPBillPostingCheck`, `GLDetermination` + COM interop via `SAPbobsCOM` |

## 2. Solution layout

`HRMS.sln` (name is a leftover from an HR product the codebase was forked from).

```
HRMS_Web/            ASP.NET Core 6 MVC + Web API — the entire application
  Controllers/         31 MVC controllers (return Razor Views)
  Controllers/api/    121 API controllers (return JSON)
  Views/             277 .cshtml across 32 feature folders + 65 partials
  wwwroot/           static assets: 415 .js (~7.2 MB), plugins/, img/, css/
  Models/DTOs/        71 DTO classes
  Services/           9 injected services (SMS, Photo/Cloudinary, Alert, Uploader,
                      Notification, + 4 "BusinessServices" for Dealer/Feature only)
  Extensions/         session helpers, encryption, file storage, SAP connection,
                      a background service, ValidateSessionAttribute
  Program.cs          DI + 3 JWT schemes + session + static files
B_DB_Model/          140 EF Core entity classes (~5.2k lines)
B_DB_Context/        DataBase_Context.cs (41 KB, ~220 DbSets) + 316 migrations
                     + a barely-used generic Repository<T>
B_Utility/           ApprovalBLL.cs (24 KB — the approval engine), CommonBLL.cs,
                     UHelper (JWT, formatting), Response_Result, enums
AutoTriggerService/  empty stub (Program.cs is 2 lines)
DataSyncer/ (solution folder)
  NewStockUploader/ UpdateStockUploader/ DeleteStockUploader/
  NewMemberUploader/ UpdateMemberProfile/
  + MemberUploader/ StockDataUploader/  (on disk, NOT in the .sln)
                     7 near-identical CSV→SQL console apps, ~230 lines each,
                     hard-coded paths like C:\DataUploader\StockDataUploader.csv
```

Root also contains `UrbanDev.rar` (103 MB) — an archive committed into the repository.

## 3. Runtime architecture

```
Browser (jQuery + DataTables + Select2 + Bootstrap 3-era theme "Venmond")
   │  form posts  ─────────────► MVC Controllers ──► Views (.cshtml, up to 250 KB each)
   │  $.ajax JSON ─────────────► api/ Controllers ──► DataBase_Context (EF Core 6)
                                        │                    │
                                        │                    ├─► SQL Server (DHA_Live)
                                        │                    └─► ~50 stored procedures
                                        │                        (paged reports returning
                                        │                         a single JSON column,
                                        │                         deserialized in C#)
                                        ├─► SAPbobsCOM (COM interop) ──► SAP Business One
                                        ├─► Cloudinary (photos)
                                        ├─► Telecard SMS gateway
                                        └─► FirebaseAdmin (push)
```

### Authentication — two parallel, unreconciled mechanisms

1. **Server-side session** (`Login.LoginToPortal`): validates HMACSHA512 password hash,
   then stuffs `ID`, `EMP_CODE`, `desig`, `departm`, `managerId`, `FullName`, and a
   serialized permission list into `HttpContext.Session` (9-hour idle timeout).
   MVC views read permissions from session. Enforced by `ValidateSessionAttribute`,
   which is applied in **one** place.
2. **JWT bearer** (`UHelper.CreateJWT`): 12-hour token signed HS256 with
   `AppSettings:Key`, carrying only name + user id — **no roles, no permissions**.
   `Program.cs` registers three schemes: `LoginScheme`, `ResetScheme` (password reset),
   `TwoFactorScheme` (2FA). Default scheme is `LoginScheme`.

`[Authorize]` appears on ~96 of 152 controllers, and only ever bare — authorization is
authentication-only. Per-form CRUD rights live in `UserPermissionMapping`
(`CanAdd/CanEdit/CanDelete/CanView`) but are enforced **client-side in Razor/JS**, not by
any server-side policy or handler.

### Navigation and the permission catalogue are the same string

The entire application menu is hard-coded in `Views/Shared/_Layout.cshtml` — 22 top-level
groups, nested up to four levels, 200 leaf links to 178 distinct targets. Each is wrapped in
`Html.UserHavePermission("<menu label>")`.

`Permissions.FormName` is a `string`, and `PermissionForms` is a flat table — `Name`, `Title`,
`IsActive`, `SerialNo` — with no parent, no module and no hierarchy. So:

- **The menu label is the permission key.** Renaming a menu item revokes access for every role.
- **There is no module concept in the data model.** Modules exist only as `<li>` nesting in Razor.
- The menu, the permission catalogue and the API each hold a separate idea of what a form is,
  and nothing keeps them in agreement.

Full measurement, defect list and the target structure: `05-MODULE-ARCHITECTURE.md`.

### The dominant controller pattern

Every CRUD API controller repeats this shape (see `Controllers/api/BlockController.cs`):

```csharp
[Route("api/[controller]/[Action]")] [ApiController] [Authorize]
public class XController : ControllerBase {
    private readonly DataBase_Context _db;          // DbContext injected straight in
    [HttpPost] public async Task<Response_Result> AddX(X x) {
        var r = new Response_Result();
        try {
            // exists-check, then branch on id==0 for insert vs update
            // manual field-by-field copy for update
            _db.SaveChanges();                      // sync inside async, no await
            r.code = (int)ResponseCode.succcess;    // [sic]
        } catch (Exception ex) {
            r.code = (int)ResponseCode.exception;
            r.message = ex.Message;                 // raw exception text to client
        }
        return r;                                    // always HTTP 200
    }
}
```

`Response_Result { int code; string message; object data; object secondData; string token; }`
is the universal envelope. HTTP status codes are not used to signal outcome.

### Reporting

Two mechanisms:

- **Stored-procedure reports** (`SPController`, `FilterController`): DataTables server-side
  pagination reads `Request.Form["draw"/"start"/"length"/…]`, calls an SP via
  `FromSqlRaw` with `SqlParameter`s (correctly parameterized), gets back one JSON string
  column, `JsonConvert.DeserializeObject`s it, and separately re-runs a LINQ `Count()`
  for the total.
- **DynamicQuery**: admins store raw SQL templates in the `DynamicQueries` table with
  `{placeholder}` params; `SapIntegrationController.GenerateDynamicReport` substitutes
  params by string replace and executes against SQL Server or SAP.

### SAP Business One integration

`HRMS_Web.csproj` declares `<COMReference Include="SAPbobsCOM">` / `SAPbouiCOM` (v10),
consumed by `Extensions/SAPConnection.cs`, `SAPOperationDb.cs`, `SAPBillingDb.cs` and
`Controllers/api/SapIntegrationController.cs` (275 KB — the largest file in the repo).
Credentials and connection settings live in the `SAPOperations` / `SAPBilling` tables.

## 4. Scale

| Metric | Value |
|---|---|
| C# source (excl. migrations) | ~58,000 lines |
| EF migrations | 316 (2022-11-16 → 2026-07-27), ~2.2 M generated lines |
| Entities / DbSets | 140 / ~220 |
| Controllers | 152 (31 MVC + 121 API) |
| Razor views | 277 (largest: `_Layout.cshtml` 249 KB / 2,909 lines, 35 `<script>` blocks) |
| Front-end JS | 415 files, 7.2 MB, all vendored (jQuery, Select2, DataTables, Bootstrap) |
| Automated tests | **0** |
| CI pipelines | **0** (`.github/` is empty) |
| `try/catch` blocks | 804 |
| `SaveChanges()` vs `SaveChangesAsync()` | 667 vs 11 |
| `DateTime.Now` occurrences | 739 |
| `ILogger` occurrences | 4 |

## 5. Build reality

- `dotnet build HRMS.sln` **fails**: `MSB4803 — ResolveComReference is not supported on
  the .NET Core version of MSBuild`. The `COMReference` items force full-framework MSBuild.
- Building `HRMS_Web.csproj` with VS MSBuild on this machine also **fails**: 40+
  `CS0246: SAPbobsCOM could not be found` — the SAP B1 DI API is not installed/registered here.
- Everything else (`B_DB_Model`, `B_DB_Context`, `B_Utility`, all uploaders) builds clean:
  0 errors, 326 warnings (mostly `CS8618` nullable).

**Consequence:** the application can only be compiled on a workstation with the SAP
Business One client installed, using Visual Studio's MSBuild. No CI is possible in the
current shape.

## 6. Configuration

`HRMS_Web/appsettings.json` is committed and contains live-looking secrets: SQL Server
`sa` credentials, Cloudinary API secret, Telecard SMS account, and all three JWT signing
keys. `.gitignore` does not exclude it. Attachment storage is a hard-coded local path
(`C:\PMSTestAttachmentFiles\Attachments`).
