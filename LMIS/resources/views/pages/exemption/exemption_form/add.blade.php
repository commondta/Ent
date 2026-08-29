@extends('layouts/main')

@section('content')
    <style>
        th{
            border: 1px solid var(--lm-border)!important;
            text-align: center;
            background-color: var(--lm-surface);
        }
        td{
            border : 1px solid var(--lm-border)!important;
            width: 130px;

        }
        .row-level{
            border: none;
            width: 130px;
        }
        input.row-level:focus {
            outline: none; /* Remove the default focus outline */
            border: none;  /* Remove the border */
        }
        
    </style>
    <style>
        table {
            border-collapse: collapse;
            width: 100%;
        }
        th, td {
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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Exemption Form</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('exemption_form.store') }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-6">
                                                        <label class="form-label" for="doc_no">Doc No.</label>
                                                        <input class="form-control" id="doc_no" type="text" name="doc_no"  value="{{$doc_no+1}}"  readonly required="" />
                                                        <div class="valid-feedback">Please Add Doc No..</div>
                                                        @error('doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-6">
                                                        <label class="form-label" for="date"> Date</label>
                                                        <?php
                                                        $dt = new DateTime();
                                                        ?>
                                                        <input class="form-control" id="date" type="date"
                                                               name="date" required=""
                                                               value="{{$dt->format('Y-m-d')}}"/>

                                                        <div class="valid-feedback">Please Add Doc Date</div>
                                                        @error('date')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="base_doc_no">Base Doc No</label>
                                                        <select id="base_doc_no" name="base_doc_no" class="form-control"
                                                                required="">
                                                            <option value="">Kindly Select</option>
                                                            @foreach($purchase_of_land as $row)
                                                                <option value="{{ $row->File_No }}">Base Doc No - {{ $row->File_No }}</option>
                                                            @endforeach
                                                        </select>

                                                        <div class="invalid-feedback">Please select Purchase of Land.</div>
                                                        @error('base_doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="file_no">File Name</label>
                                                            <input class="form-control"
                                                                   id="file_no"
                                                                   name="file_no"
                                                                   type="text" 
                                                                   value="{{ old('file_no') }}"
                                                                   required=""/>
                                                        </div>

                                                    </div>  <div class="col-md-4">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="lo_name">LO Name</label>
                                                            <input class="form-control"
                                                                   id="lo_name"
                                                                   name="lo_name"
                                                                   type="text" 
                                                                   value="{{ old('lo_name') }}"
                                                                   required=""/>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-4">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="so">so</label>
                                                            <input class="form-control"
                                                                   id="so"
                                                                   name="so"
                                                                   type="text" 
                                                                   value="{{ old('so') }}"
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
                                                                   value="{{ old('lp_name') }}"
                                                                   required=""/>
                                                        </div>

                                                    </div>

                                                    <div class="col-md-12" style="margin-top: 20px">
                                                        <div class="card">

                                                            <div class="card-body">

                                                                <h5 class="card-title" >Land Details</h5>
                                                                <div class="row">

                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="reg_no">Reg No</label>
                                                                            <input class="form-control"
                                                                                   id="reg_no"
                                                                                   name="reg_no"
                                                                                   type="text" 
                                                                                   value="{{ old('reg_no') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <label class="form-label" for="reg_date"> Date</label>
                                                                        <?php
                                                                        $dt = new DateTime();
                                                                        ?>
                                                                        <input class="form-control" id="reg_date" type="reg_date"
                                                                               name="reg_date" required=""
                                                                               value="{{$dt->format('Y-m-d')}}"/>

                                                                        <div class="valid-feedback">Please Add Doc Date</div>
                                                                        @error('reg_date')
                                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                                        @enderror
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="mouza">Mouza</label>
                                                                            <input class="form-control"
                                                                                   id="mouza"
                                                                                   name="mouza"
                                                                                   type="text" 
                                                                                   value="{{ old('mouza') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="exemption_rate">Exemption Rate</label>
                                                                            <input class="form-control"
                                                                                   id="exemption_rate"
                                                                                   name="exemption_rate"
                                                                                   type="text" 
                                                                                   value="{{ old('exemption_rate') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>


                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="total_files">Total Files</label>
                                                                            <input class="form-control"
                                                                                   id="total_files"
                                                                                   name="total_files"
                                                                                   type="text"
                                                                                   value="{{ old('total_files') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="file_security">20% File Security</label>
                                                                            <input class="form-control"
                                                                                   id="file_security"
                                                                                   name="file_security"
                                                                                   type="text"
                                                                                   value="{{ old('file_security') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="balance">Balance</label>
                                                                            <input class="form-control"
                                                                                   id="balance"
                                                                                   name="balance"
                                                                                   type="text"
                                                                                   value="{{ old('balance') }}"
                                                                                   required=""/>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <div class="mb-3">
                                                                            <label class="form-label"
                                                                                   for="transfer_of_decimals">Transfer of Decimals</label>
                                                                            <input class="form-control"
                                                                                   id="transfer_of_decimals"
                                                                                   name="transfer_of_decimals"
                                                                                   type="text"
                                                                                   value="{{ old('transfer_of_decimals') }}"
                                                                                   required=""/>
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
                                                                                            <th>Khewat No</th>
                                                                                            <th>Khatooni No</th>
                                                                                            <th>Qatat</th>
                                                                                            <th>Kanal</th>
                                                                                            <th>Marla</th>
                                                                                            <th>Sq Feet</th>
                                                                                        </tr>
                                                                                        </thead>
                                                                                        <tbody id="tbodyrow">
                                                                                        <tr id="1">
                                                                                            <td><input class="row-level form-control" name="item_lines[1][khewat_no]" value="{{ old('item_lines[1][khewat_no]') }}"></td>
                                                                                            <td><input class="row-level form-control" name="item_lines[1][khatooni_no]" value="{{ old('item_lines[1][khatooni_no]') }}"></td>
                                                                                            <td><input class="row-level form-control" name="item_lines[1][qatat]" value="{{ old('item_lines[1][qatat]') }}"></td>
                                                                                             <td><input class="row-level form-control" name="item_lines[1][kanal]" value="{{ old('item_lines[1][kanal]') }}"></td>
                                                                                            <td><input class="row-level form-control" name="item_lines[1][marla]" value="{{ old('item_lines[1][marla]') }}"></td>
                                                                                            <td><input class="row-level form-control" name="item_lines[1][sq_feet]" value="{{ old('item_lines[1][sq_feet]') }}"></td>
                                                                                        </tr>
                                                                                        </tbody>
                                                                                    </table>


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
    <input type="hidden" id="rownumber_participant" value="100">

    <script>


        $('#base_doc_no').change(function() {
            var selectedValue = $(this).val();

            $.ajax({
                url: baseUrl+'/get_purchase_of_land',
                type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue }), // You can send data to the server if required
                contentType: 'application/json', // Set the appropriate content type
                success: function(data) {

                    console.log(data);
                    $('#file_no').val(data.File_No);
                    $('#lo_name').val(data.lo_name);
                    $('#lp_name').val(data.lp_name);
                    $('#so').val(data.so);
                    $('#mouza').val(data.mouza);
                    $('#exemption_rate').val(data.exemption_rate);
                    $('#kanal').val(data.kanal);
                    $('#marla').val(data.marla);
                    $('#exemption_rate').val(data.khewat_no);
                    $('#sq_feet').val(data.sq_feet);
                    $('#khewat').val(data.khewat_no);
                    $('#qatat').val(data.qatat);
                    $('#khatooni').val(data.khatoni);





                    $('#lo_name').prop('readonly', true);
                    $('#lp_name').prop('readonly', true);
                    $('#mouza').prop('readonly', true);
//                    $('#exemption_rate').prop('readonly', true);
                    $('#sq_feet').prop('readonly', true);
                    $('#khewat').prop('readonly', true);
                    $('#qatat').prop('readonly', true);
                    $('#khatooni').prop('readonly', true);

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
        $(function(){
            $('#add_row').click(function() {
                var rownumber = parseFloat($("#rownumber").val());
                var LineId = rownumber;
                rownumber = rownumber + 1;
                $("#rownumber").val(rownumber);


                var row = '<tr id="' + rownumber + '" DetailId="0"> ' +
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][khewat_no]"   value="{{ old("") }}"> </td>'+
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][khatooni_no]"   value="{{ old("") }}"> </td>'+
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][qatat]"   value="{{ old("") }}"> </td>'+
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][kanal]"   value="{{ old("") }}"> </td>'+
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][marla]"   value="{{ old("") }}"> </td>'+
                        '<td><input class="row-level form-control"  name="item_lines[' + rownumber + '][sq_feet]"   value="{{ old("") }}"> </td>'+
                        '</tr>';

                $("#tbodyrow").append(row);

            });
        });

    </script>
@endsection