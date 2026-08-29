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
    <div class="content">
        <div class="mt-4">
            <div class="row g-4">
                <div class="col-12 col-xl-12 order-1 order-xl-0">
                    <div class="mb-9">
                        <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                            <div class="card-header p-4 border-bottom border-300 bg-soft">
                                <div class="row g-3 justify-content-between align-items-center">
                                    <div class="col-12 col-md">
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Indemnity Bond</h4>
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
                                        <form class="row g-3 needs-validation" method="post" action="{{ route('indemnity_bond.update',$indemnity_bond->id) }}" novalidate=""  enctype="multipart/form-data">
                                            @csrf
                                            @method('PUT')
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-4">
                                                        <label class="form-label" for="doc_no">Doc No.</label>
                                                        <input class="form-control" id="doc_no" type="text" name="doc_no"  value="{{$indemnity_bond->doc_no}}"  readonly required="" />
                                                        <div class="valid-feedback">Please Add Doc No..</div>
                                                        @error('doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-4">
                                                        <label class="form-label" for="date"> Date</label>
                                                        <?php
                                                        $dt = new DateTime();
                                                        ?>
                                                        <input class="form-control" id="date" type="date"
                                                               name="date" required=""
                                                               value="{{$indemnity_bond->date}}"/>

                                                        <div class="valid-feedback">Please Add Doc Date</div>
                                                        @error('date')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="date_of_execution">Date of Execution</label>
                                                        <?php
                                                        $dt = new DateTime();
                                                        ?>
                                                        <input class="form-control" id="date_of_execution" type="date_of_execution"
                                                               name="date_of_execution" required=""
                                                               value="{{$indemnity_bond->date_of_execution}}"/>

                                                        <div class="valid-feedback">Please Add Date of Creation</div>
                                                        @error('date_of_execution')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-12">
                                                        <label class="form-label" for="base_doc_no">Base Doc No</label>
                                                        <select id="base_doc_no" name="base_doc_no" class="form-control"
                                                                required="">
                                                            <option value="">Kindly Select</option>
                                                            @foreach($conveyance as $row)
                                                                <option @if($indemnity_bond->base_doc_no == $row->doc_no ) selected @endif value="{{ $row->doc_no }}">File No - {{ $row->doc_no }}</option>
                                                            @endforeach
                                                        </select>

                                                        <div class="invalid-feedback">Please select Base Doc No.</div>
                                                        @error('base_doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
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
                url: baseUrl+'/get_conveyance_deed',
                type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue }), // You can send data to the server if required
                contentType: 'application/json', // Set the appropriate content type
                success: function(data) {

                    console.log(data);
                    $('#lo_name').val(data.lo_name);
                     $('#lo_father_name').val(data.deed_executed_by_lo_father_name);
                    $('#lo_cnic').val(data.lo_cnic);
                    $('#lo_caste').val(data.deed_executed_by_caste);
                    $('#lo_address').val(data.lo_address);
                    $('#chak').val(data.chak_no);
                    $('#tehsil').val(data.tehsil);
                    $('#b_name').val(data.deed_in_favor_of_name);
                    $('#principle_office').val(data.deed_in_favor_of_principal_office);
                    $('#project_office').val(data.deed_in_favor_of_project_office);
                    $('#b_representative').val(data.deed_in_favor_of_representative);
                    $('#khewat').val(data.khewat_no);
                    $('#khatooni').val(data.khatooni_no);
                    $('#qatat').val(data.qatat);
                    $('#transfer_share').val(data.transferred_share);
                    $('#vide_fad_id_no').val(data.vide_fad_id_no);
                    $('#chak_no').val(data.chak_no);
                    $('#tehsil_no').val(data.tehsil);
                    $('#district_no').val(data.tehsil);





                    $('#lo_name').prop('readonly', true);
                    $('#lo_father_name').prop('readonly', true);
                    $('#lo_cnic').prop('readonly', true);
                    $('#lo_caste').prop('readonly', true);
                    $('#lo_address').prop('readonly', true);
                    $('#chak').prop('readonly', true);
                    $('#tehsil').prop('readonly', true);
                    $('#project_office').prop('readonly', true);
                    $('#b_representative').prop('readonly', true);
                    $('#khewat').prop('readonly', true);
                    $('#khatooni').prop('readonly', true);
                    $('#qatat').prop('readonly', true);
                    $('#transfer_share').prop('readonly', true);
                    $('#vide_fad_id_no').prop('readonly', true);
                    $('#chak_no').prop('readonly', true);
                    $('#tehsil_no').prop('readonly', true);
                    $('#district_no').prop('readonly', true);


                    // Do something with the data (e.g., update content on the page)
                },
                error: function(error) {
                    // Handle any errors that occurred during the AJAX call
                    console.error('Error:', error);
                }
            });

        });

    </script>



@endsection