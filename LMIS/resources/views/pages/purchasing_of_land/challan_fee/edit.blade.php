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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Exemption Rate</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('challan_fee.update',$challan_fee->id) }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        @method('PUT')
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-4">
                                                        <label class="form-label" for="sr_code">Sr.</label>
                                                        <input class="form-control" id="sr_code" type="text" name="sr_code" readonly value="{{$challan_fee->sr_code}}" required="" />
                                                        <div class="valid-feedback">Please Add Sr.</div>
                                                        @error('sr_code')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-4">
                                                        <label class="form-label" for="category">Category</label>
                                                        <input class="form-control" id="category" type="text" name="category" value="{{ $challan_fee->category}}" required="" />
                                                        <div class="invalid-feedback">Please add Category.</div>
                                                        @error('category')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-4">
                                                        <label class="form-label" for="amount">Amount</label>
                                                        <input class="form-control" id="amount" type="text" name="amount" value="{{ $challan_fee->amount }}" required="" />
                                                        <div class="invalid-feedback">Please add Amount.</div>
                                                        @error('amount')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
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

    <script>


            $('#mySelect').change(function() {
                var selectedValue = $(this).val();

                $.ajax({
                    url: baseUrl+'/get_seller_data',
                    type: 'POST', // or 'GET', 'PUT', 'DELETE', etc. depending on your API
                    data: JSON.stringify({"_token": "{{ csrf_token() }}", value: selectedValue }), // You can send data to the server if required
                    contentType: 'application/json', // Set the appropriate content type
                    success: function(data) {

                        $('#lo_cnic').val(data.lo_cnic);
                        $('#mouza_name').val(data.mouza_name);
                        $('#so').val(data.so);
                        $('#mouza').val(data.mouza);
                        $('#contact_no').val(data.contact_no);

                        $('#contact_no').prop('readonly', true);
                        $('#mouza').prop('readonly', true);
                        $('#so').prop('readonly', true);
                        $('#mouza_name').prop('readonly', true);
                        $('#lo_cnic').prop('readonly', true);
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