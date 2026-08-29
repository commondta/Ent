<?php

namespace App\Http\Middleware;

use App\Models\User;
use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Schema;
use Illuminate\Support\Facades\View;
use Illuminate\Support\Str;

/**
 * Single sign-on from the ERP platform (the .NET PMS login).
 *
 * Every web request: read the `erp_sso` cookie, validate it against
 * ERP_Platform.dbo.Sessions, confirm the central user may use this
 * application (roles -> applications), map the central user to the local
 * LIMS user (by id, then e-mail; first-time ERP administrators are
 * provisioned with full rights), sign them in, and share the user's
 * application list with the views for the app switcher.
 *
 * Without a valid cookie the local auth state is cleared, so the `auth`
 * middleware sends the user to the ERP login. Local permission columns
 * on `users` continue to gate individual forms (application-level
 * authorisation is central; form-level stays app-local until phase 2).
 */
class ErpSso
{
    public function handle(Request $request, Closure $next)
    {
        if (!config('erp.enabled')) {
            return $next($request);
        }

        $token = (string) $request->cookie(config('erp.cookie'), '');
        $session = $request->session();

        if (!preg_match('/^[A-Fa-f0-9]{64}$/', $token)) {
            if ($session->has('erp_sso_token')) {
                $this->forgetLocalLogin($request);
            }
            return $next($request);
        }

        $fresh = $session->get('erp_sso_token') === $token
            && Auth::check()
            && (time() - (int) $session->get('erp_sso_checked_at', 0)) < config('erp.revalidate_seconds');

        if (!$fresh) {
            $row = $this->centralSession($token);
            if (!$row) {
                if ($session->has('erp_sso_token')) {
                    $this->forgetLocalLogin($request);
                }
                return $next($request);
            }

            $apps = $this->applicationsFor((int) $row->UserId);
            $mine = collect($apps)->first(fn ($a) => ($a['Code'] ?? '') === config('erp.app_code') && (bool) ($a['IsActive'] ?? true));   // inactive apps are listed ("soon") but do not grant access
            if (!$mine) {
                abort(403, 'Your ERP account has no access to the ' . config('app.name') . '.');
            }

            $user = $this->localUser($row, $apps);
            if (!Auth::check() || Auth::id() !== $user->id) {
                Auth::login($user);
                $session->regenerate();
            }

            $session->put([
                'erp_sso_token'      => $token,
                'erp_sso_checked_at' => time(),
                'erp_user_id'        => (int) $row->UserId,
                'erp_username'       => $row->Username,
                'erp_apps'           => $apps,
            ]);

            try {
                DB::connection(config('erp.connection'))->update(
                    'UPDATE dbo.Sessions SET LastSeenAt = SYSUTCDATETIME() WHERE Token = ?', [$token]
                );
            } catch (\Throwable $e) { /* heartbeat only */ }
        }

        View::share('erpApps', $session->get('erp_apps', []));
        View::share('erpCurrentApp', config('erp.app_code'));

        return $next($request);
    }

    /** The central session row joined with its user, or null when missing/expired/revoked. */
    protected function centralSession(string $token)
    {
        $row = DB::connection(config('erp.connection'))->selectOne(
            'SELECT s.UserId, s.ExpiresAt, s.RevokedAt, u.Username, u.Email, u.FullName, u.IsActive, u.LimsUserId
               FROM dbo.Sessions s JOIN dbo.Users u ON u.Id = s.UserId
              WHERE s.Token = ?', [$token]
        );
        if (!$row || $row->RevokedAt !== null || !$row->IsActive) {
            return null;
        }
        if (strtotime((string) $row->ExpiresAt . ' UTC') < time()) {
            return null;
        }
        return $row;
    }

    /** Applications (code, name, base url) the central user may open, via role -> application. */
    protected function applicationsFor(int $erpUserId): array
    {
        $rows = DB::connection(config('erp.connection'))->select(
            'SELECT DISTINCT a.Code, a.Name, a.Description, a.BaseUrl, a.SortOrder, a.IsActive,
                    CASE WHEN EXISTS (SELECT 1 FROM dbo.UserRoles x JOIN dbo.Roles r ON r.Id = x.RoleId WHERE x.UserId = ur.UserId AND r.Code = \'ERP_ADMIN\') THEN 1 ELSE 0 END AS IsErpAdmin
               FROM dbo.UserRoles ur
               JOIN dbo.RoleApplication ra ON ra.RoleId = ur.RoleId
               JOIN dbo.Applications a ON a.Id = ra.ApplicationId
              WHERE ur.UserId = ?
              ORDER BY a.SortOrder', [$erpUserId]
        );
        return array_map(fn ($r) => (array) $r, $rows);
    }

    /** Resolve (or provision) the local LIMS account for a central user. */
    protected function localUser($row, array $apps): User
    {
        $user = $row->LimsUserId ? User::find($row->LimsUserId) : null;
        if (!$user && $row->Email) {
            $user = User::where('email', $row->Email)->first();
        }
        if (!$user && $row->Username && str_contains($row->Username, '@')) {
            $user = User::where('email', $row->Username)->first();
        }
        if (!$user) {
            $isAdmin = (bool) ($apps[0]['IsErpAdmin'] ?? false);
            $user = new User();
            $user->name = $row->FullName ?: $row->Username;
            $user->email = $row->Email ?: ($row->Username . '@erp.local');
            $user->password = Hash::make(Str::random(40));
            $user->designation = $isAdmin ? 'ERP Administrator' : 'ERP User';
            $user->is_admin = $isAdmin ? 1 : 0;
            if ($isAdmin) {
                foreach (Schema::getColumnListing('users') as $col) {
                    if (preg_match('/_(list|add|edit|delete|print)$/', $col)) {
                        $user->{$col} = 1;
                    }
                }
            }
            $user->save();
        }
        if ((int) $row->LimsUserId !== (int) $user->id) {
            try {
                DB::connection(config('erp.connection'))->update(
                    'UPDATE dbo.Users SET LimsUserId = ? WHERE Id = ?', [$user->id, $row->UserId]
                );
            } catch (\Throwable $e) { /* mapping is a convenience; e-mail lookup still works */ }
        }
        return $user;
    }

    protected function forgetLocalLogin(Request $request): void
    {
        Auth::guard('web')->logout();
        $request->session()->forget(['erp_sso_token', 'erp_sso_checked_at', 'erp_user_id', 'erp_username', 'erp_apps']);
    }
}
