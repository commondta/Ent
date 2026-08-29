# PMS — Enterprise Readiness Verdict

> Written 2026-08-14, synthesised from every document in `docs/` and from four sessions of
> direct measurement of the codebase. This is a judgement, not a summary — where the other
> documents record what *is*, this one answers a single question:
>
> **Can this rebuild reach enterprise level?**

> **Update 2026-08-16** — of the five process risks this verdict names, **git is resolved**
> (installed, repo live on `Dhafeature/dev` with a GitHub remote, milestone commits since).
> Gate erosion is also re-framed: the review gate was retired deliberately by the workbook's
> final Instructions sheet (D26), not eroded. Still standing: single machine, schema drift
> (`#136`), unrotated credentials.

---

## 1 · The verdict

**Yes — conditionally.** And the condition is not the one you might expect.

The domain does not need to *become* enterprise-grade. It already **is** enterprise-scale and
carries enterprise load in production today: 439 tables, 205 system-versioned audit tables, a
configurable multi-stage approval engine driving ~30 request types, live SAP Business One ERP
integration, ~60 business processes covering the full property lifecycle including the hard
parts most systems skip — amalgamation, repurchase, joint ownership, soft-lock vetoes,
re-numbering. Real members, real money, real legal cases run through this system every day.

What is **not** enterprise-grade is the engineering wrapped around that domain — and, more
decisively, the **delivery process** wrapped around the engineering. The technical defects are
all catalogued, all verified, and all covered by the plan in `03-REENGINEERING-PLAN.md`. The
process gaps are covered by nothing, and they are the ones that can sink the rebuild.

So the honest formulation is:

| Question | Answer |
|:--|:--|
| Is the domain worth enterprise treatment? | It already demands it — it is in production |
| Is the target architecture enterprise-grade? | Yes — see §3 |
| Does the plan close the technical gap? | Yes, if executed as written — see §4 |
| What can actually stop it? | Five process risks, none of them code — see §5 |
| Overall | **Achievable. The threats are procedural, and every one is fixable cheaply** |

---

## 2 · Scorecard — today, after the plan, against the enterprise bar

Grades are mine, from verified evidence, not aspiration. "Plan" = the state after
`03-REENGINEERING-PLAN.md` and the workbook stages complete as written.

| Dimension | Today | After plan | Enterprise bar met? | Decisive evidence |
|:--|:--:|:--:|:--:|:--|
| **Security** | F | B+ | ✅ | Today: two unauthenticated arbitrary-SQL endpoints, app runs as `sa`, committed secrets, client-side-only authorization. Plan: policy-based server enforcement, one auth scheme, secrets out of source, `Result<T>`/ProblemDetails. Conditional on credential **rotation**, not just removal — the leaked values are in git history |
| **Architecture** | D | A– | ✅ | Today: no layering, 275 KB controllers, business rules living in action methods. Plan: Domain/Application/Infrastructure split, SAP isolated in the only COM assembly, vertical slices. This is a textbook-correct enterprise shape |
| **Data integrity** | D+ | A– | ✅ | Today: no transactions across multi-step operations, `SaveChanges()` inside loops, last-write-wins, two base-entity conventions blocking all cross-cutting behaviour. Plan: unit-of-work, `RowVersion` concurrency, global soft-delete filters, UTC via `IClock`. The 205 temporal tables are already an enterprise-grade audit substrate — the plan's job is to not destroy them, which the blank-database work this week proved is a live risk *and* proved manageable |
| **Testing** | F (0 tests) | B | ⚠️ conditional | The plan's test pyramid is right. But a full rewrite of an untested 58k-line system is only safe if **Phase 1 behaviour capture is enforced as a hard gate** — the plan says so itself. Billing formulas need worked examples *before* code. This is the largest single risk to correctness |
| **Delivery** | F | B+ | ⚠️ conditional | Today: **no git**, build was machine- and toolchain-locked (mitigated), no CI, no artifacts, no repeatable deploy. Plan: clean `dotnet build` everywhere, GitHub Actions. All of it is gated on installing git — see §5, condition 1 |
| **Operability** | F | C+ | ❌ not yet | Today: 4 `ILogger` calls in 58k lines, every failure is HTTP 200, no monitoring can see anything. Plan: Serilog + correlation ids + real status codes — good, but **no doc yet covers backups, restore drills, monitoring/alerting, or a DR position**. For a system of record holding financial data, this is a required chapter that does not exist. See §6 |
| **Compliance & audit** | C+ | A– | ✅ | Temporal tables already answer who-changed-what-when at the database layer for 205 tables — better than most enterprise systems start with. B-1 (client-controlled audit fields) currently poisons it; the plan fixes stamping server-side. N-4 (audit strategy decision) closes the remainder |
| **Documentation & governance** | **A** | A | ✅ exceeds | Worth stating plainly: the `docs/` discipline — measured claims, per-form deep dives, decision records, reversal instructions on every change, a work log substituting for missing git — is **above** what most enterprise teams maintain. This is the project's strongest asset after the domain itself |
| **Continuity** | **F** | F (unaddressed) | ❌ | One person, one machine, no version control, no off-machine copy. **This week's laptop restart destroyed in-flight work** — the risk is not theoretical, it happened on 2026-08-13. Nothing in any plan addresses it. See §5 |

**Reading:** every technical row lands at B+ or better under the existing plan. Both rows that
stay red are process rows the plan never claims to cover. That is exactly where attention
belongs.

---

## 3 · Are the locked bets sound at enterprise scale?

The five structural bets, judged independently:

| Bet | Verdict | Reasoning |
|:--|:--|:--|
| **Full rewrite** (reusing domain model + approval engine) | Defensible, highest-risk | The assessment itself says why: zero tests, rules buried in 275 KB controllers. The mitigation — Phase 1 behaviour capture, per-module parity checks, one module at a time so there is never a "half-migrated, nothing works" state — is the correct enterprise-grade mitigation. It works only if the gate is never skipped under schedule pressure |
| **.NET 10 LTS, server-rendered Razor, no SPA** | Sound | For an internal line-of-business system with ~200 forms, server-rendered with a component structure is what Dynamics-class products effectively are. It avoids a second front-end platform, a second build chain, and a second skills requirement. Supported to Nov 2028. Enterprise-appropriate, not merely acceptable |
| **Registry-driven shell** (menu = permissions = API policy, one data source) | The single best decision in the plan | It eliminates the string-matching coupling that makes the current system unrenamable, and it is the *precondition* for the workbook's 58 renames. This is the pattern enterprise platforms (D365, NetSuite) actually use. The zero-difference permission migration check is the right safety mechanism |
| **5 modules / 5 levels** (workbook, supersedes 12/2-click) | Sound, with one caveat | Deeper navigation is fine — D365 F&O itself nests deeply. The caveat: 46 of 209 real forms are unmapped in the workbook and 11 restored hidden forms have no slot. The registry principle "a form not in the registry does not exist" turns every unmapped form into a deliberately dead one. The G-1…G-8 answers are therefore not paperwork; they are scope |
| **SAP behind one gateway, ERP-agnostic later (N-7)** | Sound and proven | The build-switch already proved the isolation works — `dotnet build` is real again. N-7's config-driven field mapping is ambitious but correctly layered on top of, not instead of, the gateway |

No locked decision needs reversing. That is rarer than it sounds.

---

## 4 · What "enterprise level" means here, concretely

Not a slogan — a checklist this project can be measured against. Items already guaranteed by
the written plan are marked ✅; items that exist nowhere yet are marked ⬜.

**Guaranteed by the current plan, if executed:**

- ✅ Server-side authorization on every endpoint, derived from one registry
- ✅ Transactions around every multi-entity business operation; optimistic concurrency
- ✅ Structured logging with correlation ids; real HTTP semantics; no leaked exception text
- ✅ Reproducible build from a clean clone on any machine; CI on every push
- ✅ One reproducible schema baseline (the blank `.bak` + squashed migrations, diffed against live — `#136`)
- ✅ Test pyramid with the deepest suites on the approval engine and billing
- ✅ Full audit trail: temporal tables + server-stamped actor identity
- ✅ Secrets out of source; local no-op adapters for every external service

**Required for the enterprise claim, currently owned by no document:**

- ⬜ Version control with an off-machine remote *(condition 1, §5)*
- ⬜ A second environment — even one more machine acting as test/staging — and a deployment
  that is a procedure, not an event
- ⬜ Backup and restore **drills** for the database and the repository *(the restore half was
  involuntarily rehearsed this week; it should become deliberate)*
- ⬜ A data-migration and cutover rehearsal plan with rollback criteria *(Phase 9 names it in
  one line; for 26.5 GB of live financial data it needs its own document)*
- ⬜ Defined performance targets *(even two numbers: expected concurrent users, acceptable
  p95 form-load — nothing is currently stated, so "fast enough" is unfalsifiable)*
- ⬜ Credential rotation at the source systems for every leaked secret
- ⬜ A named bus-factor answer: where a second person could pick this up from, and what they
  would read first *(the docs/ discipline means the answer is nearly free — it just has to be
  stated and the repository has to exist somewhere other than one laptop)*

---

## 5 · The five risks that can actually stop this — none of them code

Ordered by how cheap the fix is versus how much it protects.

**1 · No version control.** *(`#13` — open since session 1)*
Disqualifying for the enterprise claim on its own, and it gates CI, the repository merge
(`#147`, ADR-004), and any second contributor. `WORK-LOG.md` is a heroic substitute and still a
substitute. The fix costs one download and one `git init`, plus a private remote so the history
leaves this laptop. **Everything else in this document matters less than this item.**

**2 · Single machine, demonstrated data-loss event.**
On 2026-08-13 a restart destroyed an in-flight 310 GB operation. It was recoverable because
the artefacts were scripted and re-runnable — which is exactly the point: *the discipline
already practised here is the mitigation; it needs to extend to the repository itself.* An
off-machine copy of `PMS/` (and of `F:\DHA_Blank_Structure.bak`) converts a catastrophic risk
into an inconvenience.

**3 · Rewrite-without-tests, if the gate erodes.**
The one-module-at-a-time gate with behaviour capture is the plan's own de-risking mechanism.
The threat is schedule pressure quietly turning "spec → tests → build → parity check" into
"build". The billing module (M10) is where a silent error costs real money; its worked-example
requirement must be treated as non-negotiable.

**4 · The live schema is not what the code says it is.** *(D5b)*
235 tables were hand-patched in production outside the migration history. The blank-schema
`.bak` produced this week is the faithful target; the EF model is not. Any migration squash or
data cutover that trusts the migrations will silently drop hand-added columns. `#136` (diff
live vs model vs migrations, then a corrective baseline) must complete before Phase 3 touches
persistence.

**5 · Unrotated credentials.**
The `sa` password, JWT signing keys, Cloudinary and SMS credentials are in git history and in
this document trail. Removing them from files going forward does not un-leak them. Rotation
happens at the source systems and is outside what any code change can deliver.

---

## 6 · What the blank-schema work this week contributes

Worth recording because it quietly closed two enterprise-checklist items:

- **A reproducible, production-faithful schema artefact** — `F:\DHA_Blank_Structure.bak`,
  12.9 MB, restores in ~1 second, verified to contain all 439 tables, 205 temporal pairings,
  193 FKs, 632 indexes and 48 procedures with zero rows. This is what every new environment,
  CI database, and migration rehearsal starts from. Before this week, no clean environment
  could be built from the repository at all (D5b).
- **A rehearsed restore path** — the scripts are re-runnable, resumable after failure, and
  gated against protected databases. That is the seed of the backup/restore drill practice
  §4 asks for.

The local app now runs against that faithful schema (`PMS_Blank`), which means parity work in
Phase 1 can compare behaviour against the *real* structure, not the drifted EF model.

---

## 7 · Sequenced recommendation

Nothing here changes the agreed plan; it adds the process floor underneath it.

| When | Action | Cost | Buys |
|:--|:--|:--|:--|
| **This week** | Install git; `git init` both repos; push to a private remote | Hours | Removes the single largest disqualifier; unblocks `#147`, CI, and continuity |
| **This week** | Copy `F:\DHA_Blank_Structure.bak` and the repo off this machine | Minutes | Survives the next restart/disk failure |
| **Stage A (now)** | Answer G-1…G-8 and the hidden-form verdicts | Your time | Turns the 5-module structure from a spreadsheet into implementable data; closes the 46-form scope hole |
| **Before Phase 3** | Run `#136` — three-way schema diff, corrective baseline | 1–2 days | Prevents silent column loss in the squash and the cutover |
| **Before M10** | Billing formulas as worked examples, signed off | Your time | The single highest-value correctness investment in the project |
| **Before cutover** | Write the missing operations chapter: backup/restore drill, second environment, migration rehearsal with rollback criteria, two performance targets | 2–3 days of writing | The remaining ⬜ items in §4 |
| **At the source systems** | Rotate every leaked credential | Admin effort | Closes the only security hole no rewrite can close |

---

## 8 · Closing judgement

This project has an unusual profile: the **domain** is stronger than most enterprise systems
(a complete property lifecycle with a genuinely good approval engine and database-level audit
history), the **documentation discipline** is stronger than most enterprise teams, and the
**delivery process** is weaker than a student project — no version control, one machine, one
person.

That combination is good news, because process is the cheapest of the three to fix and the
other two are the expensive ones you already have. The technical plan needs no rescue; it
needs its gates respected and a floor of ordinary operational hygiene put underneath it.

**Enterprise level is reachable from here. The path is the one already written down — plus
git, plus a second copy of everything, plus the operations chapter no one has written yet.**
