{{-- Application switcher (ERP single solution) — the same menu as PMS's header switcher:
     My Home · "Applications" · one row per application with its logo tile (current = black tile + check,
     inactive = "soon"). Other applications open through the host's /Apps/Go so the central session
     carries over — no second login. --}}
@if(config('erp.enabled'))
@php
    $erpBase   = config('erp.base_url');
    $current   = $erpCurrentApp ?? config('erp.app_code');
    $appsList  = collect($erpApps ?? [])->sortBy(fn ($a) => $a['SortOrder'] ?? 99)->values();
@endphp
<div class="dropdown-menu lm-appswitch-menu" role="menu" aria-label="Applications">
    <a class="dropdown-item lm-appswitch-home" role="menuitem" href="{{ $erpBase . config('erp.home_path') }}">
        <span data-feather="grid"></span><span>Applications Library</span>
    </a>
    <div class="lm-appswitch-sep"></div>
    <span class="lm-appswitch-label">Applications</span>
    @forelse($appsList as $app)
        @php
            $code      = $app['Code'] ?? '';
            $isCurrent = $code === $current;
            $isActive  = (bool) ($app['IsActive'] ?? true);
            $href      = $isCurrent ? route('home') : ($isActive ? $erpBase . '/Apps/Go?code=' . urlencode($code) : '#');
        @endphp
        <a class="dropdown-item lm-appswitch-app {{ $isCurrent ? 'is-current' : '' }} {{ $isActive ? '' : 'is-off' }}" role="menuitem" href="{{ $href }}"
           @if($isCurrent) aria-current="true" @endif @if(!$isActive) aria-disabled="true" tabindex="-1" @endif>
            <span class="lm-appswitch-logo" aria-hidden="true">
                @if($code === 'LIMS')
                    <img src="{{ asset('public/assets/img/lmis-logo-dark.svg') }}" alt="" class="lm-logo-img lm-logo-img-dark"><img src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt="" class="lm-logo-img lm-logo-img-light">
                @elseif($code === 'PMS')
                    <img src="{{ $erpBase }}/img/brand/pms-icon-plain.svg" alt="" class="lm-logo-img lm-logo-img-dark"><img src="{{ $erpBase }}/img/brand/pms-icon-plain-white.svg" alt="" class="lm-logo-img lm-logo-img-light">
                @elseif($code === 'PAYROLL')
                    <img src="{{ $erpBase }}/payroll/Content/brand/payroll-mark.svg" alt="" class="lm-logo-img lm-logo-img-dark"><img src="{{ $erpBase }}/payroll/Content/brand/payroll-mark-white.svg" alt="" class="lm-logo-img lm-logo-img-light">
                @else
                    <svg viewBox="0 0 24 24" class="lm-logo-line"><circle cx="9" cy="8" r="3.5"/><path d="M2.5 20c0-3.6 2.9-6 6.5-6s6.5 2.4 6.5 6"/><circle cx="17" cy="9" r="2.5"/><path d="M16 14c3 0 5.5 2 5.5 5"/></svg>
                @endif
            </span>
            <span class="lm-appswitch-name">{{ $app['Name'] ?? $code }}</span>
            @if($isCurrent)<span class="lm-appswitch-check" aria-hidden="true"><svg viewBox="0 0 24 24"><polyline points="20 6 9 17 4 12"/></svg></span>@endif
            @if(!$isActive)<span class="lm-appswitch-soon">soon</span>@endif
        </a>
    @empty
        <span class="dropdown-item disabled">No applications assigned</span>
    @endforelse
</div>
@endif
