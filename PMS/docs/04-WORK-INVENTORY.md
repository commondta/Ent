# PMS — Work Inventory

Every screen, controller and process in the system, grouped into the 16 modules we will work
through **one at a time**. This is the pick-list: we choose one row, I take it through the full
gate (understand → document → feasibility → task breakdown → your review → build), and only then
move to the next.

Counted 2026-08-03 from the repository: **277 views**, **152 controllers**, **140 entities**.
Sizes are the current file sizes — they are the honest signal of where the complexity is hiding.

---

## How to read this

- **Screens** — the `.cshtml` forms a user actually opens. This is the unit we work in.
- **Controllers** — the code behind them, with size. Anything over 20 KB is holding business rules
  that exist nowhere else.
- **Processes** — what actually *happens*, as opposed to what gets stored. These are the things
  that need worked examples and tests, and the things I cannot infer with certainty from code alone.
- **Read** — my honest assessment of difficulty and risk for that module.

---

## Two groupings, and why they differ

This file groups work into **16 modules for rebuild sequencing** — ordered by dependency and
risk, so each one lands after the things it needs. It answers *what do I build next*.

`05-MODULE-ARCHITECTURE.md` groups the same forms into **12 modules for the user interface** —
ordered by how people work. It answers *where does a user find this*.

They are deliberately not the same list. Members and dealers are two rebuild modules (M05 splits
by risk) but one thing to a user (Parties). SAP and the import jobs are two rebuild modules but
live under Administration. Master data is one rebuild module (M01, ~40 near-identical screens,
one pattern) but its forms are distributed to the module that owns them in the shell — Block
under Property, Charges Type under Billing, Case Type under Litigation.

| | This file | `05-MODULE-ARCHITECTURE.md` |
|:--|:--|:--|
| Unit | 16 rebuild modules | 12 shell modules |
| Ordered by | Dependency and risk | How a user works |
| Answers | What is built next | Where a form lives |
| Drives | `PROJECT.md` §5 module queue | The menu, permissions and routes |

A form appears in exactly one of each. Where they disagree about a name, the shell name is what
users see and this file's name is what the plan calls it.

---

## M01 — Master data and setup

The reference tables everything else points at.

**Screens (≈40).** Almt, Block, Category, Features, Finishes, Floor, Force, LDA Plot No, Member
Category, Nature, Phase, Postfix, Prefix, Project, Property Type, Quota, Rank, Real Estate, Sector,
Social Status, Unit of Measure, Verification Type, Department, Tax Type, Transfer Type, NDC Request
Type, Dealer Category, Dealer Designation, Payment Plan Type, Case Category, Case Type, Forum Setup,
Meter Phase, Meter Status, Meter Type, Construction Stage, Violation Group, Violation Type, Charges
Type, Alert Name, Soft Lock Name.

**Controllers.** ~40 API controllers, almost all 5–7 KB and near-identical.

**Processes.** Create, edit, soft-delete, list. Most carry a name-uniqueness rule.

**Correction, verified 2026-08-03:** these tables have **no parent-child foreign keys** — there is
no Phase → Sector → Block chain in the model. Block, for example, references nothing, and 20 other
entities store the block *name* as free text instead of referencing the row. See
`docs/modules/block.md` §6. Several of these tables are also SQL Server **temporal tables**, which
constrains how the migration squash can be done.

**Read.** Lowest risk, highest leverage. Forty controllers collapse into one generic slice plus
per-entity configuration. Doing this first proves the new architecture on real data with almost no
chance of breaking a business rule, and deletes roughly a quarter of the controller count. **This
is where I recommend we start.**

---

## M02 — Property and inventory

The spine of the system. Everything else references a plot.

**Screens (13).** Property Setup, Property Profile, Property List, Property Binding, Stock Creation,
Stock Creation Setup, Site Plan, Registration No Profile, Registration NPD, Re-Design, Map Design,
Map Approval, Possession Announcement.

**Controllers.** `StockCreationController` 47 KB · `RegistrationNoProfileController` 30 KB ·
`PropertyController` 19 KB · `MapApprovalController` 20 KB · `SitePlanController` 10 KB.

**Processes.** Plot creation and numbering · registration number allocation and re-allocation ·
plot status lifecycle (available → reserved → allotted → transferred → surrendered) · site plan
and map revision approval · binding a plot to a property record.

**Read.** `StockCreation` is the largest entity in the model and the hub of the graph. The status
lifecycle is the single most important thing to document correctly — almost every other module
reads or writes it. High value, medium risk.

---

## M03 — Users, roles and permissions

**Screens (11).** Login, Forgot Password, Change Password, PMS User, Department, User Permission
Mapping, Permission Form, Approval UI Setup, Fingerprint Enrolment, Fingerprint Verification,
Credential Config.

**Controllers.** `PMSUserController` 11 KB · `RolesPermissionsController` 7 KB ·
`UserPermissionMappingController` 6 KB · `FingerPrintController` 9 KB · `Login` 5 KB.

**Processes.** Authentication (session and JWT, currently unaware of each other) · two-factor ·
password reset · per-form permission grants (add/edit/delete/view) · biometric verification.

**Read.** Must land before any module that enforces permissions. The permission model itself is
sound — the defect is that nothing checks it on the server. Low domain risk, high platform impact.

---

## M04 — Approval engine

**Screens (5).** Approval Setup, Inbox, View Approval, Approval Permission, Index.

**Controllers.** `ApprovalsController` 96 KB · `ApprovalUISetupController` 5 KB. Plus
`B_Utility/BLL/ApprovalBLL.cs`.

**Processes.** Define an approval chain per request type · stages with a per-stage quorum
(`NumberOfApprovalRequired`) · route a request into the right chain · approve, reject, return ·
delegate · escalate · full history trail.

**Read.** The crown jewel and the highest-value thing in the repository. Roughly 30 request types
across the whole system route through it. `Inbox.cshtml` is 197 KB on its own. This gets the
deepest specification and the deepest test suite of anything we do — but it must come **after**
users and permissions, because it depends on both.

---

## M05 — Members and dealers

**Screens (14).** Member Profile, Member Registration, KYC, Dealer Profile, Dealer Registration,
Dealer Reservation, Dealer Renewal, and six dealer sub-tabs (Attachments, Deals, Estate,
Financials, Properties, Relationship History).

**Controllers.** `MemberProfileController` 69 KB · `DealerController` 29 KB ·
`DealerProfileController` 3 KB. Views: Member Profile 124 KB, Dealer Profile 93 KB.

**Processes.** Member onboarding and KYC · joint members and share splits · profile amendment
under approval · dealer registration, reservation, renewal and expiry · dealer-to-property
relationships · document attachment.

**Read.** Joint ownership and share splits are the part I most expect to get wrong without your
input. A 124 KB view means the form is doing far more than it looks.

---

## M06 — Sales pipeline

**Screens (11).** Lead Generation, Advance Application, Pre-Sale Approval, Booking Form, Booking
Backlog, Deals, Bulk Deal, Deal Merger, Deal Setup, Payment Plan Setup, Payment Plan Binding.

**Controllers.** `BookingController` 28 KB · `PreSaleController` 16 KB · `DealController` 16 KB ·
`BulkDealController` 15 KB · `LeadGenrationController` 14 KB · `PaymentPlanSetupController` 12 KB ·
`AdvanceApplicationController` 15 KB. Views: Booking Form 124 KB, Pre-Sale Approval 108 KB.

**Processes.** Lead → advance application → pre-sale approval → booking → deal · bulk deals for
multiple plots · deal merger · payment plan definition and binding to a booking · instalment
schedule generation · booking cancellation and backlog handling.

**Read.** A real state machine with money attached. The payment plan generation is a calculation
that needs worked examples before anything is rewritten.

---

## M07 — Transfers and ownership

The most intricate domain in the system.

**Screens (12).** Transfer, Transfer Form, Transfer Set Receiving, Transfer Tax Estimation,
Amalgamation, Change of Particulars, De-Allocation, Re-Number, Re-Purchase, Surrender,
Re-Surrender, Property Binding.

**Controllers.** `TransferReceiptProcessingController` 59 KB · `TransferHistoryController` 53 KB ·
`COPController` 28 KB · `TransferSetReceivingController` 26 KB · `RepurchaseController` 24 KB ·
`AmalgamationController` 22 KB · `SurrenderController` 15 KB · `RenumberController` 9 KB ·
`DeAllocationController` 5 KB. **`Transfer.cshtml` is 178 KB — the largest view in the system.**

**Processes.** Ownership transfer with tax estimation and receipt processing · amalgamating two
plots into one · splitting or re-numbering · repurchase by the authority · surrender and
re-surrender · change of particulars · de-allocation · the full ownership history chain.

**Read.** Highest complexity in the repository, and every process here touches money, approval and
the plot lifecycle simultaneously. This is the module where Phase 1 documentation earns its keep.
Nothing here gets rewritten until its spec is signed off.

---

## M08 — NDC, clearance and file movement

**Screens (15).** Member NDC, Dealer NDC, NDC1, File Verification NDC1, File Verification Request,
File Request, Client File Receiving, Clearance, Clearance Form, Store Room File Moving, File
Location Assignment, Demarcation, Demarcation Request, Demarcation Charges, Demarcation Charges I.

**Controllers.** `FileVerificationController` 40 KB · `NDC1Controller` 30 KB ·
`DemarcationController` 22 KB · `DemarcationRequestController` 14 KB ·
`StoreRoomFileMovingController` 13 KB · `ClearanceController` 8 KB. Views: Member NDC 98 KB,
Dealer NDC 90 KB, NDC1 69 KB.

**Processes.** No-dues certificate issuance for members and dealers · dues verification against
billing · physical file request, issue, return and location tracking · demarcation request,
charging and completion.

**Read.** NDC is where billing, transfers and approvals meet — it cannot be correct unless those
three are. Physical file tracking is a genuinely separate concern that may deserve its own slice.

---

## M09 — Construction and metering

**Screens (9).** Construction Monitoring, Construction Security, Construction Stage, Material
Testing, Meter Installation, Meter Reading, Reading Officer, Meter Phase-Wise Rate, Possession
Announcement.

**Controllers.** `ConstructionMonitoringController` 29 KB · `ConstructionSecurityController` 18 KB ·
`MeterInstallationController` 14 KB · `MeterReadingController` 13 KB · `MeterialTestingController`
4 KB.

**Processes.** Construction stage progression and inspection · security deposit against
construction · violation recording · meter installation and status · reading capture by officer ·
phase-wise tariff rates feeding the billing module.

**Read.** Self-contained and moderate. Meter readings feed M10, so the data contract between them
must be fixed before either is rewritten.

---

## M10 — Billing, charges and receipts

Where the money is. Highest correctness bar in the project.

**Screens (24).** Charges Group, Charges Group Form, Charges Setup, Surcharge Setup, Grace Period
Setup, Fixed Charge Generation, Fixed Bill Generation (Property-Wise), Monthly Bill Generation,
Monthly Bill Generation Backlog, Meter Bill Generation, Meter Bill Generation (One Go), Individual
Bill, Sale Tax, Withholding Tax, General Adjustment, Stand-Alone Adjustment, Receipt, Booking
Receipt Processing, Transfer Receipt Processing, Demand Note Form, DN Custodian, DN HOD, Purchase
Request, Commission.

**Controllers.** `GlobalChargesSetupController` 52 KB · `FixedChargeGenerationController` 39 KB ·
`DemandNoteController` 23 KB · `GenralAdjustmentController` 16 KB · `MeterBillGenerationController`
14 KB · `WithHoldingTaxController` 11 KB · `IndividualBillController` 9 KB · `SaleTaxController` 7 KB ·
`SurchargeSetupController` 6 KB. View: Charges Setup 62 KB, Fixed Bill Generation 65 KB.

**Processes.** Charge definition by group, type and property attributes · fixed and recurring
charge generation · meter-based billing from readings · surcharge and grace period application ·
sales tax and withholding tax calculation · manual adjustments · demand note issuance and approval
chain · receipt capture and allocation against dues · commission calculation.

**Read.** **Every calculation here needs a written formula and a worked example before any code is
written.** These become the first tests. This is the module where a silent error costs real money,
and it is the one I will be slowest and most careful with.

---

## M11 — Documents and letters

**Screens (15).** Allotment Letter, Allocation Letter, Additional Allotment, Intimation Letter,
First Intimation Letter, Ownership Agreement, Sales Agreement, Direct Sale, Privilege Allotment,
Defense Gardenia, Orchard Enclave, Orchard Enclave (Plot No), Service Benefit 13, Service Benefit
14, Service Benefit (Plot No).

**Controllers.** `DocumentController` 39 KB.

**Processes.** Generate a formatted legal document from live plot, member and payment data · scheme-
specific variants · print and archive.

**Read.** Fifteen near-identical templates that differ in wording and a few merge fields. Strong
candidate for one templating engine plus fifteen templates. Low logic risk, high tidy-up value.

---

## M12 — Litigation and soft-locks

**Screens (6).** Case Profile, Case Category, Case Type, Forum Setup, Lawyer Data, Soft Lock Name.

**Controllers.** `CaseProfileController` 22 KB · `SoftLockNameController` 5 KB. View: Case Profile
53 KB.

**Processes.** Register a legal case against a plot or member · attach a soft lock that blocks
transfer, NDC or billing action · hearing schedule and outcome · lawyer assignment · lock release.

**Read.** Small module, disproportionate importance: a soft lock is a *veto* over other modules.
The rule for which operations a lock blocks must be documented before M07 is rewritten.

---

## M13 — Reporting and dashboards

**Screens (21).** Twelve reports (Allocation, Cancel/Restoration, Caution, Dealer, File In/Out,
Member, NDC State, Record Room, Tax, Transfer, Transfer Revenue, Transfer Set Receiving), eight
dashboards (Admin, Member, Sales, Transfer, NDC, Inventory, Allotted Inventory, Available
Inventory), and Dynamic Report.

**Controllers.** `SPController` 171 KB (46 report endpoints, **no authorization attribute**) ·
`FilterController` 165 KB · `DashboardController` 43 KB · `DynamicQueryController` 4 KB.

**Processes.** Stored-procedure-backed reports returning a single JSON column · grid paging driven
by raw form fields · dashboard aggregates · user-defined dynamic reports.

**Read.** `SPController` and `FilterController` together are 336 KB and unauthenticated. The
dynamic report feature is genuinely useful and gets rebuilt safely rather than deleted. Reporting
lands late because it reads from everything else.

---

## M14 — Notifications, calendar and promotions

**Screens (9).** Alert Name, Create Alert, Form Alerts, Notifications, Week Schedule, Week Schedule
(Executive), Banners, Promotions, SMS.

**Controllers.** `NotificationController` 10 KB · `CalendarController` 10 KB · `BannerController`
15 KB · `PromotionController` 11 KB · `SMSController` 1 KB.

**Processes.** Per-form alert configuration · in-app notification delivery · SMS dispatch through
Telecard · appointment scheduling · promotional banners.

**Read.** Peripheral and low risk. SMS becomes a local no-op in Phase 0 and stays that way until
you decide otherwise.

---

## M15 — SAP Business One integration

**Screens (3).** GL Determination, SAP Billing, SAP Operations.

**Controllers.** **`SapIntegrationController` 268 KB** — the largest file in the repository ·
`SAPOperationsController` 7 KB · `SAPBillingController` 6 KB · `SAPDataBaseIntegrationController`
1 KB. Plus `Extensions/SAPConnection.cs`, `SAPBillingDb.cs`, `SAPOperationDb.cs`.

**Processes.** Push billing documents into SAP · GL account determination · master data sync ·
operational document posting · direct SAP query execution.

**Read.** This is what makes the solution uncompilable on any machine without SAP installed.
Isolating it behind one interface is the single highest-leverage structural change in the whole
plan. The rewrite happens **last**, against a fake, and only the adapter assembly ever touches COM.

---

## M16 — Data import jobs

**Screens (1).** Uploader.

**Projects.** `MemberUploader`, `NewMemberUploader`, `UpdateMemberProfile`, `StockDataUploader`,
`NewStockUploader`, `UpdateStockUploader`, `DeleteStockUploader`, `AutoTriggerService`.

**Processes.** Bulk import of members and stock from CSV · profile updates · stock deletion ·
scheduled triggering.

**Read.** Seven copy-pasted console applications, each with a hard-coded file path and a hard-coded
connection string to a different machine. Every one of them catches its own errors, prints, and
**exits with code zero** — so a scheduled task reports success after importing nothing. Two are not
even in the solution file. Becomes one worker.

---

## Recommended order, and why

| Order | Module | Reason it sits here |
|---|---|---|
| 1 | **M01 Master data** | Proves the architecture with near-zero domain risk, and deletes ~40 controllers |
| 2 | M03 Users and permissions | Everything downstream needs real server-side authorization |
| 3 | M04 Approval engine | ~30 request types depend on it; needs M03 first |
| 4 | M02 Property and inventory | The spine — the plot lifecycle every other module reads |
| 5 | M05 Members and dealers | The other half of every transaction |
| 6 | M06 Sales pipeline | Needs plots and parties to exist |
| 7 | M12 Litigation and soft-locks | Must exist before transfers, because locks veto transfers |
| 8 | M07 Transfers and ownership | The hardest module; needs everything above it |
| 9 | M09 Construction and metering | Feeds billing; independent of transfers |
| 10 | M10 Billing and charges | Needs meters, transfers and parties settled first |
| 11 | M08 NDC and file movement | Sits on top of billing, transfers and approvals |
| 12 | M11 Documents and letters | Reads from everything; pure output |
| 13 | M13 Reporting and dashboards | Reads from everything; last of the read-side work |
| 14 | M14 Notifications and calendar | Peripheral |
| 15 | M16 Data import jobs | Can move earlier if you need bulk loading sooner |
| 16 | M15 SAP integration | Last, behind the gateway, once everything it posts is stable |

**Totals.** ≈190 user-facing screens · 152 controllers · ~60 distinct business processes.

**Recounted 2026-08-04** for `05-MODULE-ARCHITECTURE.md`: **209 real forms**, once the 64
partials and 4 shared layout files are excluded from the 277 `.cshtml` count. Of those, 178 are
reachable from the menu and **15 are not reachable at all** — KYC, Deal Merger, Dealer
Reservation, Booking Backlog, Purchase Request, Map Design, Re-Design, Registration NPD, Admin
Dashboard and six more. Whether each is restored or retired is `#133`.

---

## Where I want to start

**M01, and inside it a single form — Block.** Five kilobytes of controller, one entity, four
endpoints. Small enough to read the whole analysis in a few minutes, complete enough that building
it establishes every pattern the other ~39 master-data screens reuse.

**Analysis complete — `docs/modules/block.md`.** It found 16 defects in the simplest screen in the
application, a temporal-table trap that would have destroyed audit history during the migration
squash, and one structural question about foreign keys that affects 20 entities and needs your
answer before any code is written.
