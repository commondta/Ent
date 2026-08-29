@extends('layouts/main')

@section('content')
    <style>
        th {
            border: 1px solid var(--lm-border) !important;
            text-align: center;
            background-color: var(--lm-surface);
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
            outline: none; /* Remove the default focus outline */
            border: none; /* Remove the border */
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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Intimation Letter</h4>
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
                                              method="post" action="{{ route('intimation_letter.update',$intimation_letter->id) }}" novalidate=""
                                              enctype="multipart/form-data">
                                            @csrf
                                            @method('PUT')
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-6">
                                                        <label class="form-label" for="doc_no">Doc No.</label>
                                                        <input class="form-control" id="doc_no" type="text"
                                                               name="doc_no" value="{{$intimation_letter->doc_no}}" readonly
                                                               required=""/>

                                                        <div class="valid-feedback">Please Add Doc No..</div>
                                                        @error('doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-6">
                                                        <label class="form-label" for="date"> Date</label>
                                                        <?php
                                                        $dt = new DateTime();
                                                        ?>
                                                        <input class="form-control" id="date" type="date"
                                                               name="date" required=""
                                                               value="{{$intimation_letter->date}}"/>

                                                        <div class="valid-feedback">Please Add Doc Date</div>
                                                        @error('date')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">

                                                        <label class="form-label" for="application_no">Application
                                                            No</label>
                                                        <select id="base_doc_no" name="application_no"
                                                                class="form-control"
                                                                required="">
                                                            <option value="">Kindly Select</option>
                                                            @foreach($intimation_applicaion as $row)
                                                                <option @if($intimation_letter->application_no == $row->doc_no ) selected @endif value="{{ $row->doc_no }}">Intimation
                                                                    Application - {{ $row->doc_no     }}</option>
                                                            @endforeach
                                                        </select>

                                                        <div class="invalid-feedback">Please select Lo Name</div>
                                                        @error('base_doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">

                                                        <label class="form-label" for="file_no">File No </label>
                                                        <select id="file_no" name="file_no" class="form-control"
                                                                >
                                                            <option value="">Kindly Select</option>
                                                            @foreach($exemption_form as $row)
                                                                <option @if($intimation_letter->file_no == $row->doc_no ) selected @endif  value="{{ $row->doc_no }}">Exemption Form
                                                                    - {{ $row->doc_no }}</option>
                                                            @endforeach
                                                        </select>

                                                        <div class="invalid-feedback">Please select Lo Name</div>
                                                        @error('file_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>


                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <input type="hidden" name="lo_code" id="lo_code" value="{{$intimation_letter->lo_code}}">
                                                            <label class="form-label"
                                                                   for="lo_name">LO Name</label>
                                                            <input class="form-control"
                                                                   id="lo_name"
                                                                   name="lo_name"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->lo_name }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="lo_father_name">LO Father Name</label>
                                                            <input class="form-control"
                                                                   id="lo_father_name"
                                                                   name="lo_father_name"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->lo_father_name }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="lo_address">LO Address</label>
                                                            <input class="form-control"
                                                                   id="lo_address"
                                                                   name="lo_address"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->lo_address }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>


                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="code_no">Code NO</label>
                                                            <input class="form-control"
                                                                   id="code_no"
                                                                   name="code_no"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->code_no }}"
                                                                   required=""/>
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
                                                                   value="{{ $intimation_letter->district }}"
                                                                   required=""/>
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
                                                                   value="{{ $intimation_letter->tehsil }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="lp_name">LP Name</label>
                                                            <input class="form-control"
                                                                   id="lp_name"
                                                                   name="lp_name"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->lp_name }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="lp_father_name">LP Father Name</label>
                                                            <input class="form-control"
                                                                   id="lp_father_name"
                                                                   name="lp_father_name"
                                                                   type="text"
                                                                   value="{{ $intimation_letter->lp_father_name }}"
                                                                   required=""/>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="affidavit_no">Affidavit No</label>
                                                            <select id="affidavit_no" name="affidavit_no" class="form-control"
                                                                    >
                                                                <option value="">Kindly Select</option>
                                                                @foreach($affidavit as $row)
                                                                    <option @if($intimation_letter->affidavit_no == $row->doc_no) selected @endif value="{{ $row->doc_no }}">Affidavit No
                                                                        - {{ $row->doc_no }}</option>
                                                                @endforeach
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" style="margin-top: 20px">
                                                        <div class="card">

                                                            <div class="card-body">
                                                                <p class="card-title btn btn-success"  id="add_row" >Add Row</p>
                                                                <div class="row">

                                                                    <table>
                                                                        <thead>
                                                                        <tr>
                                                                            <th>Purchaser Name</th>
                                                                            <th>Purchaser Address</th>
                                                                            <th>Purchaser CNIC</th>
                                                                        </tr>
                                                                        </thead>
                                                                        <tbody id="tbodyrow">

                                                                        <?php
                                                                        $lineCount = 100;

                                                                        ?>
                                                                        @foreach($intimation_letter['rows'] as $rows)
                                                                            <tr id="{{$lineCount}}">
                                                                                <input type="hidden" name="item_lines[{{$lineCount}}][id]" value="{{$rows['id']}}">
                                                                                <td><input class="row-level" name="item_lines[{{$lineCount}}][purchaser_name]"  value="{{ $rows['purchaser_name'] }}"> </td>
                                                                                <td><input class="row-level" name="item_lines[{{$lineCount}}][purchaser_address]"  value="{{$rows['purchaser_address'] }}"> </td>
                                                                                <td><input class="row-level" name="item_lines[{{$lineCount}}][purchaser_cnic]" value="{{ $rows['purchaser_cnic'] }}"> </td>
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
    <input type="hidden" id="rownumber" value="1">
    <input type="hidden" id="rownumber_participant" value="100">

    <script>


        $('#base_doc_no').change(function () {
            var selectedValue = $(this).val();

            $.ajax({
                url: baseUrl+'/get_intimation_application',
                type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue}), // You can send data to the server if required
                contentType: 'application/json', // Set the appropriate content type
                success: function (data) {

                    $('#lo_code').val(data.lo_code);
                    $('#lo_name').val(data.lo_name);
                    $('#lo_address').val(data.lo_address);
                    $('#lo_father_name').val(data.lo_father_name);

                    $('#lo_code').prop('readonly', true);
                    $('#lo_name').prop('readonly', true);
                    $('#lo_cnic').prop('readonly', true);
                    $('#lo_father_name').prop('readonly', true);


                    // Do something with the data (e.g., update content on the page)
                },
                error: function (error) {
                    // Handle any errors that occurred during the AJAX call
                    console.error('Error:', error);
                }
            });

        });
        $('#file_no').change(function () {
            var selectedValue = $(this).val();

            $.ajax({
                url: baseUrl+'/get_exemption_form',
                type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue}), // You can send data to the server if required
                contentType: 'application/json', // Set the appropriate content type
                success: function (data) {

                    console.log(data);
                    $('#lp_name').val(data.lp_name);

                    $('#lp_name').prop('readonly', true);


                    // Do something with the data (e.g., update content on the page)
                },
                error: function (error) {
                    // Handle any errors that occurred during the AJAX call
                    console.error('Error:', error);
                }
            });

        });

    </script>



@endsection