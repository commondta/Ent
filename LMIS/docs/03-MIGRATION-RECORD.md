# 03 — Migration Record

**Date:** 2026-08-12
**Objective:** Run LMIS locally in a browser, with Microsoft SQL Server as the
database instead of MySQL.
**Outcome:** Running and verified at `http://127.0.0.1:8000`.

This document records **everything that was changed**, why, and how to
reproduce or reverse it.

---

## 1. Change Summary

| # | Change | Location | Type |
|---|---|---|---|
| C1 | Created `php.ini`, enabled 13 extensions | `C:\PHP\php.ini` | Environment (outside repo) |
| C2 | Installed Microsoft PHP drivers 5.13.2 | `C:\PHP\ext\` | Environment (outside repo) |
| C3 | Created database + login in SQL Server | `MSSQLSERVER` instance | Environment (outside repo) |
| C4 | Converted MySQL dump to T-SQL and imported | `database/` | New files in repo |
| C5 | Repointed Laravel at SQL Server | `.env` | Modified (untracked file) |
| C6 | Enabled `trust_server_certificate` | `config/database.php` | **Modified tracked file** |
| C7 | Added the missing dev-server router | `server.php` | **New tracked file** |
| C8 | Reset the administrator password | `users` table, id 1 | Data change |
| C9 | Added project documentation | `docs/` | New files in repo |

Only **C6** and **C7** alter tracked application files. No controller, model,
view, or migration was modified.

---

## 2. C1 — PHP Configuration

**Problem:** `php --ini` reported `Loaded Configuration File: (none)`. PHP was
running on built-in defaults with only 30 core modules. Laravel requires
Mbstring, Fileinfo, OpenSSL, Ctype, JSON, Tokenizer, XML, and BCMath; the
SQL Server work additionally needs PDO drivers.

**Action:** Created `C:\PHP\php.ini` from `php.ini-development` with these
changes:

```ini
extension_dir = "C:\PHP\ext"

extension=curl
extension=exif
extension=fileinfo
extension=gd
extension=intl
extension=mbstring
extension=mysqli
extension=openssl
extension=pdo_mysql
extension=pdo_sqlite
extension=sodium
extension=sqlite3
extension=zip

display_errors      = Off
error_reporting     = E_ALL & ~E_DEPRECATED & ~E_STRICT
memory_limit        = 512M
upload_max_filesize = 64M
post_max_size       = 64M
max_execution_time  = 300
date.timezone       = Asia/Karachi
```

**Why `display_errors = Off`:** Laravel 8.83 on PHP 8.5 emits a large volume of
"Implicitly marking parameter as nullable" deprecations from `vendor/` at
autoload time — *before* Laravel installs its own error handler. With
`display_errors = On` these print into the HTTP response body and can break
header emission. With it off, they go to the log and Laravel's own debug page
still reports real errors. See ADR-002.

**Reproduction gotcha:** the first attempt to write this file silently failed to
enable anything. `php.ini-development` uses CRLF line endings, and the PowerShell
regexes anchored with `$` did not match because `\r` sat before the line end.
The fix was to anchor with `\r?$` and to write the file **without a BOM**.

**Verification:** all 13 extensions appear in `php -m`;
`memory_limit=512M`, `display_errors` empty, `date.timezone=Asia/Karachi`.

---

## 3. C2 — SQL Server PHP Drivers

**Constraint:** the driver DLL must match the PHP build exactly. This PHP is:

```
PHP 8.5.9 | ZTS = yes | 64-bit
```

so the required files are the **8.5 / thread-safe / x64** variants.

**Action:**

1. Downloaded Microsoft Drivers for PHP for SQL Server **5.13.2**
   (released 2026-07-29) from `https://go.microsoft.com/fwlink/?LinkId=2374713`.
   *Note:* the download is served with an `.exe` name but is actually a **ZIP**
   archive (magic bytes `50 4B 03 04`); it must be extracted, not executed.
2. Copied into `C:\PHP\ext\`:
   - `php_sqlsrv_85_ts_x64.dll`
   - `php_pdo_sqlsrv_85_ts_x64.dll`
3. Appended to `php.ini`:

```ini
; --- Microsoft Drivers for PHP for SQL Server 5.13.2 ---
extension=php_sqlsrv_85_ts_x64.dll
extension=php_pdo_sqlsrv_85_ts_x64.dll
```

**Verification:** `PDO::getAvailableDrivers()` returns `mysql, sqlite, sqlsrv`.

---

## 4. C3 — SQL Server Objects

The instance was **already installed and running** — SQL Server 2019 Standard
(15.0.2000.5), default instance `MSSQLSERVER`, TCP 1433 listening, ODBC Driver
17 and 18 present, and `SERVERPROPERTY('IsIntegratedSecurityOnly') = 0`
(Mixed Mode).

```sql
CREATE DATABASE [legacy_land_management];   -- collation Latin1_General_CI_AS

CREATE LOGIN [lmis_app]
    WITH PASSWORD = 'Lmis@Local2026!',
         CHECK_POLICY = OFF,
         DEFAULT_DATABASE = [legacy_land_management];

USE [legacy_land_management];
CREATE USER [lmis_app] FOR LOGIN [lmis_app];
ALTER ROLE [db_owner] ADD MEMBER [lmis_app];
```

**Why a SQL login rather than Windows authentication:** Windows auth would tie
the application's database identity to whichever Windows account happens to run
PHP. That works for `artisan serve` under the developer's own account but breaks
the moment the app runs under IIS, a service account, or a scheduled task. A
dedicated login is portable. See ADR-006.

`db_owner` is scoped to this database only — `lmis_app` has no server-level
privileges.

---

## 5. C4 — MySQL → T-SQL Conversion

### 5.1 Deliverables

| File | Purpose |
|---|---|
| `database/mysql2mssql.php` | The converter — reusable if the dump is refreshed |
| `database/legacy_land_management_mssql.sql` | Generated T-SQL (134 KB) |
| `database/legacy_land_management (1).sql` | **Original MySQL dump, untouched** |

Usage:

```bash
php database/mysql2mssql.php "database/legacy_land_management (1).sql" out.sql
```

### 5.2 How the converter works

1. **Statement splitter** — walks the dump character by character, tracking
   `'`, `"` and backtick quoting plus MySQL backslash escapes, so it splits on
   genuine statement-terminating semicolons and never on a `;` inside data. It
   also strips `--`/`#` line comments and `/*! … */` conditional directives.
2. **Two-pass table model** — phpMyAdmin emits keys and `AUTO_INCREMENT` in
   trailing `ALTER TABLE` statements rather than inline. The converter parses
   `CREATE TABLE` first, then folds the `ALTER` clauses back in, so `IDENTITY`
   and the primary key are emitted *inside* `CREATE TABLE` as SQL Server requires.
3. **Literal re-encoding** — decodes MySQL escapes (`\n`, `\'`, `\\`, `\0`,
   `\Z`, …) and re-encodes as T-SQL, doubling single quotes and prefixing `N`
   for Unicode.
4. **Emission order** — drop all FKs → drop tables → create tables → insert data
   (wrapped in `SET IDENTITY_INSERT`) → reseed identities → create indexes →
   add foreign keys. Data loads before FKs so table order cannot cause failures.

### 5.3 Type mapping applied

| MySQL | T-SQL | Rationale |
|---|---|---|
| `bigint(20) UNSIGNED` | `bigint` | SQL Server has no unsigned integers; range is sufficient |
| `int(11)`, `int(150)`, `int(100)`, `int(10) UNSIGNED` | `int` | MySQL display widths are meaningless |
| `tinyint(1)` | `tinyint` | Lossless; see ADR-003 for why not `bit` |
| `varchar(n)` | `nvarchar(n)` | Unicode preservation; `nvarchar(max)` above 4000 |
| `text` | `nvarchar(max)` | — |
| `timestamp`, `datetime` | `datetime` | Matches Laravel's own SQL Server grammar, which formats `Y-m-d H:i:s.v` |
| `date` | `date` | — |
| `decimal(10,2)` | `decimal(10,2)` | — |
| `AUTO_INCREMENT` | `IDENTITY(1,1)` | Folded into `CREATE TABLE` |
| `ENGINE=InnoDB DEFAULT CHARSET=…` | *(dropped)* | No equivalent |
| `DEFAULT current_timestamp()` | `DEFAULT SYSDATETIME()` | — |
| `ON UPDATE current_timestamp()` | *(dropped)* | No equivalent; Laravel maintains `updated_at` itself |

### 5.4 Two semantic differences handled deliberately

**Reserved words.** The schema uses `date`, `status`, `user` and `view` as
column names; `view` and `user` are T-SQL reserved words. Every identifier in
the output is bracketed (`[view]`), so no collision is possible.

**NULLs in unique indexes.** MySQL allows many NULLs in a unique index;
SQL Server allows only one. Unique keys were therefore emitted as **filtered**
indexes to preserve the original semantics:

```sql
CREATE UNIQUE INDEX [users_email_unique] ON [users] ([email])
    WHERE [email] IS NOT NULL;
```

### 5.5 Import result

Executed via PDO in GO-delimited batches:

```
Batches succeeded: 84
Batches failed   : 0
Tables           : 44
Total rows       : 345
```

Row count reconciles exactly with the converter's own count of 345.

---

## 6. C5 / C6 — Laravel Configuration

`.env`:

```diff
-APP_URL=http://192.168.109.7:8080/test_Land_mgt
+APP_URL=http://127.0.0.1:8000

-DB_CONNECTION=mysql
-DB_HOST=127.0.0.1
-DB_PORT=3306
-DB_DATABASE=legacy_land_management
-DB_USERNAME=root
-DB_PASSWORD=
+DB_CONNECTION=sqlsrv
+DB_HOST=127.0.0.1
+DB_PORT=1433
+DB_DATABASE=legacy_land_management
+DB_USERNAME=lmis_app
+DB_PASSWORD=Lmis@Local2026!
+DB_TRUST_SERVER_CERTIFICATE=true
```

`config/database.php` — one line added to the existing `sqlsrv` block:

```php
'trust_server_certificate' => env('DB_TRUST_SERVER_CERTIFICATE', false),
```

**Why:** ODBC Driver 18 defaults to `Encrypt=yes`. A local SQL Server presents a
self-signed certificate, so the connection is refused unless the client is told
to trust it. Laravel's `SqlServerConnector` already honours a
`trust_server_certificate` config key (verified at
`vendor/laravel/framework/src/Illuminate/Database/Connectors/SqlServerConnector.php:127`);
the key simply was not wired to an env variable. Defaulting to `false` keeps the
insecure setting opt-in.

**Verification:**

```
Connection: sqlsrv
Database  : legacy_land_management
Login     : lmis_app
Users     : 12
```

---

## 7. C7 — The Dev-Server Router (`server.php`)

### The problem, precisely

Two facts collided:

1. This app is deployed with the **project root as document root**. Root
   `index.php` is Laravel's router content, and Blade templates call
   `asset('public/assets/...')`, producing URLs like `/public/assets/js/config.js`.
2. `artisan serve` does `chdir(public_path())` — so the built-in server's
   document root is `public/`, and `/public/assets/...` resolves to
   `public/public/assets/...`, which does not exist.

With `server.php` missing entirely, the first symptom was simply HTTP 500.
Adding a stock `server.php` fixed the 500 but left every asset 404ing.

### The solution

`server.php` resolves a request in three steps:

1. **Deny** anything matching a blocklist — dotfiles, `vendor/`, `storage/`,
   `app/`, `config/`, `database/`, `routes/`, `resources/`, `tests/`,
   `bootstrap/`, `artisan`, `composer.*`, `package.*`, `phpunit*`, build configs,
   and `server.php` itself → 404.
2. **Serve** a real file. If the built-in server can reach it from its own
   `DOCUMENT_ROOT`, return `false` and let PHP serve it natively. Otherwise —
   the `/public/...` case — stream it from the project root with an explicit
   `Content-Type` from a MIME map, guarded by a `realpath()` containment check
   and an absolute refusal to stream `.php`/`.phtml`.
3. **Forward** everything else to `public/index.php`.

This works under both document-root arrangements, so `php artisan serve`
remains the command to use.

**Verification:**

| Path | Result |
|---|---|
| `/` , `/login` | 200 |
| `/public/assets/js/config.js` | 200, `application/javascript` |
| `/public/vendors/simplebar/simplebar.min.js` | 200 |
| All 28 assets referenced by `/login` | 200 |
| `/.env` | 404 |
| `/config/database.php` | 404 |

---

## 8. C8 — Administrator Password Reset

Passwords carried over from MySQL as intact bcrypt hashes and continue to work
with their original values. The administrator's original password was not
known, so on request it was reset:

| Field | Value |
|---|---|
| Email | `admin@gmail.com` |
| Password | `Admin@12345` |
| User id | 1 (`Administrator`, `is_admin = 1`) |

Only that one row was updated; the other 11 accounts retain their original
hashes. Verified by a full HTTP login: POST `/login` → 302 → `/dashboard` →
`/land_provider`, session active.

**This is a full administrator credential and should be changed from within the
application.**

---

## 9. Verification Summary

| Test | Scope | Result |
|---|---|---|
| Schema/data import | 84 batches | 0 failures |
| Authenticated GET routes | 48 routes | 48 pass |
| Edit / show routes with real ids | 58 requests | 58 pass |
| Static assets on `/login` | 28 assets | 28 pass |
| Blocked-path checks | `/.env`, `/config/database.php` | Both 404 |
| Admin login | End-to-end HTTP | Pass |

Testing used throwaway admin accounts created and removed in a `try/finally`
block. Final state: 12 users, zero test residue.

One apparent failure during testing — `/exemption_inventory/{id}/edit` → 404 —
was traced to the **test's** wrong table mapping: that route binds to
`Exemption_inventory_approval` (table `exemption_inventory_approvals`), not
`exemption_inventory_rows`. With correct ids it returns 200. The application
was behaving correctly.

---

## 10. How to Run

```powershell
cd "C:\Users\Adnan Ahmed\Pictures\LMIS"
php artisan serve
```

Then open **http://127.0.0.1:8000**.

Prerequisite services: the `MSSQLSERVER` Windows service must be running.

---

## 11. How to Reverse

| Change | Reversal |
|---|---|
| C1 `php.ini` | Delete `C:\PHP\php.ini` (returns PHP to unconfigured state) |
| C2 drivers | Remove the two DLLs from `C:\PHP\ext` and their `extension=` lines |
| C3 database | `DROP DATABASE [legacy_land_management]; DROP LOGIN [lmis_app];` |
| C4 conversion | Delete the two generated files in `database/`; the original dump is untouched |
| C5 `.env` | Restore the MySQL block (requires a MySQL server to be useful) |
| C6 `config/database.php` | Remove the one added line |
| C7 `server.php` | Delete the file (returns `artisan serve` to HTTP 500) |
| C8 password | Set a new password; the original hash is not recoverable |

---

## 12. Rebuilding the Database From Scratch

```powershell
sqlcmd -S localhost -E -Q "DROP DATABASE IF EXISTS [legacy_land_management]; CREATE DATABASE [legacy_land_management];"
php database/mysql2mssql.php "database/legacy_land_management (1).sql" rebuild.sql
sqlcmd -S localhost -U lmis_app -P "Lmis@Local2026!" -d legacy_land_management -C -i rebuild.sql
```

**Do not use `artisan migrate:fresh`.** Three migration files contain
MySQL-only SQL and will fail on SQL Server — see risk R1 in
[02-ASSESSMENT.md](02-ASSESSMENT.md) and task 2.1 in
[06-WORK-INVENTORY.md](06-WORK-INVENTORY.md).
