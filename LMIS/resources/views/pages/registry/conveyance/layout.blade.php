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

    .li_align {
        padding-left: 22px;
    }

    .flex_display {
        display: flex;
        text-align: justify;
    }

    .left_flex {
        width: 50%;
        margin-right: 30px;
    }

    .right_flex {
        width: 50%;
        margin-left: 30px;
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
</style>

<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                    <div class="card-body">
                        <div class="d-flex align-items-center"><img
                                src="{{ asset('public/assets/img/icons/logo.png'); }}"
                                alt="phoenix"
                                width="200" />

                            <div class="center">
                                <h1 class="heading">CONVEYANCE DEED</h1>
                            </div>
                            <div>
                                <?php
                                // Parse Conveyance record date
                                $dateOfCreation = $record->date_of_creation ?? $record->date;
                                $date = new DateTime($dateOfCreation);
                                $day = $date->format('d');
                                $month = $date->format('M');
                                $year = $date->format('Y');

                                // Parse Fard date
                                $fardDate = $fard_row->date ?? $record->date;
                                $fdate = new DateTime($fardDate);
                                $fday = $fdate->format('d');
                                $fmonth = $fdate->format('M');
                                $fyear = $fdate->format('Y');
                                ?>

                                <p>This Deed made at {{ $record->district ?? 'Bahawalpur' }} on the <span class="text_bold">{{ $day }}
                                        <span class="abbrivation">{{ $day > 20 ? 'TH' : ($day % 10 == 1 ? 'ST' : ($day % 10 == 2 ? 'ND' : ($day % 10 == 3 ? 'RD' : 'TH'))) }}</span></span><span class="text_bold">
                                        day of <span>{{ $month }}</span> <span>{{ $year }}</span></span> for conveyance of
                                    <?php foreach ($purchase_land_row as $key => $land_row) { ?>
                                        <span title="land details from purchase of land form" class="text_bold">Khewat No.
                                            <span>{{ $land_row['khewat_no'] ?? 'N/A' }}</span>, Khatooni No. <span>{{ $land_row['khatooni_no'] ?? 'N/A' }}</span>, Qatat <span>{{ $land_row['qatat'] ?? 'N/A' }}</span>, measuring (<span>{{ ($land_row->measuring_k ?? '0') }} </span> Kanal,
                                            <span>{{ $land_row['measuring_m'] ?? '0' }}</span> Marlas and <span>{{$land_row->measuring_sqft ?? '0' }}</span> Sqft),
                                            transferred share <span>{{ $land_row['transfer_share'] ?? 'N/A' }}</span>,
                                            Land measuring (<span>{{ ($land_row->land_measuring_k ?? '0') }} </span> Kanal,
                                            <span>{{ $land_row['land_measuring_m'] ?? '0' }}</span> Marlas and <span>{{$land_row->land_measuring_sqft ?? '0' }}</span> Sqft),

                                        <?php } ?>
                                        <span style="font-size: 20px;"> Total land measuring (<span>{{ $purchase_doc->total_kanal ?? '0' }} </span> Kanal,
                                            <span>{{ $purchase_doc->total_marla ?? '0' }}</span> Marlas and <span>{{$purchase_doc->total_sqft ?? 'N/A' }}</span> Sqft)</span>, as per Aks Shajrah
                                        verified by the Revenue Patwari circle Record of Rights for the Year {{ $record->record_of_rights_year ?? 'N/A' }}, Vide Fard ID No.
                                        <span>{{ $purchase_doc->fard_id ?? 'N/A' }}</span>
                                        dated {{ $purchase_doc->fard_date ? \Carbon\Carbon::parse($purchase_doc->fard_date)->format('d M Y') : 'N/A' }}

                                        @if(!empty($purchase_doc->fard_id2) && !empty($purchase_doc->fard_date2))
                                        and Fard ID 2 No.
                                        <span>{{ $purchase_doc->fard_id2 }}</span>
                                        dated {{ $purchase_doc->fard_date2 ? \Carbon\Carbon::parse($purchase_doc->fard_date2)->format('d M Y') : 'N/A' }}
                                        @endif , situated in {{ $purchase_doc->mouza ?? 'N/A' }} Tehsil {{ $record->tehsil ?? 'Bahawalpur' }}
                                        District {{ $record->district ?? 'Bahawalpur' }}</span>, hereinafter referred to as the "Deed Land" which is bounded as follows:
                                </p>

                                <?php foreach ($block as $key => $lr) { ?>

                                    <p class="text_bold">
                                        Block No. <span>{{ $lr['block_no'] ?? 'N/A' }}</span>, Rectangle No. <span>{{ $lr['rectangle_no'] ?? 'N/A' }}</span>, Khasra No. <span>{{ $lr['khasra_no'] ?? 'N/A' }}</span>
                                    </p>

                                    <div style="margin-left: 100px;" class="text_bold">
                                        <p>EAST BY&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>{{ $lr['east_by'] ?? 'N/A' }}</span>
                                        </p>

                                        <p>WEST BY&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>{{ $lr['west_by'] ?? 'N/A' }}</span></p>

                                        <p>NORTH BY&nbsp;&nbsp;&nbsp; :
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>{{ $lr['north_by'] ?? 'N/A' }}</span></p>

                                        <p>SOUTH BY&nbsp;&nbsp;&nbsp;&nbsp;: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>{{ $lr['south_by'] ?? 'N/A' }}</span></p>
                                    </div>
                                <?php } ?>
                                <?php
                                function convertNumberToWord($num = false)
                                {
                                    $num = str_replace(array(',', ' '), '', trim($num));
                                    if (!$num) {
                                        return false;
                                    }

                                    $decimalSeparator = '.';
                                    $decimalInWords = '';

                                    if (strpos($num, $decimalSeparator) !== false) {
                                        list($num, $decimal) = explode($decimalSeparator, $num);
                                        $decimalInWords = ' point ' . convertNumberToWord($decimal);
                                    }

                                    $num = (int)$num;

                                    $words = array();
                                    $list1 = array(
                                        '',
                                        'One',
                                        'Two',
                                        'Three',
                                        'Four',
                                        'five',
                                        'six',
                                        'seven',
                                        'eight',
                                        'nine',
                                        'ten',
                                        'eleven',
                                        'twelve',
                                        'thirteen',
                                        'fourteen',
                                        'fifteen',
                                        'sixteen',
                                        'seventeen',
                                        'eighteen',
                                        'nineteen'
                                    );
                                    $list2 = array('', 'ten', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety');
                                    $list3 = array(
                                        '',
                                        'Thousand',
                                        'million',
                                        'billion',
                                        'trillion',
                                        'quadrillion',
                                        'quintillion',
                                        'sextillion',
                                        'septillion',
                                        'octillion',
                                        'nonillion',
                                        'decillion',
                                        'undecillion',
                                        'duodecillion',
                                        'tredecillion',
                                        'quattuordecillion',
                                        'quindecillion',
                                        'sexdecillion',
                                        'septendecillion',
                                        'octodecillion',
                                        'novemdecillion',
                                        'vigintillion'
                                    );

                                    $num_length = strlen($num);
                                    $levels = (int)(($num_length + 2) / 3);
                                    $max_length = $levels * 3;
                                    $num = substr('00' . $num, -$max_length);
                                    $num_levels = str_split($num, 3);

                                    for ($i = 0; $i < count($num_levels); $i++) {
                                        $levels--;
                                        $hundreds = (int)($num_levels[$i] / 100);
                                        $hundreds = ($hundreds ? ucfirst($list1[$hundreds]) . ' Hundred' . ' ' : '');
                                        $tens = (int)($num_levels[$i] % 100);
                                        $singles = '';

                                        if ($tens < 20) {
                                            $tens = ($tens ? ucfirst($list1[$tens]) . ' ' : '');
                                        } else {
                                            $tens = (int)($tens / 10);
                                            $tens = ucfirst($list2[$tens]) . ' ';
                                            $singles = (int)($num_levels[$i] % 10);
                                            $singles = ucfirst($list1[$singles]) . ' ';
                                        }

                                        $words[] = $hundreds . $tens . $singles . (($levels && (int)($num_levels[$i])) ? ucfirst($list3[$levels]) . ' ' : '');
                                    }

                                    $commas = count($words);
                                    if ($commas > 1) {
                                        $commas = $commas - 1;
                                    }

                                    return implode(' ', $words) . $decimalInWords;
                                }

                                $amount = $record->fixed_deed_rs ?? 0;
                                $stamp_paper_value = $record->stamp_paper_value ?? 0;
                                $amountInWords = convertNumberToWord($amount);
                                $stamp_paper_valueinwords = convertNumberToWord($stamp_paper_value);
                                ?>



                                <p>
                                    alongwith all rights, titles, interests, claims, easements, fittings and fixture and
                                    demands whatsoever into
                                    or upon the said Deed Land, the consideration of which although is {{ $record->scheme ?? 'N/A' }} land
                                    value of which for the
                                    purposes of this Deed is fixed at <span class="text_bold">Rs. <span>{{ $record->fixed_deed_rs }}</span>/- (Rupees <span>{{ $amountInWords }}</span> only)
                                        as per stamp papers value Rs. <span>{{ $record->stamp_paper_value }}</span>/- (Rupees <span>{{ $stamp_paper_valueinwords }}</span> only)
                                        according to Govt Schedule for the year {{ $record->schedule_year ?? 'N/A' }}.</span>
                                </p>

                                <div class="center">
                                    <h2 class="heading">DEED EXECUTED BY</h2>
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
                                    <h3 class="heading">IN FAVOUR OF</h3>
                                </div>
                                <p>
                                    <span class="text_bold">{{ config('app.org_name') }}</span>, a statutory body created and existing under the provisions of the {{ config('app.org_name') }} Order (Chief’s Executive Order No. XXVI of 2002), having its principal office at <span class="text_bold">{{ config('app.org_name') }}, Main Office Complex, Sector A Commercial, Phase-VI, Lahore Cantt</span> and a project office namely {{ config('app.org_name') }}, Head Office Jinnah Avenue (MB-2) APE Canal Road Bahawalpur, through <span class="text_bold">{{ $record->deed_in_favor_of_name }}</span>, its duly authorized officer, (hereinafter called the Vendee, which expression includes its successors and assigns) of the other part.
                                </p>



                                <div class="center">
                                    <h4>WHEREAS</h4>
                                </div>
                                <ol type="a" class="li_align">
                                    <li>
                                        That We, the Vendor is the lawful owner of Deed Land,

                                        @foreach ($purchase_land_row as $land_row)
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
                                            District {{ $record->district ?? 'Bahawalpur' }}</span>,

                                        along with all rights, title, interests, claims, fixtures and demands
                                        whatsoever into or upon the land hereby conveyed onto the vendee.
                                    </li>

                                    <li>
                                        That the Vendee is interested in purchasing land in <span class="text_bold">{{ $purchase_doc->mouza ?? 'N/A' }} Tehsil {{ $record->tehsil ?? 'Bahawalpur' }}
                                            District {{ $record->district ?? 'Bahawalpur' }}</span>, for furthering Vendee's objectives.
                                    </li>
                                    <li>
                                        That We/I, the Vendor has approached the Vendee and offered the Deed Land to the
                                        Vendee and the Vendee has
                                        agreed to purchase the same against consideration of the allotment of plots
                                        under the <span
                                            class="text_bold">"{{ $record->scheme ?? 'N/A' }}"</span>
                                        of the Vendee in vogue, on mutually agreed terms and conditions.
                                    </li>
                                    <li>
                                        That this Deed is being executed by the Parties hereto to record the mutual
                                        understandings reached and
                                        to effectively Transferred the subject Deed Land to the Vendee along with all
                                        the rights and benefits
                                        attached thereto.
                                    </li>
                                </ol>
                                <p>NOW THEREFORE it is agreed between the Parties hereto as follows:</p>
                                <ol class="li_align">
                                    <li>
                                        That We/I, the Vendor hereby transfers the subject Deed Land fully described
                                        hereinabove along with all
                                        the Vendor's rights attached to the Deed Land and/or were possessed by the
                                        Vendor by virtue of ownership
                                        and/or possession thereof, with Vendee's free will, consent by knowing time and
                                        space and without any
                                        undue influence, coercion or fear.
                                    </li>
                                    <li>
                                        That the actual/physical possession of the subject Deed Land has already been
                                        taken over by the
                                        Vendee, with the consent of the Vendor (s), on adequate compensation, calculated
                                        under the <span
                                            class="text_bold">"Exemption
                                            Plots"</span> policy, as expressly agreed between the Parties and the Vendor has sold the subject
                                        Deed Land
                                        with all rights possessed by the Vendor to the Vendee. By the execution of this
                                        Deed the Vendor
                                        acknowledges the Transferred of the possession to the Vendee unconditionally by
                                        the Vendor.
                                    </li>
                                    <li>
                                        That the consideration of the aforesaid Transferred, for the purposes of this
                                        Deed, is at <span class="text_bold">Rs.
                                            <span>{{ $record->fixed_deed_rs }}</span>/- (Rupees <span>{{ $amountInWords }}</span> only)</span> which has been
                                        received in kind
                                        by the Vendee in the form of <span class="text_bold"> "{{ $record->scheme ?? 'N/A' }} Plot File (s)"</span>, in full and final
                                        settlement of the
                                        consideration amount of the said Deed Land, receipt whereof is hereby
                                        acknowledged and there is nothing
                                        further due from the Vendee to the Vendor (s). The Vendor acknowledges receipt
                                        of the said consideration
                                        in full and final settlement of all accounts pertaining to this Transfers.
                                    </li>
                                    <li>
                                        That We/I, the Vendor hereby assures the Vendee that the title of the Vendor (s) is
                                        legal, complete
                                        proper and that the Vendor is empowered, in law and equity, to alienate and sell
                                        the said Deed Land to
                                        the Vendor against consideration mentioned supra. The Vendor also assures and
                                        hereby undertake to
                                        indemnify and keep indemnifying the Vendee, to the Vendee's entire satisfaction,
                                        against any defect in
                                        the title and against any claim of any third party arising howsoever.
                                    </li>
                                    <li>
                                        That We/I, the Vendor hereby assure that, the Vendor, directly or indirectly,
                                        impliedly or expressly,
                                        verbally or orally, had not previously entered into any agreement and/or
                                        understanding with any person
                                        That We/Is adverse to the interest of the Vendee accruing through this deed and in
                                        case of any such claim
                                        this Deed shall remain un-fettered and the Vendor shall be responsible for the
                                        legal consequences, if
                                        any, including compensation for any damage, loss and/or injury sustained by the
                                        Vendee on account of any
                                        breach of any undertaking, covenant or assurance on which the Vendee has relied
                                        upon.
                                    </li>
                                    <li>
                                        That We/I, the Vendor hereby agrees and undertakes to get the mutation of the said
                                        Deed Land attested in
                                        favor of Vendee on basis of this deed.
                                    </li>
                                    <li>
                                        That the Vendor hereby surrenders the Vendor's rights of village / Shamlat /
                                        deh, common land, share
                                        of Khall, Nala (Canal) or easement rights attached to the ownership of said Deed
                                        Land etc in favor of
                                        Vendee. Furthermore, the Vendor (s) acknowledges and confirms that all the legal
                                        heirs or successors in
                                        interest of the Vendor (s) have no interest to the extent of above said Deed
                                        Land.
                                    </li>
                                    <li>
                                        That the above said Deed Land is free of all or any encumbrances i.e lease,
                                        mortgage, lien, tenancy,
                                        pledge, water rate, duties, rent and cesses etc. In case of any defect, the
                                        Vendor shall be responsible
                                        for legal action, if any. Furthermore all charges, taxes, cesses etc attracted
                                        to the subject Land or
                                        enjoyment thereof or any services etc attached thereto, pertaining to a period
                                        prior to the execution of
                                        this Deed, are the responsibility of the Vendor (s) and the Vendee acknowledges
                                        and affirms, by
                                        executing this Deed, that the same are cleared in full.
                                    </li>
                                    <li>
                                        That the Vendee shall hold and/or possess the Deed Land peacefully and enjoy the
                                        possession thereof
                                        without any hindrance and/or claim whatsoever and in case of any defect/claim,
                                        the Vendor shall be
                                        responsible for legal consequences.
                                    </li>
                                    <li>
                                        That the above said land is free of all/any litigations, in case of any
                                        litigation or dispute
                                        pending in any court of law or authority, the vendor shall be responsible for
                                        any loss or damage
                                        sustained by the Vendee on account of any such legal action / proceeding.
                                    </li>
                                    <li>
                                        That any dispute, between the Parties hereto, touching the Deed Land or any
                                        interpretation or
                                        implementation of any term of this Deed shall be referred for settlement through
                                        arbitration, under the
                                        provisions of the Arbitration Act 1940, to the Corps Commander Bahawalpur or his
                                        nominee, who shall act
                                        as the Sole Arbitrator, and any decision made by the said Sole Arbitrator shall
                                        be final and binding
                                        between the Parties hereto.
                                    </li>
                                    <li>
                                        That all registration expenses, i.e cost of non judicial stamp papers,
                                        registration fee etc have been
                                        paid by the vendor.
                                    </li>
                                    <li>
                                        That the Parties hereto are duly authorized and competent to execute this Deed.
                                    </li>
                                </ol>
                                <p>IN WITNESS WHEREOF the parties hereto have affixed their seal and signatures along
                                    with the thumb impression,
                                    hereunder on the date and place hereinbefore mentioned above.</p>

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
                                        as per CNIC Record,
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
                                        as per CNIC Record,
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
                                    <div class="center">
                                        <h4 class="heading">WITNESSES</h4>
                                    </div>
                                    <ol class="flex_display li_align">
                                        <li class="left_flex">
                                            <span class="text_bold">
                                                <hr>
                                                {{ $record->deed_executed_by_lo_name ?? 'N/A' }}
                                                {{ $record->vendor_relationship ?? 'N/A' }}
                                                {{ $record->deed_executed_by_lo_father_name ?? 'N/A' }}
                                                (CNIC No. {{ $record->deed_executed_by_cnic ?? 'N/A' }}), Caste {{ $record->deed_executed_by_caste ?? 'N/A' }}, resident of {{ $record->deed_executed_by_address ?? 'N/A' }}
                                            </span>
                                        </li>
                                        <li class="right_flex">
                                            <span class="text_bold">

                                                <hr>
                                                {{ $record->vendee_witness_name ?? 'N/A' }}
                                                {{ $record->vendee_relationship ?? 'N/A' }}
                                                {{ $record->vendee_witness_father_name ?? 'N/A' }}
                                                (CNIC No. {{ $record->vendee_witness_cnic ?? 'N/A' }}), Caste {{ $record->vendee_witness_caste ?? 'N/A' }}, resident of {{ $record->vendee_witness_address ?? 'N/A' }}
                                            </span>
                                        </li>
                                    </ol>
                                </div>
                            </div>
                            <button class="btn" onclick="window.print()">Print</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
</div>
<script>
    window.onload = function() {
        window.print();
    }
</script>