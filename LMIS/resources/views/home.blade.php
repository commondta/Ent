@extends('layouts.main')

@section('content')
{{-- My Home — the LIMS landing workspace, the same shape as PMS's My Home:
     Overview Analytics · To-Dos · Apps · Recent · Favourites. Data from HomeController;
     Recent/Favourites live in the browser (lmis-theme.js records every form you open). --}}
<style>
    .lm-home { --mh-gap: 14px; }
    .lm-home-bar { display: flex; align-items: center; justify-content: space-between; gap: 12px; margin: 4px 0 16px; flex-wrap: wrap; }
    .lm-home-title { display: inline-flex; align-items: center; gap: 10px; font-size: 15px; font-weight: 600; text-transform: uppercase; letter-spacing: .04em; color: var(--lm-ink); margin: 0; }
    .lm-home-title .lm-title-icon { width: 30px; height: 30px; border-radius: 8px; background: var(--lm-ink); color: var(--lm-bg); display: inline-flex; align-items: center; justify-content: center; }
    .lm-home-title .lm-title-icon svg { width: 15px; height: 15px; stroke: currentColor; }
    .lm-home-sub { font-size: 12.5px; color: var(--lm-muted); }
    .lm-home-sub strong { color: var(--lm-text-2); font-weight: 600; }

    .mh-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: var(--mh-gap); }
    .mh-span-full { grid-column: 1 / -1; }
    @media (max-width: 900px) { .mh-grid { grid-template-columns: 1fr; } }

    .mh-card { background: var(--lm-card); border: 1px solid var(--lm-border); border-radius: 10px; box-shadow: 0 1px 3px rgba(16,21,31,.06); padding: 16px 18px; min-width: 0; transition: box-shadow .15s ease, transform .15s ease, border-color .15s ease; }
    .mh-card:hover { box-shadow: 0 6px 18px rgba(16,21,31,.12); transform: translateY(-2px); border-color: var(--lm-border-strong); }
    .mh-card-head { display: flex; align-items: center; justify-content: space-between; gap: 10px; margin-bottom: 12px; }
    .mh-card-title { font-size: 12px; font-weight: 700; color: var(--lm-text-2); text-transform: uppercase; letter-spacing: .08em; display: inline-flex; align-items: center; gap: 8px; }
    .mh-card-title svg { width: 14px; height: 14px; color: var(--lm-ink); opacity: .75; }
    .mh-card-link { font-size: 12px; font-weight: 600; color: var(--lm-ink); text-decoration: none; white-space: nowrap; }
    .mh-card-link:hover { text-decoration: underline; color: var(--lm-ink); }

    .mh-kpi-segments { display: flex; flex-wrap: wrap; gap: 14px 18px; }
    .mh-kpi-seg { flex: 1 1 auto; min-width: 150px; }
    .mh-kpi-seg + .mh-kpi-seg { border-left: 1px solid var(--lm-border); padding-left: 18px; }
    .mh-kpi-seg-title { display: block; font-size: 11px; font-weight: 700; color: var(--lm-muted); text-transform: uppercase; letter-spacing: .08em; margin-bottom: 8px; }
    .mh-kpi-row { display: flex; flex-wrap: wrap; gap: 10px; }
    .mh-kpi-tile { flex: 1 1 140px; min-width: 140px; max-width: 220px; border: 1px solid var(--lm-border-light); border-left: 3px solid var(--lm-ink); border-radius: 8px; background: var(--lm-surface); padding: 10px 12px; display: flex; flex-direction: column; gap: 2px; text-decoration: none; color: inherit; transition: border-color .15s ease, background .15s ease; }
    .mh-kpi-tile:hover { border-color: var(--lm-border-strong); border-left-color: var(--lm-ink); background: var(--lm-hover); color: inherit; }
    .mh-kpi-label { font-size: 11px; font-weight: 600; color: var(--lm-text-2); text-transform: uppercase; letter-spacing: .04em; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .mh-kpi-value { font-size: 22px; font-weight: 750; color: var(--lm-ink); line-height: 1.2; font-variant-numeric: tabular-nums; }
    .mh-kpi-delta { font-size: 10.5px; color: var(--lm-muted); }
    .mh-kpi-delta.up { color: var(--lm-success); }
    .mh-kpi-delta.down { color: var(--lm-danger); }
    .mh-note { margin-top: 10px; font-size: 11px; color: var(--lm-muted); font-style: italic; }

    .mh-todo-row { display: flex; flex-wrap: wrap; gap: 10px; }
    .mh-todo-chip { flex: 1 1 120px; display: flex; flex-direction: column; align-items: center; gap: 2px; border: 1px solid var(--lm-border-light); border-radius: 8px; background: var(--lm-surface); padding: 12px 10px; text-decoration: none; color: inherit; transition: border-color .15s ease, box-shadow .15s ease; min-width: 0; }
    a.mh-todo-chip:hover { border-color: var(--lm-border-strong); box-shadow: 0 1px 4px rgba(16,21,31,.08); color: inherit; }
    .mh-todo-count { font-size: 24px; font-weight: 750; color: var(--lm-ink); line-height: 1.1; font-variant-numeric: tabular-nums; }
    .mh-todo-label { font-size: 11px; font-weight: 600; color: var(--lm-text-2); text-transform: uppercase; letter-spacing: .05em; text-align: center; }

    .mh-apps-row { display: grid; grid-template-columns: repeat(auto-fill, minmax(170px, 1fr)); gap: 10px; }
    .mh-app-tile { border: 1px solid var(--lm-border); border-radius: 8px; background: var(--lm-surface); padding: 12px; display: flex; align-items: center; gap: 10px; text-decoration: none; color: inherit; min-width: 0; transition: border-color .15s ease, box-shadow .15s ease; }
    a.mh-app-tile:hover { border-color: var(--lm-border-strong); box-shadow: 0 1px 4px rgba(16,21,31,.1); color: inherit; }
    .mh-app-tile.disabled { opacity: .55; cursor: default; }
    /* the solution you are in: ink outline, live dot, a slow breathing ring (monochrome) */
    .mh-app-tile.is-current { border-color: var(--lm-ink); box-shadow: inset 3px 0 0 var(--lm-ink); background: var(--lm-card); animation: mhCurrentRing 2.6s ease-in-out infinite; }
    .mh-app-tile.is-current .mh-app-state { color: var(--lm-ink); display: inline-flex; align-items: center; gap: 6px; }
    .mh-app-tile.is-current .mh-app-state::before { content: ''; width: 7px; height: 7px; border-radius: 50%; background: var(--lm-ink); box-shadow: 0 0 0 0 rgba(17,17,17,.35); animation: mhCurrentDot 1.8s ease-out infinite; }
    @keyframes mhCurrentRing { 0%, 100% { box-shadow: inset 3px 0 0 var(--lm-ink), 0 0 0 0 rgba(17,17,17,0); } 50% { box-shadow: inset 3px 0 0 var(--lm-ink), 0 0 0 4px rgba(17,17,17,.08); } }
    @keyframes mhCurrentDot { 0% { box-shadow: 0 0 0 0 rgba(17,17,17,.35); } 70% { box-shadow: 0 0 0 6px rgba(17,17,17,0); } 100% { box-shadow: 0 0 0 0 rgba(17,17,17,0); } }
    html.dark .mh-app-tile.is-current .mh-app-state::before { box-shadow: 0 0 0 0 rgba(245,245,245,.35); }
    @media (prefers-reduced-motion: reduce) { .mh-app-tile.is-current, .mh-app-tile.is-current .mh-app-state::before { animation: none; } }
    .mh-app-icon { flex: 0 0 34px; width: 34px; height: 34px; border-radius: 8px; background: var(--lm-ink); color: var(--lm-bg); font-size: 12px; font-weight: 700; display: inline-flex; align-items: center; justify-content: center; letter-spacing: .03em; overflow: hidden; }
    .mh-app-icon img { width: 26px; height: 26px; display: block; }
    .mh-app-name { font-size: 12.5px; font-weight: 650; color: var(--lm-ink); line-height: 1.3; }
    .mh-app-state { font-size: 10.5px; color: var(--lm-muted); font-weight: 600; text-transform: uppercase; letter-spacing: .05em; }

    .mh-list { list-style: none; margin: 0; padding: 0; }
    .mh-list li { display: flex; align-items: center; justify-content: space-between; gap: 8px; padding: 7px 2px; border-bottom: 1px solid var(--lm-border-light); min-width: 0; }
    .mh-list li:last-child { border-bottom: none; }
    .mh-list a.mh-item-link { font-size: 13px; font-weight: 600; color: var(--lm-ink); text-decoration: none; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; min-width: 0; }
    .mh-list a.mh-item-link small { font-weight: 500; color: var(--lm-muted); margin-left: 6px; }
    .mh-list a.mh-item-link:hover { text-decoration: underline; }
    .mh-empty { font-size: 12px; color: var(--lm-muted); font-style: italic; padding: 8px 0; }

    @media (prefers-reduced-motion: reduce) {
        .mh-card, .mh-kpi-tile, .mh-todo-chip, .mh-app-tile { transition: none !important; }
        .mh-card:hover { transform: none; }
    }
</style>

<div class="content lm-home lm-no-crumbs">
    <div class="mt-3">
        <div class="lm-home-bar">
            <h4 class="lm-home-title"><span class="lm-title-icon" aria-hidden="true"><span data-feather="home"></span></span>My Home</h4>
            <span class="lm-home-sub">Welcome back, <strong>{{ auth()->user()->name }}</strong> &middot; {{ now()->format('l, d F Y') }}</span>
        </div>

        <div class="mh-grid">

            {{-- Overview Analytics --}}
            <div class="mh-card mh-span-full">
                <div class="mh-card-head">
                    <span class="mh-card-title"><span data-feather="bar-chart-2"></span>Overview Analytics</span>
                </div>
                @if(count($segments))
                    <div class="mh-kpi-segments">
                        @foreach($segments as $title => $tiles)
                            <div class="mh-kpi-seg">
                                <span class="mh-kpi-seg-title">{{ $title }}</span>
                                <div class="mh-kpi-row">
                                    @foreach($tiles as $t)
                                        <a class="mh-kpi-tile" href="{{ $t['href'] }}" title="Open {{ $t['label'] }}">
                                            <span class="mh-kpi-label">{{ $t['label'] }}</span>
                                            <span class="mh-kpi-value">{{ number_format($t['total']) }}</span>
                                            <span class="mh-kpi-delta {{ $t['trend'] }}">
                                                @if($t['trend'] === 'up') &#9650; @elseif($t['trend'] === 'down') &#9660; @else &#8212; @endif
                                                {{ $t['month'] }} this month
                                            </span>
                                        </a>
                                    @endforeach
                                </div>
                            </div>
                        @endforeach
                    </div>
                    <div class="mh-note">Live document counts (deleted records excluded); the arrow compares this month's new documents with last month's.</div>
                @else
                    <div class="mh-empty">No modules are assigned to your account yet — ask an administrator for access.</div>
                @endif
            </div>

            {{-- To-Dos --}}
            <div class="mh-card">
                <div class="mh-card-head">
                    <span class="mh-card-title"><span data-feather="check-square"></span>To-Dos</span>
                </div>
                <div class="mh-todo-row">
                    <a class="mh-todo-chip" href="{{ route('approval_inbox', auth()->id()) }}" title="Documents waiting for your approval">
                        <span class="mh-todo-count">{{ $approvals }}</span>
                        <span class="mh-todo-label">Approvals</span>
                    </a>
                    <a class="mh-todo-chip" href="{{ route('pending_documents', auth()->id()) }}" title="Your documents still in the approval chain">
                        <span class="mh-todo-count">{{ $pending }}</span>
                        <span class="mh-todo-label">Pending</span>
                    </a>
                    @if($isAdmin)
                        <a class="mh-todo-chip" href="{{ route('users.index') }}" title="Users Management">
                            <span class="mh-todo-count">{{ $users }}</span>
                            <span class="mh-todo-label">Active Users</span>
                        </a>
                    @else
                        <span class="mh-todo-chip">
                            <span class="mh-todo-count">{{ $users }}</span>
                            <span class="mh-todo-label">Active Users</span>
                        </span>
                    @endif
                </div>
            </div>

            {{-- Apps --}}
            <div class="mh-card">
                <div class="mh-card-head">
                    <span class="mh-card-title"><span data-feather="grid"></span>Apps</span>
                    @if($erpHomeUrl)<a class="mh-card-link" href="{{ $erpHomeUrl }}">All applications &rarr;</a>@endif
                </div>
                <div class="mh-apps-row">
                    @foreach($apps as $app)
                        @php
                            $initials = implode('', array_map(fn ($w) => mb_substr($w, 0, 1), array_slice(array_filter(explode(' ', $app['name']), fn ($w) => $w !== '' && ctype_upper(mb_substr($w, 0, 1))), 0, 3))) ?: $app['code'];
                            $icon = $app['code'] === 'LIMS' ? asset('public/assets/img/lmis-icon.svg') : ($app['code'] === 'PAYROLL' && config('erp.enabled') ? config('erp.base_url') . '/payroll/Content/brand/payroll-mark-white.svg' : ($app['code'] === 'PMS' && config('erp.enabled') ? config('erp.base_url') . '/img/brand/pms-icon-plain-white.svg' : null));
                        @endphp
                        @if($app['enabled'] && !$app['current'])
                            <a class="mh-app-tile" href="{{ $app['url'] }}">
                                <span class="mh-app-icon">@if($icon)<img src="{{ $icon }}" alt="">@else{{ $initials }}@endif</span>
                                <span><span class="mh-app-name">{{ $app['name'] }}</span><br><span class="mh-app-state">Open</span></span>
                            </a>
                        @else
                            <span class="mh-app-tile {{ $app['current'] ? 'is-current' : 'disabled' }}" @if($app['current']) aria-current="true" @endif>
                                <span class="mh-app-icon">@if($icon)<img src="{{ $icon }}" alt="">@else{{ $initials }}@endif</span>
                                <span><span class="mh-app-name">{{ $app['name'] }}</span><br><span class="mh-app-state">{{ $app['current'] ? 'Current' : 'Coming soon' }}</span></span>
                            </span>
                        @endif
                    @endforeach
                </div>
            </div>

            {{-- Recent --}}
            <div class="mh-card">
                <div class="mh-card-head">
                    <span class="mh-card-title"><span data-feather="clock"></span>Recent</span>
                </div>
                <ul class="mh-list" id="mhRecentList"></ul>
                <div class="mh-empty" id="mhRecentEmpty">No recently opened forms yet — they will appear here as you work.</div>
            </div>

            {{-- Favourites --}}
            <div class="mh-card">
                <div class="mh-card-head">
                    <span class="mh-card-title"><span data-feather="star"></span>Favourites</span>
                </div>
                <ul class="mh-list" id="mhFavList"></ul>
                <div class="mh-empty" id="mhFavEmpty">No favourites yet — star a form in the Recent list to keep it here.</div>
            </div>

        </div>
    </div>
</div>

<script>
    /* Recent & Favourites cards — rendered from the shared store (window.limsRecentFav in lmis-theme.js),
       so the header popovers, the page-bar star and these cards always agree. */
    document.addEventListener('DOMContentLoaded', function () {
        var LRF = window.limsRecentFav; if (!LRF) return;
        function render() {
            var favs = LRF.favs(), map = {}; favs.forEach(function (f) { map[f.u] = true; });
            var recents = LRF.recents().slice(0, 6);
            document.getElementById('mhRecentList').innerHTML = recents.map(function (e) { return LRF.itemHtml(e, !!map[e.u], 'mh-item'); }).join('');
            document.getElementById('mhRecentEmpty').style.display = recents.length ? 'none' : '';
            document.getElementById('mhFavList').innerHTML = favs.map(function (e) { return LRF.itemHtml(e, true, 'mh-item'); }).join('');
            document.getElementById('mhFavEmpty').style.display = favs.length ? 'none' : '';
        }
        render();
        LRF.onChange(render);
    });
</script>
@endsection
