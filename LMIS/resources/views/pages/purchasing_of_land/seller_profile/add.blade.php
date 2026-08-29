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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Owner Profile Master Data</h4>
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
                                <form onsubmit="return validateForm()" class="row g-3 needs-validation" method="post" action="{{ route('seller_profile.store') }}" novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_cod">LO Code</label>
                                                    <div class="input-group has-validation">
                                                        <input class="form-control" id="lo_cod" type="text" required="" name="lo_cod" readonly value="{{$lo_code+1}}" />
                                                        <div class="invalid-feedback">Please add LO Code.</div>
                                                        @error('lo_cod')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="doc_no">Doc No</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" required="" readonly value="{{$doc_num+1}}" />
                                                    <div class="valid-feedback">Please Add Doc No</div>
                                                    @error('doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_name">LO Name as per Revenue Record</label>
                                                    <input class="form-control" id="lo_name" type="text" name="lo_name" value="{{ old('lo_name') }}" />
                                                    <div class="invalid-feedback">Please add LO Name as per Revenue Record.</div>
                                                    @error('lo_name')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label">Relationship</label>
                                                    <select class="form-control" name="relationship_revenue" >
                                                        <option value="">Select</option>
                                                        <option value="S/O">S/O</option>
                                                        <option value="W/O">W/O</option>
                                                        <option value="D/O">D/O</option>
                                                        <option value="Widow of">Widow of</option>
                                                    </select>
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="lo_father_name">Father / Husband Name</label>
                                                    <input class="form-control" id="lo_father_name" type="text" name="lo_father_name" value="{{ old('lo_father_name') }}" />
                                                    <div class="invalid-feedback">Please add Father / Husband Name.</div>
                                                    @error('lo_father_name')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_name_as_per_cnic">LO Name as per CNIC Record</label>
                                                    <input class="form-control" id="lo_name_as_per_cnic" type="text" name="lo_name_as_per_cnic" required="" value="{{ old('lo_name_as_per_cnic') }}" />
                                                    <div class="invalid-feedback">Please add LO Name as per CNIC Record.</div>
                                                    @error('lo_name_as_per_cnic')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label">Relationship</label>
                                                    <select class="form-control" name="relationship_cnic">
                                                        <option value="">Select</option>
                                                        <option value="S/O">S/O</option>
                                                        <option value="W/O">W/O</option>
                                                        <option value="D/O">D/O</option>
                                                        <option value="Widow of">Widow of</option>
                                                    </select>
                                                </div>
                                                <!-- CNIC Father/Husband -->
                                                <div class="col-md-3">
                                                    <label class="form-label">Father / Husband Name</label>
                                                    <input class="form-control" type="text" name="father_name_cnic"
                                                        value="{{ old('father_name_cnic') }}">
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="lo_cnic">LO CNIC</label>
                                                    <input class="form-control" id="lo_cnic" type="number" min="13" name="lo_cnic" required="" value="{{ old('lo_cnic') }}" />
                                                    <div class="invalid-feedback">Please add LO CNIC.</div>
                                                    @error('lo_cnic')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="contact_no">Contact NO</label>
                                                    <input class="form-control" id="contact_no" type="text" name="contact_no" required="" value="{{ old('contact_no') }}" />
                                                    <div class="invalid-feedback">Please add Contact NO.</div>
                                                    @error('contact_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-3">
                                                    <label class="form-label" for="caste">Caste</label>
                                                    <input class="form-control" id="caste" type="text" name="caste" required="" value="{{ old('caste') }}" />
                                                    <div class="invalid-feedback">Please add Caste.</div>
                                                    @error('caste')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="address">Permanent Address</label>
                                                    <input class="form-control" id="address" type="text" name="address" required="" value="{{ old('address') }}" />
                                                    <div class="invalid-feedback">Please add Permanent Address.</div>
                                                    @error('address')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="tem_address">Temporary Address</label>
                                                    <input class="form-control" id="tem_address" type="text" name="tem_address" value="{{ old('tem_address') }}"  />
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
                                                <input class="form-control" id="customFile" name="attachments" type="file" required="" value="{{ old('file') }}" />
                                            </div>
                                        </div>

                                        <div class="col-md-4">

                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">CNIC front Picture</label>
                                                <input class="form-control" id="customFile" name="cnic_front_attachments" type="file" value="{{ old('file') }}" required="" />
                                            </div>

                                        </div>
                                        <div class="col-md-4">

                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">CNIC Back Picture</label>
                                                <input class="form-control" id="customFile" name="cnic_back_attachments" type="file" value="{{ old('file') }}" required="" />
                                            </div>

                                        </div>



                                        <!-- <div class="col-md-4">
                                                <label class="form-label" for="lp_code">Land Provider</label>
                                                <select name="lp_code" class="form-control" id="lp_code" required="" value="{{ old('lp_code') }}" >
                                                    <option value="">Kindly Select Land Provider</option>


                                                    @foreach($record as $row)
                                                        <option value="{{ $row->lp_cod }}">{{ $row->lp_name }}</option>
                                                    @endforeach
                                                </select>
                                                <div class="invalid-feedback">Please add Land Provider.</div>
                                            </div> -->

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
                                                                <tr id="1">
                                                                    <td><input class="row-level form-control" name="item_lines[1][khewat_no]" value="{{ old('item_lines[1][khewat_no]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][khatooni_no]" value="{{ old('item_lines[1][khatooni_no]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][rectangle_no]" value="{{ old('item_lines[1][rectangle_no]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][muraba_no]" value="{{ old('item_lines[1][muraba_no]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][khasra_no]" value="{{ old('item_lines[1][khasra_no]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][kanal]" value="{{ old('item_lines[1][kanal]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][marla]" value="{{ old('item_lines[1][marla]') }}"></td>
                                                                    <td><input class="row-level form-control" name="item_lines[1][sq_feet]" value="{{ old('item_lines[1][sq_feet]') }}"></td>
                                                                </tr>
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

<script>
    function validateForm() {
        debugger;
        const inputField = document.getElementById('lp_cnic');
        const inputValue = inputField.value;

        // Remove any non-digit characters
        const digitsOnly = inputValue.replace(/\D/g, '');

        // Check if the input has exactly 13 digits
        if (digitsOnly.length !== 13) {
            $(".invalid-feedback.cnic").css("display", "block");
            //                const feedbackElement = document.querySelector('.invalid-feedback .cnic');
            //                feedbackElement.style.display = 'block';
            return false; // Prevent form submission
        }

        // If the validation passes, the form will be submitted
        return true;
    }
</script>
<script>
    $(function() {
        $('#add_row').click(function() {
            var rownumber = parseFloat($("#rownumber").val());
            var LineId = rownumber;
            rownumber = rownumber + 1;
            $("#rownumber").val(rownumber);


            var row = '<tr id="' + rownumber + '" DetailId="0"> ' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][khewat_no]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][khatooni_no]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][rectangle_no]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][muraba_no]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][khasra_no]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][kanal]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][marla]"   value="{{ old("") }}"> </td>' +
                '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][sq_feet]"   value="{{ old("") }}"> </td>' +
                '</tr>';

            $("#tbodyrow").append(row);

        });
    });
</script>

@endsection