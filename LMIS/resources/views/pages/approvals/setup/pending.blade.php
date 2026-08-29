@extends('layouts/main')

@section('content')
<style>
    .position-relative {
        position: relative;
    }

    .count-indicator {
        position: absolute;
        top: 0;
        right: 0;
        background-color: red;
        color: white;
        border-radius: 50%;
        padding: 0.25em 0.5em;
        font-size: 0.75rem;
        font-weight: bold;
        transform: translate(50%, -50%);
    }
</style>
<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12 order-1 order-xl-0">
                <div class="mb-9">
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




                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                            <div class="row g-3 justify-content-between align-items-end">
                                <div class="col-12 col-md">
                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Pending For Approval</h4>
                                </div>

                            </div>
                        </div>
                        <div class="card-body p-0">

                            <div class="row"></div>
                            <ul style="margin-left: 30px" class="nav nav-underline" id="myTab" role="tablist">

                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link active" id="lp_master_data-tab" data-bs-toggle="tab" href="#tab-lp_master_data"
                                        role="tab" aria-controls="tab-lp_master_data" aria-selected="true">LP Master Data</a>
                                    @if($lp_master_data_record_count > 0)
                                    <span class="count-indicator">{{ $lp_master_data_record_count }}</span>
                                    @endif
                                </li>

                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="exemption_rate-tab" data-bs-toggle="tab" href="#tab-exemption_rate"
                                        role="tab" aria-controls="tab-exemption_rate" aria-selected="true">Exemption Rate</a>
                                    @if($exemption_r_count > 0)
                                    <span class="count-indicator">{{ $exemption_r_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="challan_fee-tab" data-bs-toggle="tab" href="#tab-challan_fee"
                                        role="tab" aria-controls="tab-challan_fee" aria-selected="true">Challan Fee</a>
                                    @if($challan_fee_approvals_count > 0)
                                    <span class="count-indicator">{{ $challan_fee_approvals_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="seller_profile-tab" data-bs-toggle="tab" href="#tab-seller_profile"
                                        role="tab" aria-controls="tab-seller_profile" aria-selected="true">Seller Profile</a>
                                    @if($seller_profile_count > 0)
                                    <span class="count-indicator">{{ $seller_profile_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="challan_form-tab" data-bs-toggle="tab" href="#tab-challan_form"
                                        role="tab" aria-controls="tab-challan_form" aria-selected="true">Challan Form</a>
                                    @if($challan_form_count > 0)
                                    <span class="count-indicator">{{ $challan_form_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="land_form_seller-tab" data-bs-toggle="tab" href="#tab-land_form_seller"
                                        role="tab" aria-controls="tab-land_form_seller" aria-selected="true">Land Form Seller</a>
                                    @if($land_form_seller_count > 0)
                                    <span class="count-indicator">{{ $land_form_seller_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="purchase_of_land-tab" data-bs-toggle="tab" href="#tab-purchase_of_land"
                                        role="tab" aria-controls="tab-purchase_of_land" aria-selected="true">Purchase of Land</a>
                                    @if($purchase_of_land_count > 0)
                                    <span class="count-indicator">{{ $purchase_of_land_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="possession_certificate-tab" data-bs-toggle="tab" href="#tab-possession_certificate"
                                        role="tab" aria-controls="tab-possession_certificate" aria-selected="true">Possession Certificate</a>
                                    @if($possession_certificate_count > 0)
                                    <span class="count-indicator">{{ $possession_certificate_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="pictorial_view-tab" data-bs-toggle="tab" href="#tab-pictorial_view"
                                        role="tab" aria-controls="tab-pictorial_view" aria-selected="true">Pictorial View</a>
                                    @if($pictorial_view_count > 0)
                                    <span class="count-indicator">{{ $pictorial_view_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="conveyance_deed-tab" data-bs-toggle="tab" href="#tab-conveyance_deed"
                                        role="tab" aria-controls="tab-conveyance_deed" aria-selected="true">Conveyance Deed</a>
                                    @if($conveyance_deed_count > 0)
                                    <span class="count-indicator">{{ $conveyance_deed_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="agreement-tab" data-bs-toggle="tab" href="#tab-agreement"
                                        role="tab" aria-controls="tab-agreement" aria-selected="true">Agreement</a>
                                    @if($agreement_count > 0)
                                    <span class="count-indicator">{{ $agreement_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="indemnity_bond-tab" data-bs-toggle="tab" href="#tab-indemnity_bond"
                                        role="tab" aria-controls="tab-indemnity_bond" aria-selected="true">Idemnity Bond</a>
                                    @if($indemnity_bond_count > 0)
                                    <span class="count-indicator">{{ $indemnity_bond_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="registry_document-tab" data-bs-toggle="tab" href="#tab-registry_document"
                                        role="tab" aria-controls="tab-registry_document" aria-selected="true">Registry Document</a>
                                    @if($registry_document_count > 0)
                                    <span class="count-indicator">{{ $registry_document_count }}</span>
                                    @endif
                                </li>
                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="exemption_form-tab" data-bs-toggle="tab" href="#tab-exemption_form"
                                        role="tab" aria-controls="tab-exemption_form" aria-selected="true">Exemption Form</a>
                                    @if($exemption_form_count > 0)
                                    <span class="count-indicator">{{ $exemption_form_count }}</span>
                                    @endif
                                </li>


                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="affidavit_2-tab" data-bs-toggle="tab" href="#tab-affidavit_2"
                                        role="tab" aria-controls="tab-affidavit_2" aria-selected="true">Affidavit 2</a>
                                    @if($affidavit_2_count > 0)
                                    <span class="count-indicator">{{ $affidavit_2_count }}</span>
                                    @endif
                                </li>


                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="intimation_application-tab" data-bs-toggle="tab" href="#tab-intimation_application"
                                        role="tab" aria-controls="tab-intimation_application" aria-selected="true">Intimation Application</a>
                                    @if($intimation_application_count > 0)
                                    <span class="count-indicator">{{ $intimation_application_count }}</span>
                                    @endif
                                </li>

                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="intimation_letter-tab" data-bs-toggle="tab" href="#tab-intimation_letter"
                                        role="tab" aria-controls="tab-intimation_letter" aria-selected="true">Intimation Letter</a>
                                    @if($intimation_letter_count > 0)
                                    <span class="count-indicator">{{ $intimation_letter_count }}</span>
                                    @endif
                                </li>

                                <li class="nav-item position-relative" role="presentation">
                                    <a class="nav-link " id="exemption_inventory-tab" data-bs-toggle="tab" href="#tab-exemption_inventory"
                                        role="tab" aria-controls="tab-exemption_inventory" aria-selected="true">Exemption Inventory</a>
                                    @if($exemption_inventory_count > 0)
                                    <span class="count-indicator">{{ $exemption_inventory_count }}</span>
                                    @endif
                                </li>

                            </ul>
                            <div class="tab-content mt-3" id="myTabContent">
                                <div class="tab-pane fade active show" id="tab-lp_master_data" role="tabpanel"
                                    aria-labelledby="lp_master_data-tab">
                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Provider Master Data</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort border-top" data-sort="age">LP CNIC</th>
                                                                    <th class="sort border-top" data-sort="age">Address</th>
                                                                    <th class="sort border-top" data-sort="age">Security Deposited</th>
                                                                    <th class="sort border-top" data-sort="age">Exemption Decimals</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($lp_master_data as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle age">{{ $row->lp_cnic }}</td>
                                                                    <td class="align-middle age">{{ $row->address }}</td>
                                                                    <td class="align-middle age">{{ $row->security_deposited }}</td>
                                                                    <td class="align-middle age">{{ $row->exemption_decimals }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('land_provider.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','LP Master Data')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>




                                </div>
                                <div class="tab-pane fade" id="tab-exemption_rate" role="tabpanel"
                                    aria-labelledby="exemption_rate-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Exemption Rate</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Mouza Code</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Mouza Name</th>
                                                                    <th class="sort border-top" data-sort="email">Exemption Rate</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($exemption_r as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->mouza_code }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->mouza_name }}</td>
                                                                    <td class="align-middle email">{{ $row->exemption_rate }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('exemption_rate.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Exemption Rate')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>

                                <div class="tab-pane fade" id="tab-challan_fee" role="tabpanel"
                                    aria-labelledby="challan_fee-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Challan Fee</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Sr.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Category</th>
                                                                    <th class="sort border-top" data-sort="email">Amounte</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($challan_fee as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->sr_code }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->category }}</td>
                                                                    <td class="align-middle email">{{ $row->amount }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('challan_fee.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Challan Fee')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-seller_profile" role="tabpanel"
                                    aria-labelledby="seller_profile-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Seller Profile</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="age">LO CNIC</th>
                                                                    <th class="sort border-top" data-sort="age">Address</th>
                                                                    <th class="sort border-top" data-sort="age">Contact No</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($seller_profile as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle age">{{ $row->lo_cnic }}</td>
                                                                    <td class="align-middle age">{{ $row->address }}</td>
                                                                    <td class="align-middle age">{{ $row->contact_no }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('seller_profile.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Seller Profile')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-challan_form" role="tabpanel"
                                    aria-labelledby="challan_form-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Challan From</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Challan No</th>
                                                                    <th class="sort border-top" data-sort="email">Date</th>
                                                                    <th class="sort border-top" data-sort="email">Seller Name</th>
                                                                    <th class="sort border-top" data-sort="email">Seller CNIC</th>
                                                                    <th class="sort border-top" data-sort="email">Total Amount</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($challan_form as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->challan_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->seller_name }}</td>
                                                                    <td class="align-middle email">{{ $row->seller_cnic }}</td>
                                                                    <td class="align-middle email">{{ $row->amount }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('challan_form.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Challan Form')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-land_form_seller" role="tabpanel"
                                    aria-labelledby="challan_form-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Land Form Seller</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="age">LO CNIC</th>
                                                                    <th class="sort border-top" data-sort="age">Address</th>
                                                                    <th class="sort border-top" data-sort="age">Contact No</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($land_form_seller as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_date }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle age">{{ $row->lo_cnic }}</td>
                                                                    <td class="align-middle age">{{ $row->address }}</td>
                                                                    <td class="align-middle age">{{ $row->contact_no }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('land_form.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Land Form Seller')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-purchase_of_land" role="tabpanel"
                                    aria-labelledby="challan_form-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Purchase of Land</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">File NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort border-top" data-sort="age">LO CNIC</th>
                                                                    <th class="sort border-top" data-sort="age">Amount</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($purchase_of_land as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->File_No }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle age">{{ $row->lo_cnic }}</td>
                                                                    <td class="align-middle age">{{ $row->amount }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('purchase_of_land.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Purchase of Land')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-possession_certificate" role="tabpanel"
                                    aria-labelledby="challan_form-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Possession Certificate</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Date</th>
                                                                    <th class="sort border-top" data-sort="email">Contact No</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($possession_certificate as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->contact_no }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('possession_certificate.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Possession Certificate')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-pictorial_view" role="tabpanel"
                                    aria-labelledby="pictorial_view-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Pictorial View</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Possession Certificate</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort border-top" data-sort="email">Name of Patwari</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($pictorial_view as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->pc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle email">{{ $row->name_of_patwari }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('pictorial_view.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Pictorial View')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-conveyance_deed" role="tabpanel"
                                    aria-labelledby="conveyance_deed-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Conveyance Deed</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Date</th>
                                                                    <th class="sort border-top" data-sort="email">Base Doc No</th>
                                                                    <th class="sort border-top" data-sort="email">Date of Creation</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($conveyance_deed as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->base_doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->date_of_creation }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('conveyance.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Conveyance Deed')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-agreement" role="tabpanel"
                                    aria-labelledby="agreement-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Agreement</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Date</th>
                                                                    <th class="sort border-top" data-sort="email">Base Doc No</th>
                                                                    <th class="sort border-top" data-sort="email">Agreement Date</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($agreement as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->base_doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->agreement_date }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('agreement.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Agreement')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="tab-pane fade" id="tab-indemnity_bond" role="tabpanel"
                                    aria-labelledby="indemnity_bond-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Indemnity Bond</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Date</th>
                                                                    <th class="sort border-top" data-sort="email">Base Doc No</th>
                                                                    <th class="sort border-top" data-sort="email">Date of Execution</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($indemnity_bond as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->base_doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->date_of_execution }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('indemnity_bond.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Indemnity Bond')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-registry_document" role="tabpanel"
                                    aria-labelledby="registry_document-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Registry Document</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc No.</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Date</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LO CNIC</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($registry_document as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_cnic }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('registry_document.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Registry Document')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-exemption_form" role="tabpanel"
                                    aria-labelledby="exemption_form-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Exemption Form</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">File NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>TION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($exemption_form as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->file_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('exemption_form.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Exemption Form')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-affidavit_2" role="tabpanel"
                                    aria-labelledby="affidavit_2-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Affidavit 2</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">File NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>TION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($affidavit_2 as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->file_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('affidavit_2.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Affidavit 2')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-intimation_application" role="tabpanel"
                                    aria-labelledby="intimation_application-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Intimation Application</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">File NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top" data-sort="email">LO Name</th>
                                                                    <th class="sort border-top" data-sort="email">LP Name</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        ACTION
                                                                    </th>TION
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($intimation_application as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->file_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle email">{{ $row->lp_name }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('intimation_application.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Intimation Application')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>
                                <div class="tab-pane fade" id="tab-intimation_letter" role="tabpanel"
                                    aria-labelledby="intimation_letter-tab">

                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Intimation Letter</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>

                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">File NO</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc Date</th>
                                                                    <th class="sort border-top ps-3" data-sort="name">LO Name</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">
                                                                        Action
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($intimation_letter as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->file_no }}</td>
                                                                    <td class="align-middle ps-3 name">{{ $row->date }}</td>
                                                                    <td class="align-middle email">{{ $row->lo_name }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('intimation_letter.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Intimation Letter')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                @endforeach

                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>







                                </div>

                                <div class="tab-pane fade" id="tab-possession_certificate" role="tabpanel"
                                    aria-labelledby="possession_certificate-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-pictorial_view" role="tabpanel"
                                    aria-labelledby="pictorial_view-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-conveyance_deed" role="tabpanel"
                                    aria-labelledby="conveyance_deed-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-agreement" role="tabpanel"
                                    aria-labelledby="agreement-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-indemnity_bond" role="tabpanel"
                                    aria-labelledby="indemnity_bond-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-registry_document" role="tabpanel"
                                    aria-labelledby="registry_document-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-exemption_form" role="tabpanel"
                                    aria-labelledby="exemption_form-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-affidavit_2" role="tabpanel"
                                    aria-labelledby="affidavit_2-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-intimation_applicationlication" role="tabpanel"
                                    aria-labelledby="intimation_applicationlication-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-intimation_letter" role="tabpanel"
                                    aria-labelledby="intimation_letter-tab">
                                </div>
                                <div class="tab-pane fade" id="tab-exemption_inventory" role="tabpanel"
                                    aria-labelledby="exemption_inventory-tab">
                                    <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                                        <div class="card-header p-4 border-bottom border-300 bg-soft">
                                            <div class="row g-3 justify-content-between align-items-end">
                                                <div class="col-12 col-md">
                                                    <h4 class="text-900 mb-0" data-anchor="data-anchor">Exemption Inventory</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body p-0">

                                            <div class="p-4 code-to-copy">
                                                <div>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped table-sm fs--1 mb-0 w-100">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" data-sort="name">Doc NO</th>
                                                                    <th class="sort border-top" data-sort="email">Date</th>
                                                                    <th class="sort border-top" data-sort="age">Land Form NO</th>
                                                                    <th class="sort border-top" data-sort="age">Total Reg Land</th>
                                                                    <th class="sort border-top" data-sort="age">Rate/Acre</th>
                                                                    <th class="sort text-end align-middle pe-0 border-top" scope="col">ACTION</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                @foreach($exemption_inventory as $row)
                                                                <tr>
                                                                    <td class="align-middle ps-3 name">{{ $row->doc_no }}</td>
                                                                    <td class="align-middle email">{{ $row->date }}</td>
                                                                    <td class="align-middle age">{{ $row->land_offer_form_no }}</td>
                                                                    <td class="align-middle age">{{ $row->total_registered_land }}</td>
                                                                    <td class="align-middle age">{{ $row->rate_per_acre }}</td>
                                                                    <td class="align-middle white-space-nowrap text-end pe-0">
                                                                        <div class="font-sans-serif btn-reveal-trigger position-static"><button class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2" type="button" data-bs-toggle="dropdown" data-boundary="window" aria-haspopup="true" aria-expanded="false" data-bs-reference="parent"><span class="fas fa-ellipsis-h fs--2"></span></button>
                                                                            <div class="dropdown-menu dropdown-menu-end py-2">
                                                                                <a class="dropdown-item" href="{{ route('exemption_inventory.edit',$row->id) }}">View</a>
                                                                                <button type="button" class="dropdown-item" onclick="ViewHistory('<?php echo $row->id; ?>','Exemption Inventory')">View History</button>

                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
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

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div class="modal fade" id="staticBackdrop" tabindex="-1" data-bs-backdrop="static"
        aria-labelledby="staticBackdropLabel" style="display: none;" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h5 class="modal-title text-white" id="staticBackdropLabel"></h5>
                    <button class="btn p-1" type="button" data-bs-dismiss="modal" aria-label="Close">
                        <svg class="svg-inline--fa fa-xmark fs--1 text-white" aria-hidden="true" focusable="false"
                            data-prefix="fas" data-icon="xmark" role="img" xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 320 512" data-fa-i2svg="">
                            <path fill="currentColor"
                                d="M310.6 361.4c12.5 12.5 12.5 32.75 0 45.25C304.4 412.9 296.2 416 288 416s-16.38-3.125-22.62-9.375L160 301.3L54.63 406.6C48.38 412.9 40.19 416 32 416S15.63 412.9 9.375 406.6c-12.5-12.5-12.5-32.75 0-45.25l105.4-105.4L9.375 150.6c-12.5-12.5-12.5-32.75 0-45.25s32.75-12.5 45.25 0L160 210.8l105.4-105.4c12.5-12.5 32.75-12.5 45.25 0s12.5 32.75 0 45.25l-105.4 105.4L310.6 361.4z"></path>
                        </svg>
                        <!-- <span class="fas fa-times fs--1 text-white"></span> Font Awesome fontawesome.com -->
                    </button>
                </div>
                <form class="row g-3 needs-validation" method="post"
                    action="{{ route('approval_status_update') }}" novalidate=""
                    enctype="multipart/form-data">
                    @csrf
                    <div class="modal-body">

                        <input type="hidden" id="id" name="id" value="">
                        <input type="hidden" id="form" name="form" value="">

                        <div class="row">
                            <div class="col-md-12">
                                <label class="form-label" for="status">Status</label>
                                <select name="status" id="status" class="form-control" required="">
                                    <option value="">Select Status</option>
                                    <option value="1">Approved</option>
                                    <option value="2">Reject</option>
                                </select>
                                <div class="valid-feedback">Please Select Status</div>
                                @error('status')
                                <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                @enderror
                            </div>
                            <div class="col-md-12">
                                <label class="form-label" for="remarks">Remarks</label>
                                <textarea class="form-control" name="remarks" rows="8"></textarea>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary" type="submit">Update Status</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="documentApprovalModal" tabindex="-1" aria-labelledby="documentApprovalModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="documentApprovalModalLabel">Document Approval History</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered" id="approvalHistoryTable">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Document Name</th>
                                <th>Approval User</th>
                                <th>Status</th>
                                <th>Remarks</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Data will be appended here -->
                        </tbody>
                    </table>
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

<script>
    function confirmSubmit(id) {
        if (confirm('Are you sure you want to delete this record?')) {
            document.getElementById('deleteForm-' + id).submit();
        }
    }
</script>
<script type="text/javascript">
    function ViewHistory(id, table) {
        var documentId = id;
        var document_name = table;

        $.ajax({
            url: '{{ route("approval_document_history") }}', // Update this with your route name
            type: 'POST',
            data: {
                id: documentId,
                approval: document_name,
                _token: '{{ csrf_token() }}' // Ensure CSRF token is included
            },
            success: function(response) {
                console.log(response);

                if (response.record) {
                    var tableBody = $('#approvalHistoryTable tbody');
                    tableBody.empty();

                    var count = 1;
                    response.record.forEach(function(item) {

                        var docstatus = '';
                        if (item.status == 1) {
                            docstatus = 'Approved';
                        } else if (item.status == 2) {
                            docstatus = 'Reject';

                        } else {
                            docstatus = 'Pending';

                        }

                        var row = '<tr>' +
                            '<td>' + count + '</td>' +
                            '<td>' + item.document_name + '</td>' +
                            '<td>' + item.name + '</td>' +
                            '<td>' + docstatus + '</td>' +
                            '<td>' + item.remarks + '</td>' +
                            '<td>' + item.updated_at + '</td>' +
                            '</tr>';
                        tableBody.append(row);
                        count++;
                    });

                    $('#documentApprovalModal').modal('show');
                } else {
                    alert('No approval history found.');
                }
            },
            error: function(xhr, status, error) {
                // Handle error
                alert('Error updating status: ' + xhr.responseText);
            }
        });
    }
</script>

<!-- Your content here -->
@endsection