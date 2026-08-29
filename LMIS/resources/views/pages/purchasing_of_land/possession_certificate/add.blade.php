@extends('layouts.main')

@section('content')
<style>
    table {
        border-collapse: collapse;
        width: 100%;
        font-size: 12px;
    }

    th,
    td {
        border: 1px solid var(--lm-border);
        padding: 8px;
        text-align: left;
        font-size: 12px;
    }

    /* Improved form control styling for table inputs */
    #landDetailsBody input.form-control,
    #landDetailsBody select.form-control {
        padding: 1px 2px;
        font-size: 13px;
        min-height: 34px;
        width: 70px;
        box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
        transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
    }

    #landDetailsBody input.form-control:focus,
    #landDetailsBody select.form-control:focus {
        border-color: var(--lm-ink);
        outline: 0;
        box-shadow: 0 0 0 0.2rem var(--lm-focus-ring);
    }
</style>
<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="mb-9">
                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                            <div class="row g-3 justify-content-between align-items-center">
                                <div class="col-12 col-md">
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Possession Certificate</h4>
                                </div>

                            </div>
                        </div>
                        <div class="card-body p-0">

                            <div class="p-4 code-to-copy">
                                @if(session('status'))
                                <div class="alert alert-success mb-1 mt-1">
                                    {{ session('status') }}
                                </div>
                                @endif
                                <form class="row g-3 needs-validation" method="post" action="{{ route('possession_certificate.store') }}" novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">


                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_no">Doc No.</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" readonly value="{{$doc_num+1}}" required="" />
                                                    <div class="valid-feedback">Please Add Doc No.</div>
                                                    @error('doc_num')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_date">Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="date" type="text"
                                                        name="date" required="" readonly
                                                        value="{{$dt->format('Y-m-d')}}" />

                                                    <div class="valid-feedback">Please Add Doc Date</div>
                                                    @error('doc_date')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-4">
                                                    <label class="form-label" for="base_code_no">Land Form No</label>
                                                    <select id="base_code_no" name="base_code_no" class="form-control"
                                                        required="">
                                                        <option value="">Kindly Select</option>
                                                        @foreach($land_offer_form as $row)
                                                        <option value="{{ $row->doc_no }}">File No - {{ $row->doc_no }}</option>
                                                        @endforeach
                                                    </select>

                                                    <div class="invalid-feedback">Please select Land Form No.</div>
                                                    @error('base_code_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <!-- LO Information Section -->
                                                <div class="col-md-12">
                                                    <div class="card border border-300 bg-soft mt-3">
                                                        <div class="card-header bg-soft">
                                                            <h5 style="float: left;padding: 15px" class="mb-0">Land Owner Information</h5>
                                                        </div>
                                                        <div class="card-body" style="background-color: white">
                                                            <div class="row">
                                                                <table>
                                                                    <thead>
                                                                        <tr>
                                                                            <th>LO Name</th>
                                                                            <th>S/O</th>
                                                                            <th>LO CNIC</th>
                                                                            <th>Contact No</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody id="tbodyLoInfo">
                                                                        <!-- LO details will be populated here by JavaScript -->
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="col-md-12">

                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="mouza">Mouza/Chak</label>
                                                        <input class="form-control"
                                                            id="mouza"
                                                            name="mouza"
                                                            type="text"
                                                            value="{{ old('mouza') }}"
                                                            required="" />
                                                    </div>

                                                </div>

                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">

                                                        <div class="card-body">
                                                            <h5 class="card-title">Possession Detail</h5>

                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <label class="form-label" for="possession_date">Possession Date</label>
                                                                    <?php
                                                                    $dt = new DateTime();
                                                                    ?>
                                                                    <input class="form-control" id="possession_date" type="date"
                                                                        name="possession_date" required=""
                                                                        value="{{$dt->format('Y-m-d')}}" />

                                                                    @error('possession_date')
                                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <h6 class="card-title" style="margin-top: 20px; margin-bottom: 10px;">Land Details</h6>
                                                                    <div style="overflow-x: auto; -webkit-overflow-scrolling: touch;">
                                                                        <table class=" table-bordered table-sm" id="landDetailsTable" style="border: 1px solid var(--lm-border); margin-bottom: 0; min-width: 100%;">
                                                                            <thead style="background-color: var(--lm-surface);">
                                                                                <!-- Main Header Row -->
                                                                                <tr style="border: 1px solid var(--lm-border);">
                                                                                    <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Khewat No</th>
                                                                                    <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Khatooni No</th>
                                                                                    <!-- <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Block No</th>
                                                                                    <th rowspan="2" style="width:9%; border:1px solid var(--lm-border); padding:8px;">Rectangle No</th>
                                                                                    <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Khasra No</th> -->
                                                                                    <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Qatat</th>
                                                                                    <th rowspan="2" style="width:7%; border:1px solid var(--lm-border); padding:8px;">Sector</th>
                                                                                    <th rowspan="2" style="width:10%; border:1px solid var(--lm-border);">Land Category</th>

                                                                                    <!-- <th colspan="3" style="border:1px solid var(--lm-border); text-align:center;">Measuring</th> -->

                                                                                    <!-- <th rowspan="2" style="width:8%; border:1px solid var(--lm-border); text-align:center;">
                                                                                        Transfer Share
                                                                                    </th> -->

                                                                                    <th colspan="3" style="border:1px solid var(--lm-border); text-align:center;">Land Measuring</th>



                                                                                    <th colspan="3" style="border:1px solid var(--lm-border); text-align:center;">Possessed Land</th>
                                                                                    <th colspan="3" style="border:1px solid var(--lm-border); text-align:center;">Unpossessed Land</th>

                                                                                    <th rowspan="2" style="width:6%; border:1px solid var(--lm-border);">Action</th>
                                                                                </tr>

                                                                                <!-- Sub Header Row -->
                                                                                <tr style="border:1px solid var(--lm-border);">
                                                                                    <!-- Measuring -->
                                                                                    <!-- <th style="border:1px solid var(--lm-border);">Kanal</th>
                                                                                    <th style="border:1px solid var(--lm-border);">M</th>
                                                                                    <th style="border:1px solid var(--lm-border);">Sqft</th> -->

                                                                                    <!-- Land Measuring -->
                                                                                    <th style="border:1px solid var(--lm-border);">Kanal</th>
                                                                                    <th style="border:1px solid var(--lm-border);">M</th>
                                                                                    <th style="border:1px solid var(--lm-border);">Sqft</th>

                                                                                    <!-- Possessed Land -->
                                                                                    <th style="border:1px solid var(--lm-border);">Kanal</th>
                                                                                    <th style="border:1px solid var(--lm-border);">M</th>
                                                                                    <th style="border:1px solid var(--lm-border);">Sqft</th>

                                                                                    <!-- Unpossessed Land -->
                                                                                    <th style="border:1px solid var(--lm-border);">Kanal</th>
                                                                                    <th style="border:1px solid var(--lm-border);">M</th>
                                                                                    <th style="border:1px solid var(--lm-border);">Sqft</th>
                                                                                </tr>
                                                                            </thead>

                                                                            <tbody id="landDetailsBody">
                                                                                <tr id="1">
                                                                                    <td><input type="text" class="form-control" name="land_details[0][khewat_no]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control" name="land_details[0][khatooni_no]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control" name="land_details[0][qatat]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control" name="land_details[0][sector]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control" name="land_details[0][land_category]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control land_measuring_k" name="land_details[0][land_measuring_k]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control land_measuring_m" name="land_details[0][land_measuring_m]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control land_measuring_sqft" name="land_details[0][land_measuring_sqft]" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control possessed_k" name="land_details[0][possessed_k]" required="" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control possessed_m" name="land_details[0][possessed_m]" required="" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control possessed_sqft" name="land_details[0][possessed_sqft]" required="" style="border:none; border-radius:0;"></td>
                                                                                    <td><input type="text" class="form-control unpossessed_k" name="land_details[0][unpossessed_k]" style="border:none; border-radius:0;" readonly></td>
                                                                                    <td><input type="text" class="form-control unpossessed_m" name="land_details[0][unpossessed_m]" style="border:none; border-radius:0;" readonly></td>
                                                                                    <td><input type="text" class="form-control unpossessed_sqft" name="land_details[0][unpossessed_sqft]" style="border:none; border-radius:0;" readonly></td>
                                                                                    <td><button type="button" class="btn btn-sm btn-danger delete-row">Delete</button></td>
                                                                                </tr>
                                                                            </tbody>
                                                                            <tfoot>
                                                                                <tr style="background: var(--lm-surface);font-weight:bold">
                                                                                    <td colspan="5" style="text-align:right;border:1px solid var(--lm-border);">Total of Land Measuring, Possessed and Unpossessed Land</td>

                                                                                    <!-- Land Measuring Totals -->
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_land_kanal" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_land_marla" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_land_sqft" class="form-control" readonly>
                                                                                    </td>

                                                                                    <!-- Possessed Land Totals -->
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_poss_kanal" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_poss_marla" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_poss_sqft" class="form-control" readonly>
                                                                                    </td>

                                                                                    <!-- Unpossessed Land Totals -->
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_unposs_kanal" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_unposs_marla" class="form-control" readonly>
                                                                                    </td>
                                                                                    <td style="border:1px solid var(--lm-border);">
                                                                                        <input type="text" id="total_unposs_sqft" class="form-control" readonly>
                                                                                    </td>

                                                                                    <td></td>
                                                                                </tr>

                                                                            </tfoot>
                                                                        </table>
                                                                    </div>
                                                                    <!-- <button type="button" class="btn btn-sm btn-primary" id="addRowBtn" style="margin-top: 10px;"><i class="fas fa-plus"></i> Add Row</button> -->
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">

                                                        <div class="card-body">
                                                            <h5 class="card-title">LP Detail</h5>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <label class="form-label" for="lp_name">LP Name</label>
                                                                    <select id="lp_name" name="lp_name" class="form-control"
                                                                        >
                                                                        <option value="">Kindly Select</option>
                                                                        @foreach($land_provider as $row)
                                                                        <option value="{{ $row->lp_cod }}">{{ $row->lp_name }}</option>
                                                                        @endforeach
                                                                    </select>

                                                                    <div class="invalid-feedback">Please add LP Name.</div>
                                                                    @error('lp_name')
                                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>

                                                                <div class="col-md-6">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="lp_contact_no">Contact No</label>
                                                                        <input class="form-control"
                                                                            id="lp_contact_no"
                                                                            name="lp_contact_no"
                                                                            type="text" min="0"
                                                                            value="{{ old('lp_contact_no') }}"
                                                                            />
                                                                    </div>

                                                                </div>

                                                                <div class="col-md-6">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="lp_rep_name">LP Rep Name</label>
                                                                        <input class="form-control"
                                                                            id="lp_rep_name"
                                                                            name="lp_rep_name"
                                                                            type="text"
                                                                            value="{{ old('lp_rep_name') }}"
                                                                            required="" />
                                                                    </div>

                                                                </div>

                                                                <div class="col-md-6">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="lp_possession_jpo">Possession JCO</label>
                                                                        <input class="form-control"
                                                                            id="lp_possession_jpo"
                                                                            name="lp_possession_jpo"
                                                                            type="text"
                                                                            value="{{ old('lp_possession_jpo') }}"
                                                                            required="" />
                                                                    </div>

                                                                </div>


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">

                                                        <div class="card-body">
                                                            <h5 class="card-title">Attachments</h5>

                                                            <div class="row">

                                                                <div class="col-md-6">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="attachment_possession_certificate">Possession Certificate</label>
                                                                        <input class="form-control"
                                                                            id="attachment_possession_certificate"
                                                                            name="attachment_possession_certificate[]"
                                                                            type="file"
                                                                            multiple
                                                                            value="{{ old('attachment_possession_certificate') }}" />
                                                                    </div>

                                                                </div>
                                                                <div class="col-md-6">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="attachment">Other</label>
                                                                        <input class="form-control"
                                                                            id="attachment"
                                                                            name="attachment[]"
                                                                            type="file"
                                                                            multiple
                                                                            value="{{ old('attachment') }}" />
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" style="margin-top: 20px">
                                                            <div class="card">

                                                                <div class="card-body">
                                                                    <h5 class="card-title">Pictorial View</h5>
                                                                    <div class="row">

                                                                        <div class="col-md-6">
                                                                            <label class="form-label" for="picto_lo_name">LO Name</label>
                                                                            <input class="form-control" id="picto_lo_name" type="text" name="picto_lo_name" value="{{ old('picto_lo_name') }}" required="" />
                                                                            <div class="invalid-feedback">Please add Amount.</div>
                                                                            @error('picto_lo_name')
                                                                            <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                            @enderror
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label class="form-label" for="picto_lp_name">LP Name</label>
                                                                            <input class="form-control" id="picto_lp_name" type="text" name="picto_lp_name" value="{{ old('picto_lp_name') }}" required="" />
                                                                            <div class="invalid-feedback">Please add Amount.</div>
                                                                            @error('picto_lp_name')
                                                                            <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                            @enderror
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label class="form-label" for="picto_name_of_patwari">Name Of Patwari</label>
                                                                            <input class="form-control" id="picto_name_of_patwari" type="text" name="picto_name_of_patwari" value="{{ old('picto_name_of_patwari') }}" required="" />
                                                                            <div class="invalid-feedback">Please add Amount.</div>
                                                                            @error('picto_name_of_patwari')
                                                                            <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                            @enderror
                                                                        </div>

                                                                        <div class="col-md-6">
                                                                            <label class="form-label" for="picto_possession_jco">Possession JCO</label>
                                                                            <input class="form-control" id="picto_possession_jco" type="text" name="picto_possession_jco" value="{{ old('picto_possession_jco') }}" required="" />
                                                                            <div class="invalid-feedback">Please add Amount.</div>
                                                                            @error('picto_possession_jco')
                                                                            <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                            @enderror
                                                                        </div>



                                                                        <div class="col-md-12">

                                                                            <div class="mb-3">
                                                                                <label class="form-label"
                                                                                    for="picto_picture">Attachment</label>
                                                                                <input class="form-control"
                                                                                    id="picto_picture"
                                                                                    name="picto_picture"
                                                                                    type="file"
                                                                                    value="{{ old('picto_picture') }}"
                                                                                    required="" />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>

                                    <!-- Hidden fields for totals -->
                                    <input type="hidden" id="hidden_total_land_kanal" name="total_land_kanal">
                                    <input type="hidden" id="hidden_total_land_marla" name="total_land_marla">
                                    <input type="hidden" id="hidden_total_land_sqft" name="total_land_sqft">
                                    <input type="hidden" id="hidden_total_land_acres" name="total_land_acres">
                                    <input type="hidden" id="hidden_total_poss_kanal" name="total_poss_kanal">
                                    <input type="hidden" id="hidden_total_poss_marla" name="total_poss_marla">
                                    <input type="hidden" id="hidden_total_poss_sqft" name="total_poss_sqft">
                                    <input type="hidden" id="hidden_total_poss_acres" name="total_poss_acres">
                                    <input type="hidden" id="hidden_total_unposs_kanal" name="total_unposs_kanal">
                                    <input type="hidden" id="hidden_total_unposs_marla" name="total_unposs_marla">
                                    <input type="hidden" id="hidden_total_unposs_sqft" name="total_unposs_sqft">
                                    <input type="hidden" id="hidden_total_unposs_acres" name="total_unposs_acres">

                                    <div class="col-12">
                                        <button class="btn btn-primary" type="submit">Submit form</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 5">
        <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body p-3"></div><button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    </div>
    <footer class="footer position-absolute">
        <div class="row g-0 justify-content-between align-items-center h-100">
            <div class="col-12 col-sm-auto text-center">
                <p class="mb-0 mt-2 mt-sm-0 lm-footer-text"><span class="lm-footer-brand">Land Information Management System</span><span class="lm-footer-sep">|</span><span>&copy; {{ date('Y') }}</span><span class="lm-footer-sep">|</span><span>Powered by <img src="{{ asset('public/assets/img/n-stack-logo.png') }}" alt="" class="lm-footer-logo"> <strong>N-Stack</strong></span></p>
            </div>
            <div class="col-12 col-sm-auto text-center">
            </div>
        </div>
    </footer>
</div>

<script>
    $('#base_code_no').change(function() {
        var selectedValue = $(this).val();

        $.ajax({
            url: baseUrl + '/get_land_form',
            type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
            data: JSON.stringify({
                "_token": "{{ csrf_token() }}",
                value: selectedValue
            }), // You can send data to the server if required
            contentType: 'application/json', // Set the appropriate content type
            success: function(data) {
                $('#mouza').val(data.mouza);

                $('#mouza').prop('readonly', true);

            },
            error: function(error) {
                // Handle any errors that occurred during the AJAX call
                console.error('Error:', error);
            }
        });

    });

    $('#lp_name').change(function() {
        var selectedValue = $(this).val();

        $.ajax({
            url: baseUrl + '/get_land_provider',
            type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
            data: JSON.stringify({
                "_token": "{{ csrf_token() }}",
                value: selectedValue
            }), // You can send data to the server if required
            contentType: 'application/json', // Set the appropriate content type
            success: function(data) {

                $('#lp_contact_no').val(data.contact_no);


                $('#lp_contact_no').prop('readonly', true);

                // Do something with the data (e.g., update content on the page)
            },
            error: function(error) {
                // Handle any errors that occurred during the AJAX call
                console.error('Error:', error);
            }
        });
    });
</script>
<script>
    // Land Details Table - Add/Remove Row functionality
    let landRowCount = 1;

    document.getElementById('addRowBtn').addEventListener('click', function() {
        landRowCount++;

        const table = document.getElementById('landDetailsBody');
        const newRow = document.createElement('tr');

        newRow.innerHTML = `
            <!-- Basic Info (6) -->
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][khewat_no]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][khatooni_no]" value="" style="border:none;"></td>
            <!--
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][block_no]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][rectangle_no]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][khasra_no]" value="" style="border:none;"></td>
            -->
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][qatat]" value="" style="border:none;"></td>
                <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][sector]" value="" style="border:none;"></td>
                 <!-- Land Category (1) -->
            <td style="border:1px solid var(--lm-border);padding:0;">
                <select class="form-control" name="land_details[${landRowCount}][land_category]" style="border:none;">
                    <option value="">Select</option>
                    <option value="Ownership">Ownership</option>
                    <option value="Non Pata">Non Pata</option>
                    <option value="Govt Land">Govt Land</option>
                </select>
            </td>

            <!-- Measuring (3) 
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][measuring_k]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][measuring_m]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][measuring_sqft]" value="" style="border:none;"></td>
-->
            <!-- Transfer Share (1) 
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][transfer_share]" value="" style="border:none;"></td>
            -->

            <!-- Land Measuring (3) -->
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][land_measuring_k]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][land_measuring_m]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][land_measuring_sqft]" value="" style="border:none;"></td>

           

            <!-- Possessed Land (3) -->
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][possessed_k]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][possessed_m]" value="" style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][possessed_sqft]" value="" style="border:none;"></td>

            <!-- Unpossessed Land (3) -->
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][unpossessed_k]" value="" readonly style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][unpossessed_m]" value="" readonly style="border:none;"></td>
            <td style="border:1px solid var(--lm-border);padding:0;"><input type="text" class="form-control" name="land_details[${landRowCount}][unpossessed_sqft]" value="" readonly style="border:none;"></td>

            <!-- Action (1) -->
            <td style="border:1px solid var(--lm-border);padding:0;">
                <button type="button" class="btn btn-sm btn-danger removeRow" style="width:100%;border-radius:0;">Remove</button>
            </td>
        `;

        table.appendChild(newRow);
        updateRemoveButtons();
    });

    function updateRemoveButtons() {
        const rows = document.querySelectorAll('#landDetailsBody tr');
        document.querySelectorAll('.removeRow').forEach(btn => {
            btn.style.display = rows.length > 1 ? 'block' : 'none';
        });
    }

    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('removeRow')) {
            e.preventDefault();
            e.target.closest('tr').remove();
            updateRemoveButtons();
        }
    });

    // Initial state
    updateRemoveButtons();
</script>

<!-- Fetch LO Information when Land Form No changes -->
<script>
    $(document).ready(function() {
        // Fetch LO details when base_code_no changes
        $('#base_code_no').on('change', function() {
            var docNo = $(this).val();
            var tbodyLoInfo = $('#tbodyLoInfo');

            // Clear the table
            tbodyLoInfo.html('');

            if (docNo === '') {
                return;
            }

            // Fetch LO details from API
            $.ajax({
                url: "{{ url('/get-lo-details') }}/" + docNo,
                type: "GET",
                dataType: "json",
                success: function(response) {
                    if (response.success && response.data.length > 0) {
                        // Populate table rows with LO data
                        response.data.forEach(function(lo, index) {
                            var row = `<tr>
                                <td>
                                    <input type="text" class="form-control" value="${lo.lo_name_as_per_cnic || ''}" readonly>
                                    <input type="hidden" name="lo_name[]" value="${lo.lo_name_as_per_cnic || ''}">
                                </td>
                                <td>
                                    <input type="text" class="form-control" value="${lo.father_name_cnic || ''}" readonly>
                                    <input type="hidden" name="so[]" value="${lo.father_name_cnic || ''}">
                                </td>
                                <td>
                                    <input type="text" class="form-control" value="${lo.lo_cnic || ''}" readonly>
                                    <input type="hidden" name="lo_cnic[]" value="${lo.lo_cnic || ''}">
                                </td>
                                <td>
                                    <input type="text" class="form-control" value="${lo.contact_no || ''}" readonly>
                                    <input type="hidden" name="contact_no[]" value="${lo.contact_no || ''}">
                                </td>
                            </tr>`;
                            tbodyLoInfo.append(row);
                        });
                    } else {
                        tbodyLoInfo.html('<tr><td colspan="4" style="text-align: center;">No Land Owner data found</td></tr>');
                    }
                },
                error: function() {
                    tbodyLoInfo.html('<tr><td colspan="4" style="text-align: center; color: red;">Error fetching Land Owner data</td></tr>');
                }
            });
        });

        // Trigger on page load if base_code_no is already selected (for edit form)
        if ($('#base_code_no').val()) {
            $('#base_code_no').trigger('change');
        }
    });
</script>
<!-- Fetch Land Details when Land Form No changes -->
<script>
    $(document).ready(function() {

        $('#base_code_no').on('change', function() {
            var docNo = $(this).val();

            if (docNo === '') {
                $('#landDetailsTable tbody').empty();
                return;
            }

            $.ajax({
                url: "{{ url('/get-land-details') }}/" + docNo,
                type: "GET",
                dataType: "json",
                success: function(response) {

                    $('#landDetailsTable tbody').empty();

                    if (response.success && response.data.length > 0) {

                        response.data.forEach(function(land, index) {

                            let rowData = land.land_details ?? land;

                            var row = `
                        <tr>
                            <!-- Basic Info (6) -->
                            <td><input type="text" class="form-control" name="land_details[${index}][khewat_no]" value="${rowData.khewat_no ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][khatooni_no]" value="${rowData.khatooni_no ?? ''}" readonly></td>
                            <!--
                            <td><input type="text" class="form-control" name="land_details[${index}][block_no]" value="${rowData.block_no ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][rectangle_no]" value="${rowData.rectangle_no ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][khasra_no]" value="${rowData.khasra_no ?? ''}" readonly></td>
                            -->
                            <td><input type="text" class="form-control" name="land_details[${index}][qatat]" value="${rowData.qatat ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][sector]" value="${rowData.sector ?? ''}"></td>
                            <!-- Land Category -->
                            <td>
                                <select class="form-control readonly-select" name="land_details[${index}][land_category]">
                                    <option value="">Select</option>
                                    <option value="Ownership" ${rowData.land_category === 'Ownership' ? 'selected' : ''}>Ownership</option>
                                    <option value="Non Pata" ${rowData.land_category === 'Non Pata' ? 'selected' : ''}>Non Pata</option>
                                    <option value="Govt Land" ${rowData.land_category === 'Govt Land' ? 'selected' : ''}>Govt Land</option>
                                </select>
                            </td>

                            <!-- Measuring (3) 
                            <td><input type="text" class="form-control" name="land_details[${index}][measuring_k]" value="${rowData.measuring_k ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][measuring_m]" value="${rowData.measuring_m ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][measuring_sqft]" value="${rowData.measuring_sqft ?? ''}" readonly></td>
-->
                            <!-- Transfer Share 
                            <td><input type="text" class="form-control" name="land_details[${index}][transfer_share]" value="${rowData.transfer_share ?? ''}"></td>
                                -->

                            <!-- Land Measuring (3) -->
                            <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_k]" value="${rowData.land_measuring_k ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_m]" value="${rowData.land_measuring_m ?? ''}" readonly></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_sqft]" value="${rowData.land_measuring_sqft ?? ''}" readonly></td>

                            

                            <!-- Possessed Land (MANUAL) -->
                            <td><input type="text" class="form-control" name="land_details[${index}][possessed_k]" required="" value=""></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][possessed_m]" required="" value=""></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][possessed_sqft]" required="" value=""></td>

                            <!-- Unpossessed Land (MANUAL) -->
                            <td><input type="text" class="form-control" name="land_details[${index}][unpossessed_k]" readonly value=""></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][unpossessed_m]" readonly value=""></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][unpossessed_sqft]" readonly value=""></td>

                            <!-- Action -->
                            <td>
                                <button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)">Remove</button>
                            </td>
                        </tr>
                        `;

                            $('#landDetailsTable tbody').append(row);
                        });
                    }
                },
                error: function() {
                    alert('Error fetching Land details');
                }
            });
        });

    });

    // Remove row function
    function deleteRow(btn) {
        $(btn).closest('tr').remove();
    }
</script>

<!-- Enhanced auto calculate with Acre support -->
<script>
    $(document).ready(function() {

        // ============ CONVERSION CONSTANTS ============
        const SQFT_PER_MARLA = 272; // 1 Marla = 272 sqft
        const MARLA_PER_KANAL = 20; // 1 Kanal = 20 Marla
        const SQFT_PER_KANAL = 5445; // 1 Kanal = 5445 sqft
        const SQFT_PER_ACRE = 43560; // 1 Acre = 43560 sqft

        // ============ CONVERSION FUNCTIONS ============
        /**
         * Convert Kanal, Marla, Sqft to total Sqft
         * @param {number} kanal
         * @param {number} marla
         * @param {number} sqft
         * @returns {number} Total square feet
         */
        function convertToSqft(kanal, marla, sqft) {
            kanal = parseFloat(kanal) || 0;
            marla = parseFloat(marla) || 0;
            sqft = parseFloat(sqft) || 0;
            return (kanal * SQFT_PER_KANAL) + (marla * SQFT_PER_MARLA) + sqft;
        }

        /**
         * Convert total Sqft to Kanal, Marla, Sqft
         * @param {number} totalSqft
         * @returns {object} {k: kanal, m: marla, s: sqft}
         */
        function convertFromSqft(totalSqft) {
            if (totalSqft < 0) {
                totalSqft = 0;
            }

            let k = Math.floor(totalSqft / SQFT_PER_KANAL);
            totalSqft = totalSqft % SQFT_PER_KANAL;

            let m = Math.floor(totalSqft / SQFT_PER_MARLA);
            totalSqft = (totalSqft % SQFT_PER_MARLA).toFixed(2);

            return {
                k: k,
                m: m,
                s: parseFloat(totalSqft)
            };
        }

        /**
         * Convert Sqft to Acres
         * @param {number} sqft
         * @returns {number} Acres (with 2 decimal places)
         */
        function convertToAcres(sqft) {
            return (sqft / SQFT_PER_ACRE).toFixed(4);
        }

        /**
         * Calculate unpossessed land for a row
         * Unpossessed = Land Measuring - Possessed
         * @param {jQuery} $row - jQuery row element
         */
        function calculateRowUnpossessed($row) {
            // Get Land Measuring values
            const total_k = $row.find('input[name*="[land_measuring_k]"]').val();
            const total_m = $row.find('input[name*="[land_measuring_m]"]').val();
            const total_s = $row.find('input[name*="[land_measuring_sqft]"]').val();

            // Get Possessed values
            const poss_k = $row.find('input[name*="[possessed_k]"]').val();
            const poss_m = $row.find('input[name*="[possessed_m]"]').val();
            const poss_s = $row.find('input[name*="[possessed_sqft]"]').val();

            // Convert to Sqft and calculate difference
            const totalSqft = convertToSqft(total_k, total_m, total_s);
            const possessedSqft = convertToSqft(poss_k, poss_m, poss_s);
            const unpossessedSqft = Math.max(0, totalSqft - possessedSqft);

            // Convert back to K/M/Sqft
            const result = convertFromSqft(unpossessedSqft);

            // Update unpossessed fields
            $row.find('input[name*="[unpossessed_k]"]').val(result.k);
            $row.find('input[name*="[unpossessed_m]"]').val(result.m);
            $row.find('input[name*="[unpossessed_sqft]"]').val(result.s);
        }

        /**
         * Calculate column totals and acre values
         */
        function calculateColumnTotals() {
            let landKanal = 0,
                landMarla = 0,
                landSqft = 0;
            let possKanal = 0,
                possMarla = 0,
                possSqft = 0;
            let unpossKanal = 0,
                unpossMarla = 0,
                unpossSqft = 0;

            // Sum all rows
            $('#landDetailsBody tr').each(function() {
                // Land Measuring totals
                let lk = parseFloat($(this).find('input[name*="[land_measuring_k]"]').val()) || 0;
                let lm = parseFloat($(this).find('input[name*="[land_measuring_m]"]').val()) || 0;
                let ls = parseFloat($(this).find('input[name*="[land_measuring_sqft]"]').val()) || 0;

                // Possessed totals
                let pk = parseFloat($(this).find('input[name*="[possessed_k]"]').val()) || 0;
                let pm = parseFloat($(this).find('input[name*="[possessed_m]"]').val()) || 0;
                let ps = parseFloat($(this).find('input[name*="[possessed_sqft]"]').val()) || 0;

                // Unpossessed totals
                let uk = parseFloat($(this).find('input[name*="[unpossessed_k]"]').val()) || 0;
                let um = parseFloat($(this).find('input[name*="[unpossessed_m]"]').val()) || 0;
                let us = parseFloat($(this).find('input[name*="[unpossessed_sqft]"]').val()) || 0;

                landKanal += lk;
                landMarla += lm;
                landSqft += ls;

                possKanal += pk;
                possMarla += pm;
                possSqft += ps;

                unpossKanal += uk;
                unpossMarla += um;
                unpossSqft += us;
            });

            // Normalize values (convert excess sqft to marla, excess marla to kanal)
            function normalizeValues(kanal, marla, sqft) {
                // Convert excess sqft to marla
                if (sqft >= SQFT_PER_MARLA) {
                    let extraMarla = Math.floor(sqft / SQFT_PER_MARLA);
                    marla += extraMarla;
                    sqft = (sqft % SQFT_PER_MARLA).toFixed(2);
                }
                // Convert excess marla to kanal
                if (marla >= MARLA_PER_KANAL) {
                    let extraKanal = Math.floor(marla / MARLA_PER_KANAL);
                    kanal += extraKanal;
                    marla = marla % MARLA_PER_KANAL;
                }
                return {
                    kanal,
                    marla,
                    sqft
                };
            }

            // Normalize all three land types
            let landNorm = normalizeValues(landKanal, landMarla, landSqft);
            let possNorm = normalizeValues(possKanal, possMarla, possSqft);
            let unpossNorm = normalizeValues(unpossKanal, unpossMarla, unpossSqft);

            // Calculate acres from total sqft
            const landAcres = convertToAcres(convertToSqft(landKanal, landMarla, landSqft));
            const possAcres = convertToAcres(convertToSqft(possKanal, possMarla, possSqft));
            const unpossAcres = convertToAcres(convertToSqft(unpossKanal, unpossMarla, unpossSqft));

            // Update display fields
            $('#total_land_kanal').val(landNorm.kanal);
            $('#total_land_marla').val(landNorm.marla);
            $('#total_land_sqft').val(landNorm.sqft);

            $('#total_poss_kanal').val(possNorm.kanal);
            $('#total_poss_marla').val(possNorm.marla);
            $('#total_poss_sqft').val(possNorm.sqft);

            $('#total_unposs_kanal').val(unpossNorm.kanal);
            $('#total_unposs_marla').val(unpossNorm.marla);
            $('#total_unposs_sqft').val(unpossNorm.sqft);

            // Update acres display fields
            $('#total_land_acres').val(landAcres);
            $('#total_poss_acres').val(possAcres);
            $('#total_unposs_acres').val(unpossAcres);

            // Update hidden fields for form submission
            $('#hidden_total_land_kanal').val(landNorm.kanal);
            $('#hidden_total_land_marla').val(landNorm.marla);
            $('#hidden_total_land_sqft').val(landNorm.sqft);
            $('#hidden_total_land_acres').val(landAcres);

            $('#hidden_total_poss_kanal').val(possNorm.kanal);
            $('#hidden_total_poss_marla').val(possNorm.marla);
            $('#hidden_total_poss_sqft').val(possNorm.sqft);
            $('#hidden_total_poss_acres').val(possAcres);

            $('#hidden_total_unposs_kanal').val(unpossNorm.kanal);
            $('#hidden_total_unposs_marla').val(unpossNorm.marla);
            $('#hidden_total_unposs_sqft').val(unpossNorm.sqft);
            $('#hidden_total_unposs_acres').val(unpossAcres);
        }

        // ============ EVENT HANDLERS ============
        // Trigger calculation when any land measurement input changes
        $(document).on('input',
            'input[name*="[possessed_k]"], ' +
            'input[name*="[possessed_m]"], ' +
            'input[name*="[possessed_sqft]"], ' +
            'input[name*="[land_measuring_k]"], ' +
            'input[name*="[land_measuring_m]"], ' +
            'input[name*="[land_measuring_sqft]"]',
            function() {
                const $row = $(this).closest('tr');
                calculateRowUnpossessed($row);
                calculateColumnTotals();
            }
        );

    });
</script>





@endsection