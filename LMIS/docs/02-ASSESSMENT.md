# 02 — Assessment

**Assessment date:** 2026-08-12
**Scope:** Bringing LMIS up on this workstation (PHP 8.5 + SQL Server) and
verifying it functions.

---

## 1. State the System Was Found In

The repository was a complete Laravel 8 application with `vendor/` already
installed, but **nothing on this machine could run it**. Four independent
blockers stood between the checkout and a working page.

| # | Blocker | Severity | Evidence |
|---|---|---|---|
| B1 | PHP had no `php.ini` at all | Blocking | `php --ini` reported `Loaded Configuration File: (none)`; only 30 core modules present |
| B2 | No database driver for SQL Server | Blocking | `PDO::getAvailableDrivers()` returned `mysql, sqlite` only |
| B3 | Database did not exist in SQL Server | Blocking | Data existed only as a MySQL/MariaDB dump |
| B4 | `server.php` missing → `artisan serve` returned HTTP 500 | Blocking | Empty 500 response; Laravel's `ServeCommand` requires `base_path('server.php')` |

### B1 detail — the empty PHP install

`C:\PHP` contained the full `ext/` directory (36 DLLs) but neither
`php.ini` nor any loaded configuration. Consequently every extension Laravel
needs beyond the core set was missing: `mbstring`, `fileinfo`, `openssl`,
`curl`, `gd`, `zip`, `intl`, and every PDO driver except the built-ins.

### B2 detail — driver availability

The Microsoft drivers are not bundled with PHP and must match the exact build
(version, thread-safety, architecture). This PHP is **8.5.9, ZTS, x64**, so the
required files are `php_sqlsrv_85_ts_x64.dll` and `php_pdo_sqlsrv_85_ts_x64.dll`.
PHP 8.5 support exists as of driver release **5.13.0**; 5.13.2 is current.

### B3 detail — the data

The only copy of the data was `database/legacy_land_management (1).sql`, a
phpMyAdmin export (MariaDB 10.4, PHP 8.0.30, generated 2026-05-04) containing
**44 tables and 345 rows**. No MySQL or MariaDB server was installed on this
machine, and none was installed as part of this work.

### B4 detail — the document-root mismatch

`Illuminate\Foundation\Console\ServeCommand` does `chdir(public_path())` and
then runs `php -S host:port {base_path}/server.php`. The file did not exist.
Separately, the application's own root `index.php` is Laravel's standard
`server.php` router content — evidence the app was deployed with the **project
root as document root**, which is also why Blade emits `/public/assets/...`
URLs.

---

## 2. What Was Already Healthy

These findings materially reduced migration risk and are worth recording.

| Finding | Why it mattered |
|---|---|
| **Zero raw SQL in `app/`** | No `DB::raw`, `whereRaw`, `selectRaw`, `DB::select`, `DB::statement`, `DATE_FORMAT`, `GROUP_CONCAT`, or `IFNULL`. Pure Eloquent is dialect-portable, so the MySQL→SQL Server move required no application-code changes. |
| **No `groupBy` / `having`** | Avoided SQL Server's stricter `GROUP BY` rules, which routinely break MySQL-authored queries. Only three `distinct()` calls exist, which are portable. |
| **Clean dump data** | Zero backslash escapes, zero `0000-00-00` dates, no `json` columns, only 5 `current_timestamp` defaults and 1 `ON UPDATE`. Conversion had few edge cases. |
| **SQL Server already installed** | 2019 Standard, running, Mixed Mode authentication enabled, TCP 1433 listening, ODBC 17 and 18 present. No server installation was needed. |
| **`APP_KEY` already set** | No session/cookie decryption break. |

---

## 3. Schema Shape (from the dump)

Column type distribution across 44 tables — this drove the type-mapping table
in [03-MIGRATION-RECORD.md](03-MIGRATION-RECORD.md).

| MySQL type | Occurrences |
|---|---|
| `varchar(255)` | 307 |
| `tinyint(1)` | 127 |
| `int(11)` | 120 |
| `timestamp` | 85 |
| `bigint(20) UNSIGNED` | 48 |
| `decimal(10,2)` | 23 |
| `date` | 20 |
| `text` | 9 |
| others (`int(150)`, `varchar(128/150/100/64/50/254)`, `int(10) UNSIGNED`, `int(100)`) | 24 |

Structural elements: 44 identity (`AUTO_INCREMENT`) columns, 44 primary keys,
4 foreign keys with `ON DELETE CASCADE`, 5 unique keys, 6 plain indexes.

**Reserved-word collision:** four column names are T-SQL reserved or
problematic words — `date`, `status`, `user`, `view`. Every identifier in the
generated script is bracketed as a result.

---

## 4. Risk Register

Risks that remain **after** the work in 03-MIGRATION-RECORD. Ordered by
severity.

| ID | Risk | Severity | Status | Detail |
|---|---|---|---|---|
| R1 | `migrate:fresh` / `migrate:refresh` will fail | High | **Open** | Three migrations contain MySQL-only SQL (`SET FOREIGN_KEY_CHECKS`, `MODIFY COLUMN`, backtick quoting, `ADD COLUMN … AFTER`). All three are already recorded in the `migrations` table, so ordinary `artisan migrate` is safe — but a from-scratch rebuild is not. See [06-WORK-INVENTORY.md](06-WORK-INVENTORY.md) task 2.1. |
| R2 | Laravel 8 is past end-of-life on PHP 8.5 | High | **Accepted** | Laravel 8 security support ended Jan 2023 and it predates PHP 8.5. Deprecations are suppressed from display, not fixed. Upgrade path is a project in itself. ADR-002. |
| R3 | `APP_DEBUG=true` | Medium | **Open (intentional locally)** | Full stack traces and environment contents are rendered on error. Must be `false` before any non-local exposure. |
| R4 | DB password in plaintext in `.env` | Medium | **Accepted for local** | `lmis_app` holds `db_owner` on the application database only; it is not a server-level admin. Rotate before shared use. |
| R5 | `tinyint(1)` mapped to `tinyint`, not `bit` | Low | **Accepted** | Lossless and behaviourally equivalent for this app, but a future Laravel `$table->boolean()` migration would create a `bit` column, producing mixed representations. ADR-003. |
| R6 | Uploads are world-readable static files | Medium | **Pre-existing** | Everything in `public/assets/uploads` is served without an auth check. Land documents may be sensitive. Not introduced by this work. |
| R7 | Unique indexes are filtered | Low | **By design** | MySQL permits many NULLs in a unique index; SQL Server does not. Unique indexes were emitted with `WHERE col IS NOT NULL` to preserve MySQL semantics. ADR-005. |
| R8 | Root directory contains editor litter | Cosmetic | **Open** | `~$*.docx` and `~WRL*.tmp` files are committed alongside source. |
| R9 | Duplicate user record | Low | **Pre-existing data** | `users` id 10 and 11 both hold the same login e-mail address. There is no unique constraint on `users.email` in the dump. |

---

## 5. Verification Performed

Evidence that the system works, not just that it starts.

| Check | Result |
|---|---|
| Schema + data import | 84 batches executed, **0 failures**; 44 tables, 345 rows |
| Row-count reconciliation | Converter counted 345 rows; database reports 345 |
| Laravel → SQL Server connectivity | `DB_NAME()` = `legacy_land_management`, `SUSER_NAME()` = `lmis_app`, `users` count = 12 |
| Authenticated GET routes | **48 / 48** returned 200 (one 302 is the dashboard's intended redirect) |
| Parameterised edit/show routes | **58 / 58** returned 200 or an intended 302 |
| Referenced static assets | **28 / 28** returned 200 |
| Security — blocked paths | `/.env` → 404, `/config/database.php` → 404 |
| Admin login end-to-end | POST `/login` → 302 → `/dashboard` → `/land_provider`, session active |
| Test-account cleanup | 3 throwaway admins created and deleted; `users` back to 12, 0 residue |

**Method note:** verification used throwaway admin accounts created and deleted
inside a `try/finally`, so no existing account was modified during testing. The
only deliberate change to existing data was the admin password reset, performed
later at explicit request.

---

## 6. Assessment Conclusion

The application is **functionally healthy on SQL Server**. The migration was
low-risk because the codebase never reaches past Eloquent into dialect-specific
SQL — the entire porting effort landed in the schema and data, not the code.
No application file was modified to accommodate SQL Server.

The two changes to tracked project files were **additive**: a `server.php`
router that did not exist, and one config line enabling
`trust_server_certificate`. Everything else was environment configuration
outside the repository, or new documentation and tooling under `database/` and
`docs/`.

The material outstanding item is **R1** — the migration files are still
MySQL-dialect, so the schema can be reproduced today only from the generated
T-SQL script, not from `artisan migrate:fresh`.
