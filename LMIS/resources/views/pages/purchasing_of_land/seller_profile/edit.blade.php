@extends('layouts.main')

@section('content')
<style>
    table {
        border-collapse: collapse;
        width: 100%;
    }

    th,
    td {
        border: 1px solid var(--lm-border);
        padding: 8px;
        text-align: left;
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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Owner Profile Master
                                        Data</h4>
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
                                <form class="row g-3 needs-validation" method="post"
                                    action="{{ route('seller_profile.update', $Seller_profile->id ) }}"
                                    novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    @method('PUT')
                                    <div class="row">

                                        <?php


                                        ?>
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_cod">LO Code</label>

                                                    <div class="input-group has-validation">
                                                        {{--<span class="input-group-text" id="inputGroupPrepend">Code</span>--}}
                                                        <input class="form-control" id="lo_cod" type="text"
                                                            required="" name="lo_cod"
                                                            value="{{ $Seller_profile->lo_cod }}" />

                                                        <div class="invalid-feedback">Please add LO Code.</div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="doc_no">Doc No</label>
                                                    <input class="form-control" id="doc_no" type="text"
                                                        name="doc_no" required=""
                                                        value="{{ $Seller_profile->doc_no }}" />

                                                    <div class="valid-feedback">Please Add Doc No</div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_name">LO Name as per Revenue Record</label>
                                                    <input class="form-control" id="lo_name" type="text"
                                                        name="lo_name"
                                                        value="{{ $Seller_profile->lo_name }}" />

                                                    <div class="invalid-feedback">Please add LO Name as per Revenue Record.</div>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Relationship</label>
                                                    <select class="form-control" name="relationship_revenue">
                                                        <option value="">Select</option>
                                                        <option value="S/O" {{ $Seller_profile->relationship_revenue == 'S/O' ? 'selected' : '' }}>S/O</option>
                                                        <option value="W/O" {{ $Seller_profile->relationship_revenue == 'W/O' ? 'selected' : '' }}>W/O</option>
                                                        <option value="D/O" {{ $Seller_profile->relationship_revenue == 'D/O' ? 'selected' : '' }}>D/O</option>
                                                        <option value="Widow of" {{ $Seller_profile->relationship_revenue == 'Widow of' ? 'selected' : '' }}>Widow of</option>
                                                    </select>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label" for="lo_father_name">Father / Husband Name</label>
                                                    <input class="form-control" id="lo_father_name" type="text"
                                                        name="lo_father_name"
                                                        value="{{ $Seller_profile->lo_father_name }}" />

                                                    <div class="invalid-feedback">Please add Father / Husband Name.</div>
                                                </div>


                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_name_as_per_cnic">LO Name as per CNIC Record</label>
                                                    <input class="form-control" id="lo_name_as_per_cnic" type="text"
                                                        name="lo_name_as_per_cnic" required=""
                                                        value="{{ $Seller_profile->lo_name_as_per_cnic }}" />

                                                    <div class="invalid-feedback">Please add LO Name as per CNIC Record.</div>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Relationship</label>
                                                    <select class="form-control" name="relationship_cnic">
                                                        <option value="">Select</option>
                                                        <option value="S/O" {{ $Seller_profile->relationship_cnic == 'S/O' ? 'selected' : '' }}>S/O</option>
                                                        <option value="W/O" {{ $Seller_profile->relationship_cnic == 'W/O' ? 'selected' : '' }}>W/O</option>
                                                        <option value="D/O" {{ $Seller_profile->relationship_cnic == 'D/O' ? 'selected' : '' }}>D/O</option>
                                                        <option value="Widow of" {{ $Seller_profile->relationship_cnic == 'Widow of' ? 'selected' : '' }}>Widow of</option>
                                                    </select>
                                                </div>

                                                <div class="col-md-3">
                                                    <label class="form-label">Father / Husband Name</label>
                                                    <input class="form-control" type="text"
                                                        name="father_name_cnic"
                                                        value="{{ $Seller_profile->father_name_cnic }}">
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_cnic">LO CNIC</label>
                                                    <input class="form-control" id="lo_cnic" type="text"
                                                        name="lo_cnic" required=""
                                                        value="{{ $Seller_profile->lo_cnic }}" />

                                                    <div class="invalid-feedback">Please add LO CNIC.</div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="contact_no">Contact NO</label>
                                                    <input class="form-control" id="contact_no" type="text"
                                                        name="contact_no" required=""
                                                        value="{{ $Seller_profile->contact_no }}" />

                                                    <div class="invalid-feedback">Please add Contact NO.</div>
                                                </div>
                                                
                                                <div class="col-md-3">
                                                    <label class="form-label" for="caste">Caste</label>
                                                    <input class="form-control" id="caste" type="text"
                                                        name="caste" required=""
                                                        value="{{ $Seller_profile->caste }}" />
                                                    <div class="invalid-feedback">Please add Caste.</div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="address">Permanent Address</label>
                                                    <input class="form-control" id="address" type="text"
                                                        name="address" required=""
                                                        value="{{ $Seller_profile->address }}" />

                                                    <div class="invalid-feedback">Please add Permanent Address.</div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="tem_address">Temporary Address</label>
                                                    <input class="form-control" id="tem_address" type="text" name="tem_address" value="{{ $Seller_profile->tem_address }}"  />
                                                    <div class="invalid-feedback">Please add Temporary Address.</div>
                                                    @error('tem_address')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                            </div>


                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">Profile Picture</label>
                                                <input class="form-control" id="customFile" name="attachments"
                                                    type="file" value="{{$Seller_profile->attachment}}" />
                                                @if($Seller_profile->attachment)
                                                <?php
                                                $filename = $Seller_profile->attachment;
                                                $extension = pathinfo($filename, PATHINFO_EXTENSION); ?>
                                                @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                <img src="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->attachment;}}"
                                                    style="width: 200px;height: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                @else
                                                <a target="_blank"
                                                    href="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->attachment; }}">
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
                                                <input class="form-control" id="customFile" name="cnic_front_attachments" type="file" value="{{$Seller_profile->cnic_front_attachments}}" />
                                                @if($Seller_profile->cnic_front_attachments)
                                                <?php
                                                $filename = $Seller_profile->cnic_front_attachments;
                                                $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                ?>
                                                @if($extension == 'jpg' || $extension == 'jpeg' ||$extension == 'pgifng' ||$extension == 'png' )
                                                <img src="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->cnic_front_attachments; }}"
                                                    style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                @else
                                                <a target="_blank"
                                                    href="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->cnic_front_attachments; }}">
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
                                                <input class="form-control" id="customFile" name="cnic_back_attachments" type="file" value="{{$Seller_profile->cnic_back_attachments}}" />
                                                @if($Seller_profile->cnic_back_attachments)
                                                <?php
                                                $filename = $Seller_profile->cnic_back_attachments;
                                                $extension = pathinfo($filename, PATHINFO_EXTENSION);

                                                ?>
                                                @if($extension == 'jpg' || $extension == 'jpeg' ||$extension == 'pgifng' ||$extension == 'png' )
                                                <img src="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->cnic_back_attachments; }}"
                                                    style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                @else
                                                <a target="_blank"
                                                    href="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->cnic_back_attachments; }}">
                                                    <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                        style="width: 240px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                </a>
                                                @endif
                                                {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$Seller_profile->attachments; }}" style="width: 240px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                @endif
                                            </div>
                                        </div>

                                        <!-- <div class="col-md-12" style="margin-top: 20px">
                                                <div class="card">

                                                    <div class="card-body">
                                                        <p class="card-title btn btn-success"  id="add_row" >Add Row</p>
                                                        <div class="row">

                                                            <table>
                                                                <thead>
                                                                <tr>
                                                                    <th>Khewat No</th>
                                                                    <th>Khatooni No</th>
                                                                    <th>Rectangle No</th>
                                                                    <th>Muraba No</th>
                                                                    <th>Khasra No</th>
                                                                    <th>Kanal</th>
                                                                    <th>Marla</th>
                                                                    <th>Sq Feet</th>
                                                                </tr>
                                                                </thead>
                                                                <tbody id="tbodyrow">
                                                                <?php
                                                                $lineCount = 1;

                                                                ?>
                                                                @foreach($Seller_profile->rows as $row)

                                                                    <input type="hidden" value="{{ $row['id'] }}" name="item_lines[{{$lineCount}}][id]">
                                                                <tr id="1">
                                                                    <td><input class="row-level form-control"  name="item_lines[{{$lineCount}}][khewat_no]" value="{{ $row['khewat_no'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][khatooni_no]" value="{{ $row['khatooni_no'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][rectangle_no]" value="{{ $row['rectangle_no'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][muraba_no]" value="{{ $row['muraba_no']}}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][khasra_no]" value="{{ $row['khasra_no'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][kanal]" value="{{ $row['kanal'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][marla]" value="{{ $row['marla'] }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[{{$lineCount}}][sq_feet]" value="{{ $row['sq_feet']}}"></td>
                                                                </tr>
                                                                    <?php
                                                                    $lineCount++;

                                                                    ?>
                                                                    @endforeach
                                                                </tbody>
                                                            </table>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div> -->

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


@endsection