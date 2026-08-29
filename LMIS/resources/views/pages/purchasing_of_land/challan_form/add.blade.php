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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Challan Form</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('challan_form.store') }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        <div class="row">



                                            <div class="col-md-12">
                                                <div class="row">


                                                    <div class="col-md-4">
                                                        <label class="form-label" for="challan_no">Challan NO</label>
                                                        <input class="form-control" id="challan_no" type="text" name="challan_no" readonly value="{{$challan_no+1}}" required="" />
                                                        @error('doc_no')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4"></div>


                                                    <div class="col-md-4">
                                                        <label class="form-label" for="mouza_name">Challan Date</label>
                                                        <input class="form-control"  id="doc_date" type="text" name="date" required="" readonly value="{{date('Y-m-d')}}"/>
                                                        <div class="invalid-feedback">Please add Date.</div>
                                                        @error('date')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>

                                                    <div class="col-md-4">
                                                        <label class="form-label" for="challan_form">Seller Name</label>
                                                        <input type="hidden" name="seller_name" id="seller_name">
                                                        <select onchange="addSellerinfo(this)" name="seller_id" class="form-control" required>
                                                            <option value="">Select Seller</option>
                                                            @foreach($seller_profiles as $seller_profile)
                                                                <option data-seller_name="{{$seller_profile->lo_name.' ' . $seller_profile->lo_father_name}}" data-cnic="{{$seller_profile->lo_cnic}}" value="{{$seller_profile->id}}">{{$seller_profile->lo_name.' ' . $seller_profile->lo_father_name}}</option>
                                                             @endforeach

                                                        </select>
                                                        <div class="invalid-feedback">Please Select Challan Form.</div>
                                                        @error('challan_form')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
                                                    </div>
                                                    <div class="col-md-4"></div>
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="mouza_name">Seller CNIC</label>
                                                        <input class="form-control"  id="seller_cnic" type="text" name="seller_cnic" required="" readonly value=""/>
                                                        {{--<div class="invalid-feedback">Please add Date.</div>--}}
                                                        @error('date')
                                                        <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                        @enderror
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
                                                                        <th>Sr #</th>
                                                                        <th>Challan Type</th>
                                                                        <th>Amount</th>
                                                                    </tr>
                                                                    </thead>
                                                                    <tbody id="tbodyrow">

                                                                    <tr id="1">
                                                                        <td>1 </td>
                                                                        <td><select onchange="addamount(this,1)"  name="challan_form_row[1][challan_type]"  class="form-control"><option value="">Select Challan Type</option>@foreach($challan_fee as $row)<option data-amount_1="{{$row->amount}}" {{$row->id}}>{{$row->category}}</option>@endforeach </select> </td>
                                                                        <td><input onchange="calculateTotal()" class="row-level amount form-control" id="amount_1"  name="challan_form_row[1][amount]"   value="{{ old('challan_form_row[100][amount]') }}"> </td>
                                                                    </tr>
                                                                    </tbody>

                                                                </table>
                                                                <div class="col-md-5"></div>
                                                                <div class="col-md-5"></div>

                                                                <div class="col-md-2">
                                                                    <label class="form-label" for="total">Total Amount</label>
                                                                    <input class="form-control"  id="total" type="text" name="amount" required="" readonly value=""/>
                                                                    {{--<div class="invalid-feedback">Please add Date.</div>--}}
                                                                    @error('date')
                                                                    <div style="width: 100%; margin-top: 0.25rem;  font-size: 75%; color: var(--lm-danger);" >{{ $message }}</div>
                                                                    @enderror
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

    <script>


         function addSellerinfo(selectElement){
             debugger;
             var seller_name = $(selectElement).find('option:selected').data('seller_name');
             var cnic = $(selectElement).find('option:selected').data('cnic');


             $('#seller_name').val(seller_name);
             $('#seller_cnic').val(cnic);
         }function addamount(obj,row){
             var amount = $(obj).find('option:selected').data('amount_'+row);

             $('#amount_'+row).val(amount);
             calculateTotal();

         }

    </script>
    <script>
        function calculateTotal(){
            debugger;

            var sum = 0;
            $('.amount').each(function() {
                var value = parseFloat($(this).val()); // Get the value of the input and convert to float
                if (!isNaN(value)) { // Check if the value is a valid number
                    sum += value;
                }
            });
            // You can also display the sum in an element if needed
            $('#total').val(sum);
        }
        $(function(){
            $('#add_row').click(function() {
                var rownumber = parseFloat($("#rownumber").val());
                var LineId = rownumber;
                rownumber = rownumber + 1;
                $("#rownumber").val(rownumber);


                var row = '<tr id="' + rownumber + '" DetailId="0"> ' +
                        '<td>'+ rownumber +' </td>'+
                        '<td><select onchange="addamount(this,'+ rownumber +')"  value="{{ old("") }}"  name="challan_form_row[' + rownumber + '][challan_type]"  class="form-control"><option value="">Select Challan Type</option>@foreach($challan_fee as $row)<option  data-amount_'+ rownumber +'="{{$row->amount}}" {{$row->id}}>{{$row->category}}</option>@endforeach </select> </td>'+
                        '<td><input onchange="calculateTotal()" id="amount_' + rownumber + '" class="row-level amount form-control"  name="challan_form_row[' + rownumber + '][amount]"    value="{{ old("") }}"> </td>'+
                        '</tr>';

                $("#tbodyrow").append(row);



            });
        });

    </script>

@endsection