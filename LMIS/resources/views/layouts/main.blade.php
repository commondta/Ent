<!DOCTYPE html>
<html lang="en-US" dir="ltr">

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- ===============================================-->
    <!--    Document Title-->
    <!-- ===============================================-->
    <title>{{ config('app.name', 'LMIS') }}</title>
    <!-- ===============================================-->
    <!--    Favicons-->
    <!-- ===============================================-->
    <link rel="apple-touch-icon" sizes="180x180" href="{{ asset('public/assets/img/favicons/apple-touch-icon.png') }}">
    <link rel="icon" type="image/png" sizes="32x32" href="{{ asset('public/assets/img/favicons/favicon-32x32.png') }}">
    <link rel="icon" type="image/png" sizes="16x16" href="{{ asset('public/assets/img/favicons/favicon-16x16.png') }}">
    <link rel="icon" type="image/svg+xml" href="{{ asset('public/assets/img/lmis-icon.svg') }}">
    <link rel="shortcut icon" href="{{ asset('public/assets/img/lmis-icon.png') }}">
    <link rel="manifest" href="{{ asset('public/assets/img/favicons/manifest.json'); }}">
    <meta name="msapplication-TileImage" content="{{ asset('public/assets/img/favicons/mstile-150x150.png') }}">
    <meta name="theme-color" content="#ffffff">
    <meta name="lm-home-url" content="{{ route('home') }}">
    <script src="{{ asset('public/vendors/imagesloaded/imagesloaded.pkgd.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/simplebar/simplebar.min.js'); }}"></script>
    <script src="{{ asset('public/assets/js/config.js'); }}"></script>

    <!-- ===============================================-->
    <!--    Stylesheets-->
    <!-- ===============================================-->
    <link href="{{ asset('public/assets/css/nunito-sans.css') }}" rel="stylesheet">
    <link href="{{ asset('public/vendors/simplebar/simplebar.min.css'); }}" rel="stylesheet">
    <link rel="stylesheet" href="{{ asset('public/assets/css/line.css') }}">
    <link href="{{ asset('public/assets/css/theme-rtl.min.css'); }}" type="text/css" rel="stylesheet" id="style-rtl">
    <link href="{{ asset('public/assets/css/theme.min.css'); }}" type="text/css" rel="stylesheet" id="style-default">
    <link href="{{ asset('public/assets/css/user-rtl.min.css'); }}" type="text/css" rel="stylesheet" id="user-style-rtl">
    <link href="{{ asset('public/assets/css/user.min.css'); }}" type="text/css" rel="stylesheet" id="user-style-default">
    <style>
        thead {
            border: 1px solid var(--lm-border) !important;
            text-align: left;
            background-color: var(--lm-surface);
            padding: 8px;
        }

        th {
            border: 1px solid var(--lm-border) !important;
            text-align: left;
            background-color: var(--lm-surface);
            padding: 8px;
        }


        /* td {
        border: 1px solid black !important;
        width: 130px;

    } */
        /* ===== Reusable Multi Select ===== */

        .multi-select-wrapper {
            position: relative;
            width: 100%;
        }

        .multi-select-display {
            display: flex;
            align-items: center;
            flex-wrap: wrap;
            gap: 6px;
            padding: 8px 12px;
            border: 1px solid var(--lm-border);
            border-radius: 6px;
            background: #fff;
            cursor: pointer;
            min-height: 38px;
            transition: border-color .15s, box-shadow .15s;
        }

        .multi-select-display:hover {
            border-color: var(--lm-border-strong);
        }

        .multi-select-display.active {
            border-color: var(--lm-ink);
            box-shadow: 0 0 0 3px var(--lm-focus-ring);
        }

        .multi-select-placeholder {
            color: #999;
            font-size: 13px;
        }

        .multi-select-selected {
            display: flex;
            flex-wrap: wrap;
            gap: 6px;
            flex: 1;
        }

        .multi-select-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: var(--lm-ink);
            color: var(--lm-bg);
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 12px;
        }

        .multi-select-remove {
            cursor: pointer;
            font-weight: bold;
        }

        .multi-select-arrow {
            font-size: 12px;
            color: #666;
            transition: transform .2s;
        }

        .multi-select-display.active .multi-select-arrow {
            transform: rotate(180deg);
        }

        .multi-select-options {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            background: #fff;
            border: 1px solid var(--lm-border);
            border-top: none;
            z-index: 1000;
            max-height: 300px;
            overflow-y: auto;
        }

        .multi-select-option {
            display: flex;
            align-items: center;
            padding: 8px 12px;
            cursor: pointer;
            border-bottom: 1px solid #f1f1f1;
        }

        .multi-select-option:hover {
            background: #f8f9fa;
        }

        .multi-select-option input {
            margin-right: 8px;
        }

        input[readonly] {
            background-color: var(--lm-disabled-bg) !important;
            cursor: not-allowed;
        }
    </style>
    <style>
        .lo-table {
            width: 100%;
            border-collapse: collapse;
        }

        .lo-table th {
            background: var(--lm-surface);
            color: var(--lm-ink);
            font-weight: 600;
            text-align: center;
            padding: 10px;
            border: 1px solid var(--lm-border);
            white-space: nowrap;
            /* prevents word breaking */
        }

        .lo-table td {
            border: 1px solid var(--lm-border);
            padding: 4px;
        }

        .lo-table input,
        .lo-table select {
            width: 100%;
            height: 36px;
            border-radius: 6px;
            border: 1px solid var(--lm-border);

            padding: 5px;
        }

        .table-wrapper {
            overflow-x: auto;
        }

        .lo-table th:nth-child(1) {
            min-width: 140px;
        }

        .lo-table th:nth-child(2) {
            min-width: 160px;
        }

        .lo-table th:nth-child(3) {
            min-width: 200px;
        }

        .lo-table th:nth-child(4) {
            min-width: 120px;
        }

        .lo-table th:nth-child(5) {
            min-width: 200px;
        }

        .lo-table th:nth-child(6) {
            min-width: 200px;
        }

        .lo-table th:nth-child(7) {
            min-width: 200px;
        }

        .lo-table th:nth-child(8) {
            min-width: 160px;
        }

        .lo-table th:nth-child(9) {
            min-width: 120px;
        }

        .lo-table th:nth-child(10) {
            min-width: 160px;
        }

        .lo-table th:nth-child(11) {
            min-width: 200px;
        }
    </style>
    <script>
        document.addEventListener('DOMContentLoaded', function() {

            document.querySelectorAll('.multi-select-wrapper').forEach(wrapper => {

                const display = wrapper.querySelector('.multi-select-display');
                const options = wrapper.querySelector('.multi-select-options');
                const checkboxes = wrapper.querySelectorAll('.multi-select-checkbox');
                const selectedBox = wrapper.querySelector('.multi-select-selected');
                const placeholder = wrapper.querySelector('.multi-select-placeholder');
                const requiredInp = wrapper.querySelector('.multi-select-required');
                const errorBox = wrapper.closest('.col-md-12').querySelector('.multi-select-error');

                display.addEventListener('click', e => {
                    e.stopPropagation();
                    options.style.display = options.style.display === 'none' ? 'block' : 'none';
                    display.classList.toggle('active');
                });

                checkboxes.forEach(cb => {
                    cb.addEventListener('change', updateSelected);
                });

                function updateSelected() {
                    selectedBox.innerHTML = '';
                    const selected = Array.from(checkboxes).filter(cb => cb.checked);

                    if (selected.length > 0) {
                        placeholder.style.display = 'none';
                        requiredInp.value = '1';
                        errorBox.style.display = 'none';

                        selected.forEach(cb => {
                            const badge = document.createElement('div');
                            badge.className = 'multi-select-badge';
                            badge.innerHTML = `
                        <span>${cb.nextElementSibling.textContent}</span>
                        <span class="multi-select-remove" data-value="${cb.value}">×</span>
                    `;
                            selectedBox.appendChild(badge);
                        });

                    } else {
                        placeholder.style.display = 'inline';
                        requiredInp.value = '';
                        errorBox.style.display = 'block';
                    }
                }

                selectedBox.addEventListener('click', e => {
                    if (e.target.classList.contains('multi-select-remove')) {
                        const value = e.target.dataset.value;
                        checkboxes.forEach(cb => {
                            if (cb.value === value) cb.checked = false;
                        });
                        updateSelected();
                    }
                });

                document.addEventListener('click', e => {
                    if (!wrapper.contains(e.target)) {
                        options.style.display = 'none';
                        display.classList.remove('active');
                    }
                });

                updateSelected();
            });

        });
    </script>

    <script>
        var phoenixIsRTL = window.config.config.phoenixIsRTL;
        if (phoenixIsRTL) {
            var linkDefault = document.getElementById('style-default');
            var userLinkDefault = document.getElementById('user-style-default');
            linkDefault.setAttribute('disabled', true);
            userLinkDefault.setAttribute('disabled', true);
            document.querySelector('html').setAttribute('dir', 'rtl');
        } else {
            var linkRTL = document.getElementById('style-rtl');
            var userLinkRTL = document.getElementById('user-style-rtl');
            linkRTL.setAttribute('disabled', true);
            userLinkRTL.setAttribute('disabled', true);
        }
    </script>
    <style>
        @media print {

            .no-print,
            .no-print * {
                display: none !important;
            }

            .footer .position-absolute {
                display: none !important;

            }

            .card .setting-toggle {
                display: none !important;

            }

        }

        .count-indicator1 {
            position: absolute;
            top: 15px;
            right: 5px;
            background-color: red;
            color: white;
            border-radius: 50%;
            padding: 0.25em 0.5em;
            font-size: 0.75rem;
            font-weight: bold;
            transform: translate(50%, -50%);
        }
    </style>
    <link href="{{ asset('public/vendors/leaflet/leaflet.css'); }}" rel="stylesheet">
    <link href="{{ asset('public/vendors/leaflet.markercluster/MarkerCluster.css'); }}" rel="stylesheet">
    <link href="{{ asset('public/vendors/leaflet.markercluster/MarkerCluster.Default.css'); }}" rel="stylesheet">
    <!-- DataTables CSS -->
    <link href="{{ asset('public/vendors/datatables/dataTables.bootstrap5.min.css') }}" rel="stylesheet">
    <link href="{{ asset('public/vendors/datatables/responsive.bootstrap5.min.css') }}" rel="stylesheet">
    <script src="{{ asset('public/vendors/jquery/jquery-3.6.0.min.js') }}"></script>
    <script>
        var baseUrl = '{{ config("app.url") }}';
    </script>
    <!-- Custom Premium Global Styles -->
    <link rel="stylesheet" href="{{ asset('public/assets/css/custom-premium.css') }}">
    <!-- LMIS monochrome theme layer (Inter + tokens + components) -->
    <link rel="stylesheet" href="{{ asset('public/assets/css/inter.css') }}">
    <link rel="stylesheet" href="{{ asset('public/assets/css/lmis-theme.css') }}?v=20260828b">
</head>

<body>
    <!-- ===============================================-->
    <!--    Main Content-->
    <!-- ===============================================-->
    <main class="main" id="top">
        <nav class="navbar navbar-vertical navbar-expand-lg" style="display:none;">



            <div class="collapse navbar-collapse" id="navbarVerticalCollapse">
                <!-- scrollbar removed-->
                <div class="navbar-vertical-content">
                    <ul class="navbar-nav flex-column" id="navbarVerticalNav">
                        <li class="nav-item">
                            {{-- My Home — landing workspace (HomeController) --}}
                            <div class="nav-item-wrapper">
                                <a class="nav-link home-nav label-1" href="{{ route('home') }}" role="button" data-bs-toggle="" aria-expanded="false">
                                    <div class="d-flex align-items-center"><span class="nav-link-icon"><span data-feather="home"></span></span><span class="nav-link-text-wrapper"><span class="nav-link-text">My Home</span></span></div>
                                </a>
                            </div>
                        </li>
                        <li class="nav-item">
                            <!-- label-->
                            <p class="navbar-vertical-label">Modules</p>
                            <hr class="navbar-vertical-line" />
                            <!-- parent pages-->
                            <div class="nav-item-wrapper">
                                <a class="nav-link purchasing_of_land dropdown-indicator label-1"
                                    href="#nv-purchasing_of_land" role="button" data-bs-toggle="collapse"
                                    aria-expanded="false" aria-controls="nv-purchasing_of_land">
                                    <div class="d-flex align-items-center">
                                        <div class="dropdown-indicator-icon"><span class="fas fa-caret-right"></span></div>
                                        <span class="nav-link-icon">
                                            <span data-feather="folder"></span>
                                        </span>
                                        <span class="nav-link-text">Land Acquisition</span>
                                    </div>
                                </a>

                                <div class="parent-wrapper label-1">
                                    <ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse"
                                        id="nv-purchasing_of_land">
                                        <li class="collapsed-nav-item-title d-none">Land Acquisition</li>
                                        @if(auth()->user()->lp_master_data_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link lp_master-nav label-1" href="{{ route('land_provider.index') }}"
                                                role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="home"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Land provider Profile</span></span></div>
                                            </a>
                                        </li>
                                        @endif

                                        @if(auth()->user()->seller_profile_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link seller_profile-nav label-1"
                                                href="{{ route('seller_profile.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="user"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Land Owner Profile</span></span></div>
                                            </a>
                                        </li>
                                        @endif

                                        @if(auth()->user()->land_form_seller_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link land_form-nav label-1" href="{{ route('land_form.index') }}"
                                                role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="globe"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Land Offer Form (Land Details)</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->purchase_of_land_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link purchase_of_land-nav label-1"
                                                href="{{ route('purchase_of_land.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="align-justify"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Purchase of Land</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->exemption_inventory_list == 1 || auth()->user()->is_admin == 1)
                                        <li class="nav-item">
                                            <a class="nav-link exemption_inventory-nav label-1"
                                                href="{{ route('exemption_inventory.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="clipboard"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Exemption Inventory Approval</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->possession_certificate_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link possession_certificate-nav label-1"
                                                href="{{ route('possession_certificate.index') }}" role="button"
                                                data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="file-text"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Possession Certificate</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        <!-- @if(auth()->user()->pictorial_view_list == 1)
                                    <li class="nav-item">
                                        <a class="nav-link pictorial_view-nav label-1"
                                           href="{{ route('pictorial_view.index') }}" role="button" data-bs-toggle=""
                                           aria-expanded="false">
                                            <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="align-center"></span></span><span
                                                        class="nav-link-text-wrapper">
                                        <span class="nav-link-text">Pictorial View</span></span></div>
                                        </a>
                                    </li>
                                    @endif -->

                                    </ul>
                                </div>
                            </div>
                            <!-- parent pages-->
                            <div class="nav-item-wrapper">
                                <a class="nav-link registry dropdown-indicator label-1" href="#nv-registry_document"
                                    role="button" data-bs-toggle="collapse" aria-expanded="false"
                                    aria-controls="nv-registry_document">
                                    <div class="d-flex align-items-center">
                                        <div class="dropdown-indicator-icon"><span class="fas fa-caret-right"></span></div>
                                        <span class="nav-link-icon">
                                            <span data-feather="server"></span>
                                        </span>
                                        <span class="nav-link-text">Legal Documents</span>
                                    </div>
                                </a>

                                <div class="parent-wrapper label-1">
                                    <ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse"
                                        id="nv-registry_document">
                                        <li class="collapsed-nav-item-title d-none">Legal Documents</li>

                                        @if(auth()->user()->conveyance_deed_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link conveyance-nav  label-1"
                                                href="{{ route('conveyance.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="align-center"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text"> Conveyance Deed</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->agreement_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link agreement-nav  label-1"
                                                href="{{ route('agreement.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="align-justify"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Agreement</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->affidavit_2_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link affidavit_2-nav  label-1"
                                                href="{{ route('affidavit_2.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="book-open"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Affidavit</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->undertaking_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link undertaking-nav  label-1"
                                                href="{{ route('undertaking.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                             data-feather="file-text"></span></span><span
                                                         class="nav-link-text-wrapper">
                                                         <span class="nav-link-text">Undertaking</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->indemnity_bond_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link indemnity_bond-nav  label-1"
                                                href="{{ route('indemnity_bond.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="aperture"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Indemnity Bond</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->registry_document_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link registry_document-nav  label-1"
                                                href="{{ route('registry_document.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="arrow-down-circle"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Completion of Registry</span></span></div>
                                            </a>
                                        </li>
                                        @endif

                                    </ul>
                                </div>
                            </div>
                            <!-- parent pages-->
                            {{-- Exemption module: its only live form (Exemption Inventory Approval) now lives under Land Acquisition; the other items were already commented out. Restore by removing this Blade comment. --}}{{--<div class="nav-item-wrapper">
                                <a class="nav-link exemption dropdown-indicator label-1" href="#nv-exemption_document"
                                    role="button" data-bs-toggle="collapse" aria-expanded="false"
                                    aria-controls="nv-exemption_document">
                                    <div class="d-flex align-items-center">
                                        <div class="dropdown-indicator-icon"><span class="fas fa-caret-right"></span></div>
                                        <span class="nav-link-icon">
                                            <span data-feather="box"></span>
                                        </span>
                                        <span class="nav-link-text">Exemption</span>
                                    </div>
                                </a>

                                <div class="parent-wrapper label-1">
                                    <ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse"
                                        id="nv-exemption_document">
                                        <li class="collapsed-nav-item-title d-none">Exemption</li>
                                        <!-- @if(auth()->user()->exemption_form_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link exemption_form-nav  label-1"
                                                href="{{ route('exemption_form.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="cast"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text"> Exemption Form</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        
                                        @if(auth()->user()->exemption_rate_list == 1)

                                        <li class="nav-item">
                                            <a class="nav-link exemption_rate-nav label-1"
                                                href="{{ route('exemption_rate.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="dollar-sign"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Exemption Rates</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->challan_fee_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link challan_fee-nav  label-1"
                                                href="{{ route('challan_fee.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="dollar-sign"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Challan Fee</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->challan_form_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link challan_form-nav label-1"
                                                href="{{ route('challan_form.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="user"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Challan Form</span></span></div>
                                            </a>
                                        </li>
                                        @endif -->

                                    </ul>
                                </div>
                            </div>--}}
                            <!-- parent pages-->
                            <!-- <div class="nav-item-wrapper">
                                <a class="nav-link intimation dropdown-indicator label-1" href="#nv-intimation_documents"
                                    role="button" data-bs-toggle="collapse" aria-expanded="false"
                                    aria-controls="nv-intimation_documents">
                                    <div class="d-flex align-items-center">
                                        <div class="dropdown-indicator-icon"><span class="fas fa-caret-right"></span></div>
                                        <span class="nav-link-icon">
                                            <span data-feather="book"></span>
                                        </span>
                                        <span class="nav-link-text">Intimation Documents</span>
                                    </div>
                                </a>

                                <div class="parent-wrapper label-1">
                                    <ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse"
                                        id="nv-intimation_documents">
                                        <li class="collapsed-nav-item-title d-none">Intimation Documents</li>

                                        @if(auth()->user()->intimation_application_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link intimation_application-nav  label-1"
                                                href="{{ route('intimation_application.index') }}" role="button"
                                                data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="codesandbox"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Intimation Application</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                        @if(auth()->user()->intimation_letter_list == 1)
                                        <li class="nav-item">
                                            <a class="nav-link intimation_letter-nav  label-1"
                                                href="{{ route('intimation_letter.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="dribbble"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Intimation Letter</span></span></div>
                                            </a>
                                        </li>
                                        @endif
                                    </ul>
                                </div>
                            </div> -->
                            @if(auth()->user()->is_admin == 1)

                            {{--<div class="nav-item-wrapper">--}}
                            {{--<a class="nav-link intimation dropdown-indicator label-1" href="#nv-users_management"--}}
                            {{--role="button" data-bs-toggle="collapse" aria-expanded="false"--}}
                            {{--aria-controls="nv-users_management">--}}
                            {{--<div class="d-flex align-items-center">--}}
                            {{--<div class="dropdown-indicator-icon"><span class="fas fa-caret-right"></span></div>--}}
                            {{--<span class="nav-link-icon">--}}
                            {{--<span data-feather="settings"></span>--}}
                            {{--</span>--}}
                            {{--<span class="nav-link-text">Administration</span>--}}
                            {{--</div>--}}
                            {{--</a>--}}

                            {{--<div class="parent-wrapper label-1">--}}
                            {{--<ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse"--}}
                            {{--id="nv-users_management">--}}
                            {{--<li class="collapsed-nav-item-title d-none">Users Management</li>--}}
                            {{--<li class="nav-item">--}}
                            {{--<a class="nav-link users-nav  label-1"--}}
                            {{--href="{{ route('users.index') }}" role="button" data-bs-toggle=""--}}
                            {{--aria-expanded="false">--}}
                            {{--<div class="d-flex align-items-center"><span class="nav-link-icon"><span--}}
                            {{--data-feather="users"></span></span><span--}}
                            {{--class="nav-link-text-wrapper">--}}
                            {{--<span class="nav-link-text">Users Management</span></span></div>--}}
                            {{--</a>--}}
                            {{--</li>--}}

                            {{--</ul>--}}
                            {{--</div>--}}
                            {{--</div>--}}
                            <div class="nav-item-wrapper"><a class="nav-link dropdown-indicator label-1" href="#nv-multi-level" role="button" data-bs-toggle="collapse" aria-expanded="false" aria-controls="nv-multi-level">
                                    <div class="d-flex align-items-center">
                                        <div class="dropdown-indicator-icon">
                                            <span class="fas fa-caret-right"></span>
                                        </div>
                                        <span class="nav-link-icon">
                                            <span data-feather="settings"></span>

                                        </span>
                                        <span class="nav-link-text">Administration</span>
                                    </div>
                                </a>
                                <div class="parent-wrapper label-1">
                                    <ul class="nav collapse parent" data-bs-parent="#navbarVerticalCollapse" id="nv-multi-level">
                                        <li class="collapsed-nav-item-title d-none">Administration</li>

                                        <li class="nav-item"><a class="nav-link dropdown-indicator"
                                                href="#nv-level-three" data-bs-toggle="collapse"
                                                aria-expanded="false" aria-controls="nv-level-three">
                                                <div class="d-flex align-items-center">
                                                    <div class="dropdown-indicator-icon"><span
                                                            class="fas fa-caret-right"></span></div>
                                                    <span class="nav-link-text">Approvals</span>
                                                </div>
                                            </a><!-- more inner pages-->
                                            <div class="parent-wrapper">
                                                <ul class="nav collapse parent" data-bs-parent="#multi-level"
                                                    id="nv-level-three">
                                                    <li class="nav-item">
                                                        <a class="nav-link" href="{{ route('approval_tree.index') }}"
                                                            data-bs-toggle=""
                                                            aria-expanded="false">
                                                            <div class="d-flex align-items-center">
                                                                <span class="nav-link-text">Approval Tree</span>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li class="nav-item"><a class="nav-link" href="{{ route('approval_stage.index') }}"
                                                            data-bs-toggle="" aria-expanded="false">
                                                            <div class="d-flex align-items-center"><span
                                                                    class="nav-link-text">Approval Stages</span></div>
                                                        </a><!-- more inner pages-->
                                                    </li>
                                                    <li class="nav-item"><a class="nav-link" href="{{ route('approval_setup.index') }}"
                                                            data-bs-toggle="" aria-expanded="false">
                                                            <div class="d-flex align-items-center"><span
                                                                    class="nav-link-text">Approval Setup</span></div>
                                                        </a><!-- more inner pages-->
                                                    </li>

                                                </ul>
                                            </div>
                                        </li>

                                        <li class="nav-item">
                                            <a class="nav-link users-nav  label-1"
                                                href="{{ route('users.index') }}" role="button" data-bs-toggle=""
                                                aria-expanded="false">
                                                <div class="d-flex align-items-center"><span class="nav-link-icon"><span
                                                            data-feather="users"></span></span><span
                                                        class="nav-link-text-wrapper">
                                                        <span class="nav-link-text">Users Management</span></span></div>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            @endif
                            <!-- parent pages-->
                        </li>
                    </ul>

                </div>

            </div>

            <div class="navbar-vertical-footer">
                <button class="btn navbar-vertical-toggle border-0 fw-semi-bold w-100 white-space-nowrap d-flex align-items-center">
                    <span class="uil uil-left-arrow-to-left fs-0"></span><span class="uil uil-arrow-from-right fs-0"></span><span
                        class="navbar-vertical-footer-text ms-2">Collapsed View</span></button>
            </div>
        </nav>
        <nav class="navbar navbar-top fixed-top navbar-expand" id="navbarDefault" style="display:none;">
            <div class="collapse navbar-collapse justify-content-between">
                <div class="navbar-logo">
                    <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                        data-bs-toggle="collapse" data-bs-target="#navbarVerticalCollapse"
                        aria-controls="navbarVerticalCollapse" aria-expanded="false" aria-label="Toggle Navigation">
                        <span class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                    <div class="dropdown lm-appswitch"><a class="navbar-brand me-1 me-sm-3 lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>
                        <div class="d-flex align-items-center">
                            <div class="d-flex align-items-center"><span class="lm-brand-mark" aria-hidden="true"><img src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt=""></span><span class="lm-brand-word"><span>Land Information</span><span>Management System</span></span>

                                {{--<p class="logo-text ms-2 d-none d-sm-block">phoenix</p>--}}
                            </div>
                        </div>
                    @if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>


                </div>
                <div class="lm-cmd d-none d-md-flex" id="lmCmd" role="combobox" aria-haspopup="listbox" aria-expanded="false" aria-owns="lmCmdResults">
                    <span class="lm-cmd-icon" data-feather="search"></span>
                    <input type="text" class="lm-cmd-input" id="lmCmdInput" placeholder="Search or type a command…" autocomplete="off" aria-label="Search modules and forms" aria-controls="lmCmdResults">
                    <kbd class="lm-cmd-kbd">Ctrl K</kbd>
                    <div class="lm-cmd-results" id="lmCmdResults" role="listbox"></div>
                </div>
                <ul class="navbar-nav navbar-nav-icons flex-row no-print">
                    {{-- Recent / Favourite forms (browser-side, shared with My Home; popovers built by lmis-theme.js) --}}
                    <li class="nav-item lm-hdr-opt">
                        <button type="button" class="nav-link lm-quick-btn" id="lmRecentBtn" title="Recent forms" aria-label="Recent forms" aria-haspopup="true" aria-expanded="false">
                            <span data-feather="clock" style="height:20px;width:20px;"></span>
                        </button>
                    </li>
                    <li class="nav-item lm-hdr-opt">
                        <button type="button" class="nav-link lm-quick-btn" id="lmFavBtn" title="Favourite forms" aria-label="Favourite forms" aria-haspopup="true" aria-expanded="false">
                            <span data-feather="star" style="height:20px;width:20px;"></span>
                        </button>
                    </li>
                    <li class="nav-item lm-hdr-opt dropdown">
                        <a class="nav-link" href="{{ route('approved_documents', Auth::user()->id) }}">
                            <span data-feather="arrow-down-circle" style="height:20px;width:20px;">
                            </span>
                        </a>
                        @if (session()->has('total_count_approved'))
                        <?php $total_count_approved = session()->get('total_count_approved'); ?>
                        @if($total_count_approved > 0)
                        <span class="count-indicator1">{{ $total_count_approved }}</span>
                        @endif
                        @endif
                    </li>
                    <li class="nav-item lm-hdr-opt dropdown">
                        <a class="nav-link" href="{{ route('rejected_documents', Auth::user()->id) }}">
                            <span data-feather="alert-circle" style="height:20px;width:20px;">
                            </span>
                        </a>
                        @if (session()->has('total_count_rejected'))
                        <?php $total_count_rejected = session()->get('total_count_rejected'); ?>
                        @if($total_count_rejected > 0)
                        <span class="count-indicator1">{{ $total_count_rejected }}</span>
                        @endif
                        @endif
                    </li>
                    <li class="nav-item lm-hdr-opt dropdown">
                        <a class="nav-link" href="{{ route('pending_documents', Auth::user()->id) }}">
                            <span data-feather="bell" style="height:20px;width:20px;">
                            </span>
                        </a>
                        @if (session()->has('total_count_pending'))
                        <?php $data1 = session()->get('total_count_pending'); ?>
                        @if($data1 > 0)

                        <span class="count-indicator1">{{ $data1 }}</span>
                        @endif
                        @endif
                    </li>
                    <li class="nav-item lm-hdr-opt dropdown">

                        <a class="nav-link" href="{{ route('approval_inbox', Auth::user()->id) }}">
                            <span data-feather="book" style="height:20px;width:20px;">
                            </span>
                        </a>
                        @if (session()->has('total_count_approval'))
                        <?php $data = session()->get('total_count_approval'); ?>
                        @if($data > 0)

                        <span class="count-indicator1">{{ $data }}</span>
                        @endif
                        @endif
                    </li>

                    {{-- Search (phones): the pill is folded into this icon; it drops the pill under the bar --}}
                    <li class="nav-item lm-hdr-search">
                        <button type="button" class="nav-link lm-quick-btn" id="lmSearchBtn" title="Search" aria-label="Search forms" aria-expanded="false" aria-controls="lmCmd">
                            <span data-feather="search" style="height:20px;width:20px;"></span>
                        </button>
                    </li>
                    {{-- ⋮ More (phones): the folded header controls, same targets --}}
                    <li class="nav-item dropdown lm-hdr-more">
                        <a class="nav-link" href="#" role="button" id="lmHdrMore" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" title="More" aria-label="More options">
                            <span data-feather="more-vertical" style="height:20px;width:20px;"></span>
                        </a>
                        <div class="dropdown-menu dropdown-menu-end" aria-labelledby="lmHdrMore">
                            <a class="dropdown-item" href="#" data-act="recent"><span data-feather="clock"></span>Recent forms</a>
                            <a class="dropdown-item" href="#" data-act="favs"><span data-feather="star"></span>Favourite forms</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="{{ route('approval_inbox', Auth::user()->id) }}"><span data-feather="book"></span>Approval inbox @if(session('total_count_approval', 0) > 0)<span class="count-indicator1">{{ session('total_count_approval') }}</span>@endif</a>
                            <a class="dropdown-item" href="{{ route('pending_documents', Auth::user()->id) }}"><span data-feather="bell"></span>Pending documents @if(session('total_count_pending', 0) > 0)<span class="count-indicator1">{{ session('total_count_pending') }}</span>@endif</a>
                            <a class="dropdown-item" href="{{ route('approved_documents', Auth::user()->id) }}"><span data-feather="arrow-down-circle"></span>Approved documents</a>
                            <a class="dropdown-item" href="{{ route('rejected_documents', Auth::user()->id) }}"><span data-feather="alert-circle"></span>Rejected documents</a>
                        </div>
                    </li>
                    <li class="nav-item">
                        <div class="theme-control-toggle fa-icon-wait px-2"><input
                                class="form-check-input ms-0 theme-control-toggle-input" type="checkbox"
                                data-theme-control="phoenixTheme" value="dark" id="themeControlToggle" /><label
                                class="mb-0 theme-control-toggle-label theme-control-toggle-light"
                                for="themeControlToggle"
                                title="Switch theme"><span class="icon" data-feather="moon"></span></label><label
                                class="mb-0 theme-control-toggle-label theme-control-toggle-dark"
                                for="themeControlToggle"
                                title="Switch theme"><span class="icon" data-feather="sun"></span></label></div>
                    </li>
                    <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0" id="navbarDropdownUser" href="#!"
                            role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false">
                            <div class="avatar avatar-l ">
                                <span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span>
                            </div>
                        </a>

                        <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                            aria-labelledby="navbarDropdownUser">
                            <div class="card position-relative border-0">
                                <div class="card-body p-0">
                                    <div class="text-center pt-4 pb-3">
                                        <div class="avatar avatar-xl ">
                                            <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                        </div>
                                        <h6 class="mt-2 text-black">{{Auth::user()->name}}</h6>
                                    </div>

                                </div>
                                <div class="overflow-auto scrollbar" style="height: 10rem;display: none">
                                    <ul class="nav d-flex flex-column mb-2 pb-1">
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="user"></span><span>Profile</span></a>
                                        </li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                    data-feather="pie-chart"></span>Dashboard</a>
                                        </li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="lock"></span>Posts
                                                &amp; Activity</a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="settings"></span>Settings
                                                &amp; Privacy </a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="help-circle"></span>Help
                                                Center</a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="globe"></span>Language</a>
                                        </li>
                                    </ul>
                                </div>
                                <div class="card-footer p-0 border-top">
                                    {{--<ul class="nav d-flex flex-column my-3">--}}
                                    {{--<li style="display: none;"  class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"--}}
                                    {{--data-feather="user-plus"></span>Add--}}
                                    {{--another account</a></li>--}}
                                    {{--</ul>--}}
                                    <hr />
                                    <div class="px-3">
                                        @if (Route::has('login'))
                                        <a class="btn btn-phoenix-secondary d-flex flex-center w-100"
                                            href="{{ route('logout') }}">
                                            <span class="me-2" data-feather="log-out"> </span>Sign out</a>
                                        @endif
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
        <nav class="navbar navbar-top navbar-slim fixed-top navbar-expand" id="topNavSlim" style="display:none;">
            <div class="collapse navbar-collapse justify-content-between">
                <div class="navbar-logo">
                    <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                        data-bs-toggle="collapse" data-bs-target="#navbarVerticalCollapse"
                        aria-controls="navbarVerticalCollapse" aria-expanded="false" aria-label="Toggle Navigation">
                        <span class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                    <div class="dropdown lm-appswitch"><a class="navbar-brand navbar-brand lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>{{ config('app.name', 'LMIS') }} <span
                            class="text-1000 d-none d-sm-inline">slim</span>@if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>
                </div>
                <ul class="navbar-nav navbar-nav-icons flex-row">
                    <li class="nav-item">
                        <div class="theme-control-toggle fa-ion-wait pe-2 theme-control-toggle-slim"><input
                                class="form-check-input ms-0 theme-control-toggle-input" id="themeControlToggle"
                                type="checkbox" data-theme-control="phoenixTheme" value="dark" /><label
                                class="mb-0 theme-control-toggle-label theme-control-toggle-light"
                                for="themeControlToggle"
                                title="Switch theme"><span class="icon me-1 d-none d-sm-block"
                                    data-feather="moon"></span><span
                                    class="fs--1 fw-bold">Dark</span></label><label
                                class="mb-0 theme-control-toggle-label theme-control-toggle-dark"
                                for="themeControlToggle"
                                title="Switch theme"><span class="icon me-1 d-none d-sm-block"
                                    data-feather="sun"></span><span
                                    class="fs--1 fw-bold">Light</span></label></div>
                    </li>
                    <li class="nav-item"><a class="nav-link" href="#" data-bs-toggle="modal"
                            data-bs-target="#searchBoxModal"><span data-feather="search"
                                style="height:12px;width:12px;"></span></a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link" id="navbarDropdownNotification" href="#" role="button" data-bs-toggle="dropdown"
                            data-bs-auto-close="outside" aria-haspopup="true" aria-expanded="false"><span data-feather="bell"
                                style="height:12px;width:12px;"></span></a>

                        <div class="dropdown-menu dropdown-menu-end notification-dropdown-menu py-0 shadow border border-300 navbar-dropdown-caret"
                            id="navbarDropdownNotfication" aria-labelledby="navbarDropdownNotfication">
                            <div class="card position-relative border-0">
                                <div class="card-header p-2">
                                    <div class="d-flex justify-content-between">
                                        <h5 class="text-black mb-0">Notificatons</h5>
                                        <button class="btn btn-link p-0 fs--1 fw-normal" type="button">Mark all as read
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body p-0">
                                    <div class="scrollbar-overlay" style="height: 27rem;">
                                        <div class="border-300">
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read border-bottom">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3"><img
                                                                class="rounded-circle"
                                                                src="{{ asset('public/assets/img/team/40x40/30.webp'); }}"
                                                                alt="" /></div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Mentioned you in a
                                                                comment.<span class="ms-2 text-400 fw-bold fs--2">10m</span>
                                                            </p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">10:41 AM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3">
                                                            <div class="avatar-name rounded-circle"><span>J</span></div>
                                                        </div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Jane Foster</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Created an event.<span
                                                                    class="ms-2 text-400 fw-bold fs--2">20m</span></p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">10:20 AM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3"><img
                                                                class="rounded-circle avatar-placeholder"
                                                                src="{{ asset('public/assets/img/team/40x40/avatar.webp'); }}"
                                                                alt="" /></div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                    class="ms-2 text-400 fw-bold fs--2">1h</span></p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">9:30 AM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="border-300">
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3"><span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span></div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Kiera Anderson</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Mentioned you in a
                                                                comment.<span class="ms-2 text-400 fw-bold fs--2"></span>
                                                            </p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">9:11 AM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3"><img
                                                                class="rounded-circle"
                                                                src="{{ asset('public/assets/img/team/40x40/59.webp'); }}"
                                                                alt="" /></div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Herman Carter</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Tagged you in a
                                                                comment.<span
                                                                    class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">10:58 PM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read ">
                                                <div class="d-flex align-items-center justify-content-between position-relative">
                                                    <div class="d-flex">
                                                        <div class="avatar avatar-m status-online me-3"><img
                                                                class="rounded-circle"
                                                                src="{{ asset('public/assets/img/team/40x40/58.webp'); }}"
                                                                alt="" /></div>
                                                        <div class="flex-1 me-sm-3">
                                                            <h4 class="fs--1 text-black">Benjamin Button</h4>

                                                            <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                    class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                    class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                            <p class="text-800 fs--1 mb-0"><span
                                                                    class="me-1 fas fa-clock"></span><span
                                                                    class="fw-bold">10:18 AM </span>August 7,2021</p>
                                                        </div>
                                                    </div>
                                                    <div class="font-sans-serif d-none d-sm-block">
                                                        <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                            type="button" data-bs-toggle="dropdown"
                                                            data-boundary="window" aria-haspopup="true"
                                                            aria-expanded="false" data-bs-reference="parent"><span
                                                                class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                        </button>
                                                        <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                class="dropdown-item" href="#!">Mark as unread</a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-footer p-0 border-top border-0">
                                    <div class="my-2 text-center fw-bold fs--2 text-600"><a class="fw-bolder"
                                            href="pages/notifications.html">Notification
                                            history</a></div>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0 white-space-nowrap" id="navbarDropdownUser"
                            href="#!" role="button" data-bs-toggle="dropdown" aria-haspopup="true"
                            data-bs-auto-close="outside" aria-expanded="false">Olivia <span
                                class="fa-solid fa-chevron-down fs--2"></span></a>

                        <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                            aria-labelledby="navbarDropdownUser">
                            <div class="card position-relative border-0">
                                <div class="card-body p-0">
                                    <div class="text-center pt-4 pb-3">
                                        <div class="avatar avatar-xl ">
                                            <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                        </div>
                                        <h6 class="mt-2 text-black">Jerry Seinfield</h6>
                                    </div>
                                    <div class="mb-3 mx-3"><input class="form-control form-control-sm"
                                            id="statusUpdateInput" type="text"
                                            placeholder="Update your status" /></div>
                                </div>
                                <div class="overflow-auto scrollbar" style="height: 10rem;">
                                    <ul class="nav d-flex flex-column mb-2 pb-1">
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="user"></span><span>Profile</span></a>
                                        </li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                    data-feather="pie-chart"></span>Dashboard</a>
                                        </li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="lock"></span>Posts
                                                &amp; Activity</a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="settings"></span>Settings
                                                &amp; Privacy </a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="help-circle"></span>Help
                                                Center</a></li>
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="globe"></span>Language</a>
                                        </li>
                                    </ul>
                                </div>
                                <div class="card-footer p-0 border-top">
                                    <ul class="nav d-flex flex-column my-3">
                                        <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                    data-feather="user-plus"></span>Add
                                                another account</a></li>
                                    </ul>
                                    <hr />
                                    <div class="px-3"><a class="btn btn-phoenix-secondary d-flex flex-center w-100"
                                            href="#!"> <span class="me-2" data-feather="log-out"> </span>Sign
                                            out</a></div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
        <nav class="navbar navbar-top fixed-top navbar-expand-lg" id="navbarTop" style="display:none;">
            <div class="navbar-logo">
                <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                    data-bs-toggle="collapse" data-bs-target="#navbarTopCollapse" aria-controls="navbarTopCollapse"
                    aria-expanded="false" aria-label="Toggle Navigation"><span class="navbar-toggle-icon"><span
                            class="toggle-line"></span></span></button>
                <div class="dropdown lm-appswitch"><a class="navbar-brand me-1 me-sm-3 lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>
                    <div class="d-flex align-items-center">
                        <div class="d-flex align-items-center"><span class="lm-brand-mark" aria-hidden="true"><img src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt=""></span><span class="lm-brand-word"><span>Land Information</span><span>Management System</span></span>

                        </div>
                    </div>
                @if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>
            </div>
            <div class="collapse navbar-collapse navbar-top-collapse order-1 order-lg-0 justify-content-center"
                id="navbarTopCollapse">

                <ul class="navbar-nav navbar-nav-top" data-dropdown-on-hover="data-dropdown-on-hover">
                    {{--<li class="collapsed-nav-item-title d-none">Land Acquisition</li>--}}
                    <li class="nav-item dropdown">
                        <a class="nav-link purchasing_of_land dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-chart-pie"></span>Land Acquisition</a>

                        <ul class="dropdown-menu navbar-dropdown-caret">

                            @if(auth()->user()->lp_master_data_list == 1)

                            <li>
                                <a class="dropdown-item lp_master-nav " href="{{ route('land_provider.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" class="fs-0 me-2" data-feather="home"></span>LP
                                        Master Data
                                    </div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->exemption_rate_list == 1)
                            <li>
                                <a class="dropdown-item exemption_rate-nav" href="{{ route('exemption_rate.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            class="fs-0 me-2" data-feather="dollar-sign"></span>Exemption
                                        Rates
                                    </div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->challan_fee_list == 1)
                            <li>
                                <a class="dropdown-item challan_fee-nav  label-1" href="{{ route('challan_fee.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="dollar-sign"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Challan Fee</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->seller_profile_list == 1)
                            <li>
                                <a class="dropdown-item seller_profile-nav label-1" href="{{ route('seller_profile.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="user"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Seller Profile</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->challan_form_list == 1)
                            <li>
                                <a class="dropdown-item challan_form-nav label-1" href="{{ route('challan_form.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="user"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Challan Form</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->land_form_seller_list == 1)
                            <li>
                                <a class="dropdown-item land_form-nav label-1" href="{{ route('land_form.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="globe"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Land Form (Seller)</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->purchase_of_land_list == 1)
                            <li>
                                <a class="dropdown-item purchase_of_land-nav label-1"
                                    href="{{ route('purchase_of_land.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="align-justify"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Purchase of Land</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->possession_certificate_list == 1)
                            <li>
                                <a class="dropdown-item possession_certificate-nav label-1"
                                    href="{{ route('possession_certificate.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="file-text"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Possession Certificate</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->pictorial_view_list == 1)
                            <li>
                                <a class="dropdown-item pictorial_view-nav label-1" href="{{ route('pictorial_view.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="align-center"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Pictorial View</span></span></div>
                                </a>
                            </li>
                            @endif

                        </ul>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link  dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-document-layout-right"></span>Legal Documents</a>

                        <ul class="dropdown-menu navbar-dropdown-caret">

                            @if(auth()->user()->conveyance_deed_list == 1)

                            <li>
                                <a class="dropdown-item conveyance-nav  label-1"
                                    href="{{ route('conveyance.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="align-center"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text"> Conveyance Deed</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->agreement_list == 1)
                            <li>
                                <a class="dropdown-item agreement-nav  label-1"
                                    href="{{ route('agreement.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="align-justify"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Agreement</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->indemnity_bond_list == 1)
                            <li>
                                <a class="dropdown-item indemnity_bond-nav  label-1"
                                    href="{{ route('indemnity_bond.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="aperture"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Indemnity Bond</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->registry_document_list == 1)
                            <li>
                                <a class="dropdown-item registry_document-nav  label-1"
                                    href="{{ route('registry_document.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="arrow-down-circle"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Registry Document </span></span></div>
                                </a>
                            </li>
                            @endif


                        </ul>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link  dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-puzzle-piece"></span>Exemption</a>

                        <ul class="dropdown-menu navbar-dropdown-caret">



                            @if(auth()->user()->exemption_form_list == 1)
                            <li>
                                <a class="dropdown-item exemption_form-nav  label-1"
                                    href="{{ route('exemption_form.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="cast"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text"> Exemption Form</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->affidavit_2_list == 1)
                            <li>
                                <a class="dropdown-item affidavit_2-nav  label-1"
                                    href="{{ route('affidavit_2.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="book-open"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Affidavit 2</span></span></div>
                                </a>
                            </li>
                            @endif


                        </ul>
                    </li>



                    <li class="nav-item dropdown">
                        <a class="nav-link  dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false">
                            <span
                                class=" fs-0 me-2 uil-apps"></span>Intimation Documents</a>

                        <ul class="dropdown-menu navbar-dropdown-caret">




                            @if(auth()->user()->intimation_application_list == 1)
                            <li>
                                <a class="dropdown-item intimation_application-nav  label-1"
                                    href="{{ route('intimation_application.index') }}" role="button"
                                    data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="codesandbox"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Intimation Application</span></span></div>
                                </a>
                            </li>
                            @endif
                            @if(auth()->user()->intimation_letter_list == 1)
                            <li>
                                <a class="dropdown-item intimation_letter-nav  label-1"
                                    href="{{ route('intimation_letter.index') }}" role="button" data-bs-toggle=""
                                    aria-expanded="false">
                                    <div class="dropdown-item-wrapper"><span class="nav-link-icon"><span
                                                class="fs-0 me-2" data-feather="dribbble"></span></span><span
                                            class="nav-link-text-wrapper">
                                            <span class="nav-link-text">Intimation Letter</span></span></div>
                                </a>
                            </li>

                            @endif

                        </ul>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link  dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false">
                            <span class="me-2 uil" data-feather="settings"></span>Administration</a>

                        {{--<a class="dropdown-item dropdown-toggle" id="customization" href="#" data-bs-toggle="dropdown" data-bs-auto-close="outside">--}}
                        {{--<div class="dropdown-item-wrapper"><span class="uil fs-0 uil-angle-right lh-1 dropdown-indicator-icon"></span><span><span class="me-2 uil" data-feather="settings"></span>Administration</span></div>--}}
                        {{--</a>--}}
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="{{ route('users.index') }}">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"></span>User Managment</div>
                                </a></li>
                        </ul>
                    </li>

                </ul>


            </div>
            <ul class="navbar-nav navbar-nav-icons flex-row">
                <li class="nav-item">
                    <div class="theme-control-toggle fa-icon-wait px-2"><input
                            class="form-check-input ms-0 theme-control-toggle-input" type="checkbox"
                            data-theme-control="phoenixTheme" value="dark" id="themeControlToggle" /><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-light" for="themeControlToggle"
                            title="Switch theme"><span class="icon"
                                data-feather="moon"></span></label><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-dark" for="themeControlToggle"
                            title="Switch theme"><span class="icon"
                                data-feather="sun"></span></label>
                    </div>
                </li>

                <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0" id="navbarDropdownUser" href="#!" role="button"
                        data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-haspopup="true"
                        aria-expanded="false">
                        <div class="avatar avatar-l ">
                            <span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span>
                        </div>
                    </a>
                    <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                        aria-labelledby="navbarDropdownUser">
                        <div class="card position-relative border-0">
                            <div class="card-body p-0">
                                <div class="text-center pt-4 pb-3">
                                    <div class="avatar avatar-xl ">
                                        <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                    </div>
                                    <h6 class="mt-2 text-black">{{Auth::user()->name}}</h6>
                                </div>

                            </div>
                            <div class="overflow-auto scrollbar" style="height: 10rem;display: none">
                                <ul class="nav d-flex flex-column mb-2 pb-1">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user"></span><span>Profile</span></a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                data-feather="pie-chart"></span>Dashboard</a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="lock"></span>Posts
                                            &amp; Activity</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="settings"></span>Settings
                                            &amp; Privacy </a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="help-circle"></span>Help
                                            Center</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="globe"></span>Language</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-footer p-0 border-top">
                                {{--<ul class="nav d-flex flex-column my-3">--}}
                                {{--<li style="display: none;"  class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"--}}
                                {{--data-feather="user-plus"></span>Add--}}
                                {{--another account</a></li>--}}
                                {{--</ul>--}}
                                <hr />
                                <div class="px-3">
                                    @if (Route::has('login'))
                                    <a class="btn btn-phoenix-secondary d-flex flex-center w-100"
                                        href="{{ route('logout') }}">
                                        <span class="me-2" data-feather="log-out"> </span>Sign out</a>
                                    @endif
                                </div>
                            </div>
                        </div>
                    </div>

                </li>
            </ul>
        </nav>
        <nav class="navbar navbar-top navbar-slim justify-content-between fixed-top navbar-expand-lg" id="navbarTopSlim"
            style="display:none;">
            <div class="navbar-logo">
                <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                    data-bs-toggle="collapse" data-bs-target="#navbarTopCollapse" aria-controls="navbarTopCollapse"
                    aria-expanded="false" aria-label="Toggle Navigation"><span class="navbar-toggle-icon"><span
                            class="toggle-line"></span></span></button>
                <div class="dropdown lm-appswitch"><a class="navbar-brand navbar-brand lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>phoenix <span
                        class="text-1000 d-none d-sm-inline">slim</span>@if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>
            </div>
            <div class="collapse navbar-collapse navbar-top-collapse order-1 order-lg-0 justify-content-center"
                id="navbarTopCollapse">
                <ul class="navbar-nav navbar-nav-top" data-dropdown-on-hover="data-dropdown-on-hover">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-chart-pie"></span>Home</a>
                        <ul class="dropdown-menu navbar-dropdown-caret">
                            <li>
                                <a class="dropdown-item " href="index.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="shopping-cart"></span>E commerce
                                    </div>
                                </a>
                            </li>
                            <li><a class="dropdown-item" href="dashboard/project-management.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="clipboard"></span>Project
                                        management
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="dashboard/crm.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="phone"></span>CRM
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="apps/social/feed.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="share-2"></span>Social
                                        feed
                                    </div>
                                </a></li>
                        </ul>
                    </li>

                </ul>
            </div>
            <ul class="navbar-nav navbar-nav-icons flex-row">
                <li class="nav-item">
                    <div class="theme-control-toggle fa-ion-wait pe-2 theme-control-toggle-slim"><input
                            class="form-check-input ms-0 theme-control-toggle-input" id="themeControlToggle"
                            type="checkbox" data-theme-control="phoenixTheme" value="dark" /><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-light" for="themeControlToggle"
                            title="Switch theme"><span
                                class="icon me-1 d-none d-sm-block" data-feather="moon"></span><span
                                class="fs--1 fw-bold">Dark</span></label><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-dark" for="themeControlToggle"
                            title="Switch theme"><span
                                class="icon me-1 d-none d-sm-block" data-feather="sun"></span><span
                                class="fs--1 fw-bold">Light</span></label></div>
                </li>
                <li class="nav-item"><a class="nav-link" href="#" data-bs-toggle="modal"
                        data-bs-target="#searchBoxModal"><span data-feather="search"
                            style="height:12px;width:12px;"></span></a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link" id="navbarDropdownNotification" href="#" role="button" data-bs-toggle="dropdown"
                        data-bs-auto-close="outside" aria-haspopup="true" aria-expanded="false"><span data-feather="bell"
                            style="height:12px;width:12px;"></span></a>

                    <div class="dropdown-menu dropdown-menu-end notification-dropdown-menu py-0 shadow border border-300 navbar-dropdown-caret"
                        id="navbarDropdownNotfication" aria-labelledby="navbarDropdownNotfication">
                        <div class="card position-relative border-0">
                            <div class="card-header p-2">
                                <div class="d-flex justify-content-between">
                                    <h5 class="text-black mb-0">Notificatons</h5>
                                    <button class="btn btn-link p-0 fs--1 fw-normal" type="button">Mark all as read</button>
                                </div>
                            </div>
                            <div class="card-body p-0">
                                <div class="scrollbar-overlay" style="height: 27rem;">
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/30.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2">10m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:41 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3">
                                                        <div class="avatar-name rounded-circle"><span>J</span></div>
                                                    </div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jane Foster</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Created an event.<span
                                                                class="ms-2 text-400 fw-bold fs--2">20m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:20 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle avatar-placeholder"
                                                            src="{{ asset('public/assets/img/team/40x40/avatar.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2">1h</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:30 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Kiera Anderson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:11 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/59.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Herman Carter</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Tagged you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:58 PM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read ">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/58.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Benjamin Button</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:18 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer p-0 border-top border-0">
                                <div class="my-2 text-center fw-bold fs--2 text-600"><a class="fw-bolder"
                                        href="pages/notifications.html">Notification
                                        history</a></div>
                            </div>
                        </div>
                    </div>
                </li>
                <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0 white-space-nowrap" id="navbarDropdownUser"
                        href="#!" role="button" data-bs-toggle="dropdown" aria-haspopup="true"
                        data-bs-auto-close="outside" aria-expanded="false">Olivia <span
                            class="fa-solid fa-chevron-down fs--2"></span></a>

                    <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                        aria-labelledby="navbarDropdownUser">
                        <div class="card position-relative border-0">
                            <div class="card-body p-0">
                                <div class="text-center pt-4 pb-3">
                                    <div class="avatar avatar-xl ">
                                        <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                    </div>
                                    <h6 class="mt-2 text-black">Jerry Seinfield</h6>
                                </div>
                                <div class="mb-3 mx-3"><input class="form-control form-control-sm" id="statusUpdateInput"
                                        type="text" placeholder="Update your status" /></div>
                            </div>
                            <div class="overflow-auto scrollbar" style="height: 10rem;">
                                <ul class="nav d-flex flex-column mb-2 pb-1">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user"></span><span>Profile</span></a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                data-feather="pie-chart"></span>Dashboard</a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="lock"></span>Posts
                                            &amp; Activity</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="settings"></span>Settings
                                            &amp; Privacy </a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="help-circle"></span>Help
                                            Center</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="globe"></span>Language</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-footer p-0 border-top">
                                <ul class="nav d-flex flex-column my-3">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user-plus"></span>Add
                                            another account</a></li>
                                </ul>
                                <hr />
                                <div class="px-3"><a class="btn btn-phoenix-secondary d-flex flex-center w-100" href="#!">
                                        <span class="me-2" data-feather="log-out"> </span>Sign out</a></div>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </nav>
        <nav class="navbar navbar-top fixed-top navbar-expand-lg" id="navbarCombo" data-navbar-top="combo"
            data-move-target="#navbarVerticalNav" style="display:none;">
            <div class="navbar-logo">
                <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                    data-bs-toggle="collapse" data-bs-target="#navbarVerticalCollapse"
                    aria-controls="navbarVerticalCollapse" aria-expanded="false" aria-label="Toggle Navigation"><span
                        class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                <div class="dropdown lm-appswitch"><a class="navbar-brand me-1 me-sm-3 lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>
                    <div class="d-flex align-items-center">
                        <div class="d-flex align-items-center"><span class="lm-brand-mark" aria-hidden="true"><img src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt=""></span><span class="lm-brand-word"><span>Land Information</span><span>Management System</span></span>

                        </div>
                    </div>
                @if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>
            </div>
            <div class="collapse navbar-collapse navbar-top-collapse order-1 order-lg-0 justify-content-center"
                id="navbarTopCollapse">
                <ul class="navbar-nav navbar-nav-top" data-dropdown-on-hover="data-dropdown-on-hover">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-chart-pie"></span>Home</a>
                        <ul class="dropdown-menu navbar-dropdown-caret">
                            <li><a class="dropdown-item " href="index.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="shopping-cart"></span>E commerce
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="dashboard/project-management.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="clipboard"></span>Project
                                        management
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="dashboard/crm.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="phone"></span>CRM
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="apps/social/feed.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="share-2"></span>Social
                                        feed
                                    </div>
                                </a></li>
                        </ul>
                    </li>

                </ul>
            </div>
            <ul class="navbar-nav navbar-nav-icons flex-row">
                <li class="nav-item">
                    <div class="theme-control-toggle fa-icon-wait px-2"><input
                            class="form-check-input ms-0 theme-control-toggle-input" type="checkbox"
                            data-theme-control="phoenixTheme" value="dark" id="themeControlToggle" /><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-light" for="themeControlToggle"
                            title="Switch theme"><span class="icon"
                                data-feather="moon"></span></label><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-dark" for="themeControlToggle"
                            title="Switch theme"><span class="icon"
                                data-feather="sun"></span></label>
                    </div>
                </li>
                <li class="nav-item"><a class="nav-link" href="#" data-bs-toggle="modal"
                        data-bs-target="#searchBoxModal"><span data-feather="search"
                            style="height:19px;width:19px;margin-bottom: 2px;"></span></a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link" href="#" style="min-width: 2.5rem" role="button" data-bs-toggle="dropdown"
                        aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside"><span data-feather="bell"
                            style="height:20px;width:20px;"></span></a>

                    <div class="dropdown-menu dropdown-menu-end notification-dropdown-menu py-0 shadow border border-300 navbar-dropdown-caret"
                        id="navbarDropdownNotfication" aria-labelledby="navbarDropdownNotfication">
                        <div class="card position-relative border-0">
                            <div class="card-header p-2">
                                <div class="d-flex justify-content-between">
                                    <h5 class="text-black mb-0">Notificatons</h5>
                                    <button class="btn btn-link p-0 fs--1 fw-normal" type="button">Mark all as read</button>
                                </div>
                            </div>
                            <div class="card-body p-0">
                                <div class="scrollbar-overlay" style="height: 27rem;">
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/30.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2">10m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:41 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3">
                                                        <div class="avatar-name rounded-circle"><span>J</span></div>
                                                    </div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jane Foster</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Created an event.<span
                                                                class="ms-2 text-400 fw-bold fs--2">20m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:20 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle avatar-placeholder"
                                                            src="{{ asset('public/assets/img/team/40x40/avatar.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2">1h</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:30 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Kiera Anderson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:11 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/59.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Herman Carter</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Tagged you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:58 PM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read ">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/58.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Benjamin Button</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:18 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer p-0 border-top border-0">
                                <div class="my-2 text-center fw-bold fs--2 text-600"><a class="fw-bolder"
                                        href="pages/notifications.html">Notification
                                        history</a></div>
                            </div>
                        </div>
                    </div>
                </li>
                <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0" id="navbarDropdownUser" href="#!" role="button"
                        data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-haspopup="true"
                        aria-expanded="false">
                        <div class="avatar avatar-l ">
                            <span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span>
                        </div>
                    </a>

                    <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                        aria-labelledby="navbarDropdownUser">
                        <div class="card position-relative border-0">
                            <div class="card-body p-0">
                                <div class="text-center pt-4 pb-3">
                                    <div class="avatar avatar-xl ">
                                        <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                    </div>
                                    <h6 class="mt-2 text-black">Jerry Seinfield</h6>
                                </div>
                                <div class="mb-3 mx-3"><input class="form-control form-control-sm" id="statusUpdateInput"
                                        type="text" placeholder="Update your status" /></div>
                            </div>
                            <div class="overflow-auto scrollbar" style="height: 10rem;">
                                <ul class="nav d-flex flex-column mb-2 pb-1">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user"></span><span>Profile</span></a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                data-feather="pie-chart"></span>Dashboard</a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="lock"></span>Posts
                                            &amp; Activity</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="settings"></span>Settings
                                            &amp; Privacy </a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="help-circle"></span>Help
                                            Center</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="globe"></span>Language</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-footer p-0 border-top">
                                <ul class="nav d-flex flex-column my-3">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user-plus"></span>Add
                                            another account</a></li>
                                </ul>
                                <hr />
                                <div class="px-3"><a class="btn btn-phoenix-secondary d-flex flex-center w-100" href="#!">
                                        <span class="me-2" data-feather="log-out"> </span>Sign out</a></div>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </nav>
        <nav class="navbar navbar-top fixed-top navbar-slim justify-content-between navbar-expand-lg" id="navbarComboSlim"
            data-navbar-top="combo" data-move-target="#navbarVerticalNav" style="display:none;">
            <div class="navbar-logo">
                <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                    data-bs-toggle="collapse" data-bs-target="#navbarVerticalCollapse"
                    aria-controls="navbarVerticalCollapse" aria-expanded="false" aria-label="Toggle Navigation"><span
                        class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                <a class="navbar-brand navbar-brand" href="index.html">phoenix <span class="text-1000 d-none d-sm-inline">slim</span></a>
            </div>
            <div class="collapse navbar-collapse navbar-top-collapse order-1 order-lg-0 justify-content-center"
                id="navbarTopCollapse">
                <ul class="navbar-nav navbar-nav-top" data-dropdown-on-hover="data-dropdown-on-hover">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle lh-1" href="#!" role="button"
                            data-bs-toggle="dropdown" data-bs-auto-close="outside"
                            aria-haspopup="true" aria-expanded="false"><span
                                class="uil fs-0 me-2 uil-chart-pie"></span>Home</a>
                        <ul class="dropdown-menu navbar-dropdown-caret">
                            <li><a class="dropdown-item " href="index.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="shopping-cart"></span>E commerce
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="dashboard/project-management.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                            data-feather="clipboard"></span>Project
                                        management
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="dashboard/crm.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="phone"></span>CRM
                                    </div>
                                </a></li>
                            <li><a class="dropdown-item" href="apps/social/feed.html">
                                    <div class="dropdown-item-wrapper"><span class="me-2 uil" data-feather="share-2"></span>Social
                                        feed
                                    </div>
                                </a></li>
                        </ul>
                    </li>

                </ul>
            </div>
            <ul class="navbar-nav navbar-nav-icons flex-row">
                <li class="nav-item">
                    <div class="theme-control-toggle fa-ion-wait pe-2 theme-control-toggle-slim"><input
                            class="form-check-input ms-0 theme-control-toggle-input" id="themeControlToggle"
                            type="checkbox" data-theme-control="phoenixTheme" value="dark" /><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-light" for="themeControlToggle"
                            title="Switch theme"><span
                                class="icon me-1 d-none d-sm-block" data-feather="moon"></span><span
                                class="fs--1 fw-bold">Dark</span></label><label
                            class="mb-0 theme-control-toggle-label theme-control-toggle-dark" for="themeControlToggle"
                            title="Switch theme"><span
                                class="icon me-1 d-none d-sm-block" data-feather="sun"></span><span
                                class="fs--1 fw-bold">Light</span></label></div>
                </li>
                <li class="nav-item"><a class="nav-link" href="#" data-bs-toggle="modal"
                        data-bs-target="#searchBoxModal"><span data-feather="search"
                            style="height:12px;width:12px;"></span></a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link" id="navbarDropdownNotification" href="#" role="button" data-bs-toggle="dropdown"
                        data-bs-auto-close="outside" aria-haspopup="true" aria-expanded="false"><span data-feather="bell"
                            style="height:12px;width:12px;"></span></a>

                    <div class="dropdown-menu dropdown-menu-end notification-dropdown-menu py-0 shadow border border-300 navbar-dropdown-caret"
                        id="navbarDropdownNotfication" aria-labelledby="navbarDropdownNotfication">
                        <div class="card position-relative border-0">
                            <div class="card-header p-2">
                                <div class="d-flex justify-content-between">
                                    <h5 class="text-black mb-0">Notificatons</h5>
                                    <button class="btn btn-link p-0 fs--1 fw-normal" type="button">Mark all as read</button>
                                </div>
                            </div>
                            <div class="card-body p-0">
                                <div class="scrollbar-overlay" style="height: 27rem;">
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/30.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2">10m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:41 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3">
                                                        <div class="avatar-name rounded-circle"><span>J</span></div>
                                                    </div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jane Foster</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Created an event.<span
                                                                class="ms-2 text-400 fw-bold fs--2">20m</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:20 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle avatar-placeholder"
                                                            src="{{ asset('public/assets/img/team/40x40/avatar.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2">1h</span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:30 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="border-300">
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Kiera Anderson</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Mentioned you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">9:11 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/59.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Herman Carter</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Tagged you in a
                                                            comment.<span class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:58 PM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read ">
                                            <div class="d-flex align-items-center justify-content-between position-relative">
                                                <div class="d-flex">
                                                    <div class="avatar avatar-m status-online me-3"><img
                                                            class="rounded-circle"
                                                            src="{{ asset('public/assets/img/team/40x40/58.webp'); }}"
                                                            alt="" /></div>
                                                    <div class="flex-1 me-sm-3">
                                                        <h4 class="fs--1 text-black">Benjamin Button</h4>

                                                        <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                class='me-1 fs--2'>?</span>Liked your comment.<span
                                                                class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                        <p class="text-800 fs--1 mb-0"><span
                                                                class="me-1 fas fa-clock"></span><span class="fw-bold">10:18 AM </span>August
                                                            7,2021</p>
                                                    </div>
                                                </div>
                                                <div class="font-sans-serif d-none d-sm-block">
                                                    <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                        type="button" data-bs-toggle="dropdown" data-boundary="window"
                                                        aria-haspopup="true" aria-expanded="false"
                                                        data-bs-reference="parent"><span
                                                            class="fas fa-ellipsis-h fs--2 text-900"></span></button>
                                                    <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                            class="dropdown-item" href="#!">Mark as unread</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer p-0 border-top border-0">
                                <div class="my-2 text-center fw-bold fs--2 text-600"><a class="fw-bolder"
                                        href="pages/notifications.html">Notification
                                        history</a></div>
                            </div>
                        </div>
                    </div>
                </li>
                <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0 white-space-nowrap" id="navbarDropdownUser"
                        href="#!" role="button" data-bs-toggle="dropdown" aria-haspopup="true"
                        data-bs-auto-close="outside" aria-expanded="false">Olivia <span
                            class="fa-solid fa-chevron-down fs--2"></span></a>

                    <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                        aria-labelledby="navbarDropdownUser">
                        <div class="card position-relative border-0">
                            <div class="card-body p-0">
                                <div class="text-center pt-4 pb-3">
                                    <div class="avatar avatar-xl ">
                                        <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                    </div>
                                    <h6 class="mt-2 text-black">Jerry Seinfield</h6>
                                </div>
                                <div class="mb-3 mx-3"><input class="form-control form-control-sm" id="statusUpdateInput"
                                        type="text" placeholder="Update your status" /></div>
                            </div>
                            <div class="overflow-auto scrollbar" style="height: 10rem;">
                                <ul class="nav d-flex flex-column mb-2 pb-1">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user"></span><span>Profile</span></a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"><span class="me-2 text-900"
                                                data-feather="pie-chart"></span>Dashboard</a>
                                    </li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="lock"></span>Posts
                                            &amp; Activity</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="settings"></span>Settings
                                            &amp; Privacy </a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="help-circle"></span>Help
                                            Center</a></li>
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="globe"></span>Language</a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-footer p-0 border-top">
                                <ul class="nav d-flex flex-column my-3">
                                    <li class="nav-item"><a class="nav-link px-3" href="#!"> <span class="me-2 text-900"
                                                data-feather="user-plus"></span>Add
                                            another account</a></li>
                                </ul>
                                <hr />
                                <div class="px-3"><a class="btn btn-phoenix-secondary d-flex flex-center w-100" href="#!">
                                        <span class="me-2" data-feather="log-out"> </span>Sign out</a></div>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </nav>
        <nav class="navbar navbar-top fixed-top navbar-expand-lg" id="dualNav" style="display:none;">
            <div class="w-100">
                <div class="d-flex flex-between-center dual-nav-first-layer">
                    <div class="navbar-logo">
                        <button class="btn navbar-toggler navbar-toggler-humburger-icon hover-bg-transparent" type="button"
                            data-bs-toggle="collapse" data-bs-target="#navbarTopCollapse"
                            aria-controls="navbarTopCollapse" aria-expanded="false" aria-label="Toggle Navigation"><span
                                class="navbar-toggle-icon"><span class="toggle-line"></span></span></button>
                        <div class="dropdown lm-appswitch"><a class="navbar-brand me-1 me-sm-3 lm-appswitch-toggle" href="{{ config('erp.enabled') ? '#' : route('home') }}" @if(config('erp.enabled')) role="button" data-bs-toggle="dropdown" data-bs-auto-close="outside" aria-expanded="false" title="Switch application" @endif>
                            <div class="d-flex align-items-center">
                                <div class="d-flex align-items-center"><span class="lm-brand-mark" aria-hidden="true"><img src="{{ asset('public/assets/img/lmis-logo.svg') }}" alt=""></span><span class="lm-brand-word"><span>Land Information</span><span>Management System</span></span>

                                    <p class="logo-text ms-2 d-none d-sm-block">{{ config('app.name', 'LMIS') }}</p>
                                </div>
                            </div>
                        @if(config('erp.enabled'))<span class="lm-appswitch-caret" aria-hidden="true"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"><polyline points="6 9 12 15 18 9"/></svg></span>@endif</a>@include('partials.app-switcher')</div>
                    </div>
                    <div class="search-box navbar-top-search-box d-none d-lg-block" data-list='{"valueNames":["title"]}'
                        style="width:25rem;">
                        <form class="position-relative" data-bs-toggle="search" data-bs-display="static"><input
                                class="form-control search-input fuzzy-search rounded-pill form-control-sm"
                                type="search" placeholder="Search..." aria-label="Search" />
                            <span class="fas fa-search search-box-icon"></span>
                        </form>
                        <div class="btn-close position-absolute end-0 top-50 translate-middle cursor-pointer shadow-none"
                            data-bs-dismiss="search">
                            <button class="btn btn-link btn-close-falcon p-0" aria-label="Close"></button>
                        </div>
                        <div class="dropdown-menu border border-300 font-base start-0 py-0 overflow-hidden w-100">
                            <div class="scrollbar-overlay" style="max-height: 30rem;">
                                <div class="list pb-3">
                                    <h6 class="dropdown-header text-1000 fs--2 py-2">24 <span
                                            class="text-500">results</span></h6>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">Recently
                                        Searched </h6>

                                    <div class="py-2"><a class="dropdown-item"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-clock-rotate-left"
                                                        data-fa-transform="shrink-2"></span> Store Macbook
                                                </div>
                                            </div>
                                        </a>
                                        <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-clock-rotate-left"
                                                        data-fa-transform="shrink-2"></span> MacBook Air - 13?
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                        Products</h6>

                                    <div class="py-2"><a class="dropdown-item py-2 d-flex align-items-center"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="file-thumbnail me-2"><img class="h-100 w-100 fit-cover rounded-3"
                                                    src="{{ asset('public/assets/img/products/60x60/3.png'); }}"
                                                    alt="" /></div>
                                            <div class="flex-1">
                                                <h6 class="mb-0 text-1000 title">MacBook Air - 13?</h6>

                                                <p class="fs--2 mb-0 d-flex text-700"><span class="fw-medium text-600">8GB Memory - 1.6GHz - 128GB Storage</span>
                                                </p>
                                            </div>
                                        </a>
                                        <a class="dropdown-item py-2 d-flex align-items-center"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="file-thumbnail me-2"><img class="img-fluid"
                                                    src="{{ asset('public/assets/img/products/60x60/3.png'); }}"
                                                    alt="" /></div>
                                            <div class="flex-1">
                                                <h6 class="mb-0 text-1000 title">MacBook Pro - 13?</h6>

                                                <p class="fs--2 mb-0 d-flex text-700"><span class="fw-medium text-600 ms-2">30 Sep at 12:30 PM</span>
                                                </p>
                                            </div>
                                        </a>
                                    </div>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">Quick
                                        Links</h6>

                                    <div class="py-2"><a class="dropdown-item"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-link text-900"
                                                        data-fa-transform="shrink-2"></span> Support MacBook House
                                                </div>
                                            </div>
                                        </a>
                                        <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-link text-900"
                                                        data-fa-transform="shrink-2"></span> Store MacBook?
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                        Files</h6>

                                    <div class="py-2"><a class="dropdown-item"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-file-zipper text-900"
                                                        data-fa-transform="shrink-2"></span> Library MacBook folder.rar
                                                </div>
                                            </div>
                                        </a>
                                        <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-file-lines text-900"
                                                        data-fa-transform="shrink-2"></span> Feature MacBook
                                                    extensions.txt
                                                </div>
                                            </div>
                                        </a>
                                        <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-solid fa-image text-900"
                                                        data-fa-transform="shrink-2"></span> MacBook Pro_13.jpg
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                        Members</h6>

                                    <div class="py-2"><a class="dropdown-item py-2 d-flex align-items-center"
                                            href="pages/members.html">
                                            <div class="avatar avatar-l status-online  me-2 text-900">
                                                <img class="rounded-circle "
                                                    src="{{ asset('public/assets/img/team/40x40/10.webp'); }}" alt="" />
                                            </div>
                                            <div class="flex-1">
                                                <h6 class="mb-0 text-1000 title">Carry Anna</h6>

                                                <p class="fs--2 mb-0 d-flex text-700">anna@technext.it</p>
                                            </div>
                                        </a>
                                        <a class="dropdown-item py-2 d-flex align-items-center" href="pages/members.html">
                                            <div class="avatar avatar-l  me-2 text-900">
                                                <img class="rounded-circle "
                                                    src="{{ asset('public/assets/img/team/40x40/12.webp'); }}" alt="" />
                                            </div>
                                            <div class="flex-1">
                                                <h6 class="mb-0 text-1000 title">John Smith</h6>

                                                <p class="fs--2 mb-0 d-flex text-700">smith@technext.it</p>
                                            </div>
                                        </a>
                                    </div>
                                    <hr class="text-200 my-0" />
                                    <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">Related
                                        Searches</h6>

                                    <div class="py-2"><a class="dropdown-item"
                                            href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-brands fa-firefox-browser text-900"
                                                        data-fa-transform="shrink-2"></span> Search in the Web MacBook
                                                </div>
                                            </div>
                                        </a>
                                        <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                            <div class="d-flex align-items-center">
                                                <div class="fw-normal text-1000 title"><span
                                                        class="fa-brands fa-chrome text-900"
                                                        data-fa-transform="shrink-2"></span> Store MacBook?
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </div>
                                <div class="text-center">
                                    <p class="fallback fw-bold fs-1 d-none">No Result Found.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <ul class="navbar-nav navbar-nav-icons flex-row">
                        <li class="nav-item">
                            <div class="theme-control-toggle fa-icon-wait px-2"><input
                                    class="form-check-input ms-0 theme-control-toggle-input" type="checkbox"
                                    data-theme-control="phoenixTheme" value="dark" id="themeControlToggle" /><label
                                    class="mb-0 theme-control-toggle-label theme-control-toggle-light"
                                    for="themeControlToggle"
                                    title="Switch theme"><span class="icon" data-feather="moon"></span></label><label
                                    class="mb-0 theme-control-toggle-label theme-control-toggle-dark"
                                    for="themeControlToggle"
                                    title="Switch theme"><span class="icon" data-feather="sun"></span></label></div>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link" href="#" style="min-width: 2.5rem" role="button" data-bs-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside"><span
                                    data-feather="bell" style="height:20px;width:20px;"></span></a>

                            <div class="dropdown-menu dropdown-menu-end notification-dropdown-menu py-0 shadow border border-300 navbar-dropdown-caret"
                                id="navbarDropdownNotfication" aria-labelledby="navbarDropdownNotfication">
                                <div class="card position-relative border-0">
                                    <div class="card-header p-2">
                                        <div class="d-flex justify-content-between">
                                            <h5 class="text-black mb-0">Notificatons</h5>
                                            <button class="btn btn-link p-0 fs--1 fw-normal" type="button">Mark all as
                                                read
                                            </button>
                                        </div>
                                    </div>
                                    <div class="card-body p-0">
                                        <div class="scrollbar-overlay" style="height: 27rem;">
                                            <div class="border-300">
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read border-bottom">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3"><img
                                                                    class="rounded-circle"
                                                                    src="{{ asset('public/assets/img/team/40x40/30.webp'); }}"
                                                                    alt="" /></div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Mentioned you in a
                                                                    comment.<span
                                                                        class="ms-2 text-400 fw-bold fs--2">10m</span>
                                                                </p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">10:41 AM </span>August 7,2021
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3">
                                                                <div class="avatar-name rounded-circle"><span>J</span></div>
                                                            </div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Jane Foster</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Created an
                                                                    event.<span
                                                                        class="ms-2 text-400 fw-bold fs--2">20m</span>
                                                                </p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">10:20 AM </span>August 7,2021
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3"><img
                                                                    class="rounded-circle avatar-placeholder"
                                                                    src="{{ asset('public/assets/img/team/40x40/avatar.webp'); }}"
                                                                    alt="" />
                                                            </div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Jessie Samson</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Liked your
                                                                    comment.<span
                                                                        class="ms-2 text-400 fw-bold fs--2">1h</span>
                                                                </p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">9:30 AM </span>August 7,2021</p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="border-300">
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3"><span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span></div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Kiera Anderson</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Mentioned you in a
                                                                    comment.<span
                                                                        class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">9:11 AM </span>August 7,2021</p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative unread border-bottom">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3"><img
                                                                    class="rounded-circle"
                                                                    src="{{ asset('public/assets/img/team/40x40/59.webp'); }}"
                                                                    alt="" /></div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Herman Carter</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Tagged you in a
                                                                    comment.<span
                                                                        class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">10:58 PM </span>August 7,2021
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="px-2 px-sm-3 py-3 border-300 notification-card position-relative read ">
                                                    <div class="d-flex align-items-center justify-content-between position-relative">
                                                        <div class="d-flex">
                                                            <div class="avatar avatar-m status-online me-3"><img
                                                                    class="rounded-circle"
                                                                    src="{{ asset('public/assets/img/team/40x40/58.webp'); }}"
                                                                    alt="" /></div>
                                                            <div class="flex-1 me-sm-3">
                                                                <h4 class="fs--1 text-black">Benjamin Button</h4>

                                                                <p class="fs--1 text-1000 mb-2 mb-sm-3 fw-normal"><span
                                                                        class='me-1 fs--2'>?</span>Liked your
                                                                    comment.<span
                                                                        class="ms-2 text-400 fw-bold fs--2"></span></p>

                                                                <p class="text-800 fs--1 mb-0"><span
                                                                        class="me-1 fas fa-clock"></span><span
                                                                        class="fw-bold">10:18 AM </span>August 7,2021
                                                                </p>
                                                            </div>
                                                        </div>
                                                        <div class="font-sans-serif d-none d-sm-block">
                                                            <button class="btn fs--2 btn-sm dropdown-toggle dropdown-caret-none transition-none notification-dropdown-toggle"
                                                                type="button" data-bs-toggle="dropdown"
                                                                data-boundary="window" aria-haspopup="true"
                                                                aria-expanded="false" data-bs-reference="parent"><span
                                                                    class="fas fa-ellipsis-h fs--2 text-900"></span>
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end py-2"><a
                                                                    class="dropdown-item" href="#!">Mark as unread</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer p-0 border-top border-0">
                                        <div class="my-2 text-center fw-bold fs--2 text-600"><a class="fw-bolder"
                                                href="pages/notifications.html">Notification
                                                history</a></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li class="nav-item dropdown"><a class="nav-link lh-1 pe-0" id="navbarDropdownUser" href="#!"
                                role="button" data-bs-toggle="dropdown"
                                data-bs-auto-close="outside" aria-haspopup="true"
                                aria-expanded="false">
                                <div class="avatar avatar-l ">
                                    <span class="lm-avatar" aria-hidden="true"><span data-feather="user"></span></span>
                                </div>
                            </a>

                            <div class="dropdown-menu dropdown-menu-end navbar-dropdown-caret py-0 dropdown-profile shadow border border-300"
                                aria-labelledby="navbarDropdownUser">
                                <div class="card position-relative border-0">
                                    <div class="card-body p-0">
                                        <div class="text-center pt-4 pb-3">
                                            <div class="avatar avatar-xl ">
                                                <span class="lm-avatar lm-avatar-xl" aria-hidden="true"><span data-feather="user"></span></span>
                                            </div>
                                            <h6 class="mt-2 text-black">Jerry Seinfield</h6>
                                        </div>
                                        <div class="mb-3 mx-3"><input class="form-control form-control-sm"
                                                id="statusUpdateInput" type="text"
                                                placeholder="Update your status" /></div>
                                    </div>
                                    <div class="overflow-auto scrollbar" style="height: 10rem;">
                                        <ul class="nav d-flex flex-column mb-2 pb-1">
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900"
                                                        data-feather="user"></span><span>Profile</span></a></li>
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"><span
                                                        class="me-2 text-900" data-feather="pie-chart"></span>Dashboard</a>
                                            </li>
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900" data-feather="lock"></span>Posts &amp;
                                                    Activity</a></li>
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900" data-feather="settings"></span>Settings
                                                    &amp; Privacy </a></li>
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900" data-feather="help-circle"></span>Help
                                                    Center</a></li>
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900" data-feather="globe"></span>Language</a>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="card-footer p-0 border-top">
                                        <ul class="nav d-flex flex-column my-3">
                                            <li class="nav-item"><a class="nav-link px-3" href="#!"> <span
                                                        class="me-2 text-900" data-feather="user-plus"></span>Add
                                                    another account</a></li>
                                        </ul>
                                        <hr />
                                        <div class="px-3"><a class="btn btn-phoenix-secondary d-flex flex-center w-100"
                                                href="#!"> <span class="me-2" data-feather="log-out"> </span>Sign
                                                out</a></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="collapse navbar-collapse navbar-top-collapse justify-content-center" id="navbarTopCollapse">
                    <ul class="navbar-nav navbar-nav-top" data-dropdown-on-hover="data-dropdown-on-hover">
                        <li class="nav-item dropdown"><a class="nav-link dropdown-toggle lh-1" href="#!" role="button"
                                data-bs-toggle="dropdown" data-bs-auto-close="outside"
                                aria-haspopup="true" aria-expanded="false"><span
                                    class="uil fs-0 me-2 uil-chart-pie"></span>Home</a>
                            <ul class="dropdown-menu navbar-dropdown-caret">
                                <li><a class="dropdown-item " href="index.html">
                                        <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                                data-feather="shopping-cart"></span>E
                                            commerce
                                        </div>
                                    </a></li>
                                <li><a class="dropdown-item" href="dashboard/project-management.html">
                                        <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                                data-feather="clipboard"></span>Project
                                            management
                                        </div>
                                    </a></li>
                                <li><a class="dropdown-item" href="dashboard/crm.html">
                                        <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                                data-feather="phone"></span>CRM
                                        </div>
                                    </a></li>
                                <li><a class="dropdown-item" href="apps/social/feed.html">
                                        <div class="dropdown-item-wrapper"><span class="me-2 uil"
                                                data-feather="share-2"></span>Social feed
                                        </div>
                                    </a></li>
                            </ul>
                        </li>

                    </ul>
                </div>
            </div>
        </nav>
        <div class="modal fade" id="searchBoxModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="true"
            data-phoenix-modal="data-phoenix-modal" style="--phoenix-backdrop-opacity: 1;">
            <div class="modal-dialog">
                <div class="modal-content mt-15 rounded-pill">
                    <div class="modal-body p-0">
                        <div class="search-box navbar-top-search-box" data-list='{"valueNames":["title"]}'
                            style="width: auto;">
                            <form class="position-relative" data-bs-toggle="search" data-bs-display="static"><input
                                    class="form-control search-input fuzzy-search rounded-pill form-control-lg"
                                    type="search" placeholder="Search..." aria-label="Search" />
                                <span class="fas fa-search search-box-icon"></span>
                            </form>
                            <div class="btn-close position-absolute end-0 top-50 translate-middle cursor-pointer shadow-none"
                                data-bs-dismiss="search">
                                <button class="btn btn-link btn-close-falcon p-0" aria-label="Close"></button>
                            </div>
                            <div class="dropdown-menu border border-300 font-base start-0 py-0 overflow-hidden w-100">
                                <div class="scrollbar-overlay" style="max-height: 30rem;">
                                    <div class="list pb-3">
                                        <h6 class="dropdown-header text-1000 fs--2 py-2">24 <span
                                                class="text-500">results</span></h6>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Recently Searched </h6>

                                        <div class="py-2"><a class="dropdown-item"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-clock-rotate-left"
                                                            data-fa-transform="shrink-2"></span> Store Macbook
                                                    </div>
                                                </div>
                                            </a>
                                            <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-clock-rotate-left"
                                                            data-fa-transform="shrink-2"></span> MacBook Air - 13?
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Products</h6>

                                        <div class="py-2"><a class="dropdown-item py-2 d-flex align-items-center"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="file-thumbnail me-2"><img
                                                        class="h-100 w-100 fit-cover rounded-3"
                                                        src="{{ asset('public/assets/img/products/60x60/3.png'); }}"
                                                        alt="" /></div>
                                                <div class="flex-1">
                                                    <h6 class="mb-0 text-1000 title">MacBook Air - 13?</h6>

                                                    <p class="fs--2 mb-0 d-flex text-700"><span class="fw-medium text-600">8GB Memory - 1.6GHz - 128GB Storage</span>
                                                    </p>
                                                </div>
                                            </a>
                                            <a class="dropdown-item py-2 d-flex align-items-center"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="file-thumbnail me-2"><img class="img-fluid"
                                                        src="{{ asset('public/assets/img/products/60x60/3.png'); }}"
                                                        alt="" /></div>
                                                <div class="flex-1">
                                                    <h6 class="mb-0 text-1000 title">MacBook Pro - 13?</h6>

                                                    <p class="fs--2 mb-0 d-flex text-700"><span
                                                            class="fw-medium text-600 ms-2">30 Sep at 12:30 PM</span>
                                                    </p>
                                                </div>
                                            </a>
                                        </div>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Quick Links</h6>

                                        <div class="py-2"><a class="dropdown-item"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-link text-900"
                                                            data-fa-transform="shrink-2"></span> Support MacBook House
                                                    </div>
                                                </div>
                                            </a>
                                            <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-link text-900"
                                                            data-fa-transform="shrink-2"></span> Store MacBook?
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Files</h6>

                                        <div class="py-2"><a class="dropdown-item"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-file-zipper text-900"
                                                            data-fa-transform="shrink-2"></span> Library MacBook
                                                        folder.rar
                                                    </div>
                                                </div>
                                            </a>
                                            <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-file-lines text-900"
                                                            data-fa-transform="shrink-2"></span> Feature MacBook
                                                        extensions.txt
                                                    </div>
                                                </div>
                                            </a>
                                            <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-solid fa-image text-900"
                                                            data-fa-transform="shrink-2"></span> MacBook Pro_13.jpg
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Members</h6>

                                        <div class="py-2"><a class="dropdown-item py-2 d-flex align-items-center"
                                                href="pages/members.html">
                                                <div class="avatar avatar-l status-online  me-2 text-900">
                                                    <img class="rounded-circle "
                                                        src="{{ asset('public/assets/img/team/40x40/10.webp'); }}"
                                                        alt="" />
                                                </div>
                                                <div class="flex-1">
                                                    <h6 class="mb-0 text-1000 title">Carry Anna</h6>

                                                    <p class="fs--2 mb-0 d-flex text-700">anna@technext.it</p>
                                                </div>
                                            </a>
                                            <a class="dropdown-item py-2 d-flex align-items-center"
                                                href="pages/members.html">
                                                <div class="avatar avatar-l  me-2 text-900">
                                                    <img class="rounded-circle "
                                                        src="{{ asset('public/assets/img/team/40x40/12.webp'); }}"
                                                        alt="" />
                                                </div>
                                                <div class="flex-1">
                                                    <h6 class="mb-0 text-1000 title">John Smith</h6>

                                                    <p class="fs--2 mb-0 d-flex text-700">smith@technext.it</p>
                                                </div>
                                            </a>
                                        </div>
                                        <hr class="text-200 my-0" />
                                        <h6 class="dropdown-header text-1000 fs--1 border-bottom border-200 py-2 lh-sm">
                                            Related Searches</h6>

                                        <div class="py-2"><a class="dropdown-item"
                                                href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-brands fa-firefox-browser text-900"
                                                            data-fa-transform="shrink-2"></span> Search in the Web
                                                        MacBook
                                                    </div>
                                                </div>
                                            </a>
                                            <a class="dropdown-item" href="apps/e-commerce/landing/product-details.html">
                                                <div class="d-flex align-items-center">
                                                    <div class="fw-normal text-1000 title"><span
                                                            class="fa-brands fa-chrome text-900"
                                                            data-fa-transform="shrink-2"></span> Store MacBook?
                                                    </div>
                                                </div>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="text-center">
                                        <p class="fallback fw-bold fs-1 d-none">No Result Found.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="documentApprovalModal" tabindex="-1" aria-labelledby="documentApprovalModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="documentApprovalModalLabel">Document Approval History</h5>
                        {{--<button type="button" class="close" data-dismiss="modal" aria-label="Close">--}}
                        {{--<span aria-hidden="true">&times;</span>--}}
                        {{--</button>--}}
                    </div>
                    <div class="modal-body">
                        <table class="table table-bordered" id="approvalHistoryTable">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Document Name</th>
                                    <th>Approval User</th>
                                    <th>Status</th>
                                    <th>Remarks</th>
                                    <th>Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- Data will be appended here -->
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <script>
            var navbarTopShape = window.config.config.phoenixNavbarTopShape;
            var navbarPosition = window.config.config.phoenixNavbarPosition;
            var body = document.querySelector('body');
            var navbarDefault = document.querySelector('#navbarDefault');
            var navbarTop = document.querySelector('#navbarTop');
            var topNavSlim = document.querySelector('#topNavSlim');
            var navbarTopSlim = document.querySelector('#navbarTopSlim');
            var navbarCombo = document.querySelector('#navbarCombo');
            var navbarComboSlim = document.querySelector('#navbarComboSlim');
            var dualNav = document.querySelector('#dualNav');

            var documentElement = document.documentElement;
            var navbarVertical = document.querySelector('.navbar-vertical');

            if (navbarPosition === 'dual-nav') {
                topNavSlim.remove();
                navbarTop.remove();
                navbarVertical.remove();
                navbarTopSlim.remove();
                navbarCombo.remove();
                navbarComboSlim.remove();
                navbarDefault.remove();
                dualNav.removeAttribute('style');
                documentElement.classList.add('dual-nav');
            } else if (navbarTopShape === 'slim' && navbarPosition === 'vertical') {
                navbarDefault.remove();
                navbarTop.remove();
                navbarTopSlim.remove();
                navbarCombo.remove();
                navbarComboSlim.remove();
                topNavSlim.style.display = 'block';
                navbarVertical.style.display = 'inline-block';
                body.classList.add('nav-slim');
            } else if (navbarTopShape === 'slim' && navbarPosition === 'horizontal') {
                navbarDefault.remove();
                navbarVertical.remove();
                navbarTop.remove();
                topNavSlim.remove();
                navbarCombo.remove();
                navbarComboSlim.remove();
                navbarTopSlim.removeAttribute('style');
                body.classList.add('nav-slim');
            } else if (navbarTopShape === 'slim' && navbarPosition === 'combo') {
                navbarDefault.remove();
                //- navbarVertical.remove();
                navbarTop.remove();
                topNavSlim.remove();
                navbarCombo.remove();
                navbarTopSlim.remove();
                navbarComboSlim.removeAttribute('style');
                navbarVertical.removeAttribute('style');
                body.classList.add('nav-slim');
            } else if (navbarTopShape === 'default' && navbarPosition === 'horizontal') {
                navbarDefault.remove();
                topNavSlim.remove();
                navbarVertical.remove();
                navbarTopSlim.remove();
                navbarCombo.remove();
                navbarComboSlim.remove();
                navbarTop.removeAttribute('style');
                documentElement.classList.add('navbar-horizontal');
            } else if (navbarTopShape === 'default' && navbarPosition === 'combo') {
                topNavSlim.remove();
                navbarTop.remove();
                navbarTopSlim.remove();
                navbarDefault.remove();
                navbarComboSlim.remove();
                navbarCombo.removeAttribute('style');
                navbarVertical.removeAttribute('style');
                documentElement.classList.add('navbar-combo')

            } else {
                topNavSlim.remove();
                navbarTop.remove();
                navbarTopSlim.remove();
                navbarCombo.remove();
                navbarComboSlim.remove();
                navbarDefault.removeAttribute('style');
                navbarVertical.removeAttribute('style');
            }

            var navbarTopStyle = window.config.config.phoenixNavbarTopStyle;
            var navbarTop = document.querySelector('.navbar-top');
            if (navbarTopStyle === 'darker') {
                navbarTop.classList.add('navbar-darker');
            }

            var navbarVerticalStyle = window.config.config.phoenixNavbarVerticalStyle;
            var navbarVertical = document.querySelector('.navbar-vertical');
            if (navbarVerticalStyle === 'darker') {
                navbarVertical.classList.add('navbar-darker');
            }
        </script>
        @yield('content')

    </main>
    <!-- ===============================================-->
    <!--    End of Main Content-->
    <!-- ===============================================-->

    <div class="offcanvas offcanvas-end settings-panel border-0" id="settings-offcanvas" tabindex="-1"
        aria-labelledby="settings-offcanvas">
        <div class="offcanvas-header align-items-start border-bottom flex-column">
            <div class="pt-1 w-100 mb-6 d-flex justify-content-between align-items-start">
                <div>
                    <h5 class="mb-2 me-2 lh-sm"><span class="fas fa-palette me-2 fs-0"></span>Theme Customizer</h5>

                    <p class="mb-0 fs--1">Explore different styles according to your preferences</p>
                </div>
                <button class="btn p-1 fw-bolder" type="button" data-bs-dismiss="offcanvas" aria-label="Close"><span
                        class="fas fa-times fs-0"> </span></button>
            </div>
            <button class="btn btn-phoenix-secondary w-100" data-theme-control="reset"><span
                    class="fas fa-arrows-rotate me-2 fs--2"></span>Reset to default
            </button>
        </div>
        <div class="offcanvas-body scrollbar px-card" id="themeController">
            {{--<div class="setting-panel-item mt-0">--}}
            {{--<h5 class="setting-panel-item-title">Color Scheme</h5>--}}

            {{--<div class="row gx-2">--}}
            {{--<div class="col-6"><input class="btn-check" id="themeSwitcherLight" name="theme-color" type="radio"--}}
            {{--value="light" data-theme-control="phoenixTheme"/><label--}}
            {{--class="btn d-inline-block btn-navbar-style fs--1" for="themeSwitcherLight"> <span--}}
            {{--class="mb-2 rounded d-block"><img class="img-fluid img-prototype mb-0"--}}
            {{--src="{{ asset('public/assets/img/generic/default-light.png'); }}"--}}
            {{--alt=""/></span><span--}}
            {{--class="label-text">Light</span></label></div>--}}
            {{--<div class="col-6"><input class="btn-check" id="themeSwitcherDark" name="theme-color" type="radio"--}}
            {{--value="dark" data-theme-control="phoenixTheme"/><label--}}
            {{--class="btn d-inline-block btn-navbar-style fs--1" for="themeSwitcherDark"> <span--}}
            {{--class="mb-2 rounded d-block"><img class="img-fluid img-prototype mb-0"--}}
            {{--src="{{ asset('public/assets/img/generic/default-dark.png'); }}"--}}
            {{--alt=""/></span><span--}}
            {{--class="label-text"> Dark</span></label></div>--}}
            {{--</div>--}}
            {{--</div>--}}
            {{--<div class="setting-panel-item">--}}
            {{--<h5 class="setting-panel-item-title">Navigation Type</h5>--}}

            {{--<div class="row gx-2">--}}
            {{--<div class="col-6"><input class="btn-check" id="navbarPositionVertical" name="navigation-type"--}}
            {{--type="radio" value="vertical"--}}
            {{--data-theme-control="phoenixNavbarPosition"/><label--}}
            {{--class="btn d-inline-block btn-navbar-style fs--1" for="navbarPositionVertical"> <span--}}
            {{--class="mb-2 rounded d-block"><img class="img-fluid img-prototype d-dark-none"--}}
            {{--src="public/assets/img/generic/default-light.png"--}}
            {{--alt=""/><img--}}
            {{--class="img-fluid img-prototype d-light-none"--}}
            {{--src="public/assets/img/generic/default-dark.png" alt=""/></span><span class="label-text">Vertical</span></label>--}}
            {{--</div>--}}
            {{--<div class="col-6"><input class="btn-check" id="navbarPositionHorizontal" name="navigation-type"--}}
            {{--type="radio" value="horizontal"--}}
            {{--data-theme-control="phoenixNavbarPosition"/><label--}}
            {{--class="btn d-inline-block btn-navbar-style fs--1" for="navbarPositionHorizontal"> <span--}}
            {{--class="mb-2 rounded d-block"><img class="img-fluid img-prototype d-dark-none"--}}
            {{--src="public/assets/img/generic/top-default.png" alt=""/><img--}}
            {{--class="img-fluid img-prototype d-light-none"--}}
            {{--src="public/assets/img/generic/top-default-dark.png" alt=""/></span><span--}}
            {{--class="label-text"> Horizontal</span></label></div>--}}
            {{--</div>--}}
            {{--</div>--}}
            {{--<div class="setting-panel-item">--}}
            {{--<h5 class="setting-panel-item-title">Vertical Navbar Appearance</h5>--}}

            {{--<div class="row gx-2">--}}
            {{--<div class="col-6"><input class="btn-check" id="navbar-style-default" type="radio" name="config.name"--}}
            {{--value="default" data-theme-control="phoenixNavbarVerticalStyle"/><label--}}
            {{--class="btn d-block w-100 btn-navbar-style fs--1" for="navbar-style-default"> <img--}}
            {{--class="img-fluid img-prototype d-dark-none" src="public/assets/img/generic/default-light.png"--}}
            {{--alt=""/><img class="img-fluid img-prototype d-light-none"--}}
            {{--src="public/assets/img/generic/default-dark.png" alt=""/><span--}}
            {{--class="label-text d-dark-none"> Default</span><span class="label-text d-light-none">Default</span></label>--}}
            {{--</div>--}}
            {{--<div class="col-6"><input class="btn-check" id="navbar-style-dark" type="radio" name="config.name"--}}
            {{--value="darker" data-theme-control="phoenixNavbarVerticalStyle"/><label--}}
            {{--class="btn d-block w-100 btn-navbar-style fs--1" for="navbar-style-dark"> <img--}}
            {{--class="img-fluid img-prototype d-dark-none" src="public/assets/img/generic/vertical-darker.png"--}}
            {{--alt=""/><img class="img-fluid img-prototype d-light-none"--}}
            {{--src="public/assets/img/generic/vertical-lighter.png" alt=""/><span--}}
            {{--class="label-text d-dark-none"> Darker</span><span class="label-text d-light-none">Lighter</span></label>--}}
            {{--</div>--}}
            {{--</div>--}}
            {{--</div>--}}
            <div class="setting-panel-item">
                <h5 class="setting-panel-item-title">Horizontal Navbar Appearance</h5>
                {{--<div class="row gx-2">--}}
                {{--<div class="col-6"><input class="btn-check" id="navbarTopDefault" name="navbar-top-style" type="radio" value="default" data-theme-control="phoenixNavbarTopStyle" /><label class="btn d-inline-block btn-navbar-style fs--1" for="navbarTopDefault"> <span class="mb-2 rounded d-block"><img class="img-fluid img-prototype d-dark-none mb-0" src="public/assets/img/generic/top-default.png" alt=""/><img class="img-fluid img-prototype d-light-none mb-0" src="public/assets/img/generic/top-style-darker.png" alt=""/></span><span class="label-text">Default</span></label></div>--}}
                {{--<div class="col-6"><input class="btn-check" id="navbarTopDarker" name="navbar-top-style" type="radio" value="darker" data-theme-control="phoenixNavbarTopStyle" /><label class="btn d-inline-block btn-navbar-style fs--1" for="navbarTopDarker"> <span class="mb-2 rounded d-block"><img class="img-fluid img-prototype d-dark-none mb-0" src="public/assets/img/generic/navbar-top-style-light.png" alt=""/><img class="img-fluid img-prototype d-light-none mb-0" src="public/assets/img/generic/top-style-lighter.png" alt=""/></span><span class="label-text d-dark-none">Darker</span><span class="label-text d-light-none">Lighter</span></label></div>--}}
                {{--</div>--}}
            </div>

        </div>
    </div>
    {{--<a class="card setting-toggle" href="#settings-offcanvas" data-bs-toggle="offcanvas">--}}
    {{--<div class="card-body d-flex align-items-center px-2 py-1">--}}
    {{--<div class="position-relative rounded-start" style="height:34px;width:28px">--}}
    {{--<div class="settings-popover"><span class="ripple"><span--}}
    {{--class="fa-spin position-absolute all-0 d-flex flex-center"><span--}}
    {{--class="icon-spin position-absolute all-0 d-flex flex-center"><svg width="20" height="20"--}}
    {{--viewBox="0 0 20 20"--}}
    {{--fill="#ffffff"--}}
    {{--xmlns="http://www.w3.org/2000/svg">--}}
    {{--<path d="M19.7369 12.3941L19.1989 12.1065C18.4459 11.7041 18.0843 10.8487 18.0843 9.99495C18.0843 9.14118 18.4459 8.28582 19.1989 7.88336L19.7369 7.59581C19.9474 7.47484 20.0316 7.23291 19.9474 7.03131C19.4842 5.57973 18.6843 4.28943 17.6738 3.20075C17.5053 3.03946 17.2527 2.99914 17.0422 3.12011L16.393 3.46714C15.6883 3.84379 14.8377 3.74529 14.1476 3.3427C14.0988 3.31422 14.0496 3.28621 14.0002 3.25868C13.2568 2.84453 12.7055 2.10629 12.7055 1.25525V0.70081C12.7055 0.499202 12.5371 0.297594 12.2845 0.257272C10.7266 -0.105622 9.16879 -0.0653007 7.69516 0.257272C7.44254 0.297594 7.31623 0.499202 7.31623 0.70081V1.23474C7.31623 2.09575 6.74999 2.8362 5.99824 3.25599C5.95774 3.27861 5.91747 3.30159 5.87744 3.32493C5.15643 3.74527 4.26453 3.85902 3.53534 3.45302L2.93743 3.12011C2.72691 2.99914 2.47429 3.03946 2.30587 3.20075C1.29538 4.28943 0.495411 5.57973 0.0322686 7.03131C-0.051939 7.23291 0.0322686 7.47484 0.242788 7.59581L0.784376 7.8853C1.54166 8.29007 1.92694 9.13627 1.92694 9.99495C1.92694 10.8536 1.54166 11.6998 0.784375 12.1046L0.242788 12.3941C0.0322686 12.515 -0.051939 12.757 0.0322686 12.9586C0.495411 14.4102 1.29538 15.7005 2.30587 16.7891C2.47429 16.9504 2.72691 16.9907 2.93743 16.8698L3.58669 16.5227C4.29133 16.1461 5.14131 16.2457 5.8331 16.6455C5.88713 16.6767 5.94159 16.7074 5.99648 16.7375C6.75162 17.1511 7.31623 17.8941 7.31623 18.7552V19.2891C7.31623 19.4425 7.41373 19.5959 7.55309 19.696C7.64066 19.7589 7.74815 19.7843 7.85406 19.8046C9.35884 20.0925 10.8609 20.0456 12.2845 19.7729C12.5371 19.6923 12.7055 19.4907 12.7055 19.2891V18.7346C12.7055 17.8836 13.2568 17.1454 14.0002 16.7312C14.0496 16.7037 14.0988 16.6757 14.1476 16.6472C14.8377 16.2446 15.6883 16.1461 16.393 16.5227L17.0422 16.8698C17.2527 16.9907 17.5053 16.9504 17.6738 16.7891C18.7264 15.7005 19.4842 14.4102 19.9895 12.9586C20.0316 12.757 19.9474 12.515 19.7369 12.3941ZM10.0109 13.2005C8.1162 13.2005 6.64257 11.7893 6.64257 9.97478C6.64257 8.20063 8.1162 6.74905 10.0109 6.74905C11.8634 6.74905 13.3792 8.20063 13.3792 9.97478C13.3792 11.7893 11.8634 13.2005 10.0109 13.2005Z"--}}
    {{--fill="#2A7BE4"></path>--}}
    {{--</svg></span></span></span></div>--}}
    {{--</div>--}}
    {{--<small class="text-uppercase text-700 fw-bold py-2 pe-2 ps-1 rounded-end">customize</small>--}}
    {{--</div>--}}
    {{--</a>--}}

    <!-- ===============================================-->
    <!--    JavaScripts-->
    <!-- ===============================================-->
    <script src="{{ asset('public/vendors/popper/popper.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/bootstrap/bootstrap.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/anchorjs/anchor.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/is/is.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/fontawesome/all.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/lodash/lodash.min.js'); }}"></script>
    {{--<script src="../../../polyfill.io/v3/polyfill.min58be.js?features=window.scroll"></script>--}}
    <script src="{{ asset('public/vendors/list.js/list.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/feather-icons/feather.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/dayjs/dayjs.min.js'); }}"></script>
    <script src="{{ asset('public/assets/js/phoenix.js'); }}"></script>
    <script src="{{ asset('public/vendors/echarts/echarts.min.js'); }}"></script>
    <script src="{{ asset('public/vendors/leaflet/leaflet.js'); }}"></script>
    <script src="{{ asset('public/vendors/leaflet.markercluster/leaflet.markercluster.js'); }}"></script>
    <script src="{{ asset('public/vendors/leaflet.tilelayer.colorfilter/leaflet-tilelayer-colorfilter.min.js'); }}"></script>
    <!-- DataTables JS -->
    <script src="{{ asset('public/vendors/datatables/jquery.dataTables.min.js') }}"></script>
    <script src="{{ asset('public/vendors/datatables/dataTables.bootstrap5.min.js') }}"></script>
    <script src="{{ asset('public/vendors/datatables/dataTables.responsive.min.js') }}"></script>
    <script src="{{ asset('public/vendors/datatables/responsive.bootstrap5.min.js') }}"></script>
    <!-- DataTables Auto-Initialize for Show Pages -->
    <script>
        $(document).ready(function() {
            // Initialize DataTables on all tables with class 'table' that are not excluded
            $('table.table').not('.table-no-datatable').each(function() {
                // Skip if already initialized
                if (!$.fn.DataTable.isDataTable(this)) {
                    try {
                        $(this).DataTable({
                            responsive: true,
                            pageLength: 25,
                            lengthMenu: [
                                [10, 25, 50, 100, -1],
                                [10, 25, 50, 100, "All"]
                            ],
                            order: [], // No default sorting
                            columnDefs: [{
                                orderable: false,
                                targets: -1 // Disable sorting on last column (action buttons)
                            }],
                            language: {
                                search: "Search Records:",
                                lengthMenu: "Show _MENU_ entries",
                                info: "Showing _START_ to _END_ of _TOTAL_ entries",
                                paginate: {
                                    first: "First",
                                    last: "Last",
                                    next: "Next",
                                    previous: "Previous"
                                }
                            },
                            dom: '<"row"<"col-md-6"l><"col-md-6"f>>t<"row"<"col-md-6"i><"col-md-6"p>>'
                        });
                    } catch (e) {
                        console.log('DataTables error:', e);
                    }
                }
            });
            // Fix DataTables width calculation in hidden Bootstrap tabs
            $('button[data-bs-toggle="tab"], a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust().responsive.recalc();
            });
        });
    </script>
    <script>
    //make select field readonly
    $(document).on('focus', '.readonly-select', function() {
        this.oldValue = this.value;
    }).on('change', '.readonly-select', function() {
        this.value = this.oldValue;
    });
</script>
    <script src="{{ asset('public/assets/js/ecommerce-dashboard.js'); }}"></script>
    <script src="{{ asset('public/assets/js/sidebar-active.js'); }}"></script>
    <script src="{{ asset('public/assets/js/lmis-theme.js') }}?v=20260828a"></script>
    <script src="{{ asset('public/vendors/dropzone/dropzone.min.js'); }}"></script>

    <script>
        function ViewHistory(id, table) {
            var documentId = id;
            var approval = table;

            $.ajax({
                url: '{{ route("approval_document_history") }}', // Update this with your route name
                type: 'POST',
                data: {
                    id: documentId,
                    approval: approval,
                    _token: '{{ csrf_token() }}' // Ensure CSRF token is included
                },
                success: function(response) {
                    console.log(response);

                    if (response.record) {
                        var tableBody = $('#approvalHistoryTable tbody');
                        tableBody.empty();

                        if (response.record != '') {
                            var count = 1;
                            response.record.forEach(function(item) {

                                var docstatus = '';
                                if (item.status == 1) {
                                    docstatus = 'Approved';
                                }
                                if (item.status == 2) {
                                    docstatus = 'Reject';

                                }

                                var row = '<tr>' +
                                    '<td>' + count + '</td>' +
                                    '<td>' + item.document_name + '</td>' +
                                    '<td>' + item.name + '</td>' +
                                    '<td>' + docstatus + '</td>' +
                                    '<td>' + item.remarks + '</td>' +
                                    '<td>' + item.updated_at + '</td>' +
                                    '</tr>';
                                tableBody.append(row);
                                count++;
                            });

                        } else {
                            var row = '<tr>' +
                                '<td colspan="6" style="text-align:center;font-weight:700">No History Exist</td>' +
                                '</tr>';
                            tableBody.append(row);
                        }

                        $('#documentApprovalModal').modal('show');
                    } else {
                        alert('No approval history found.');
                    }
                },
                error: function(xhr, status, error) {
                    // Handle error
                    alert('Error updating status: ' + xhr.responseText);
                }
            });
        }
    </script>
</body>


</html>
