<style>
    @media print {
        @page {
            size: A4;
            margin-top: 50px;
        }


        .page-break {
            page-break-before: always;
        }


        body {
            margin: 20px 50px;
            text-align: justify;
        }


        .btn {
            display: none;
        }
    }


    body {
        text-align: justify;
        margin: 12px 50px;
    }


    hr {
        border-top: 1px solid var(--lm-border);
    }


    .text_bold {
        font-weight: bold;
    }


    .abbrivation {
        font-size: 12px;
        vertical-align: top;
    }


    .heading {
        border-bottom: 1px solid var(--lm-border);
        display: inline-block;
    }


    .center {
        text-align: center;
    }


    .btn {
        background-color: blue;
        border: none;
        color: white;
        border-radius: 5px;
        padding: 10px 20px;
        font-size: 16px;
        font-weight: bold;
        cursor: pointer;
        float: right;
        margin-bottom: 8px;
    }
</style>


<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12">


                <?php
                $date = new DateTime($Indemnity_bond->date);
                $day = $date->format('d');
                $month = $date->format('M');
                $year = $date->format('Y');
                ?>
                @php
                $poaCodes = !empty($land_form->poa_lo_code)
                ? array_map('trim', explode(',', $land_form->poa_lo_code))
                : [];
                @endphp


                @foreach($land_owners as $key => $owner)


                <?php
                $landDetail = $land_form_details->get($key) ?? null;
                $purchaselandDetail = $purchase_land_row->get($key) ?? null;
                ?>


                <div class="card shadow-none border border-300 my-4">
                    <div class="card-body">


                        <div class="d-flex align-items-center">
                            <img src="{{ asset('public/assets/img/icons/logo.png'); }}">
                        </div>


                        <div class="center">
                            <h1 class="heading">INDEMNITY BOND</h1>
                        </div>


                        <!-- MAIN PARAGRAPH -->
                        <p>
                            <span class="text_bold">THIS DEED OF INDEMNITY</span> is made at {{ $record->district ?? 'Bahawalpur' }} on this
                            <span class="text_bold">{{ $day }} <span class="abbrivation">TH</span></span>
                            <span class="text_bold">day of {{ $month }} {{ $year }} by I,
                                @php
                                $isPoaOwner = in_array($owner->lo_cod, $poaCodes);
                                @endphp


                                @if($isPoaOwner)
                                {{-- OWNER INFO --}}
                                <span class="text_bold">{{$land_form->poa_name}}</span>
                                <span>{{$land_form->relationship ?? ''}}</span>
                                <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                                CNIC NO <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>,
                                Guardian/Power of Attorney Holder on behalf of
                                @if(!empty($owner->lo_name) || !empty($owner->so))
                                {{ $owner->lo_name }}
                                {{ $owner->relationship_revenue }}
                                {{ $owner->so }} as per Revenue Record,


                                {{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}
                                {{ $owner->relationship_cnic }}
                                {{ $owner->father_name_cnic ?? $owner->so }} as per CNIC,


                                @else


                                {{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}
                                {{ $owner->relationship_cnic }}
                                {{ $owner->father_name_cnic ?? $owner->so }},


                                @endif


                                (CNIC No. {{ $owner->lo_cnic }}),
                                Caste {{ $owner->caste }},
                                resident of {{ $owner->address }}</span>
                            @else
                            {{-- OWNER INFO --}}


                            {{-- OWNER INFO --}}
                            @if(!empty($owner->lo_name) || !empty($owner->so))
                            {{ $owner->lo_name }}
                            {{ $owner->relationship_revenue }}
                            {{ $owner->so }} as per Revenue Record,


                            {{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}
                            {{ $owner->relationship_cnic }}
                            {{ $owner->father_name_cnic ?? $owner->so }} as per CNIC,


                            @else


                            {{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}
                            {{ $owner->relationship_cnic }}
                            {{ $owner->father_name_cnic ?? $owner->so }},


                            @endif


                            (CNIC No. {{ $owner->lo_cnic }}),
                            Caste {{ $owner->caste }},
                            resident of {{ $owner->address }}</span>
                            @endif


                            {{-- LAND DETAIL --}}
                            @if($landDetail)
                            ,<span class="text_bold" title="land details from land offer form"> Khewat No. {{ $landDetail->khewat_no }},
                                Khatooni No. {{ $landDetail->khatooni_no }},
                                Qatat {{ $landDetail->qatat }},
                                measuring
                                {{ $landDetail->measuring_k }} Kanal,
                                {{ $landDetail->measuring_m }} Marlas,
                                {{ $landDetail->measuring_sqft }} Sqft,
                                transferred share {{ $landDetail->transfer_share }},
                                Land Measuring
                                {{ $landDetail->land_measuring_k }} Kanal,
                                {{ $landDetail->land_measuring_m }} Marlas,
                                {{ $landDetail->land_measuring_sqft }} Sqft
                            </span>
                            @endif


                            , hereinafter referred to as executant which expression shall include where the context so permits his legal heirs, successors in interest and assignee(s).
                        </p>


                        <!-- WHEREAS -->
                        <p>
                            <span class="text_bold">WHEREAS</span> the executant has executed sale deed dated
                            <span class="text_bold">{{ $day }} {{ $month }} {{ $year }}</span>
                            regarding the land:
                            @if($purchaselandDetail)


                            <span title="land details from purchase of land form" class="text_bold">
                                Khewat No. {{ $purchaselandDetail->khewat_no ?? '' }},
                                Khatooni No. {{ $purchaselandDetail->khatooni_no ?? '' }},
                                Qatat {{ $purchaselandDetail->qatat ?? '' }},
                                measuring ({{ $purchaselandDetail->measuring_k ?? '0' }} Kanal,
                                {{ $purchaselandDetail->measuring_m ?? '0' }} Marlas,
                                {{ $purchaselandDetail->measuring_sqft ?? '0' }} Sqft), transferred share {{ $purchaselandDetail->transfer_share ?? '0' }}, Land Measuring
                                {{ $purchaselandDetail->land_measuring_k ?? '0' }} Kanal,
                                {{ $purchaselandDetail->land_measuring_m ?? '0' }} Marlas,
                                {{ $purchaselandDetail->land_measuring_sqft ?? '0' }} Sqft <span class="text_bold" style="font-size:20px;">Total Land Measuring {{ $purchaselandDetail->land_measuring_k ?? '0' }} Kanal,
                                    {{ $purchaselandDetail->land_measuring_m ?? '0' }} Marlas,
                                    {{ $purchaselandDetail->land_measuring_sqft ?? '0' }} Sqft</span>


                            </span>
                            @endif


                            <span class="text_bold">as per Aks Shajrah verified by the Revenue Patwari circle Record of Rights for the Year {{ $record->record_of_rights_year }},
                                Vide Fard ID No.
                                {{ $purchase_doc->fard_id ?? 'N/A' }}
                                dated {{ $purchase_doc->fard_date ? \Carbon\Carbon::parse($purchase_doc->fard_date)->format('d M Y') : 'N/A' }}


                                @if(!empty($purchase_doc->fard_id2) && !empty($purchase_doc->fard_date2))
                                and Fard ID 2 No.
                                {{ $purchase_doc->fard_id2 }}
                                dated {{ $purchase_doc->fard_date2 ? \Carbon\Carbon::parse($purchase_doc->fard_date2)->format('d M Y') : 'N/A' }}
                                @endif , situated in {{ $purchase_doc->mouza ?? 'N/A' }} Tehsil {{ $record->tehsil ?? 'Bahawalpur' }}
                                District {{ $record->district ?? 'Bahawalpur' }},</span> vide Mutation no. __________ dated ________, in favour of {{ config('app.org_name') }}, having its principal office at <span class="text_bold">{{ config('app.org_name') }}, Main Office Complex, Sector A Commercial, Phase-VI Lahore,</span> and a project office namely {{ config('app.org_name') }}, Head Office Jinnah Avenue (MB-2) APE Canal Road Bahawalpur.
                        </p>


                        <!-- {{ config('app.org_short') }} -->
                        <p>
                            <span class="text_bold">AND WHEREAS</span>
                            the <span class="text_bold">{{ config('app.org_name') }},</span> having project office namely {{ config('app.org_name') }}, Head Office Jinnah Avenue (MB-2) APE Canal Road Bahawalpur has desired to keep it indemnified against all losses, claims, damages and charges of whatsoever nature regarding the above mentioned land, therefore, the executant is executing this Indemnity Bond for getting the Authority secured against all losses, claims, damages and charges etc of whatsoever nature from any person or party including the land owner.
                        </p>


                        <!-- EXECUTANT -->
                        <div class="center">
                            <h1 class="heading">EXECUTANT</h1>
                            <p>________________________________________</p>
                            @php
                            $isPoaOwner = in_array($owner->lo_cod, $poaCodes);
                            @endphp


                            @if($isPoaOwner)
                            <span class="text_bold">{{$land_form->poa_name}}</span>
                            <span>{{$land_form->relationship ?? ''}}</span>
                            <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                            CNIC NO: <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>,
                            Caste <span class="text_bold">{{$land_form->poa_caste ?? ''}}</span>,
                            resident of <span class="text_bold">{{$land_form->poa_permanent_address ?? ''}}</span>,
                            @else


                            <p>
                                <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? '' }}
                                    {{ $owner->relationship_cnic }}
                                    {{ $owner->father_name_cnic ?? '' }},
                                    (CNIC No. {{ $owner->lo_cnic ?? '' }}),
                                    Caste {{ $owner->caste ?? '' }},
                                    resident of {{ $owner->address ?? '' }}</span>
                            </p>
                            @endif
                        </div>


                        @if(!$loop->last)
                        <div class="page-break"></div>
                        @endif


                    </div>
                </div>


                @endforeach


                <button class="btn" onclick="window.print()">Print</button>


            </div>
        </div>
    </div>
</div>


<script>
    window.onload = function() {
        window.print();
    }
</script>

