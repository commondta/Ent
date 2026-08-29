# LMIS — Land Management Information System

Laravel 8 application for land acquisition, exemption, intimation, registry and approvals workflows.

## Run locally

```
copy .env.example .env
php artisan key:generate
php artisan serve --host=127.0.0.1 --port=8000
```

Open http://127.0.0.1:8000/ — the server binds to the loopback interface only.

## Offline by design

All front-end libraries, fonts and icons are served from `public/vendors` and `public/assets`;
mail uses the `log` driver; the repository has no remote. No outbound network connection is made.

Documentation lives in `docs/` (see `docs/WORK-LOG.md` for the session history).
