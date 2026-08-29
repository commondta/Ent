<?php

/*
|--------------------------------------------------------------------------
| Ignition (local error page) — offline hardening
|--------------------------------------------------------------------------
| Disables every feature of the Ignition error page that would reach out
| to an external host (Flare "share" button, remote solution fetching).
*/

return [
    'enable_share_button'      => false,
    'enable_runnable_solutions' => false,
    'register_commands'        => false,
];
