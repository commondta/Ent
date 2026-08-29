@extends('layouts.main')

@section('content')
<!-- Select2 CSS -->
<link href="{{ asset('public/vendors/select2/select2.min.css') }}" rel="stylesheet" />
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
        padding: 4px;
        font-size: 13px;
        min-height: 34px;
        width: 100%;
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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Purchase of Land</h4>
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
                                <form onsubmit="return validateForm()" class="row g-3 needs-validation"
                                    method="post" action="{{ route('purchase_of_land.update',$purchase_of_land->id) }}" novalidate=""
                                    enctype="multipart/form-data">
                                    @csrf
                                    @method('PUT')
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_date">File No</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="File_No" type="text"
                                                        name="File_No" required="" value="{{ $purchase_of_land->File_No }}" />

                                                    <div class="invalid-feedback">Please Add File No</div>
                                                    @error('File_No')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                
                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_date">Doc Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="doc_date" type="text"
                                                        name="doc_date" required="" readonly
                                                        value="{{$purchase_of_land->doc_date}}" />

                                                    <div class="invalid-feedback">Please Add Doc Date</div>
                                                    @error('doc_date')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-4">
                                                    <label class="form-label" for="posting_date">Posting Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="posting_date" type="text"
                                                        name="posting_date" required="" readonly
                                                        value="{{$purchase_of_land->posting_date}}" />

                                                    <div class="invalid-feedback">Please Add Doc Date</div>
                                                    @error('posting_date')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <!-- <div class="col-md-12">
                                                    <label class="form-label">LP Name</label>

                                                    @php
                                                    $selectedLpNames = is_string($purchase_of_land->lp_name)
                                                    ? json_decode($purchase_of_land->lp_name, true)
                                                    : (is_array($purchase_of_land->lp_name) ? $purchase_of_land->lp_name : []);

                                                    if (!is_array($selectedLpNames)) {
                                                    $selectedLpNames = [];
                                                    }
                                                    @endphp

                                                    <div class="multi-select-wrapper" data-required="true">
                                                       
                                                        <input type="hidden" class="multi-select-required" required>

                                                        <div class="multi-select-display">
                                                            <span class="multi-select-placeholder">Select LP Names</span>
                                                            <div class="multi-select-selected"></div>
                                                            <i class="fas fa-chevron-down multi-select-arrow"></i>
                                                        </div>

                                                        <div class="multi-select-options" style="display:none;">
                                                            @foreach($land_provider as $row)
                                                            <label class="multi-select-option">
                                                                <input
                                                                    type="checkbox"
                                                                    name="lp_name[]"
                                                                    value="{{ $row->lp_name }}"
                                                                    class="multi-select-checkbox"
                                                                    {{ in_array($row->lp_name, $selectedLpNames) ? 'checked' : '' }}>
                                                                <span>{{ $row->lp_cod }} - {{ $row->lp_name }}</span>
                                                            </label>
                                                            @endforeach
                                                        </div>
                                                    </div>

                                                    <div class="invalid-feedback d-block multi-select-error" style="display:none;">
                                                        
                                                    </div>

                                                    @error('lp_name')
                                                    <div class="text-danger mt-1" style="font-size:75%;">
                                                        {{ $message }}
                                                    </div>
                                                    @enderror
                                                </div> -->
                                                <div class="col-md-6">
                                                    <label class="form-label" for="land_form_no">Land Form
                                                        NO</label>
                                                    <select name="land_form_no" id="land_form_no" class="form-control"
                                                        required="">
                                                        <option value="">Kindly Select</option>

                                                        @foreach($land_owner as $row)
                                                        <option @if($purchase_of_land->land_form_no == $row->doc_no ) selected @endif value="{{ $row->doc_no }}">Land Form No
                                                            - {{ $row->doc_no }}</option>
                                                        @endforeach
                                                    </select>

                                                    <div class="invalid-feedback">Kindly Select Land Form NO.</div>
                                                    @error('land_form_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="lp_name">LP Name</label>
                                                    <select id="lp_name" name="lp_name" class="form-control">
                                                        <option value="">Kindly Select</option>
                                                        @foreach($land_provider as $row)
                                                        <option @if($purchase_of_land->lp_name == $row->lp_cod) selected @endif value="{{ $row->lp_cod }}">{{ $row->lp_name }}</option>
                                                        @endforeach
                                                    </select>

                                                    <div class="invalid-feedback">Please add LP Name.</div>
                                                    @error('lp_name')
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




                                                <div class="col-md-3">
                                                    <label class="form-label" for="mouza">Mouza/Chak No</label>
                                                    <input class="form-control" id="mouza" type="text" name="mouza"
                                                        required="" value="{{ $purchase_of_land->mouza }}" />

                                                    <div class="invalid-feedback">Please add Mouza/Chak No.</div>
                                                    @error('mouza')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>





                                                <div class="col-md-3">
                                                    <label class="form-label" for="acre">Acre</label>
                                                    <input class="form-control" id="acre" type="number" step="0.01" name="acre"
                                                        required="" value="{{ $purchase_of_land->acre }}" />
                                                    <div class="invalid-feedback">Please add acre.</div>
                                                    @error('acre')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="district_rate">District Rate</label>
                                                    <input class="form-control" id="district_rate" type="number" step="0.01" name="district_rate"
                                                        required="" value="{{ $purchase_of_land->district_rate }}" />
                                                    <div class="invalid-feedback">Please add district rate.</div>
                                                    @error('district_rate')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="district_amount">District Amount</label>
                                                    <input class="form-control" id="district_amount" type="number" step="0.01" name="district_amount"
                                                        required="" value="{{ $purchase_of_land->district_amount }}" />
                                                    <div class="invalid-feedback">Please add district amount.</div>
                                                    @error('district_amount')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="society_rate">{{ config('app.org_label') }} Rate</label>
                                                    <input class="form-control" id="society_rate" type="number" step="0.01" name="society_rate"
                                                        required="" readonly value="{{ $purchase_of_land->society_rate }}" />
                                                    <div class="invalid-feedback">Please add {{ config('app.org_label') }} rate.</div>
                                                    @error('society_rate')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="society_amount">{{ config('app.org_label') }} Amount</label>
                                                    <input class="form-control" id="society_amount" type="number" step="0.01" name="society_amount"
                                                        required="" readonly value="{{ $purchase_of_land->society_amount }}" />
                                                    <div class="invalid-feedback">Please add {{ config('app.org_label') }} amount.</div>
                                                    @error('society_amount')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>


                                                <div class="col-md-3">
                                                    <label class="form-label" for="exemption_rate">Exemption Rate</label>

                                                    <input
                                                        type="number"
                                                        step="0.01"
                                                        min="0"
                                                        name="exemption_rate"
                                                        id="exemption_rate"
                                                        class="form-control @error('exemption_rate') is-invalid @enderror"
                                                        value="{{ old('exemption_rate', $purchase_of_land->exemption_rate ?? '') }}"
                                                        required>

                                                    <div class="invalid-feedback">
                                                        Please add Exemption Rate.
                                                    </div>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label" for="mode_of_payment">Mode of payment</label>
                                                    <select name="mode_of_payment" class="form-control" required="">
                                                        <option value="">Kindly Select</option>
                                                        @foreach($mode_of_payment as $rate)
                                                        <option @if($purchase_of_land->mode_of_payment == $rate->mode_of_payment ) selected @endif value="{{$rate->mode_of_payment}}">{{$rate->mode_of_payment}}</option>
                                                        @endforeach
                                                    </select>
                                                    <div class="invalid-feedback">Kindly Select Mode of payment.</div>
                                                    @error('mode_of_payment')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="margin-top: 20px">
                                                <div class="card">

                                                    <div class="card-body">
                                                        <h5 class="card-title">Fard Details</h5>
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <label class="form-label" for="fard_id">Fard Id</label>
                                                                <input class="form-control" id="fard_id" type="text"
                                                                    name="fard_id" required="" value="{{ $purchase_of_land->fard_id }}" />

                                                                <div class="invalid-feedback">Please Add Fard Id</div>
                                                                @error('fard_id')
                                                                <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                @enderror
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-label" for="fard_date">Fard Date 1</label>
                                                                <?php
                                                                $dt = new DateTime();
                                                                ?>
                                                                <input class="form-control" id="fard_date" type="text"
                                                                    name="fard_date" required=""
                                                                    value="{{$purchase_of_land->fard_date}}" />

                                                                <div class="invalid-feedback">Please Add Fard Date</div>
                                                                @error('fard_date')
                                                                <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                @enderror
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-label" for="fard_id2">Fard Id 2 <span>(Optional)</span></label>
                                                                <input class="form-control" id="fard_id2" type="text"
                                                                    name="fard_id2" value="{{ $purchase_of_land->fard_id2 }}" />

                                                                <div class="invalid-feedback">Please Add Fard Id 2</div>
                                                                @error('fard_id2')
                                                                <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                @enderror
                                                            </div>
                                                            <div class="col-md-3">
                                                                <label class="form-label" for="fard_date2">Fard Date 2</label>
                                                                <?php
                                                                $dt = new DateTime();
                                                                ?>
                                                                <input class="form-control" id="fard_date2" type="text"
                                                                    name="fard_date2"
                                                                    value="{{$purchase_of_land->fard_date2}}" />

                                                                <div class="invalid-feedback">Please Add Fard Date2</div>
                                                                @error('fard_date2')
                                                                <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                @enderror
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12">
                                                            <div class="card border border-300 bg-soft mt-3">
                                                                <div class="card-header bg-soft">
                                                                    <h5 style="float: left;padding: 15px" class="mb-0">Land Details</h5>
                                                                    <p style="float: right" class="card-title btn btn-success" id="add_row">Add Row</p>
                                                                </div>
                                                                <div class="card-body" style="background-color: white">
                                                                    <div class="row">
                                                                        <div style="overflow-x: auto; -webkit-overflow-scrolling: touch;">
                                                                            <table class="table-bordered table-sm" id="landDetailsTable" style="border: 1px solid var(--lm-border); margin-bottom: 0; min-width: 100%;">
                                                                                <thead style="background-color: var(--lm-surface);">
                                                                                    <tr style="border: 1px solid var(--lm-border);">
                                                                                        <th style="width: 100px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 100px;">Khewat No</th>
                                                                                        <th style="width: 100px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 100px;">Khatooni No</th>

                                                                                        <th style="width: 100px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 100px;">Qatat</th>
                                                                                        <th colspan="3" style="width: 240px; border: 1px solid var(--lm-border); padding: 8px; text-align: center; font-weight: bold; min-width: 240px;">Measuring</th>
                                                                                        <th style="width: 100px; border: 1px solid var(--lm-border); padding: 8px; text-align: center; font-weight: bold; min-width: 100px;">Transfer Share</th>
                                                                                        <th colspan="3" style="width: 240px; border: 1px solid var(--lm-border); padding: 8px; text-align: center; font-weight: bold; min-width: 240px;">Land Measuring</th>
                                                                                        <th style="width: 160px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 160px;">Land Category</th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 80px;">Action</th>
                                                                                    </tr>
                                                                                    <tr style="border: 1px solid var(--lm-border);">
                                                                                        <th colspan="3" style="border: 1px solid var(--lm-border);"></th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">Kanal</th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">M</th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">Sqft</th>
                                                                                        <th style="width: 100px; border: 1px solid var(--lm-border);"></th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">Kanal</th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">M</th>
                                                                                        <th style="width: 80px; border: 1px solid var(--lm-border); padding: 4px; text-align: center; font-size: 0.85em;">Sqft</th>
                                                                                        <th style="border: 1px solid var(--lm-border);"></th>
                                                                                        <th style="border: 1px solid var(--lm-border);"></th>
                                                                                    </tr>
                                                                                </thead>
                                                                                <tbody id="tbodyrow">
                                                                                    @if($landDetails && count($landDetails) > 0)
                                                                                    @foreach($landDetails as $index => $detail)
                                                                                    <tr id="{{ $index + 1 }}" style="border: 1px solid var(--lm-border);">
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][khewat_no]" value="{{ $detail->khewat_no }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][khatooni_no]" value="{{ $detail->khatooni_no }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][qatat]" value="{{ $detail->qatat ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][measuring_k]" value="{{ $detail->measuring_k ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][measuring_m]" value="{{ $detail->measuring_m ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][measuring_sqft]" value="{{ $detail->measuring_sqft ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][transfer_share]" value="{{ $detail->transfer_share ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][land_measuring_k]" value="{{ $detail->land_measuring_k ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][land_measuring_m]" value="{{ $detail->land_measuring_m ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[{{ $index + 1 }}][land_measuring_sqft]" value="{{ $detail->land_measuring_sqft ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 160px; border: 1px solid var(--lm-border); padding: 0px;">
                                                                                            <select class="form-control" name="land_details[{{ $index + 1 }}][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                                                                                                <option value="">Select</option>
                                                                                                <option value="Ownership" @if($detail->land_category == 'Ownership') selected @endif>Ownership</option>
                                                                                                <option value="Non Pata" @if($detail->land_category == 'Non Pata') selected @endif>Non Pata</option>
                                                                                                <option value="Govt Land" @if($detail->land_category == 'Govt Land') selected @endif>Govt Land</option>
                                                                                            </select>
                                                                                        </td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)">Remove</button></td>
                                                                                    </tr>
                                                                                    @endforeach
                                                                                    @else
                                                                                    <tr id="1" style="border: 1px solid var(--lm-border);">
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][khewat_no]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][khatooni_no]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][qatat]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][measuring_k]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][measuring_m]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][measuring_sqft]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][transfer_share]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][land_measuring_k]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][land_measuring_m]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="land_details[1][land_measuring_sqft]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                        <td style="width: 160px; border: 1px solid var(--lm-border); padding: 0px;">
                                                                                            <select class="form-control" name="land_details[1][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                                                                                                <option value="">Select</option>
                                                                                                <option value="Ownership">Ownership</option>
                                                                                                <option value="Non Pata">Non Pata</option>
                                                                                                <option value="Govt Land">Govt Land</option>
                                                                                            </select>
                                                                                        </td>
                                                                                        <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)">Remove</button></td>
                                                                                    </tr>
                                                                                    @endif
                                                                                </tbody>
                                                                                <tfoot>
                                                                                    <tr style="background: var(--lm-surface);font-weight:bold">
                                                                                        <td colspan="7" style="text-align:right;border:1px solid var(--lm-border);">Total of Kanal, Marla,Sqft and Acre respectively</td>
                                                                                        <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                            <input type="text" id="total_kanal" name="total_kanal" class="form-control" readonly style="padding: 4px;"
                                                                                                value="{{ $Land_form->total_kanal ?? '' }}">
                                                                                        </td>
                                                                                        <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                            <input type="text" id="total_marla" name="total_marla" class="form-control" readonly style="padding: 4px;"
                                                                                                value="{{ $Land_form->total_marla ?? '' }}">
                                                                                        </td>
                                                                                        <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                            <input type="text" id="total_sqft" name="total_sqft" class="form-control" readonly style="padding: 4px;"
                                                                                                value="{{ $Land_form->total_sqft ?? '' }}">
                                                                                        </td>
                                                                                        <td style="width: 160px; border:1px solid var(--lm-border);">
                                                                                            <input type="text" id="total_acre" name="total_acre" class="form-control" readonly style="padding: 4px;"
                                                                                                value="{{ $Land_form->total_acre ?? '' }}">
                                                                                        </td>
                                                                                        <td></td>
                                                                                    </tr>
                                                                                </tfoot>

                                                                            </table>
                                                                        </div>
                                                                    </div>
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
                                                            <div class="col-md-4">

                                                                <div class="mb-3">
                                                                    <label class="form-label"
                                                                        for="attachment_nfc_sub_registrar">NFC
                                                                        from Sub
                                                                        Registrar</label>
                                                                    <input class="form-control"
                                                                        id="attachment_nfc_sub_registrar"
                                                                        name="attachment_nfc_sub_registrar[]"
                                                                        type="file" />
                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="mb-3">
                                                                    <label class="form-label"
                                                                        for="attachment_aks_shajra">Aks
                                                                        Shajra</label>
                                                                    <input class="form-control"
                                                                        id="attachment_aks_shajra"
                                                                        name="attachment_aks_shajra[]"
                                                                        type="file" />

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="mb-3">
                                                                    <label class="form-label" for="attachment_massavi">Massavi</label>
                                                                    <input class="form-control" id="attachment_massavi" name="attachment_massavi[]" type="file" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="mb-3">
                                                                    <label class="form-label" for="attachment_girdwari">khasra Girdawari</label>
                                                                    <input class="form-control" id="attachment_girdwari" name="attachment_girdwari[]" type="file" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="mb-3">
                                                                    <label class="form-label"
                                                                        for="attachment_fard_milkiyat">Fard-e-malkiyat</label>
                                                                    <input class="form-control"
                                                                        id="attachment_fard_milkiyat"
                                                                        name="attachment_fard_milkiyat[]"
                                                                        type="file" />

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="mb-3">
                                                                    <label class="form-label"
                                                                        for="attachment_khata_of_land">Khata
                                                                        of Land
                                                                        (Mufasl)</label>
                                                                    <input class="form-control"
                                                                        id="attachment_khata_of_land"
                                                                        name="attachment_khata_of_land[]"
                                                                        type="file" />

                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">

                                                                <div class="mb-3">
                                                                    <label class="form-label"
                                                                        for="attachment">Other</label>
                                                                    <input class="form-control"
                                                                        id="attachment"
                                                                        name="attachment[]"
                                                                        type="file"
                                                                        multiple />
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-12" style="margin-top: 20px">
                                                <div class="card">
                                                    <table class="table table-striped table-sm fs--1 mb-0">

                                                        <thead>
                                                            <tr>
                                                                <td class="sort border-top">#</td>
                                                                <td class="sort border-top">Document</td>
                                                                <td class="sort border-top">Attachment</td>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <?php $count = 1; ?>
                                                            @foreach($attachments as $signle)
                                                            <tr>
                                                                <td class="align-middle ps-3 name">{{$count}}</td>
                                                                <td class="align-middle ps-3 name"><?php
                                                                                                    if ($signle->document == 'Other') {
                                                                                                        echo 'Other';
                                                                                                    }
                                                                                                    if ($signle->document == 'attachment_khata_of_land') {
                                                                                                        echo 'KHATA OF LAND (MUFASL)';
                                                                                                    }
                                                                                                    if ($signle->document == 'attachment_fard_milkiyat') {
                                                                                                        echo 'FARD-E-MALKIYAT';
                                                                                                    }
                                                                                                    if ($signle->document == 'attachment_girdwari') {
                                                                                                        echo 'KHASRA GIRDAWARI';
                                                                                                    }

                                                                                                    if ($signle->document == 'attachment_aks_shajra') {
                                                                                                        echo 'AKS SHAJRA';
                                                                                                    }
                                                                                                    if ($signle->document == 'attachment_massavi') {
                                                                                                        echo 'MASSAVI';
                                                                                                    }
                                                                                                    if ($signle->document == 'attachment_nfc_sub_registrar') {
                                                                                                        echo 'NFC FROM SUB REGISTRAR';
                                                                                                    }


                                                                                                    ?></td>
                                                                <td class="align-middle ps-3 name">

                                                                    @if($signle->attachment)
                                                                    <?php
                                                                    $filename = $signle->attachment;
                                                                    $extension = pathinfo($filename, PATHINFO_EXTENSION);

                                                                    ?>
                                                                    @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                    <a target="_blank"
                                                                        href="{{ URL::asset('public/assets/uploads/').'/'.$signle->attachment; }}">
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$signle->attachment; }}"
                                                                            style="width: 50px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                    </a>

                                                                    @else
                                                                    <a target="_blank"
                                                                        href="{{ URL::asset('public/assets/uploads/').'/'.$signle->attachment; }}">
                                                                        <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                            style="width: 50px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                    </a>
                                                                    @endif
                                                                    {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$purchase_of_land->attachment_khata_of_land; }}" style="width: 279px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                                    @endif

                                                                </td>

                                                            </tr>
                                                            <?php $count++; ?>

                                                            @endforeach
                                                        </tbody>
                                                    </table>
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
    <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert"
        aria-live="assertive" aria-atomic="true">
        <div class="d-flex">
            <div class="toast-body p-3"></div>
            <button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast"
                aria-label="Close"></button>
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
    // Truncate decimal without rounding
    function truncateToDecimal(value, decimals) {
        const factor = Math.pow(10, decimals);
        return Math.trunc(value * factor) / factor;
    }

    $(document).ready(function() {

        let ratePerAcre = 0;

        // ============================
        // CALCULATION FUNCTIONS
        // ============================

        // function calculateDistrictAmount() {
        //     let acre = parseFloat($('#acre').val()) || 0;
        //     let districtRate = parseFloat($('#district_rate').val()) || 0;

        //     let districtAmount = districtRate * acre;
        //     $('#district_amount').val(districtAmount.toFixed(2));
        // }

        function calculateSocietyAmount() {
            let acre = parseFloat($('#acre').val()) || 0;

            //  Safe condition
            if (acre > 0 && ratePerAcre > 0) {
                let societyAmount = ratePerAcre * acre;

                $('#society_rate').val(ratePerAcre.toFixed(2));
                $('#society_amount').val(societyAmount.toFixed(2));
            }
        }

        function calculateAll() {
            // calculateDistrictAmount();
            calculateSocietyAmount();
        }

        // ============================
        // SINGLE CHANGE HANDLER
        // ============================

        $('#land_form_no').on('change', function() {

            let selectedValue = $(this).val();

            if (!selectedValue) {
                ratePerAcre = 0;
                $('#society_rate, #society_amount').val('');
                return;
            }

            // -------- LAND FORM --------
            $.ajax({
                url: "{{ url('/get_land_form') }}",
                type: "POST",
                data: {
                    _token: "{{ csrf_token() }}",
                    value: selectedValue
                },
                success: function(data) {

                    $('#mouza').val(data.mouza).prop('readonly', true);
                    $('#acre').val(data.total_acre).prop('readonly', true);
                    calculateAll(); //  safe
                },
                error: function(xhr) {
                    console.error(xhr.responseText);
                }
            });

            // -------- RATE --------
            $.ajax({
                url: "{{ url('/get-land-rate') }}/" + selectedValue,
                type: "GET",
                dataType: "json",
                success: function(response) {

                    if (response.success) {
                        ratePerAcre = parseFloat(response.rate_per_acre) || 0;
                        calculateAll(); //  safe
                    } else {
                        ratePerAcre = 0;
                        $('#society_rate, #society_amount').val('');
                    }
                },

            });

        });

        // ============================
        // INPUT EVENTS
        // ============================

        $('#acre').on('input', function() {
            calculateAll();
        });

        $('#district_rate').on('input', function() {
            // calculateDistrictAmount();
        });

        // ============================
        // EDIT PAGE PREFILL FIX 🔥
        // ============================

        let existingDocNo = $('#land_form_no').val();

        if (existingDocNo) {
            // Only fetch rate (land form already filled in edit)
            $.ajax({
                url: "{{ url('/get-land-rate') }}/" + existingDocNo,
                type: "GET",
                dataType: "json",
                success: function(response) {

                    if (response.success) {
                        ratePerAcre = parseFloat(response.rate_per_acre) || 0;
                        calculateAll(); //  calculate on load
                    }
                }
            });
        }

        // Initial calculation (district)
        calculateDistrictAmount();

    });
</script>


<!-- Fetch LO Information when Land Form No changes -->
<script>
    $(document).ready(function() {
        // Fetch LO details when land_form_no changes
        $('#land_form_no').on('change', function() {
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

        // Trigger on page load if land_form_no is already selected (for edit form)
        if ($('#land_form_no').val()) {
            $('#land_form_no').trigger('change');
        }
    });
</script>

<!-- Initialize Select2 for Land Form No -->
<script src="{{ asset('public/vendors/select2/select2.min.js') }}"></script>
<script>
    $(document).ready(function() {
        // Initialize Select2 on land_form_no field
        $('#land_form_no').select2({
            placeholder: 'Search and select Land Form No',
            allowClear: true,
            width: '100%'
        });
    });
</script>

<script>
    function AddLoRow() {
        debugger;
        var rownumber = parseFloat($("#loRowNumber").val());
        var LineId = rownumber;
        rownumber = rownumber + 1;
        $("#loRowNumber").val(rownumber);
        var row = '<tr><td>' +
            '<select name="lo_lines[' + rownumber + '][lo_cod]" id="lo_lines[' + rownumber + '][lo_cod]" name="lo_cod" onchange="get_seller_profile(this)"' +
            'class="form-control" required><option value="">Kindly Select</option> @foreach($record as $row) <option value="{{ $row->lo_cod }}">{{ $row->lo_cod .' - '. $row->lo_name_as_per_cnic }}</option>@endforeach' +
            '</select>' +
            '</td>' +

            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][lo_name_as_per_cnic]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][relationship_cnic]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][father_name_cnic]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][lo_name]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][relationship_revenue]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][so]"></td>' +
            '<td><input type="number" class="row-level form-control" name="lo_lines[' + rownumber + '][lo_cnic]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][caste]"></td>' +
            '<td><input type="number" class="row-level form-control" name="lo_lines[' + rownumber + '][contact_no]"></td>' +
            '<td><input type="text" class="row-level form-control" name="lo_lines[' + rownumber + '][address]"></td>'
        '<tr>';

        $("#tbodyLorow").append(row);
    }
</script>

<!-- Fetch Land Details when Land Form No changes -->
<script>
    $(document).ready(function() {

        $('#land_form_no').on('change', function() {
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
                            <td><input type="text" class="form-control" name="land_details[${index}][khewat_no]" value="${rowData.khewat_no ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][khatooni_no]" value="${rowData.khatooni_no ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][qatat]" value="${rowData.qatat ?? ''}"></td>
                             <td><input type="text" class="form-control" name="land_details[${index}][measuring_k]" value="${rowData.measuring_k ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][measuring_m]" value="${rowData.measuring_m ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][measuring_sqft]" value="${rowData.measuring_sqft ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][transfer_share]" value="${rowData.transfer_share ?? ''}"></td>
                             <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_k]" value="${rowData.land_measuring_k ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_m]" value="${rowData.land_measuring_m ?? ''}"></td>
                            <td><input type="text" class="form-control" name="land_details[${index}][land_measuring_sqft]" value="${rowData.land_measuring_sqft ?? ''}"></td>
                            <td>
                                <select class="form-control" name="land_details[${index}][land_category]">
                                    <option value="">Select</option>
                                    <option value="Ownership" ${rowData.land_category === 'Ownership' ? 'selected' : ''}>Ownership</option>
                                    <option value="Non Pata" ${rowData.land_category === 'Non Pata' ? 'selected' : ''}>Non Pata</option>
                                    <option value="Govt Land" ${rowData.land_category === 'Govt Land' ? 'selected' : ''}>Govt Land</option>
                                </select>
                            </td>

                            <!-- Action -->
                            <td>
                                <button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)">Remove</button>
                            </td>
                        </tr>
                        `;

                            $('#landDetailsTable tbody').append(row);
                        });
                        // Calculate totals after populating the table
                        calculateTotals();
                    }
                },
                error: function() {
                    alert('Error fetching Land details');
                }
            });
        });

    });
</script>

<script>
    function deleteRow(btn) {
        btn.closest('tr').remove();
        calculateTotals();
    }


    $(function() {

        $('#add_row').click(function() {

            var rownumber = parseFloat($("#rownumber").val());
            rownumber++;
            $("#rownumber").val(rownumber);

            var row = `
            <tr id="${rownumber}" DetailId="0" style="border: 1px solid var(--lm-border);">

                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][khewat_no]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][khatooni_no]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][qatat]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][measuring_k]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][measuring_m]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][measuring_sqft]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][transfer_share]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][land_measuring_k]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][land_measuring_m]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="land_details[${rownumber}][land_measuring_sqft]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>

                <td style="width: 160px; border: 1px solid var(--lm-border); padding: 0px;">
                    <select class="form-control" 
                            name="land_details[${rownumber}][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                        <option value="">Select</option>
                        <option value="Ownership">Ownership</option>
                        <option value="Non Pata">Non Pata</option>
                        <option value="Govt Land">Govt Land</option>
                    </select>
                </td>

                <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)">Remove</button></td>

            </tr>
        `;

            $("#tbodyrow").append(row);

        });
        calculateTotals();

    });
</script>
<script>
    function calculateTotals() {

        let totalKanal = 0;
        let totalMarla = 0;
        let totalSqft = 0;

        $('#tbodyrow tr').each(function() {

            let kanal = parseFloat($(this).find('input[name*="[land_measuring_k]"]').val()) || 0;
            let marla = parseFloat($(this).find('input[name*="[land_measuring_m]"]').val()) || 0;
            let sqft = parseFloat($(this).find('input[name*="[land_measuring_sqft]"]').val()) || 0;

            totalKanal += kanal;
            totalMarla += marla;
            totalSqft += sqft;
        });

        /* Sqft → Marla Conversion */
        if (totalSqft >= 272) {

            let extraMarla = Math.floor(totalSqft / 272);
            totalMarla += extraMarla;
            totalSqft = totalSqft % 272;
        }

        /* Marla → Kanal Conversion */
        if (totalMarla >= 20) {

            let extraKanal = Math.floor(totalMarla / 20);
            totalKanal += extraKanal;
            totalMarla = totalMarla % 20;
        }

        /* Acre Calculation */
        let acre = totalKanal / 8 + totalMarla / 160 + totalSqft / 43560;

        $('#total_kanal').val(totalKanal);
        $('#total_marla').val(totalMarla);
        $('#total_sqft').val(totalSqft);
        $('#total_acre').val(truncateToDecimal(acre, 2));
    }

    $(document).on('keyup change',
        'input[name*="[land_measuring_k]"], input[name*="[land_measuring_m]"], input[name*="[land_measuring_sqft]"]',
        function() {
            calculateTotals();
        });
</script>


@endsection