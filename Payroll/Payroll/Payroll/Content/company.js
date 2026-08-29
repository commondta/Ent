$(document).ready(function () {
    $('#emp-salary-dataTable input[type="checkbox"]').parent().css("text-align", "center");
    $('#emp-leave-dataTable input[type="checkbox"]').parent().css("text-align", "center");
});

function addEmployee() {
    var employeeDetail_tab = $("#tab_emp-details");
    var employeeDetail = {
        "employeeFirstName": employeeDetail_tab.find("#employee-first-name").val(),
        "employeeLastName": employeeDetail_tab.find("#employee-last-name").val(),
        "fatherName": employeeDetail_tab.find("#father-name").val(),
        "employeeCategory": employeeDetail_tab.find("#employee-category").val(),
        "grade": employeeDetail_tab.find("#grade").val(),
        "gender": employeeDetail_tab.find("#gender").val(),
        "shift": employeeDetail_tab.find("#shift").val(),
        "designation": employeeDetail_tab.find("#designation").val(),
        "department": employeeDetail_tab.find("#department").val(),
        "sectionType": employeeDetail_tab.find("#section-type").val(),
        "dateOfBirth": employeeDetail_tab.find("#date-of-birth").val(),
        "nationality": employeeDetail_tab.find("#nationality").val(),
        "homePhoneNo": employeeDetail_tab.find("#home-phone-no").val(),
        "mobilePhoneNo1": employeeDetail_tab.find("#mobile-phone-no-1").val(),
        "mobilePhoneNo2": employeeDetail_tab.find("#mobile-phone-no-2").val(),
        "email": employeeDetail_tab.find("#email").val(),
        "dateOfJoining": employeeDetail_tab.find("#date-of-joining").val(),
        "originalDateOfBirth": employeeDetail_tab.find("#original-date-of-birth").val(),
        "insurancePolicyNo": employeeDetail_tab.find("#insurance-policy-no").val(),
        "pfNo": employeeDetail_tab.find("#pf-no").val(),
        "otherInfo": employeeDetail_tab.find("#other-info").val(),
        "incrementEffectiveMonth": employeeDetail_tab.find("#increment-effective-month").val(),
        "passportSubmissionDate": employeeDetail_tab.find("#passport-submission-date").val(),
        "panCardNo": employeeDetail_tab.find("#pan-card-no").val(),
        "visaIssuedFrom": employeeDetail_tab.find("#visa-issued-from").val(),
        "contactNo": employeeDetail_tab.find("#contact-no").val(),
        "inductionTrainingDate": employeeDetail_tab.find("#induction-training-date").val(),
        "otFactor": employeeDetail_tab.find("#ot-factor").val(),
        "previousTax": employeeDetail_tab.find("#previous-tax").val(),
        "taxPercentage": employeeDetail_tab.find("#tax-percentage").val(),
        "projectSite": employeeDetail_tab.find("#project-site").val(),
        "weeklyDayOff": employeeDetail_tab.find("#weekly-day-off").val(),
        "policyContactNo": employeeDetail_tab.find("#policy-contact-no").val(),
        "weddingDate": employeeDetail_tab.find("#wedding-date").val(),
        "esiNo": employeeDetail_tab.find("#esi-no").val(),
        "MedClaimAndAcctNo": employeeDetail_tab.find("#med-claim-and-acct-no").val(),
        "address1": employeeDetail_tab.find("#address-1").val(),
        "address2": employeeDetail_tab.find("#address-2").val(),
        "address3": employeeDetail_tab.find("#address-3").val(),
        "city": employeeDetail_tab.find("#city").val(),
        "pinCode": employeeDetail_tab.find("#pin-code").val(),
        "state": employeeDetail_tab.find("#state").val(),
        "country": employeeDetail_tab.find("#country").val()
    };

    $.ajax({
        url: '/Company/AddEmployee',
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ emp_obj: employeeDetail }),
        success: function () {
            //$("#comp_table tbody").append('<tr id=' + id + '><td><a href="Home/Index/' + id + '" class="black-text">' + $("#comp-name_text").val() + '</td>' +
            //                            '<td>' + $("#comp-additions_text").val() + '</td>' +
            //                            '<td>' + $("#comp-deductions_text").val() + '</td>' +
            //                            '<td>' + $("#comp-username_text").val() + '</td>' +
            //                            '<td>' + $("#comp-passwd_text").val() + '</td>' +
            //                            '<td><span class="icons"><a href="#update-comp_modal" onclick="updateCompPop(' + id + ')" class="waves-effect waves-circle btn-flat secondary-content center-align modal-trigger"><i class="material-icons">edit</i></a></td></span>' +
            //                            '<td><span class="icons"><a href="#!" onclick="deleteCompAJAX(' + id + ')" class="waves-effect waves-circle btn-flat secondary-content center-align"><i class="material-icons">delete</i></a></td></span>');

            //document.getElementById(id).scrollIntoView();
            //$("#comp_table tbody tr").last().css({ "animation-name": "rowHighlight", "animation-duration": "2s" });
            //setTimeout(function () {
            //    $("#" + id).removeAttr("style");
            //}, 2000);
            //$('#add-comp_form').trigger("reset");
            //before_search = true;
        }
    });
}