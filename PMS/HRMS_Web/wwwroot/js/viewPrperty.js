
function GetAllTypes() {

    $.ajax({
        type: 'GET',
        url: `/api/PropertyType/GetAllPropertyTypes`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Type").empty();
                var row = ' <option value="-1">Select Option</option> ';

                $("#Type").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Type").append(row);


                        sr++;

                    }
                }

                //  $('#Type').select2();
            }
        }
    });


}



function GetAllFloor() {

    $.ajax({
        type: 'GET',
        url: `/api/Floor/GetAllBlocks`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Floor").empty();
                var row = ' <option value="-1">Select Option</option> ';

                $("#Floor").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Floor").append(row);


                        sr++;

                    }
                }

                // $('#Floor').select2();
            }
        }
    });


}


function GetAllBlocks() {

    $.ajax({
        type: 'GET',
        url: `/api/Block/GetAllBlocks`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Block").empty();
                var row = ' <option value="-1">Select Option</option> ';

                $("#Block").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Block").append(row);


                        sr++;

                    }
                }

                //  $('#Block').select2();
            }
        }
    });


}
function GetAllFinishing() {

    $.ajax({
        type: 'GET',
        url: `/api/Finishes/GetAllFinishes`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Finishing").empty();
                var row = ' <option value="-1">Select Finishing</option> ';

                $("#Finishing").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Finishing").append(row);


                        sr++;

                    }
                }

                //  $('#Finishing').select2();
            }
        }
    });


}



function GetAllSizes() {

    $.ajax({
        type: 'GET',
        url: `/api/Size/GetAllSizes`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Size").empty();
                var row = ' <option value="-1">Select Size</option> ';

                $("#Size").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Size").append(row);


                        sr++;

                    }
                }

                //   $('#Size').select2();
            }
        }
    });


}


function GetAllCategories() {

    $.ajax({
        type: 'GET',
        url: `/api/Category/GetAllCategories`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Category").empty();
                var row = ' <option value="-1">Select Category</option> ';

                $("#Category").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Category").append(row);


                        sr++;

                    }
                }

                //   $('#Category').select2();
            }
        }
    });


}

function GetAllNatures() {

    $.ajax({
        type: 'GET',
        url: `/api/Nature/GetAllNatures`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Nature").empty();
                var row = ' <option value="-1">Select Nature</option> ';

                $("#Nature").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Nature").append(row);


                        sr++;

                    }
                }

                //  $('#Nature').select2();
            }
        }
    });


}
function GetAllPhases() {

    $.ajax({
        type: 'GET',
        url: `/api/Phase/GetAllPhases`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Phase").empty();
                var row = ' <option value="-1">Select Phase</option> ';

                $("#Phase").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Phase").append(row);


                        sr++;

                    }
                }

                //  $('#Phase').select2();
            }
        }
    });


}

function GetAllRealEstate() {
    debugger;
    $.ajax({
        type: 'GET',
        url: `/api/RealEstate/GetAllRealEstate`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#RealStateType").empty();
                var row = ' <option value="-1">Select Option</option> ';

                $("#RealStateType").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#RealStateType").append(row);


                        sr++;

                    }
                }

                //  $('#Phase').select2();
            }
        }
    });


}


function GetAllProjects() {

    $.ajax({
        type: 'GET',
        url: `/api/Project/GetAllProjects`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        async: false,
        contentType: 'application/json',
        success: function (response) {

            if (response.code === 0) {

                $("#Project").empty();
                var row = ' <option value="-1">Select Option</option> ';

                $("#Project").append(row);

                var relist = response.data;
                if (relist != null && relist.length > 0) {
                    var sr = 1;
                    for (let list of relist) {
                        var row = ' <option value="' + list.id + '" >'   + list.description + '</option> ';

                        $("#Project").append(row);


                        sr++;

                    }
                }

                // $('#Project').select2();
            }
        }
    });


}






function EditProperty(id) {

    $.ajax({
        type: 'GET',
        url: `/api/Property/GetSingleProperty`,
        dataType: "json",
        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
        data: { id: id },
        async: false,
        contentType: 'application/json',
        success: function (response) {
            if (response.code === 0) {
                var result_User = response.data;
                $("#RegistrationNo").val(result_User.registrationNo);
                $("#PropertyNo").val(result_User.propertyNo);
                $('#RealStateType').val(result_User.realStateType);
                $('#Phase').val(result_User.phase);
                $('#Category').val(result_User.category);
                $('#Type').val(result_User.type);
                $('#Nature').val(result_User.nature);
                $('#Finishing').val(result_User.finishing);
                $('#User').val(result_User.user);
                $('#Floor').val(result_User.floor);
                $('#ActualSize').val(result_User.actualSize);
                $('#ActualSizeUnit').val(result_User.actualSizeUnit);
                $('#Project').val(result_User.project);
                $('#Block').val(result_User.block);
                $('#StandardSize').val(result_User.standardSize);
                $('#StandardSizeUnit').val(result_User.standardSizeUnit);
                $('#Status').val(result_User.status);



                $("#myModal").modal("show");
            }

            else {
                notification("topright", "error", "fa fa-exclamation-circle vd_red", "Error", response.message);
                // window.alert(response.message);
            }
        }
    });
}