# Observations — `AI Files/AI file.xlsx`

> Source: `AI Files/AI file.xlsx`, read 2026-08-13 · 4 sheets · 163 forms mapped · 5 modules
> Scope of this document: **only** what that workbook asks for. Nothing else from the backlog.
>
> **Updated 2026-08-16 — the workbook grew to 6 sheets and closed every open question. See §12;
> it supersedes §5 (gaps) and §10 (questions) below.**

---

## 0 · What the workbook contains

| Sheet | Holds | Rows with content |
|:--|:--|:--|
| **Restructure** | 13 numbered requirements + an "Others" list + the Land Management sequencing note | 257 |
| **PMS-Modules** | The new structure: every form mapped to Module → Sub-Module → L3 → L4, with its new name. Plus a registry of modules and sub-modules with sequence numbers | 168 |
| **Points** | 17 answers and instructions — this is where you answered my open questions | 16 |
| **UI** | Home screen mock, header strip, and 7 mandatory navigation requirements | 81 |

**The structure you defined**

| | Count | Detail |
|:--|:--|:--|
| Modules | **5** | Estate Operations (named "Property Business Operations" in the workbook; renamed 2026-08-17, D32) · Financial Operations · Records & File Management · Business Analytics · Administration |
| Sub-modules | **19** | 8 under Estate Operations, 5 under Administration, 4 under Business Analytics, 2 under Financial Operations, **0 under Records & File Management** |
| Level-3 groups | **18** | Property Cards, Booking & Allocation, NDC & Transfer Services, Finance Setup, Utility Billing, Meter Operations, System Structure, ERP Integration, … |
| Level-4 groups | **6** | Urban Planning & Development, Charges Configuration, Billing Configuration, Customer Intimations, Plot Identification, Transfer Planning & Scheduling |
| Forms mapped | **163** | of which **58 are renamed** and 105 keep their name |

---

## 1 · The one constraint that governs the whole workbook

**Requirements 6, 11 and 12 — rename Charges→Revenue, rename 58 forms, rebrand N-Stack→N-Stack —
cannot be done safely on the system as it stands today.**

The reason is in `docs/05-MODULE-ARCHITECTURE.md` §1.4 and it is verified, not theoretical:

```
_Layout.cshtml                        Permissions table
──────────────                        ─────────────────
Html.UserHavePermission               FormName    (string)
  ("Charges Group")          ───────► isPermitted (bool)
                                      RolesPermissionsId
        ▲                                    │
        └──── the menu label IS the ─────────┘
              permission key, matched by string
```

`Permissions.FormName` is a **string**, and the layout checks it by literal text.
**Renaming a menu label silently revokes that form for every role.** No error, no warning —
the form simply stops appearing for everyone.

Seven live permission keys contain the word "Charge":

`Charge Setup` · `Charges Group` · `Charges Incorporation Setup` · `Charges Setup` ·
`Charges Type` · `Fixed Charges Bill Generation` · `Surcharge Setup`

Rename those to "Revenue" today and seven forms disappear from every user's menu.
Apply all 58 renames and a large part of the application goes dark.

**Consequence for sequencing:** the form registry (display name separate from permission key)
must exist **before** any renaming happens. This is not a preference — it is the difference
between requirement 11 working and requirement 11 breaking the system. Your own requirement
**8.2 Form Alias / Form Name Configuration** is exactly this mechanism; it has to be built first
and then used to deliver 6, 11 and 12.

> **Softened by `New Instructions.xlsx` (2026-08-14, → D24):** you accepted the risk — the sole
> user is admin with full access during the rephasing. Renames done before the registry exists
> must update the matching `PermissionForms.Name` rows in the same change; the registry stays the
> durable mechanism.

---

## 2 · Where the workbook changes decisions we had already locked

| Locked | Workbook says | Verdict |
|:--|:--|:--|
| **D14** — 12 top-level modules | **5 modules** (PMS-Modules sheet L2:M6) | **Workbook wins.** Points #4 and #7: "I have developed Module structure to follow in Restructuring". `docs/05-MODULE-ARCHITECTURE.md` §5 must be rewritten to your 5-module structure. D14 is superseded |
| **D10 principle 4** — "nothing above two clicks"; metric target *menu depth 2* | Points #3 and #12: forms planned "at most 5th level", "Menu Depth needs to be down to 5th level" | **Workbook wins.** The mapping sheet genuinely uses 5 levels. The 2-click principle is withdrawn; the target metric changes from *depth 2* to *depth ≤ 5, no dead ends* |
| **§2.1** — SAP Fiori model *rejected* | UI!D6 wants My Home sections as "HD cards, look of SAP Fiori"; UI!A120 wants D365 F&O across the whole application | **Both, and they fit.** D365 F&O is the system-wide interaction model; Fiori-style tiles are the My Home landing page only. Recorded as reconciled, not a conflict |
| **§3 shell sketch** — Favourites and Recent in the **left rail** | UI!A77: *"Recent/Frequently Used forms are NOT to be placed inside the left navigation menu"* — header icon instead | **Workbook wins.** My §3 sketch is wrong and must be redrawn |
| **§10 metrics** — "largest view < 200 lines", "largest controller < 10 KB" | Points #14: *"Our object shall not be to lower file sizes and compromise on quality, remove important features and mishandle all the mappings"* | **Correction accepted.** Those two metrics are withdrawn as goals. Size was a *symptom* I was measuring; the real targets are *no business logic in controllers* and *no duplicated markup*. A view stays as long as it needs to be |
| **§10 metrics** — "exception messages returned to clients: 804 → 0" | Points #16: error messages must be understandable — "why logic breaks / what needs to be done" | **Reframed.** Target becomes *0 raw exception texts*, replaced by explained business messages — not 0 messages |
| **D15–D19 + `#140`–`#146`** — REMS launcher is the active item | Restructure!B367: *"we have to get land management system within this solution but this is our last task, we will bother on it after maturing PMS"* | **Workbook wins — this stands the active item down.** See §3 below |
| **D4** — local only, every remote connection severed | Restructure!B69: *"Existing SAP integration should remain operational and should not be disturbed"* | **Both hold.** D4 is about *this repo not phoning production*; requirement 5 is about *the SAP code path staying functional*. `#35` (SAP behind one interface + a fake) delivers both |

---

## 3 · Land Management — the active item is stood down

`PROJECT.md` §4 currently has `#140`–`#146` (the REMS app launcher) as the single in-flight item,
plan locked, waiting on your go. **Restructure!B367 reverses that:** Land Management comes last,
after PMS matures.

But it is not a straight cancellation, because UI!D17–D19 still asks for the launcher region:

> *"APPS (We are planning to develop a launcher in which there would be multiple Apps and we want
> each app's Home page to have link to other Apps)"* —
> `[ Property Management Solution ] [ Land Management Information Solution ] [ Human Resources Management Solution ]`

So the split is:

| Part | When |
|:--|:--|
| The **APPS region on My Home** — tiles linking to the other applications | **Now**, as part of the shell (it is drawn into your own home-screen mock) |
| **Land Management actually integrated / reachable / merged** | **Last**, per B367 |

Two further notes on that tile list:

- A **third application appears — Human Resources Management Solution** — which does not exist in
  either repository. The APPS region must therefore render a configured app as *present*,
  *disabled* or *coming soon*; it cannot assume every tile resolves.
- This vindicates D15 (app list held as **configuration**, not code). The configuration part of
  `#140` is still worth doing now; the Land-specific parts (`#144` permission key, `#145`
  verification against a running Laravel app) move to the end.

---

## 4 · New scope — in the workbook, not previously in the plan

These are real work items with no existing task number.

| # | Requirement | Source | Why it is new |
|:--|:--|:--|:--|
| **N-1** | **Field-level security** — per form, per role: show/hide a field, make it read-only; reached from the form's **top-right Actions/Options → Form Settings**, never the left menu; configurable, not hard-coded | UI!A95–A113 | Nothing in the current system or the plan does this. `Permissions` is form-level only. Needs its own table, its own UI, and **server-side enforcement** — UI!A111 says restricted fields must not be editable through the UI, which means the server must reject them too, not just hide them |
| **N-2** | **Two navigation modes** — Complete Menu (icon + label) and Short Bar (icons only + tooltip), user-switchable, both reading one configuration source | UI!A40–A66 | New. Reinforces the registry: one source, two renderings |
| **N-3** | **Idle-logout overlay** — on inactivity the whole screen blurs, the open form stays as it is, and an in-page re-login panel opens in the same browser window | Points #6 | Today `Program.cs:45` hard-codes `IdleTimeout = 9 hours` and sign-out is a plain redirect that loses the form. Also needs the config form in Administration 8.1 (Restructure!B283) |
| **N-4** | **Audit trail** — complete where possible, capturing Field / Form / System information, **without inflating the database** | Points #6.1 | The tension is real: SQL Server temporal tables (already on Block and others) capture everything but are storage-heavy. Recommend a split — temporal for low-volume master data, an append-only field-change log for high-volume transactions. Needs a decision |
| **N-5** | **Generate Alert** — header icon, opens a small window over the current work screen; on submit it sends, the window closes, a side toast confirms | Restructure!B18, B357 | The Alert *module* exists (`AlertNameController`, `NotificationController`); this header-level send flow does not |
| **N-6** | **Global record search** — NetSuite-style, across 14 named forms, "intelligently identify relevant records and provide direct navigation" | Restructure!B20–B40 | `#126` in the plan searches the *registry* (finds forms). This searches **records** (finds Member #4589). Much larger — needs a search index and a per-entity result contract |
| **N-7** | **ERP-agnostic integration layer** + an API Integration configuration form, with SAP still working | Restructure!B64–B70, B207–B216 | `#35` isolates SAP behind one interface. This adds a *second* requirement: additional ERPs pluggable by configuration — connection string, API config, **field mapping** — with no core code change |
| **N-8** | **Implementation Center** — in-app uploaders organised module-wise and form-wise, plus System Initialization | Restructure!B171–B216 | Changes the shape of `#83`. The 8 console uploader apps were to become one worker host; they must also gain an in-application UI surface, grouped by module and form |
| **N-9** | **Company Profile & Branding form** — company name, legal name, telephone, registration number, logo | Restructure!B196–B205 | Makes branding *data*. This is how requirement 12 (N-Stack → N-Stack) should be delivered permanently, instead of a one-off find-and-replace |
| **N-10** | **Validation inventory** — mandatory fields and other existing controls must be carried across | Points #17 | Nothing captures these today. They live in per-view JavaScript. They must be inventoried per form *before* that form is touched, or they are lost silently |
| **N-11** | **Approval Box in the header** | UI!B4 | New header element beside search, alerts and the user menu |

---

## 5 · Internal gaps and conflicts inside the workbook

These need your word before the structure can be implemented as data.

| # | Finding | Detail |
|:--|:--|:--|
| **G-1** | **Administration is defined twice, differently** | Restructure §8 gives **6** sections: General Settings, System Configuration, Setups, Users & Departments, Authorizations, Approvals. The PMS-Modules sheet gives **5**: System Configuration, Setup - Sub Module, Implementation Center, Organizational Governance, Analytics Development. Column C on the mapping sheet shows the reconciliation you intended — Users & Departments, Authorizations and Approvals all collapse into **Organizational Governance**. But **General Settings has no home in the mapping**, and **Implementation Center and Analytics Development are absent from §8** |
| **G-2** | **Implementation Center — module or sub-module?** | Restructure §9 introduces it as a dedicated area (reads top-level). The mapping sheet places it under Administration (O16, sequence 3). Cannot be both |
| **G-3** | **Business Analytics has no Business Intelligence node** | Restructure §10 specifies BI → Executive Analytics → Pervasive Dashboards / KPIs / Strategic Reports, plus Finance / HR / Operations / Project / Customer Services Analytics. The mapping sheet's Business Analytics has only Performance Insights, Property Operation Reports, Stakeholder Analysis Reports, File & Record Reports. The whole §10 BI tree is unmapped |
| **G-4** | **6 forms have no sub-module** | `Transfer Tax Estimations`, `Clearance` (both Financial Operations), `Record Room File Moving`, `File Location` (both Records & File Management), `Dynamic Report` (Business Analytics), and `Generate Alert` — which has **no module at all**; cell B108 holds a description where the module name should be |
| **G-5** | **Records & File Management has zero sub-modules** | It is one of the 5 top-level modules but holds only 2 forms and defines no structure, while its *reports* sit under Business Analytics → File & Record Reports |
| **G-6** | **"Setup - Sub Module" is a placeholder, not a name** | It is the destination for ~60 setup forms — the single biggest node in the tree — and needs a real user-facing name |
| **G-7** | **~46 forms are unmapped** | The codebase has **209** real forms; the workbook maps **163**. The remainder have no home in the new structure — including 11 of the 15 hidden forms below |
| **G-8** | **Recent/Favourites — the workbook contradicts itself** | UI!A77 is emphatic: *"Important: Recent/Frequently Used forms are NOT to be placed inside the left navigation menu"*, and Restructure!B11–B17 lists them as Home Strip (header) icons. But the UI mock at B7–B12 draws Recents and Favourites **inside** the Navigation Bar column. Two of three say header. **I am proceeding with header** — say so if that is wrong |

---

## 6 · Requirement-by-requirement notes

Only the ones where there is something to say beyond "understood".

**1 · Black & white theme** — Fine, and it composes with D365 F&O (which is itself near-monochrome
with a single accent). Note the interaction with N-1: if colour is removed as a channel, disabled
and read-only field states need a non-colour treatment or field-level security becomes invisible.
The instruction to keep icons *inside* forms and replace only main-menu icons keeps this cheap.

**2 · Global search** — see N-6. The 14 named forms span 6 different entity shapes; each needs a
result contract (what to show in a hit, where the hit navigates to). This is the largest single
item in the workbook after the registry.

**3 · Header alerts** — cheap, the module exists. Header surface only.

**4 · Favourites** — "Add to Favourite" disabled on Home with no form open implies the shell must
know *which registry item is currently open*. That is a property of the registry-driven shell, so
it comes free once the registry exists — and is expensive to bolt on before.

**6 · Charges → Revenue** — see §1. The mapping sheet already applies it: Charges Group → **Revenue
Group**, Charges Type → **Revenue Type**, Charges Setup → **Revenue Rules**, Charges Incorporation
Setup → **Revenue Incorporation Setup**. Two cautions:
- `Surcharge Setup` and `Drawing Scrutiny Charges` contain the string but are **not** the same
  concept. A blind find-and-replace produces "Surrevenue Setup". **Confirmed by
  `New Instructions.xlsx` (→ D25): only the word "Charge" renames; `Surcharge Setup` and
  `Fixed Charges Bill Generation` are excluded by name.**
- The word also appears in **table and column names**. Requirement 11's own rule applies — this is
  a label change; the database stays as it is unless there is a reason.

**8 · Administration restructuring** — see G-1. Also: the session-timeout configuration form
(B283) lands in 8.1 General Settings, which is the section that G-1 leaves homeless.

**11 · Global renaming** — 58 renames. Requirement B282 — *"where an instruction refers to an
existing form by its old name, it should be intelligently interpreted as referring to the renamed
form"* — is an instruction to **me**, and it needs a durable artefact, not memory: an old-name →
new-name table checked into `docs/`. Otherwise it survives exactly as long as one session.

**12 · N-Stack → N-Stack** — measured: **2 live occurrences**, both in
`HRMS_Web/Views/Login/Index.cshtml` (lines 515 and 518, an inline SVG wordmark). Every other match
is a build artefact under `obj/` and `bin/`. So the code change is trivial — but you also have
**~25 logo image files** under `wwwroot/img/`, which is the real work, and which is why N-9
(the branding form) should carry it rather than hard-coding a second wordmark.

**Others → Forms Mapping / Modules Organization / User Permission** — you flagged all three as
"not working good". They are `PermissionSetupController` → `Permission Form` and `Approval UI
Setup`. This is not three broken screens; it is one missing concept. `PermissionForms` is a **flat
table** — `Name`, `Title`, `IsActive`, `SerialNo` — with no parent, no module, no hierarchy. The
forms cannot work because there is nothing for them to organise. Points #2 asks for exactly the
fix: **module-level authorization first, then form-level variation**. Same registry, again.

---

## 7 · Clarifications you asked for — answered

### Points #6.2 — "shall entities continue storing the Block Name as free text? do what is standard practice"

**Answer: no — a foreign key.** Standard practice, and it closes **D9**.

`Block` is a real table. Twenty entities store the block *name* as free text. In `StockCreation` —
the hub of the model — the foreign key **was written and then commented out**. Free text means a
typo creates a new block, renaming a block orphans twenty tables, and no join is reliable.

Migration is the careful part, in this order: add the key column nullable → match existing text to
`Block` rows → report every unmatched value to you (do not guess) → backfill → make it required →
keep the text column read-only for one release as a safety net, then drop it.

`#106` is now decided. `#105` (audit the other ~39 master-data entities for the same trap) still
needs to run, because the same pattern will be there.

### Points #6.3 — "should blocks be scoped to a phase or sector? … Do the needful"

**Answer: yes — scope Block to its parent, and make the uniqueness rule composite.**

Today the block list is **global**, so "Block C" can exist only once across the whole estate.
Your own mapping sheet puts `Block Definition`, `Phase Definition` and `Sector Definition` together
under Property Setup → Urban Planning & Development, which is the same conclusion.

Two things follow, and the second matters more than the rebuild:

1. `Block` gains a parent (Phase or Sector — **one open sub-question: which?** Sector is the
   narrower container and Phase reaches it transitively, so Sector is my recommendation), and
   uniqueness becomes *(parent, block name)* instead of *(block name)*.
2. **This is a live data problem right now, not a rebuild question.** If two phases each have a
   "Block C" today, the current system has them sharing one row. Every plot in both is pointing at
   the same block. That has to be untangled with your knowledge of the estate — I cannot infer it.

`Block` is a **SQL Server temporal table**, so any restructuring must preserve system-versioning or
its history is destroyed (`#46`, R2).

### Points #14 — file sizes

Taken as a correction, and applied in §2 above. I was measuring size as a proxy for "logic in the
wrong place". The proxy was wrong to state as a target. Nothing gets deleted to hit a number, and
no mapping gets simplified away.

### Points #15 — the front end is good

Recorded as a constraint on every module rebuild: **fields, tabs and their links to master forms
are the specification.** A rebuilt form is parity-checked field by field, including the lookups
that fetch from base forms. This makes N-10 (the validation inventory) mandatory, not optional.

### Points #5 and #1 and #11 — hidden and confusing forms

**Done — see §8. All 15 are live on your local instance right now.**

---

## 8 · The hidden forms — located and enabled

> **Review page: `http://localhost:5217/HiddenForms`** — every form below, one click each.
> The application is running now.

Two of the fifteen needed a code change to open at all. Both are marked in place and reverse by
deleting the marked method:

| File | Change |
|:--|:--|
| `HRMS_Web/Controllers/DemandNoteController.cs:25` | `PurchaseRequest()` was **commented out**. Uncommented |
| `HRMS_Web/Controllers/Operations.cs:110` | `TransferForm()` **did not exist**, so a 386-line view was dead. Added |
| `HRMS_Web/Controllers/HiddenFormsController.cs` | **New** — serves the review page. Temporary scaffolding |
| `HRMS_Web/Views/HiddenForms/Index.cshtml` | **New** — the review page itself |

The other thirteen were already reachable by URL; they were simply absent from the menu.
Every route below was probed and returns **HTTP 200**.

### 8.1 · The 15

| # | Form | URL | View file | Size | Note |
|:--|:--|:--|:--|--:|:--|
| 1 | KYC Form | `/Sales/KYCForm` | `Views/Sales/KYCForm.cshtml` | 620 ln | Largest of the set — finished-looking work |
| 2 | Deal Merger | `/Sales/DealMerger` | `Views/Sales/DealMerger.cshtml` | 314 ln | Sits beside the live Deal / Bulk Deal forms |
| 3 | Dealer Reservation | `/Sales/DealerReservation` | `Views/Sales/DealerReservation.cshtml` | 200 ln | |
| 4 | Booking Backlog | `/Sales/BookingBacklog` | `Views/Sales/BookingBackLog.cshtml` | 238 ln | Action spelling differs from the file name |
| 5 | Map Design | `/Home/MapDesign` | `Views/Home/MapDesign.cshtml` | 364 ln | Map *Approval* is in the menu; its two design forms are not |
| 6 | Re-Design | `/Home/ReDesign` | `Views/Home/ReDesign.cshtml` | 643 ln | |
| 7 | Registration NPD | `/Home/RegistrationNPD` | `Views/Home/RegistrationNPD.cshtml` | 115 ln | Small — may be a stub |
| 8 | Purchase Request | `/DemandNote/PurchaseRequest` | `Views/DemandNote/PurchaseRequest.cshtml` | 719 ln | **Action was commented out** |
| 9 | Demarcation Charges | `/Home/DemarcationCharges` | `Views/Home/DemarcationCharges.cshtml` | 217 ln | Pairs with #10 |
| 10 | Demarcation Charges Invoice | `/Home/DemarcChargesI` | `Views/Home/DemarcChargesI.cshtml` | 326 ln | The larger of the pair |
| 11 | Charges Group Form | `/GlobalSetup/ChargesGroupForm` | `Views/GlobalSetup/ChargesGroupForm.cshtml` | 405 ln | **The menu points at `…FormTest` instead** |
| 12 | Unit of Measure | `/Home/Unitofmeasure` | `Views/Home/Unitofmeasure.cshtml` | 356 ln | `Home/UOMDef` is the one in the menu |
| 13 | Admin Dashboard | `/Dashboard/AdminDashboard` | `Views/Dashboard/AdminDashboard.cshtml` | 487 ln | No dashboard link reaches it |
| 14 | Property Binding (Operations) | `/Operations/Propertybinding` | `Views/Operations/Propertybinding.cshtml` | 285 ln | `Home/PropertyBinding` (382 ln) is in the menu |
| 15 | Transfer Form (Operations) | `/Operations/TransferForm` | `Views/Operations/TransferForm.cshtml` | 386 ln | **Had no action at all** |

### 8.2 · Where the confusion lies — 6 near-duplicate pairs

Two views doing one job. One is live, one is a stale copy, and the menu does not always point at
the right one. My reading is in the last column; **your call decides**.

| Pair | Probably live | Probably stale | Reading |
|:--|:--|:--|:--|
| Property Binding | `Home/PropertyBinding` (382 ln) | `Operations/Propertybinding` (285 ln) | Menu points at Home; the Operations copy is smaller — an older draft |
| Unit of Measure | `Home/UOMDef` (358 ln) | `Home/Unitofmeasure` (356 ln) | **Genuinely ambiguous** — 2 lines apart. Needs your eye |
| Transfer | `Operations/Transfer` (2 753 ln) | `Operations/TransferForm` (386 ln) | Size gap is decisive |
| Demarcation charges | `Home/DemarcChargesI` (326 ln) | `Home/DemarcationCharges` (217 ln) | **Neither is in the menu.** Both need a verdict |
| Charges group | `GlobalSetup/ChargesGroupForm` (405 ln) | `GlobalSetup/ChargesGroupFormTest` (215 ln) | **The menu ships the Test one.** Almost certainly backwards — a form named *Test* is in production navigation |
| Fingerprint | `FingerPrint/FingerPrint` (328 ln) | `Home/FingurePrint` (104 ln) | The menu links `Home/FingurePrint` **three times**, and it is the 104-line one |

### 8.3 · The finding that connects this to the workbook

**Eleven of the fifteen have no home in your new module structure.** Absent from the mapping sheet:
KYC, Deal Merger, Dealer Reservation, Booking Backlog, Map Design, Re-Design, Registration NPD,
Purchase Request, Demarcation Charges, Demarcation Charges Invoice, Admin Dashboard.

Points #1 says these forms "require to re-alive as they are meaningful". Whichever ones you keep
therefore need a Module / Sub-Module / Level-3 slot added to the mapping sheet before the registry
is seeded — otherwise they go straight back to being unreachable, this time by design
(§2.3 principle: *a form that is not in the registry does not exist*).

### 8.4 · Also still broken in the live menu

Independent of the hidden forms, and cheap to fix (`#130`–`#132`):

| Defect | Detail |
|:--|:--|
| Wrong target | "Transfer Set Receiving" → `Home/SitePlan` |
| Wrong label | "Drawing Scrutiny Charges" → `Home/DemarcationRequest` |
| Dead links ×2 | "Finger Uploader" → `Uploader/FingerUploader`; that action does not exist |
| Fake subtree | `Administration → Reports` — the *setup* items pasted in, three times over |
| 22 duplicated links | `Home/FingurePrint` ×3, `Sales/LeadGeneration` ×3, 18 more ×2 |

---

## 9 · Step-wise plan

Ordered by the constraint in §1, not by the workbook's numbering. Each stage still ends at your
review before the next begins — the gate in `PROJECT.md` §2 is unchanged.

### Stage A — Close the open decisions *(days, not weeks — mostly already done)*

1. **Hidden forms** — *done today.* All 15 live at `/HiddenForms`. You return a restore/retire
   verdict per form, and a Module/Sub-Module slot for each one you keep.
2. **Confusion pairs** — you name the live one in each of the 6 pairs. The other is retired.
3. **Block** — decided in §7. Sub-question left: parent = **Sector** or **Phase**?
4. **Fill the workbook gaps** — G-1 to G-7. Six answers and the structure is implementable as data.
5. **Rewrite `docs/05-MODULE-ARCHITECTURE.md` §5** to your 5-module structure; retire D14, amend
   D10, amend the §10 metrics per Points #14 and #16.

### Stage B — The registry *(everything else depends on it)*

6. Registry schema: `Module` → `SubModule` → `Level3` → `Level4` → `Form`, **5 levels**, each row
   carrying `SequenceNo`, `Icon`, `DisplayName`, and a **`PermissionKey` that never changes when
   `DisplayName` changes**. This single separation is what makes requirements 6, 11 and 12 safe.
7. Seed it from the mapping sheet — 163 forms, plus whatever Stage A adds.
8. **Zero-difference permission migration**: every user's effective form access before and after
   must be provably identical. A query proves it; nothing proceeds until it returns empty.
9. **Form Alias / Name Configuration** form (requirement 8.2) — the registry's own UI.

### Stage C — Authorization on the registry

10. **Module-level authorization first, then form-level variation** (Points #2, Others → User
    Permission). Replaces the flat `PermissionForms` table.
11. **Field-level security** (N-1) — table, the top-right Actions → Form Settings panel, and
    **server-side enforcement**, because UI!A111 requires that restricted fields cannot be edited
    through the UI at all.
12. Rebuild Forms Mapping, Modules Organization and User Permission on top of it — the three
    screens you flagged as "not working good".

### Stage D — The shell

13. Header strip: Home · Search · Alerts · Approval Box · Favourites · Recent · Generate Alert · user.
    **Recent and Favourites are header icons, not left-menu items** (UI!A77).
14. Navigation: **Complete Menu** and **Short Bar**, switchable, both reading the registry.
    No fixed widths, no hard-coded names — a renamed form must not break the layout (UI!A59–A66).
15. **My Home**: Fiori-style HD cards — Overview Analytics with date filter and period comparison,
    Total Sales / Transfers / NDC Requests, Total Stock (Sold / Registered / Available), Members,
    Dealers, System Users; To-Dos; Recent; Favourites; **APPS region**.
16. D365 F&O design language across theme, layout, forms, actions, grids and dialogs; black & white.
17. Idle-logout overlay (N-3) + its configuration form in Administration → General Settings.

### Stage E — Renaming and branding *(safe only after B)*

18. 58 form renames, driven from the registry. Titles, labels, buttons, reports, messages,
    notifications, breadcrumbs, lookups, linked forms, workflow references — requirement 11's list.
19. Charges → Revenue, with `Surcharge` and `Drawing Scrutiny Charges` excluded by hand.
20. **Old-name → new-name table checked into `docs/`** so requirement B282 survives past one session.
21. Company Profile & Branding form (N-9), then N-Stack → N-Stack through it — including the
    `Views/Login/Index.cshtml` wordmark and the logo assets under `wwwroot/img/`.

### Stage F — Structural moves

22. Financials consolidation — Adjustments, Transfer Tax Estimation, Clearance Setup (requirement 7).
23. Administration restructured per §8, once G-1 and G-2 are answered.
24. Implementation Center — module-wise/form-wise uploaders + System Initialization (N-8).
25. Business Analytics, including the §10 BI tree once G-3 is answered.

### Stage G — Cross-cutting behaviour

26. Audit trail (N-4) — after the storage-strategy decision.
27. Understandable error messages (Points #16) — raw exception text out, explained business
    messages in, with the reason and the remedy.
28. Validation inventory per form (N-10) — captured **before** that form is rebuilt.

### Stage H — Integration and search

29. ERP-agnostic adapter + API Integration configuration form; SAP keeps working throughout (N-7).
30. Global record search across the 14 named forms (N-6).

### Stage I — Land Management

31. Last, per Restructure!B367. The APPS tiles ship in Stage D; the integration behind the Land
    tile happens here.

---

## 10 · What I need from you

| # | Question | Blocks |
|:--|:--|:--|
| 1 | The 15 hidden forms — restore or retire, one verdict each. **Open `/HiddenForms` and go down the list** | Stage A, then the registry seed |
| 2 | The 6 confusion pairs — which one is live? | Same |
| 3 | Block's parent — **Sector** or Phase? | `#106`, `#107`, and the data cleanup |
| 4 | G-1 — Administration: 6 sections (§8) or 5 (mapping sheet)? Where does **General Settings** go? | Stage F |
| 5 | G-2 — Implementation Center: top-level, or under Administration? | Stage B seed |
| 6 | G-3 — is the §10 Business Intelligence tree in scope, given HR / Project / Customer Services analytics have no data behind them yet? | Stage F |
| 7 | G-6 — a real name for **"Setup - Sub Module"** | Stage B seed |
| 8 | G-8 — Recent and Favourites: header icons (my reading), or in the navigation bar as the mock draws them? | Stage D |
| 9 | N-4 — audit trail: temporal tables everywhere (complete, heavy) or the split I recommend? | Stage G |
| 10 | Confirm the REMS launcher (`#140`–`#146`) stands down to Stage I, keeping only the APPS tile region in Stage D | Frees the active slot now |

---

## 11 · Files touched today

| File | Change | Reverse by |
|:--|:--|:--|
| `HRMS_Web/Controllers/DemandNoteController.cs` | `PurchaseRequest()` uncommented, marked in place | Comment it out again |
| `HRMS_Web/Controllers/Operations.cs` | `TransferForm()` added, marked in place | Delete the marked method |
| `HRMS_Web/Controllers/HiddenFormsController.cs` | New — review page controller | Delete the file |
| `HRMS_Web/Views/HiddenForms/Index.cshtml` | New — review page | Delete the file |
| `docs/AI-FILE-OBSERVATIONS.md` | New — this file | — |
| `CURRENT-WORKS.md` | New — the live working sheet | — |

Build: **0 errors**. All 21 routes probed return HTTP 200.

---

## 12 · Workbook update, 2026-08-16 — every question answered, build ordered

The workbook went from 4 sheets to **6**. Three sheets are new — **Instructions** (a 32-section
final directive), **HIdden Forms** (the verdicts), **Folders Structure** (the definitive 4-level
tree with sequence numbers) — and the Points sheet is gone, absorbed into Instructions §29.

**The governing instruction (Instructions §1, §2, §31):** the decisions in the workbook are
final; do not stop for clarification rounds; implement rather than document. The
one-item-at-a-time review gate is retired for this restructuring programme. Requirements that
emerge later extend the architecture — they do not restart analysis.

### 12.1 · The ten §10 questions — all closed

| # | Question | Answer, and where it came from |
|:--|:--|:--|
| 1 | 15 hidden forms | **10 restored with placements** (HIdden Forms sheet): KYC → Stakeholder Management · Deal Merger, Dealer Reservation → Commercial Deal Management · Booking Backlog → Booking & Allocation · Map Design, Unit of Measure → Setup → Property Setup → Urban Planning & Development · Re-Design, Demarcation Charges → Construction & Development · Purchase Request → Financial Operations · Charges Group Form → Setup → Finance Setup, **renamed "Form Wise Charges"**. *"Remove all other hidden forms"* — Registration NPD, Demarcation Charges I, Admin Dashboard, Operations/Propertybinding, Operations/TransferForm leave the scope (kept in code until the final stage per §10 of the Instructions) |
| 2 | 6 confusion pairs | **Both stay available for now**; retired at the final stage after review (Instructions §10). The hidden-form verdicts already settle three de facto: ChargesGroupForm (not the Test one) is the live one; Home/PropertyBinding and Operations/Transfer survive their pairs |
| 3 | Block parent | Contextual scoping confirmed (Instructions §15 — Block C in Phase 5 *and* Phase 6 must both exist). Sector vs Phase left to standard practice — **Sector**, as recommended, reaching Phase transitively |
| 4 | G-1 Administration | **5 sub-modules** (Folders Structure): System Configuration · Setup · Implementation Center · Organizational Governance · Analytics Development. General Settings folds into System Configuration |
| 5 | G-2 Implementation Center | **Under Administration**, sequence 3, with ERP Integration · Credentials & API Configuration · Data Migration Management |
| 6 | G-3 BI tree | **Not in the Folders Structure — out of scope for now.** Business Analytics = Performance Insights · Property Operation Reports · Stakeholder Analysis Reports · File & Record Reports |
| 7 | G-6 "Setup - Sub Module" | Named **Setup**, now structured: 8 Level-3 children — Property Setup, Finance Setup, Stakeholder Management, Utilities Configuration, Litigation Configurations, Alerts & Controls, Violation Configurations, Biometric Configuration |
| 8 | G-8 Recent/Favourites | **In the navigation system only** (Instructions §18) — supersedes my header-icon reading. The My Home workspace cards also show them (UI sheet mock) |
| 9 | N-4 audit trail | Meaningful change capture — entity, field, old/new value, user, time, action — **without inflating the database** (§14). The split strategy stands. Plus an italic audit strip on every applicable form (§13) |
| 10 | REMS launcher | Confirmed last (§23). Only the APPS tile region ships now — delivered on My Home 2026-08-16, list held in `RealEstate:Apps` configuration per D15 |

### 12.2 · New in the Instructions sheet beyond the questions

- **Development order (§30):** 1 Home screen → 2 registry/architecture → 3 navigation →
  4 restore forms → 5 security (permissions, endpoint protection, idle-logout overlay, audit) →
  6 search → 7 database configuration → 8 refinement. §32: start with the Home screen.
- **Map tab (§4):** investigate a PDF / vector / CAD-based plot map with permanent plot markings
  and per-plot detail panels — do not discard the current map before understanding what is possible.
- **Database configuration (§20):** a secure in-app connection-configuration capability, later.
- **Git (§24):** installed — verified live; `#13` closed. Meaningful commits at stable milestones.
- **Registry contract (§5–§6):** stable internal form IDs/keys; display names never used as
  identity; server-side authorization; module → form → action levels (§7). Confirms Stage B–C.
- **Q1–Q6 classifications (§29):** Category = Property Category · Allotment Type/Quota sit at
  stock-creation level · COP = Change of Plot · the four Demarcation forms belong under
  Construction · restore the 15 · keep the near-duplicates.

### 12.3 · Consequences applied

- Stage A of §9 is **closed**. Stage B (registry) now has its authoritative seed source:
  the Folders Structure sheet + the mapping sheet + the hidden-form placements.
- `docs/05-MODULE-ARCHITECTURE.md` §5 rewrite lands with Stage B seeding.
- Phase 1 build started 2026-08-16: My Home workspace tab live — `docs/WORK-LOG.md` §7.
