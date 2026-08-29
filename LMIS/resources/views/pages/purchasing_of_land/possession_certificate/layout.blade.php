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

    .possession-title {
        text-align: center;
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 10px;
        text-decoration: underline;
    }

    .possession-declaration {
        margin-bottom: 20px;
        line-height: 1.8;
        text-align: justify;
    }

    .possession-table {
        width: 100%;
        border-collapse: collapse;
        margin: 20px 0;
    }

    .possession-table th,
    .possession-table td {
        border: 1px solid var(--lm-border);
        padding: 10px;
        text-align: center;
        font-size: 12px;
    }

    .possession-table th {
        background-color: var(--lm-surface);
        font-weight: bold;
    }

    .signature-section {
        margin-top: 30px;
        width: 100%;
    }

    .signature-line {
        display: inline-block;
        width: 48%;
        text-align: center;
        vertical-align: bottom;
        margin: 20px 1%;
    }

    .signature-boxes {
        display: flex;
        justify-content: space-between;
        margin-top: 40px;
    }

    .signature-box {
        text-align: center;
        width: 30%;
    }

    .signature-box-line {
        border-top: 1px solid var(--lm-border);
        margin-top: 50px;
        font-weight: bold;
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


                                <div class="possession-title">POSSESSION CERTIFICATE</div>

                                <div class="possession-declaration">
                                    <p><strong>File No. {{ $record->base_code_no ?? '________________' }} &nbsp;&nbsp; Date {{ $record->date ?? '________________' }}</strong></p>

                                    <p>
                                        I,
                                        @if($land_owners && $land_owners->count() > 0)
                                        @php $loRow = $land_owners->first(); @endphp
                                        <span class="text_bold">{{ $loRow->lo_name_as_per_cnic ?? '' }}</span> <span class="text_bold">{{ $loRow->relationship_cnic ?? '' }}</span> <span class="text_bold">{{ $loRow->father_name_cnic ?? '' }}</span>
                                        Caste <span class="text_bold">{{ $loRow->caste ?? '' }}</span> CNIC No. <span class="text_bold">{{ $loRow->lo_cnic ?? '' }}</span>
                                        Resident of <span class="text_bold">{{ $loRow->address ?? '' }}</span>
                                        @endif
                                        Hand over the possession of my land area measuring <span class="text_bold">{{ $record->total_land_kanal ?? '_____' }}</span> Kanal <span class="text_bold">{{ $record->total_land_marla ?? '_____' }}</span> Marla <span class="text_bold">{{ $record->total_land_sqft ?? '_____' }} </span>Sq. Ft., the details of which are as follows, to {{ config('app.org_short') }}. I am bound to get the registry (title deed) of the said land transferred in the name of {{ config('app.org_short') }} within 15 days. If there is a delay in the registry for any reason, I will not demand the return of the land nor will I cultivate it. From today onwards, every kind of possession and control of the mentioned land will belong to {{ config('app.org_short') }}.
                                    </p>
                                    <br>
                                    <p><span class="text_bold">Mauza / Chak No:</span> {{ $record->mouza ?? '____________________' }} <span class="text_bold">Date of Issuance of Possession Certificate:</span> {{ !empty($record->date) ? \Carbon\Carbon::parse($record->date)->format('d M Y') : '____________________' }} </p>
                                </div>
                                <!-- <div class="section-land-owners" style="margin-top: 30px;">
                                    <h3 style="text-align: center; text-decoration: underline; margin-bottom: 15px;">LAND OWNERS DETAILS</h3>

                                    <table class="possession-table">
                                        <thead>
                                            <tr>
                                                <th style="width: 5%;">S/No</th>
                                                <th style="width: 30%;">Land Owner Name</th>
                                                <th style="width: 25%;">W/O,/S/O,D/O,Widow of</th>
                                                <th style="width: 25%;">CNIC No</th>
                                                <th style="width: 15%;">Contact No</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            @if($land_owners && $land_owners->count() > 0)
                                            @foreach($land_owners as $key => $lo)
                                            <tr>
                                                <td>{{ $key + 1 }}</td>
                                                <td>{{ $lo->lo_name_as_per_cnic ?? '______' }}</td>
                                                <td>{{ $lo->father_name_cnic ?? '______' }}</td>
                                                <td>{{ $lo->lo_cnic ?? '______' }}</td>
                                                <td>{{ $lo->contact_no ?? '______' }}</td>
                                            </tr>
                                            @endforeach
                                            @else
                                            <tr>
                                                <td colspan="5" style="text-align: center; font-style: italic;">No land owners available</td>
                                            </tr>
                                            @endif
                                        </tbody>
                                    </table>
                                </div> -->

                                <div class="section-land-details">
                                    <h3 style="text-align: center; text-decoration: underline; margin-bottom: 15px;">LAND DETAILS</h3>

                                    <table class="possession-table">
                                        <thead>
                                            <tr>
                                                <th style="width: 5%;">S/No</th>

                                                <th style="width: 10%;">Khewat No</th>
                                                <th style="width: 10%;">Khatooni No</th>
                                                <th style="width: 10%;">Qatat</th>
                                                <th style="width: 10%;">Sector</th>
                                                <th style="width: 10%;">Land Category</th>
                                                <th colspan="3" style="text-align: center; width: 20%;">Land Measuring</th>
                                                <th colspan="3" style="text-align: center; width: 20%;">Possessed Land</th>
                                                <th colspan="3" style="text-align: center; width: 20%;">Unpossessed Land</th>

                                            </tr>
                                            <tr>
                                                <th colspan="6"></th>
                                                <th style="width: 6.67%;">Acre</th>
                                                <th style="width: 6.67%;">Kanal</th>
                                                <th style="width: 6.67%;">Marla</th>
                                                <th style="width: 6.67%;">Acre</th>
                                                <th style="width: 6.67%;">Kanal</th>
                                                <th style="width: 6.67%;">Marla</th>
                                                <th style="width: 6.67%;">Acre</th>
                                                <th style="width: 6.67%;">Kanal</th>
                                                <th style="width: 6.67%;">Marla</th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                            @if($land_details && $land_details->count() > 0)
                                            @foreach($land_details as $key => $landRow)
                                            <tr>
                                                <td>{{ $key + 1 }}</td>
                                                <td>{{ $landRow->khewat_no ?? '______' }}</td>
                                                <td>{{ $landRow->khatooni_no ?? '______' }}</td>
                                                <td>{{ $landRow->qatat ?? '______' }}</td>
                                                <td>{{ $landRow->sector ?? '______' }}</td>
                                                <td>{{ $landRow->land_category ?? '______' }}</td>
                                                <td>{{ $landRow->land_measuring_k ?? '______' }}</td>
                                                <td>{{ $landRow->land_measuring_m ?? '______' }}</td>
                                                <td>{{ $landRow->land_measuring_sqft ?? '______' }}</td>
                                                <td>{{ $landRow->possessed_k ?? '______' }}</td>
                                                <td>{{ $landRow->possessed_m ?? '______' }}</td>
                                                <td>{{ $landRow->possessed_sqft ?? '______' }}</td>
                                                <td>{{ $landRow->unpossessed_k ?? '______' }}</td>
                                                <td>{{ $landRow->unpossessed_m ?? '______' }}</td>
                                                <td>{{ $landRow->unpossessed_sqft ?? '______' }}</td>
                                            </tr>
                                            @endforeach
                                            @else
                                            <tr>
                                                <td colspan="15" style="text-align: center; font-style: italic;">No land details available</td>
                                            </tr>
                                            @endif
                                            <tr style="background-color: var(--lm-selected); font-weight: bold;">
                                                <td colspan="6" style="text-align: right;">TOTAL</td>
                                                <td>{{ $record->total_land_kanal ?? '______' }}</td>
                                                <td>{{ $record->total_land_marla ?? '______' }}</td>
                                                <td>{{ $record->total_land_sqft ?? '______' }}</td>
                                                <td>{{ $record->total_poss_kanal ?? '______' }}</td>
                                                <td>{{ $record->total_poss_marla ?? '______' }}</td>
                                                <td>{{ $record->total_poss_sqft ?? '______' }}</td>
                                                <td>{{ $record->total_unposs_kanal ?? '______' }}</td>
                                                <td>{{ $record->total_unposs_marla ?? '______' }}</td>
                                                <td>{{ $record->total_unposs_sqft ?? '______' }}</td>
                                            </tr>
                                            <tr style="background-color: var(--lm-selected); font-weight: bold;">
                                                <td colspan="6" style="text-align: right;">TOTAL ACRE</td>
                                                <td colspan="3">{{ $record->total_land_acres ?? '______' }}</td>
                                                <td colspan="3">{{ $record->total_poss_acres ?? '______' }}</td>
                                                <td colspan="3">{{ $record->total_unposs_acres ?? '______' }}</td>
                                            </tr>
                                        </tbody>
                                    </table>


                                </div>


                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Owner Name : <span style="text-decoration: underline;">{{ $loRow->lo_name_as_per_cnic ?? '' }}</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Address: <span style="text-decoration: underline;">{{ $loRow->address ?? '' }}</span></p>
                                </div>
                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Mobile Number: <span style="text-decoration: underline;">{{ $loRow->contact_no ?? '' }}</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Signature: ________________________</p>
                                </div>
                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Land Provider Name: <span style="text-decoration: underline;">{{ $land_p->lp_name ?? '' }}</span></p>
                                </div>
                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Mobile Number: <span style="text-decoration: underline;">{{ $land_p->contact_no ?? '' }}</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Signature: ________________________</p>
                                </div>
                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Possession Patwari: <span style="text-decoration: underline;">{{ $record->picto_name_of_patwari ?? '' }}</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Signature: ________________________ </p>
                                </div>
                                <div style="margin-top: 30px; margin-bottom: 30px;">
                                    <p style="font-weight: bold; margin-bottom: 10px;">Possession JCO: <span style="text-decoration: underline;">{{ $record->lp_possession_jpo ?? '' }}</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Signature: ________________________ </p>
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

</div>
<script>
    window.onload = function() {
        window.print();
    }
</script>