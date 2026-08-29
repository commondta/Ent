@extends('layouts.main')

@section('content')
<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="mb-9">
                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                            <div class="row g-3 justify-content-between align-items-center">
                                <div class="col-12 col-md">
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Offer Form (Land Details) </h4>
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
                                <form class="row g-3 needs-validation" method="post" action="{{ route('land_form.update',$Land_form->id) }}" novalidate="" enctype="multipart/form-data">
                                    <input type="hidden" id="rownumber"
                                        value="{{ $Land_form->rows ? $Land_form->rows->count() : 1 }}">

                                    <input type="hidden" id="loRowNumber"
                                        value="{{ $Land_form->lo_lines ? $Land_form->lo_lines->count() : 1 }}">



                                    @csrf
                                    @method('PUT')
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">

                                                <div class="col-md-6">
                                                    <label class="form-label" for="doc_date">Doc Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="doc_date" type="text" name="doc_date" required="" readonly value="{{$Land_form->doc_date}}" />
                                                    <div class="valid-feedback">Please Add Doc Date</div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="doc_no">Doc No</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" readonly value="{{$Land_form->doc_no}}" required="" />
                                                    <div class="valid-feedback">Please Add Doc No</div>
                                                </div>

                                                <div class="col-md-12">
                                                    <div class="card border border-300 bg-soft mt-3">
                                                        <div class="card-header bg-soft">
                                                            <h5 style="float: left;padding: 15px" class="mb-0">LO Record</h5>
                                                            <p style="float: right" class="card-title btn btn-success" onclick="AddLoRow();">Add LO Record</p>
                                                        </div>
                                                        <div class="card-body" style="background-color: white">
                                                            <div class="row">
                                                                <div class="table-wrapper" style="overflow-x: auto; -webkit-overflow-scrolling: touch;">
                                                                    <table class="lo-table" style="border: 1px solid var(--lm-border); margin-bottom: 0; min-width: 100%;">
                                                                        <thead>
                                                                            <tr>
                                                                                <th>LO Code</th>
                                                                                
                                                                                <th>LO Name as per CNIC Record</th>
                                                                                <th>Relationship</th>
                                                                                <th>Father / Husband Name</th>
                                                                                <th>LO Name as per Revenue Record</th>
                                                                                <th>Relationship</th>
                                                                                <th>Father / Husband Name</th>
                                                                                <th>LO CNIC</th>
                                                                                <th>Caste</th>
                                                                                <th>Contact No</th>
                                                                                <th>Address</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody id="tbodyLorow">
                                                                            @if($Land_form->lo_lines && count($Land_form->lo_lines) > 0)
                                                                            @foreach($Land_form->lo_lines as $index => $line)
                                                                            <tr id="{{ $index + 1 }}">
                                                                                <td>
                                                                                    <select name="lo_lines[{{ $index + 1 }}][lo_cod]" id="lo_lines[{{ $index + 1 }}][lo_cod]" name="lo_cod" onchange="get_seller_profile(this)"
                                                                                        class="form-control" required>
                                                                                        <option value="">Kindly Select</option> @foreach($record as $row) <option value="{{ $row->lo_cod }}" {{ $line->lo_cod === $row->lo_cod ? 'selected' : '' }}>{{ $row->lo_cod .' - '. $row->lo_name_as_per_cnic }}</option>@endforeach
                                                                                    </select>
                                                                                </td>
                                                                               

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][lo_name_as_per_cnic]" value="{{ $line->lo_name_as_per_cnic }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][relationship_cnic]" value="{{ $line->relationship_cnic }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][father_name_cnic]" value="{{ $line->father_name_cnic }}"></td>
                                                                                 <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][lo_name]" value="{{ $line->lo_name }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][relationship_revenue]" value="{{ $line->relationship_revenue }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][so]" value="{{ $line->so }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][lo_cnic]" value="{{ $line->lo_cnic }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][caste]" value="{{ $line->caste }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][contact_no]" value="{{ $line->contact_no }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[{{ $index + 1 }}][address]" value="{{ $line->address }}"></td>
                                                                            </tr>
                                                                            @endforeach
                                                                            @else
                                                                            <tr id="1">
                                                                                <td>
                                                                                    <select name="lo_lines[1][lo_cod]" id="lo_lines[1][lo_cod]" name="lo_cod" onchange="get_seller_profile(this)"
                                                                                        class="form-control" required>
                                                                                        <option value="">Kindly Select</option> @foreach($record as $row) <option value="{{ $row->lo_cod }}">{{ $row->lo_cod .' - '. $row->lo_name_as_per_cnic }}</option>@endforeach
                                                                                    </select>
                                                                                </td>
                                                                                

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][lo_name_as_per_cnic]" value="{{ $Land_form->lo_name_as_per_cnic ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][relationship_cnic]" value="{{ $Land_form->relationship_cnic ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][father_name_cnic]" value="{{ $Land_form->father_name_cnic ?? '' }}"></td>
                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][lo_name]" value="{{ $Land_form->lo_name ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][relationship_revenue]" value="{{ $Land_form->relationship_revenue ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][so]" value="{{ $Land_form->so ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][lo_cnic]" value="{{ $Land_form->lo_cnic ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][caste]" value="{{ $Land_form->caste ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][contact_no]" value="{{ $Land_form->contact_no ?? '' }}"></td>

                                                                                <td><input type="text" class="row-level form-control" name="lo_lines[1][address]" value="{{ $Land_form->address ?? '' }}"></td>
                                                                            </tr>
                                                                            @endif
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
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
                                                                                 <th style="width: 250px; border: 1px solid var(--lm-border); padding: 8px; font-weight: bold; min-width: 250px;">Lo Code</th>
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
                                                                                <th colspan="4" style="border: 1px solid var(--lm-border);"></th>
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
                                                                            @if($Land_form->rows && count($Land_form->rows) > 0)
                                                                            @foreach($Land_form->rows as $index => $row)
                                                                            <tr id="{{ $index + 1 }}" style="border: 1px solid var(--lm-border);">
                                                                                <td style="width: 250px; border: 1px solid var(--lm-border);">
                                                                                     <select name="item_lines[{{ $index + 1 }}][lo_cod]" onchange="get_seller_profile(this)" class="form-control" required style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                                                                                         <option value="">Kindly Select</option>
                                                                                         @foreach($record as $seller)
                                                                                         <option value="{{ $seller->lo_cod }}" {{ $row->lo_cod == $seller->lo_cod ? 'selected' : '' }}>
                                                                                             {{ $seller->lo_cod .' - '. $seller->lo_name_as_per_cnic }}
                                                                                         </option>
                                                                                         @endforeach
                                                                                     </select>
                                                                                 </td>

                                                                                 <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][khewat_no]" value="{{ $row->khewat_no }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][khatooni_no]" value="{{ $row->khatooni_no }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][qatat]" value="{{ $row->qatat ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][measuring_k]" value="{{ $row->measuring_k ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][measuring_m]" value="{{ $row->measuring_m ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][measuring_sqft]" value="{{ $row->measuring_sqft ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][transfer_share]" value="{{ $row->transfer_share ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][land_measuring_k]" value="{{ $row->land_measuring_k ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][land_measuring_m]" value="{{ $row->land_measuring_m ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                 <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[{{ $index + 1 }}][land_measuring_sqft]" value="{{ $row->land_measuring_sqft ?? '' }}" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 140px; border: 1px solid var(--lm-border); padding: 0px;">
                                                                                    <select class="form-control" name="item_lines[{{ $index + 1 }}][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%;">
                                                                                        <option value="">Select</option>
                                                                                        <option value="Ownership" @if($row->land_category == 'Ownership') selected @endif>Ownership</option>
                                                                                        <option value="Non Pata" @if($row->land_category == 'Non Pata') selected @endif>Non Pata</option>
                                                                                        <option value="Govt Land" @if($row->land_category == 'Govt Land') selected @endif>Govt Land</option>
                                                                                    </select>
                                                                                </td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)"><i class="fas fa-trash"></i></button></td>
                                                                            </tr>
                                                                            @endforeach
                                                                            @else
                                                                            <tr id="1" style="border: 1px solid var(--lm-border);">
                                                                                <td style="width: 250px; border: 1px solid var(--lm-border);">
                                                                                    <select name="item_lines[1][lo_cod]" onchange="get_seller_profile(this)" class="form-control" required style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                                                                                        <option value="">Kindly Select</option>
                                                                                        @foreach($record as $seller)
                                                                                        <option value="{{ $seller->lo_cod }}">{{ $seller->lo_cod .' - '. $seller->lo_name_as_per_cnic }}</option>
                                                                                        @endforeach
                                                                                    </select>
                                                                                </td>
                                                                                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][khewat_no]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][khatooni_no]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][qatat]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][measuring_k]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][measuring_m]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][measuring_sqft]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][transfer_share]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][land_measuring_k]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][land_measuring_m]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="row-level form-control" name="item_lines[1][land_measuring_sqft]" value="" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                                                                                <td style="width: 140px; border: 1px solid var(--lm-border); padding: 0px;">
                                                                                    <select class="form-control" name="item_lines[1][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%;">
                                                                                        <option value="">Select</option>
                                                                                        <option value="Ownership">Ownership</option>
                                                                                        <option value="Non Pata">Non Pata</option>
                                                                                        <option value="Govt Land">Govt Land</option>
                                                                                    </select>
                                                                                </td>
                                                                                <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)"><i class="fas fa-trash"></i></button></td>
                                                                            </tr>
                                                                            @endif
                                                                        </tbody>
                                                                        <tfoot>
                                                                            <tr style="background: var(--lm-surface);font-weight:bold">
                                                                                <td colspan="8" style="text-align:right;border:1px solid var(--lm-border);">Total of Kanal, Marla,Sqft and Acre respectively</td>

                                                                                <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                    <input type="text" id="total_kanal" name="total_kanal" class="form-control" readonly
                                                                                        value="{{ $Land_form->total_kanal ?? '' }}" style="padding: 4px;">
                                                                                </td>

                                                                                <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                    <input type="text" id="total_marla" name="total_marla" class="form-control" readonly
                                                                                        value="{{ $Land_form->total_marla ?? '' }}" style="padding: 4px;">
                                                                                </td>

                                                                                <td style="width: 80px; border:1px solid var(--lm-border);">
                                                                                    <input type="text" id="total_sqft" name="total_sqft" class="form-control" readonly
                                                                                        value="{{ $Land_form->total_sqft ?? '' }}" style="padding: 4px;">
                                                                                </td>

                                                                                <td style="width: 160px; border:1px solid var(--lm-border);">
                                                                                    <input type="text" id="total_acre" name="total_acre" class="form-control" readonly
                                                                                        value="{{ $Land_form->total_acre ?? '' }}" style="padding: 4px;">
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

                                                <div class="col-md-4">
                                                    <label class="form-label">Mouza/Chak No</label>
                                                    <input class="form-control" name="mouza" type="text" value="{{$Land_form->mouza}}">
                                                </div>
                                                <div class="col-md-4">
                                                    <label class="form-label" for="sector">Sector</label>
                                                    <input class="form-control" id="sector" type="text" name="sector" value="{{$Land_form->sector}}" required="" />
                                                    <div class="invalid-feedback">Please add Sector.</div>
                                                </div>


                                                <div class="col-md-4">
                                                    <label class="form-label" for="rate_per_acre">Rate per acre in Millions</label>
                                                    <input class="form-control" id="rate_per_acre" type="text" name="rate_per_acre" value="{{$Land_form->rate_per_acre}}" required="" />
                                                    <div class="invalid-feedback">Please add Rate per acre in Millions.</div>
                                                </div>


                                                <div class="col-md-4">
                                                    <label class="form-label" for="tehsil">Tehsil</label>
                                                    <input class="form-control" id="tehsil" type="text" name="tehsil" value="{{$Land_form->tehsil}}" required="" />
                                                    <div class="invalid-feedback">Please add Tehsil.</div>
                                                </div>

                                                <div class="col-md-4">
                                                    <label class="form-label" for="district">District</label>
                                                    <input class="form-control" id="district" type="text" name="district" value="{{$Land_form->district}}" required="" />
                                                    <div class="invalid-feedback">Please add District.</div>
                                                </div>



                                                <!-- Power of Attorney Section for Minor Land Provider -->
                                                <div class="col-md-12">
                                                    <div class="card border border-300 bg-soft mt-3">
                                                        <div class="card-header bg-soft">
                                                            <h5 class="mb-0">Power of Attorney (In Case of Minor Land Owner)</h5>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="row g-3">
                                                                <!-- do not change the col-md-12 class as it is required for multi select js to work if you want then first in main blade line no 151 change the col-md-12 to col-md-6 and then change here also but do not change the multi-select-error class as it is required for error handling of multi select js -->
                                                                <div class="col-md-12">
                                                                    <label class="form-label">LO Code</label>
                                                                    @php
                                                                    $selectedLo_code = [];

                                                                    if (is_string($Land_form->poa_lo_code) && !empty($Land_form->poa_lo_code)) {
                                                                    // If stored as comma-separated string
                                                                    $selectedLo_code = explode(',', $Land_form->poa_lo_code);
                                                                    } elseif (is_array($Land_form->poa_lo_code)) {
                                                                    $selectedLo_code = $Land_form->poa_lo_code;
                                                                    }
                                                                    @endphp


                                                                    <div class="multi-select-wrapper" data-required="true">
                                                                        <!-- Hidden input for required validation -->
                                                                        <input type="hidden" class="multi-select-required" required>

                                                                        <div class="multi-select-display">
                                                                            <span class="multi-select-placeholder">Select Lo Code</span>
                                                                            <div class="multi-select-selected"></div>
                                                                            <i class="fas fa-chevron-down multi-select-arrow"></i>
                                                                        </div>

                                                                        <div class="multi-select-options" style="display:none;">
                                                                            @foreach($record as $row)
                                                                            <label class="multi-select-option">
                                                                                <input
                                                                                    type="checkbox"
                                                                                    name="poa_lo_code[]"
                                                                                    value="{{ $row->lo_cod }}"
                                                                                    class="multi-select-checkbox"
                                                                                    {{ in_array($row->lo_cod, $selectedLo_code) ? 'checked' : '' }}>
                                                                                <span>{{ $row->lo_cod }} - {{ $row->lo_name_as_per_cnic }}</span>
                                                                            </label>
                                                                            @endforeach
                                                                        </div>
                                                                    </div>

                                                                    <div class="invalid-feedback d-block multi-select-error" style="display:none;">
                                                                        <!-- Please select at least one LP Name. -->
                                                                    </div>

                                                                    @error('poa_lo_code')
                                                                    <div class="text-danger mt-1" style="font-size:75%;">
                                                                        {{ $message }}
                                                                    </div>
                                                                    @enderror
                                                                </div>
                                                                <div class="col-md-5">
                                                                    <label class="form-label" for="poa_name">Name</label>
                                                                    <input class="form-control" id="poa_name" type="text" name="poa_name" value="{{ $Land_form->poa_name }}" />
                                                                    <div class="invalid-feedback">Please add Name.</div>
                                                                    @error('poa_name')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <label class="form-label">Relationship</label>
                                                                    <select class="form-control" name="relationship">
                                                                        <option value="">Select</option>
                                                                        <option value="S/O" {{ $Land_form->relationship == 'S/O' ? 'selected' : '' }}>S/O</option>
                                                                        <option value="W/O" {{ $Land_form->relationship == 'W/O' ? 'selected' : '' }}>W/O</option>
                                                                        <option value="D/O" {{ $Land_form->relationship == 'D/O' ? 'selected' : '' }}>D/O</option>
                                                                        <option value="Widow of" {{ $Land_form->relationship == 'Widow of' ? 'selected' : '' }}>Widow of</option>
                                                                    </select>
                                                                </div>

                                                                <div class="col-md-5">
                                                                    <label class="form-label" for="poa_father_name">Father / Husband Name</label>
                                                                    <input class="form-control" id="poa_father_name" type="text" name="poa_father_name" value="{{ $Land_form->poa_father_name }}" />
                                                                    <div class="invalid-feedback">Please add Father Name.</div>
                                                                    @error('poa_father_name')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <label class="form-label" for="poa_cnic">CNIC</label>
                                                                    <input class="form-control" id="poa_cnic" type="text" name="poa_cnic" value="{{ $Land_form->poa_cnic }}" />
                                                                    <div class="invalid-feedback">Please add CNIC.</div>
                                                                    @error('poa_cnic')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>
                                                                 <div class="col-md-6">
                                                                    <label class="form-label" for="poa_caste">Caste</label>
                                                                    <input class="form-control" id="poa_caste" type="text"
                                                                        name="poa_caste"
                                                                        value="{{ $Land_form->poa_caste }}" />
                                                                    <div class="invalid-feedback">Please add Caste.</div>
                                                                </div>



                                                                <div class="col-md-4">
                                                                    <label class="form-label" for="poa_current_address">Current Address</label>
                                                                    <textarea class="form-control" id="poa_current_address" name="poa_current_address" rows="3">{{ $Land_form->poa_current_address }}</textarea>
                                                                    <div class="invalid-feedback">Please add Current Address.</div>
                                                                    @error('poa_current_address')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <label class="form-label" for="poa_permanent_address">Permanent Address</label>
                                                                    <textarea class="form-control" id="poa_permanent_address" name="poa_permanent_address" rows="3">{{ $Land_form->poa_permanent_address }}</textarea>
                                                                    <div class="invalid-feedback">Please add Permanent Address.</div>
                                                                    @error('poa_permanent_address')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label class="form-label" for="poa_remarks">Remarks</label>
                                                                    <textarea class="form-control" id="poa_remarks" name="poa_remarks" rows="3">{{ $Land_form->poa_remarks }}</textarea>
                                                                    <div class="invalid-feedback">Please add Remarks.</div>
                                                                    @error('poa_remarks')
                                                                    <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                    @enderror
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label" for="customFile">Profile Picture</label>
                                                                        <input class="form-control" id="customFile" name="attachments"
                                                                            type="file" value="{{$Land_form->attachments}}" />
                                                                        @if($Land_form->attachments)
                                                                        <?php
                                                                        $filename = $Land_form->attachments;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION); ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->attachments;}}"
                                                                            style="width: 200px;height: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->attachments; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;height: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif

                                                                        @endif
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label" for="customFile">CNIC Front</label>
                                                                        <input class="form-control" id="customFile" name="cnic_front_attachments" type="file" value="{{$Land_form->cnic_front_attachments}}" />
                                                                        @if($Land_form->cnic_front_attachments)
                                                                        <?php
                                                                        $filename = $Land_form->cnic_front_attachments;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if($extension == 'jpg' || $extension == 'jpeg' ||$extension == 'pgifng' ||$extension == 'png' )
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->cnic_front_attachments; }}"
                                                                            style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->cnic_front_attachments; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->attachments; }}" style="width: 240px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                                        @endif
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label" for="customFile">CNIC Back</label>
                                                                        <input class="form-control" id="customFile" name="cnic_back_attachments" type="file" value="{{$Land_form->cnic_back_attachments}}" />
                                                                        @if($Land_form->cnic_back_attachments)
                                                                        <?php
                                                                        $filename = $Land_form->cnic_back_attachments;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);

                                                                        ?>
                                                                        @if($extension == 'jpg' || $extension == 'jpeg' ||$extension == 'pgifng' ||$extension == 'png' )
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->cnic_back_attachments; }}"
                                                                            style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->cnic_back_attachments; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$Land_form->attachments; }}" style="width: 240px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                                        @endif
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

<script src="{{ asset('public/vendors/jquery/jquery-3.6.0.min.js') }}"></script>
<script>
    var baseUrl = '<?php echo e(config("app.url")); ?>';
</script>
<script>
    function get_seller_profile(selectObj) {

        let $row = $(selectObj).closest('tr'); // ? current row
        let selectedValue = $(selectObj).val();

        if (!selectedValue) return;

        $.ajax({
            url: baseUrl + '/get_seller_data',
            type: 'POST',
            data: {
                _token: "{{ csrf_token() }}",
                value: selectedValue
            },
            success: function(data) {

                $row.find('input[name$="[lo_name]"]')
                    .val(data.lo_name)
                    .prop('readonly', true);

                $row.find('input[name$="[relationship_revenue]"]')
                    .val(data.relationship_revenue)
                    .prop('readonly', true);

                $row.find('input[name$="[so]"]')
                    .val(data.lo_father_name)
                    .prop('readonly', true);

                $row.find('input[name$="[lo_name_as_per_cnic]"]')
                    .val(data.lo_name_as_per_cnic)
                    .prop('readonly', true);

                $row.find('input[name$="[relationship_cnic]"]')
                    .val(data.relationship_cnic)
                    .prop('readonly', true);

                $row.find('input[name$="[father_name_cnic]"]')
                    .val(data.father_name_cnic)
                    .prop('readonly', true);

                $row.find('input[name$="[lo_cnic]"]')
                    .val(data.lo_cnic)
                    .prop('readonly', true);

                $row.find('input[name$="[caste]"]')
                    .val(data.caste)
                    .prop('readonly', true);

                $row.find('input[name$="[contact_no]"]')
                    .val(data.contact_no)
                    .prop('readonly', true);

                $row.find('input[name$="[address]"]')
                    .val(data.address)
                    .prop('readonly', true);
            },
            error: function() {
                alert('Unable to fetch seller data');
            }
        });
    }
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
<script>
    var sellerOptions = `
        <option value="">Kindly Select</option>
        @foreach($record as $seller)
            <option value="{{ $seller->lo_cod }}">
                {{ $seller->lo_cod }} - {{ $seller->lo_name_as_per_cnic }}
            </option>
        @endforeach
    `;
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

                <td style="width: 250px; border: 1px solid var(--lm-border);">
                    <select class="form-control" 
                            name="item_lines[${rownumber}][lo_cod]" onchange="get_seller_profile(this)" required style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                        ${sellerOptions}
                    </select>
                </td>

                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][khewat_no]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][khatooni_no]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][qatat]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][measuring_k]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][measuring_m]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][measuring_sqft]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 100px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][transfer_share]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][land_measuring_k]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][land_measuring_m]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>
                <td style="width: 80px; border: 1px solid var(--lm-border);"><input class="form-control" name="item_lines[${rownumber}][land_measuring_sqft]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;"></td>

                <td style="width: 160px; border: 1px solid var(--lm-border); padding: 0px;">
                    <select class="form-control" 
                            name="item_lines[${rownumber}][land_category]" style="border-radius: 0; border: none; margin: 0; height: 100%; padding: 4px;">
                        <option value="">Select</option>
                        <option value="Ownership">Ownership</option>
                        <option value="Non Pata">Non Pata</option>
                        <option value="Govt Land">Govt Land</option>
                    </select>
                </td>

                <td style="width: 80px; border: 1px solid var(--lm-border); text-align: center;"><button type="button" class="btn btn-sm btn-danger" onclick="deleteRow(this)"><i class="fas fa-trash"></i></button></td>

            </tr>
        `;

            $("#tbodyrow").append(row);

        });
        calculateTotals();

    });
</script>
<script>
    // Truncate decimal without rounding
    function truncateToDecimal(value, decimals) {
        const factor = Math.pow(10, decimals);
        return Math.trunc(value * factor) / factor;
    }

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

