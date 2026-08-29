@extends('layouts/main')

@section('content')
<style>
    th {
        border: 1px solid var(--lm-border) !important;
        text-align: center;
        background-color: var(--lm-surface);
        border: 1px solid var(--lm-border);
        padding: 8px;
    }

    td {
        border: 1px solid var(--lm-border) !important;
        width: 130px;

    }

    .row-level {
        border: none;
        width: 130px;
    }

    input.row-level:focus {
        outline: none;
        /* Remove the default focus outline */
        border: none;
        /* Remove the border */
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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Conveyance Deed</h4>
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
                                <form class="row g-3 needs-validation" method="post" action="{{ route('conveyance.store') }}" novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label class="form-label" for="doc_no">Doc No.</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" value="{{$doc_no+1}}" readonly required="" />
                                                    <div class="valid-feedback">Please Add Doc No..</div>
                                                    @error('doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label" for="date"> Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="date" type="date"
                                                        name="date" required=""
                                                        value="{{$dt->format('Y-m-d')}}" />

                                                    <div class="valid-feedback">Please Add Doc Date</div>
                                                    @error('date')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="date_of_creation"> Date of Creation</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="date_of_creation" type="date_of_creation"
                                                        name="date_of_creation" required=""
                                                        value="{{$dt->format('Y-m-d')}}" />

                                                    <div class="valid-feedback">Please Add Date of Creation</div>
                                                    @error('date_of_creation')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label" for="base_doc_no">Purchase of Land</label>
                                                    <select id="base_doc_no" name="base_doc_no" class="form-control"
                                                        required="">
                                                        <option value="">Kindly Select</option>
                                                        @foreach($purchase_of_land as $row)
                                                        <option value="{{ $row->File_No }}">File No - {{ $row->File_No }}</option>
                                                        @endforeach
                                                    </select>

                                                    <div class="invalid-feedback">Please select Purchase of Land.</div>
                                                    @error('base_doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <!-- LO Information Section -->
                                                <div class="col-md-12">
                                                    <div class="card border border-300 bg-soft mt-3">
                                                        <div class="card-header bg-soft">
                                                            <h5 style="float: left;" class="mb-0">Deed executed By</h5>
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

                                                            <div class="row">
                                                                <h5 class="card-title mt-3">Witness of Vendor</h5>

                                                                <div class="col-md-4">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_executed_by_lo_name">Name</label>
                                                                        <input class="form-control"
                                                                            id="deed_executed_by_lo_name"
                                                                            name="deed_executed_by_lo_name"
                                                                            type="text"
                                                                            value="{{ old('deed_executed_by_lo_name') }}"
                                                                            required="" />
                                                                    </div>

                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label class="form-label">Relationship</label>
                                                                    <select class="form-control" name="vendor_relationship" required="">
                                                                        <option value="">Select</option>
                                                                        <option value="S/O">S/O</option>
                                                                        <option value="W/O">W/O</option>
                                                                        <option value="D/O">D/O</option>
                                                                        <option value="Widow of">Widow of</option>
                                                                    </select>
                                                                    <div class="invalid-feedback">Please select Relationship.</div>

                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_executed_by_lo_father_name">Father / Husband Name</label>
                                                                        <input class="form-control"
                                                                            id="deed_executed_by_lo_father_name"
                                                                            name="deed_executed_by_lo_father_name"
                                                                            type="text"
                                                                            value="{{ old('deed_executed_by_lo_father_name') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_executed_by_cnic">CNIC NO</label>
                                                                        <input class="form-control"
                                                                            id="deed_executed_by_cnic"
                                                                            name="deed_executed_by_cnic"
                                                                            type="text"
                                                                            value="{{ old('deed_executed_by_cnic') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_executed_by_caste">Caste</label>
                                                                        <input class="form-control"
                                                                            id="deed_executed_by_caste"
                                                                            name="deed_executed_by_caste"
                                                                            type="text"
                                                                            value="{{ old('deed_executed_by_caste') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_executed_by_address">Address</label>
                                                                        <input class="form-control"
                                                                            id="deed_executed_by_address"
                                                                            name="deed_executed_by_address"
                                                                            type="text"
                                                                            value="{{ old('deed_executed_by_address') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="district">District</label>
                                                        <input class="form-control"
                                                            id="district"
                                                            name="district"
                                                            type="text"
                                                            value="{{ old('district', 'Bahawalpur') }}"
                                                             />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="tehsil">Tehsil</label>
                                                        <input class="form-control"
                                                            id="tehsil"
                                                            name="tehsil"
                                                            type="text"
                                                            value="{{ old('tehsil', 'Bahawalpur') }}"
                                                             />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">

                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="scheme">scheme</label>
                                                        <select id="scheme" name="scheme" class="form-control"
                                                            required="">
                                                            <option value="">Kindly Select</option>
                                                            <option value="Exemption Scheme">Exemption Scheme</option>
                                                            <option value="Other">Other</option>
                                                        </select>

                                                        <div class="invalid-feedback">Please select Scheme.</div>
                                                        @error('scheme')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                </div>
                                                <div class="col-md-4">

                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="fixed_deed_rs">fixed Deed RS</label>
                                                        <input class="form-control"
                                                            id="fixed_deed_rs"
                                                            name="fixed_deed_rs"
                                                            type="text"
                                                            value="{{ old('fixed_deed_rs') }}"
                                                            required="" />
                                                    </div>

                                                </div>

                                                <div class="col-md-4">
                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="stamp_paper_value">Stamp Paper Value</label>
                                                        <input class="form-control"
                                                            id="stamp_paper_value"
                                                            name="stamp_paper_value"
                                                            type="text"
                                                            value="{{ old('stamp_paper_value') }}"
                                                            required="" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="schedule_year">Schedule Year</label>
                                                        <input class="form-control"
                                                            id="schedule_year"
                                                            name="schedule_year"
                                                            type="text"
                                                            value="{{ old('schedule_year') }}"
                                                            required="" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4">
                                                    <div class="mb-3">
                                                        <label class="form-label"
                                                            for="record_of_rights_year">Record of Rights Year</label>
                                                        <input class="form-control"
                                                            id="record_of_rights_year"
                                                            name="record_of_rights_year"
                                                            type="text"
                                                            value="{{ old('record_of_rights_year') }}"
                                                            required="" />
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

                                                                        <th>Block No</th>
                                                                        <th>Rectangle No</th>
                                                                        <th>Khasra No</th>





                                                                        <th>East By</th>
                                                                        <th>West By</th>
                                                                        <th>North By</th>
                                                                        <th>South By</th>



                                                                        <th>Action</th>
                                                                    </tr>


                                                                </thead>

                                                                <tbody id="landDetailsBody">
                                                                    <tr id="1">
                                                                        <td><input type="text" class="row-level" name="land_details[1][block_no]" value="{{ old('land_details[1][block_no]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][rectangle_no]" value="{{ old('land_details[1][rectangle_no]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][khasra_no]" value="{{ old('land_details[1][khasra_no]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][east_by]" value="{{ old('land_details[1][east_by]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][west_by]" value="{{ old('land_details[1][west_by]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][north_by]" value="{{ old('land_details[1][north_by]') }}"> </td>
                                                                        <td><input type="text" class="row-level" name="land_details[1][south_by]" value="{{ old('land_details[1][south_by]') }}"> </td>
                                                                        <td><button type="button" class="btn btn-sm btn-danger delete-row">Delete</button> </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <button type="button" class="btn btn-sm btn-success" id="add_row_land" style="margin-top: 10px;">Add Row</button>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">

                                                        <div class="card-body">
                                                            <!-- <p class="card-title btn btn-success" id="add_row_fard">Add Row</p> -->
                                                            <div class="row">

                                                                <table>
                                                                    <thead>
                                                                        <tr>
                                                                            <!-- <th>Vide Fard ID NO</th> -->
                                                                            <th>Fard ID 1</th>
                                                                            <th>Date</th>
                                                                            <th>Fard ID 2</th>
                                                                            <th>Date 2</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody id="tbodyrowfard">
                                                                        <tr id="1">
                                                                            <td><input type="text" class="form-control" id="vide_fad_id_no_1" name="fard_item_lines[1][vide_fad_id_no]" value="{{ old('item_lines[1][vide_fad_id_no]') }}"></td>
                                                                            <td><input type="date" class="form-control" id="date_1" name="fard_item_lines[1][date]" value="{{ old('item_lines[1][date]') }}"></td>
                                                                            <td><input type="text" class="form-control" id="vide_fad_id_no_2" name="fard_item_lines[1][vide_fad_id_no_2]" value="{{ old('item_lines[1][vide_fad_id_no_2]') }}"></td>
                                                                            <td><input type="date" class="form-control" id="date_2" name="fard_item_lines[1][date_2]" value="{{ old('item_lines[1][date_2]') }}"></td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">

                                                        <div class="card-body">

                                                            <h5 class="card-title">Deed in Favor Of</h5>
                                                            <div class="row">

                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="deed_in_favor_of_name">Name</label>
                                                                        <input class="form-control"
                                                                            id="deed_in_favor_of_name"
                                                                            name="deed_in_favor_of_name"
                                                                            type="text"
                                                                            value="Brigadier Syed Asher Mehboob, (Retired)"
                                                                            required="" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="rep_cnic">CNIC </label>
                                                                        <input class="form-control"
                                                                            id="rep_cnic"
                                                                            name="rep_cnic"
                                                                            type="text"
                                                                            value="32203-2091751-3"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <h5 class="card-title mt-3">Witness of Vendee</h5>

                                                                <div class="col-md-4">

                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="vendee_witness_name">Name</label>
                                                                        <input class="form-control"
                                                                            id="vendee_witness_name"
                                                                            name="vendee_witness_name"
                                                                            type="text"
                                                                            value="{{ old('vendee_witness_name') }}"
                                                                            required="" />
                                                                    </div>

                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label class="form-label">Relationship</label>
                                                                    <select class="form-control" name="vendee_relationship" required="">
                                                                        <option value="">Select</option>
                                                                        <option value="S/O">S/O</option>
                                                                        <option value="W/O">W/O</option>
                                                                        <option value="D/O">D/O</option>
                                                                        <option value="Widow of">Widow of</option>
                                                                    </select>
                                                                    <div class="invalid-feedback">Please select Relationship.</div>

                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="vendee_witness_father_name">Father / Husband Name</label>
                                                                        <input class="form-control"
                                                                            id="vendee_witness_father_name"
                                                                            name="vendee_witness_father_name"
                                                                            type="text"
                                                                            value="{{ old('vendee_witness_father_name') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="vendee_witness_cnic">CNIC NO</label>
                                                                        <input class="form-control"
                                                                            id="vendee_witness_cnic"
                                                                            name="vendee_witness_cnic"
                                                                            type="text"
                                                                            value="{{ old('vendee_witness_cnic') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="vendee_witness_caste">Caste</label>
                                                                        <input class="form-control"
                                                                            id="vendee_witness_caste"
                                                                            name="vendee_witness_caste"
                                                                            type="text"
                                                                            value="{{ old('vendee_witness_caste') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="vendee_witness_address">Address</label>
                                                                        <input class="form-control"
                                                                            id="vendee_witness_address"
                                                                            name="vendee_witness_address"
                                                                            type="text"
                                                                            value="{{ old('vendee_witness_address') }}"
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
<input type="hidden" id="rownumber" value="1">
<input type="hidden" id="rownumber_land" value="1">
<input type="hidden" id="rownumber_transfered" value="1">
<input type="hidden" id="rownumber_fard" value="1">
<!-- Fetch LO Information when Land Form No changes -->
<script>
    $(document).ready(function() {
        // Fetch LO details when base_doc_no changes
        $('#base_doc_no').on('change', function() {
            var docNo = $(this).val();
            var tbodyLoInfo = $('#tbodyLoInfo');

            // Clear the table
            tbodyLoInfo.html('');

            if (docNo === '') {
                return;
            }

            // Fetch LO details from API
            $.ajax({
                url: "{{ url('/get-purchase_lo-details') }}/" + docNo,
                type: "GET",
                dataType: "json",
                success: function(response) {
                    if (response.success && response.data.length > 0) {
                        // Populate table rows with LO data
                        response.data.forEach(function(lo, index) {
                            var row = `<tr>
                                <td>
                                    <input type="text" class="form-control" value="${lo.lo_name || ''}" readonly>
                                    <input type="hidden" name="lo_name[]" value="${lo.lo_name || ''}">
                                </td>
                                <td>
                                    <input type="text" class="form-control" value="${lo.so || ''}" readonly>
                                    <input type="hidden" name="so[]" value="${lo.so || ''}">
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

        // Trigger on page load if base_doc_no is already selected (for edit form)
        if ($('#base_doc_no').val()) {
            $('#base_doc_no').trigger('change');
        }
    });
</script>

<script>
    $('#base_doc_no').change(function() {
        var selectedValue = $(this).val();

        $.ajax({
            url: baseUrl + '/get_purchase_of_land',
            type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
            data: JSON.stringify({
                "_token": "{{ csrf_token() }}",
                value: selectedValue
            }), // You can send data to the server if required
            contentType: 'application/json', // Set the appropriate content type
            success: function(data) {

                console.log(data);

                $('#fixed_deed_rs').val(parseFloat(data.district_amount).toFixed(2));
                $('#vide_fad_id_no_1').val(data.fard_id);
                $('#date_1').val(data.fard_date);
                $('#vide_fad_id_no_2').val(data.fard_id2);
                $('#date_2').val(data.fard_date2);

                $('#fixed_deed_rs').prop('readonly', true);
                $('#vide_fad_id_no_1').prop('readonly', true);
                $('#date_1').prop('readonly', true);
                $('#vide_fad_id_no_2').prop('readonly', true);
                $('#date_2').prop('readonly', true);

            },
            error: function(error) {
                // Handle any errors that occurred during the AJAX call
                console.error('Error:', error);
            }
        });

    });
</script>

<script>
    $(function() {
        $('#add_row_fard').click(function() {
            var rownumber = parseFloat($("#rownumber_fard").val());
            var LineId = rownumber;
            rownumber = rownumber + 1;
            $("#rownumber_fard").val(rownumber);
            var row = '<tr id="' + rownumber + '" DetailId="0"> ' +
                '<td><input type="text" class="form-control" id="vide_fad_id_no_' + rownumber + '"  name="fard_item_lines[' + rownumber + '][vide_fad_id_no]"   value="{{ old("") }}"> </td>' +
                '<td><input type="date" class="form-control" id="date_' + rownumber + '"  name="fard_item_lines[' + rownumber + '][date]"   value="{{ old("") }}"> </td>' +
                '<td><input type="text" class="form-control" id="vide_fad_id_no_2_' + rownumber + '"  name="fard_item_lines[' + rownumber + '][vide_fad_id_no_2]"   value="{{ old("") }}"> </td>' +
                '<td><input type="date" class="form-control" id="date_2_' + rownumber + '"  name="fard_item_lines[' + rownumber + '][date_2]"   value="{{ old("") }}"> </td>' +
                '</tr>';

            $("#tbodyrowfard").append(row);

        });
    });
</script>

<script>
    $(function() {
        $('#add_row_land').click(function(e) {
            e.preventDefault();
            var rownumber_land = parseFloat($("#rownumber_land").val());
            rownumber_land = rownumber_land + 1;
            $("#rownumber_land").val(rownumber_land);
            var row = '<tr id="' + rownumber_land + '" DetailId="0"> ' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][block_no]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][rectangle_no]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][khasra_no]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][east_by]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][west_by]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][north_by]"> </td>' +
                '<td><input type="text" class="row-level" name="land_details[' + rownumber_land + '][south_by]"> </td>' +
                '<td><button type="button" class="btn btn-sm btn-danger delete-row">Delete</button> </td>' +
                '</tr>';

            $("#landDetailsBody").append(row);
        });
    });
</script>
<script>
    $(function() {
        $(document).on('click', '.delete-row', function(e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });
    });
</script>

@endsection