<?php

/*
|--------------------------------------------------------------------------
| ERP platform (single login across PMS / LIMS / HRMS)
|--------------------------------------------------------------------------
| The .NET PMS login is the only login. After it authenticates a user it
| writes a row to ERP_Platform.dbo.Sessions and sets the `erp_sso` cookie
| on the shared host. LIMS runs behind the PMS host under /lims and only
| trusts that cookie (App\Http\Middleware\ErpSso). Authorisation for the
| application comes from ERP_Platform (roles -> applications).
*/
return [
    'enabled'    => (bool) env('ERP_ENABLED', false),
    'app_code'   => env('ERP_APP_CODE', 'LIMS'),
    // The PMS host that serves the login, My Home and the /lims proxy
    'base_url'   => rtrim(env('ERP_BASE_URL', 'http://localhost:5217'), '/'),
    'login_path' => env('ERP_LOGIN_PATH', '/Login/Index'),
    'home_path'  => env('ERP_HOME_PATH', '/Apps'),
    'logout_path'=> env('ERP_LOGOUT_PATH', '/Login/SignOut'),
    'cookie'     => env('ERP_SSO_COOKIE', 'erp_sso'),
    // shared with the PMS host; lets the single login verify LIMS-native credentials (POST /erp/verify)
    'shared_secret' => env('ERP_SHARED_SECRET', ''),
    'connection' => env('ERP_DB_CONNECTION', 'erp'),
    // re-validate the SSO session against the central DB at most every N seconds
    'revalidate_seconds' => (int) env('ERP_REVALIDATE_SECONDS', 60),
];
