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
        outline: none;
        /* Remove the default focus outline */
        border: none;
        /* Remove the border */
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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Completion of Registry </h4>
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
                                <form class="row g-3 needs-validation" method="post" action="{{ route('registry_document.store') }}" novalidate="" enctype="multipart/form-data">
                                    @csrf
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_no">Doc No.</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" value="{{$doc_no+1}}" readonly required="" />
                                                    <div class="valid-feedback">Please Add Doc No..</div>
                                                    @error('doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-4">
                                                    <label class="form-label" for="date"> Date</label>
                                                    <?php
                                                    $dt = new DateTime();
                                                    ?>
                                                    <input class="form-control" id="date" type="date"
                                                        name="date" required=""
                                                        value="{{$dt->format('Y-m-d')}}" />
                                                    <div class="valid-feedback">Please Add Doc Date</div>
                                                    @error('date')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <div class="col-md-4">
                                                    <label class="form-label" for="base_doc_no">Purchase of Land</label>
                                                    <select id="base_doc_no" name="base_doc_no" class="form-control"
                                                        required="">
                                                        <option value="">Kindly Select</option>
                                                        @foreach($purchase_of_land as $row)
                                                        <option value="{{ $row->File_No }}">File No - {{ $row->File_No }}</option>
                                                        @endforeach
                                                    </select>

                                                    <div class="invalid-feedback">Please select Purchase of Land.</div>
                                                    @error('base_doc_no')
                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                    @enderror
                                                </div>
                                                <!-- LO Information Section -->
                                                <div class="col-md-12">
                                                    <div class="card border border-300 bg-soft mt-3">
                                                        <div class="card-header bg-soft">
                                                            <h5 style="float: left;" class="mb-0">LO Information</h5>
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
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">
                                                        <div class="card-body">
                                                            <div class="row">


                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="registry_no">Registry No</label>
                                                                        <input class="form-control"
                                                                            id="registry_no"
                                                                            name="registry_no"
                                                                            type="text"
                                                                            value="{{ old('registry_no') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="registry_date">Registry Date</label>
                                                                        <input class="form-control"
                                                                            id="registry_date"
                                                                            name="registry_date"
                                                                            type="date"
                                                                            value="{{ old('registry_date') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="mutation_no">Mutation No</label>
                                                                        <input class="form-control"
                                                                            id="mutation_no"
                                                                            name="mutation_no"
                                                                            type="text"
                                                                            value="{{ old('mutation_no') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="mutation_date">Mutation Date</label>
                                                                        <input class="form-control"
                                                                            id="mutation_date"
                                                                            name="mutation_date"
                                                                            type="date"
                                                                            value="{{ old('mutation_date') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">
                                                        <div class="card-body">
                                                            <h5 class="card-title">Land Offer Form Fee</h5>
                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="land_challan_no">Challan No</label>
                                                                        <input class="form-control"
                                                                            id="land_challan_no"
                                                                            name="land_challan_no"
                                                                            type="text"
                                                                            value="{{ old('land_challan_no') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="land_challan_date">Challan Date</label>
                                                                        <input class="form-control"
                                                                            id="land_challan_date"
                                                                            name="land_challan_date"
                                                                            type="date"
                                                                            value="{{ old('land_challan_date') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="land_challan_amount">Challan Amount</label>
                                                                        <input class="form-control"
                                                                            id="land_challan_amount"
                                                                            name="land_challan_amount"
                                                                            type="text"
                                                                            value="{{ old('land_challan_amount') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" style="margin-top: 20px">
                                                    <div class="card">
                                                        <div class="card-body">
                                                            <h5 class="card-title">Newspaper Ad Fee</h5>
                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="newspaper_challan_no">Challan No</label>
                                                                        <input class="form-control"
                                                                            id="newspaper_challan_no"
                                                                            name="newspaper_challan_no"
                                                                            type="text"
                                                                            value="{{ old('newspaper_challan_no') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="newspaper_challan_date">Challan Date</label>
                                                                        <input class="form-control"
                                                                            id="newspaper_challan_date"
                                                                            name="newspaper_challan_date"
                                                                            type="date"
                                                                            value="{{ old('newspaper_challan_date') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="newspaper_challan_amount">Challan Amount</label>
                                                                        <input class="form-control"
                                                                            id="newspaper_challan_amount"
                                                                            name="newspaper_challan_amount"
                                                                            type="text"
                                                                            value="{{ old('newspaper_challan_amount') }}"
                                                                            required="" />
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
                                                                            for="registry">Registry</label>
                                                                        <input class="form-control"
                                                                            id="registry"
                                                                            name="registry"
                                                                            type="file"
                                                                            value="{{ old('registry') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="mutation">Mutation</label>
                                                                        <input class="form-control"
                                                                            id="mutation"
                                                                            name="mutation"
                                                                            type="file"
                                                                            value="{{ old('mutation') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="indemnity_bond">Indemnity Bond</label>
                                                                        <input class="form-control"
                                                                            id="indemnity_bond"
                                                                            name="indemnity_bond"
                                                                            type="file"
                                                                            value="{{ old('indemnity_bond') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="agreement">Agreement</label>
                                                                        <input class="form-control"
                                                                            id="agreement"
                                                                            name="agreement"
                                                                            type="file"
                                                                            value="{{ old('agreement') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="undertaking">UnderTaking</label>
                                                                        <input class="form-control"
                                                                            id="undertaking"
                                                                            name="undertaking"
                                                                            type="file"
                                                                            value="{{ old('undertaking') }}"
                                                                            required="" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <div class="mb-3">
                                                                        <label class="form-label"
                                                                            for="afidavit">Afidavit-1</label>
                                                                        <input class="form-control"
                                                                            id="afidavit"
                                                                            name="afidavit"
                                                                            type="file"
                                                                            value="{{ old('afidavit') }}"
                                                                            required="" />
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
<script>
    $(document).ready(function() {
        // Fetch LO details when base_doc_no changes
        $('#base_doc_no').on('change', function() {
            var docNo = $(this).val();
            var tbodyLoInfo = $('#tbodyLoInfo');

            // Clear the table
            tbodyLoInfo.html('');

            if (docNo === '') {
                return;
            }

            // Fetch LO details from API
            $.ajax({
                url: "{{ url('/get-purchase_lo-details') }}/" + docNo,
                type: "GET",
                dataType: "json",
                success: function(response) {
                    if (response.success && response.data.length > 0) {
                        // Populate table rows with LO data
                        response.data.forEach(function(lo, index) {
                            var row = `<tr>
                                <td>
                                    <input type="text" class="form-control" value="${lo.lo_name || ''}" readonly>
                                    <input type="hidden" name="lo_name[]" value="${lo.lo_name || ''}">
                                </td>
                                <td>
                                    <input type="text" class="form-control" value="${lo.so || ''}" readonly>
                                    <input type="hidden" name="so[]" value="${lo.so || ''}">
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

        // Trigger on page load if base_doc_no is already selected (for edit form)
        if ($('#base_doc_no').val()) {
            $('#base_doc_no').trigger('change');
        }
    });
</script>


@endsection