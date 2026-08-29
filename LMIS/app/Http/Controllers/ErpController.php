<?php

namespace App\Http\Controllers;

use App\Models\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;

/**
 * ERP platform endpoints used by the PMS host (the single login).
 *
 *  GET  /erp/touch   – "pre-authenticate": runs through ErpSso like any page and reports the
 *                      signed-in state. /Apps calls it right after login so LIMS is already
 *                      signed in before the user clicks its tile.
 *  POST /erp/verify  – credential check for LIMS-native accounts, so the ONE login page accepts
 *                      every solution's credentials. Only callable by the PMS host with the
 *                      shared secret; never returns the hash; rate limited.
 */
class ErpController extends Controller
{
    public function touch(Request $request)
    {
        return response()->json([
            'app'           => config('erp.app_code'),
            'authenticated' => Auth::check(),
            'user'          => Auth::check() ? ['id' => Auth::id(), 'name' => Auth::user()->name, 'email' => Auth::user()->email] : null,
        ], 200, ['Cache-Control' => 'no-store']);
    }

    public function verify(Request $request)
    {
        $secret = (string) config('erp.shared_secret');
        if ($secret === '' || !hash_equals($secret, (string) $request->header('X-Erp-Secret', ''))) {
            return response()->json(['ok' => false, 'error' => 'forbidden'], 403);
        }
        $username = trim((string) $request->input('username', ''));
        $password = (string) $request->input('password', '');
        if ($username === '' || $password === '') {
            return response()->json(['ok' => false], 200);
        }
        $user = User::where('email', $username)->orderBy('id')->first();
        if (!$user || !Hash::check($password, $user->password)) {
            return response()->json(['ok' => false], 200);
        }
        if ((int) ($user->isDeleted ?? 0) === 1) {
            return response()->json(['ok' => false], 200);
        }
        return response()->json([
            'ok'   => true,
            'user' => ['id' => $user->id, 'email' => $user->email, 'name' => $user->name, 'is_admin' => (int) ($user->is_admin ?? 0)],
        ]);
    }
}
