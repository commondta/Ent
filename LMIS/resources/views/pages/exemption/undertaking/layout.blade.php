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
        margin: 10px;
    }

    .center {
        text-align: center;
    }

    .right {
        text-align: right;
        line-height: 2;
    }

    .left {
        text-align: left;
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
                $date = new DateTime($undertaking->date);
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
                            <h2 class="heading">UNDERTAKING</h2>
                        </div>

                        <!-- MAIN PARAGRAPH -->
                        <p>
                        <div class="left">
                            <h3 class="heading">Land Owner</h3>
                        </div>
                        Land measuring <span class="text_bold">{{ $landDetail->land_measuring_k }}</span> Kanals, <span class="text_bold">{{ $landDetail->land_measuring_m }}</span> Marlas, <span class="text_bold">{{ $landDetail->land_measuring_sqft }}</span> Sqft, Khewat No <span class="text_bold">{{ $landDetail->khewat_no }}</span>, Khatooni No <span class="text_bold">{{ $landDetail->khatooni_no }}</span>, Khasra Nos/Share <span class="text_bold">{{ $landDetail->qatat }}</span>, Total land measuring <span class="text_bold">{{ $landDetail->land_measuring_k }}</span> Kanals, <span class="text_bold">{{ $landDetail->land_measuring_m }}</span> Marlas, <span class="text_bold">{{ $landDetail->land_measuring_sqft }}</span> Sqft as shown in his name in register Haqdaran for year <span class="text_bold">{{ $record->record_of_rights_year ?? 'N/A' }}</span> at Mouza/Chak No. <span class="text_bold">{{ $land_form->mouza }}</span> Tehsil <span class="text_bold">{{ $land_form->tehsil }}</span> District Bahawalpur is owned by
                        @php
                        $isPoaOwner = in_array($owner->lo_cod, $poaCodes);
                        @endphp

                        @if($isPoaOwner)
                        <span class="text_bold">{{$land_form->poa_name}}</span>
                        <span>{{$land_form->relationship ?? ''}}</span>
                        <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                        CNIC NO <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>,
                        Guardian/Power of Attorney Holder on behalf of <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? '' }} {{ $owner->relationship_cnic ?? '' }} {{ $owner->father_name_cnic ?? '' }}</span>
                        @else

                        <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? '' }} {{ $owner->relationship_cnic ?? '' }} {{ $owner->father_name_cnic ?? '' }}</span>
                        @endif
                        I authorized to <span class="text_bold">{{$land_p->lp_name ?? 'N/A' }} {{$land_p->relationship ?? 'N/A' }} {{$land_p->father_name ?? 'N/A' }}</span> through its representative ________________________ S/O _________________________ CNIC No __________________________ to sell my land to {{ config('app.org_legal') }} and complete documentation as per {{ config('app.org_short') }} rules.


                        <div class="right">
                            <h2 class="">(Land Owner)</h2>
                            @php
                            $isPoaOwner = in_array($owner->lo_cod, $poaCodes);
                            @endphp

                            @if($isPoaOwner)
                            <span class="text_bold">{{$land_form->poa_name}}</span>
                            <span>{{$land_form->relationship ?? ''}}</span>
                            <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,<br>
                            CNIC NO: <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span><br>
                            @else
                            <span class="text_bold">Name: {{ $owner->lo_name_as_per_cnic ?? '' }} {{ $owner->relationship_cnic ?? '' }} {{ $owner->father_name_cnic ?? '' }} </span><br>
                            <span class="text_bold">CNIC No: {{ $owner->lo_cnic ?? '' }}</span><br>
                            @endif
                            <span class="text_bold">Signature & Thumb: ______________________</span>

                        </div>
                        <div class="left">
                            <h3 class="heading">Investor / Land Provider</h3>
                        </div>
                        The land has been sold to {{ config('app.org_legal') }} through me. Any problem, if arises any time after sale of this land to {{ config('app.org_short') }}, will be resolved by me. Any query or question of any nature of litigation will also be addressed by me at my own risk and cost, failing which {{ config('app.org_short') }} will have the right to claim damages from me directly or through court of law.

                        <div class="right">
                            <h2 class="">(Land Provider)</h2>
                            <span class="text_bold">Name: <span class="text_bold">{{$land_p->lp_name ?? 'N/A' }} {{$land_p->relationship ?? 'N/A' }} {{$land_p->father_name ?? 'N/A' }}</span><br>
                                <span class="text_bold">CNIC No: {{ $land_p->lp_cnic ?? 'N/A' }}</span><br>
                                <span class="text_bold">Signature & Thumb: ______________________</span>

                        </div>
                        <div class="center">
                            <h2 class="heading">VERIFICATION</h2>
                        </div>
                        Verified on Oath at Bahawalpur this __________day of ________________ that the contents of this affidavit are true and correct to the best of my / our knowledge, information and behalf and nothing has been concealed therein from.

                        </p>

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