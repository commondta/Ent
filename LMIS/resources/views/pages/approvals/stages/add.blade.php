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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Add New Stage</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('approval_stage.store') }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">

                                                    <div class="col-md-6">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="name">Stage Name</label>
                                                            <input class="form-control"
                                                                   id="name"
                                                                   name="name"
                                                                   type="text" 
                                                                   value="{{ old('name') }}"
                                                                   required=""/>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6">

                                                        <div class="mb-3">
                                                            <input type="hidden" id="lo_code" name="lo_code" value="">
                                                            <label class="form-label"
                                                                   for="no_of_approvals">No of Approvals</label>
                                                            <input class="form-control"
                                                                   id="no_of_approvals"
                                                                   name="no_of_approvals"
                                                                   type="text" 
                                                                   value="{{ old('no_of_approvals') }}"
                                                                   required=""/>
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
                url: baseUrl+'/get_exemption_form',
                type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue }), // You can send data to the server if required
                contentType: 'application/json', // Set the appropriate content type
                success: function(data) {

                    console.log(data);
                    $('#name').val(data.name);
                    $('#lo_code').val(data.lo_code);
                    $('#no_of_approvals').val(data.no_of_approvals);
                    $('#lo_cnic').val(data.lo_cnic);
                    $('#lo_address').val(data.lo_address);
                    $('#lp_name').val(data.lp_name);
                    $('#lp_cnic').val(data.lp_cnic);
                    $('#mouza').val(data.mouza);
                    $('#kanal').val(data.kanal);
                    $('#khewat').val(data.khewat);
                    $('#marla').val(data.marla);
                    $('#qatat').val(data.qatat);
                    $('#khatooni').val(data.khatooni);






                    $('#name').prop('readonly', true);
                    $('#no_of_approvals').prop('readonly', true);
                    $('#lo_cnic').prop('readonly', true);
                    $('#lo_address').prop('readonly', true);
                    $('#lp_name').prop('readonly', true);
                    $('#lp_cnic').prop('readonly', true);
                    $('#mouza').prop('readonly', true);
                    $('#kanal').prop('readonly', true);
                    $('#khewat').prop('readonly', true);
                    $('#marla').prop('readonly', true);
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



@endsection