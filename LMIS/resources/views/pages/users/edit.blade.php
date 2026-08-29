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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Add User</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('users.update',$User->id) }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        @method('PUT')

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">



                                                    <div class="col-md-6">
                                                        <label class="form-label" for="name"> User Name</label>
                                                        <?php
                                                        ?>
                                                        <input class="form-control" id="name" type="text" value="{{$User->name}}"
                                                               name="name" required=""/>

                                                        <div class="valid-feedback">Please Add User Name</div>
                                                        @error('name')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="email"> User Email</label>
                                                        <?php
                                                        ?>
                                                        <input class="form-control" id="email" type="email"  value="{{$User->email}}"
                                                               name="email" required=""/>

                                                        <div class="valid-feedback">Please Add User Email</div>
                                                        @error('email')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="email"> User Password</label>
                                                        <?php
                                                        ?>
                                                        <input class="form-control" id="password" type="password"
                                                               name="password" />

                                                        <div class="valid-feedback">Please Add User Password</div>
                                                        @error('password')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="Designation">Designation</label>
                                                        <?php
                                                        ?>
                                                        <input class="form-control" id="Designation" type="text"
                                                               value="{{$User->designation}}"     name="designation" required=""/>

                                                        <div class="valid-feedback">Please Add User Designation</div>
                                                        @error('designation')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);">{{ $message }}</div>
                                                        @enderror
                                                    </div>



                                                    <div class="col-md-12" style="margin-top: 20px">
                                                       <div class="card">
                                                          <div class="card-header">
                                                              <h5 class="card-title">Purchasing of Land</h5>
                                                          </div>
                                                          <div class="card-body">
                                                              <div class="row">
                                                                  <div class="col-md-3">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">LP Master Data</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="lp_master_data_list" name="lp_master_data_list" @if($User->lp_master_data_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="lp_master_data_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="inlineCheckbox1" name="lp_master_data_add" @if($User->lp_master_data_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="inlineCheckbox1" name="lp_master_data_edit" @if($User->lp_master_data_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="inlineCheckbox1" name="lp_master_data_delete" @if($User->lp_master_data_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="inlineCheckbox1" name="lp_master_data_print" @if($User->lp_master_data_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Exemption Rate</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="exemption_rate_list" name="exemption_rate_list" @if($User->exemption_rate_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="exemption_rate_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="exemption_rate_add" @if($User->exemption_rate_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="exemption_rate_edit" @if($User->exemption_rate_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="exemption_rate_delete" @if($User->exemption_rate_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="exemption_rate_print" @if($User->exemption_rate_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Challan Fee</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="challan_fee_list" name="challan_fee_list" @if($User->challan_fee_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="challan_fee_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_fee_add" @if($User->challan_fee_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_fee_edit" @if($User->challan_fee_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_fee_delete" @if($User->challan_fee_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_fee_print" @if($User->challan_fee_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Seller Profile</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="seller_profile_list" name="seller_profile_list" @if($User->seller_profile_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="seller_profile_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="seller_profile_add" @if($User->seller_profile_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="seller_profile_edit" @if($User->seller_profile_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="seller_profile_delete" @if($User->seller_profile_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="seller_profile_print" @if($User->seller_profile_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Challan Form</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="challan_form_list" name="challan_form_list" @if($User->challan_form_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="challan_form_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_form_add" @if($User->challan_form_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_form_edit" @if($User->challan_form_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_form_delete" @if($User->challan_form_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="challan_form_print" @if($User->challan_form_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Land Form (seller)</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="land_form_seller_list" name="land_form_seller_list" @if($User->land_form_seller_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="land_form_seller_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="land_form_seller_add" @if($User->land_form_seller_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="land_form_seller_edit" @if($User->land_form_seller_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="land_form_seller_delete" @if($User->land_form_seller_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="land_form_seller_print" @if($User->land_form_seller_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Purchase of Land</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="purchase_of_land_list" name="purchase_of_land_list" @if($User->purchase_of_land_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="purchase_of_land_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="purchase_of_land_add" @if($User->purchase_of_land_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="purchase_of_land_edit" @if($User->purchase_of_land_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="purchase_of_land_delete" @if($User->purchase_of_land_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="purchase_of_land_print" @if($User->purchase_of_land_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Possession Certificate</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="possession_certificate_list" name="possession_certificate_list" @if($User->possession_certificate_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="possession_certificate_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="possession_certificate_add" @if($User->possession_certificate_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="possession_certificate_edit" @if($User->possession_certificate_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="possession_certificate_delete" @if($User->possession_certificate_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="possession_certificate_print" @if($User->possession_certificate_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                      <div class="card">
                                                                          <div class="card-header">
                                                                              <h5 class="card-title">Possession Certificate</h5>
                                                                          </div>
                                                                          <div class="card-body">
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="pictorial_view_list" name="pictorial_view_list" @if($User->pictorial_view_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="pictorial_view_list">List</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="pictorial_view_add" @if($User->pictorial_view_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Add</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="pictorial_view_edit" @if($User->pictorial_view_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Edit</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="pictorial_view_delete" @if($User->pictorial_view_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Delete</label>
                                                                              </div>
                                                                              <div class="form-check form-check-inline">
                                                                                  <input class="form-check-input" id="inlineCheckbox1" name="pictorial_view_print" @if($User->pictorial_view_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                  <label class="form-check-label" for="inlineCheckbox1">Print</label>
                                                                              </div>
                                                                          </div>
                                                                      </div>
                                                                  </div>
                                                              </div>

                                                              </div>
                                                          </div>
                                                       <div class="card">
                                                          <div class="card-header">
                                                              <h5 class="card-title">Registry Documents</h5>
                                                          </div>
                                                          <div class="card-body">
                                                              <div class="row">
                                                                  <div class="col-md-3">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Conveyance Deed</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="conveyance_deed_list" name="conveyance_deed_list" @if($User->conveyance_deed_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="conveyance_deed_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="conveyance_deed_add" name="conveyance_deed_add" @if($User->conveyance_deed_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="conveyance_deed_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="conveyance_deed_edit" name="conveyance_deed_edit" @if($User->conveyance_deed_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="conveyance_deed_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="conveyance_deed_delete" name="conveyance_deed_delete" @if($User->conveyance_deed_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="conveyance_deed_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="conveyance_deed_print" name="conveyance_deed_print" @if($User->conveyance_deed_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="conveyance_deed_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Agreement</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="agreement_list" name="agreement_list" @if($User->agreement_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="agreement_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="agreement_add" name="agreement_add" @if($User->agreement_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="agreement_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="agreement_edit" name="agreement_edit" @if($User->agreement_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="agreement_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="agreement_delete" name="agreement_delete" @if($User->agreement_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="agreement_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="agreement_print" name="agreement_print" @if($User->agreement_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="agreement_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Indemnity Bond</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="indemnity_bond_list" name="indemnity_bond_list" @if($User->indemnity_bond_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="indemnity_bond_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="indemnity_bond_add" name="indemnity_bond_add" @if($User->indemnity_bond_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="indemnity_bond_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="indemnity_bond_edit" name="indemnity_bond_edit" @if($User->indemnity_bond_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="indemnity_bond_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="indemnity_bond_delete" name="indemnity_bond_delete" @if($User->indemnity_bond_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="indemnity_bond_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="indemnity_bond_print" name="indemnity_bond_print" @if($User->indemnity_bond_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="indemnity_bond_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-3">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Registry Document</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="registry_document_list" name="registry_document_list" @if($User->registry_document_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="registry_document_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="registry_document_add" name="registry_document_add" @if($User->registry_document_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="registry_document_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="registry_document_edit" name="registry_document_edit" @if($User->registry_document_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="registry_document_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="registry_document_delete" name="registry_document_delete" @if($User->registry_document_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="registry_document_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="registry_document_print" name="registry_document_print" @if($User->registry_document_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="registry_document_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                              </div>

                                                              </div>
                                                          </div>
                                                       <div class="card">
                                                          <div class="card-header">
                                                              <h5 class="card-title">Exemption Documents</h5>
                                                          </div>
                                                          <div class="card-body">
                                                              <div class="row">
                                                                  <div class="col-md-6">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Exemption Form</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_form_list" name="exemption_form_list" @if($User->exemption_form_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_form_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_form_add" name="exemption_form_add" @if($User->exemption_form_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_form_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_form_edit" name="exemption_form_edit" @if($User->exemption_form_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_form_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_form_delete" name="exemption_form_delete" @if($User->exemption_form_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_form_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_form_print" name="exemption_form_print" @if($User->exemption_form_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_form_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-6">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Affidavit 2</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="affidavit_2_list" name="affidavit_2_list" @if($User->affidavit_2_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="affidavit_2_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="affidavit_2_add" name="affidavit_2_add" @if($User->affidavit_2_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="affidavit_2_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="affidavit_2_edit" name="affidavit_2_edit" @if($User->affidavit_2_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="affidavit_2_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="affidavit_2_delete" name="affidavit_2_delete" @if($User->affidavit_2_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="affidavit_2_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="affidavit_2_print" name="affidavit_2_print" @if($User->affidavit_2_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="affidavit_2_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                              </div>

                                                              </div>
                                                          </div>
                                                       <div class="card">
                                                          <div class="card-header">
                                                              <h5 class="card-title">Intimation Documents</h5>
                                                          </div>
                                                          <div class="card-body">
                                                              <div class="row">
                                                                  <div class="col-md-6">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Intimation Form</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_application_list" name="intimation_application_list" @if($User->intimation_application_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_application_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_application_add" name="intimation_application_add" @if($User->intimation_application_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_application_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_application_edit" name="intimation_application_edit" @if($User->intimation_application_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_application_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_application_delete" name="intimation_application_delete" @if($User->intimation_application_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_application_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_application_print" name="intimation_application_print" @if($User->intimation_application_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_application_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-6">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Intimation Letter</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_letter_list" name="intimation_letter_list" @if($User->intimation_letter_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_letter_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_letter_add" name="intimation_letter_add" @if($User->intimation_letter_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_letter_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_letter_edit" name="intimation_letter_edit" @if($User->intimation_letter_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_letter_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_letter_delete" name="intimation_letter_delete" @if($User->intimation_letter_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_letter_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="intimation_letter_print" name="intimation_letter_print" @if($User->intimation_letter_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="intimation_letter_print">Print</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                  </div>
                                                                  <div class="col-md-6">
                                                                    <div class="card">
                                                                        <div class="card-header">
                                                                            <h5 class="card-title">Exemption Inventory Approval</h5>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_inventory_list" name="exemption_inventory_list" @if($User->exemption_inventory_list == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_inventory_list">List</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_inventory_add" name="exemption_inventory_add" @if($User->exemption_inventory_add == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_inventory_add">Add</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_inventory_edit" name="exemption_inventory_edit" @if($User->exemption_inventory_edit == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_inventory_edit">Edit</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_inventory_delete" name="exemption_inventory_delete" @if($User->exemption_inventory_delete == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_inventory_delete">Delete</label>
                                                                            </div>
                                                                            <div class="form-check form-check-inline">
                                                                                <input class="form-check-input" id="exemption_inventory_print" name="exemption_inventory_print" @if($User->exemption_inventory_print == 1) checked @endif  type="checkbox" value="1" />
                                                                                <label class="form-check-label" for="exemption_inventory_print">Print</label>
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