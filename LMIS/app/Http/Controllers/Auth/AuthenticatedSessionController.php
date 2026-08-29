<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Http\Requests\Auth\LoginRequest;
use App\Providers\RouteServiceProvider;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\DB;

class AuthenticatedSessionController extends Controller
{
    /**
     * Display the login view.
     *
     * With the ERP platform enabled there is a single login for the whole
     * solution – the .NET PMS login page – so this only redirects there.
     *
     * @return \Illuminate\View\View|\Illuminate\Http\RedirectResponse
     */
    public function create(Request $request)
    {
        if (config('erp.enabled')) {
            return redirect()->away(config('erp.base_url') . config('erp.login_path'));
        }
        return view('auth.login');
    }

    /**
     * Handle an incoming authentication request.
     *
     * @param  \App\Http\Requests\Auth\LoginRequest  $request
     * @return \Illuminate\Http\RedirectResponse
     */
    public function store(LoginRequest $request)
    {
        $request->authenticate();

        $request->session()->regenerate();

        return redirect()->intended(RouteServiceProvider::HOME);
    }

    /**
     * Destroy an authenticated session.
     *
     * Revokes the central ERP session as well and hands off to the ERP
     * sign-out so every application is signed out together.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\RedirectResponse
     */
    public function destroy(Request $request)
    {
        $token = (string) $request->cookie(config('erp.cookie'), '');

        Auth::guard('web')->logout();

        $request->session()->invalidate();

        $request->session()->regenerateToken();

        if (config('erp.enabled')) {
            if (preg_match('/^[A-Fa-f0-9]{64}$/', $token)) {
                try {
                    DB::connection(config('erp.connection'))->update(
                        'UPDATE dbo.Sessions SET RevokedAt = SYSUTCDATETIME() WHERE Token = ? AND RevokedAt IS NULL',
                        [$token]
                    );
                } catch (\Throwable $e) {
                    // the ERP sign-out below revokes as well
                }
            }
            return redirect()->away(config('erp.base_url') . config('erp.logout_path'))
                ->withCookie(cookie()->forget(config('erp.cookie'), '/'));
        }

        return redirect('/');
    }
}
