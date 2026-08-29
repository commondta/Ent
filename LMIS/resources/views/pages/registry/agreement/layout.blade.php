<style>
    @media print {
        @page {
            size: A4;
            margin-top: 50px;
            /* Adjust the top margin as needed */
        }

        .page-break {
            page-break-before: always;
        }

        body {
            margin: 20px 70px;
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
        margin-top: 60px;
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

    .text_indent {
        text-indent: 30px;
    }

    .li_align {
        padding-left: 17px;
    }

    .flex_display {
        display: flex;
        text-align: justify;
    }

    .left_flex {
        width: 50%;
        margin-right: 100px;


    }

    .right_flex {
        width: 50%;
        margin-left: 100px;
    }

    .btn {
        background-color: blue;
        border: none;
        color: white;
        border-radius: 5px;
        padding: 10px 20px;
        ;
        font-size: 16px;
        font-weight: bold;
        cursor: pointer;
        float: right;
        margin-bottom: 8px;
    }

    .no-list-style {
        list-style: none;
        padding-left: 0;
    }
</style>


<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="mb-9">
                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-body">
                            <div class="d-flex align-items-center"><img
                                    src="{{ asset('public/assets/img/icons/logo.png'); }}"
                                    alt="phoenix"
                                    width="200" />

                                <div class="center">
                                    <h1 class="heading">AGREEMENT</h1>
                                </div>
                                <?php
                                // Parse agreement date
                                $dateOfCreation = $agreement->agreement_date ?? $agreement->date;

                                // Create a DateTime object
                                $date = new DateTime($dateOfCreation);

                                // Format the date to get only the day
                                $day = $date->format('d');
                                $month = $date->format('M');
                                $year = $date->format('Y');

                                ?>
                                <?php
                                function daySuffix($day)
                                {
                                    if (in_array($day % 100, [11, 12, 13])) {
                                        return 'TH';
                                    }

                                    return match ($day % 10) {
                                        1 => 'ST',
                                        2 => 'ND',
                                        3 => 'RD',
                                        default => 'TH',
                                    };
                                }

                                $suffix = daySuffix((int) $day);
                                ?>

                                <p class="text_indent">This agreement is made and Executed at {{ $record->district ?? 'Bahawalpur' }} <span class="text_bold">{{ $day }}
                                        <span class="abbrivation">{{ $day > 20 ? 'TH' : ($day % 10 == 1 ? 'ST' : ($day % 10 == 2 ? 'ND' : ($day % 10 == 3 ? 'RD' : 'TH'))) }}</span></span><span class="text_bold">
                                        day of <span>{{ $month }}</span> <span>{{ $year }}</span></span></p>

                                <div class="center">
                                    <h2 class="heading">BETWEEN</h2>
                                </div>
                               @php
                                $poaCodes = !empty($land_form->poa_lo_code)
                                ? array_map('trim', explode(',', $land_form->poa_lo_code))
                                : [];
                                @endphp

                                @if(!empty($land_form->poa_name))
                                <p>

                                    @foreach($land_owners as $key => $owner)

                                    @php
                                    $isPOAOwner = in_array($owner->lo_cod, $poaCodes);
                                    @endphp

                                    {{-- POA BLOCK (ONLY SELECTED OWNERS) --}}
                                    @if($isPOAOwner)
                                    <span class="text_bold">{{$land_form->poa_name}}</span>
                                    <span>{{$land_form->relationship ?? ''}}</span>
                                    <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                                    <span class="text_bold">CNIC No. {{$land_form->poa_cnic ?? ''}}</span>,
                                    Caste <span class="text_bold">{{$land_form->poa_caste ?? ''}}</span>,
                                    resident of <span class="text_bold">{{$land_form->poa_permanent_address ?? ''}}</span>,
                                    <span class="text_bold">{{$land_form->poa_remarks ?? ''}}</span>
                                    <!-- Guardian/Power of Attorney Holder on behalf of -->
                                    @endif

                                    {{-- OWNER DETAILS --}}
                                    @if(!empty($owner->lo_name) || !empty($owner->so))

                                    <span class="text_bold">{{ $owner->lo_name }}</span>
                                    @if($owner->relationship_revenue)
                                    {{ $owner->relationship_revenue }}
                                    @endif
                                    <span class="text_bold">{{ $owner->so }}</span> as per Revenue Record,

                                    <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                    @if($owner->relationship_cnic)
                                    {{ $owner->relationship_cnic }}
                                    @endif
                                    <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>
                                    as per CNIC Record,
                                    (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                    Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                    resident of <span class="text_bold">{{ $owner->address }}</span>

                                    @else

                                    <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                    @if($owner->relationship_cnic)
                                    {{ $owner->relationship_cnic }}
                                    @endif
                                    <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                    (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                    Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                    resident of <span class="text_bold">{{ $owner->address }}</span>

                                    @endif

                                    {{-- LAND DETAILS --}}
                                    @if($land_form_details && $land_form_details->count() > $key)
                                    @php
                                    $details = $land_form_details->where('lo_cod', $owner->lo_cod);
                                    @endphp

                                    @foreach($details as $landDetail)
                                    , Khewat No. <span class="text_bold">{{ $landDetail->khewat_no ?? 'N/A' }}</span>,
                                    Khatooni No. <span class="text_bold">{{ $landDetail->khatooni_no ?? 'N/A' }}</span>,
                                    Qatat <span class="text_bold">{{ $landDetail->qatat ?? 'N/A' }}</span>,
                                    measuring <span class="text_bold">
                                        {{ $landDetail->measuring_k ?? '0' }} Kanal,
                                        {{ $landDetail->measuring_m ?? '0' }} Marlas and
                                        {{ $landDetail->measuring_sqft ?? '0' }} Sqft
                                    </span>,
                                    transferred share <span class="text_bold">{{ $landDetail->transfer_share ?? 'N/A' }}</span>,
                                    Land Measuring <span class="text_bold">
                                        {{ $landDetail->land_measuring_k ?? '0' }} Kanal,
                                        {{ $landDetail->land_measuring_m ?? '0' }} Marlas and
                                        {{ $landDetail->land_measuring_sqft ?? '0' }} Sqft
                                    </span>
                                    @endforeach
                                    @endif

                                    @if(!$loop->last),@endif

                                    @endforeach

                                    , (hereinafter called the VENDORS, which expression includes the heirs, successors & Assigns) of the one part.

                                </p>

                                @else

                                <p>

                                    @if($land_owners && $land_owners->count() > 0)
                                    I/We,

                                    @foreach($land_owners as $key => $owner)

                                    @if(!empty($owner->lo_name) || !empty($owner->so))

                                    <span class="text_bold">{{ $owner->lo_name }}</span>
                                    @if($owner->relationship_revenue)
                                    {{ $owner->relationship_revenue }}
                                    @endif
                                    <span class="text_bold">{{ $owner->so }}</span> as per Revenue Record,

                                    <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                    @if($owner->relationship_cnic)
                                    {{ $owner->relationship_cnic }}
                                    @endif
                                    <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>  as per CNIC Record,
                                    (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                    Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                    resident of <span class="text_bold">{{ $owner->address }}</span>

                                    @else

                                    <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                    @if($owner->relationship_cnic)
                                    {{ $owner->relationship_cnic }}
                                    @endif
                                    <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                    (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                    Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                    resident of <span class="text_bold">{{ $owner->address }}</span>

                                    @endif
                                     {{-- LAND DETAILS --}}
                                    @if($land_form_details && $land_form_details->count() > $key)
                                    @php
                                    $details = $land_form_details->where('lo_cod', $owner->lo_cod);
                                    @endphp

                                    @foreach($details as $landDetail)
                                    , Khewat No. <span class="text_bold">{{ $landDetail->khewat_no ?? 'N/A' }}</span>,
                                    Khatooni No. <span class="text_bold">{{ $landDetail->khatooni_no ?? 'N/A' }}</span>,
                                    Qatat <span class="text_bold">{{ $landDetail->qatat ?? 'N/A' }}</span>,
                                    measuring <span class="text_bold">
                                        {{ $landDetail->measuring_k ?? '0' }} Kanal,
                                        {{ $landDetail->measuring_m ?? '0' }} Marlas and
                                        {{ $landDetail->measuring_sqft ?? '0' }} Sqft
                                    </span>,
                                    transferred share <span class="text_bold">{{ $landDetail->transfer_share ?? 'N/A' }}</span>,
                                    Land Measuring <span class="text_bold">
                                        {{ $landDetail->land_measuring_k ?? '0' }} Kanal,
                                        {{ $landDetail->land_measuring_m ?? '0' }} Marlas and
                                        {{ $landDetail->land_measuring_sqft ?? '0' }} Sqft
                                    </span>
                                    @endforeach
                                    @endif


                                    @if(!$loop->last),@endif

                                    @endforeach

                                    , (hereinafter called the VENDORS, which expression includes the heirs, successors & Assigns) of the one part.

                                    @endif

                                </p>

                                @endif
                                <div class="center">
                                    <h2 class="heading">AND</h2>
                                </div>
                                <p class="text_indent">
                                    <span class="text_bold">{{ config('app.org_legal') }}</span>, through <span class="text_bold">{{ $record->deed_in_favor_of_name }}</span>, <span class="text_bold">(Project Secretary) called Vendee,</span> which expression shall includes where the context so permits its successors in interest, representatives and assignee(s).
                                </p>
                                <?php
                                function convertToKanalsMarlasAndSquareFeet($areaInSquareFeet)
                                {
                                    //  Force numeric value
                                    $areaInSquareFeet = (float) $areaInSquareFeet;

                                    // Conversion factors
                                    $squareFeetPerMarla = 272;        // 1 Marla = 272 Sqft
                                    $marlasPerKanal = 20;             // 1 Kanal = 20 Marlas
                                    $squareFeetPerKanal = $squareFeetPerMarla * $marlasPerKanal; // 5440 Sqft

                                    // Full kanals (integer)
                                    $kanals = floor($areaInSquareFeet / $squareFeetPerKanal);

                                    // Remaining sqft after kanals
                                    $remainingSquareFeetAfterKanals =
                                        $areaInSquareFeet - ($kanals * $squareFeetPerKanal);

                                    // Marlas
                                    $marlas = floor($remainingSquareFeetAfterKanals / $squareFeetPerMarla);

                                    // Remaining sqft
                                    $remainingSquareFeet =
                                        round($remainingSquareFeetAfterKanals - ($marlas * $squareFeetPerMarla), 2);

                                    return [
                                        'kanals' => $kanals,
                                        'marlas' => $marlas,
                                        'square_feet' => $remainingSquareFeet
                                    ];
                                }


                                // Example usage
                                $area = $record->area; // area in square feet
                                $result = convertToKanalsMarlasAndSquareFeet($area);

                                // Access results
                                // echo $result['kanals'], $result['marlas'], $result['square_feet'];
                                ?>



                                <?php
                                // Parse Fard date
                                $fardDate = null;
                                if (isset($Conveyance_land_fard_row) && $Conveyance_land_fard_row) {
                                    $fardDate = $Conveyance_land_fard_row->date ?? $record->agreement_date ?? $record->date;
                                } else {
                                    $fardDate = $record->agreement_date ?? $record->date;
                                }

                                // Create a DateTime object
                                $fdate = new DateTime($fardDate);

                                // Format the date to get only the day
                                $fday = $fdate->format('d');
                                $fmonth = $fdate->format('M');

                                ?>

                                <p class="text_indent">
                                    <span class="text_bold">WHEREAS</span> the Vendor is the lawful owner of land
                                    measuring <span> @foreach ($purchase_land_row as $land_row)
                                        <span title="land details from purchase of land form" class="text_bold">
                                            Khewat No. <span>{{ $land_row['khewat_no'] ?? 'N/A' }}</span>,
                                            Khatooni No. <span>{{ $land_row['khatooni_no'] ?? 'N/A' }}</span>,
                                            Qatat <span>{{ $land_row['qatat'] ?? 'N/A' }}</span>,
                                            measuring (
                                            <span>{{ $land_row['measuring_k'] ?? '0' }}</span> Kanal,
                                            <span>{{ $land_row['measuring_m'] ?? '0' }}</span> Marlas and
                                            <span>{{ $land_row['measuring_sqft'] ?? '0' }}</span> Sqft
                                            ),
                                            transferred share <span>{{ $land_row['transfer_share'] ?? 'N/A' }}</span>,
                                            Land measuring (
                                            <span>{{ $land_row['land_measuring_k'] ?? '0' }}</span> Kanal,
                                            <span>{{ $land_row['land_measuring_m'] ?? '0' }}</span> Marlas and
                                            <span>{{ $land_row['land_measuring_sqft'] ?? '0' }}</span> Sqft
                                            )
                                        </span>,
                                        @endforeach

                                        <span class="text_bold" style="font-size:20px;">
                                            Total land measuring (
                                            <span>{{ $purchase_doc->total_kanal ?? '0' }}</span> Kanal,
                                            <span>{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and
                                            <span>{{ $purchase_doc->total_sqft ?? '0' }}</span> Sqft
                                            )
                                        </span>,

                                        <span class="text_bold">as per Aks Shajrah verified by the Revenue Patwari circle Record of Rights
                                            for the Year {{ $record->record_of_rights_year ?? 'N/A' }},

                                            Vide Fard ID No.
                                            <span>{{ $purchase_doc->fard_id ?? 'N/A' }}</span>
                                            dated {{ $purchase_doc->fard_date ? \Carbon\Carbon::parse($purchase_doc->fard_date)->format('d M Y') : 'N/A' }}

                                            @if(!empty($purchase_doc->fard_id2) && !empty($purchase_doc->fard_date2))
                                            and Fard ID 2 No.
                                            <span>{{ $purchase_doc->fard_id2 }}</span>
                                            dated {{ $purchase_doc->fard_date2 ? \Carbon\Carbon::parse($purchase_doc->fard_date2)->format('d M Y') : 'N/A' }}
                                            @endif,

                                            situated in {{ $purchase_doc->mouza ?? 'N/A' }}
                                            Tehsil {{ $record->tehsil ?? 'Bahawalpur' }}
                                            District {{ $record->district ?? 'Bahawalpur' }}</span>, said land of Vendor falls in Housing Scheme of
                                        vendee.
                                </p>

                                <p class="text_indent">
                                    <span class="text_bold">AND WHEREAS</span> Vendor is owner in possession, free from all encumbrances and charges, by (Sale Deed or mutation of inheritance whatsoever etc) and has the right to sell, transfer, lien etc the land hereinafter described and has shown his desire and has agreed to sell the said land measuring <span class="text_bold" style="font-size:20px;">
                                        Total land measuring (
                                        <span>{{ $purchase_doc->total_kanal ?? '0' }}</span> Kanal,
                                        <span>{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and
                                        <span>{{ $purchase_doc->total_sqft ?? '0' }}</span> Sqft
                                        )
                                    </span>, as shown in Fard Malkiyat (attached as Annex A) provided by the Vendor against ______ residential plot Files of 1 Kanal (500 Sq Yds) each as exemption. After exemption balance held with {{ config('app.org_short') }} can be utilized for Vendor, by selling the right to any other person, party or getting as an additional plot by completing _______, in {{ config('app.org_short') }} by Vendor.
                                </p>

                                <p class="text_indent">
                                    <span class="text_bold">AND WHEREAS</span> Vendee has agreed to purchase <span class="text_bold">(
                                        <span>{{ $purchase_doc->total_kanal ?? '0' }}</span> Kanal,
                                        <span>{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and
                                        <span>{{ $purchase_doc->total_sqft ?? '0' }}</span> Sqft
                                        )</span>
                                    </span>, land and is already in possession of said land.
                                </p>

                                <p class="text_indent text_bold">THEREFORE, THIS AGREEMENT WITNESSETH AS UNDER:-</p>

                                <ol class="li_align">
                                    <li class="text_indent">
                                        That vendee will allocate / allot plots to the Vendor from {{ config('app.org_short') }} as per the town planning of {{ config('app.org_short') }} against said land as per agreed terms and conditions, i.e _______ residential plot files of 1 Kanal (500 Sq yds) each against
                                        <span class="text_bold"><span class="text_bold">{{ $purchase_doc->total_kanal ?? '0' }}</span> Kanal,
                                            <span class="text_bold">{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and
                                            <span class="text_bold">{{ $purchase_doc->total_sqft ?? '0' }}</span> Sqft,</span> of land subject to provision of documents / handing over possession by Vendor, registration and mutation of land in favour of Vendee. Vendor will pay Rs.6500/- as miscellaneous charges per plot file given against land. The development charges will be paid on demand by Vendor to {{ config('app.org_short') }} according to the actual cost of development in said Housing Scheme or as per the town planning of {{ config('app.org_short') }}.
                                    </li>
                                    <li class="text_indent">
                                        That vendor shall be bound and undertakes to execute and complete the registered sale deed for the transfer of ownership of the land as and when required by Vendee (Allocation letter against <span class="text_bold"><span class="text_bold">{{ $purchase_doc->total_kanal ?? '0' }}</span> Kanal,
                                            <span class="text_bold">{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and
                                            <span class="text_bold">{{ $purchase_doc->total_sqft ?? '0' }}</span> Sqft,</span> i.e ______ residential plot files of 1 Kanal (500 Sq Yds) will be issued by Vendee after registration, possession and mutation of land in favour of Vendee). In case, Vendor does not execute the registered sale deed under the term of this agreement, vendee shall be entitled to get the sale deed registered before the Sub-registrar in accordance with law and at the expense of Vendee.
                                    </li>
                                    <li class="text_indent">
                                        That vendor confirms that he will provide documents in light of {{ config('app.org_label') }} policy (which has been explained to Vendor) and in support of their entitlement of the land to enable Vendor to get the sale deed registered. Vendor will also ensure that whenever required, signatures of the owner / tenants would be obtained on various documents.
                                    </li>
                                    <li class="text_indent">
                                        That if any person claims the title of the sold land at any stage or challenge
                                        possession, vendor will be
                                        held responsible for clearance of the same at their own risk and cost and vendor
                                        shall keep the Vendee
                                        harmless and indemnified. Vendee has the right to roll back the complete
                                        transaction, at any time in future
                                        and all the costs in this connection shall be borne by the vendor if the said
                                        dispute is not resolved and
                                        circumstances go beyond the control of vendor and there is apprehension of
                                        unending litigation.
                                    </li>
                                    <li class="text_indent">
                                        That if any dispute arises between parties regarding any of provision of this
                                        agreement, the matter will be
                                        referred to Chairman Project Management Committee {{ $record->b_project_office ?? config('app.org_short') }}
                                        who shall be the sole
                                        Arbitrator. Decision of Chairman / Arbitrator will be final and binding on both
                                        parties.
                                    </li>
                                    <li class="text_indent">
                                        That vendor has assured Vendee of his undisputed title over the said land and
                                        has also assured to indemnify
                                        and keep indemnifying Vendee of any loss occasioned on account of any defect in
                                        the title of Vendor or
                                        because of any lien, charge or encumbrance of any nature upon the said land or
                                        any part thereof, which may
                                        be discovered at any time.
                                    </li>
                                    <li class="text_indent">
                                        That residential plots against above mentioned land would be allotted to owner
                                        of land in question or
                                        nominees / representatives to be nominated by Vendor, through an affidavit.
                                    </li>
                                    <li class="text_indent">
                                        That vendor / its representatives / legal heirs or any person claiming land of
                                        Vendee or otherwise etc would
                                        be debarred for all times to come to lay any claim with regard to said land. For
                                        all particular purposes and
                                        in future the land shall be deemed to vest in and owned by Vendee/ or its
                                        transferees.
                                    </li>
                                    <li class="text_indent">
                                        That vendor has surrendered his rights regarding Shamlat Deh, School, Graveyard,
                                        Watt, Khal, orchards,
                                        crops, fittings, fixture and all other easement rights to vendee.
                                    </li>
                                </ol>
                                <p>
                                    <span class="text_bold">IN WITNESS WHEREOF</span>, the parties herein, have set
                                    their respective hands, in their complete senses, executed and
                                    signed this agreement at the place and date mentioned herein above and in the
                                    presence of witnesses named
                                    below:-
                                </p>

                                <div class="flex_display">
                                    @if(!empty($land_form->poa_name))
                                    <div class="left_flex">
                                        <div class="center">
                                            <h5 class="heading">VENDORS</h5>
                                            <hr>
                                        </div>

                                        @if($land_owners && $land_owners->count() > 0)
                                        @php
                                        $poaCodes = !empty($land_form->poa_lo_code)
                                        ? array_map('trim', explode(',', $land_form->poa_lo_code))
                                        : [];
                                        @endphp

                                        @foreach($land_owners as $key => $owner)
                                        @php
                                        $isPOAOwner = in_array($owner->lo_cod, $poaCodes);
                                        @endphp

                                        @if($isPOAOwner)
                                        <span class="text_bold">{{$land_form->poa_name}}</span>
                                        <span>{{$land_form->relationship ?? ''}}</span>
                                        <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                                        CNIC NO <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>,
                                        Caste <span class="text_bold">{{$land_form->poa_caste ?? ''}}</span>,
                                        resident of <span class="text_bold">{{$land_form->poa_permanent_address ?? ''}}</span>,
                                        Guardian/Power of Attorney Holder on behalf of
                                        @endif

                                        {{-- IF Revenue Data Exists --}}
                                        @if(!empty($owner->lo_name) || !empty($owner->so))

                                        <span class="text_bold">{{ $owner->lo_name }}</span>
                                        @if($owner->relationship_revenue)
                                        {{ $owner->relationship_revenue }}
                                        @endif
                                        <span class="text_bold">{{ $owner->so }}</span>, as per Revenue Record,

                                        <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                        @if($owner->relationship_cnic)
                                        {{ $owner->relationship_cnic }}
                                        @endif
                                        <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                        as per CNIC,
                                        (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                        Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                        resident of <span class="text_bold">{{ $owner->address }}</span><br><span style="text-align: center; margin-left: 150px" class="text_bold">(Vendor)</span>

                                        @else

                                        {{-- ONLY CNIC --}}
                                        <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                        @if($owner->relationship_cnic)
                                        {{ $owner->relationship_cnic }}
                                        @endif
                                        <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                        (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                        Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                        resident of <span class="text_bold">{{ $owner->address }}</span><br><span style="text-align: center; margin-left: 150px" class="text_bold">(Vendor)</span>

                                        @endif

                                        {{-- COMMA HANDLING --}}
                                        @if(!$loop->last)
                                        <hr>
                                        @endif

                                        @endforeach

                                        @endif


                                    </div>
                                    @else
                                    <div class="left_flex">
                                        <div class="center">
                                            <h5 class="heading">VENDORS</h5>
                                            <hr>
                                        </div>

                                        @if($land_owners && $land_owners->count() > 0)

                                        @foreach($land_owners as $key => $owner)

                                        {{-- IF Revenue Data Exists --}}
                                        @if(!empty($owner->lo_name) || !empty($owner->so))

                                        <span class="text_bold">{{ $owner->lo_name }}</span>
                                        @if($owner->relationship_revenue)
                                        {{ $owner->relationship_revenue }}
                                        @endif
                                        <span class="text_bold">{{ $owner->so }}</span>, as per Revenue Record,

                                        <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                        @if($owner->relationship_cnic)
                                        {{ $owner->relationship_cnic }}
                                        @endif
                                        <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                        as per CNIC,
                                        (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                        Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                        resident of <span class="text_bold">{{ $owner->address }}</span><br><span style="text-align: center; margin-left: 150px" class="text_bold">(Vendor)</span>

                                        @else

                                        {{-- ONLY CNIC --}}
                                        <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? $owner->lo_name }}</span>
                                        @if($owner->relationship_cnic)
                                        {{ $owner->relationship_cnic }}
                                        @endif
                                        <span class="text_bold">{{ $owner->father_name_cnic ?? $owner->so }}</span>,
                                        (CNIC No. <span class="text_bold">{{ $owner->lo_cnic }}</span>),
                                        Caste <span class="text_bold">{{ $owner->caste }}</span>,
                                        resident of <span class="text_bold">{{ $owner->address }}</span><br><span style="text-align: center; margin-left: 150px" class="text_bold">(Vendor)</span>

                                        @endif

                                        {{-- COMMA HANDLING --}}
                                        @if(!$loop->last)
                                        <hr>
                                        @endif

                                        @endforeach

                                        @endif


                                    </div>
                                    @endif
                                    <div class="right_flex">
                                        <div class="center">
                                            <h5 class="heading">VENDEE</h5>
                                        </div>
                                        <p>



                                            <hr>
                                            {{ $record->deed_in_favor_of_name }} <br>
                                            CNIC No. ({{ $record->rep_cnic }})
                                            For and on behalf of
                                            <span class="text_bold">{{ config('app.org_name') }} (vendee)</span>
                                        </p>
                                    </div>
                                </div>
                                <div>
                                    @if($agreement->is_land_provider)
                                    <h4 class="heading">INVESTOR / LAND PROVIDER</h4>

                                    <p><span class="text_bold">{{$land_p->lp_name ?? 'N/A' }} {{ $land_p->relationship ?? 'S/O' }} {{ $land_p->father_name ?? 'N/A' }} </span></p>
                                    <p><span class="text_bold">CNIC No. {{ $land_p->lp_cnic ?? 'N/A' }}</span></p>
                                    @endif



                                </div>

                                <div>
                                    <div class="center">
                                        <h4 class="heading">WITNESSES</h4>
                                    </div>
                                    <ol class="flex_display li_align no-list-style">
                                        <li class="left_flex">
                                            <span class="text_bold">

                                                {{ $agreement->witness1_rank ?? 'N/A' }}<br>
                                                {{ $agreement->witness1_appointment ?? 'N/A' }}<br>
                                                {{ $agreement->witness1_name ?? 'N/A' }}
                                            </span>
                                        </li>
                                        <li class="right_flex">
                                            <span class="text_bold">


                                                {{ $agreement->witness2_rank ?? 'N/A' }}<br>
                                                {{ $agreement->witness2_appointment ?? 'N/A' }}<br>
                                                {{ $agreement->witness2_name ?? 'N/A' }}
                                            </span>
                                        </li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                        <button class="btn" onclick="window.print()">Print</button>

                    </div>
                </div>
            </div>
        </div>

    </div>
</div>

<script>
    // Optional auto-print - uncomment below to auto-print on load
    // window.onload = function() {
    //     window.print();
    // }

    // Or user can click Print button manually
</script>