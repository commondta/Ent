<?php
/**
 * Converts a phpMyAdmin/MySQL dump into a T-SQL script for Microsoft SQL Server.
 *
 * Usage: php mysql2mssql.php <input.sql> <output.sql>
 */

if ($argc < 3) {
    fwrite(STDERR, "Usage: php mysql2mssql.php <input.sql> <output.sql>\n");
    exit(1);
}

$src = $argv[1];
$dst = $argv[2];
$sql = file_get_contents($src);
if ($sql === false) {
    fwrite(STDERR, "Cannot read $src\n");
    exit(1);
}

$notes = [];

/* ------------------------------------------------------------------ *
 * 1. Split the dump into statements, respecting quotes and comments.
 * ------------------------------------------------------------------ */
function splitStatements(string $sql): array
{
    $stmts = [];
    $cur = '';
    $len = strlen($sql);
    $i = 0;

    while ($i < $len) {
        $ch = $sql[$i];

        if ($ch === '#' || ($ch === '-' && substr($sql, $i, 3) === '-- ') || ($ch === '-' && substr($sql, $i, 2) === '--' && trim(substr($sql, $i, 3)) === '--')) {
            $nl = strpos($sql, "\n", $i);
            if ($nl === false) { break; }
            $i = $nl + 1;
            continue;
        }

        // Block comments, including MySQL conditional /*! ... */ directives.
        if ($ch === '/' && substr($sql, $i, 2) === '/*') {
            $end = strpos($sql, '*/', $i + 2);
            if ($end === false) { break; }
            $i = $end + 2;
            continue;
        }

        if ($ch === "'" || $ch === '"' || $ch === '`') {
            $q = $ch;
            $cur .= $ch;
            $i++;
            while ($i < $len) {
                $c2 = $sql[$i];
                if ($c2 === '\\' && $q !== '`') {          // MySQL backslash escape
                    $cur .= substr($sql, $i, 2);
                    $i += 2;
                    continue;
                }
                if ($c2 === $q) {
                    if ($i + 1 < $len && $sql[$i + 1] === $q) {   // doubled-quote escape
                        $cur .= $q . $q;
                        $i += 2;
                        continue;
                    }
                    $cur .= $q;
                    $i++;
                    break;
                }
                $cur .= $c2;
                $i++;
            }
            continue;
        }

        if ($ch === ';') {
            $stmts[] = trim($cur);
            $cur = '';
            $i++;
            continue;
        }

        $cur .= $ch;
        $i++;
    }

    if (trim($cur) !== '') { $stmts[] = trim($cur); }

    return array_values(array_filter($stmts, fn($s) => $s !== ''));
}

/* ------------------------------------------------------------------ *
 * 2. Type mapping.
 * ------------------------------------------------------------------ */
function mapType(string $mysqlType, array &$notes): string
{
    $t = strtolower(trim($mysqlType));
    $t = preg_replace('/\s+unsigned\b/', '', $t);
    $t = preg_replace('/\s+zerofill\b/', '', $t);
    $t = trim($t);

    if (preg_match('/^bigint/', $t))                       return 'bigint';
    if (preg_match('/^tinyint/', $t))                      return 'tinyint';
    if (preg_match('/^smallint/', $t))                     return 'smallint';
    if (preg_match('/^mediumint/', $t))                    return 'int';
    if (preg_match('/^int/', $t))                          return 'int';
    if (preg_match('/^varchar\((\d+)\)/', $t, $m))         return ((int)$m[1] <= 4000) ? "nvarchar({$m[1]})" : 'nvarchar(max)';
    if (preg_match('/^char\((\d+)\)/', $t, $m))            return "nchar({$m[1]})";
    if (preg_match('/^(longtext|mediumtext|text|tinytext)$/', $t)) return 'nvarchar(max)';
    if (preg_match('/^(longblob|mediumblob|blob|tinyblob)$/', $t)) return 'varbinary(max)';
    if ($t === 'json')                                     return 'nvarchar(max)';
    if (preg_match('/^decimal\((\d+),\s*(\d+)\)/', $t, $m)) return "decimal({$m[1]},{$m[2]})";
    if (preg_match('/^decimal/', $t))                      return 'decimal(18,2)';
    if (preg_match('/^(double|float|real)/', $t))          return 'float';
    if (preg_match('/^(timestamp|datetime)/', $t))         return 'datetime';
    if ($t === 'date')                                     return 'date';
    if (preg_match('/^time\b/', $t))                       return 'time';
    if ($t === 'year')                                     return 'smallint';
    if (preg_match('/^(enum|set)\s*\(/', $t))              return 'nvarchar(255)';

    $notes[] = "UNMAPPED TYPE '$mysqlType' -> nvarchar(255)";
    return 'nvarchar(255)';
}

/* ------------------------------------------------------------------ *
 * 3. Literal conversion: MySQL string -> T-SQL N'...' literal.
 * ------------------------------------------------------------------ */
function convertLiteral(string $lit): string
{
    $inner = substr($lit, 1, -1);
    $out = '';
    $n = strlen($inner);

    for ($i = 0; $i < $n; $i++) {
        $c = $inner[$i];
        if ($c === '\\' && $i + 1 < $n) {
            $nx = $inner[++$i];
            switch ($nx) {
                case 'n':  $out .= "\n";     break;
                case 'r':  $out .= "\r";     break;
                case 't':  $out .= "\t";     break;
                case '0':  $out .= "\0";     break;
                case 'b':  $out .= chr(8);   break;
                case 'Z':  $out .= chr(26);  break;
                case '\\': $out .= '\\';     break;
                case "'":  $out .= "'";      break;
                case '"':  $out .= '"';      break;
                default:   $out .= $nx;      break;   // \% and \_ keep the backslash meaning literally
            }
        } elseif ($c === "'" && $i + 1 < $n && $inner[$i + 1] === "'") {
            $out .= "'";
            $i++;
        } else {
            $out .= $c;
        }
    }

    return "N'" . str_replace("'", "''", $out) . "'";
}

/** Split a parenthesised list on top-level commas, honouring quotes. */
function splitTopLevel(string $s, string $sep = ','): array
{
    $parts = [];
    $cur = '';
    $depth = 0;
    $len = strlen($s);

    for ($i = 0; $i < $len; $i++) {
        $c = $s[$i];

        if ($c === "'" || $c === '`' || $c === '"') {
            $q = $c;
            $cur .= $c;
            $i++;
            while ($i < $len) {
                $c2 = $s[$i];
                if ($c2 === '\\' && $q !== '`') { $cur .= substr($s, $i, 2); $i += 2; continue; }
                if ($c2 === $q) {
                    if ($i + 1 < $len && $s[$i + 1] === $q) { $cur .= $q . $q; $i += 2; continue; }
                    $cur .= $q;
                    break;
                }
                $cur .= $c2;
                $i++;
            }
            continue;
        }

        if ($c === '(') { $depth++; }
        if ($c === ')') { $depth--; }

        if ($c === $sep && $depth === 0) { $parts[] = trim($cur); $cur = ''; continue; }
        $cur .= $c;
    }

    if (trim($cur) !== '') { $parts[] = trim($cur); }
    return $parts;
}

function ident(string $raw): string
{
    return '[' . str_replace(['`', '[', ']'], ['', '', ''], trim($raw)) . ']';
}

function bareName(string $raw): string
{
    return str_replace(['`', '[', ']'], '', trim($raw));
}

/* ------------------------------------------------------------------ *
 * 4. Parse the dump into a table model.
 * ------------------------------------------------------------------ */
$statements = splitStatements($sql);
$tables = [];   // name => model
$inserts = [];  // ordered list of ['table'=>, 'cols'=>[], 'rows'=>[[...]]]

foreach ($statements as $stmt) {
    $norm = preg_replace('/\s+/', ' ', $stmt);

    /* ---- CREATE TABLE ---- */
    if (preg_match('/^CREATE TABLE\s+(?:IF NOT EXISTS\s+)?`?([^`\s(]+)`?\s*\((.*)\)\s*(ENGINE.*)?$/is', $stmt, $m)) {
        $tbl = bareName($m[1]);
        $body = $m[2];

        $model = ['name' => $tbl, 'cols' => [], 'pk' => [], 'unique' => [], 'keys' => [], 'fks' => [], 'identity' => null];

        foreach (splitTopLevel($body) as $def) {
            $def = trim($def);
            if ($def === '') { continue; }

            if (preg_match('/^PRIMARY KEY\s*\((.+)\)$/is', $def, $k)) {
                $model['pk'] = array_map('bareName', splitTopLevel($k[1]));
                continue;
            }
            if (preg_match('/^UNIQUE(?: KEY| INDEX)?\s+`?([^`\s(]*)`?\s*\((.+)\)$/is', $def, $k)) {
                $model['unique'][] = ['name' => bareName($k[1]) ?: ($tbl . '_unique'), 'cols' => array_map('bareName', splitTopLevel($k[2]))];
                continue;
            }
            if (preg_match('/^(?:KEY|INDEX)\s+`?([^`\s(]*)`?\s*\((.+)\)$/is', $def, $k)) {
                $model['keys'][] = ['name' => bareName($k[1]) ?: ($tbl . '_idx'), 'cols' => array_map('bareName', splitTopLevel($k[2]))];
                continue;
            }
            if (preg_match('/^CONSTRAINT\s+`?([^`\s]*)`?\s+FOREIGN KEY\s*\((.+?)\)\s*REFERENCES\s+`?([^`\s(]+)`?\s*\((.+?)\)(.*)$/is', $def, $k)) {
                $model['fks'][] = [
                    'name'    => bareName($k[1]),
                    'cols'    => array_map('bareName', splitTopLevel($k[2])),
                    'refTbl'  => bareName($k[3]),
                    'refCols' => array_map('bareName', splitTopLevel($k[4])),
                    'action'  => trim($k[5]),
                ];
                continue;
            }
            if (preg_match('/^(FULLTEXT|SPATIAL|CHECK)\b/i', $def)) { continue; }

            // Ordinary column definition.
            if (preg_match('/^`([^`]+)`\s+(.+)$/is', $def, $c)) {
                $colName = $c[1];
                $rest    = trim($c[2]);

                // Type is the leading token, possibly with (..) and UNSIGNED.
                if (!preg_match('/^([a-z]+(?:\s*\([^)]*\))?(?:\s+unsigned)?(?:\s+zerofill)?)(.*)$/is', $rest, $tm)) {
                    continue;
                }
                $rawType = trim($tm[1]);
                $attrs   = trim($tm[2]);

                $isAuto  = (bool) preg_match('/\bAUTO_INCREMENT\b/i', $attrs);
                $notNull = (bool) preg_match('/\bNOT NULL\b/i', $attrs);

                $default = null;
                if (preg_match("/\bDEFAULT\s+('(?:[^'\\\\]|\\\\.|'')*'|[A-Za-z0-9_]+\(\)|[-\w.]+)/is", $attrs, $dm)) {
                    $default = trim($dm[1]);
                }

                $model['cols'][] = [
                    'name'     => $colName,
                    'type'     => mapType($rawType, $notes),
                    'rawType'  => $rawType,
                    'notNull'  => $notNull,
                    'default'  => $default,
                    'identity' => $isAuto,
                ];

                if ($isAuto) { $model['identity'] = $colName; }
            }
        }

        $tables[$tbl] = $model;
        continue;
    }

    /* ---- ALTER TABLE (phpMyAdmin emits keys / AUTO_INCREMENT separately) ---- */
    if (preg_match('/^ALTER TABLE\s+`?([^`\s]+)`?\s+(.*)$/is', $stmt, $m)) {
        $tbl = bareName($m[1]);
        if (!isset($tables[$tbl])) { continue; }
        $clauses = splitTopLevel($m[2]);

        foreach ($clauses as $cl) {
            $cl = trim($cl);

            if (preg_match('/^ADD PRIMARY KEY\s*\((.+)\)$/is', $cl, $k)) {
                $tables[$tbl]['pk'] = array_map('bareName', splitTopLevel($k[1]));
                continue;
            }
            if (preg_match('/^ADD UNIQUE(?: KEY| INDEX)?\s+`?([^`\s(]*)`?\s*\((.+)\)$/is', $cl, $k)) {
                $tables[$tbl]['unique'][] = ['name' => bareName($k[1]), 'cols' => array_map('bareName', splitTopLevel($k[2]))];
                continue;
            }
            if (preg_match('/^ADD (?:KEY|INDEX)\s+`?([^`\s(]*)`?\s*\((.+)\)$/is', $cl, $k)) {
                $tables[$tbl]['keys'][] = ['name' => bareName($k[1]), 'cols' => array_map('bareName', splitTopLevel($k[2]))];
                continue;
            }
            if (preg_match('/^ADD CONSTRAINT\s+`?([^`\s]*)`?\s+FOREIGN KEY\s*\((.+?)\)\s*REFERENCES\s+`?([^`\s(]+)`?\s*\((.+?)\)(.*)$/is', $cl, $k)) {
                $tables[$tbl]['fks'][] = [
                    'name'    => bareName($k[1]),
                    'cols'    => array_map('bareName', splitTopLevel($k[2])),
                    'refTbl'  => bareName($k[3]),
                    'refCols' => array_map('bareName', splitTopLevel($k[4])),
                    'action'  => trim($k[5]),
                ];
                continue;
            }
            // MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT
            if (preg_match('/^(?:MODIFY|CHANGE)\s+(?:COLUMN\s+)?`([^`]+)`.*AUTO_INCREMENT/is', $cl, $k)) {
                $col = $k[1];
                $tables[$tbl]['identity'] = $col;
                foreach ($tables[$tbl]['cols'] as &$cRef) {
                    if ($cRef['name'] === $col) { $cRef['identity'] = true; }
                }
                unset($cRef);
                continue;
            }
        }
        continue;
    }

    /* ---- INSERT ---- */
    if (preg_match('/^INSERT\s+INTO\s+`?([^`\s(]+)`?\s*\((.*?)\)\s*VALUES\s*(.*)$/is', $stmt, $m)) {
        $tbl  = bareName($m[1]);
        $cols = array_map('bareName', splitTopLevel($m[2]));
        $rows = [];

        foreach (splitTopLevel(trim($m[3])) as $tuple) {
            $tuple = trim($tuple);
            if ($tuple === '' || $tuple[0] !== '(') { continue; }
            $tuple = substr($tuple, 1, strrpos($tuple, ')') - 1);
            $rows[] = splitTopLevel($tuple);
        }

        if ($rows) { $inserts[] = ['table' => $tbl, 'cols' => $cols, 'rows' => $rows]; }
        continue;
    }
}

/* ------------------------------------------------------------------ *
 * 5. Emit T-SQL.
 * ------------------------------------------------------------------ */
$defaultLiteral = function (string $d, string $type): ?string {
    $dl = strtolower($d);
    if ($dl === 'null')                       return null;   // nullable already covers it
    if (in_array($dl, ['current_timestamp()', 'current_timestamp', 'now()'], true)) {
        return 'SYSDATETIME()';
    }
    if ($d !== '' && $d[0] === "'")           return convertLiteral($d);
    if (is_numeric($d))                       return $d;
    return "N'" . str_replace("'", "''", trim($d, "'")) . "'";
};

$out = [];
$out[] = "/* ------------------------------------------------------------------";
$out[] = " * T-SQL script generated from a MySQL/MariaDB dump.";
$out[] = " * Source : " . basename($src);
$out[] = " * Tables : " . count($tables);
$out[] = " * ------------------------------------------------------------------ */";
$out[] = "SET QUOTED_IDENTIFIER ON;";
$out[] = "SET ANSI_NULLS ON;";
$out[] = "GO";

/* -- Drop every existing foreign key, then every table we are about to build. -- */
$out[] = "/* Drop existing foreign keys so tables can be dropped in any order. */";
$out[] = <<<'SQLBLK'
DECLARE @dropFk NVARCHAR(MAX) = N'';
SELECT @dropFk += N'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(t.schema_id)) + N'.' + QUOTENAME(t.name)
                + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';' + CHAR(10)
FROM sys.foreign_keys fk
JOIN sys.tables t ON t.object_id = fk.parent_object_id;
EXEC sp_executesql @dropFk;
SQLBLK;
$out[] = "GO";

foreach (array_reverse(array_keys($tables)) as $t) {
    $out[] = "IF OBJECT_ID(N'dbo." . $t . "', N'U') IS NOT NULL DROP TABLE " . ident($t) . ";";
}
$out[] = "GO";

/* -- CREATE TABLE -- */
foreach ($tables as $tbl => $model) {
    $lines = [];

    foreach ($model['cols'] as $col) {
        $line = '    ' . ident($col['name']) . ' ' . $col['type'];

        if ($col['identity']) {
            $line .= ' IDENTITY(1,1)';
        } elseif ($col['default'] !== null) {
            $lit = $defaultLiteral($col['default'], $col['type']);
            if ($lit !== null) {
                $line .= ' CONSTRAINT ' . ident('DF_' . $tbl . '_' . $col['name']) . ' DEFAULT ' . $lit;
            }
        }

        $line .= $col['notNull'] ? ' NOT NULL' : ' NULL';
        $lines[] = $line;
    }

    if ($model['pk']) {
        $pkCols = implode(', ', array_map('ident', $model['pk']));
        $lines[] = '    CONSTRAINT ' . ident('PK_' . $tbl) . ' PRIMARY KEY CLUSTERED (' . $pkCols . ')';
    }

    $out[] = "CREATE TABLE " . ident($tbl) . " (";
    $out[] = implode(",\n", $lines);
    $out[] = ");";
    $out[] = "GO";
}

/* -- Data -- */
$CHUNK = 50;
foreach ($inserts as $ins) {
    $tbl = $ins['table'];
    if (!isset($tables[$tbl])) {
        $notes[] = "INSERT skipped, unknown table: $tbl";
        continue;
    }

    $hasIdentity = $tables[$tbl]['identity'] !== null;
    $colList = implode(', ', array_map('ident', $ins['cols']));

    // Map each column to its target type so literals can be emitted correctly.
    $typeOf = [];
    foreach ($tables[$tbl]['cols'] as $c) { $typeOf[strtolower($c['name'])] = $c['type']; }

    if ($hasIdentity) { $out[] = "SET IDENTITY_INSERT " . ident($tbl) . " ON;"; }

    foreach (array_chunk($ins['rows'], $CHUNK) as $chunk) {
        $tuples = [];
        foreach ($chunk as $row) {
            $vals = [];
            foreach ($row as $idx => $v) {
                $v = trim($v);
                $colName = $ins['cols'][$idx] ?? '';
                $type    = $typeOf[strtolower($colName)] ?? 'nvarchar(255)';

                if (strcasecmp($v, 'NULL') === 0) {
                    $vals[] = 'NULL';
                } elseif ($v !== '' && $v[0] === "'") {
                    $lit = convertLiteral($v);
                    // Numeric / date targets take a plain quoted literal, not N'..'.
                    if (preg_match('/^(bigint|int|smallint|tinyint|decimal|float|bit)/', $type)) {
                        $inner = substr($lit, 2, -1);
                        $vals[] = ($inner === '') ? 'NULL' : "'" . $inner . "'";
                    } else {
                        $vals[] = $lit;
                    }
                } else {
                    $vals[] = $v;
                }
            }
            $tuples[] = '(' . implode(', ', $vals) . ')';
        }

        $out[] = "INSERT INTO " . ident($tbl) . " (" . $colList . ") VALUES";
        $out[] = implode(",\n", $tuples) . ";";
    }

    if ($hasIdentity) { $out[] = "SET IDENTITY_INSERT " . ident($tbl) . " OFF;"; }
    $out[] = "GO";
}

/* -- Reseed identities -- */
foreach ($tables as $tbl => $model) {
    if ($model['identity'] !== null) {
        $out[] = "IF EXISTS (SELECT 1 FROM " . ident($tbl) . ") DBCC CHECKIDENT ('dbo." . $tbl . "', RESEED) WITH NO_INFOMSGS;";
    }
}
$out[] = "GO";

/* -- Unique constraints and indexes -- */
$seenIdx = [];
foreach ($tables as $tbl => $model) {
    foreach ($model['unique'] as $u) {
        $name = $u['name'] ?: ($tbl . '_' . implode('_', $u['cols']) . '_unique');
        if (isset($seenIdx[$name])) { $name .= '_' . $tbl; }
        $seenIdx[$name] = true;
        $out[] = "CREATE UNIQUE INDEX " . ident($name) . " ON " . ident($tbl)
               . " (" . implode(', ', array_map('ident', $u['cols'])) . ") WHERE "
               . implode(' AND ', array_map(fn($c) => ident($c) . ' IS NOT NULL', $u['cols'])) . ";";
    }
    foreach ($model['keys'] as $k) {
        $name = $k['name'] ?: ($tbl . '_' . implode('_', $k['cols']) . '_index');
        if (isset($seenIdx[$name])) { $name .= '_' . $tbl; }
        $seenIdx[$name] = true;
        $out[] = "CREATE INDEX " . ident($name) . " ON " . ident($tbl)
               . " (" . implode(', ', array_map('ident', $k['cols'])) . ");";
    }
}
$out[] = "GO";

/* -- Foreign keys -- */
foreach ($tables as $tbl => $model) {
    foreach ($model['fks'] as $fk) {
        $action = '';
        if (preg_match('/ON DELETE CASCADE/i', $fk['action'])) { $action .= ' ON DELETE CASCADE'; }
        if (preg_match('/ON UPDATE CASCADE/i', $fk['action'])) { $action .= ' ON UPDATE CASCADE'; }

        $out[] = "ALTER TABLE " . ident($tbl) . " ADD CONSTRAINT " . ident($fk['name'])
               . " FOREIGN KEY (" . implode(', ', array_map('ident', $fk['cols'])) . ")"
               . " REFERENCES " . ident($fk['refTbl']) . " (" . implode(', ', array_map('ident', $fk['refCols'])) . ")"
               . $action . ";";
    }
}
$out[] = "GO";

file_put_contents($dst, implode("\n", $out) . "\n");

/* ------------------------------------------------------------------ *
 * 6. Report.
 * ------------------------------------------------------------------ */
$totalRows = array_sum(array_map(fn($i) => count($i['rows']), $inserts));
echo "Tables parsed   : " . count($tables) . "\n";
echo "Insert batches  : " . count($inserts) . "\n";
echo "Data rows       : " . $totalRows . "\n";
echo "Identity tables : " . count(array_filter($tables, fn($t) => $t['identity'] !== null)) . "\n";
echo "Foreign keys    : " . array_sum(array_map(fn($t) => count($t['fks']), $tables)) . "\n";
echo "Unique indexes  : " . array_sum(array_map(fn($t) => count($t['unique']), $tables)) . "\n";
echo "Plain indexes   : " . array_sum(array_map(fn($t) => count($t['keys']), $tables)) . "\n";
echo "Output          : $dst (" . number_format(filesize($dst)) . " bytes)\n";

if ($notes) {
    echo "\nNotes:\n";
    foreach (array_unique($notes) as $n) { echo "  - $n\n"; }
}
