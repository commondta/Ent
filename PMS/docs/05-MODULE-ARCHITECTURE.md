# PMS — Module and Navigation Architecture

> How every form in the system is grouped, named, reached and secured.
> Measured from the repository 2026-08-04. **No solution code changed.**

> **Largely superseded 2026-08-16.** The live source of truth for the structure is now the
> **`NavigationNodes` registry** (223 nodes — 4 top-level modules after D31, up to 5 levels)
> seeded from `HRMS_Web/App_Data/navigation-seed.json`, which was generated from the
> `AI file.xlsx` Folders Structure + PMS-Modules sheets. The menu renders from it;
> `DisplayName` is separated from `PermissionKey` (the §1.4 renaming trap is closed); the
> measured defects in §1 are fixed by the swap. **§5's 12-module taxonomy is dead** (D20/D30/
> D31). What remains valuable here: the §1 measurements as the historical baseline, and the
> item-type/shell thinking that fed the registry design. Do not seed or plan from this file —
> use the registry and `AI-FILE-OBSERVATIONS.md` §12.

This document does three things:

1. Records **what the structure is today**, measured — not remembered.
2. Defines the **target module taxonomy** and the shell that presents it.
3. Maps **every existing form** to its target module, sub-area and item type.

It is the contract for how forms are organised. Once agreed, `PROJECT.md`, the menu, the
permission catalogue and the API route scheme all follow from it.

---

## 1 · Current state, measured

### 1.1 Folders are not modules

`HRMS_Web/Views` holds **32 folders, 277 `.cshtml` files**. The folders carry almost no meaning:

| Folder | Views | What is actually in it |
|:--|--:|:--|
| `Home` | 50 | Block, Sector, Property Setup, Construction Monitoring, Map Approval, Clearance, Demarcation, Stock Creation — eight unrelated domains |
| `PartialPage` | 64 | Shared partials |
| `Operations` | 22 | Transfers, NDC, file movement, plus two setup tables |
| `Sales` | 19 | Members, dealers, leads, bookings, payment plans |
| `Billing` | 17 | Bill runs **and** meter master data |
| `Document` | 15 | 15 near-identical letters |
| `Reports` | 12 | 12 reports |
| 25 others | 78 | Mostly 1–8 views each |

Excluding partials and `Shared`, there are **209 real forms**.

### 1.2 The menu

The entire navigation is hard-coded in `HRMS_Web/Views/Shared/_Layout.cshtml` — **243 KB**,
2 750 lines. It contains:

- **22 top-level groups**, nested up to **4 levels** deep
- **200 leaf links**, but only **178 distinct targets**

### 1.3 Verified defects

| # | Defect | Evidence |
|:--|:--|:--|
| N1 | Master data buried inside a transaction menu | `Transfer & Records → Transfer & Record` holds Phase, Block, Force, Rank, Category, UOM, Prefix, Postfix, Quota, Almt |
| N2 | Near-duplicate group names | `Transfer & Records` contains a child called `Transfer & Record` |
| N3 | One menu spanning five domains | `Operation Forms` — 30 flat items: Stock Creation, Member Profile, Booking, NDC, Transfer, Surrender, Surcharge Setup, LDA Plot No |
| N4 | Dead copy-paste subtree | `Administration → Reports → MemberReports` lists Floor, Features, Finishes, Sector, Property Setup — the *setup* items pasted in. `NDC Reports` and `Transfer Reports` under it are the same paste. The whole subtree is fake |
| N5 | Wrong target shipped | "Transfer Set Receiving" → `Home/SitePlan` |
| N6 | Label/target mismatch | "Drawing Scrutiny Charges" → `Home/DemarcationRequest` |
| N7 | Dead links in the live menu | "Finger Uploader" ×2 → `Uploader/FingerUploader`; `UploaderController` only has `Index()` |
| N8 | Foreign items leaked into a group | `Calendar Setup` contains SAP Billing and GL Determination |
| N9 | Empty groups rendered | `Setup Forms → Billing`, `Global Master Data Forms` |
| N10 | Singleton menus | Transfer Tax Estimation, Clearance Setup, Receipt, Operations (1 item — Dealer Registration), Govt Taxes, Commission |
| N11 | One domain across six menus | Dealer: Registration, Profile, Category, Designation, Renewal, NDC — six different places |
| N12 | Setup interleaved with daily work | Meter Type / Phase / Status / Reading Officer sit beside Meter Bill Generation runs |
| N13 | A form named *Test* is in production navigation | "Charges Incorporation Setup" → `Globalsetup/ChargesGroupFormTest` |
| N14 | 22 duplicated links | `Home/FingurePrint` ×3, `Sales/LeadGeneration` ×3, and 18 more ×2 |
| N15 | ~15 working forms unreachable | KYC Form, Deal Merger, Dealer Reservation, Booking Backlog, Map Design, Re-Design, Registration NPD, Purchase Request, Demarcation Charges, Demarcation Charges I, Charges Group Form, Unit of Measure, Admin Dashboard, `Operations/Propertybinding`, `Operations/TransferForm` |
| N16 | Naming has no standard | "Definition" on some, "Form" on others, neither on the rest; typos shipped in labels: *Privilidge*, *Meterial*, *Applictaion*, *Permisison*, *Genral*, *ReStrotion*, *Exective* |

### 1.4 The constraint that shapes the whole design

```
  _Layout.cshtml                    Permissions table            PermissionForms table
  ──────────────                    ─────────────────            ─────────────────────
  Html.UserHavePermission           FormName   (string)          Name    (string)
    ("Transfer & Records")   ─────► isPermitted (bool)           Title   (string)
                                    RolesPermissionsId           IsActive
         ▲                                                       SerialNo
         │                                                          │
         └──────── the menu label IS the permission key ────────────┘
                          matched by string
```

`Permissions.FormName` is a **string** and the layout checks it by literal. `PermissionForms`
is a **flat table** — `Name`, `Title`, `IsActive`, `SerialNo` — with **no parent, no module,
no hierarchy**.

Three consequences:

- Renaming a menu item **silently revokes access** for every role.
- There is **no module concept anywhere in the data model** — modules exist only as `<li>`
  nesting in a Razor file.
- The menu, the permission catalogue and the API have **three separate ideas** of what a form is,
  and nothing keeps them in agreement.

**So the deliverable is not a re-ordered menu. It is a module/form registry held as data,**
from which the menu, the permission catalogue and the API authorization policies are all derived.

---

## 2 · Design basis

### 2.1 Alternatives considered

| Model | Shape | Verdict |
|:--|:--|:--|
| **Module workspace** (Dynamics 365 F&O) | Module rail → module landing page → forms grouped by *what they are*: Workspaces, Transactions, Inquiries, Periodic, Reports, Setup | **Chosen.** Every form has exactly one home; two clicks to anything; maps 1:1 onto a registry table; closest to the left-sidebar habit users already have |
| **Work vs Setup split** (Oracle Fusion) | Navigator for transactions only; one separate Setup & Maintenance area for all configuration | Rejected as the primary model — configuring a Block means leaving the Property module. Its best idea is kept: see D11 |
| **Role launchpad** (SAP Fiori) | Role-scoped tiles with live counts, search-first | Rejected for now. Best-looking and best for focused roles, but it hides the system map and needs the most front-end work. The tile/count idea is kept as module **Workspaces** |

### 2.2 Decisions taken

| # | Decision | Rationale |
|:--|:--|:--|
| **D10** | Module-workspace navigation. A module rail, a module landing page per module, forms grouped by item type | Chosen above. Bounds depth at 2 clicks and kills the 4-level tree |
| **D11** | Setup lives **inside its owning module**, and Administration additionally carries a flat, searchable **Configuration index** of every setup form | Users configure in context; an administrator doing initial setup does not walk twelve modules. One registry, two views — no duplication |
| **D12** | The menu, the permission catalogue and the API authorization policies all read **one module/form registry** held as data | Ends the string-matching coupling in §1.4. A form is defined once |
| **D13** | The module list is **data, versioned in the registry** — modules and sub-areas are added or renamed without touching shell code | Your instruction: the module set will grow as features are added |
| **D14** | Twelve top-level modules; sub-areas carry the detail | Fits the rail without scrolling; folds the 16 inventory modules where users think of them as one thing |

### 2.3 Principles

1. **One home per form.** A form belongs to exactly one module and one sub-area. It may be
   *surfaced* elsewhere (a report on a dashboard, a dashboard as a module workspace), but it is
   *defined* once.
2. **Group by what a form is, not by who built it.** Item type is a property of the form.
3. **Doing work and configuring the system are different activities**, and the shell shows them
   differently — Setup is always the last group on a module page, visually separated.
4. **Nothing above two clicks.** Rail → module page → form. Search reaches anything in one.
5. **A form that is not in the registry does not exist** — no orphans, no dead links, no
   unreachable views.

---

## 3 · The shell

```
┌──────────────────────────────────────────────────────────────────────────────┐
│  PMS    ⌂ Home    🔍 Search anything…            🔔 3   ✉ Inbox 12   Adnan ▾ │
├────────────────┬─────────────────────────────────────────────────────────────┤
│                │  Home  ›  Property & Inventory                              │
│  MY WORK       │                                                             │
│   ✉ Inbox   12 │  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐            │
│   ↩ Returned 3 │  │     412     │ │      87     │ │      19     │            │
│   🔔 Alerts    │  │  Available  │ │  Allotted   │ │  Pending    │  WORKSPACES│
│                │  │    plots    │ │  this month │ │  map appr.  │            │
│  ★ FAVOURITES  │  └─────────────┘ └─────────────┘ └─────────────┘            │
│   Transfer Form│                                                             │
│   Block        │  TRANSACTIONS                                               │
│  ⏱ RECENT      │   Stock Creation          Registration No. Profile          │
│   Booking Form │   Property Binding        Property Profile                  │
│                │   Site Plan               Map Design                        │
│  MODULES       │   Re-Design               Map Approval                      │
│   Dashboards   │                                                             │
│  ▸Property     │  INQUIRIES                                                  │
│   Parties      │   Property List                                             │
│   Sales        │                                                             │
│   Transfers    │  REPORTS                                                    │
│   Billing      │   Allocation Report       Caution Report                    │
│   NDC & Records│                                                             │
│   Construction │  ──────────────────────────────────────────────────────     │
│   Litigation   │  ⚙ SETUP                                                    │
│   Documents    │   Land model     Phase · Sector · Block · LDA Plot No ·     │
│   Reports      │                  Project · Real Estate Type                 │
│   Administration│  Plot attributes Property Type · Nature · Category ·        │
│                │                  Floor · Features · Finishes · Prefix ·     │
│                │                  Postfix · Unit of Measure                  │
│                │   Configuration  Property Setup · Stock Creation Setup      │
└────────────────┴─────────────────────────────────────────────────────────────┘
```

### 3.1 Regions

| Region | Holds | Source |
|:--|:--|:--|
| **My Work** | Approval Inbox, items returned to me, my alerts — pinned, always visible, never inside a module | Live counts per user |
| **Favourites** | User-pinned forms | Per user, saved |
| **Recent** | Last 8 forms opened | Per user, session-persisted |
| **Modules** | The 12 module entries the user has any permission in | Registry, filtered by claims |
| **Module page** | That module's forms, grouped by item type | Registry, filtered by claims |
| **Search** | Every form the user may open, matched on title, synonyms and module | Registry |

The approval **Inbox** is deliberately *not* a module. It is where a user starts their day, and
today it is not in the sidebar at all — only a top-bar link — while being the single most-used
screen in the system.

### 3.2 Item types

Every registry entry declares exactly one:

| Type | Meaning | Placement on the module page |
|:--|:--|:--|
| `Workspace` | Landing page with counts and quick actions | Top, as cards |
| `Transaction` | Creates or changes business data | First list |
| `Inquiry` | Read-only search or list | Second list |
| `Periodic` | A batch run over many records | Third list, marked as a run |
| `Report` | Printable or exportable output | Fourth list |
| `Setup` | Configuration or master data | Last, below a rule, with a ⚙ marker |
| `Component` | A tab or partial inside another form; never in a menu | Not shown |

`Periodic` is separated from `Transaction` on purpose: bill generation runs act on thousands of
records at once, and an operator must never reach one by accident while looking for a single bill.

### 3.3 Setup, shown twice from one definition

```
   Registry  (one row per form)
        │
        ├──► Module page → SETUP section        (in context, while working)
        │
        └──► Administration → Configuration     (flat, searchable, grouped by module;
                                                 for initial and bulk configuration)
```

The Configuration index is a *view* over the registry, not a second list. Adding a setup form to
a module makes it appear in both places automatically.

---

## 4 · Target module map

Twelve modules. `T` transaction · `I` inquiry · `P` periodic · `R` report · `S` setup ·
`W` workspace.

| # | Module | Sub-areas | Forms | Of which setup |
|:--|:--|:--|--:|--:|
| 1 | **Dashboards** | Inventory · Sales · Transfer · NDC · Member · Admin | 8 | 0 |
| 2 | **Property & Inventory** | Plots · Drawings & Maps · Land model · Plot attributes · Configuration | 29 | 20 |
| 3 | **Parties** | Members · Dealers · Identity · Member setup · Dealer setup | 24 | 7 |
| 4 | **Sales** | Pipeline · Deals · Payment plans · Setup | 14 | 4 |
| 5 | **Transfers & Ownership** | Transfer · Ownership change · Exit · Scheduling · Setup | 20 | 4 |
| 6 | **Billing & Finance** | Bills · Receipts · Adjustments · Demand notes · Bill runs · Charges setup · Tax setup | 25 | 9 |
| 7 | **NDC & Records** | NDC · File verification · File movement · Clearance · Setup | 16 | 1 |
| 8 | **Construction & Utilities** | Construction · Demarcation · Metering · Setup | 18 | 8 |
| 9 | **Litigation & Locks** | Cases · Setup | 6 | 5 |
| 10 | **Documents & Letters** | Allotment · Intimation · Plot-No variants · Agreements | 15 | 0 |
| 11 | **Reports & Insights** | Dynamic reporting · cross-listed module reports | 2 (+12 shared) | 0 |
| 12 | **Administration** | Users & security · Workflow · Notifications · Content · Integration · Data management · Configuration index | 25 | — |

Plus **My Work** (pinned, 3 entries) and the Home launcher.

---

## 5 · Form-by-form mapping

Every one of the 209 real forms. `Current view` is the path under `HRMS_Web/Views`.

### 5.1 My Work — pinned, not a module

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Approval/Inbox` | Approval Inbox | W | 197 KB — largest view in the system; today reachable only from the top bar |
| `Approval/ViewApproval` | View Approval | T | Opened from the inbox |
| `Notification/Index` | My Notifications | I | Today reachable only from the top bar |

### 5.2 Dashboards

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Dashboard/InventoryDashboard` | Inventory | W | Also module 2 workspace |
| `Dashboard/AvailableInventoryDashboard` | Available Inventory | W | Also module 2 |
| `Dashboard/AllotedInventoryDashboard` | Allotted Inventory | W | Also module 2 |
| `Dashboard/SalesDashboard` | Allocation & Sales | W | Also module 4 |
| `Dashboard/TransferDashboard` | Transfers | W | Also module 5 |
| `Dashboard/NDCDashboard` | NDC | W | Also module 7 |
| `Dashboard/MemberDashboard` | Members | W | Also module 3 |
| `Dashboard/AdminDashboard` | Administration | W | **Orphan today** — restore or retire |

### 5.3 Property & Inventory

**Plots**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/StockCreation` | Stock Creation | T | Hub of the model; 77 KB view, 47 KB controller |
| `Home/PropertyBinding` | Property Binding | T | |
| `Operations/Propertybinding` | — | — | **Duplicate view, orphan.** Retire one |
| `Home/RegistrationNoProfile` | Registration No. Profile | T | 117 KB view |
| `Home/RegistrationNPD` | Registration NPD | T | **Orphan today** |
| `Home/PropertyProfile` | Property Profile | T | |
| `Home/propertyList` | Property List | I | |

**Drawings & maps**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/SitePlan` | Site Plan | T | N5: a menu item labelled "Transfer Set Receiving" points here |
| `Home/MapDesign` | Map Design | T | **Orphan today** |
| `Home/ReDesign` | Re-Design | T | **Orphan today** |
| `Home/MapApproval` | Map Approval | T | 77 KB |

**Setup — land model**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/PhaseDef` | Phase | S | |
| `Home/Sector` | Sector | S | Listed in two menus today |
| `Home/Block` | Block | S | First rebuild slice — `docs/modules/block.md` |
| `Home/LDAPlotNo` | LDA Plot No. | S | Today sits under *Operation Forms* |
| `Home/Project` | Project | S | |
| `Home/RealEstate` | Real Estate Type | S | |

**Setup — plot attributes**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/PropertyType` | Property Type | S | |
| `Home/Nature` | Property Nature | S | |
| `Home/Category` | Category | S | **Open Q1** — plot category or member category? |
| `Home/Floor` | Floor | S | In two menus today |
| `Home/FeaturesDef` | Features | S | In two menus today |
| `Home/Finishes` | Finishes | S | In two menus today |
| `Home/Prefix` | Prefix | S | |
| `Home/Postfix` | Postfix | S | |
| `Home/UOMDef` | Unit of Measure | S | |
| `Home/Unitofmeasure` | — | — | **Duplicate view, orphan.** Retire one |
| `Home/Almt` | Allotment Type | S | **Open Q2** — Property or Sales? |
| `Home/Quota` | Quota | S | **Open Q2** — Property or Sales? |

**Setup — configuration**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/PropertySetup` | Property Setup | S | 62 KB; in two menus today |
| `Home/StockCreationSetup` | Stock Creation Setup | S | In two menus today |

### 5.4 Parties

**Members**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Sales/MemberRegistration` | Member Registration | T | |
| `Sales/MemberProfile` | Member Profile | T | 124 KB view, 69 KB controller |
| `Sales/KYCForm` | KYC | T | **Orphan today** |

**Dealers**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Sales/DealerRegistration` | Dealer Registration | T | Today the only item in a menu called *Operations* |
| `Sales/DealerProfile` | Dealer Profile | T | 93 KB |
| `Sales/DealerReservation` | Dealer Reservation | T | **Orphan today** |
| `Sales/RenewalForm` | Dealer Renewal | T | |
| `Dealer/AttachmentsDetails` | Attachments | Component | Tab inside Dealer Profile |
| `Dealer/DealsDetails` | Deals | Component | Tab |
| `Dealer/EstateDetails` | Estate | Component | Tab |
| `Dealer/FinancialsDetails` | Financials | Component | Tab |
| `Dealer/PropertiesDetails` | Properties | Component | Tab |
| `Dealer/RelationshipHistoryDetails` | Relationship History | Component | Tab |
| `Dealer/RenewalDetails` | Renewal | Component | Tab |

**Identity**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `FingerPrint/FingerPrint` | Enrol Fingerprint | T | |
| `FingerPrint/VerifyFingerPrint` | Verify Fingerprint | T | |
| `Home/FingurePrint` | — | — | **Third duplicate**, linked 3× from the menu. Resolve against the two above |

**Setup**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/MemberCategory` | Member Category | S | |
| `Home/SocialStatus` | Social Status | S | In two menus today |
| `Home/Force` | Force | S | Military service branch |
| `Home/Rank` | Rank | S | |
| `Home/VerificationType` | Verification Type | S | |
| `Sales/DealerCategory` | Dealer Category | S | In two menus today |
| `Sales/DealerDesignation` | Dealer Designation | S | Today under *Global Forms* |

### 5.5 Sales

**Pipeline**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Sales/LeadGeneration` | Lead Generation | T | Linked 3× today |
| `Sales/AdvanceApp` | Advance Application | T | |
| `Sales/PreSaleApproval` | Pre-Sale Approval | T | 108 KB; linked 2× |
| `Sales/BookingForm` | Booking | T | 124 KB; linked 2× |
| `Sales/BookingBackLog` | Booking Backlog | I | **Orphan today** |

**Deals**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Deal/Deals` | Deal | T | |
| `Deal/BUlkDeal` | Bulk Deal | T | Filename typo |
| `Sales/DealMerger` | Deal Merger | T | **Orphan today** |

**Payment plans**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Sales/PaymentPlanBinding` | Payment Plan Binding | T | |

**Setup**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Sales/DealSetup` | Deal Setup | S | |
| `Sales/PaymentPlanSetup` | Payment Plan Setup | S | Linked 2×; a setup form sitting in *Operation Forms* today |
| `Sales/PaymentPlanType` | Payment Plan Type | S | |

**Reports** — cross-listed from module 11: Allocation Report, Cancel/Restoration Report.

### 5.6 Transfers & Ownership

**Transfer**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/Transfer` | Transfer | T | **178 KB — largest view in the system** |
| `Operations/TransferForm` | — | — | 23 KB. **Orphan.** Resolve against the above |
| `Operations/TransferSetReceiving` | Transfer Set Receiving | T | |
| `GovtTaxes/TransferReceiptProcessing` | Transfer Receipt Processing | T | 186 KB view |
| `Operations/TransferTaxEstimation` | Transfer Tax Estimation | I | Its own top-level menu today, for one form |

**Ownership change**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/Amalgamation` | Amalgamation | T | |
| `Operations/COP` | Change of Particulars | T | Menu calls it "Change Of Plot" — **Open Q3** |
| `Operations/ReNumber` | Re-Number | T | |

**Exit**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/DeAllocation` | De-Allocation | T | |
| `Operations/RePurchase` | Repurchase / Refund / Cancellation | T | |
| `Operations/Surrender` | Surrender | T | |
| `Operations/ReSurrender` | Re-Surrender | T | |

**Setup**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/TransferType` | Transfer Type | S | In two menus today |
| `Operations/TaxType` | Tax Type | S | |
| `Calendar/WeekSchedule` | Transfer Schedule — Regular | S | Today under *Calendar Setup*, alongside two leaked SAP items |
| `Calendar/WeekScheduleExective` | Transfer Schedule — Executive | S | Filename typo |

**Reports** — cross-listed: Transfer, Transfer Revenue, Transfer Set Receiving, Tax.

### 5.7 Billing & Finance

**Bills and receipts**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Billing/IndividualBill` | Individual Bill | T | |
| `Receipt/Receipt` | Receipt | T | Its own top-level menu today, for one form |
| `GovtTaxes/BookingReceiptProcessing` | Booking Receipt Processing | T | Today under *Administration → Govt Taxes* |
| `Commission/Index` | Commission | T | A lone leaf directly under Administration today |

**Adjustments**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `GenralAdjustment/GenralAdjustment` | General Adjustment | T | Folder and label typo |
| `GenralAdjustment/StandAlone` | Stand-Alone Invoice | T | |

**Demand notes**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `DemandNote/DemandNoteForm` | Demand Note | T | |
| `DemandNote/DNHOD` | Demand Note — HOD Action | T | |
| `DemandNote/DNCustodian` | Demand Note — Custodian Action | T | |
| `DemandNote/PurchaseRequest` | Purchase Request | T | **Orphan today** |

**Bill runs**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Billing/FixedChargeGeneration` | Fixed Charge Generation | P | 39 KB controller |
| `Billing/FixedBillGenerationPropertyWise` | Fixed Bill Generation — Property-Wise | P | Linked 2×, once as a *setup* item |
| `Billing/MonthlyBillGeneration` | Monthly Bill Generation | P | |
| `Billing/MonthlyBillGenerationBackLog` | Monthly Bill Backlog | P | |
| `Billing/MeterBillGeneration` | Meter Bill Generation | P | |
| `Billing/MeterBillGenerationOneGo` | Meter Bill Generation — All | P | |

**Setup — charges**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `GlobalSetup/ChargesGroup` | Charges Group | S | |
| `GlobalSetup/ChargesType` | Charges Type | S | |
| `GlobalSetup/ChargesSetup` | Charges Setup | S | 62 KB view, 52 KB controller |
| `GlobalSetup/ChargesGroupFormTest` | Charges Incorporation Setup | S | **N13 — a form named *Test* is in the live menu** |
| `GlobalSetup/ChargesGroupForm` | — | — | **Orphan.** Resolve against the above |
| `Home/SurchargeSetup` | Surcharge Setup | S | Today inside *Operation Forms* |
| `Billing/GracePeriodSetup` | Grace Period Setup | S | |

**Setup — tax**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Billing/SaleTax` | Sales Tax | S | |
| `Billing/WithHoldingTax` | Withholding Tax | S | |

### 5.8 NDC & Records

**NDC**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/MemberNDC` | Member NDC | T | 98 KB |
| `Operations/DealerNDC` | Dealer NDC | T | 90 KB |
| `Operations/NDC1` | NDC-1 | T | 69 KB |

**File verification and movement**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/FileVerificationRequest` | File Verification Request | T | |
| `Operations/FileVerificationNDC1` | File Verification — NDC1 | T | |
| `Operations/FileRequest` | File Doc / Duplicate Request | T | |
| `Operations/ClientFileReceiving` | Client File Receiving | T | |
| `StoreRoomFileMoving/StoreRoomFileMoving` | Record Room File Movement | T | |
| `StoreRoomFileMoving/FileLocationAssignment` | File Location Assignment | T | |

**Clearance**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/Clearance` | Clearance | T | 61 KB; its own top-level menu today |
| `Home/ClearanceForm` | Clearance Form | T | Today under *Building Control* |

**Setup**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Operations/NDCRequestType` | NDC Request Type | S | |

**Reports** — cross-listed: NDC State, Record Room Files, Files In/Out, Caution.

### 5.9 Construction & Utilities

**Construction**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/ConstructionM` | Construction Monitoring | T | **105 KB view** |
| `Home/ConstructionSecurity` | Construction Security | T | |
| `Home/MeterialTesting` | Material Testing | T | Folder and label typo |
| `Home/PossessionAnnouncement` | Possession Application | T | |

**Demarcation**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/Demarcation` | Demarcation | T | **Open Q4** — Construction or NDC & Records? |
| `Home/DemarcationRequest` | Demarcation Request | T | N6: labelled "Drawing Scrutiny Charges" today |
| `Home/DemarcationCharges` | Demarcation Charges | T | **Orphan today** |
| `Home/DemarcChargesI` | Demarcation Charges I | T | **Orphan today**; resolve against the above |

**Metering**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Billing/MeterInstallation` | Meter Installation | T | Moves out of Billing |
| `Billing/MeterReading` | Meter Reading | T | Moves out of Billing |

**Setup**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Home/ConstructionStage` | Construction Stage | S | In two menus today |
| `Billing/MeterType` | Meter Type | S | |
| `Billing/MeterPhase` | Meter Phase | S | |
| `Billing/MeterStatus` | Meter Status | S | |
| `Billing/MeterPhaseWiseRate` | Meter Phase-Wise Rate | S | Feeds meter billing |
| `Billing/ReadingOfficer` | Reading Officer | S | |
| `GlobalSetup/ViolationGroup` | Violation Group | S | Today under *Global Setup Forms* |
| `GlobalSetup/ViolationType` | Violation Type | S | |

### 5.10 Litigation & Locks

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Litigation/CaseProfile` | Case Profile | T | 53 KB |
| `Litigation/CaseCategory` | Case Category | S | |
| `Litigation/CaseType` | Case Type | S | |
| `Litigation/ForumSetup` | Forum | S | In two menus today |
| `Litigation/LawyerData` | Lawyer | S | |
| `Notification/SoftLockName` | Soft Lock Name | S | Moves out of *Alerts*. A soft lock **vetoes** transfers, NDC and billing |

### 5.11 Documents & Letters

| Current view | Item | Type | Sub-area |
|:--|:--|:--|:--|
| `Document/AllotmentLetter` | Allotment Letter | T | Allotment |
| `Document/AllocationLetter` | Allocation Letter | T | Allotment |
| `Document/AdditionalAllotment` | Additional Allotment | T | Allotment |
| `Document/FirstIntimationLetter` | First Intimation Letter | T | Intimation |
| `Document/IntimationLetter` | General Letter | T | Intimation |
| `Document/DefenseGardenia` | Defence Gardenia | T | Intimation — scheme |
| `Document/DirectSale` | Direct Sale | T | Intimation — scheme |
| `Document/SvcBenefit13` | Service Benefit 13 | T | Intimation — scheme |
| `Document/SvcBenefit14` | Service Benefit 14 | T | Intimation — scheme |
| `Document/OrchardEnclave` | Orchard Enclave | T | Intimation — scheme |
| `Document/PrivilidgeAllotment` | Privilege Allotment | T | Intimation — scheme; typo |
| `Document/SvcBenefitPlotNo` | Service Benefit — by Plot No. | T | Plot-No variant |
| `Document/OrchardEnclavePlotNo` | Orchard Enclave — by Plot No. | T | Plot-No variant |
| `Document/OwnershipAgreement` | Ownership Agreement | T | Agreements |
| `Document/SalesAgreement` | Sales Agreement | T | Agreements |

All fifteen are one engine plus fifteen templates. The sub-areas above become template
categories, not fifteen menu entries.

### 5.12 Reports & Insights

| Current view | Item | Type | Also listed in |
|:--|:--|:--|:--|
| `DynamicQuery/Index` | Dynamic Query | I | — |
| `DynamicQuery/DynamicReport` | Dynamic Report | R | — |
| `Reports/TransferReport` | Transfer Report | R | Transfers |
| `Reports/TransferRevenueReport` | Transfer Revenue Report | R | Transfers |
| `Reports/TransferSetReceivingReport` | Transfer Set Receiving Report | R | Transfers |
| `Reports/TaxReport` | Tax Report | R | Transfers, Billing |
| `Reports/NdcStateReport` | NDC State Report | R | NDC & Records |
| `Reports/RecordRoomReport` | Record Room Files Report | R | NDC & Records |
| `Reports/FileInOutReport` | Files In/Out Report | R | NDC & Records |
| `Reports/CautionReport` | Caution Report | R | NDC & Records |
| `Reports/AllocationReport` | Allocation Report | R | Sales |
| `Reports/CancelReStrotionReport` | Cancellation / Restoration Report | R | Sales; typo |
| `Reports/MemberReport` | Member Report | R | Parties |
| `Reports/DealerReport` | Dealer Report | R | Parties |

Cross-listing is a registry flag, not a duplicated entry.

### 5.13 Administration

**Users & security**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `PMSUser/PMSUser` | Users | S | |
| `PMSUser/Department` | Departments | S | |
| `Approval/Permission` | Role Permissions | S | |
| `UserPermissionMapping/Index` | User Permissions | S | |
| `PermissionSetup/PermissionForm` | Permission Forms | S | **Becomes the registry admin screen** |
| `CredentialConfig/Index` | Credentials Config | S | |
| `Login/Index` | Sign In | — | Outside the shell |
| `Login/Forget` | Forgot Password | — | Outside the shell |
| `Login/ChangePassword` | Change Password | T | Under the user menu, not a module |

**Workflow & approvals**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Approval/ApprovalSetup` | Approval Setup | S | Chains, stages, quorum |
| `Approval/Index` | Approval Tree | I | |
| `PermissionSetup/ApprovalUISetup` | Approval UI Setup | S | |

**Notifications**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Notification/AlertName` | Alert Name | S | |
| `Notification/Create` | Generate Alert | T | |
| `Notification/FormAlerts` | Form Alerts | S | Per-form alert configuration |

**Content**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Promotion/Promotions` | Promotions | T | |
| `Promotion/Banners` | Assets & Media | T | |

**Integration**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `SAPDataBaseIntegration/SAPOperations` | SAP Operations | T | Behind the gateway once `#35` lands |
| `SAPDataBaseIntegration/SAPBilling` | SAP Billing | T | Leaked into *Calendar Setup* today |
| `SAPDataBaseIntegration/GLDetermination` | GL Determination | S | Leaked into *Calendar Setup* today |

**Data management**

| Current view | Item | Type | Note |
|:--|:--|:--|:--|
| `Uploader/Index` | Table Uploader | T | |
| — | Finger Uploader | — | **N7 — two live menu links to an action that does not exist** |

**Configuration index** — new. A flat, searchable list of every `Setup` row in the registry,
grouped by module. No new forms; a view over data.

### 5.14 Not navigable

| Current view | Disposition |
|:--|:--|
| `Home/Index` | Becomes the Home launcher — My Work, favourites, recent, module cards |
| `Home/_MainDomainBreakdownTable` | Component |
| `Views/PartialPage/*` (64) | Components |
| `Views/Shared/*` (4) | Layout and error |

---

## 6 · The registry

The structure above must be stored, not coded. Proposed shape:

```
  Module                      NavigationArea               NavigationItem
  ──────────────────          ──────────────────           ────────────────────────
  Id                          Id                           Id
  Key        "property"       ModuleId ──────────┐         AreaId ─────────┐
  Title      "Property…"      Key    "land-model"│         Key   "block"   │
  Icon                        Title  "Land model"│         Title "Block"   │
  SortOrder                   SortOrder          │         ItemType  S/T/I/P/R/W
  IsActive                    IsActive           │         Route "/property/block"
      ▲                                          │         PermissionKey  ◄── stable,
      └──────────────────────────────────────────┘         SortOrder          never the
                                                           IsActive           label
                                                           CrossListedIn (0..n)
```

Rules:

- **`PermissionKey` is stable and opaque** — `property.block`, not "Block Definition". Titles
  become free to change; access does not move.
- One row per form. `CrossListedIn` handles a report appearing under both its module and Reports.
- The API authorization policy for an endpoint is derived from the same `PermissionKey`, so a
  hidden menu item and a rejected API call cannot disagree.
- Migration from today: `PermissionForms.Name` values are mapped one-to-one to `PermissionKey`,
  and existing `Permissions.FormName` grants are rewritten in the same transaction. **No role
  loses access during the change** — this is a data migration with a verification query, not a
  re-grant exercise.

---

## 7 · Naming standard

Applied to every `Title` in the registry.

| Rule | Do | Don't |
|:--|:--|:--|
| Name the thing, not the screen | `Block` | `Block Definition`, `Block Form` |
| Setup items are singular nouns | `Charges Type` | `Charges Types Setup` |
| Transactions are the business act | `Transfer`, `Surrender` | `Transfer Form` |
| Runs say what they do | `Monthly Bill Generation` | `Monthly Bill Gen` |
| Reports end in `Report` | `Transfer Revenue Report` | `Transfer Revenue` |
| Expand abbreviations except NDC, DN, UOM, LDA, GL, SAP, KYC | `Change of Particulars` | `COP` |
| British-Pakistani English, spelled correctly | `Privilege`, `Material`, `Defence` | `Privilidge`, `Meterial`, `Defense` |

The 12 shipped typos in §1.3/N16 are fixed as part of this — in titles only. Route keys and
`PermissionKey` values are chosen once, correctly, and never renamed after.

---

## 8 · Adding to the structure later

D13 requires this to be routine. It is data, in every case:

| To add | Do this | Code change? |
|:--|:--|:--|
| A form to an existing sub-area | Insert a `NavigationItem`; grant the permission | None |
| A sub-area to a module | Insert a `NavigationArea` | None |
| A whole module | Insert a `Module` + its areas + its items | None — rail, search and Configuration index pick it up |
| Re-order anything | Change `SortOrder` | None |
| Retire a form | `IsActive = false` — grants and history are preserved | None |
| Move a form between modules | Change `AreaId`; `PermissionKey` **stays** | None, and no role loses access |
| A new item type | Extend the enum and the module-page template | Small, shell only |

Two rules that keep this honest:

1. **A form is registered before it is routed.** An endpoint with no registry entry has no
   permission policy, so it fails closed.
2. **Retire, never delete.** `IsActive = false` keeps audit history and permission grants intact.

---

## 9 · Feasibility

| | |
|:--|:--|
| **Solution code changed by this document** | None |
| **Blocked by** | Nothing. The registry is new tables plus a data migration; independent of the .NET 10 skeleton (`#32`) |
| **Depends on** | Your answers to §11 |
| **Can start before** | Phase 2. The registry can be designed and the legacy menu fixed while the new solution is being scaffolded |
| **Risk** | Low structurally. The one real risk is the permission migration: a mis-mapped `FormName` silently changes access. Mitigated by a before/after grant-comparison query that must return zero differences |

**Effort** — 8 to 12 working days, split as: registry and migration 3–4, navigation service and
permission-aware menu 2–3, shell (rail, module page, search, favourites, recent) 3–4, legacy menu
repairs 0.5.

**What it buys.** The 243 KB layout stops being the source of truth for navigation *and*
permissions. Menu, permission catalogue and API policy become one definition. ~15 lost forms come
back. Four wrong links, 22 duplicates, two dead links and three fake subtrees go away. Every
module rebuilt afterwards drops into a structure that already exists.

---

## 10 · Tasks

Added to `PROJECT.md` as `#117`–`#129`.

| # | Task | Est. |
|:--|:--|:--|
| 117 | Current-state navigation audit | done |
| 118 | Target taxonomy and full form mapping — this document | done |
| 119 | **Your review — the gate** | — |
| 120 | Resolve the open questions in §11 | — |
| 121 | Registry schema — `Module`, `NavigationArea`, `NavigationItem` | 1d |
| 122 | Seed the registry from §5, with stable `PermissionKey` values | 1d |
| 123 | Permission migration + zero-difference verification query | 1.5d |
| 124 | Navigation service — claims-filtered module and area tree | 1d |
| 125 | App shell — rail, module page, breadcrumb, My Work | 2d |
| 126 | Global search over the registry | 0.5d |
| 127 | Favourites and Recent, per user | 0.5d |
| 128 | Administration → Configuration index | 0.5d |
| 129 | Retire the menu block in `_Layout.cshtml` | 0.5d |

**Independent legacy repairs** — small, and they fix live defects now:

| # | Task | Est. |
|:--|:--|:--|
| 130 | Fix the wrong link (N5) and the mislabelled item (N6) | 15m |
| 131 | Remove the two dead "Finger Uploader" links (N7) | 10m |
| 132 | Delete the fake `Administration → Reports` subtree and the two empty groups (N4, N9) | 20m |
| 133 | Decide the 15 unreachable forms: restore or retire (N15) | needs 119 |

---

## 11 · Open questions — your call

| # | Question | Why it matters |
|:--|:--|:--|
| **Q1** | `Home/Category` — is this a **plot** category or a **member** category? There is already a separate `Home/MemberCategory` | Decides whether it sits in Property or Parties setup |
| **Q2** | `Home/Almt` (Allotment Type) and `Home/Quota` — attributes of the **plot**, or of the **allocation**? | Property setup vs Sales setup |
| **Q3** | `Operations/COP` — the menu says "Change Of Plot", the codebase says "Change of Particulars". Which is it? | The title users see, and which sub-area it belongs to |
| **Q4** | Demarcation (4 forms) — does it belong with **Construction** or with **NDC & Records**? | It is a survey activity that produces a charge; both are defensible |
| **Q5** | The 15 unreachable forms (N15) — restore each, or retire it? | KYC, Deal Merger, Dealer Reservation and Purchase Request look like real functionality that was simply never linked |
| **Q6** | Six pairs of near-duplicate views (`Home/PropertyBinding` vs `Operations/Propertybinding`, `Home/UOMDef` vs `Home/Unitofmeasure`, `Operations/Transfer` vs `Operations/TransferForm`, `Home/DemarcationCharges` vs `Home/DemarcChargesI`, `GlobalSetup/ChargesGroupForm` vs `…FormTest`, `Home/FingurePrint` vs `FingerPrint/FingerPrint`) — which one of each is live? | I can determine this by reading both, but you will know in seconds |

Q1–Q4 have a working assumption already applied in §5, marked in place. Q5 and Q6 need
investigation or your knowledge before anything is retired.

---

## 12 · What this does not cover

- **Visual design** — colour, typography, spacing, component library. That is `#74` in
  `PROJECT.md` and comes after this structure is agreed.
- **The forms themselves** — this document decides where a form lives, not what it contains.
  Per-form behaviour stays in `docs/modules/<item>.md`.
- **Route scheme** — proposed as `/{module}/{item}` in §6, to be confirmed with `#55`.
