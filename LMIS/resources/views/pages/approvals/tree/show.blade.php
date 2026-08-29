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
        .card-header {
            padding: 17px 0 0 40px;
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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Approval Tree</h4>
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
                                        @if(session('success'))

                                            <div class="alert alert-outline-success d-flex align-items-center" role="alert">
                                                <span class="fas fa-check-circle text-success fs-3 me-3"></span>

                                                <p class="mb-0 flex-1">{{ session('success') }}</p>
                                                <button class="btn-close" type="button" data-bs-dismiss="alert"
                                                        aria-label="Close"></button>
                                            </div>

                                        @endif
                                        @if(session('danger'))
                                            <div class="alert alert-outline-danger d-flex align-items-center" role="alert">
                                                <span class="fas fa-times-circle text-danger fs-3 me-3"></span>

                                                <p class="mb-0 flex-1">{{ session('danger') }}</p>
                                                <button class="btn-close" type="button" data-bs-dismiss="alert"
                                                        aria-label="Close"></button>
                                            </div>
                                        @endif
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('approval_tree.update',1) }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        @method('PUT')
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">

                                                    <div class="col-md-12" style="margin-top: 20px">
                                                        <div class="card">

                                                            <div class="card-body">
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        <div class="card">
                                                                            <div class="card-header">
                                                                                <h5 class="card-title">Purchasing of Land</h5>
                                                                            </div>
                                                                            <div class="card-body">
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="lp_master_data_list" name="lp_master_data" type="checkbox" @if($record->lp_master_data == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="lp_master_data_list">LP Master Data</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="exemption_rate" type="checkbox"  @if($record->exemption_rate == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Exemption Rates</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="challan_fee" type="checkbox"  @if($record->challan_fee == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Cahllan Fee</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="seller_profile" type="checkbox"  @if($record->seller_profile == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Seller Profile</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="challan_form" type="checkbox"  @if($record->challan_form == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Challan Form</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="land_form_seller" type="checkbox"  @if($record->land_form_seller == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Land Form (Seller)</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="purchase_of_land" type="checkbox"  @if($record->purchase_of_land == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Purchase of Land</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="possession_certificate" type="checkbox"  @if($record->possession_certificate == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Possession Certificate</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="pictorial_view" type="checkbox"  @if($record->pictorial_view == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Pictorial View</label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <div class="card">
                                                                            <div class="card-header">
                                                                                <h5 class="card-title">Registry Documents</h5>
                                                                            </div>
                                                                            <div class="card-body">
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="exemption_rate_list" name="conveyance_deed" type="checkbox"  @if($record->conveyance_deed == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="exemption_rate_list">Conveyance Deed</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="agreement" type="checkbox"  @if($record->agreement == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Agreement</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="indemnity_bond" type="checkbox"  @if($record->indemnity_bond == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Indemnity Bond</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="registry_document" type="checkbox"  @if($record->registry_document == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Registry Document</label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <div class="card">
                                                                            <div class="card-header">
                                                                                <h5 class="card-title">Exemption Documents</h5>
                                                                            </div>
                                                                            <div class="card-body">
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="challan_fee_list" name="exemption_form" type="checkbox"  @if($record->exemption_form == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="challan_fee_list">Exemption Form</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="affidavit_2" type="checkbox"  @if($record->affidavit_2 == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Affidavit 2</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="exemption_inventory" type="checkbox"  @if($record->exemption_inventory == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Exemption Inventory</label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <div class="card">
                                                                            <div class="card-header">
                                                                                <h5 class="card-title">Intimation Documents</h5>
                                                                            </div>
                                                                            <div class="card-body">
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="challan_fee_list" name="intimation_application" type="checkbox"  @if($record->intimation_application == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="challan_fee_list">Intimation Application</label>
                                                                                </div>
                                                                                <div class="form-check form-check-inline">
                                                                                    <input class="form-check-input" id="inlineCheckbox1" name="intimation_letter" type="checkbox"  @if($record->intimation_letter == 1) checked @endif value="1" />
                                                                                    <label class="form-check-label" for="inlineCheckbox1">Intimation Letter</label>
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




@endsection