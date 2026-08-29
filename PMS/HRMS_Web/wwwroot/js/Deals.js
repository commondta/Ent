
$(document).ready(function () {
    $("#Deals").validate({
        errorElement: 'div', //default input error message container
        errorClass: 'vd_red', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            DealName: {
                required: true
            },
            DealNature: {
                required: true
            },
            DealType: {
                required: true
            },

            DealerName: {
                required: true
            },
            DealDate: {
                required: true
            },
            ExpiryDate: {
                required: true
            },

        },

        errorPlacement: function (error, element) {

            if (element.parent().hasClass("vd_checkbox") || element.parent().hasClass("vd_radio")) {
                element.parent().append(error);
            } else if (element.parent().hasClass("vd_input-wrapper")) {
                error.insertAfter(element.parent());
            } else if (element.hasClass("js-example-basic-single")) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },

        invalidHandler: function (event, validator) { //display error alert on form submit
           // notification("bottomleft", "error", "fa fa-exclamation-circle vd_red", "Error", "Enter Valid Form Data");
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Enter Valid Form Data',

            });
        },

        highlight: function (element) { // hightlight error inputs

            $(element).addClass('vd_bd-red');
            $(element).siblings('.help-inline').removeClass('help-inline fa fa-check vd_green mgl-10');

        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        },

        success: function (label, element) {
            //   label
            //       .addClass('valid').addClass('help-inline fa fa-check vd_green mgl-10') // mark the current input as valid and display OK icon
            //       .closest('.control-group').removeClass('error').addClass('success'); // set success class to the control group
            $(element).removeClass('vd_bd-red');
        },

        submitHandler: function (form) {
            // alert("validation Successfully");
            var id = $("#ID").val();
            if (id == null || id == "") {
                SubmitRequest();
            }
            else {
                UpdateRequest();
            }

        }

    });
    GetPreviousRecords();
    GetAllDealerProfiles();
});

function refreshPage() {
    location.reload();
}
function GetAllDealerProfiles() {

    $.ajax({
        type: 'GET',
        url: `/api/Dealer/GetFilterListForPreSale`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },

        contentType: 'application/json',
        success: function (response) {

            if (response.code == 100) {
                $("#Dealertbody").empty();
                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <tr> ' +
                            ' <td class="Id">' + list.id + '</td> ' +
                            ' <td class="principalOwner">' + list.principalOwner + '</td>' +
                            '<td><a class="btn btn-primary" onclick="SelectDealer(' + list.id + ')">Select</a></td> </tr>';
                        $("#Dealertbody").append(row);
                        sr++;
                    }
                }

                $('#dealerlist').dataTable();
            }
        }
    });
}

function SelectDealer(id) {

    var url = "/api/Dealer/Get?id=" + id + "";

    $.ajax({
        type: 'GET',
        url: url,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        data: { id: id },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 100) {
                var result_User = response.data;

                $("#DealerCode").val(result_User.id);
                $("#DealerName").val(result_User.principalOwner);
                $("#dealerId").val(result_User.id);
                $("#dealerModal").modal("hide");

            }

            else {
                //notification("bottomleft", "error", "fa fa-exclamation-circle vd_red", "Eror", "Problem Finding Dealer");
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Problem Finding Dealer',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });
            }
        }
    });
}

function GetPreviousRecords() {
    $.ajax({
        type: 'GET',
        url: `/api/Deal/GetAll`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 100) {

                var datatabl = $('#findmodaltable').DataTable();
                datatabl.destroy();
                $("#findmodaltbody").empty();
                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var dealdate = list.dealDate;
                        var dealExpDate = list.dealExpDate;
                        var dealdatenew = dealdate.split('T', 1)[0];
                        var dealExpDatenew = dealExpDate.split('T', 1)[0];
                        var row = '<tr>' +
                            '<td>' + list.dealer.principalOwner + '</td>' +
                            '<td>' + list.dealName + '</td>' +
                            '<td>' + list.dealNature + '</td>' +
                            '<td>' + list.dealType + '</td>' +
                            '<td>' + dealdatenew + '</td>' +
                            '<td>' + dealExpDatenew + '</td>' +
                            '<td><a onclick="SelectDeal(' + list.id + ')"><i class="fa fa-eye"></i></a></td>' +
                            '</tr>';
                        $("#findmodaltbody").append(row);
                        sr++;

                    }
                }

                $('#findmodaltable').dataTable();
            }
        }
    });

}
function findmodal() {
    $("#findmodal").modal("show");
}
function SelectDeal(id) {


    var url = "/api/Deal/Get?id=" + id + "";

    $.ajax({
        type: 'GET',
        url: url,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        data: { id: id },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 100) {
                debugger;
                var dealData = response.data;
                $("#ID").val(dealData.id);
                $("#docDate").val(dealData.createdOn.slice(0, 10));
                $("#DealNature").val(dealData.dealNature);

                $("#DealerName").val(dealData.dealer.principalOwner);
                $("#DealName").val(dealData.dealName);
                $("#DealType").val(dealData.dealType);
                $("#dealerId").val(dealData.dealerId);
                $("#Quantity").val(dealData.qtyProperty);
                $("#DealDate").val(dealData.dealDate.slice(0, 10));
                $("#ExpiryDate").val(dealData.dealExpDate.slice(0, 10));
                $("#Measure").val(dealData.commissionType);
                $("#Commission").val(dealData.commission);
                $("#UnitMeasure").val(dealData.rebateType);
                $("#Rebate").val(dealData.rebate);
                $("#TotalValue").val(dealData.totalValue);
                $("#TotalReceived").val(dealData.totalReceied);
                $("#NetReceivable").val(dealData.netReceivable);
                $("#OutstandingReceivable").val(dealData.outstandingBalance);
                $("#GracePeriod").val(dealData.gracePeriod);
                $("#SurchargePerDay").val(dealData.surchargePerDay);
                $("#OneTimePayment").val(dealData.oneTimePayment);
                $("#Installment").val(dealData.installment);
                $("#remarks").val(dealData.remarks);
                $("#Prownumber").val(0);
                $("#Drownumber").val(0);
                $("#Propertiestbody").empty();


                $.each(dealData.dealProperty, function (index, Propertyitem) {
                    GetPropertiesDataInPropertiesRow(Propertyitem);
                    GetPlanDatainRow(Propertyitem.dealPaymentPlan, index);
                });

                $("#findmodal").modal("hide");
                $("#submitbtn").addClass("hide");
                $("#updatebtn").removeClass("hide");

            }

            else {
                //notification("bottomleft", "error", "fa fa-exclamation-circle vd_red", "Eror", "Problem Finding Lead");
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Problem Finding Lead',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });
            }
        }
    });
}

function GetPropertiesDataInPropertiesRow(obj) {


    var rownumber = parseInt($("#Drownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#Drownumber").val(rownumber);

    var modal = '<div class="modal fade" id="myModal' + rownumber + '" tabindex="-1" role="dialog" aria-hidden="true">                                                                                         ' +
        '  <div class="modal-dialog modal-lg">                                                                                                                                        ' +
        '      <div class="modal-content">                                                                                                                                            ' +
        '          <div class="modal-header vd_bg-blue vd_white">                                                                                                                     ' +
        '              <h4 class="modal-title">Add Payment Details</h4>                                                                                                               ' +
        '              <button type="button" class="close" data-dismiss="modal">&times;</button>                                                                                      ' +
        '                                                                                                                                                                             ' +
        '          </div>                                                                                                                                                             ' +
        '          <div class="modal-body">                                                                                                                                           ' +
        '              <div class="container">                                                                                                                                        ' +
        '                  <div class="row">                                                                                                                                          ' +
        '                      <div class="col-sm-12">                                                                                                                                ' +
        '                          <div class="menu">                                                                                                                                 ' +
        '                              <div class="menu" style="float: right; margin-right: 25px;">                                                                                   ' +
        '                                  <a class="btn btn-primary btn-sm btn-theme addpaymentplanrow" data-property="' + rownumber + '" style="padding: 6px 20px;">Add New</a>                               ' +
        '                              </div>                                                                                                                                         ' +
        '                          </div>                                                                                                                                             ' +
        '                          <br>                                                                                                                                               ' +
        '                          <br>                                                                                                                                               ' +
        '                          <div class="">                                                                                                                                     ' +
        '                              <table id="proplist' + rownumber + '" class="table table-bordered table-hover mb-0">                                                                            ' +
        '                                  <thead class="bg-theme">                                                                                                                   ' +
        '                                      <tr>                                                                                                                                   ' +
        '                                          <th class="text-white" style="width: 90px; text-align: center;">#</th>                                                             ' +
        '                                          <th class="text-white" style="width: 165px; text-align: center;">Charges Type</th>                                                 ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Gross Amount</th>                                                 ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Rebate</th>                                                       ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Net Amount</th>                                                   ' +
        '                                          <th class="text-white" style="width: 165px; text-align: center;">Payment Method</th>                                               ' +
        '                                          <th class="text-white">Action</th>                                                                                                 ' +
        '                                      </tr>                                                                                                                                  ' +
        '                                  </thead>                                                                                                                                   ' +
        '                                  <tbody id="Paymenttbody' + rownumber + '">                                                                                                                  ' +
        '                                  </tbody>                                                                                                                                   ' +
        '                              </table>                                                                                                                                       ' +
        '                          </div>                                                                                                                                             ' +
        '                      </div>                                                                                                                                                 ' +
        '                  </div>                                                                                                                                                     ' +
        '              </div>                                                                                                                                                         ' +
        '                                                                                                                                                                             ' +
        '                                                                                                                                                                             ' +
        '                                                                                                                                                                             ' +
        '          </div>                                                                                                                                                             ' +
        '          <div class="modal-footer background-login">                                                                                                                        ' +
        '                                                                                    ' +
        '              <button type="button" data-dismiss="modal" class="btn btn-primary btn-theme">Save</button>                                                                                          ' +
        '          </div>                                                                                                                                                             ' +
        '      </div>                                                                                                                                                                 ' +
        '      <!-- /.modal-content -->                                                                                                                                               ' +
        '  </div>                                                                                                                                                                     ' +
        '  <!-- /.modal-dialog -->                                                                                                                                                    ' +
        '</div>';



    var row = '<tr id="PR' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
        '<td> <select id="RegistrationNo' + rownumber + '" class="form-control registrationNo" ><option>Select Registration </option></select></td>' +
        '<td class="hide"> <input disabled class="form-control registration" /></td>' +
        '<td> <input disabled class="form-control propertyNo" /></td>' +
        '<td> <input disabled class="form-control project" /></td>' +
        '<td> <input disabled class="form-control realEstateType" /></td>' +
        '<td> <input disabled class="form-control block" /></td>' +
        '<td> <input type="number" class="form-control totalAmount" value="' + obj.totalAmount + '" /></td>' +
        '<td> <input type="number" value="' + obj.rebate + '" class="form-control rebate" /></td>' +
        '<td> <input type="number" value="' + obj.netReceivable + '" class="form-control netReceivable" /></td>' +
        '<td> <input disabled type="number" value="' + obj.receiedAmount + '" class="form-control receivedAmount" /></td>' +
        '<td> <input disabled type="number" value="' + obj.outstandingBalance + '" class="form-control outstandingBalance" /></td>' +
        '<td> <input value="' + obj.remarks + '" class="form-control Remarks" /></td>' +

        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePRRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a> <a class="btn btn-primary btn-sm btn-theme" data-toggle="modal" data-target="#myModal' + rownumber + '" style="padding: 6px 7px;"> Add Payment</a>' + modal + ' </td > ' +
        '</tr>';


    $("#Propertiestbody").append(row);
    GetAllPropertyListsAllData(rownumber, obj.stockId);//createdbecauseonce deal is created the property will be not available
}




function AddPropertiesRow() {


    var rownumber = parseInt($("#Drownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#Drownumber").val(rownumber);

    var modal = '<div class="modal fade" id="myModal' + rownumber + '" tabindex="-1" role="dialog" aria-hidden="true">                                                                                         ' +
        '  <div class="modal-dialog modal-lg">                                                                                                                                        ' +
        '      <div class="modal-content">                                                                                                                                            ' +
        '          <div class="modal-header vd_bg-blue vd_white">                                                                                                                     ' +
        '              <h4 class="modal-title">Add Payment Details</h4>                                                                                                               ' +
        '              <button type="button" class="close" data-dismiss="modal">&times;</button>                                                                                      ' +
        '                                                                                                                                                                             ' +
        '          </div>                                                                                                                                                             ' +
        '          <div class="modal-body">                                                                                                                                           ' +
        '              <div class="container">                                                                                                                                        ' +
        '                  <div class="row">                                                                                                                                          ' +
        '                      <div class="col-sm-12">                                                                                                                                ' +
        '                          <div class="menu">                                                                                                                                 ' +
        '                              <div class="menu" style="float: right; margin-right: 25px;">                                                                                   ' +
        '                                  <a class="btn btn-primary btn-sm btn-theme addpaymentplanrow" data-property="' + rownumber + '" style="padding: 6px 20px;">Add New</a>                               ' +
        '                              </div>                                                                                                                                         ' +
        '                          </div>                                                                                                                                             ' +
        '                          <br>                                                                                                                                               ' +
        '                          <br>                                                                                                                                               ' +
        '                          <div class="">                                                                                                                                     ' +
        '                              <table id="proplist' + rownumber + '" class="table table-bordered table-hover mb-0">                                                                            ' +
        '                                  <thead class="bg-theme">                                                                                                                   ' +
        '                                      <tr>                                                                                                                                   ' +
        '                                          <th class="text-white" style="width: 90px; text-align: center;">#</th>                                                             ' +
        '                                          <th class="text-white" style="width: 165px; text-align: center;">Charges Type</th>                                                 ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Gross Amount</th>                                                 ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Rebate</th>                                                       ' +
        '                                          <th class="text-white" style="width: 130px; text-align: center;">Net Amount</th>                                                   ' +
        '                                          <th class="text-white" style="width: 165px; text-align: center;">Payment Method</th>                                               ' +
        '                                          <th class="text-white">Action</th>                                                                                                 ' +
        '                                      </tr>                                                                                                                                  ' +
        '                                  </thead>                                                                                                                                   ' +
        '                                  <tbody id="Paymenttbody' + rownumber + '">                                                                                                                  ' +
        '                                  </tbody>                                                                                                                                   ' +
        '                              </table>                                                                                                                                       ' +
        '                          </div>                                                                                                                                             ' +
        '                      </div>                                                                                                                                                 ' +
        '                  </div>                                                                                                                                                     ' +
        '              </div>                                                                                                                                                         ' +
        '                                                                                                                                                                             ' +
        '                                                                                                                                                                             ' +
        '                                                                                                                                                                             ' +
        '          </div>                                                                                                                                                             ' +
        '          <div class="modal-footer background-login">                                                                                                                        ' +
        '                                                                                    ' +
        '              <button type="button" data-dismiss="modal" class="btn btn-primary btn-theme">Save</button>                                                                                          ' +
        '          </div>                                                                                                                                                             ' +
        '      </div>                                                                                                                                                                 ' +
        '      <!-- /.modal-content -->                                                                                                                                               ' +
        '  </div>                                                                                                                                                                     ' +
        '  <!-- /.modal-dialog -->                                                                                                                                                    ' +
        '</div>';



    var row = '<tr id="PR' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
        '<td> <select required name="RegistrationNo[' + rownumber + ']" id="RegistrationNo' + rownumber + '" class="form-control registrationNo" ><option>Select Registration </option></select></td>' +
        '<td class="hide"> <input disabled class="form-control hide registration" /></td>' +
        '<td> <input disabled class="form-control propertyNo" /></td>' +
        '<td> <input disabled class="form-control project" /></td>' +
        '<td> <input disabled class="form-control realEstateType" /></td>' +
        '<td> <input disabled class="form-control block" /></td>' +
        '<td> <input type="number" disabled class="form-control totalAmount" value="0" /></td>' +
        '<td> <input type="number" disabled class="form-control rebate" value="0" /></td>' +
        '<td> <input type="number" disabled class="form-control netReceivable" value="0" /></td>' +
        '<td> <input disabled type="number" class="form-control receivedAmount" value="0.00" /></td>' +
        '<td> <input disabled type="number" class="form-control outstandingBalance" value="0.00" /></td>' +
        '<td class="hide"> <input disabled type="number" class="form-control otpAmount" value="0.00" /></td>' +
        '<td class="hide"> <input disabled type="number" class="form-control instalmentAmount" value="0.00" /></td>' +
        '<td> <input type="text" class="form-control Remarks" /></td>' +

        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePRRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a> <a class="btn btn-primary btn-sm btn-theme" data-toggle="modal" data-target="#myModal' + rownumber + '" style="padding: 6px 7px;"> Add Payment</a>' + modal + ' </td > ' +
        '</tr>';


    $("#Propertiestbody").append(row);
    GetAllPropertyLists(rownumber);

    var qty = parseInt($("#Quantity").val());

    $("#Quantity").val(qty + 1);
}

$('body').on('click', '.addpaymentplanrow', appendpaymentplanrow);
$('body').on('change', '.registrationNo', SelectRegistration);
$('body').on('change', '.paymentMethod', CalculateInstallmentAndOneTimePaymentforpaymentPlan);
$('body').on('change', '#Rebate', calculateTotalrecievedOnRebateChange);

function SelectRegistration() {

    var select2row = $(this);
    var data_Val = $(this).select2('data');
    var stockid = data_Val[0].id;




    $.ajax({
        type: 'GET',
        url: `/api/Property/GetSingleProperty`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        data: { id: stockid },
        async: false,
        contentType: 'application/json',
        success: function (response) {
            if (response.code === 0) {

                var result_User = response.data;
                select2row.parent().parent().find(".registration").val(result_User.registrationNo);
                select2row.parent().parent().find(".propertyNo").val(result_User.propertyNo);
                select2row.parent().parent().find(".realEstateType").val(result_User.realStateTypeName);
                select2row.parent().parent().find(".project").val(result_User.projectName);
                select2row.parent().parent().find(".block").val(result_User.blockName);

            }

            else {
                //notification("topright", "error", "fa fa-exclamation-circle vd_red", "Error", response.message);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });
                // window.alert(response.message);
            }
        }
    });
}

function GetPlanDatainRow(obj, propertyno) {

    var rownumber = parseInt($("#Prownumber").val());
    var LineId = rownumber;
    
   
    var propertnumber = propertyno;

    $.each(obj, function (index, item) {
        rownumber = rownumber + 1;
        var sr = index + 1;
        var row = '<tr data-property="' + propertnumber + '" id="PD' + propertnumber + '' + index + '">' +
            '<td>' + sr + '</td>' +
            '<td><select id="chargeType' + rownumber + "" + sr + '" class="form-control chargeType"><option value="-1">Select Option</option><option value="LandCost">Land Cost</option><option value="DevCost">Dev. Cost</option></select></td>' +
            '<td><input type="number" value="' + item.grossAmount + '" class="form-control grossAmount number" /></td>' +
            '<td><input type="number" value="' + item.rebate + '" class="form-control rebate" /></td>' +
            '<td><input type="number" value="' + item.netAmount + '" class="form-control netAmount" /></td>' +
            '<td><select id="paymentMethod' + rownumber + "" + sr + '" class="form-control paymentMethod"><option value="-1">Select Option</option><option value="Installment">Installment</option><option value="OneTimePayment">One Time Payment</option></select></td>' +
            '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePDRow(' + propertnumber + '' + rownumber + ')"><span class="append-icon fa fa-trash-o"></span></a> </td > ' +
            '</tr>';
        $("#Paymenttbody" + rownumber).append(row);
        $("#chargeType" + rownumber + "" + sr).val(item.chargeType)
        $("#paymentMethod" + rownumber + "" + sr).val(item.paymentMethod)

    });
    $("#Prownumber").val(rownumber);




}


function appendpaymentplanrow() {


    var rownumber = parseInt($("#Prownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#Prownumber").val(rownumber);
    var propertnumber = $(this).attr("data-property");
    var disAttr;

    if ($("#UnitMeasure").val() != "-1") {
        disAttr = "disabled";
    }
    else {
        disAttr = "";
    }
    var row = '<tr data-property="' + propertnumber + '" id="PD' + propertnumber + '' + rownumber + '">' +
        '<td>' + rownumber + '</td>' +
        '<td><select required name="chargeType[' + rownumber + ']" class="form-control chargeType"><option value="">Select Option</option><option value="LandCost">Land Cost</option><option value="DevCost">Dev. Cost</option></select></td>' +
        '<td><input type="number" min="0" class="form-control grossAmount" value="0" /></td>' +
        '<td><input type="number" min="0" value="0" class="form-control rebatepayment" ' + disAttr + ' /></td>' +
        '<td><input type="number" disabled class="form-control netAmount" value="0" /></td>' +
        '<td><select required name="paymentMethod[' + rownumber + ']" class="form-control paymentMethod"><option value="">Select Option</option><option value="Installment">Installment</option><option value="OneTimePayment">One Time Payment</option></select></td>' +
        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePDRow(' + propertnumber + '' + rownumber + ')"><span class="append-icon fa fa-trash-o"></span></a> </td > ' +
        '</tr>';


    $("#Paymenttbody" + propertnumber).append(row);


}



function DeletePRRow(id) {
    $('#PR' + id).remove();
    var qty = parseInt($("#Quantity").val());
    $("#Quantity").val(qty - 1);
    calculatePropertiesSum();

}


function DeletePDRow(id) {
    $('#PD' + id).remove();
    var rownumber = $("#Prownumber").val();
    //$("#Paymenttbody tr").each(function () {
    //    var row = $(this);
    //    row.find(".line").val(parseInt(rownumber));
    //    rownumber++;
    //});
    $("#Prownumber").val(rownumber - 1);
}



function GetAllPropertyListsAllData(rownumber, stockid) {

    $.ajax({
        type: 'GET',
        url: `/api/Property/GetAllRegistrationNameAndIdAllData`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#RegistrationNo" + rownumber).empty();
                var option = "<option value=''>Select Registration Number</option>"
                $("#RegistrationNo" + rownumber).append(option);
                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var option = "<option value='" + list.id + "'>" + list.registrationNo + " </option>"
                        $("#RegistrationNo" + rownumber).append(option);
                    }
                    $("#RegistrationNo" + rownumber).select2();


                    if (stockid != null && stockid != '') {
                        $("#RegistrationNo" + rownumber).select2("val", [stockid]);
                    }
                }

            }
        }
    });
}
function GetAllPropertyLists(rownumber, stockid) {

    $.ajax({
        type: 'GET',
        url: `/api/Property/GetAllRegistrationNameAndId`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#RegistrationNo" + rownumber).empty();
                var option = "<option value=''>Select Registration Number</option>"
                $("#RegistrationNo" + rownumber).append(option);
                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var option = "<option value='" + list.id + "'>" + list.registrationNo + " </option>"
                        $("#RegistrationNo" + rownumber).append(option);
                    }
                    $("#RegistrationNo" + rownumber).select2();


                    if (stockid != null && stockid != '') {
                        $("#RegistrationNo" + rownumber).select2("val", [stockid]);
                    }
                }

            }
        }
    });
}

function SubmitRequest() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Save it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true,
        allowOutsideClick: false,
        allowEscapeKey: false
    }).then((result) => {
        if (result.isConfirmed) {
            Submit();
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire({
                icon: 'error',
                title: 'Cancelled',
                text: 'Your data is safe !',
                allowOutsideClick: false,
                allowEscapeKey: false
            });
        }
    })
}

function Submit() {
    var DealPropertyArr = new Array();
    var Deal = new Object();

    //set values 
    let commission = $("#Commission").val() == "" ? "0.00" : $("#Commission").val();
    let rebate = $("#Rebate").val() == "" ? "0.00" : $("#Rebate").val();
    let gracePriod = $("#GracePeriod").val() == "" ? "0" : $("#GracePeriod").val();
    let surChargePerDay = $("#SurchargePerDay").val() == "" ? "0.00" : $("#SurchargePerDay").val();

    Deal.DealerId = $("#dealerId").val();
    Deal.DealName = $("#DealName").val();
    Deal.DealNature = $("#DealNature").val();
    Deal.DealType = $("#DealType").val();
    Deal.QtyProperty = $("#Quantity").val();
    Deal.DealDate = $("#DealDate").val();
    Deal.DealExpDate = $("#ExpiryDate").val();
    Deal.CommissionType = $("#Measure").val();
    Deal.Commission = commission;
    Deal.RebateType = $("#UnitMeasure").val();
    Deal.Rebate = rebate;
    Deal.TotalValue = $("#TotalValue").val();
    Deal.NetReceivable = $("#NetReceivable").val();
    Deal.TotalReceied = $("#TotalReceived").val();
    Deal.OutstandingBalance = $("#OutstandingReceivable").val();
    Deal.GracePeriod = gracePriod;
    Deal.SurchargePerDay = surChargePerDay;
    Deal.OneTimePayment = $("#OneTimePayment").val();
    Deal.Installment = $("#Installment").val();
    Deal.ModifiedBy = $("#User_Id").val();
    Deal.CreatedBy = $("#User_Id").val();
    Deal.LastModifiedUserName = $("#User_Name").val();

    $("#Propertiestbody > tr").each(function () {
        var $row = $(this);
        var DealPaymentPlanArr = new Array();
        var paymentplannum = $row.find(".line").val();
        var paymentplanrow = "#Paymenttbody" + paymentplannum;
        var DealProperty = new Object();

        DealProperty.StockId = $row.find(".registrationNo").val();
        DealProperty.RegistrationNo = $row.find(".registration").val();
        DealProperty.PropertyNo = $row.find(".propertyNo").val();
        DealProperty.RealStateType = $row.find(".realEstateType").val();
        DealProperty.Project = $row.find(".project").val();
        DealProperty.Block = $row.find(".block").val();
        DealProperty.Rebate = $row.find(".rebate").val();
        DealProperty.TotalAmount = $row.find(".totalAmount").val();
        DealProperty.NetReceivable = $row.find(".netReceivable").val();
        DealProperty.ReceiedAmount = $row.find(".receivedAmount").val();
        DealProperty.OutstandingBalance = $row.find(".outstandingBalance").val();
        DealProperty.Remarks = $row.find(".Remarks").val();

        $(paymentplanrow + " tr").each(function () {
            var DealPaymentPlan = new Object();
            var $paymentplanrow = $(this);
            // set value 
            let rebate = $paymentplanrow.find(".rebatepayment").val() == "" ? "0" : $paymentplanrow.find(".rebatepayment").val();

            DealPaymentPlan.ChargeType = $paymentplanrow.find(".chargeType").val();
            DealPaymentPlan.GrossAmount = $paymentplanrow.find(".grossAmount").val();
            DealPaymentPlan.Rebate = rebate;
            DealPaymentPlan.NetAmount = $paymentplanrow.find(".netAmount").val();
            DealPaymentPlan.PaymentMethod = $paymentplanrow.find(".paymentMethod").val();
            DealPaymentPlan.NetTotal = $paymentplanrow.find(".netAmount").val();

            DealPaymentPlanArr.push(DealPaymentPlan);
        });
        DealProperty.DealPaymentPlan = DealPaymentPlanArr;

        DealPropertyArr.push(DealProperty);

    });

    Deal.DealProperty = DealPropertyArr;

    var ModelData = JSON.stringify(Deal);



    $('#submitbtn').prop('disabled', true); //disble
    $('#loader').fadeIn();
    $.ajax({
        type: "POST",
        url: `/api/Deal/AddNewDeal`,
        contentType: 'application/json; charset=utf-8',
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        processData: false,
        dataType: "json",
        async: false,
        data: ModelData,
        success: function (response) {



            $('#loader').fadeOut();
            console.log(response);
            if (response.code === 100) {

                //notification("bottomleft", "success", "fa fa-check-circle vd_green", "Success", response.message);
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: response.message,
                    customClass: 'swal-wide',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload(true);
                    }
                    else if (result.isDenied) {
                        Swal.fire('Changes are not saved', '', 'info')
                    }
                })


            }
            else if (response.code === 2) {
                notification("bottomleft", "warning", "fa fa-warning", "Warning", response.message);
                //Swal.fire({
                //    icon: 'warning',
                //    title: 'Warning',
                //    text: response.message,
                //    allowOutsideClick: false,
                //    allowEscapeKey: false
                //});
            }
            else {
                notification("bottomleft", "error", "fa fa-error", "Error", response.message);
                //Swal.fire({
                //    icon: 'error',
                //    title: 'Error',
                //    text: response.message,
                //    allowOutsideClick: false,
                //    allowEscapeKey: false
                //});
            }
        }

    });


    $('#submitbtn').prop('disabled', false); //enable



}
function UpdateRequest() {
    var DealPropertyArr = new Array();
    var Deal = new Object();

    //set values 
    let commission = $("#Commission").val() == "" ? "0.00" : $("#Commission").val();
    let rebate = $("#Rebate").val() == "" ? "0.00" : $("#Rebate").val();
    let gracePriod = $("#GracePeriod").val() == "" ? "0" : $("#GracePeriod").val();
    let surChargePerDay = $("#SurchargePerDay").val() == "" ? "0.00" : $("#SurchargePerDay").val();

    Deal.Id = $("#ID").val();
    Deal.DealerId = $("#dealerId").val();
    Deal.DealName = $("#DealName").val();
    Deal.DealNature = $("#DealNature").val();
    Deal.DealType = $("#DealType").val();
    Deal.QtyProperty = $("#Quantity").val();
    Deal.DealDate = $("#DealDate").val();
    Deal.DealExpDate = $("#ExpiryDate").val();
    Deal.CommissionType = $("#Measure").val();
    Deal.Commission = commission;
    Deal.RebateType = $("#UnitMeasure").val();
    Deal.Rebate = rebate;
    Deal.TotalValue = $("#TotalValue").val();
    Deal.NetReceivable = $("#NetReceivable").val();
    Deal.TotalReceied = $("#TotalReceived").val();
    Deal.OutstandingBalance = $("#OutstandingReceivable").val();
    Deal.GracePeriod = gracePriod;
    Deal.SurchargePerDay = surChargePerDay;
    Deal.OneTimePayment = $("#OneTimePayment").val();
    Deal.Installment = $("#Installment").val();
    Deal.ModifiedBy = $("#User_Id").val();
    Deal.CreatedBy = $("#User_Id").val();
    Deal.LastModifiedUserName = $("#User_Name").val();

    $("#Propertiestbody > tr").each(function () {
        var $row = $(this);
        var DealPaymentPlanArr = new Array();
        var paymentplannum = $row.find(".line").val();
        var paymentplanrow = "#Paymenttbody" + paymentplannum;
        var DealProperty = new Object();

        DealProperty.StockId = $row.find(".registrationNo").val();
        DealProperty.RegistrationNo = $row.find(".registration").val();
        DealProperty.PropertyNo = $row.find(".propertyNo").val();
        DealProperty.RealStateType = $row.find(".realEstateType").val();
        DealProperty.Project = $row.find(".project").val();
        DealProperty.Block = $row.find(".block").val();
        DealProperty.Rebate = $row.find(".rebate").val();
        DealProperty.TotalAmount = $row.find(".totalAmount").val();
        DealProperty.NetReceivable = $row.find(".netReceivable").val();
        DealProperty.ReceiedAmount = $row.find(".receivedAmount").val();
        DealProperty.OutstandingBalance = $row.find(".outstandingBalance").val();
        DealProperty.Remarks = $row.find(".Remarks").val();

        $(paymentplanrow + " tr").each(function () {

            var DealPaymentPlan = new Object();
            var $paymentplanrow = $(this);

            //set value 
            let rebate = $paymentplanrow.find(".rebatepayment").val() == "" ? "0" : $paymentplanrow.find(".rebatepayment").val();

            DealPaymentPlan.ChargeType = $paymentplanrow.find(".chargeType").val();
            DealPaymentPlan.GrossAmount = $paymentplanrow.find(".grossAmount").val();
            DealPaymentPlan.Rebate = rebate;
            DealPaymentPlan.NetAmount = $paymentplanrow.find(".netAmount").val();
            DealPaymentPlan.PaymentMethod = $paymentplanrow.find(".paymentMethod").val();
            DealPaymentPlan.NetTotal = $paymentplanrow.find(".netAmount").val();

            DealPaymentPlanArr.push(DealPaymentPlan);
        });
        DealProperty.DealPaymentPlan = DealPaymentPlanArr;

        DealPropertyArr.push(DealProperty);

    });

    Deal.DealProperty = DealPropertyArr;

    var ModelData = JSON.stringify(Deal);



    $.ajax({
        type: "PUT",
        url: `/api/Deal/UpdateDeal`,
        contentType: 'application/json; charset=utf-8',
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        processData: false,
        dataType: "json",
        async: false,
        data: ModelData,
        success: function (response) {

            console.log(response);
            if (response.code === 100) {
                //notification("bottomleft", "success", "fa fa-check-circle vd_green", "Success", response.message);
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: response.message,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });


            }
            else if (response.code === 2) {
                //notification("bottomleft", "warning", "fa fa-warning", "Warning", response.message);
                Swal.fire({
                    icon: 'warning',
                    title: 'Warning',
                    text: response.message,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });
            }
            else {
                //notification("bottomleft", "error", "fa fa-error", "Error", response.message);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });

            }
        }

    });



}
$('.number').on('keypress', function (event) {

    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

$('body').on('change', '.grossAmount', calculteTotalForpropertyrow);
$('body').on('change', '.rebatepayment', calculteTotalrebateForpropertyrow);

function calculteTotalForpropertyrow() {
    $that = $(this);
    var propertyNumber = $that.closest("tr").attr("data-property");
    var table = "#Paymenttbody" + propertyNumber + " tr";
    var rebateinheader = $("#UnitMeasure").val();

    var grosstotal = 0;
    var headernetrecievable = 0;
    var headertotalvalue = 0;
    var totalLineAmountofpaymentTable = 0;
    $(table).each(function () {
        $row = $(this);

        var linegrosstotal = $row.find(".grossAmount").val();
        var linerebatetotal = $row.find(".rebatepayment").val();

        if (rebateinheader == "Percentage") {
            $row.find(".netAmount").val(linegrosstotal);
        }
        else if (rebateinheader == "Amount") {
            $row.find(".netAmount").val(linegrosstotal);
        }
        else {

            if (linerebatetotal != null && linerebatetotal != "") {

                $row.find(".netAmount").val(linegrosstotal - linerebatetotal);
            }
            else {
                $row.find(".netAmount").val(linegrosstotal);
            }
        }
        if (linegrosstotal != '' && linegrosstotal != null) {
            grosstotal = grosstotal + parseInt(linegrosstotal);
        }
        else {
            grosstotal = grosstotal + 0;
        }
    });
    $("#PR" + propertyNumber).find(".totalAmount").val(grosstotal);
    $("#PR" + propertyNumber).find(".netReceivable").val(grosstotal);
    //Calculation on header level start
    $("#Propertiestbody tr").each(function () {
         
        $row = $(this);
        var linenetamounttotal = $row.find(".totalAmount").val();
        var linenetReceivable = $row.find(".netReceivable").val();
        if (linenetamounttotal != '' && linenetamounttotal != null) {
            debugger;
            var receivedAmount = $row.find(".receivedAmount").val();
            $row.find(".outstandingBalance").val(linenetReceivable - receivedAmount);
            headertotalvalue = headertotalvalue + parseInt(linenetamounttotal);
            headernetrecievable = headernetrecievable + parseInt(linenetReceivable);
        }
        else {
            headertotalvalue = headertotalvalue + 0;
            headernetrecievable = headernetrecievable + 0;
        }
    });

    debugger;
    if (rebateinheader == "Percentage") {
        var rebatePercenage = $("#Rebate").val();
        var afterrebatevalue = (headertotalvalue * rebatePercenage) / 100;
        $("#TotalValue").val(headertotalvalue);
        $("#NetReceivable").val(headertotalvalue - afterrebatevalue);
       var totalRecieved= $("#TotalReceived").val();
        $("#OutstandingReceivable").val((headertotalvalue - afterrebatevalue) - totalRecieved);

    }
    else if (rebateinheader == "Amount") {
        var rebateAmount = $("#Rebate").val();
        var afterrebatevalue = headertotalvalue - rebateAmount;
        $("#TotalValue").val(headertotalvalue);
        $("#NetReceivable").val(afterrebatevalue);
        var totalRecieved = $("#TotalReceived").val();
        $("#OutstandingReceivable").val(afterrebatevalue - totalRecieved);
    }
    else {
        $("#NetReceivable").val(headernetrecievable);

        $("#TotalValue").val(headertotalvalue);
        var totalRecieved = $("#TotalReceived").val();
        debugger;
        $("#OutstandingReceivable").val(headernetrecievable - parseInt(totalRecieved));
    }
    CalculateInstallmentAndOneTimePaymentforpaymentPlanviapropertyno(propertyNumber);
}

function calculateTotalrecievedOnRebateChange() {
   var rebatevalue= $("#Rebate").val();

    var rebatemeasure = $("#UnitMeasure").val();
    if (rebatemeasure != "-1") {
        $(".rebate").val(0);
        $(".rebatepayment").val(0);
       // $(".rebatepayment").attr("readonly");
        var totalvalue = 0;
        $('.rebatepayment').prop("disabled", true);
            $("#Propertiestbody tr").each(function () {
                 
                var $that = $(this);
                var linenetamounttotal = $that.find(".totalAmount").val();
                var lineotpamount = parseInt($that.find(".otpAmount").val());
                var lineinsamount = parseInt($that.find(".instalmentAmount").val());
                if (linenetamounttotal != undefined) {
                    $that.find(".netReceivable").val(linenetamounttotal);
                    var receivedAmount = $that.find(".receivedAmount").val();
                    $that.find(".outstandingBalance").val(linenetamounttotal - receivedAmount);
                    if (lineotpamount > 0) {
                        $that.find(".otpAmount").val(linenetamounttotal);
                    }
                    if (lineinsamount > 0) {
                        $that.find(".instalmentAmount").val(linenetamounttotal);
                    }
                    
                    totalvalue = totalvalue + parseInt(linenetamounttotal);
                }
                else {
                    grosstotal = $that.find(".grossAmount").val();
                    $that.find(".netAmount").val(grosstotal);
                }
                
                
            });
            if (rebatemeasure == "Percentage") {
                $("#TotalValue").val(totalvalue);
                var totalrecieved = totalvalue;
                if (totalrecieved != "") {
                    var netrecieved = parseFloat(totalrecieved) - (parseFloat(totalrecieved) * parseFloat(rebatevalue) / 100);
                    $("#NetReceivable").val(netrecieved);
                    var totalRecieved = $("#TotalReceived").val();
                    $("#OutstandingReceivable").val(netrecieved - totalRecieved);

                }

            }
            else
            {
                $("#TotalValue").val(totalvalue);
                var totalrecieved = totalvalue;
                if (totalrecieved != "") {
                    var netrecieved = parseFloat(totalrecieved) - parseFloat(rebatevalue);
                    $("#NetReceivable").val(netrecieved);
                    var totalRecieved = $("#TotalReceived").val();
                    $("#OutstandingReceivable").val(netrecieved - totalRecieved);
                }

            }
        calculatePropertiesSum();
    }
   

}
function calculteTotalrebateForpropertyrow() {

    $that = $(this);
    var propertyNumber = $that.closest("tr").attr("data-property");
    var table = "#Paymenttbody" + propertyNumber + " tr";
    var rebatetotal = 0;
    var totalamount = 0;
    var headertotalvalue = 0;
    var headernetrecievable = 0;
    var rebateinheader = $("#UnitMeasure").val();
    $(table).each(function () {
        $row = $(this);
        var linerebatetotal = $row.find(".rebatepayment").val();
        var linegrossAmount = $row.find(".grossAmount").val();

        if (linegrossAmount != null && linegrossAmount != "") {
            $row.find(".netAmount").val(linegrossAmount - linerebatetotal);
            rebatetotal = rebatetotal + parseInt(linerebatetotal);
            totalamount = totalamount + parseInt(linegrossAmount);
        }

    });
    $("#PR" + propertyNumber).find(".rebate").val(rebatetotal);
  var lineRecievedAmount=  $("#PR" + propertyNumber).find(".receivedAmount").val();
    $("#PR" + propertyNumber).find(".netReceivable").val(totalamount - rebatetotal);
    $("#PR" + propertyNumber).find(".outstandingBalance").val((totalamount - rebatetotal) - lineRecievedAmount);
    // $("#PR" + propertyNumber).find(".netReceivable").val(rebatetotal);
    $("#Propertiestbody tr").each(function () {
         
        $row = $(this);
        var linenetamounttotal = $row.find(".totalAmount").val();
        var linenetReceivable = $row.find(".netReceivable").val();
        if (linenetamounttotal != '' && linenetamounttotal != null) {
            headertotalvalue = headertotalvalue + parseInt(linenetamounttotal);
            headernetrecievable = headernetrecievable + parseInt(linenetReceivable);
        }
        else {
            headertotalvalue = headertotalvalue + 0;
            headernetrecievable = headernetrecievable + 0;
        }
    });


    if (rebateinheader == "Percentage") {
        var rebatePercenage = $("#Rebate").val();
        var afterrebatevalue = (headertotalvalue * rebatePercenage) / 100;
        $("#TotalValue").val(headertotalvalue);
        $("#NetReceivable").val(headertotalvalue - afterrebatevalue);
        var totalRecieved = $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalRecieved - (headertotalvalue - afterrebatevalue));
      //  $("#TotalReceived").val(headertotalvalue);
    }
    else if (rebateinheader == "Amount") {
        var rebateAmount = $("#Rebate").val();
        var afterrebatevalue = headertotalvalue - rebateAmount;
        $("#TotalValue").val(headertotalvalue);
        $("#NetReceivable").val(headernetrecievable);
      //  $("#TotalReceived").val(headertotalvalue);
        var totalRecieved = $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalRecieved - (headernetrecievable));
    }
    else {
        debugger;
        $("#NetReceivable").val(headernetrecievable);

        $("#TotalValue").val(headernetrecievable);
       // $("#TotalReceived").val(headertotalvalue);
        var totalRecieved = $("#TotalReceived").val();
        $("#OutstandingReceivable").val((headernetrecievable) - totalRecieved);
    }
    CalculateInstallmentAndOneTimePaymentforpaymentPlanviapropertyno(propertyNumber);

}


function CalculateInstallmentAndOneTimePaymentforpaymentPlan() {
    $that = $(this);
    var propertyNumber = $that.closest("tr").attr("data-property");
    var table = "#Paymenttbody" + propertyNumber + " tr";
    var otpAmount = 0;
    var instalmentAmount = 0;
    $(table).each(function () {

        row = $(this);
        if (row.find(".paymentMethod").val() == "Installment") {
            var rowins = parseInt(row.find(".netAmount").val());
            instalmentAmount = instalmentAmount + rowins;
        }
        else if (row.find(".paymentMethod").val() == "OneTimePayment") {
            var rowotp = parseInt(row.find(".netAmount").val());
            otpAmount = otpAmount + rowotp;
        }

    });


    $("#PR" + propertyNumber).find(".otpAmount").val(otpAmount);
    $("#PR" + propertyNumber).find(".instalmentAmount").val(instalmentAmount);
    calclateInstallmentandonetimepaymentforheader();
}
function calclateInstallmentandonetimepaymentforheader() {
    var temptotalopt = 0;
    var temptotalins = 0;
    var countlimit = parseInt($("#Quantity").val());
    var count = 0;

    $("#Propertiestbody > tr").each(function () {

        if (countlimit > count) {
            var currotp = $(this).find(".otpAmount").val() != "" ? parseInt($(this).find(".otpAmount").val()) : 0;
            var currins = $(this).find(".instalmentAmount").val() != "" ? parseInt($(this).find(".instalmentAmount").val()) : 0;
            temptotalopt = temptotalopt + currotp;
            temptotalins = temptotalins + currins;
            count++;
        }

    });
    $("#OneTimePayment").val(temptotalopt);
    $("#Installment").val(temptotalins);
}

function CalculateInstallmentAndOneTimePaymentforpaymentPlanviapropertyno(propertyId) {
    var propertyNumber = propertyId;
    var table = "#Paymenttbody" + propertyNumber + " tr";
    var otpAmount = 0;
    var instalmentAmount = 0;
    $(table).each(function () {

        row = $(this);
        if (row.find(".paymentMethod").val() == "Installment") {
            var rowins = parseInt(row.find(".netAmount").val());
            instalmentAmount = instalmentAmount + rowins;
        }
        else if (row.find(".paymentMethod").val() == "OneTimePayment") {
            var rowotp = parseInt(row.find(".netAmount").val());
            otpAmount = otpAmount + rowotp;
        }

    });


    $("#PR" + propertyNumber).find(".otpAmount").val(otpAmount);
    $("#PR" + propertyNumber).find(".instalmentAmount").val(instalmentAmount);
    calclateInstallmentandonetimepaymentforheader();
}

function calculatePropertiesSum() {
    var rebateinheader = $("#UnitMeasure").val();
    var headertotalvalue = 0;
    var headernetrecievable = 0;
    var headerinstallment = 0;
    var headerotp = 0;
    $("#Propertiestbody tr").each(function () {

        $row = $(this);
        var linenetamounttotal = $row.find(".totalAmount").val();
        var linenetReceivable = $row.find(".netReceivable").val();
        var lineins = $row.find(".instalmentAmount").val();
        var lineotp = $row.find(".otpAmount").val();
        if (linenetamounttotal != '' && linenetamounttotal != null) {
            headertotalvalue = headertotalvalue + parseInt(linenetamounttotal);
            headernetrecievable = headernetrecievable + parseInt(linenetReceivable);
            headerinstallment = headerinstallment + parseInt(lineins);
            headerotp = headerotp + parseInt(lineotp);
        }
        else {
            headertotalvalue = headertotalvalue + 0;
            headernetrecievable = headernetrecievable + 0;
            headerinstallment = headerinstallment + parseInt(0);
            headerotp = headerotp + parseInt(0);
        }
    });

    debugger;
    if (rebateinheader == "Percentage") {
        var rebatePercenage = $("#Rebate").val();
        var afterrebatevalue = (headertotalvalue * rebatePercenage) / 100;
        $("#TotalValue").val(headertotalvalue);
        $("#OneTimePayment").val(headerotp);
        $("#Installment").val(headerinstallment);
        $("#NetReceivable").val(headertotalvalue - afterrebatevalue);
    }
    else if (rebateinheader == "Amount") {
        var rebateAmount = $("#Rebate").val();
        var afterrebatevalue = headertotalvalue - rebateAmount;
        $("#TotalValue").val(headertotalvalue);
        $("#NetReceivable").val(afterrebatevalue);
        $("#OneTimePayment").val(headerotp);
        $("#Installment").val(headerinstallment);
    }
    else {
        $("#NetReceivable").val(headernetrecievable);

        $("#TotalValue").val(headertotalvalue);
        $("#OneTimePayment").val(headerotp);
        $("#Installment").val(headerinstallment);
    }
}



