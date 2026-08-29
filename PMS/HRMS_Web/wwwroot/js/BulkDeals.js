
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
        url: `/api/BulkDeal/GetAll`,
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


    var url = "/api/BulkDeal/Get?id=" + id + "";

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
                $("#PaymentScheduleTbody").empty();
                $("#ProposePlanbody").empty();


                $.each(dealData.bulkDealProperty, function (index, Propertyitem) {
                    GetPropertiesDataInPropertiesRow(Propertyitem);
                   
                });
                $.each(dealData.bulkDealProposePlan, function (index, Propertyitem) {
                    GetProposePlan(Propertyitem);
                   
                });
                
                $.each(dealData.bulkPaymentSchedule, function (index, Propertyitem) {
                    GetBulkPaymentSchedule(Propertyitem);
                   
                });

                $("#findmodal").modal("hide");
                $("#submitbtn").addClass("hide");
                $("#updatebtn").removeClass("hide");
                $("#AddPropBtn").removeClass("hide");

            }

            else {
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


    var rownumber = parseInt($("#Prownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#Prownumber").val(rownumber);

    var row = '<tr id="PR' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
        '<td> <select style="width:100%" id="RegistrationNo' + rownumber + '" class="form-control registrationNo" ><option>Select Registration </option></select></td>' +
        '<td class="hide"> <input disabled class="form-control registration" /></td>' +
        '<td> <input disabled class="form-control propertyNo" /></td>' +
        '<td><input disabled class="form-control category" /></td>' +
        '<td> <input disabled class="form-control project" /></td>' +
        '<td> <input disabled class="form-control realEstateType" /></td>' +
        '<td> <input disabled class="form-control block" /></td>' +
        '<td> <input disabled type="number" class="form-control totalAmount" value="' + obj.totalAmount + '" /></td>' +
        '<td> <input type="number" value="' + obj.rebate + '" class="form-control rebate" /></td>' +
        '<td> <input disabled type="number" value="' + obj.netReceivable + '" class="form-control netReceivable" /></td>' +
        '<td> <input disabled type="number" value="' + obj.receiedAmount + '" class="form-control receivedAmount" /></td>' +
        '<td> <input disabled type="number" value="' + obj.outstandingBalance + '" class="form-control outstandingBalance" /></td>' +
        '<td> <input value="' + obj.remarks + '" class="form-control Remarks" /></td>' +

        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePRRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>' +
        '</tr>';


    $("#Propertiestbody").append(row);
    GetAllPropertyListsAllData(rownumber, obj.stockId);//createdbecauseonce deal is created the property will be not available
    $("#Quantity").val(rownumber);
}


function GetProposePlan(obj) {

    var rownumber = parseInt($("#PPrownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#PPrownumber").val(rownumber);
    var row = '<tr id="PP' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +

        '<td> <select style="width:100%" name="Category[' + rownumber + ']" id="Category' + rownumber + '" class="form-control category" ><option>Select Category </option></select></td>' +
        '<td> <input value="'+obj.quantity+'" class="form-control quantity" /></td>' +
        '<td> <input value="' + obj.unitPrice +'" class="form-control unitPrice" /></td>' +
        '<td> <input readonly value="' + obj.totalAmount +'" class="form-control totalAmount" /></td>' +


        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePPRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>' +
        '</tr>';


    $('#ProposePlanbody').append(row);
    GetAllCategories(rownumber, obj.categoryId);

}

function GetBulkPaymentSchedule(obj) {
    var dueDate = obj.dueDate == null || obj.dueDate == "" ? "0001-01-01" : obj.dueDate.split("T")[0];
    debugger;
    var rownumber = parseInt($("#PSrownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#PSrownumber").val(rownumber);
    debugger;
    var row = '<tr id="PP' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled  class="form-control line" value="' + rownumber + '" /></td>' +
        '<td> <input value="' + dueDate +'" type="date" class="form-control dueDate" /></td>' +
        '<td> <input value="' + obj.amount +'" type="number" class="form-control amount" /></td>' +
        '<td> <input value="' + obj.remarks +'" class="form-control remarks" /></td>' +
        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePSRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>' +
        '</tr>';
    $("#PaymentScheduleTbody").append(row);
}




function AddProposePlanRow() {
    var rownumber = parseInt($("#PPrownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $('#PPrownumber').val(rownumber);
    debugger;
    var row = '<tr id="PP' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
      
        '<td> <select name="Category[' + rownumber + ']" id="Category' + rownumber + '" class="form-control category" ><option>Select Category </option></select></td>' +
        '<td> <input class="form-control quantity" /></td>' +
        '<td> <input class="form-control unitPrice" /></td>' +
        '<td> <input class="form-control totalAmount" /></td>' +
        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePPRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>' +
        '</tr>';
    $('#ProposePlanbody').append(row);
    GetAllCategories(rownumber);

}

function AddPaymentScheduleRow() {
    var rownumber = parseInt($("#PSrownumber").val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $("#PSrownumber").val(rownumber);
    debugger;
    var row = '<tr id="PP' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
       '<td> <input type="date" class="form-control dueDate" /></td>' +
        '<td> <input type="number" class="form-control amount" /></td>' +
        '<td> <input class="form-control remarks" /></td>' +
        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePSRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>' +
        '</tr>';
    $("#PaymentScheduleTbody").append(row);
}

function AddPropertiesRow() {
   
  
    var rownumber = parseInt($('#Prownumber').val());
    var LineId = rownumber;
    rownumber = rownumber + 1;
    $('#Prownumber').val(rownumber);
    debugger;
    var row = '<tr id="PR' + rownumber + '" DetailId="0"> ' +
        '<td> <input disabled class="form-control line" value="' + rownumber + '" /></td>' +
        '<td> <select required name="RegistrationNo[' + rownumber + ']" id="RegistrationNo' + rownumber + '" class="form-control registrationNo" ><option>Select Registration </option></select></td>' +
        '<td class="hide"> <input disabled class="form-control hide registration" /></td>' +
        '<td> <input disabled class="form-control propertyNo" /></td>' +
        '<td> <input disabled class="form-control category" /></td>' +
        '<td> <input disabled class="form-control project" /></td>' +
        '<td> <input disabled class="form-control realEstateType" /></td>' +
        '<td> <input disabled class="form-control block" /></td>' +
        '<td> <input type="number" disabled class="form-control totalAmount" value="0" /></td>' +
        '<td> <input  type="number" class="form-control rebate" value="0" /></td>' +
        '<td> <input type="number" disabled class="form-control netReceivable" value="0" /></td>' +
        '<td> <input disabled type="number" class="form-control receivedAmount" value="0.00" /></td>' +
        '<td> <input disabled type="number" class="form-control outstandingBalance" value="0.00" /></td>' +
        '<td class="hide"> <input disabled type="number" class="form-control otpAmount" value="0.00" /></td>' +
        '<td class="hide"> <input disabled type="number" class="form-control instalmentAmount" value="0.00" /></td>' +
        '<td> <input type="text" class="form-control Remarks" /></td>' +

        '<td style="padding-top:10px"><a class="btn btn-sm btn-danger" onclick="return DeletePRRow(\'' + rownumber + '\')"><span class="append-icon fa fa-trash-o"></span></a>  </td > ' +
        '</tr>';


    $("#Propertiestbody").append(row);
    GetAllPropertyLists(rownumber);

    $("#Quantity").val(rownumber);
}

$('body').on('change', '.registrationNo', SelectRegistration);

function SelectRegistration() {

    var select2row = $(this);
    var data_Val = $(this).select2('data');
    var stockid = data_Val[0].id;


    var dealId=$("#ID").val();
    if (dealId != "" && dealId != null) {
        $.ajax({
            type: 'GET',
            url: `/api/Property/GetSinglePropertyForBulkDeal`,
            dataType: "json",
            headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
            data: { id: stockid, dealId: dealId },
            async: false,
            contentType: 'application/json',
            success: function (response) {
                if (response.code === 0) {

                    var result_User = response.data;
                    select2row.parent().parent().find(".registration").val(result_User.registrationNo);
                    select2row.parent().parent().find(".category").val(result_User.categoryName);
                    select2row.parent().parent().find(".propertyNo").val(result_User.propertyNo);
                    select2row.parent().parent().find(".realEstateType").val(result_User.realStateTypeName);
                    select2row.parent().parent().find(".project").val(result_User.projectName);
                    select2row.parent().parent().find(".block").val(result_User.blockName);
                    select2row.parent().parent().find(".totalAmount").val(result_User.bulkDealAmount);

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
    else {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: "Please Select The Posted Deal First",
            allowOutsideClick: false,
            allowEscapeKey: false
        });
    }
  
}

function DeletePRRow(id) {Prownumber
    $('#PR' + id).remove();
    var qty = parseInt($("#Quantity").val());
    $("#Quantity").val(qty - 1);

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
function GetAllCategories(rownumber, categoryId) {

    $.ajax({
        type: 'GET',
        url: `/api/Category/GetAllCategories`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Category" + rownumber).empty();
                var option = "<option value=''>Select Category</option>"
                $("#Category" + rownumber).append(option);
                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var option = "<option value='" + list.id + "'>" + list.description + " </option>"
                        $("#Category" + rownumber).append(option);
                    }
                    $("#Category" + rownumber).select2();


                    if (categoryId != null && categoryId != '') {
                        $("#Category" + rownumber).select2("val", [categoryId]);
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
    debugger;
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
    debugger;
    var DealPropertyArr = new Array();
    var DealScheduleArr = new Array();
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


    $('#ProposePlanbody tr').each(function () {
        var $row = $(this);
        var DealProposePlan = new Object();

        DealProposePlan.CategoryId = $row.find('.category').val();
        DealProposePlan.Quantity = $row.find('.quantity').val();
        DealProposePlan.UnitPrice = $row.find('.unitPrice').val();
        DealProposePlan.TotalAmount = $row.find('.totalAmount').val();
        DealPropertyArr.push(DealProposePlan);

    });
    
    $("#PaymentScheduleTbody tr").each(function () {
        var $row = $(this);
        var PaymentSchedule = new Object();

        PaymentSchedule.DueDate = $row.find(".dueDate").val();
        PaymentSchedule.Amount = $row.find(".amount").val();
        PaymentSchedule.Remarks = $row.find('.remarks').val();

        DealScheduleArr.push(PaymentSchedule);

    });

    Deal.BulkDealProposePlan = DealPropertyArr;
    Deal.BulkPaymentSchedule = DealScheduleArr;

    var ModelData = JSON.stringify(Deal);
    debugger;


    $('#submitbtn').prop('disabled', true); //disble
    $('#loader').fadeIn();
    $.ajax({
        type: "POST",
        url: `/api/BulkDeal/AddNewBulkDeal`,
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
    var DealProposePlanArr = new Array();
    var DealScheduleArr = new Array();
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

    $("#Propertiestbody tr").each(function () {
        var $row = $(this);
        var DealProperty = new Object();

        DealProperty.StockId = $row.find(".registrationNo").val();
        DealProperty.RegistrationNo = $row.find(".registration").val();
        DealProperty.PropertyNo = $row.find(".propertyNo").val();
        DealProperty.Category = $row.find(".category").val();
        DealProperty.RealStateType = $row.find(".realEstateType").val();
        DealProperty.Project = $row.find(".project").val();
        DealProperty.Block = $row.find(".block").val();
        DealProperty.Rebate = $row.find(".rebate").val();
        DealProperty.TotalAmount = $row.find(".totalAmount").val();
        DealProperty.NetReceivable = $row.find(".netReceivable").val();
        DealProperty.ReceiedAmount = $row.find(".receivedAmount").val();
        DealProperty.OutstandingBalance = $row.find(".outstandingBalance").val();
        DealProperty.Remarks = $row.find(".Remarks").val();

        DealPropertyArr.push(DealProperty);

    });


    Deal.BulkDealProperty = DealPropertyArr;
    debugger;
    $('#ProposePlanbody tr').each(function () {
        var $row = $(this);
        var DealProposePlan = new Object();

        DealProposePlan.CategoryId = $row.find('.category').val();
        DealProposePlan.Quantity = $row.find('.quantity').val();
        DealProposePlan.UnitPrice = $row.find('.unitPrice').val();
        DealProposePlan.TotalAmount = $row.find('.totalAmount').val();
        DealProposePlanArr.push(DealProposePlan);

    });

    Deal.BulkDealProposePlan = DealProposePlanArr;
    $("#PaymentScheduleTbody tr").each(function () {
        var $row = $(this);
        var PaymentSchedule = new Object();

        PaymentSchedule.DueDate = $row.find(".dueDate").val();
        PaymentSchedule.Amount = $row.find(".amount").val();
        PaymentSchedule.Remarks = $row.find('.remarks').val();

        DealScheduleArr.push(PaymentSchedule);

    });
    Deal.BulkPaymentSchedule = DealScheduleArr;
    var ModelData = JSON.stringify(Deal);



    $.ajax({
        type: "PUT",
        url: `/api/BulkDeal/UpdateBulkDeal`,
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

$('body').on('change', '.quantity', calculateTotalValueOnQtyChange);
$('body').on('change', '.unitPrice', calculateTotalValueOnQtyChange);
$('body').on('change', '.rebate', calculateRebateOnPropertiesGrid);
$('body').on('change', '#Rebate', calculateRebateOnHeaderChange);

function calculateTotalValueOnQtyChange() {

    debugger;
    $that = $(this);
    var quantity = $that.closest("tr").find(".quantity").val() == '' ? 0 : parseInt($that.closest("tr").find(".quantity").val()) ;
    var unitprice = $that.closest("tr").find(".unitPrice").val() == '' ? 0 : parseInt($that.closest("tr").find(".unitPrice").val());
    var totalprice = quantity * unitprice;
    $that.closest("tr").find(".totalAmount").val(totalprice);
    calculateCategoriesSum();
}
function calculateRebateOnPropertiesGrid() {
    //setting Grid values start
    debugger;
    $that = $(this);
    var rebate = $that.closest("tr").find(".rebate").val() == '' ? 0 : parseInt($that.closest("tr").find(".rebate").val()) ;
    var totalAmount = $that.closest("tr").find(".totalAmount").val() == '' ? 0 : parseInt($that.closest("tr").find(".totalAmount").val());
    var netRecievable = totalAmount - rebate;
    $that.closest("tr").find(".netReceivable").val(netRecievable);

    var recievedAmount = $that.closest("tr").find(".receivedAmount").val();

    $that.closest("tr").find(".outstandingBalance").val(netRecievable - recievedAmount);
    //setting Grid Values end

    ////setting header values
    //let SumOfRebate = 0;
    //let SumOftotalAmount = 0;
    //$("#Propertiestbody tr").each(function () {
    //    var $row = $(this);

    //    SumOfRebate += $row.find(".rebate").val() != '' ? parseInt($row.find(".rebate").val()):0;
    //    SumOftotalAmount += $row.find(".totalAmount").val() != '' ? parseInt($row.find(".totalAmount").val()) : 0;

    //});
    //$("#TotalValue").val(SumOftotalAmount);
    ////$("#Rebate").val(SumOfRebate);
    //var netrecievable = SumOftotalAmount - SumOfRebate;
    //$("#NetReceivable").val(netrecievable);
    //var totalrecieved= $("#TotalReceived").val();
    //$("#OutstandingReceivable").val(netrecievable - totalrecieved);

    //setting header values end

}

function calculateRebateOnHeaderChange() {

    var rebateType = $("#UnitMeasure").val();
    var Rebatevalue = $("#Rebate").val() == '' ? 0 : $("#Rebate").val();
    if (rebateType == 'Percentage') {
        var SumOfTotalAmount = 0;
        $(".rebate").prop("disabled", true);
        $("#ProposePlanbody tr").each(function () {
            var $row = $(this);
            var currentRowtotalAmount = $row.find(".totalAmount").val() == "" ? 0 : $row.find(".totalAmount").val();
            SumOfTotalAmount = SumOfTotalAmount + parseInt(currentRowtotalAmount);
        });
        $("#TotalValue").val(SumOfTotalAmount);
        var totalnetrecievable = SumOfTotalAmount - (SumOfTotalAmount *(Rebatevalue / 100));
        $("#NetReceivable").val(totalnetrecievable);
        var totalRecievable = $("#TotalReceived").val() == '' ? 0 : $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalnetrecievable - totalRecievable)

    }
    else if (rebateType == 'Amount') {
        var SumOfTotalAmount = 0;
        $("#ProposePlanbody tr").each(function () {
            var $row = $(this);
            var currentRowtotalAmount = $row.find(".totalAmount").val() == "" ? 0 : $row.find(".totalAmount").val();
            SumOfTotalAmount = SumOfTotalAmount + parseInt(currentRowtotalAmount);
        });
        $("#TotalValue").val(SumOfTotalAmount);
        var totalnetrecievable = SumOfTotalAmount - Rebatevalue;
        $("#NetReceivable").val(totalnetrecievable);
        var totalRecievable = $("#TotalReceived").val() == '' ? 0 : $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalnetrecievable - totalRecievable)
    }
}

function calculateCategoriesSum() {
    var UnitMeasure = $("#UnitMeasure").val();
    var Rebatevalue = $("#Rebate").val() == '' ? 0 : $("#Rebate").val();
    var sumgridTotalAmount = 0;
    $("#ProposePlanbody tr").each(function () {
        var $row = $(this);
        sumgridTotalAmount += $row.find(".totalAmount").val() != '' ? parseInt($row.find(".totalAmount").val()) : 0;

    });
    $("#TotalValue").val(sumgridTotalAmount);
    debugger;
    if (UnitMeasure == "Percentage") {
        var totalnetrecievable = sumgridTotalAmount - (sumgridTotalAmount * (Rebatevalue / 100));
        $("#NetReceivable").val(totalnetrecievable);
        var totalRecievable = $("#TotalReceived").val() == '' ? 0 : $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalnetrecievable - totalRecievable)
    }
    else if (UnitMeasure == "Amount") {
        var totalnetrecievable = sumgridTotalAmount - Rebatevalue;
        $("#NetReceivable").val(totalnetrecievable);
        var totalRecievable = $("#TotalReceived").val() == '' ? 0 : $("#TotalReceived").val();
        $("#OutstandingReceivable").val(totalnetrecievable - totalRecievable)
    }
    else {
        $("#NetReceivable").val(sumgridTotalAmount);
        var totalRecievable = $("#TotalReceived").val() == '' ? 0 : $("#TotalReceived").val();
        $("#OutstandingReceivable").val(sumgridTotalAmount - totalRecievable)
    }



}


