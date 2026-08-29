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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Provider Master Data</h4>
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
                                <form onsubmit="return validateForm()" class="row g-3 needs-validation" method="post" action="{{ route('land_provider.store') }}" novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="row">

                                                <div class="col-md-6">
                                                    <label class="form-label" for="doc_no">Doc No</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" readonly value="{{$doc_num+1}}" required="" />
                                                    <div class="valid-feedback">Please Add Doc No</div>
                                                    @error('doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="lp_cod">LP Code</label>

                                                    <div class="input-group has-validation">
                                                        {{--<span class="input-group-text" id="inputGroupPrepend">Code</span>--}}
                                                        <input class="form-control" id="lp_cod" type="text" required="" name="lp_cod" readonly value="{{$lp_code+1}}" />
                                                        <div class="invalid-feedback">Please add Land Provider Code.</div>
                                                        @error('lp_cod')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="lp_name">LP Name</label>
                                                    <input class="form-control" id="lp_name" type="text" name="lp_name" value="{{ old('lp_name') }}" required="" />
                                                    <div class="invalid-feedback">Please add Land Provider Name.</div>
                                                    @error('lp_name')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label">Relationship</label>
                                                    <select class="form-control" name="relationship" required="">
                                                        <option value="">Select</option>
                                                        <option value="S/O">S/O</option>
                                                        <option value="W/O">W/O</option>
                                                        <option value="D/O">D/O</option>
                                                        <option value="Widow of">Widow of</option>
                                                    </select>
                                                    <div class="invalid-feedback">Please select Relationship.</div>
                                                   
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="father_name">Father / Husband Name</label>
                                                    <input class="form-control" id="father_name" type="text" name="father_name" value="{{ old('father_name') }}" required="" />
                                                    <div class="invalid-feedback">Please add Father / Husband Name.</div>
                                                    @error('father_name')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="lp_cnic">LP CNIC</label>
                                                    <input class="form-control" id="lp_cnic" pattern="\d{13,13}" min="1" type="number" name="lp_cnic" value="{{ old('lp_cnic') }}" required="" />
                                                    <div class="invalid-feedback cnic">Please add Valid 13 digit CNIC.</div>
                                                    @error('lp_cnic')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="contact_no">Contact NO</label>
                                                    <input class="form-control" id="contact_no" type="number" name="contact_no" value="{{ old('contact_no') }}" required="" />
                                                    <div class="invalid-feedback">Please add Contact NO.</div>
                                                    @error('contact_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>

                                                <div class="col-md-6">
                                                    <label class="form-label" for="ntn_no">NTN No</label>
                                                    <input class="form-control" id="ntn_no" type="text" name="ntn_no" value="{{ old('ntn_no') }}" required="" />
                                                    <div class="invalid-feedback">Please add NTN No.</div>
                                                    @error('ntn_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label" for="address">Permanent Address</label>
                                                    <input class="form-control" id="address" type="text" name="address" value="{{ old('address') }}" required="" />
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


                                                <div class="col-md-6">
                                                    <label class="form-label" for="security_deposited">Security Deposited</label>
                                                    <input class="form-control" id="security_deposited" type="text" name="security_deposited" value="{{ old('security_deposited') }}" required="" />
                                                    <div class="invalid-feedback">Please add Security Deposited.</div>
                                                    @error('security_deposited')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                            </div>


                                        </div>
                                        <div class="col-md-4">

                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">Upload Picture</label>
                                                <input class="form-control" id="customFile" name="attachments" type="file" value="{{ old('file') }}" required="" />
                                            </div>
                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">CNIC front Picture</label>
                                                <input class="form-control" id="customFile" name="cnic_front_attachments" type="file" value="{{ old('file') }}" required="" />
                                            </div>
                                            <div class="mb-3">
                                                <label class="form-label" for="customFile">CNIC Back Picture</label>
                                                <input class="form-control" id="customFile" name="cnic_back_attachments" type="file" value="{{ old('file') }}" required="" />
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


@endsection