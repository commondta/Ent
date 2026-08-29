<?php

/**
 * Laravel - A PHP Framework For Web Artisans
 *
 * @package  Laravel
 * @author   Taylor Otwell <taylor@laravel.com>
 */

$uri = urldecode(
    parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH) ?? ''
);

// This application is deployed with the project root as the document root, so
// its assets are addressed as "/public/assets/...". Serve any real file below
// the project root, but never hand out configuration, source or VCS metadata.
$blocked = '#^/(\.|vendor/|storage/|app/|config/|database/|routes/|resources/|tests/|bootstrap/|artisan|composer\.|package\.|phpunit|webpack|vite\.|postcss|tailwind|server\.php)#i';

if ($uri !== '/' && preg_match($blocked, $uri)) {
    http_response_code(404);

    return true;
}

if ($uri !== '/') {
    // "php artisan serve" chdirs into public/, so the built-in server can only
    // resolve files below that directory. Anything it cannot reach itself --
    // notably the "/public/..." asset URLs this application generates -- is
    // streamed from the project root instead.
    $docRoot = realpath($_SERVER['DOCUMENT_ROOT'] ?? '') ?: '';

    if ($docRoot !== '' && is_file($docRoot.$uri)) {
        return false;
    }

    $path = realpath(__DIR__.$uri);

    if ($path !== false && is_file($path) && strpos($path, realpath(__DIR__)) === 0) {
        static $mimes = [
            'css'   => 'text/css',
            'js'    => 'application/javascript',
            'mjs'   => 'application/javascript',
            'json'  => 'application/json',
            'map'   => 'application/json',
            'html'  => 'text/html',
            'txt'   => 'text/plain',
            'xml'   => 'application/xml',
            'svg'   => 'image/svg+xml',
            'png'   => 'image/png',
            'jpg'   => 'image/jpeg',
            'jpeg'  => 'image/jpeg',
            'gif'   => 'image/gif',
            'webp'  => 'image/webp',
            'ico'   => 'image/x-icon',
            'bmp'   => 'image/bmp',
            'woff'  => 'font/woff',
            'woff2' => 'font/woff2',
            'ttf'   => 'font/ttf',
            'otf'   => 'font/otf',
            'eot'   => 'application/vnd.ms-fontobject',
            'pdf'   => 'application/pdf',
            'mp4'   => 'video/mp4',
            'webm'  => 'video/webm',
            'mp3'   => 'audio/mpeg',
            'zip'   => 'application/zip',
            'docx'  => 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            'xlsx'  => 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        ];

        $ext = strtolower(pathinfo($path, PATHINFO_EXTENSION));

        // Never execute PHP that happens to sit outside the document root.
        if ($ext === 'php' || $ext === 'phtml') {
            http_response_code(404);

            return true;
        }

        header('Content-Type: '.($mimes[$ext] ?? 'application/octet-stream'));
        header('Content-Length: '.filesize($path));
        readfile($path);

        return true;
    }
}

require_once __DIR__.'/public/index.php';
