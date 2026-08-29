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
            text-align: justify;
        }

        .btn {
            display: none;
        }

        .section3,
        .section4 {
            page-break-before: always;
        }

        .grid-table {
            font-size: 11px;
        }

    }

    body {
        text-align: justify;
        margin: 60px 80px;
    }

    .text_bold {
        font-weight: bold;
    }

    .heading {
        border-bottom: 1px solid var(--lm-border);
        display: inline-block;
    }

    .alert {
        position: absolute;
        border-bottom: 1px solid var(--lm-border);
        border-top: 1px solid var(--lm-border);
        display: inline-block;
        rotate: 314deg;
        margin-left: -65px;
        margin-top: -60px;
    }

    .dotted_line {
        border-top: 2px dashed black;
    }

    .center {
        text-align: center;
    }

    .li_align {
        padding-left: 17px;
    }

    .li_space {
        padding-left: 30px;
    }

    .li_height {
        line-height: 1.6;
    }

    .align_end {
        text-align: end;
    }

    .Photos {
        border: 1px solid var(--lm-border);
        width: 140px;
        height: 160px;
        display: flex;
        align-items: center;
        text-align: center;
        float: right;
    }

    .grid-table {
        display: grid;
        grid-template-columns: 5fr 3fr 3fr 3fr 1fr 1fr 1fr 3fr;
        gap: 30px;

    }

    .line_light {
        border-bottom: 1px solid var(--lm-border);
    }

    .line {
        border-bottom: 1px solid var(--lm-border);
    }


    .custom_table {
        width: 100%;
        border-collapse: collapse;
    }

    .custom_table td,
    .custom_table th {
        border: 1px solid var(--lm-border);
        padding: 8px;
    }

    .custom_line {
        width: 200px;
        margin: auto;
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
        margin: 10px 0 8px 0;
    }
</style>

<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="mb-9">
                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-body">
                            <div class="d-flex align-items-center"><img src="{{ asset('public/assets/img/icons/logo.png'); }}"
                                    alt="phoenix"
                                    width="200" />
                                <div class="align_end" style="margin-top: 60px">
                                    <p>Serial No : _________</p>
                                    <p>Challan No:________</p>
                                    <p style="line-height: 0px;"><span class="line">Rs. 2000/-</span>
                                    <p><span>(Per Acre)</span></p>
                                    </p>
                                    <div class="Photos">
                                        <p style="margin: 0 10px;">
                                            6 x Photos of
                                            Land Owner / Investor
                                        </p>
                                    </div>
                                </div>
                                <p class="alert text_bold">NO TRANSACTION ON THIS FORM</p>
                                <div class="center">
                                    <h1 class="heading" style="margin-top: 0px;">LAND OFFER FORM</h1>
                                </div>
                                <h3 class="heading" style="margin-top: 60px;">IMPORTANT</h3>
                                <ol class="li_align li_height">
                                    <li class="li_space">No column will be left blank. </li>
                                    <li class="li_space">Will be submitted with six attested copies of passport size photographs and three copies of
                                        attested CNIC of land owner or investor (as applicable).</li>
                                    <li class="li_space">No sale of plot and affidavit to be issued against this land form till acceptance of land
                                        by the Authority and completion of all essential formalities.</li>
                                    <li class="li_space">Legal proceedings will be initiated against the misuse of this form.</li>
                                    <li class="li_space">Form will not be accepted without complete documents. </li>
                                    <li class="li_space">Specimen of affidavit and undertaking are attached, however both documents will be typed on
                                        a stamp paper of Rs. 100/- or as prescribed by the Government of Pakistan.</li>
                                    <li class="li_space">Rs. 2000/- per acre will be deposited along with the form.</li>
                                    <li class="li_space">This Form does not constitute any basis for marketing / land transaction of any kind.</li>
                                </ol>
                                <div class="dotted_line"></div>

                                <div class="section2">
                                    <div class="center" style="margin-top: 15px;">
                                        <h3 class="heading">PERSONAL DATA</h3>
                                    </div>

                                    <ol start="9" class="li_align li_height">
                                        @if(!empty($land_form->poa_name))

                                        <li class="li_space">
                                            I, <span class="text_bold">{{$land_form->poa_name}}</span>
                                            <span>{{$land_form->relationship ?? ''}}</span>
                                            <span class="text_bold">{{$land_form->poa_father_name ?? ''}}</span>
                                            Caste <span class="text_bold">{{$land_form->poa_caste ?? ''}}</span>

                                            the authorized representative of the land measuring
                                            <span class="text_bold">{{$record->total_acre ?? ''}}</span> Acres,
                                            <span class="text_bold">{{$record->total_kanal ?? ''}}</span> Kanals
                                            <span class="text_bold">{{$record->total_marla ?? ''}}</span> Marlas
                                            and <span class="text_bold">{{$record->total_sqft ?? ''}}</span> Sqft
                                            located in Mouza <span class="text_bold">{{$land_form->mouza ?? ''}}</span>
                                            Tehsil <span class="text_bold">{{$land_form->tehsil ?? ''}}</span>
                                            District <span class="text_bold">{{$land_form->district ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            Computerized National Identity Card No:
                                            <span class="text_bold">{{$land_form->poa_cnic ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            <span class="text_bold">Present Residential Address</span>:
                                            {{$land_form->poa_current_address ?? ''}}
                                        </li>

                                        <li class="li_space">
                                            <span class="text_bold">Permanent Residential Address</span>:
                                            {{$land_form->poa_permanent_address ?? ''}}
                                        </li>

                                        @elseif($land_owners && $land_owners->count() > 0)

                                        @php $loRow = $land_owners->first(); @endphp

                                        <li class="li_space">
                                            I, <span class="text_bold">{{$loRow->lo_name_as_per_cnic ?? ''}}</span>
                                            <span>{{$loRow->relationship_cnic ?? ''}}</span>
                                            <span class="text_bold">{{$loRow->father_name_cnic ?? ''}}</span>
                                            Caste <span class="text_bold">{{$loRow->caste ?? ''}}</span>

                                            the sole owner / investor of the land measuring
                                            <span class="text_bold">{{$record->total_acre ?? ''}}</span> Acres,
                                            <span class="text_bold">{{$record->total_kanal ?? ''}}</span> Kanals
                                            <span class="text_bold">{{$record->total_marla ?? ''}}</span> Marlas
                                            and <span class="text_bold">{{$record->total_sqft ?? ''}}</span> Sqft
                                            located in Mouza <span class="text_bold">{{$land_form->mouza ?? ''}}</span>
                                            Tehsil <span class="text_bold">{{$land_form->tehsil ?? ''}}</span>
                                            District <span class="text_bold">{{$land_form->district ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            Computerized National Identity Card No:
                                            <span class="text_bold">{{$loRow->lo_cnic ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            <span class="text_bold">Present Residential Address</span>:
                                            {{$loRow->address ?? ''}}
                                        </li>

                                        <li class="li_space">
                                            <span class="text_bold">Permanent Residential Address</span>:
                                            {{$loRow->address ?? ''}}
                                        </li>

                                        @else

                                        <li class="li_space">
                                            I, <span class="text_bold">{{$land_form->lo_name ?? ''}}</span>
                                            (Alias) <span class="text_bold">{{$land_form->so ?? ''}}</span>,
                                            am the sole owner / investor of the land measuring
                                            <span class="text_bold">{{$record->acre ?? ''}}</span> Acres,
                                            <span class="text_bold">{{$record->kanal ?? ''}}</span> Kanals
                                            <span class="text_bold">{{$record->total_marla ?? ''}}</span> Marlas
                                            located in Mouza <span class="text_bold">{{$land_form->mouza ?? ''}}</span>
                                            Tehsil <span class="text_bold">{{$land_form->tehsil ?? ''}}</span>
                                            District <span class="text_bold">{{$land_form->district ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            Computerized National Identity Card No:
                                            <span class="text_bold">{{$land_form->lo_cnic ?? ''}}</span>
                                        </li>

                                        <li class="li_space">
                                            <h4 class="heading">Permanent Home Address</h4>
                                            <p style="margin-top: 0px;">{{$land_form->address ?? ''}}</p>
                                        </li>

                                        <li class="li_space">
                                            <span class="text_bold">Present Residential Address</span>:
                                            {{$land_form->address ?? ''}}
                                        </li>

                                        @endif

                                    </ol>
                                    <div class="center" style="margin-top: 15px;">
                                        <h3 class="heading">LAND DATA</h3>
                                    </div>

                                    <ol start="13" class="li_align li_height">
                                        <li class="li_space">
                                            <h4 class="heading">Detail of Land</h4>
                                        </li>
                                        @if($land_form_details && $land_form_details->count() > 0)
                                        <div style="margin-bottom: 20px;">
                                            <table class="custom_table">
                                                <thead>
                                                    <tr>
                                                        <th class="center">Mouza</th>
                                                        <th class="center">Khewat No</th>
                                                        <th class="center">Khatooni No</th>
                                                        <th class="center">Khasra No</th>

                                                        <th class="center">Kanal</th>
                                                        <th class="center">Marla</th>
                                                        <th class="center">Sqft</th>
                                                        <th class="center">Land Category</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    @foreach($land_form_details as $landRow)
                                                    <tr>
                                                        <td class="center">{{$land_form->mouza ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->khewat_no ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->khatooni_no ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->qatat ?? 'N/A'}}</td>

                                                        <td class="center">{{$landRow->land_measuring_k ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->land_measuring_m ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->land_measuring_sqft ?? 'N/A'}}</td>
                                                        <td class="center">{{$landRow->land_category ?? 'N/A'}}</td>
                                                    </tr>

                                                    @endforeach
                                                    <tr style="background: var(--lm-surface);font-weight:bold">
                                                        <td colspan="4" style="text-align:right;border:1px solid var(--lm-border);">Total of Kanal, Marla,Sqft and Acre respectively</td>

                                                        <td class="center">{{$record->total_kanal ?? 'N/A'}}</td>

                                                        <td class="center">{{$record->total_marla ?? 'N/A'}}</td>

                                                        <td class="center">{{$record->total_sqft ?? 'N/A'}}</td>

                                                        <td class="center">{{$record->total_acre ?? 'N/A'}}</td>


                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        @else
                                        <div class="grid-table">
                                            <div class="center"><span class="line_light">Mouza</span></div>
                                            <div class="center"><span class="line_light">Khewat No</span></div>
                                            <div class="center"><span class="line_light">Khatooni No</span></div>
                                            <div class="center"><span class="line_light">Khasra No</span></div>
                                            <div class="center"><span class="line_light">Acre</span></div>
                                            <div class="center"><span class="line_light">Kanal</span></div>
                                            <div class="center"><span class="line_light">Marla</span></div>
                                            <div class="center"><span class="line_light">Total</span></div>

                                        </div>
                                        @endif
                                    </ol>
                                    <ol start="14" class="li_align li_height">
                                        <li class="li_space">
                                            <h3 class=" text_bold">Following documents are enclosed:-</h3>
                                            <ol type="a" class="li_align li_height">
                                                <li class="li_space">Original copy of Fard Malkiyat attested by Tehsildar. (Not older than two
                                                    weeksfrom the date of issue)</li>
                                                <li class="li_space">NEC from Sub-Registrar.</li>
                                                <li class="li_space">Copy of original Khasra girdawari from Halqa patwari and attested by Tehsildar.
                                                </li>
                                                <li class="li_space">Original copy of Transfer Order if Vendor is allottee (if applicable).</li>
                                                <li class="li_space">Original copy of sale deed forming basis of title.</li>
                                                <li class="li_space">Attested copy of above sale deed from Registrar.</li>
                                                <li class="li_space">Copy of Intiqal stamped and attested by Tehsildar.</li>
                                                <li class="li_space">Copy of Aks Shajra from Halqa Patwari and attested by Tehsildar.</li>
                                                <li class="li_space">Affidavit as per specimen.</li>
                                                <li class="li_space">Undertaking as per specimen.</li>
                                                <li class="li_space">Original Payment Challan Form.</li>
                                                <li class="li_space">6 x attested passport size photographs of land owner or investor (as
                                                    applicable)</li>
                                                <li class="li_space">3 x attested copies of CNIC of land owner or investor (as applicable).</li>
                                            </ol>

                                        </li>
                                    </ol>
                                    <div class="center" style="margin-top: 15px;">
                                        <h3 class="heading">AFFIRMATION</h3>
                                    </div>
                                    <ol start="15" class="li_align li_height">
                                        <li class="li_space">
                                            I swear by <span class="text_bold">ALMIGHTY GOD /</span> solemnly affirm in the presence of <span class="text_bold">ALMIGHTY GOD</span> that the informations given in
                                            this Sale of Land Form is true and correct to the best of my knowledge. I fully understand that my false
                                            statement or material omission / suppression of any fact shall render me liable to legal action as per
                                            the Law of the Land, besides being declared unfit for any business in {{ config('app.org_short') }}.
                                        </li>
                                    </ol>

                                    <table class="custom_table">
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th class="center"><span>Owner / Investor (As applicable)</span></th>
                                        </tr>
                                        <tr>
                                            <td>Place: <span class="heading text_bold">Bahawalpur</span></td>
                                            <td>Thumb Impression: <span style="display: block;">(Left Hand Thumb)</span></td>
                                            <td>
                                                <div class="line custom_line"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date: __________</td>
                                            <td>Signature</td>
                                            <td>
                                                <div class="line custom_line"></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>



                                <button class="btn" onclick="window.print()">Print</button>

                            </div>
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