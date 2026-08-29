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


    .right {
        text-align: right;
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
                $date = new DateTime($affidavit->date);
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
                            <h1 class="heading">AFFIDAVIT</h1>
                        </div>


                        <!-- MAIN PARAGRAPH -->
                        <p>
                            (Affidavit of Mr/Mrs/Messrs:
                            @php
                            $isPOAOwner = in_array($owner->lo_cod, $poaCodes);
                            @endphp


                            @if(!empty($land_form->poa_name) && $isPOAOwner)
                            <span class="text_bold">{{$land_form->poa_name}}</span>
                            <span>{{$land_form->relationship ?? ''}}</span>
                            <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>,
                            CNIC NO <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>,
                            Guardian/Power of Attorney Holder on behalf of


                            <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? '' }} {{ $owner->relationship_cnic ?? '' }} {{ $owner->father_name_cnic ?? '' }} CNIC No: {{ $owner->lo_cnic ?? '' }}</span><br>
                            @else
                            <span class="text_bold">{{ $owner->lo_name_as_per_cnic ?? '' }} {{ $owner->relationship_cnic ?? '' }} {{ $owner->father_name_cnic ?? '' }} CNIC No: {{ $owner->lo_cnic ?? '' }}</span><br>
                            @endif


                            I / We the above named deponent(s) do hereby solemnly affirm and declare as under: -
                        <ol class="li_align">
                            <li class="text_indent">
                                That I/we are the lawful owner(s) of land measuring <span class="text_bold">{{ $landDetail->land_measuring_k }}</span> Kanals, <span class="text_bold">{{ $landDetail->land_measuring_m }}</span> Marlas, <span class="text_bold">{{ $landDetail->land_measuring_sqft }}</span> Sqft in Khasra/Share No(s) <span class="text_bold">{{ $landDetail->transfer_share }}</span>,
                                Khatooni No <span class="text_bold">{{ $landDetail->khatooni_no }}</span>, Khewat No <span class="text_bold">{{ $landDetail->khewat_no }}</span> of Mouza/Chak <span class="text_bold">{{ $land_form->mouza }}</span> Tehsil <span class="text_bold">{{ $land_form->tehsil }}</span> District Bahawalpur which falls in {{ config('app.org_short') }}.


                            </li>
                            <li class="text_indent">
                                That I / we have offered the above mentioned land for sale to {{ config('app.org_legal') }} for its Housing Scheme and acknowledge the possession lawfully handed over to the Authority.
                            </li>
                            <li class="text_indent">
                                That I / we have not mortgaged the above land to any person or agency, nor is the land under any charge, lien or encumbrance of any type in any manner whatsoever.
                            </li>
                            <li class="text_indent">
                                That I / we have not entered into any agreement for sale of above land with any other person other than {{ config('app.org_name') }}.
                            </li>
                            <li class="text_indent">
                                I / we have not filed any writ petition/civil suit against said {{ config('app.org_short') }} in respect of the area being offered for sale. The particulars of the writ petition / civil suit (if any) are: -<br>
                                a. Name / Names of Petitioners.<br>
                                b. Writ Petition/Civil Suit No with date of institution.<br>
                                c. Name of Court<br>
                                d. Relief sought and further that the area being offered for sale to {{ config('app.org_short') }} is not under litigation and / or subjudice in any court of law.


                            </li>


                        </ol>
                        <div class="right">
                            <h2 class="heading">DEPONENT</h2>
                        </div>
                        <div class="center">
                            <h2 class="heading">VERIFICATION</h2>
                        </div>
                        Verified on Oath at Bahawalpur this_______ day of ______________ that the contents of this affidavit are true and correct to the best of my / our knowledge, information and behalf and nothing has been concealed therein from.
                        <div class="right">
                            <h2 class="heading">DEPONENT</h2>
                        </div>
                        <h4 class="heading">Note:</h4> This affidavit has to be typed on a stamp paper of Rs. 300/- or as prescribed by the Government.


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


<!-- <script>
    window.onload = function() {
        window.print();
    }
</script> -->

