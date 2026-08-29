# 05 — Architecture Decision Records

Format: MADR. Each record states the context, the alternatives genuinely
considered, the decision, and its consequences — including the negative ones.

| ADR | Decision | Status | Date |
|---|---|---|---|
| [ADR-001](#adr-001) | Migrate the database to Microsoft SQL Server | Accepted | 2026-08-12 |
| [ADR-002](#adr-002) | Run Laravel 8 on PHP 8.5 with deprecations suppressed | Accepted | 2026-08-12 |
| [ADR-003](#adr-003) | Map `tinyint(1)` to `tinyint`, not `bit` | Accepted | 2026-08-12 |
| [ADR-004](#adr-004) | Keep project root as document root; add a router | Accepted | 2026-08-12 |
| [ADR-005](#adr-005) | Emit filtered unique indexes | Accepted | 2026-08-12 |
| [ADR-006](#adr-006) | Use a dedicated SQL login, not Windows auth | Accepted | 2026-08-12 |
| [ADR-007](#adr-007) | Import from the dump rather than run migrations | Accepted | 2026-08-12 |

---

## ADR-001

### Migrate the database to Microsoft SQL Server

**Status:** Accepted · 2026-08-12

#### Context

The application was written against MySQL/MariaDB and its only data was a
phpMyAdmin dump from MariaDB 10.4. No MySQL server was installed on this
workstation. A database engine had to be chosen before the app could run.

Discovery then established two decisive facts:

1. **SQL Server 2019 Standard was already installed and running** on this
   machine — default instance, TCP 1433, Mixed Mode auth, ODBC 17 and 18.
2. **The application contains no raw SQL.** `app/` has zero occurrences of
   `DB::raw`, `whereRaw`, `selectRaw`, `DB::select`, `DB::statement`, or
   MySQL-specific functions, and no `groupBy`/`having`. It is pure Eloquent.

Fact 2 is what made this decision cheap: Eloquent generates dialect-appropriate
SQL, so the port was confined to schema and data.

#### Options considered

| Option | Assessment |
|---|---|
| **Install MariaDB** | Lowest-friction restore — the dump is native. But it adds a second database engine to a machine that already runs SQL Server, for no lasting benefit. |
| **Install MySQL 8.4** | Same as above, plus stricter SQL modes that create import friction with a MariaDB-authored dump. |
| **Use the existing SQL Server** | No new server software. One-time schema and data conversion. Aligns with the platform already present. |
| **Switch to SQLite** | Trivial locally, but diverges sharply from any production target and would not exercise the same SQL the app will really run. |

#### Decision

Convert to **Microsoft SQL Server**, using the already-installed 2019 instance.

The user directed this choice explicitly ("convert mysql to microsoft sql and go
with it"). Discovery independently supported it: the server was already there,
and the absence of raw SQL made the port low-risk.

#### Consequences

**Positive**
- No new database server installed; one engine on the machine.
- Zero application-code changes were required — confirmed by testing.
- A reusable converter (`database/mysql2mssql.php`) now exists for future dumps.

**Negative**
- The migration files remain MySQL-dialect, so `migrate:fresh` no longer
  reproduces the schema (ADR-007, risk R1).
- The team loses phpMyAdmin as a data-browsing tool; SSMS or `sqlcmd` replaces it.
- Any future contributor writing raw MySQL SQL will now break the application —
  the codebase's dialect neutrality became load-bearing.

**Risks**
- Subtle semantic differences (NULL handling in unique indexes, string
  comparison collation) are addressed in ADR-005 but could surface elsewhere.
  The database collation is `Latin1_General_CI_AS` — case-insensitive, matching
  MySQL's `utf8mb4_unicode_ci` behaviour for equality, though not identical for
  ordering of non-Latin text.

---

## ADR-002

### Run Laravel 8 on PHP 8.5 with deprecations suppressed

**Status:** Accepted · 2026-08-12

#### Context

The installed runtime is PHP 8.5.9. The application is Laravel 8.83.27, whose
`composer.json` declares `"php": "^7.3|^8.0"`. Laravel 8 predates PHP 8.5 by
several major versions and its dependencies (Symfony, Monolog, PsySH, Dotenv)
emit "Implicitly marking parameter as nullable" deprecations at autoload time —
*before* Laravel installs its own error handler, so they land directly in the
HTTP response body.

#### Options considered

| Option | Assessment |
|---|---|
| **Downgrade PHP to 8.1** | Best dialect match for Laravel 8. But PHP is already installed and configured at 8.5, and downgrading affects anything else on the machine. |
| **Upgrade Laravel to 10/11** | The correct long-term answer. It is a multi-day project touching Breeze, the auth scaffold, and 34 controllers — far outside the scope of "run it locally". |
| **Suppress display of deprecations** | Immediate, reversible, and does not alter application code. Does not fix the underlying incompatibility. |

#### Decision

Set `display_errors = Off` and
`error_reporting = E_ALL & ~E_DEPRECATED & ~E_STRICT` in `php.ini`, and run
the application as-is.

Laravel's `HandleExceptions` bootstrapper calls `error_reporting(-1)` and sets
`display_errors` off for itself once booted, so this only affects the
pre-bootstrap window. Real errors still surface through Laravel's own debug
page while `APP_DEBUG=true`.

#### Consequences

**Positive**
- The application renders correctly with no code modification.
- Genuine errors remain visible via Laravel's handler.
- Reversible by editing one `php.ini` line.

**Negative**
- Deprecations are hidden, not resolved. The incompatibility is still present
  and will worsen with each PHP release.
- Log files still accumulate deprecation entries (`storage/logs/laravel.log`
  was already 27 MB on arrival).

**Risks**
- Laravel 8 reached end of security support in January 2023. Running it on a
  current PHP is a maintenance liability, not a stable resting place. A
  framework upgrade should be planned independently — see
  [06-WORK-INVENTORY.md](06-WORK-INVENTORY.md) Phase 5.

---

## ADR-003

### Map `tinyint(1)` to `tinyint`, not `bit`

**Status:** Accepted · 2026-08-12

#### Context

127 columns are declared `tinyint(1)` in the dump — chiefly `isDeleted` and the
~100 per-module permission flags on `users`. MySQL uses `tinyint(1)` as its
boolean; Laravel's own SQL Server grammar maps `$table->boolean()` to `bit`.

The columns had to land as one type or the other before any data was imported.

#### Options considered

| Option | Assessment |
|---|---|
| **`bit`** | Laravel-native for SQL Server; semantically "boolean". But `bit` coerces any non-zero value to 1, so if any column held a value >1 the data would be silently altered. The dump could not be cheaply proven free of such values. |
| **`tinyint`** | Byte-for-byte lossless (SQL Server `tinyint` is 0–255, matching MySQL's unsigned `tinyint`). Comparisons against `0`/`1` behave identically. PHP casts the returned int to bool exactly as before. |

#### Decision

Map `tinyint(1)` → **`tinyint`**.

The deciding factor was data preservation: `tinyint` cannot lose information,
`bit` can. Behaviour for this application is identical because every observed
check is `== 1` or `where('isDeleted', 0)`.

#### Consequences

**Positive**
- No possible data loss on import.
- Identical runtime behaviour; verified across 106 page loads.

**Negative**
- Diverges from what a fresh Laravel migration would generate. If someone later
  adds a column via `$table->boolean()`, that column will be `bit` while its
  neighbours are `tinyint` — a mixed representation in one table.

**Mitigation**
- Recorded as risk R5. If uniformity becomes desirable, the columns can be
  converted after confirming no value exceeds 1:
  ```sql
  SELECT MAX([isDeleted]) FROM [users];   -- verify <= 1 before ALTER
  ```

---

## ADR-004

### Keep the project root as document root; add a router

**Status:** Accepted · 2026-08-12

#### Context

`artisan serve` returned HTTP 500 with an empty body because
`base_path('server.php')` did not exist. Investigating revealed a deeper
structural fact: the application is deployed with the **project root** as
document root, not `public/`.

Evidence:
- The root `index.php` contains Laravel's standard `server.php` router logic.
- Blade templates call `asset('public/assets/...')`, producing URLs that
  include `/public/`.

`artisan serve` does `chdir(public_path())`, so `/public/assets/x.js` resolves
to `public/public/assets/x.js` and 404s. Adding a stock `server.php` fixed the
500 but left every asset broken.

#### Options considered

| Option | Assessment |
|---|---|
| **Rewrite the Blade templates** to use `asset('assets/...')` and serve from `public/` | The conventional Laravel layout, and the right long-term structure. But it touches many of 109 templates and would break the existing production deployment, which serves from the project root. Far beyond "run it locally". |
| **Serve with `php -S -t .`** (project root as docroot) | Faithful to production, but abandons `php artisan serve` as the command and exposes `.env` to HTTP unless separately guarded. |
| **A router that works under both layouts** | Preserves `artisan serve`, matches production URL semantics, and can deny sensitive paths centrally. |

#### Decision

Add `server.php` implementing a three-step resolution: **deny** blocklisted
paths → **serve** real files (natively when the built-in server can reach them,
streamed from the project root when it cannot) → **forward** everything else to
`public/index.php`.

#### Consequences

**Positive**
- `php artisan serve` works — the command a Laravel developer expects.
- URL semantics match production, so asset paths behave identically in both.
- Central denial of `.env`, `/config`, `/app`, `/vendor`, `/storage` — verified
  404. The original XAMPP arrangement relied on `.htaccess` for this.
- No Blade template was modified.

**Negative**
- `server.php` carries a MIME map and file-streaming logic — more moving parts
  than the stock 8-line file, and a place where a future contributor could
  introduce a path-traversal bug.
- The non-standard document-root arrangement is now entrenched rather than
  corrected.

**Mitigations applied**
- `realpath()` containment check ensures streamed files sit inside the project.
- `.php`/`.phtml` are never streamed, so source cannot leak as text.
- Blocklist is checked *before* any file resolution.

---

## ADR-005

### Emit filtered unique indexes

**Status:** Accepted · 2026-08-12

#### Context

MySQL permits any number of NULLs in a unique index. SQL Server permits exactly
one. The dump contains 5 unique keys. A direct translation could reject data
that MySQL accepted, or impose a constraint the application does not expect.

#### Options considered

| Option | Assessment |
|---|---|
| **Plain `UNIQUE` index** | Stricter than the source. Any table with two NULLs in a unique column fails to import, and the application could later fail on a legitimate insert. |
| **Filtered unique index** (`WHERE col IS NOT NULL`) | Reproduces MySQL semantics exactly: uniqueness enforced among non-NULL values, unlimited NULLs allowed. |
| **Drop uniqueness** | Loses a real data-integrity guarantee. |

#### Decision

Emit unique keys as filtered indexes:

```sql
CREATE UNIQUE INDEX [name] ON [table] ([col]) WHERE [col] IS NOT NULL;
```

#### Consequences

**Positive**
- Import succeeded with zero constraint violations.
- MySQL NULL semantics preserved, so application behaviour is unchanged.

**Negative**
- Filtered indexes are not what Laravel's schema builder generates, so a
  `migrate`-created index would differ from the imported one.
- Filtered indexes require `SET QUOTED_IDENTIFIER ON` at write time — set at the
  top of the generated script, but a client connecting with it off will fail on
  writes to these tables. `pdo_sqlsrv` sets it on by default; verified working
  across all tested writes.

---

## ADR-006

### Use a dedicated SQL login rather than Windows authentication

**Status:** Accepted · 2026-08-12

#### Context

SQL Server was in Mixed Mode (`IsIntegratedSecurityOnly = 0`), so both
authentication styles were available. `sqlcmd -E` confirmed the developer's
Windows account already had access.

#### Options considered

| Option | Assessment |
|---|---|
| **Windows authentication** | No password to store in `.env`. But the database identity becomes whichever Windows account runs PHP — the developer under `artisan serve`, an app-pool identity under IIS, a service account under a scheduler. Each requires separate grants. |
| **Dedicated SQL login** | One identity regardless of host process. Costs a password in `.env`. |

#### Decision

Create login `lmis_app`, mapped to a database user with `db_owner` on
`legacy_land_management` only.

#### Consequences

**Positive**
- Portable: works identically under `artisan serve`, IIS, or a scheduled task.
- Least-privilege at the server level — `lmis_app` has no server-wide rights.
- Makes the application's database access auditable as a distinct principal.

**Negative**
- A plaintext password now sits in `.env` (risk R4).
- `CHECK_POLICY = OFF` was set so the password is not subject to Windows policy
  expiry — convenient locally, inappropriate for production.
- `db_owner` is broader than the application needs; it can drop tables. A
  production deployment should reduce this to `db_datareader` +
  `db_datawriter` plus explicit grants.

---

## ADR-007

### Import from the converted dump rather than run migrations

**Status:** Accepted · 2026-08-12

#### Context

Two routes existed to create the schema: run the 51 migration files against
SQL Server, or convert and import the dump.

Three migration files contain MySQL-only SQL:

- `2026_02_10_000000_modify_lp_name_column` — `SET FOREIGN_KEY_CHECKS`,
  `ADD COLUMN … AFTER`, `JSON` type
- `2026_03_15_230046_fix_conveyance_rows_foreign_key` — `DROP FOREIGN KEY`,
  `MODIFY COLUMN`
- `2026_03_17_071000_fix_conveyance_rows_deed_id_data_type` — same

Additionally, migrations create an **empty** schema. The dump carries 345 rows
including the 12 user accounts needed to log in.

#### Options considered

| Option | Assessment |
|---|---|
| **Port the 3 migrations to T-SQL, then `migrate`** | Restores `migrate:fresh` as a working command. But it yields an empty database — the existing users and land records would still need importing separately, and editing historical migrations rewrites project history. |
| **Convert and import the dump** | Delivers schema *and* data in one step, including working logins. Leaves the migration files untouched and honest about what they were. |

#### Decision

Convert the dump to T-SQL and import it. Leave the migration files unmodified.

The imported `migrations` table carries all 57 recorded rows, including the
three MySQL-specific ones — so `artisan migrate` sees them as already run and
will not attempt to execute them.

#### Consequences

**Positive**
- Schema and data arrived together; the application was immediately usable with
  real records.
- Migration history is intact and truthful.
- Ordinary `artisan migrate` remains safe for future migrations.

**Negative**
- **`migrate:fresh` and `migrate:refresh` will fail on SQL Server.** The schema
  can currently be rebuilt only from the generated T-SQL script. This is risk R1
  and the highest-priority outstanding item.
- Any *new* migration must be written in dialect-neutral Schema-builder code, or
  it will break the same way.

**Follow-up:** [06-WORK-INVENTORY.md](06-WORK-INVENTORY.md) task 2.1 scopes
porting the three migrations so a from-scratch rebuild works again.
