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
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Completion of Registry</h4>
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
                                    method="post" action="{{ route('registry_document.update',$registry_document->id) }}" novalidate=""
                                    enctype="multipart/form-data">
                                    @csrf
                                    @method('PUT')
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label class="form-label" for="doc_no">Doc No.</label>
                                                    <input class="form-control" id="doc_no" type="text" name="doc_no" value="{{$registry_document->doc_no}}" readonly required="" />
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
                                                        value="{{$registry_document->date}}" />
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
                                                        <option @if($row->File_No == $registry_document->base_doc_no) selected @endif value="{{ $row->File_No }}">File No - {{ $row->File_No }}</option>
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
                                                                            value="{{ $registry_document->registry_no}}"
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
                                                                            value="{{ $registry_document->registry_date}}"
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
                                                                            value="{{ $registry_document->mutation_no}}"
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
                                                                            value="{{ $registry_document->mutation_date}}"
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
                                                                            value="{{ $registry_document->land_challan_no }}"
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
                                                                            value="{{ $registry_document->land_challan_date }}"
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
                                                                            value="{{ $registry_document->land_challan_amount }}"
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
                                                                            value="{{ $registry_document->newspaper_challan_no }}"
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
                                                                            value="{{ $registry_document->newspaper_challan_date }}"
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
                                                                            value="{{ $registry_document->newspaper_challan_amount }}"
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
                                                                            value="{{ old('registry') }}" />
                                                                        @if($registry_document->registry)
                                                                        <?php
                                                                        $filename = $registry_document->registry;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->registry; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->registry; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        @endif
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
                                                                            value="{{ old('mutation') }}" />
                                                                        @if($registry_document->mutation)
                                                                        <?php
                                                                        $filename = $registry_document->mutation;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->mutation; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->mutation; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        @endif
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
                                                                            value="{{ old('indemnity_bond') }}" />
                                                                        @if($registry_document->indemnity_bond)
                                                                        <?php
                                                                        $filename = $registry_document->indemnity_bond;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->indemnity_bond; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->indemnity_bond; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        @endif
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
                                                                            value="{{ old('agreement') }}" />
                                                                        @if($registry_document->agreement)
                                                                        <?php
                                                                        $filename = $registry_document->agreement;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->agreement; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->agreement; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->agreement; }}" style="width: 200px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                                        @endif
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
                                                                            value="{{ old('undertaking') }}" />
                                                                        @if($registry_document->undertaking)
                                                                        <?php
                                                                        $filename = $registry_document->undertaking;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->undertaking; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->undertaking; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        {{-- <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->undertaking; }}" style="width: 200px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
                                                                        @endif
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
                                                                            value="{{ old('afidavit') }}" />
                                                                        @if($registry_document->afidavit)
                                                                        <?php
                                                                        $filename = $registry_document->afidavit;
                                                                        $extension = pathinfo($filename, PATHINFO_EXTENSION);
                                                                        ?>
                                                                        @if(in_array($extension, ['jpg', 'jpeg', 'png', 'gif']))
                                                                        <img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->afidavit; }}"
                                                                            style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        @else
                                                                        <a target="_blank"
                                                                            href="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->afidavit; }}">
                                                                            <img src="{{ URL::asset('public/assets/').'/'.'file.png'; }}"
                                                                                style="width: 200px;  border: 1px solid #CBD0DD;  border-radius: 4px;">
                                                                        </a>
                                                                        @endif
                                                                        {{--<img src="{{ URL::asset('public/assets/uploads/').'/'.$registry_document->afidavit; }}" style="width: 200px; border: 1px solid #CBD0DD; border-radius: 4px;">--}}
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
<!-- Fetch LO Information when Land Form No changes -->
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