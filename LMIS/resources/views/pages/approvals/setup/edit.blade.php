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
            padding-left: 25px;

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
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Add Approvals Setup</h4>
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
                                    <form class="row g-3 needs-validation" method="post" action="{{ route('approval_setup.store') }}" novalidate=""  enctype="multipart/form-data">
                                        @csrf
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row">

                                                    <div class="col-md-6">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="name">Approval</label>
                                                            <select class="form-control" name="approval" required>
                                                                <option value="">Select Approval</option>
                                                                @if($tree->lp_master_data == 1) <option @if($approval_setup_header->approval == 'LP Master Data') selected @endif value="LP Master Data">LP Master Data</option> @endif
                                                                @if($tree->exemption_rate == 1) <option  @if($approval_setup_header->approval == 'Exemption Rate') selected @endif value="Exemption Rate">Exemption Rate</option> @endif
                                                                @if($tree->challan_fee == 1) <option @if($approval_setup_header->approval == 'Challan Fee') selected @endif value="Challan Fee">Challan Fee</option> @endif
                                                                @if($tree->seller_profile == 1) <option @if($approval_setup_header->approval == 'Seller Profile') selected @endif  value="Seller Profile">Seller Profile</option> @endif
                                                                @if($tree->challan_form == 1) <option @if($approval_setup_header->approval == 'Challan Form') selected @endif  value="Challan Form">Challan Form</option> @endif
                                                                @if($tree->land_form_seller == 1) <option @if($approval_setup_header->approval == 'Land Form Seller') selected @endif  value="Land Form Seller">Land Form Seller</option> @endif
                                                                @if($tree->purchase_of_land == 1) <option @if($approval_setup_header->approval == 'Purchase of Land') selected @endif  value="Purchase of Land">Purchase of Land</option> @endif
                                                                @if($tree->possession_certificate == 1) <option @if($approval_setup_header->approval == 'Possession Certificate') selected @endif  value="Possession Certificate">Possession Certificate</option> @endif
                                                                @if($tree->pictorial_view == 1) <option @if($approval_setup_header->approval == 'Pictorial View') selected @endif  value="Pictorial View">Pictorial View</option> @endif
                                                                @if($tree->conveyance_deed == 1) <option  @if($approval_setup_header->approval == 'Conveyance Deed') selected @endif value="Conveyance Deed">Conveyance Deed</option> @endif
                                                                @if($tree->agreement == 1) <option  @if($approval_setup_header->approval == 'Agreement') selected @endif value="Agreement">Agreement</option> @endif
                                                                @if($tree->indemnity_bond == 1) <option @if($approval_setup_header->approval == 'Indemnity Bond') selected @endif value="Indemnity Bond">Indemnity Bond</option> @endif
                                                                @if($tree->registry_document == 1) <option @if($approval_setup_header->approval == 'Registry Document') selected @endif  value="Registry Document">Registry Document</option> @endif
                                                                @if($tree->exemption_form == 1) <option @if($approval_setup_header->approval == 'Exemption Form') selected @endif  value="Exemption Form">Exemption Form</option> @endif
                                                                @if($tree->affidavit_2 == 1) <option @if($approval_setup_header->approval == 'Affidavit') selected @endif  value="Affidavit 2">Affidavit 2</option> @endif
                                                                @if($tree->intimation_application == 1) <option  @if($approval_setup_header->approval == 'Intimation Application') selected @endif value="Intimation Application">Intimation Application</option> @endif
                                                                @if($tree->intimation_letter == 1) <option  @if($approval_setup_header->approval == 'Intimation Letter') selected @endif value="Intimation Letter">Intimation Letter</option> @endif
                                                                @if($tree->exemption_inventory == 1) <option  @if($approval_setup_header->approval == 'Exemption Inventory') selected @endif value="Exemption Inventory">Exemption Inventory</option> @endif
                                                            </select>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6">

                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="no_of_approvals">Stage</label>
                                                            <select id="approval-select" onchange="getNumberOfApprovals()" class="form-control" name="stage" required>
                                                                <option value="">Select Stage</option>
                                                                @foreach($stages as $row)
                                                                    <option @if($approval_setup_header->stage ==  $row->name) selected @endif data-id="{{ $row->no_of_approvals }}" value="{{ $row->name }}">{{ $row->name }}</option>
                                                                @endforeach

                                                            </select>

                                                        </div>

                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="mb-3">
                                                            <label class="form-label"
                                                                   for="no_of_approvals">No of Approvals</label>
                                                            <input class="form-control"
                                                                   id="no_of_approvals"
                                                                   name="no_of_approvals"
                                                                   type="number"
                                                                   value="{{$approval_setup_header->no_of_approvals}}" readonly
                                                                   required=""/>
                                                        </div>
                                                    </div>


                                                    <div class="card">
                                                        <div class="card-body">
                                                            <table style="width: 100%;height: 45px">
                                                                <thead>
                                                                <tr>
                                                                    <td style="width: 50px;padding-left: 20px">#</td>
                                                                    <td>Select User</td>
                                                                    <td>Designation</td>
                                                                </tr>
                                                                </thead>
                                                                <tbody id="user_table_body">
                                                                <?php $count = 1; ?>

                                                                @foreach($approval_setup_header->rows as $row_data)

                                                                    <input type="hidden"  name="challan_form_row[<?php echo $count; ?>][id]"  value="{{$row_data['id']}}">


                                                                    <tr>
                                                                        <td>{{$count}}</td>
                                                                        <td style="padding: 0">
                                                                            <select
                                                                                    onchange="getDesgination({{$count}})"
                                                                                    id="' + userId + '"
                                                                                    name="item_lines[{{$count}}][user]"
                                                                                    class="form-control" required>
                                                                                <option value="">Select User
                                                                                </option>
                                                                         <?php foreach($users as $user){ ?>
                                                                                <option @if($row_data['user'] == $user->id ) selected @endif data-id="<?php echo $user->designation; ?>"
                                                                                        value="<?php echo $user->id; ?>"><?php echo $user->name; ?></option>
                                                                            <?php } ?>
                                                                            </select>
                                                                        </td>

                                                                        <td style="padding: 0"><input
                                                                                    name="item_lines[{{$count}}][designation]"
                                                                                    class="form-control"
                                                                                    value="{{$row_data['designation']}}"
                                                                                    id="' + designationId + '"></td>
                                                                    </tr>

                                                                    <?php $count++; ?>

                                                                @endforeach
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>

                                                </div>


                                            </div>
                                        </div>


                                        <div class="col-12">
                                            {{--<button class="btn btn-primary" type="submit">Submit form</button>--}}
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
    <input type="hidden" id="rownumber_participant" value="100">
    <input type="hidden" id="rownumber" value="<?php echo $count-1; ?>">

    <script>


        function getNumberOfApprovals(){
            var selectedOption = $('#approval-select').find(':selected');

            // Get the data-id attribute of the selected option
            var noOfApprovals = selectedOption.data('id');

            $('#no_of_approvals').val(noOfApprovals);

//            user_table_body
            var count = 1;
            {{--var row = '<tr>' +--}}
            {{--'<td><select onchange="getDesgination(count)" name="user[]" class="form-control"><?php foreach($users as $user){ ?><option value="<?php echo $user->name; ?>"><?php echo $user->name; ?></option><?php } ?> </td>' +--}}
            {{--'<td><input class="form-control" id="designation" name="designation[]"> </td></tr></select>';--}}

            $('#user_table_body').empty();

            for (var i = 0; i < noOfApprovals; i++) {
                var userId = 'user_' + count; // Dynamic ID for select element
                var designationId = 'designation_' + count; // Dynamic ID for input element


                var row = '<tr>' +
                        '<td>'+count+'</td>'+
                        '<td style="padding: 0"><select  onchange="getDesgination(' + count + ')" id="' + userId + '"  name="item_lines[' + count + '][user]" class="form-control" required><option value="">Select User</option><?php foreach($users as $user){ ?><option data-id="<?php echo $user->designation; ?>" value="<?php echo $user->id; ?>"><?php echo $user->name; ?></option><?php } ?> </td>' +
                        '<td style="padding: 0"><input name="item_lines[' + count + '][designation]" class="form-control" id="' + designationId + '"  > </td></tr>';

                $('#user_table_body').append(row);

                count++;
            }




        }
        function getDesgination(id){
            var selectedOption = $('#user_'+id).find(':selected');

            // Get the data-id attribute of the selected option
            var designation = selectedOption.data('id');
            $('#designation_'+id).val(designation);

        }

    </script>



@endsection