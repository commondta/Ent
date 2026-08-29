

//let Global_Array = new Array();
//show data
$(document).ready(function () {
    $('.clickable-row').css("cursor", "pointer");
    $('.clickable-row').click(function () {
        window.location = "https://www.google.com.pk/";
    });
});


//function view() {
//    $.ajax({
//        type: "get",
//        url: "@Url.Action("list", "Company")",
//        success: function (result) {
//            if (result != "error")
//            {
//                Global_Array = result;
//                $("td").remove();
//                for (let i = 0; i < result.length; i++) {
//                    var tab = $('<tr />');
//                    tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].email + "</td>", "<td>" + result[i].password + "</td>", "<td>" + result[i].companyName + "</td>", "<td>" + result[i].address + "</td>", "<td>" + result[i].phone + "</td>", "<td >" + "<a onclick='all(" + result[i].id + ")'>ViewAll</a> | <a onclick='Edit(" + result[i].id + ")'>Edit</a> | <a onclick='del(" + result[i].id + ")'>DELETE</a>" + "</td>");
//                    $('#myTable').append(tab);
//                }
//            }
//        }
//});
//}

//$("#search").keyup(function () {
//    let value = $("#search").val();
//    if (value) {
//        $.ajax({
//            type: "post",
//            data: { term: value },
//            url: "@Url.Action("auto", "Company")",
//            success: function (result) {
//                $('#showList').empty();
//                for (let i = 0; i < result.length; i++) {
//                    console.log(result[i].email);
//                    $("#showList").append('<li>' + result[i].email + '</li>');
//                }
//            }
//    });
//}
//else {
//            $('#showList').empty();
//}
            
//});

//view employees agaist these company
//function all(idRow) {
//    var id = idRow
//    debugger;
//    $.ajax({
//        type: "get",
//        url: "@Url.Action("allEmployee", "Company")",
//        data: {
//        id: id,
//        },
//    success: function (result) {
//        if (result != "error") {
//            $("td").remove();
//            for (let i = 0; i < result.length; i++) {
//                var tab = $('<tr />');
//                $('#myTable').append(tab);
//            }
//        }
//    }
//});
//}

//add Data
//function Add() {
//    $('#addCompModal').modal('show');
//}

function AddCompany() {
    var company = {
        "email": $("#email").val(),
        "password": $("#password").val(),
        "companyName": $("#cname").val(),
        "address": $("#address").val(),
        "phone": $("#phone").val()
    }

    $.ajax({
        url: '/Admin/AddCompany',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ obj: company }),
        success: function (result) {
            $('#addCompModal').modal('hide');
            if (result != "error")
            {
                $("td").remove();
                for (let i = 0; i < result.length; i++) {
                    var tab = $('<tr />');
                    tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].email + "</td>", "<td>" + result[i].password + "</td>", "<td>" + result[i].companyName + "</td>", "<td>" + result[i].address + "</td>", "<td>" + result[i].phone + "</td>", "<td >" + "<a onclick='Edit(" + result[i].id + ")'>Edit</a> | <a onclick='del(" + result[i].id + ")'>DELETE</a>" + "</td>");
                    $('#myTable').append(tab);
                }
            }
        }
    });
}

//Edit data
//function Edit(idRow) {
//    for (let i = 0; i < Global_Array.length; i++) {
//        if (Global_Array[i].id == idRow) {
//            $('#myModal2').modal('show');
//            $("#eid").val(idRow);
//            $('#eemail').val(Global_Array[i].email);
//            $('#epassword').val(Global_Array[i].password);
//            $('#ecname').val(Global_Array[i].companyName);
//            $('#eaddress').val(Global_Array[i].address);
//            $('#ephone').val(Global_Array[i].phone); 
//            $('#elogid').val(Global_Array[i].login_id);
//        }
//    }
//}

//function update() {
//    let id = $("#eid").val();
//    let email = $("#eemail").val();
//    let password = $("#epassword").val();
//    let company = $("#ecname").val();
//    let address = $("#eaddress").val();
//    let phone = $("#ephone").val();
//    let logid = $("#elogid").val();
//    $.ajax({
//        type: "post",
//        url: "@Url.Action("EditCompany", "Company")",
//        data: {
//        id:id,
//        email: email,
//        password: password,
//        company: company,
//        address: address,
//        phone: phone
//        },
//    success: function (result) {
//        $('#myModal2').modal('hide');
//        if (result != "error") {
//            $("td").remove();
//            for (let i = 0; i < result.length; i++) {
//                var tab = $('<tr />');
//                tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].email + "</td>", "<td>" + result[i].password + "</td>", "<td>" + result[i].companyName + "</td>", "<td>" + result[i].address + "</td>", "<td>" + result[i].phone + "</td>", "<td >" + "<a onclick='Edit(" + result[i].id + ")'>Edit</a> | <a onclick='del(" + result[i].id + ")'>DELETE</a>" + "</td>");
//                $('#myTable').append(tab);
//            }
//        }
//    }
//});
//}

// Delete data
//function del(idrow) {
//    var id = idrow
//    $.ajax({
//        type: "post",
//        url: "@Url.Action("DeleteCompany", "Company")",
//        data: {
//        id: id,
//        },
//    success: function (result) {
//        if (result != "error") {
//            $("td").remove();
//            for (let i = 0; i < result.length; i++) {
//                var tab = $('<tr />');
//                tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].email + "</td>", "<td>" + result[i].password + "</td>", "<td>" + result[i].companyName + "</td>", "<td>" + result[i].address + "</td>", "<td>" + result[i].phone + "</td>", "<td >" + "<a onclick='Edit(" + result[i].id + ")'>Edit</a> | <a onclick='del(" + result[i].id + ")'>DELETE</a>" + "</td>");
//                $('#myTable').append(tab);
//            }
//        }
//    }
//});
//}