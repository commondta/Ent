# Block — deep-dive analysis

**Module:** M01 Master data · **Status:** analysis complete, awaiting your review
**Analysed:** 2026-08-03, from source. Nothing here is inferred from a sibling module.

Block is the smallest complete slice in the system, chosen deliberately as the first item: one
entity, a 156-line controller, a 454-line view. If my reading of it is sound, the same reading
applies to the other ~39 master-data forms. If it isn't, we've lost an afternoon rather than a
module.

---

## 1. What it is

A Block is a named subdivision of the housing scheme — the level between a sector and an
individual plot. In N-Stack addressing, a plot is identified as something like *Phase 6, Block C,
Plot 142*. The Block table is the reference list of those names.

It is pure master data: no workflow, no approval, no money. Its entire job is to exist so that
other records can point at it.

---

## 2. The complete surface

| Layer | File | Size |
|---|---|---|
| Page route | `HRMS_Web/Controllers/HomeController.cs:22` — `Block()` returns the view | 3 lines |
| View | `HRMS_Web/Views/Home/Block.cshtml` | 454 lines |
| API | `HRMS_Web/Controllers/api/BlockController.cs` | 156 lines |
| Entity | `B_DB_Model/Block.cs` | 27 lines |
| Mapping | `B_DB_Context/DataBase_Context.cs:22, 363` | 2 lines |

**Four endpoints**, all under `/api/Block/`:

| Method | Route | Purpose |
|---|---|---|
| `POST` | `AddBlock` | Create **and** update — the same endpoint does both, branching on `ID == 0` |
| `GET` | `GetAllBlocks` | List all active blocks |
| `GET` | `GetSingleBlock?id=` | Fetch one for the edit modal |
| `GET` | **`DeleteBlock?id=`** | Soft-delete — **a state change over GET** |

---

## 3. The data model, and one genuine surprise

```csharp
public class Block {
    [Key] public int ID { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }   // the block name
    public DateTime Created_at { get; set; }
    public int? Created_By { get; set; }
    public DateTime Updated_at { get; set; }
    public int? Updated_By { get; set; }
    public bool? is_active { get; set; }
    public bool? is_deleted { get; set; }
}
```

This is the **legacy** naming convention (`ID`, `is_active`, `Created_at`), not the `BaseModel`
convention (`Id`, `IsActive`, `CreatedOn`) used elsewhere in the same context. Both live side by
side, which is the root cause of why no cross-cutting behaviour can be applied to this codebase.

**The surprise, at `DataBase_Context.cs:363`:**

```csharp
modelBuilder.Entity<Block>().ToTable(name: "Block", t => t.IsTemporal());
```

**Block is a SQL Server temporal table.** Every change is already system-versioned into a history
table by the database itself — who-changed-what-when is being captured whether the application
cooperates or not.

This is a genuinely good decision that was made somewhere in this project's history, and it
changes three things in the rebuild:

1. The migration squash **must** preserve system-versioning, or we silently destroy the audit
   history. This is a real trap and I'd have walked into it if I hadn't opened the context file.
2. `is_deleted` as a soft-delete flag is largely redundant against temporal history — but it
   cannot simply be dropped, because 20 other entities join on live rows.
3. Temporal tables constrain schema changes: you cannot alter a system-versioned table freely.
   Any column rename in Phase 3 needs the versioning switched off and back on deliberately.

I want to check the remaining ~39 master-data entities for `IsTemporal()` before we generalise,
because if only some are temporal, that inconsistency matters more than the individual forms.

---

## 4. What actually happens today

### Loading the page

`GET /Home/Block` → `HomeController.Block()` → renders the view.

**`HomeController` carries no `[Authorize]` attribute and no session check.** The page — including
the full 249 KB layout with the complete navigation menu — renders for an anonymous visitor. The
data won't load (the API calls need a bearer token), but the shell, the menu structure and the
form fields are all disclosed. The same controller serves roughly 50 master-data screens.

### Listing

The view calls `GetAllBlocks` on document ready, filters `is_active == true`, and renders rows
into a DataTable client-side. No paging — every block is returned in one payload.

### Creating and editing

Both go through `POST AddBlock`, branching on whether `ID` is zero.

```csharp
var existingList = _db.Blocks
    .Where(x => x.Description == block.Description && x.ID != block.ID && x.is_deleted != true)
    .FirstOrDefault();
```

Duplicate names are rejected — case-insensitively, because SQL Server's default collation is
case-insensitive, so "Block A" and "block a" collide. Whether that's intended, I can't tell from
code.

On create it stamps timestamps, sets `is_active = true`, `is_deleted = false`, and saves.
On update it loads the row and copies across `Description`, `Code` and `Updated_By`.

### Deleting

`DeleteBlock` sets `is_deleted = true` and `is_active = false`.

**But the delete button is commented out of the view** (`Block.cshtml:358`) — the table row only
renders an edit icon. So delete is unreachable through the interface while the endpoint stays
live and callable.

---

## 5. Defects found

Ordered by how much they matter. Every one is verified against the source, not assumed.

| # | Defect | Evidence | Consequence |
|---|---|---|---|
| B-1 | **Audit fields are client-controlled.** `block.Created_By = block.Created_By` is a self-assignment — a literal no-op. The value used is whatever the browser posted. | `BlockController.cs:38-39`; view posts `Created_By: $("#userid").val()` from a hidden input | Any caller can claim to be any user. The audit trail is forgeable, and the temporal history faithfully records the forged value |
| B-2 | **`DeleteBlock` is `[HttpGet]`.** | `BlockController.cs:126-127` | A state change reachable by URL. No CSRF protection, and any prefetcher, crawler or accidental link visit deletes data |
| B-3 | **Null dereference on update and delete.** `FirstOrDefault()` result is used with no null check. | `:51-52` and `:135-136` | A non-existent `ID` throws `NullReferenceException`, caught by the blanket handler, returned to the caller as `ex.Message` |
| B-4 | **No server-side validation.** `Description` is only validated by jQuery in the browser. | `Block.cshtml:165-170`; controller has no checks | A direct API call stores a block with a null or empty name, or a 10 MB one — the column has no length limit |
| B-5 | **No authorization on the page.** `HomeController` has no `[Authorize]`. | `HomeController.cs:7-22` | Anonymous visitors get the application shell and full navigation structure |
| B-6 | **No permission check anywhere.** `[Authorize]` on the API means *authenticated*, nothing more. | `BlockController.cs:13` | Any logged-in user can create, rename or delete blocks regardless of their add/edit/delete grants |
| B-7 | **Uniqueness is a race, not a constraint.** Check-then-insert with no unique index. | `:30-42` | Two concurrent requests both pass the check and both insert. The duplicate is permanent |
| B-8 | **Deleted rows are editable.** The update branch never checks `is_deleted`. | `:51` | A deleted block can be renamed back into existence through the update path |
| B-9 | **`async` with no `await`.** All four methods are declared `async` and contain no asynchronous call; `SaveChanges()` is synchronous. | throughout | Thread-pool threads block on I/O. This is the pattern in 667 places across the codebase |
| B-10 | **Exception messages returned to the client.** | `:72, 96, 120, 148` | Leaks schema and internals. One of 804 such sites |
| B-11 | **Every response is HTTP 200**, including failures, with success encoded as `code === 0`. | `Response_Result` usage throughout | Monitoring, retries and clients can't distinguish success from failure |
| B-12 | **`Code` is dead.** Never set on create — only on update — never sent by the form, and its table column is commented out. | `:54` vs `:34-42`; `Block.cshtml:55, 101-105` | Every row has `Code = null`. It's either a dropped requirement or a field someone still expects |
| B-13 | **A guard that never fires.** `if ($("#Code").val() !== "" && ...)` — there is no `#Code` element, so this is `undefined !== ""`, always true. | `Block.cshtml:250` | The client-side guard is decorative |
| B-14 | **Synchronous AJAX** (`async: false`) on all four calls. | `Block.cshtml:220, 275, 341, 431` | Freezes the browser during every request. Deprecated, and removed in some browsers |
| B-15 | **Handler accumulation.** `$("#BlockForm").submit(function(e){ e.preventDefault(); })` inside success callbacks *binds a new handler* each time rather than suppressing anything. | `:284, 305, 322, 438, 444` | Handlers pile up across a session; the intent was almost certainly `return false` at the point of submit |
| B-16 | **Full page reload after every save.** `location.reload(true)` | `:297` | The list refresh already exists (`GetAllBlocks`) and is never used after a save |

Sixteen defects in what is, by a wide margin, the simplest screen in the application. Not because
the work was careless — the shape is consistent and someone clearly established it deliberately —
but because **the pattern itself carries the defects, and the pattern was replicated about forty
times.**

That is the actual finding here. Fixing Block is worth little. Fixing the *pattern* fixes forty
screens at once.

---

## 6. The structural problem: Block is barely a foreign key

I checked every reference to Block across the domain model. This is the most important thing in
this document.

**Three entities reference it properly:**
`Banner.BlockId` · `GlobalChargeSetup.BlockId` · `PaymentPlanSetup.BlockId`

**Twenty entities store the block as free text instead:**
`StockCreation.Block` · `Deal.Block` · `BulkDeal.Block` · `PropertyList.Block` · `SitePlan.Block` ·
`SpPropertyDto.Block` · `GenralAdjustment.Block` · `StandAlone.Block` · `FileLocationAssigment.Block` ·
`FileReceivingRegister.Block` · `FileVerificationRequest.Block` · `FileDocDupRequest.Block` ·
`NDCRequestForMember.Block` · `TransferSetReceiving.Block` · `ClientFileVerification.BlockName` ·
`TransferReceiptProcessing.BlockName` · `PaymentPlanSetup.BlockName` · `COPHistery.CurrentPropertyBlock`
and `.ProposedPropertyBlock` · `RenumberHistery.CurrentPropertyBlock` and `.ProposedPropertyBlock`

And most tellingly, in `StockCreation.cs:38-42` — the hub entity of the entire system:

```csharp
//[ForeignKey("BlockID")]
//public int? BlockID { get; set; }
//public Block? Block { get; set; }

public string? Block { get; set; }
```

**The foreign key was written, then commented out and replaced with a string.** Someone made that
change deliberately.

### What this means

- Renaming a block in this form **changes nothing anywhere else**. Every plot, deal, file and
  transfer keeps the old text. The master list and the data drift apart silently.
- There is no referential integrity. A typo in any of those twenty places creates a block that
  doesn't exist, and nothing complains.
- The uniqueness rule in this controller protects a list that most of the system doesn't consult.

I can see *what* was done. I cannot see *why* from the code, and this is exactly the kind of
decision where guessing would be expensive. Three explanations fit the evidence:

1. **Historical accuracy was wanted** — a transfer record from 2023 should show the block name as
   it was in 2023, even if renamed since. If so, the string is correct and should stay, and the
   temporal table exists for the same reason.
2. **The join was too slow or too awkward**, and denormalising was a performance fix.
3. **It was expedient** under deadline and never revisited.

**These lead to three completely different rebuilds.** If (1), we keep the denormalised value and
add a proper FK alongside it — value plus reference, which is standard practice for historical
records. If (2) or (3), we normalise and backfill, and every one of those twenty fields becomes a
real relationship.

**This is my first real question for you, and I'd rather ask than assume.**

---

## 7. Feasibility

**Rebuilding Block itself: trivial.** A day, including tests. No workflow, no approval, no money,
no SAP, four endpoints.

**The dependencies are what matter:**

| Dependency | State | Effect |
|---|---|---|
| Solution skeleton (Phase 2) | Not started | Blocks any code — there is nowhere to put it yet |
| Permission policies (Phase 4) | Not started | B-6 can't be fixed properly without them |
| Local database | Not created | Can't run or verify anything against real data |
| The FK-versus-string decision | **Needs you** | Determines whether this is a 1-day or a 4-day task |

**Risk: low, with one exception.** The temporal table (§3) means a careless migration destroys
audit history. That's the one thing here that is genuinely hard to undo.

### Recommendation

**Build Block as the reference implementation — but not yet.** Two things should happen first:

1. **Phase 0 safety work**, which is independent and urgent.
2. **The Phase 2 skeleton**, so there's somewhere for the code to live.

Then Block becomes the first slice built, and it establishes — concretely, in code you can read —
the vertical slice layout, validation, permission enforcement, the `Result<T>` return, the tests,
and the Razor components. Every subsequent master-data form is then a copy of a *good* pattern
instead of a copy of the current one.

**What I recommend against:** patching these sixteen defects in the legacy controller. It's a day's
work that has to be thrown away, and it would need repeating forty times. The two exceptions worth
fixing in place are **B-2** (delete over GET) and **B-5** (the anonymous page), because they're
security issues, they're two-line changes, and they apply to all ~50 screens `HomeController`
serves.

---

## 8. Target design

```
src/Pms.Domain/MasterData/Block.cs                    entity + invariants
src/Pms.Application/MasterData/Blocks/
    CreateBlock.cs  UpdateBlock.cs  DeleteBlock.cs    commands + validators
    GetBlocks.cs    GetBlock.cs                       queries
src/Pms.Infrastructure/Configurations/BlockConfiguration.cs   mapping, temporal, unique index
src/Pms.Web/Areas/MasterData/Controllers/BlocksController.cs  5 real REST endpoints
src/Pms.Web/Views/MasterData/Blocks/Index.cshtml              ~40 lines, using shared components
tests/Pms.Application.Tests/MasterData/BlockTests.cs
```

Endpoints become honest REST with real status codes:

| Now | Becomes |
|---|---|
| `POST AddBlock` (create *and* update) | `POST /api/v1/blocks` → 201 · `PUT /api/v1/blocks/{id}` → 204 |
| `GET GetAllBlocks` | `GET /api/v1/blocks?page=&size=` → 200, paged |
| `GET GetSingleBlock?id=` | `GET /api/v1/blocks/{id}` → 200 / 404 |
| `GET DeleteBlock?id=` | `DELETE /api/v1/blocks/{id}` → 204 / 404 |

Fixes applied by construction: audit fields from the authenticated principal, never the request
body (B-1) · `DELETE` verb with anti-forgery (B-2) · 404 instead of a null dereference (B-3) ·
FluentValidation with a length limit (B-4) · authorization on the page (B-5) · `RequirePermission`
on every endpoint (B-6) · a unique index doing the real work (B-7) · deleted rows excluded by a
global query filter (B-8) · genuinely async (B-9) · `ProblemDetails` (B-10, B-11) · and the view
rebuilt on shared components, killing B-13 through B-16 outright.

`Code` (B-12) I will not decide unilaterally — see §10.

---

## 9. Task breakdown

Added to `PROJECT.md` as rows 105–112. Nothing here starts until you say go.

| # | Task | Est. |
|---|---|---|
| 105 | Audit the other ~39 master-data entities for `IsTemporal()` and for FK-versus-string | 2h |
| 106 | Decide FK-versus-string with you, and record the decision | — |
| 107 | `Block` entity, configuration, unique index, temporal preserved | 2h |
| 108 | Commands, queries, validators, `Result<T>` | 3h |
| 109 | REST controller with permission policies | 2h |
| 110 | Razor view on shared components (~40 lines, replacing 454) | 3h |
| 111 | Tests — unit, integration, endpoint authorization | 3h |
| 112 | Write it up as **the** master-data pattern for the remaining ~39 forms | 2h |

Two small security fixes worth doing to the legacy app now, independent of all of the above:

| # | Task | Est. |
|---|---|---|
| 113 | `DeleteBlock` → `[HttpPost]` (B-2) | 15m |
| 114 | `[Authorize]` on `HomeController` — covers ~50 screens (B-5) | 15m |

---

## 10. What I could not determine from the code

Where I'd be guessing, I'm asking instead.

1. **Should Block be a real foreign key, or stay free text?** (§6) The biggest question here, and
   it changes twenty entities.
2. **What is `Code` for?** Every row has it null. Was it dropped, or is it a requirement that was
   never finished?
3. **Should blocks be scoped?** There is no link to Phase, Sector or Project — the list is global.
   Real N-Stack addressing has Block C in Phase 5 *and* Phase 6. If those are meant to be distinct
   rows, today's global uniqueness rule is actively wrong.
4. **Is delete meant to exist?** The button is commented out but the endpoint is live. Deliberate,
   or an unfinished change?
5. **Is case-insensitive matching intended?** "Block A" and "block a" currently collide.

Question 3 is the one I'd most like answered, because if blocks are meant to be phase-scoped, that
is a live data-quality problem right now, not just a rebuild question.
