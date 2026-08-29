<?php

namespace App\Http\Controllers;

use App\Models\Affidavit_2;
use App\Models\Agreement;
use App\Models\Challan_fee;
use App\Models\Challan_form_header;
use App\Models\Conveyance;
use App\Models\Document_approval;
use App\Models\Exemption_form;
use App\Models\Exemption_inventory_approval;
use App\Models\Exemption_rate;
use App\Models\Indemnity_bond;
use App\Models\Int_application;
use App\Models\Intimation_letter;
use App\Models\Land_form;
use App\Models\Land_provider;
use App\Models\Pictorial_view;
use App\Models\Possession_certificate;
use App\Models\Purchase_of_land;
use App\Models\Registry_document;
use App\Models\Seller_profile;
use App\Models\Undertaking;
use App\Models\User;
use Carbon\Carbon;
use Illuminate\Support\Facades\Log;

/**
 * LIMS landing page — "My Home", the same workspace PMS opens on:
 * Overview Analytics (document counts per module, permission-gated like the
 * sidebar), To-Dos (approvals waiting for me, my documents awaiting approval,
 * users), Apps (the ERP applications), Recent and Favourites (browser-side,
 * fed by lmis-theme.js).
 */
class HomeController extends Controller
{
    /** Document register: [model, tile label, index route, *_list permission flag] per module. */
    private const SEGMENTS = [
        'Land Acquisition' => [
            [Land_provider::class,               'Land Providers',          'land_provider.index',          'lp_master_data_list'],
            [Seller_profile::class,              'Land Owners',             'seller_profile.index',         'seller_profile_list'],
            [Land_form::class,                   'Land Offer Forms',        'land_form.index',              'land_form_seller_list'],
            [Purchase_of_land::class,            'Purchases of Land',       'purchase_of_land.index',       'purchase_of_land_list'],
            [Exemption_inventory_approval::class,'Exemption Inventory',     'exemption_inventory.index',    'exemption_inventory_list'],
            [Possession_certificate::class,      'Possession Certificates', 'possession_certificate.index', 'possession_certificate_list'],
            [Pictorial_view::class,              'Pictorial Views',         'pictorial_view.index',         'pictorial_view_list'],
        ],
        'Legal Documents' => [
            [Conveyance::class,        'Conveyance Deeds',   'conveyance.index',        'conveyance_deed_list'],
            [Agreement::class,         'Agreements',         'agreement.index',         'agreement_list'],
            [Affidavit_2::class,       'Affidavits',         'affidavit_2.index',       'affidavit_2_list'],
            [Undertaking::class,       'Undertakings',       'undertaking.index',       'undertaking_list'],
            [Indemnity_bond::class,    'Indemnity Bonds',    'indemnity_bond.index',    'indemnity_bond_list'],
            [Registry_document::class, 'Registry Documents', 'registry_document.index', 'registry_document_list'],
        ],
        'Exemption & Intimation' => [
            [Exemption_form::class,      'Exemption Forms',         'exemption_form.index',         'exemption_form_list'],
            [Exemption_rate::class,      'Exemption Rates',         'exemption_rate.index',         'exemption_rate_list'],
            [Challan_fee::class,         'Challan Fees',            'challan_fee.index',            'challan_fee_list'],
            [Challan_form_header::class, 'Challan Forms',           'challan_form.index',           'challan_form_list'],
            [Int_application::class,     'Intimation Applications', 'intimation_application.index', 'intimation_application_list'],
            [Intimation_letter::class,   'Intimation Letters',      'intimation_letter.index',      'intimation_letter_list'],
        ],
    ];

    /** Overview Analytics shows only these tiles (user instruction 2026-08-23); SEGMENTS stays the full register for the pending count. */
    private const ANALYTICS = [
        'Land Acquisition' => ['Land Providers', 'Land Owners', 'Land Offer Forms'],
        'Legal Documents'  => ['Conveyance Deeds', 'Agreements', 'Affidavits', 'Undertakings', 'Indemnity Bonds'],
    ];

    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {
        $user    = auth()->user();
        $isAdmin = (int) ($user->is_admin ?? 0) === 1;
        $can     = function (string $flag) use ($user, $isAdmin): bool {
            return $isAdmin || (int) ($user->{$flag} ?? 0) === 1;
        };

        $now        = Carbon::now();
        $monthStart = $now->copy()->startOfMonth();
        $prevStart  = $monthStart->copy()->subMonth();

        $segments = [];
        foreach (self::ANALYTICS as $title => $labels) {
            $tiles = [];
            foreach (self::SEGMENTS[$title] as [$model, $label, $route, $flag]) {
                if (!in_array($label, $labels, true) || !$can($flag)) {
                    continue;
                }
                $tiles[] = $this->tile($model, $label, $route, $monthStart, $prevStart);
            }
            if ($tiles) {
                $segments[$title] = $tiles;
            }
        }

        // Approvals waiting for me — same predicate as the header inbox count.
        $approvals = $this->safeCount(function () use ($user) {
            return Document_approval::where('approval_user_id', $user->id)
                ->where('status', '!=', 1)->where('isDeleted', 0)->where('priority', 1)->count();
        });

        // My own documents still in the approval chain (status 1 = submitted), as pending_documents counts them.
        $pending = 0;
        foreach (self::SEGMENTS as $rows) {
            foreach ($rows as [$model]) {
                $pending += $this->safeCount(function () use ($model, $user) {
                    return $model::where('createdBy', $user->id)->where('status', 1)->where('isDeleted', 0)->count();
                });
            }
        }

        $users = $this->safeCount(function () {
            return User::where('isDeleted', 0)->count();
        });

        // ERP applications (shared by the ErpSso middleware); LIMS alone when the platform is off.
        $apps = [];
        if (config('erp.enabled')) {
            $current = config('erp.app_code');
            foreach ((array) view()->shared('erpApps', []) as $app) {
                $code = $app['Code'] ?? '';
                $apps[] = [
                    'name'    => $app['Name'] ?? $code,
                    'code'    => $code,
                    'url'     => $code === $current ? route('home') : config('erp.base_url') . ($app['BaseUrl'] ?? '/'),
                    'current' => $code === $current,
                    'enabled' => (bool) ($app['IsActive'] ?? true), // the central query already filters IsActive = 1
                ];
            }
        }
        if (!$apps) {
            $apps[] = ['name' => 'Land Management Information System', 'code' => 'LIMS', 'url' => route('home'), 'current' => true, 'enabled' => true];
        }

        return view('home', [
            'segments'   => $segments,
            'approvals'  => $approvals,
            'pending'    => $pending,
            'users'      => $users,
            'apps'       => $apps,
            'isAdmin'    => $isAdmin,
            'erpHomeUrl' => config('erp.enabled') ? config('erp.base_url') . config('erp.home_path') : null,
        ]);
    }

    /** One KPI tile: total live documents + movement this month against last month. */
    private function tile(string $model, string $label, string $route, Carbon $monthStart, Carbon $prevStart): array
    {
        $total = $this->safeCount(fn () => $model::where('isDeleted', 0)->count());
        $month = $this->safeCount(fn () => $model::where('isDeleted', 0)->where('created_at', '>=', $monthStart)->count());
        $prev  = $this->safeCount(fn () => $model::where('isDeleted', 0)
            ->where('created_at', '>=', $prevStart)->where('created_at', '<', $monthStart)->count());

        return [
            'label' => $label,
            'href'  => route($route),
            'total' => $total,
            'month' => $month,
            'trend' => $month > $prev ? 'up' : ($month < $prev ? 'down' : 'flat'),
        ];
    }

    private function safeCount(callable $query): int
    {
        try {
            return (int) $query();
        } catch (\Throwable $e) {
            Log::warning('Home KPI query failed: ' . $e->getMessage());
            return 0;
        }
    }
}
