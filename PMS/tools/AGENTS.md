# tools

## Purpose

Scripts that build and reset the **local development environment**. Nothing here ships with the
application or is part of the rebuild, and nothing here connects to a live server — a *restored
local copy* of a production backup is in scope, the production instance itself never is.

## Ownership

Owned by this doc. Parent: root `AGENTS.md`.

## Local Contracts

- `local-run/` holds everything needed to recreate the local environment from an empty SQL Server.
  The sequence, and how to reverse it, is in `docs/WORK-LOG.md`.
- **These scripts are throwaway.** They are generated output, kept so the environment is
  reproducible while there is no version control. They are not maintained as products.
- `seed-local.sql` and `patch-schema-drift.sql` were generated **from the state of the repository
  on 2026-08-04**. If migrations or `_Layout.cshtml` change, regenerate them rather than editing
  them by hand — a hand-edited copy stops matching what the code expects and is worse than no copy.
- `seed-local.sql` is the only way to get a login into a freshly blanked database — zero rows means
  no row in `PMSUser`. It touches only `PMSUser`, `PermissionForms` and `UserPermissionMapping`, and
  those three are identical between `PMS_Local` and the blank production schema, so it applies to
  both. Run it with **`sqlcmd -I`** against a blank-schema database or its inserts fail with
  error 1934.
- `HRMS_Web.csproj.original` is a reversal artifact, not a template. Do not build from it.
- `legacy-menu-tree.txt` is parsed evidence behind the navigation defects in
  `docs/05-MODULE-ARCHITECTURE.md` §1.3. Read-only.
- `promote-construction-utilities-modules.sql` (2026-08-23) moves the two navigation groups
  `Construction & Development` and `Utilities Management` to the root of `NavigationNodes` as modules and
  recomputes depths — idempotent, run against the live registry DB (`PMS_Blank`); the seed JSON carries
  the same structure for fresh installs.
- `blank-database.sql` + `restore-and-blank.ps1` turn a full backup into a **structure-only**
  backup: restore, empty every table, shrink, re-backup. Unlike the rest of this folder these two
  are written to be re-run and re-read, not regenerated. They **only ever read** the source `.bak`.
  Both carry a safety gate that refuses to run against `master`, `model`, `msdb`, `tempdb`,
  `DHA_Live`, `PMS_Local` or `test_dha_land_management`; extend that list, never shorten it.
  Current output: `F:\DHA_Blank_Structure.bak`, 12.9 MB from a 26.5 GB source.
- `blank-database.sql` truncates rather than deletes, so it must drop and recreate every foreign
  key and disable system versioning — if it is ever edited, keep the recreate half in step with
  the drop half or the schema comes back incomplete. Both halves' state lives in
  `dbo.__BlankState_TemporalPairs` and `dbo.__BlankState_ForeignKeys`, **real tables inside the
  database being blanked**, not temp tables. They must stay real: once system versioning is off,
  `sys.tables.history_table_id` is `NULL` and the history-table pairing exists nowhere else, so
  state that dies with the session makes a failed run unrecoverable. They are captured once,
  reused by a re-run, excluded from the truncate loop, and dropped only after step 8 confirms the
  foreign-key and temporal counts came back and every table is empty.
- **A failed blanking run is resumable — run the script again.** Never drop the
  `__BlankState_*` tables by hand; they are the only record of how to rebuild the schema.
- `restore-and-blank.ps1` drops any existing copy of `-TargetDb` before checking free space.
  `RESTORE` does not reuse the files of the database it replaces — it requires the full size free
  again — so without the drop no re-run can ever fit. The protected-name gate is what keeps an
  unconditional drop safe; do not remove one without the other.
- **Encoding is load-bearing in this pair.** `restore-and-blank.ps1` must stay **UTF-8 with BOM**:
  PowerShell 5.1 reads a BOM-less file as ANSI, and a non-ASCII character inside a double-quoted
  string then breaks parsing. `blank-database.sql` is deliberately **ASCII-only** and should stay
  that way. Both invoke sqlcmd with `-I`; this database has indexed views and filtered indexes,
  and sqlcmd defaults `QUOTED_IDENTIFIER` to OFF, which fails every DML with error 1934.

## Work Guidance

- **Never put a credential in this folder.** The original `appsettings.json` is deliberately kept
  out of the repository for that reason; see `docs/WORK-LOG.md` §2.1.
- Anything that must survive into the rebuilt solution belongs in the new solution, not here.
- `patch-schema-drift.sql` is a **local workaround**, not the fix. The real fix is `#136`.

## Verification

No automated checks exist yet. A script is correct if, from an empty SQL Server, the sequence in
`docs/WORK-LOG.md` produces an application that signs in as `admin` and renders `Home/Index`.

## Child DOX Index

No child AGENTS.md files. `local-run/` is covered by this doc.
