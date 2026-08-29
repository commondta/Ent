var payPeriodDataTable;
var bonusMasterTable;
let cflPayElementsTable;
let cflCallInput;
var formulaMasterDataTable;
var insertMode = false;
var payPeriodCflDataTable;
var paymentsAndDedDataTable;
var salaryDataTable;
var payElementsTable;
var employeeCategoryMasterTable;
var employees;
var payrollProcessTable;
var navCounter;
var payrollProcess;
var payrollProcessLength;
var addMessage = "Added successfully.";
var updateMessage = "Updated successfully."
var deleteMessage = "Deleted successfully."
var updateMode = false;
var selectedEmpId;
var selectedEmp;
var selectedEmpIndex;
var taxFormulaCalcTable;
var taxFormulaCalc;
var taxFormulaCalcLen;
var cflCall;
var CompanyTable;
var rowSelected;

function get_nbsp(n) {
    var nbsps = "";
    for (var i = 0; i < n; i++) {
        nbsps += '&nbsp';
    }
    return nbsps;
}

function getSalaryRows(salaryDetail) {
    var rows = '';
    for (var i = 0; i < salaryDetail.length; i++) {
        rows += '<tr>' +
            '<td>' + salaryDetail[i].id + '</td>' +
            '<td>' + salaryDetail[i].Code + '</td>' +
            '<td>' + salaryDetail[i].Name + '</td>' +
            '<td>' + moment(salaryDetail[i].EffectiveDate).format("DD-MM-YYYY") + '</td>' +
            '<td>' + salaryDetail[i].Type + '</td>' +
            '<td>' + salaryDetail[i].Amount + '</td>' +
            '<td>' + salaryDetail[i].OT + '</td>' +
            '<td>' + salaryDetail[i].Tax + '</td>' +
        '</tr>';
    }
    return rows;
}
function format(d) {
    // `d` is the original data object for the row
    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
            '<h3>Personal Detail</h3>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Payroll Name</td>' +
            '<td>' + d.PayrollName + '</td>' +
            '<td>' + get_nbsp(25) + '</td>' +
            '<td class="variable">Date of Birth</td>' +
            '<td>' + moment(d.DateOfBirth).format("DD-MM-YYYY") + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Employee Number</td>' +
            '<td>' + d.EmployeeNumber + '</td>' +
            '<td></td>' +
            '<td class="variable">Company Start Date</td>' +
            '<td>' + moment(d.CompanyStartDate).format("DD-MM-YYYY") + '</td>' +
            
        '</tr>' +
        '<tr>' +
            '<td class="variable">Salutation / Title</td>' +
            '<td>' + d.SalutationTitle + '</td>' +
            '<td></td>' +
            '<td class="variable">Citizenship (Country)</td>' +
            '<td>' + d.CitizenshipCountry + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Legal First Name</td>' +
            '<td>' + d.LegalFirstName + '</td>' +
            '<td></td>' +
            '<td class="variable">Phone #</td>' +
            '<td>' + d.PhoneNo + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Legal Last Name</td>' +
            '<td>' + d.LegalLastName + '</td>' +
            '<td></td>' +
            '<td class="variable">Mobile #</td>' +
            '<td>' + d.MobileNo + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Marital Status</td>' +
            '<td>' + d.MaritalStatus + '</td>' +
            '<td></td>' +
            '<td class="variable">Email Address</td>' +
            '<td>' + d.EmailAddress + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Gender</td>' +
            '<td>' + d.Gender + '</td>' +
            '<td></td>' +
        '</tr>' +
        '<tr>' +
            '<td><h3>Address Detail</h3></td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Postal Address 1</td>' +
            '<td>' + d.PostalAddress1 + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Postal Address 2</td>' +
            '<td>' + d.PostalAddress2 + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Postal Address 3</td>' +
            '<td>' + d.PostalAddress3 + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Postal Town</td>' +
            '<td>' + d.PostalTown + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Postal / Zip Code</td>' +
            '<td>' + d.PostalZipCode + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td><h3>Bank Details</h3></td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Account Name</td>' +
            '<td>' + d.AccountName + '</td>' +
            '<td></td>' +
            '<td class="variable">Branch Name</td>' +
            '<td>' + d.BranchName + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Account Type</td>' +
            '<td>' + d.AccountType + '</td>' +
            '<td></td>' +
            '<td class="variable">Branch Code</td>' +
            '<td>' + d.BranchCode + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Account Number</td>' +
            '<td>' + d.AccountNumber + '</td>' +
            '<td></td>' +
            '<td class="variable">Bank Postal Address 1</td>' +
            '<td>' + d.BankPostalAddress1 + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Swift Code</td>' +
            '<td>' + d.SwiftCode + '</td>' +
            '<td></td>' +
            '<td class="variable">Bank Postal Address 2</td>' +
            '<td>' + d.BankPostalAddress2 + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">IBAN #</td>' +
            '<td>' + d.IBANno + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Bank Name</td>' +
            '<td>' + d.BankName + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td><h3>Job Detail</h3></td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Days Worked Each Week</td>' +
            '<td>' + d.DaysWorkedEachWeek + '</td>' +
            '<td></td>' +
            '<td class="variable">Payroll Assignment Start Date</td>' +
            '<td>' + moment(d.PayrollAssignmentStartDate).format("DD-MM-YYYY") + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Hours Per Week</td>' +
            '<td>' + d.HoursPerWeek + '</td>' +
            '<td></td>' +
            '<td class="variable">Payroll Assignment End Date</td>' +
            '<td>' + moment(d.PayrollAssignmentEndDate).format("DD-MM-YYYY") + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Cost Center</td>' +
            '<td>' + d.CostCenter + '</td>' +
            '<td></td>' +
            '<td class="variable">Job Title / Position</td>' +
            '<td>' + d.JobTitlePosition + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Department</td>' +
            '<td>' + d.Department + '</td>' +
            '<td></td>' +
            '<td class="variable">Salary Installments</td>' +
            '<td>' + d.SalaryInstallments + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td><h3>Country Detail</h3></td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">National Identity Card #</td>' +
            '<td>' + d.NationalIdentityCardNo + '</td>' +
            '<td></td>' +
            '<td class="variable">National Tax Number</td>' +
            '<td>' + d.NationalTaxNumber + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">Country of Birth</td>' +
            '<td>' + d.CountryOfBirth + '</td>' +
            '<td></td>' +
            '<td class="variable">Employment Contract</td>' +
            '<td>' + d.EmploymentContract + '</td>' +
        '</tr>' +
        '<tr>' +
            '<td><h3>Salary Detail</h3></td>' +
        '</tr>' +
        '<tr>' +
            '<td class="variable">#</td>' +
            '<td class="variable">Code</td>' +
            '<td class="variable">Name</td>' +
            '<td class="variable">Effective Date</td>' +
            '<td class="variable">Type</td>' +
            '<td class="variable">Amount</td>' +
            '<td class="variable">OT</td>' +
            '<td class="variable">Tax</td>' +
        '</tr>' +
        getSalaryRows(d.SalaryDetail) +

    '</table>';
}

$(document).ready(function () {
    var expandIcon = function (data, type, row) {
        if (type == 'display') {
            return '<i class="fa fa-plus-circle" /><span>' + data + '</span>';
        }
        return data;
    }

    $("#table-buttons").hide();
    var empTable = $('#emp-dataTable').DataTable({
        "columns": [
            {
                "className": 'details-control',
                render: expandIcon
            },
            { "data": "JobTitle" },
            { "data": "Gender" },
            { "data": "CompanyStartDate" }
        ]
    });

    if ($("#emp-dataTable").length != 0) {
        $.ajax({
            type: "POST",
            url: "GetEmployees",
            contentType: "application/json",
            success: function (employeesObj) {
                employees = employeesObj;
            }
        });
    }

    if ($("#payroll-process-table").length != 0) {
        $.ajax({
            type: "POST",
            url: "getPayrollProcess",
            contentType: "application/json",
            success: function (data) {
                navCounter = 0;
                if (data[0].length != 0) {
                    payrollProcess = data;
                    PayrollProcessPopulate(payrollProcess[0][navCounter], payrollProcess[1]);
                    payrollProcessLength = payrollProcess[0].length;
                    payrollProcessTable.columns.adjust();
                }
                else {
                    PayrollProcessInsertMode();
                    payrollProcess = [[], []];
                }
            }
        });
    }

    if ($("#tax-formula-calculation-table").length != 0) {
        $.ajax({
            type: "POST",
            url: "GetTaxFormulaCalculation",
            contentType: "application/json",
            success: function (data) {
                navCounter = 0;
                taxFormulaCalc = data;
                TaxFormulaCalcPopulate(data[0]);
                taxFormulaCalcLen = data.length;
            }
        });
    }

    function findEmployee(id) {
        for (var i = 0; i < employees.length; i++) {
            if (employees[i].id == id) {
                return employees[i];
            }
        }
    }

    $('#emp-dataTable tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = empTable.row(tr);

        if (row.child.isShown()) {
            // Close row
            row.child.hide();
            tr.removeClass('shown');

            $(this).children().removeClass('fa-minus-circle');
            $(this).children().addClass('fa-plus-circle');
        }
        else {
            // Open row
            row.child(format(findEmployee(tr.data("id")))).show();
            tr.addClass('shown');
            
            $(this).children().removeClass('fa-plus-circle');
            $(this).children().addClass('fa-minus-circle');
        }
    });

    payPeriodDataTable = $("#pay-period-table").DataTable({
        paging: false,
        searching: false,
        scrollX: true
    });

    $('#pay-period-table tbody').on('click', 'tr', function () {
        toggleButton(this, payPeriodDataTable);
    });

    $("#payroll-setup-dataTable").DataTable({
        scrollX: true,
        paging: false,
        searching: false
    });

    bonusMasterTable = $("#bonus-master-dataTable").DataTable({
        scrollX: true,
        searching: false
    });

    cflPayElementsTable = $("#cfl-pay-elements-dataTable").DataTable({});

    payElementsTable = $("#pay-elements-table").DataTable({});

    $('#emp-dataTable tbody').on('click', 'tr>td:not(:first-child)', function () {
        toggleButton($(this).parent(), empTable);
    });

    $('#pay-elements-table tbody').on('click', 'tr', function () {
        if (notEmpty(this)) {
            toggleButton(this, payElementsTable);
        }
    });

    employeeCategoryMasterTable = $("#employee-category-master-table").DataTable({
        paging: false,
        searching: false,
        info: false
    });

    $('#employee-category-master-table tbody').on('click', 'tr', function () {
        toggleButton(this, employeeCategoryMasterTable);
    });

    $('#company-table tbody').on('click', 'tr', function () {
        if (notEmpty(this)) {
            toggleButton(this, CompanyTable);
        }
    });

    function notEmpty(tr) {
        return $(tr).find(".dataTables_empty").length == 0;
    }

    function toggleButton(row, table) {
        if ($(row).hasClass('selected')) {
            $(row).removeClass('selected');
            $("#table-buttons").hide();
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(row).addClass('selected');
            $("#table-buttons").show();
        }
        rowSelected = $(row);
    }

    formulaMasterDataTable = $('#formula-master-dataTable').DataTable({
        paging: false,
        searching: false,
        info: false,
        ordering: false
    });

    payPeriodCflDataTable = $("#cfl-pay-period-table").DataTable({
        paging: false,
        searching: false,
        info: false
    })

    $('#cfl-pay-elements-dataTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            cflPayElementsTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#cfl-pay-period-table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            payPeriodCflDataTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    paymentsAndDedDataTable = $('#payments-and-deductions-table').DataTable({
        scrollX: true,
        searching: false,
        ordering: false
    });

    $('#addRow-payments-and-deductions').on('click', function () {
        paymentsAndDedDataTable.row.add([
            '',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />',
            '<input type="text" />'
        ]).draw(false);
    });

    $('#add-row-tax-formula-calc').on('click', function () {
        taxFormulaCalcTable.row.add([
            '<input type="text" name="id" readonly />',
            '<input type="number" name="LowerAmount" min="0" />',
            '<input type="number" name="HigherAmount" min="0" />',
            '<input type="number" name="Percentage" min="0" />',
            '<input type="number" name="FixedAmount" min="0" />',
            '<input type="number" name="OtherAmount" min="0" />',
            '<input type="text" name="Remarks" />'
        ]).draw(false);
    });

    salaryDataTable = $('#emp-salary-dataTable').DataTable({
        paging: false,
        searching: false,
        info: false,
        ordering: false,
        "columns": [
            { className: "grey-cell" },
            null,
            { className: "grey-cell" },
            { className: "grey-cell" },
            { className: "grey-cell" },
            null,
            null,
            null
        ]
    });

    $("#add-row-salary-detail").on("click", function () {
        if (!updateMode) {
            salaryDataTable.row.add([
                '<input type="text" name="id" readonly />',
                '<div class="cfl-container" id="add"><input type="text" name="code"><span><i class="fa fa-reorder"></i></span></div>',
                '<input type="text" name="Name" readonly />',
                '<input type="text" name="EffectiveDate" readonly />',
                '<input type="text" name="Type" readonly />',
                '<input type="text" name="Amount" />',
                '<input type="checkbox" name="OT" />',
                '<input type="checkbox" name="Tax" />'
            ]).draw();
        }
        else {
            salaryDataTable.row.add([
                '<input type="text" name="id" readonly />',
                '<div class="cfl-container" id="update"><input type="text" name="code"><span><i class="fa fa-reorder"></i></span></div>',
                '<input type="text" name="Name" readonly />',
                '<input type="text" name="EffectiveDate" readonly />',
                '<input type="text" name="Type" readonly />',
                '<input type="text" name="Amount" />',
                '<input type="checkbox" name="OT" />',
                '<input type="checkbox" name="Tax" />'
            ]).draw();
        }
    });

    $('#emp-leave-dataTable').DataTable({
        scrollX: true,
        paging: false,
        searching: false
    });

    payrollProcessTable = $("#payroll-process-table").DataTable({
        scrollX: true,
        searching: false,
        ordering: false
    });

    taxFormulaCalcTable = $("#tax-formula-calculation-table").DataTable({
        scrollX: true,
        searching: false,
        ordering: false,
        "autoWidth": false
    });

    CompanyTable = $("#company-table").DataTable({
        paging: false,
        searching: false,
        info: false
    });
    
});

$(function () {
    $('#pay-element-code').on('keypress', function (e) {
        if (e.which == 32)
            return false;
    });
});

//function PayrollProcessPopulate(parent, child) {
//    //$("#payroll-process-parent-form").attr("data-id", parent[5].Value);

//    var child = child.filter(function (item) {
//        return item[1].Value == parent[5].Value;
//    });
//    var fixedColumns;
//    $('#payroll-process-table>tbody>tr').slice(0).remove();

//    var parentForm = document.forms['payroll-process-parent-form'];
//    parentForm.elements.EmployeeType.value = parent[0].Value;
//    parentForm.elements.PayPeriod.value = parent[1].Value;
//    parentForm.elements.PayMonth.value = parent[2].Value;
//    parentForm.elements.FromDate.value = moment(parent[3].Value).format("YYYY-MM-DD");
//    parentForm.elements.ToDate.value = moment(parent[4].Value).format("YYYY-MM-DD");
//    parentForm.elements.DocumentNo.value = parent[5].Value;
//    parentForm.elements.DocumentDate.value = moment(parent[6].Value).format("YYYY-MM-DD");
//    parentForm.elements.Status.value = parent[7].Value;

//    for (var i = 0; i < child.length; i++) {
//        fixedColumns = {};
//        child[i].find(function (item) {
//            switch (item.Key) {
//                case "id":
//                    fixedColumns.id = item.Value;
//                    break;
//                case "EmployeeID":
//                    fixedColumns.EmployeeID = item.Value;
//                    break;
//                case "Name":
//                    fixedColumns.Name = item.Value;
//                    break;
//                case "IncomeTax":
//                    fixedColumns.IncomeTax = item.Value;
//                    break;
//                case "TotalDeduction":
//                    fixedColumns.TotalDeduction = item.Value;
//                    break;
//                case "NetSalary":
//                    fixedColumns.NetSalary = item.Value;
//                    break;
//                case "TaxableSalary":
//                    fixedColumns.TaxableSalary = item.Value;
//                    break;
//            }
//        });

//        $("#payroll-process-table>tbody").append('<tr role="row">' +
//                                                    '<td class="sorting_1"><input type="text" name="id" value="' + fixedColumns.id + '" readonly=""></td>' +
//                                                    '<td><input type="text" name="EmployeeID" value="' + fixedColumns.EmployeeID + '"></td>' +
//                                                    '<td><input type="text" name="Name" value="' + fixedColumns.Name + '"></td></tr>');
//        let row = $("#payroll-process-table>tbody>tr").eq(i);
//        $("#payroll-process-table th").each(function (index) {
//            for (var j = 7; j < child[i].length; j++) {
//                if ($(this).text() == child[i][j].Key) {
//                    row.append('<td><input type="text" name="' + child[i][j].Key + '" value="' + child[i][j].Value + '"></td>');
//                }
//            }
//        });

//        row.append('<td><input type="text" name="IncomeTax" value="' + fixedColumns.IncomeTax + '"></td>' +
//                    '<td><input type="text" name="TotalDeduction" value="' + fixedColumns.TotalDeduction + '"></td>' +
//                    '<td><input type="text" name="NetSalary" value="' + fixedColumns.NetSalary + '"></td>' +
//                    '<td><input type="text" name="TaxableSalary" value="' + fixedColumns.TaxableSalary + '"></td>');
//    }
//}

function PayrollProcessPopulate(parent, child) {
    //$("#payroll-process-parent-form").attr("data-id", parent[5].Value);

    var child = child.filter(function (item) {
        return item[1].Value == parent[5].Value;
    });
    var fixedColumns;
    var parentForm = document.forms['payroll-process-parent-form'];

    parentForm.elements.EmployeeType.value = parent[0].Value;
    parentForm.elements.PayPeriod.value = parent[1].Value;
    parentForm.elements.PayMonth.value = parent[2].Value;
    parentForm.elements.FromDate.value = moment(parent[3].Value).format("YYYY-MM-DD");
    parentForm.elements.ToDate.value = moment(parent[4].Value).format("YYYY-MM-DD");
    parentForm.elements.DocumentNo.value = parent[5].Value;
    parentForm.elements.DocumentDate.value = moment(parent[6].Value).format("YYYY-MM-DD");
    parentForm.elements.Status.value = parent[7].Value;

    payrollProcessTable.rows().remove().draw();
    let thead = $("#payroll-process-table th").slice(3, -4);

    for (var i = 0; i < child.length; i++) {
        fixedColumns = {};
        child[i].find(function (item) {
            switch (item.Key) {
                case "id":
                    fixedColumns.id = item.Value;
                    break;
                case "EmployeeID":
                    fixedColumns.EmployeeID = item.Value;
                    break;
                case "Name":
                    fixedColumns.Name = item.Value;
                    break;
                case "IncomeTax":
                    fixedColumns.IncomeTax = item.Value;
                    break;
                case "TotalDeduction":
                    fixedColumns.TotalDeduction = item.Value;
                    break;
                case "NetSalary":
                    fixedColumns.NetSalary = item.Value;
                    break;
                case "TaxableSalary":
                    fixedColumns.TaxableSalary = item.Value;
                    break;
            }
        });

        let row = ['<input type="text" name="id" value="' + fixedColumns.id + '" readonly>',
                    '<input type="text" name="EmployeeID" value="' + fixedColumns.EmployeeID + '">',
                    '<input type="text" name="Name" value="' + fixedColumns.Name + '">'
        ];

        for (var j = 0; j < thead.length; j++) {
            let element = child[i].filter(function (item) { return item.Key == $(thead[j]).attr("data-code"); });
            field = '<input type="text" name="' + element[0].Key + '" value="' + element[0].Value + '">';
            row.push(field);
        }

        row.push('<input type="text" name="IncomeTax" value="' + fixedColumns.IncomeTax + '">');
        row.push('<input type="text" name="TotalDeduction" value="' + fixedColumns.TotalDeduction + '">');
        row.push('<input type="text" name="NetSalary" value="' + fixedColumns.NetSalary + '">');
        row.push('<input type="text" name="TaxableSalary" value="' + fixedColumns.TaxableSalary + '">');

        payrollProcessTable.row.add(row).draw(false);
    }
}

$("#nav-payroll-process>button[title='First']").on("click", function () {
    navCounter = 0;
    PayrollProcessPopulate(payrollProcess[0][navCounter], payrollProcess[1]);
});

$("#nav-payroll-process>button[title='Previous']").on("click", function () {
    if (navCounter > 0) PayrollProcessPopulate(payrollProcess[0][--navCounter], payrollProcess[1]);
});

$("#nav-payroll-process>button[title='Next']").on("click", function () {
    if (navCounter < payrollProcessLength - 1) PayrollProcessPopulate(payrollProcess[0][++navCounter], payrollProcess[1]);
});

$("#nav-payroll-process>button[title='Last']").on("click", function () {
    navCounter = payrollProcessLength - 1;
    PayrollProcessPopulate(payrollProcess[0][navCounter], payrollProcess[1]);
});

$("#nav-payroll-process + button + button + #cancel-btn").on("click", function () {
    $("#cancel-btn").addClass("hidden");
    $("#add-btn").addClass("hidden");
    $("#new-btn").removeClass("hidden");
    $("#delete-btn").removeClass("hidden");
    PayrollProcessViewMode(navCounter);
    payrollProcessTable.columns.adjust();
    insertMode = false;
});

$("#payroll-process-parent-form select").on('change', function () {
    PayrollProcessUpdateMode();
});

$(document).delegate('#payroll-process-child-form input, #payroll-process-parent-form input', 'input', function () {
    PayrollProcessUpdateMode();
});

function PayrollProcessUpdateMode() {
    if (!insertMode) {
        $("#cancel-btn").removeClass("hidden");
        $("#update-btn").removeClass("hidden");
        $("#new-btn").addClass("hidden");
        $("#delete-btn").addClass("hidden");
    }
}

function PayrollProcessViewMode(docIndex) {
    PayrollProcessPopulate(payrollProcess[0][docIndex], payrollProcess[1]);
    $("#update-btn").addClass("hidden");
    $("#nav-payroll-process + button + #delete-btn").removeClass("hidden");
}

function PayrollProcessDelete() {
    $.ajax({
        type: "POST",
        url: "PayrollProcessDelete",
        data: JSON.stringify({ id: payrollProcess[0][navCounter][5].Value }),
        contentType: "application/json",
        success: function (id) {
            Message(deleteMessage);
            payrollProcess[1] = payrollProcess[1].filter(function (item) {
                return item[1].Value != payrollProcess[0][navCounter][5].Value;
            });
            payrollProcess[0] = payrollProcess[0].filter(function (item) {
                return item[5].Value != payrollProcess[0][navCounter][5].Value;
            });
            
            navCounter = 0;
            PayrollProcessPopulate(payrollProcess[0][navCounter], payrollProcess[1]);
            payrollProcessLength = payrollProcess[0].length;
        }
    });
}

function PayrollProcessUpdate() {
    var parentData = formToObject($("#payroll-process-parent-form"));
    var childData = formToDocs($("#payroll-process-child-form"), "NetSalary");
    parentData.id = payrollProcess[0][navCounter][5].Value;

    $.ajax({
        type: "POST",
        url: "PayrollProcessUpdate",
        data: JSON.stringify({ parentData: parentData, childData: childData }),
        contentType: "application/json",
        success: function () {
            payrollProcess[0][navCounter][0].Value = parentData.EmployeeType;
            payrollProcess[0][navCounter][1].Value = parentData.PayPeriod;
            payrollProcess[0][navCounter][2].Value = parentData.PayMonth;
            payrollProcess[0][navCounter][3].Value = parentData.FromDate;
            payrollProcess[0][navCounter][4].Value = parentData.ToDate;
            payrollProcess[0][navCounter][6].Value = parentData.DocumentDate;
            payrollProcess[0][navCounter][7].Value = parentData.Status;

            let rowList = formToDict("payroll-process-child-form", "NetSalary");
            for (var iRowList = 0; iRowList < rowList.length; iRowList++) {
                for (var iPayrollProcess = 0; iPayrollProcess < payrollProcess[1].length; iPayrollProcess++) {
                    if (rowList[iRowList][0].value == payrollProcess[1][iPayrollProcess][0].Value) {
                        rowList[iRowList].forEach(function (itemRowList) {
                            payrollProcess[1][iPayrollProcess].forEach(function (itemPayrollProcess, indexPayrollProcess) {
                                if (itemRowList.name == itemPayrollProcess.Key) {
                                    payrollProcess[1][iPayrollProcess][indexPayrollProcess].Value = itemRowList.value;
                                }
                            });
                        });
                    }
                }
            }
            PayrollProcessPopulate(payrollProcess[0][navCounter], payrollProcess[1]);
            $("#cancel-btn").click();
            Message(updateMessage);
        }
    });
}

function PayPeriodUpdateModal() {
    $('#myModal2').modal('show');
    var payPeriodEditForm = document.forms['pay-period-edit-form'];
    var rowData = $('#pay-period-table tr.selected>td');

    for (var i = 0; i < payPeriodEditForm.elements.length; i++) {
        payPeriodEditForm.elements[i].value = rowData[i + 1].innerHTML;
    }
}

// Opens Choose List
$(document).delegate("#formula-master-dataTable .fa.fa-reorder", "click", function () {
    $('#cflModal').modal('show');
    cflCallInput = $(this).parent().prev();
});

function payElementSelect() {
    var PayElementCode = $("#cfl-pay-elements-dataTable .selected>td:nth-child(2)").text();
    cflCallInput.val(PayElementCode);
}

function PayPeriodSelect() {
    var payPeriodCode = $("#cfl-pay-period-table .selected>td:nth-child(2)").text();
    cflCallInput.val(payPeriodCode);
}

$(document).delegate("#payments-and-deductions-parent-form .fa.fa-reorder", "click", function () {
    $('#cflModal').modal('show');
    cflCallInput = $(this).parent().prev();
});

function PaymentsAndDeductionsSelect() {
    var PayElementCode = $("#cfl-pay-period-table .selected>td:nth-child(2)").text();
    cflCallInput.val(PayElementCode);
}

$("td:has(input[type='text']:not(:read-only))").hover(function () {
    $(this).css("cursor", "text");
});

$("td:has(input[type='text'])").click(function () {
    $(this).children().focus();
});

$(document).delegate("#bonus-master-dataTable tbody tr:last-child td:last-child input", 'focusout', function () {
    bonusMasterTable.row.add([
        1,
        '<input type="text" />',
        '<input type="text" />',
        '<input type="text" />',
        '<input type="text" />'
    ]).draw(false);
});

function formToObject(form) {
    var form_ser = form.serializeArray();
    var object = {};
    for (var i = 0; i < form_ser.length; i++) {
        object[form_ser[i].name] = form_ser[i].value;
    }
    return object;
}

function formToDocs(form, lastFieldName) {
    var rawData = form.serializeArray();
    var docs = [];
    var doc = {};
    for (var i = 0; i < rawData.length; i++) {
        doc[rawData[i].name] = rawData[i].value;
        if (rawData[i].name == lastFieldName) {
            docs.push(doc);
            doc = {};
        }
    }
    return docs;
}

function formToDict(formId, lastFieldName) {
    var rawData = $("#" + formId).serializeArray();
    rawData = rawData.slice(1, rawData.length); // remove unwanted data
    var row = [];
    var netSalaryIndex;
    
    do {
        netSalaryIndex = rawData.findIndex(function (item) { return item.name == lastFieldName });
        row.push(rawData.slice(0, netSalaryIndex + 1));
        rawData = rawData.slice(netSalaryIndex + 1, rawData.length);
    } while (rawData.length != 0);
    
    return row;
}

function payElementAdd() {
    var form = $("#pay-element-form");
    if (presenceCheck(form)) {
        return;
    }
    var data = formToObject(form);

    $.ajax({
        type: "POST",
        url: "PayElements",
        data: JSON.stringify({ obj: data }),
        contentType: "application/json",
        success: function (id) {
            $('#myModal').modal('hide');
            Message(addMessage);
            //$("td").remove();
            //for (let i = 0; i < result.length; i++) {
            //    var tab = $('<tr />');
            //    tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].PayElementCode + "</td>", "<td>" + result[i].Description + "</td>", "<td>" + result[i].Type + "</td>", "<td>" + result[i].PayElementType + "</td>", "<td>" + result[i].Amount + "</td>", "<td>" + moment(result[i].EffectiveDate).format("YYYY-MM-DD") + "</td>", "<td>" + result[i].Taxable + "</td>");
            //    $('#pay-elements-table').append(tab);
            //}

            payElementsTable.row.add([
                id,
                data.PayElementCode,
                data.Description,
                data.Type,
                data.PayElementType,
                data.Amount,
                data.EffectiveDate,
                data.Taxable
            ]).draw(false);
            form.trigger("reset");
        }
    });
}

function presenceCheck(form) {
    emptyElems = "";
    for (var i = 0; i < form[0].length; i++) {
        if (typeof form[0][i].attributes.required === "object" && $(form[0][i]).val() == "") {
            emptyElems += $(form[0][i]).parent().prev().text() + ", ";
        }
    }
    if (emptyElems != "") {
        emptyElems = emptyElems.slice(0, -2);
        Message("Enter " + emptyElems);
    }

    return emptyElems != "";
}

function EmployeeCategoryMasterAdd() {
    var data = formToObject($("#employee-category-master-form"));
    $.ajax({
        type: "POST",
        url: "EmployeeCategoryMaster",
        data: JSON.stringify({ obj: data }),
        contentType: "application/json",
        success: function (id) {
            $('#myModal').modal('hide');
            $('#employee-category-master-table>tbody').append("<tr><td>" + id + "</td>" +
                                                        "<td>" + data.EmployeeCategoryCode + "</td>" +
                                                        "<td>" + data.EmployeeCategoryName + "</td>" +
                                                        "<td>" + data.AccountCode + "</td>" +
                                                        "<td>" + data.Remarks + "</td><tr />");
        }
    });
}

function Message(message) {
    var x = document.getElementById("snackbar");
    x.innerHTML = message;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

function PayPeriodCreate() {
    var data = formToObject($("#pay-period-form"));

    $.ajax({
        type: "POST",
        url: "PayPeriodCreate",
        data: JSON.stringify({ data: data }),
        contentType: "application/json",
        success: function (id) {
            $('#myModal').modal('hide');
            payPeriodDataTable.row.add([
                id,
                data.LocationProjectSite,
                data.PayPeriodCodeMonth,
                data.Name,
                data.FromDate,
                data.ToDate,
                data.PayMonth,
                data.NoOfWorkingDays,
                data.NoOfFridays,
                data.NoOfHolidays,
                data.MaximumNormalOTHoursMonth,
                data.MaximumWorkingHoursMonth,
                data.Remarks
            ]).draw(false);
            Message(addMessage);
        }
    });
}

function PayPeriodUpdate() {
    var data = formToObject($("#pay-period-edit-form"));
    data.id = $("#pay-period-table .selected td")[0].innerHTML;

    $.ajax({
        type: "POST",
        url: "PayPeriodUpdate",
        data: JSON.stringify({ data: data }),
        contentType: "application/json",
        success: function () {
            $("#pay-period-table .selected td")[1].innerHTML = data.LocationProjectSite;
            $("#pay-period-table .selected td")[2].innerHTML = data.PayPeriodCodeMonth;
            $("#pay-period-table .selected td")[3].innerHTML = data.Name;
            $("#pay-period-table .selected td")[4].innerHTML = data.FromDate;
            $("#pay-period-table .selected td")[5].innerHTML = data.ToDate;
            $("#pay-period-table .selected td")[6].innerHTML = data.PayMonth;
            $("#pay-period-table .selected td")[7].innerHTML = data.NoOfWorkingDays;
            $("#pay-period-table .selected td")[8].innerHTML = data.NoOfFridays;
            $("#pay-period-table .selected td")[9].innerHTML = data.NoOfHolidays;
            $("#pay-period-table .selected td")[10].innerHTML = data.MaximumNormalOTHoursMonth;
            $("#pay-period-table .selected td")[11].innerHTML = data.MaximumWorkingHoursMonth;
            $("#pay-period-table .selected td")[12].innerHTML = data.Remarks;
            $('#myModal2').modal('hide');
            Message(updateMessage);
        }
    });
}

function FormulaMasterCreate() {
    var parentData = formToObject($("#formula-master-parent-form"));
    var childData = formToDocs($("#formula-master-child-form"), "Remarks");
    parentData.id = $('#formula-master-dataTable>tbody').attr('id');

    $.ajax({
        type: "POST",
        url: "FormulaMasterCreate",
        data: JSON.stringify({ parentData: parentData, childData: childData }),
        contentType: "application/json",
        success: function (childIds) {
            Message(addMessage);

            for (var i = 0; i < childIds.length; i++) {
                childData[i].id = childIds[(childIds.length - 1) - i].id;
                childData[i].ParentID = childIds[(childIds.length - 1) - i].ParentID;
            }
            parentData.id = childData[0].ParentID;
            setData(childData, parentData);
        }
    });
}

function FormulaMasterUpdate() {
    var parentData = formToObject($("#formula-master-parent-form"));
    var childData = formToDocs($("#formula-master-child-form"), "Remarks");
    parentData.id = $('#formula-master-dataTable>tbody').attr('id');

    $.ajax({
        type: "POST",
        url: "FormulaMasterUpdate",
        data: JSON.stringify({ parentData: parentData, childData: childData }),
        contentType: "application/json",
        success: function (id) {
            var x = document.getElementById("snackbar");
            x.innerHTML = "Successfully updated!";
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
            updateData(childData, parentData);
        }
    });
}

function InsertMode(parentForm, childForm, table) {
    insertMode = true;
    document.getElementById(parentForm).reset();
    document.getElementById(childForm).reset();
    $('#' + table + '>tbody>tr').slice(1).remove();
    $("#new-btn").addClass("hidden");
    $("#add-btn").removeClass("hidden");
    $("#cancel-btn").removeClass("hidden");
    $("#import-btn").removeClass("hidden");
}

function UpdateMode() {
    if (!insertMode) {
        $("#update-btn").removeClass("hidden");
        $("#cancel-btn").removeClass("hidden");
        $("#new-btn").addClass("hidden");
    }
}

function PayrollProcessInsertMode() {
    let table = 'payroll-process-table';
    InsertMode('payroll-process-parent-form', 'payroll-process-child-form', table);
    payrollProcessTable.rows().draw();
    $("#calculate-pay-btn").removeClass("disabled");
    $("#nav-payroll-process + button + #delete-btn").addClass("hidden");
    insertMode = true;
}

$("#formula-master-parent-form select").on('change', function () {
    UpdateMode();
});

$(document).delegate('#formula-master-child-form input', 'input', function () {
    UpdateMode();
});

$('#cancel-btn').on('click', function () {
    insertMode = false;
});

function UploadfilePayAndDed() {
    var file = $("#file").get(0).files;
    var data = new FormData;
    data.append("File", file[0]);
    var parentData = formToObject($("#payments-and-deductions-parent-form"));

    $.ajax({
        type: "POST",
        url: "UploadFile?PayPeriod=" + parentData.PayPeriod + "&DocumentDate=" + parentData.DocumentDate + "&Status=" + parentData.Status,
        data: data,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#myModal').modal('hide');
            Message(updateMessage);
        }
    });
}

$(document).delegate("#salary-details-form .cfl-container#add .fa.fa-reorder", "click", function () {
    $('#empModal').modal("hide");
    $('#cflModal').modal("show");
    cflCallInput = $(this).parent().prev();
    cflCall = true;
    $("body").addClass("modal-open");
});

$(document).delegate("#salary-details-form .cfl-container#update .fa.fa-reorder", "click", function () {
    $('#empModal').modal("hide");
    $('#cflModal').modal("show");
    $('#empModal .modal-title').text("Update Employee");
    $('#empModal #add-footer').addClass("hidden");
    $('#empModal #update-footer').removeClass("hidden");
    updateMode = true;
    cflCallInput = $(this).parent().prev();
    cflCall = true;
});

function EmployeesSelect() {
    var PayElementCode = $("#cfl-pay-elements-dataTable .selected>td:nth-child(2)").text();
    var Description = $("#cfl-pay-elements-dataTable .selected>td:nth-child(3)").text();
    var Type = $("#cfl-pay-elements-dataTable .selected>td:nth-child(4)").text();
    var EffectiveDate = $("#cfl-pay-elements-dataTable .selected>td:nth-child(5)").text();
    cflCallInput.val(PayElementCode);
    cflCallInput.parent().parent().parent().children().children('input[name="Name"]').val(Description);
    cflCallInput.parent().parent().parent().children().children('input[name="Type"]').val(Type);
    cflCallInput.parent().parent().parent().children().children('input[name="EffectiveDate"]').val(EffectiveDate);
    cflCallInput.parent().parent().parent().children().children('input[name="OT"]').val("false");
    cflCallInput.parent().parent().parent().children().children('input[name="Tax"]').val("false");
    var formulaMasterName = PayElementCode + " - " + Description;

    $.ajax({
        type: "POST",
        url: "EmployeesGetFormula",
        data: JSON.stringify({ formulaMasterName: formulaMasterName }),
        contentType: "application/json",
        success: function (formulas) {
            //var rows = cflCallInput.parent().parent().parent().parent().nextAll('*:lt(' + formulas.length + ')');
            var monthlySalary = cflCallInput.parent().parent().parent().children().children('input[name="Amount"]').val();

            if (!updateMode) {
                for (var i = 0; i < formulas.length; i++) {
                    salaryDataTable.row.add([
                        '<input type="text" name="id" readonly />',
                        '<div class="cfl-container" id="add"><input type="text" name="code" value="' + formulas[i].PayCode + '"><span><i class="fa fa-reorder"></i></span></div>',
                        '<input type="text" name="Name" readonly />',
                        '<input type="text" name="EffectiveDate" readonly />',
                        '<input type="text" name="Type" readonly />',
                        '<input type="text" name="Amount" value="' + Math.round(formulas[i].Percentages / 100 * monthlySalary) + '" />',
                        '<input type="checkbox" name="OT" />',
                        '<input type="checkbox" name="Tax" />'
                    ]).draw();
                }
            }
            else {
                for (var i = 0; i < formulas.length; i++) {
                    salaryDataTable.row.add([
                        '<input type="text" name="id" readonly />',
                        '<div class="cfl-container" id="update"><input type="text" name="code" value="' + formulas[i].PayCode + '"><span><i class="fa fa-reorder"></i></span></div>',
                        '<input type="text" name="Name" readonly />',
                        '<input type="text" name="EffectiveDate" readonly />',
                        '<input type="text" name="Type" readonly />',
                        '<input type="text" name="Amount" value="' + Math.round(formulas[i].Percentages / 100 * monthlySalary) + '" />',
                        '<input type="checkbox" name="OT" />',
                        '<input type="checkbox" name="Tax" />'
                    ]).draw();
                }
            }

            //for (var i = 0; i < formulas.length; i++) {
            //    rows.eq(i).find("input[name='code']").val(formulas[i].PayCode);
            //    rows.eq(i).find("input[name='Amount']").val(Math.round(formulas[i].Percentages / 100 * monthlySalary));
            //    rows.eq(i).find("input[name='OT']").val("false");
            //    rows.eq(i).find("input[name='Tax']").val("false");
            //}
        }
    });
    if (cflCallInput.parent().parent().attr("id") == "emp-add-cfl") {
        empNewModelMode();
    }
    $('#empModal').modal("show");
}

function EmployeesUpdateModal() {
    updateMode = true;
    
    selectedEmpId = $('#emp-dataTable tr.selected').attr("data-id");
    
    selectedEmp = employees.find(function (item, index) {
        selectedEmpIndex = index;
        return item.id == selectedEmpId;
    });
    $('#empModal').modal('show');
    $('#empModal .modal-title').text("Update Employee");
    $('#empModal #add-footer').addClass("hidden");
    $('#empModal #update-footer').removeClass("hidden");
    $('#emp-add-cfl').addClass("hidden");
    $('#emp-update-cfl').removeClass("hidden");

    var empDetailsArray = $("#employee-details-form").serializeArray();

    $("[name='PayrollName']").val(selectedEmp.PayrollName);
    $("[name='EmployeeNumber']").val(selectedEmp.EmployeeNumber);
    $("[name='SalutationTitle']").val(selectedEmp.SalutationTitle);
    $("[name='LegalFirstName']").val(selectedEmp.LegalFirstName);
    $("[name='LegalLastName']").val(selectedEmp.LegalLastName);
    $("[name='MaritalStatus']").val(selectedEmp.MaritalStatus);
    $("[Name='Gender'][value='" + selectedEmp.Gender + "']").prop("checked", true);
    $("[name='DateOfBirth']").val(moment(selectedEmp.DateOfBirth).format("YYYY-MM-DD"));
    $("[name='CompanyStartDate']").val(moment(selectedEmp.CompanyStartDate).format("YYYY-MM-DD"));
    $("[name='CitizenshipCountry']").val(selectedEmp.CitizenshipCountry);
    $("[name='PhoneNo']").val(selectedEmp.PhoneNo);
    $("[name='MobileNo']").val(selectedEmp.MobileNo);
    $("[name='EmailAddress']").val(selectedEmp.EmailAddress);
    $("[name='PostalAddress1']").val(selectedEmp.PostalAddress1);
    $("[name='PostalAddress2']").val(selectedEmp.PostalAddress2);
    $("[name='PostalAddress3']").val(selectedEmp.PostalAddress3);
    $("[name='PostalTown']").val(selectedEmp.PostalTown);
    $("[name='PostalZipCode']").val(selectedEmp.PostalZipCode);
    $("[name='AccountName']").val(selectedEmp.AccountName);
    $("[name='AccountType']").val(selectedEmp.AccountType);
    $("[name='AccountNumber']").val(selectedEmp.AccountNumber);
    $("[name='SwiftCode']").val(selectedEmp.SwiftCode);
    $("[name='IBANno']").val(selectedEmp.IBANno);
    $("[name='BankName']").val(selectedEmp.BankName);
    $("[name='BranchName']").val(selectedEmp.BranchName);
    $("[name='BranchCode']").val(selectedEmp.BranchCode);
    $("[name='BankPostalAddress1']").val(selectedEmp.BankPostalAddress1);
    $("[name='BankPostalAddress2']").val(selectedEmp.BankPostalAddress2);
    $("[name='AnnualSalary']").val(selectedEmp.AnnualSalary);
    $("[name='DaysWorkedEachWeek']").val(selectedEmp.DaysWorkedEachWeek);
    $("[name='HoursPerWeek']").val(selectedEmp.HoursPerWeek);
    $("[name='CostCenter']").val(selectedEmp.CostCenter);
    $("[name='Department']").val(selectedEmp.Department);
    $("[name='PayrollAssignmentStartDate']").val(moment(selectedEmp.PayrollAssignmentStartDate).format("YYYY-MM-DD"));
    $("[name='PayrollAssignmentEndDate']").val(moment(selectedEmp.PayrollAssignmentEndDate).format("YYYY-MM-DD"));
    $("[name='JobTitlePosition']").val(selectedEmp.JobTitlePosition);
    $("[name='SalaryInstallments'][value='" + selectedEmp.SalaryInstallments + "']").prop("checked", true);
    $("[name='NationalIdentityCardNo']").val(selectedEmp.NationalIdentityCardNo);
    $("[name='CountryOfBirth']").val(selectedEmp.CountryOfBirth);
    $("[name='NationalTaxNumber']").val(selectedEmp.NationalTaxNumber);
    $("[name='EmploymentContract']").val(selectedEmp.EmploymentContract);

    $("input[name='id']").val(selectedEmp.SalaryDetail[0].id);
    $("input[name='code']").val(selectedEmp.SalaryDetail[0].Code);
    $("input[name='Name']").val(selectedEmp.SalaryDetail[0].Name);
    $("input[name='EffectiveDate']").val(moment(selectedEmp.SalaryDetail[0].EffectiveDate).format("YYYY-MM-DD"));
    $("input[name='Type']").val(selectedEmp.SalaryDetail[0].Type);
    $("input[name='Amount']").val(selectedEmp.SalaryDetail[0].Amount);
    $("input[name='OT']").prop("checked", selectedEmp.SalaryDetail[0].OT);
    $("input[name='Tax']").prop("checked", selectedEmp.SalaryDetail[0].Tax);
    for (var i = 1; i < selectedEmp.SalaryDetail.length; i++) {
        salaryDataTable.row.add([
            '<input type="text" name="id" value="' + selectedEmp.SalaryDetail[i].id + '" readonly />',
            '<div class="cfl-container"><input type="text" name="code" value="' + selectedEmp.SalaryDetail[i].Code + '"><span><i class="fa fa-reorder"></i></span></div>',
            '<input type="text" name="Name" value="' + selectedEmp.SalaryDetail[i].Name + '" readonly />',
            '<input type="text" name="EffectiveDate" value="' + moment(selectedEmp.SalaryDetail[i].EffectiveDate).format("YYYY-MM-DD") + '" readonly />',
            '<input type="text" name="Type" value="' + selectedEmp.SalaryDetail[i].Type + '" readonly />',
            '<input type="text" name="Amount" value="' + selectedEmp.SalaryDetail[i].Amount + '" />',
            '<input type="checkbox" name="OT" ' + isChecked(selectedEmp.SalaryDetail[i].OT) + ' />',
            '<input type="checkbox" name="Tax" ' + isChecked(selectedEmp.SalaryDetail[i].Tax) + ' />'
        ]).draw();
    }

    $("#salary-details-form .cfl-container").attr("id", "update");
}

function isChecked(value) {
    if (value)
        return "checked";
    else
        return "";
}

$('#empModal').on("hidden.bs.modal", function () {
    if (!cflCall) {
        empNewModelMode();
        salaryDataTable.rows(":lt(-1)").remove().draw();
    }
    cflCall = false;
})

function empNewModelMode() {
    $('#empModal .modal-title').text("New Employee");
    $('#empModal #add-footer').removeClass("hidden");
    $('#empModal #update-footer').addClass("hidden");
    updateMode = false;
}

$("#new-btn").on("click", function () {
    $('#empModal').modal('show');
    $('#employee-details-form').trigger("reset");
    $('#salary-details-form').trigger("reset");
    //$("#emp-add-cfl").removeClass("hidden");
    //$("#emp-update-cfl").addClass("hidden");
    $("a[href='#tab_emp-details']").click();
    $(".form-group.has-error").removeClass("has-error");
    $("#salary-details-form input[type='text']").val("");
    $("#salary-details-form .cfl-container").attr("id", "add");
});

$("#cflModal[aria-hidden='true']").on('hide.bs.modal', function () {
    $('#empModal').modal("show");
});

$("#cflModal[aria-hidden='true']").on('shown.bs.modal', function () {
    $('body').addClass('modal-open');
});

$("#cflModal[aria-hidden='true']").on('hidden.bs.modal', function () {
    $('body').removeClass('modal-open');
});

$("#empModal").on('shown.bs.modal', function () {
    $('body').addClass('modal-open');
});

$("#empModal").on('hidden.bs.modal', function () {
    $('body').removeClass('modal-open');
});

$('td.details-control').on('click', function () {
    $(this).child('.fa-plus-circle:before').css('content', '\f056');
})

function EmployeeCreate() {
    var employeeDetail = formToObject($("#employee-details-form"));
    var salaryDetail = formToDocs($("#salary-details-form"), "Amount");

    salaryDetail = salaryDetail.filter(function (item) {
        return item.code != "";
    });

    let cb_SalaryDetail = $("#salary-details-form input[type='checkbox']");
    let i = 0;
    salaryDetail.forEach(function (item, index) {
        item.OT = cb_SalaryDetail[i++].checked;
        item.Tax = cb_SalaryDetail[i++].checked;
    });

    $.ajax({
        type: "POST",
        url: "EmployeesCreate",
        data: JSON.stringify({ employeeDetail: employeeDetail, salaryDetail: salaryDetail }),
        contentType: "application/json",
        success: function (childIds) {
            $('#empModal').modal('hide');
            Message(addMessage);
        }
    });
}

function EmployeeUpdate() {
    var employeeDetail = formToObject($("#employee-details-form"));
    var salaryDetail = formToDocs($("#salary-details-form"), "Amount");
    employeeDetail.id = selectedEmpId;

    salaryDetail = salaryDetail.filter(function (item) {
        return item.code != "";
    });
    
    let cb_SalaryDetail = $("#salary-details-form input[type='checkbox']");
    let i = 0;
    salaryDetail.forEach(function (item, index) {
        item.OT = cb_SalaryDetail[i++].checked;
        item.Tax = cb_SalaryDetail[i++].checked;
    });

    $.ajax({
        type: "POST",
        url: "EmployeesUpdate",
        data: JSON.stringify({ employeeDetail: employeeDetail, salaryDetail: salaryDetail }),
        contentType: "application/json",
        success: function (childIds) {
            $('#empModal').modal('hide');
            let i = selectedEmpIndex;

            employees[i].PayrollName = employeeDetail.PayrollName;
            employees[i].EmployeeNumber = employeeDetail.EmployeeNumber;
            employees[i].SalutationTitle = employeeDetail.SalutationTitle;
            employees[i].LegalFirstName = employeeDetail.LegalFirstName;
            employees[i].LegalLastName = employeeDetail.LegalLastName;
            employees[i].MaritalStatus = employeeDetail.MaritalStatus;
            employees[i].Gender = employeeDetail.Gender;
            employees[i].DateOfBirth = employeeDetail.DateOfBirth;
            employees[i].CompanyStartDate = employeeDetail.CompanyStartDate;
            employees[i].CitizenshipCountry = employeeDetail.CitizenshipCountry;
            employees[i].PhoneNo = employeeDetail.PhoneNo;
            employees[i].MobileNo = employeeDetail.MobileNo;
            employees[i].EmailAddress = employeeDetail.EmailAddress;
            employees[i].PostalAddress1 = employeeDetail.PostalAddress1;
            employees[i].PostalAddress2 = employeeDetail.PostalAddress2;
            employees[i].PostalAddress3 = employeeDetail.PostalAddress3;
            employees[i].PostalTown = employeeDetail.PostalTown;
            employees[i].PostalZipCode = employeeDetail.PostalZipCode;
            employees[i].AccountName = employeeDetail.AccountName;
            employees[i].AccountType = employeeDetail.AccountType;
            employees[i].AccountNumber = employeeDetail.AccountNumber;
            employees[i].SwiftCode = employeeDetail.SwiftCode;
            employees[i].IBANno = employeeDetail.IBANno;
            employees[i].BankName = employeeDetail.BankName;
            employees[i].BranchName = employeeDetail.BranchName;
            employees[i].BranchCode = employeeDetail.BranchCode;
            employees[i].BankPostalAddress1 = employeeDetail.BankPostalAddress1;
            employees[i].BankPostalAddress2 = employeeDetail.BankPostalAddress2;
            employees[i].DaysWorkedEachWeek = employeeDetail.DaysWorkedEachWeek;
            employees[i].HoursPerWeek = employeeDetail.HoursPerWeek;
            employees[i].CostCenter = employeeDetail.CostCenter;
            employees[i].Department = employeeDetail.Department;
            employees[i].PayrollAssignmentStartDate = employeeDetail.PayrollAssignmentStartDate;
            employees[i].PayrollAssignmentEndDate = employeeDetail.PayrollAssignmentEndDate;
            employees[i].JobTitlePosition = employeeDetail.JobTitlePosition;
            employees[i].SalaryInstallments = employeeDetail.SalaryInstallments;
            employees[i].NationalIdentityCardNo = employeeDetail.NationalIdentityCardNo;
            employees[i].CountryOfBirth = employeeDetail.CountryOfBirth;
            employees[i].NationalTaxNumber = employeeDetail.NationalTaxNumber;
            
            $('#emp-dataTable tr.selected td:nth-child(2)').text(employeeDetail.JobTitlePosition);
            $('#emp-dataTable tr.selected td:nth-child(3)').text(employeeDetail.Gender);
            $('#emp-dataTable tr.selected td:nth-child(4)').text(employeeDetail.CompanyStartDate);

            Message(updateMessage);
        }
    });
}

function TableDelete(table, controllerMethod) {
    var row = $("#" + table + ' tr.selected');

    $.ajax({
        type: "POST",
        url: controllerMethod,
        data: JSON.stringify({ id: row.attr("data-id") }),
        contentType: "application/json",
        success: function () {
            row.remove();
            $("#table-buttons").slideUp(500);
            Message(deleteMessage);
        }
    });
}

function PayPeriodPayProcSelect() {
    if ($("#payroll-process-parent-form input[name='PayPeriod']").val() != $("#cfl-pay-period-table .selected>td:nth-child(2)").text()) {
        PayrollProcessUpdateMode();
    }
    $("#payroll-process-parent-form input[name='PayPeriod']").val($("#cfl-pay-period-table .selected>td:nth-child(2)").text());
    $("#payroll-process-parent-form input[name='PayMonth']").val($("#cfl-pay-period-table .selected>td:nth-child(3)").text());
    $("#payroll-process-parent-form input[name='FromDate']").val($("#cfl-pay-period-table .selected>td:nth-child(4)").text());
    $("#payroll-process-parent-form input[name='ToDate']").val($("#cfl-pay-period-table .selected>td:nth-child(5)").text());
}

function PayElementsDelete() {
    var row = rowSelected;
    var payElement = {
        id: row.children().eq(0).text(),
        description: row.children().eq(2).text()
    }
    $.ajax({
        type: "POST",
        url: "PayElementsDelete",
        data: JSON.stringify({ payElement }),
        contentType: "application/json",
        success: function () {
            row.remove();
            $("#table-buttons").slideUp(500);
            Message(deleteMessage);
        }
    });
}

function PayPeriodDelete() {
    DeleteTableRow(payPeriodDataTable, "PayPeriodDelete");
}

function EmpCategMasterDelete() {
    DeleteTableRow(employeeCategoryMasterTable, "employeeCategoryMasterDelete");
}

function DeleteTableRow(table, controllerMethod) {
    var row = table.$('tr.selected');
    $.ajax({
        type: "POST",
        url: controllerMethod,
        data: JSON.stringify({ id: row.children().eq(0).text() }),
        contentType: "application/json",
        success: function () {
            row.remove();
            Message(deleteMessage);
        }
    });
}

function PayrollProcessCreate(parentForm, childForm, childLastField, controllerMethod) {
    var parentData = formToObject($("#" + parentForm));
    var childData = formToDocs($("#" + childForm), childLastField);
    //parentData.id = $('#formula-master-dataTable>tbody').attr('id');

    $.ajax({
        type: "POST",
        url: controllerMethod,
        data: JSON.stringify({ parentData: parentData, childData: childData }),
        contentType: "application/json",
        success: function (childIds) {
            Message(addMessage);
            
            let newEntryParent = [
                { Key: "EmployeeType", Value: parentData.EmployeeType },
                { Key: "PayPeriod", Value: parentData.PayPeriod },
                { Key: "PayMonth", Value: parentData.PayMonth },
                { Key: "FromDate", Value: parentData.FromDate },
                { Key: "ToDate", Value: parentData.ToDate },
                { Key: "DocumentNo", Value: childIds[0].ParentID },
                { Key: "DocumentDate", Value: parentData.DocumentDate },
                { Key: "Status", Value: parentData.Status }];
            payrollProcess[0].push(newEntryParent);

            var rowIndex = 0;
            var constFieldsIndex = 2;
            var row = [];
            
            row.push({ Key: "id", Value: (childIds[rowIndex].id).toString() },
                     { Key: "ParentID", Value: (childIds[rowIndex++].ParentID).toString() });

            for (var i = 0, field = {}, rawData = $("#" + childForm).serializeArray(); i < rawData.length; i++) {
                
                if (!(rawData[i].name == "payroll-process-table_length" || rawData[i].name == "id")) {
                    field.Key = rawData[i].name;
                    field.Value = rawData[i].value;
                    if (rawData[i].name == "EmployeeID" || rawData[i].name == "Name" || rawData[i].name == "IncomeTax" || rawData[i].name == "TotalDeduction" || rawData[i].name == "NetSalary" || rawData[i].name == "TaxableSalary") {
                        row.splice(constFieldsIndex++, 0, field);
                    }
                    else {
                        row.push(field);
                    }
                }
                if (rawData[i].name == childLastField) {
                    payrollProcess[1].push(row);
                    row = [];
                    if (i < rawData.length - 1) {
                        row.push({ Key: "id", Value: (childIds[rowIndex].id).toString() });
                        row.push({ Key: "ParentID", Value: (childIds[rowIndex++].ParentID).toString() });
                        constFieldsIndex = 2;
                    }
                }
                field = {};
            }

            payrollProcessLength = payrollProcess[0].length;
            PayrollProcessViewMode(payrollProcessLength - 1);
            $("#nav-payroll-process + button + button + #cancel-btn").addClass("hidden");
            $("#nav-payroll-process + #new-btn").removeClass("hidden");
            $("#nav-payroll-process + button + button + button + button + #add-btn").addClass("hidden");

        }
    });
}

function getElementsTd(data) {
    let tds = '';
    for (var i = 3; i < data.length; i++) {
        tds += '<td><input type="text" name="' + data[i].Key + '" value="' + data[i].Value + '"></td>';
    }
    return tds;
}

function CalculatePay() {
    $.ajax({
        type: "POST",
        url: "CalculatePayPayrollProcess",
        contentType: "application/json",
        data: JSON.stringify({ fromDate: $("#from-date").val(), toDate: $("#to-date").val() }),
        success: function (data) {
            payrollProcessTable.rows().draw();

            let field;
            let thead = $("#payroll-process-table th").slice(3, -4);

            for (var i = 0; i < data.length; i++) {
                let row = [
                    '<input type="text" name="id" value="" readonly>',
                    '<input type="text" name="EmployeeID" value="' + data[i][0].Value + '">',
                    '<input type="text" name="Name" value="' + data[i][1].Value + '">'
                ];

                for (var j = 0; j < thead.length; j++) {
                    let element = data[i].filter(function (item) { return item.Key == $(thead[j]).text(); });
                    if (typeof element[0] != "undefined") {
                        field = '<input type="text" name="' + $(thead[j]).attr("data-code") + '" value="' + element[0].Value + '">';
                    }
                    else {
                        field = '<input type="text" name="' + $(thead[j]).attr("data-code") + '" value="0">';
                    }
                    row.push(field);
                }

                row.push('<input type="text" name="IncomeTax" value="' + data[i][data[i].length - 4].Value + '">');
                row.push('<input type="text" name="TotalDeduction" value="' + data[i][data[i].length - 3].Value + '">');
                row.push('<input type="text" name="NetSalary" value="' + data[i][data[i].length - 2].Value + '">');
                row.push('<input type="text" name="TaxableSalary" value="' + data[i][data[i].length - 1].Value + '">');

                payrollProcessTable.row.add(row).draw(false);
            }



            $("#calculate-pay-btn").addClass("disabled");
        }
    });
}

function PayrollProcessAddMode() {
    $("#new-btn").addClass("hidden");
    $("#delete-btn").addClass("hidden");
    $("#cancel-btn").removeClass("hidden");
    $("#add-btn").removeClass("hidden");

    insertMode = true;
}

function PayrollProcessCancel() {
    $("#cancel-btn").addClass("hidden");
    $("#add-btn").addClass("hidden");
    $("#new-btn").removeClass("hidden");
    $("#delete-btn").removeClass("hidden");
}

function TaxFormulaCalcCreate() {
    var parentData = formToObject($("#tax-formula-calc-parent-form"));
    var childData = formToDocs($("#tax-formula-calc-child-form"), "Remarks");

    $.ajax({
        type: "POST",
        url: "TaxFormulaCalcCreate",
        data: JSON.stringify({ parentData: parentData, childData: childData }),
        contentType: "application/json",
        success: function (childIds) {
            Message(addMessage);

            let newChildList = [];
            let newChildEntry;
            for (var i = 0; i < childData.length; i++) {
                newChildEntry = {};
                newChildEntry.id = childIds[i].id;
                newChildEntry.ParentId = childIds[i].ParentId;
                newChildEntry.LowerAmount = childData[i].LowerAmount;
                newChildEntry.HigherAmount = childData[i].HigherAmount;
                newChildEntry.Percentage = childData[i].Percentage;
                newChildEntry.FixedAmount = childData[i].FixedAmount;
                newChildEntry.OtherAmount = childData[i].OtherAmount;
                newChildEntry.Remarks = childData[i].Remarks;

                newChildList.push(newChildEntry);
            }

            let newEntry = {
                FromDate: parentData.FromDate,
                ToDate: parentData.ToDate,
                Code: childIds[0].ParentId,
                DocumentDate: parentData.DocumentDate,
                Child: newChildList
            };

            taxFormulaCalc.push(newEntry);
            taxFormulaCalcLen = taxFormulaCalc.length;
            $("#cancel-btn").addClass("hidden");
            $("#add-btn").addClass("hidden");
            $("#new-btn").removeClass("hidden");
            
            TaxFormulaCalcPopulate(taxFormulaCalc[taxFormulaCalcLen - 1]);
        }
    });
}

function TaxFormulaCalcUpdate() {
    var parentData = formToObject($("#tax-formula-calc-parent-form"));
    var childData = formToDocs($("#tax-formula-calc-child-form"), "Remarks");
    parentData.Code = taxFormulaCalc[navCounter].Code;

    $.ajax({
        type: "POST",
        url: "TaxFormulaCalcUpdate",
        data: JSON.stringify({ parent: parentData, child: childData }),
        contentType: "application/json",
        success: function () {
            Message(updateMessage);

            //let newChildList = [];
            //let newChildEntry;
            //for (var i = 0; i < childData.length; i++) {
            //    newChildEntry = {};
            //    newChildEntry.id = childIds[i].id;
            //    newChildEntry.ParentId = childIds[i].ParentId;
            //    newChildEntry.LowerAmount = childData[i].LowerAmount;
            //    newChildEntry.HigherAmount = childData[i].HigherAmount;
            //    newChildEntry.Percentage = childData[i].Percentage;
            //    newChildEntry.FixedAmount = childData[i].FixedAmount;
            //    newChildEntry.OtherAmount = childData[i].OtherAmount;
            //    newChildEntry.Remarks = childData[i].Remarks;

            //    newChildList.push(newChildEntry);
            //}

            //let newEntry = {
            //    FromDate: parentData.FromDate,
            //    ToDate: parentData.ToDate,
            //    Code: childIds[0].ParentId,
            //    DocumentDate: parentData.DocumentDate,
            //    Child: newChildList
            //};

            //taxFormulaCalc.push(newEntry);
            //taxFormulaCalcLen = taxFormulaCalc.length;


            taxFormulaCalc[navCounter].FromDate = parentData.FromDate;
            taxFormulaCalc[navCounter].ToDate = parentData.ToDate;
            taxFormulaCalc[navCounter].DocumentDate = parentData.DocumentDate;
            taxFormulaCalc[navCounter].Child = childData;

            $("#cancel-btn").addClass("hidden");
            $("#add-btn").addClass("hidden");
            $("#update-btn").addClass("hidden");
            $("#new-btn").removeClass("hidden");

            TaxFormulaCalcPopulate(taxFormulaCalc[navCounter]);
        }
    });
}

function TaxFormulaCalcPopulate(data) {
    taxFormulaCalcTable.clear().draw();

    var parentForm = document.forms['tax-formula-calc-parent-form'];
    parentForm.elements.FromDate.value = moment(data.FromDate).format("YYYY-MM-DD");;
    parentForm.elements.ToDate.value = moment(data.ToDate).format("YYYY-MM-DD");;
    parentForm.elements.Code.value = data.Code;
    parentForm.elements.DocumentDate.value = moment(data.DocumentDate).format("YYYY-MM-DD");

    for (var i = 0; i < data.Child.length; i++) {
        taxFormulaCalcAddRow(data.Child[i]);
    }
}

$("#nav-tax-formula-calc>button[title='First']").on("click", function () {
    navCounter = 0;
    TaxFormulaCalcPopulate(taxFormulaCalc[navCounter]);
});

$("#nav-tax-formula-calc>button[title='Previous']").on("click", function () {
    if (navCounter > 0) TaxFormulaCalcPopulate(taxFormulaCalc[--navCounter]);
});

$("#nav-tax-formula-calc>button[title='Next']").on("click", function () {
    if (navCounter < taxFormulaCalcLen - 1) TaxFormulaCalcPopulate(taxFormulaCalc[++navCounter]);
});

$("#nav-tax-formula-calc>button[title='Last']").on("click", function () {
    navCounter = taxFormulaCalcLen - 1;
    TaxFormulaCalcPopulate(taxFormulaCalc[navCounter]);
});

$(document).delegate('#tax-formula-calc-child-form, #tax-formula-calc-parent-form input', 'input', function () {
    if (!insertMode) {
        $("#new-btn").addClass("hidden");
        $("#cancel-btn").removeClass("hidden");
        $("#update-btn").removeClass("hidden");
    }
});



function TaxFormulaCalcCancel() {
    $("#cancel-btn").addClass("hidden");
    $("#add-btn").addClass("hidden");
    $("#update-btn").addClass("hidden");
    $("#new-btn").removeClass("hidden");
    TaxFormulaCalcPopulate(taxFormulaCalc[navCounter]);
    insertMode = false;
}

function TaxFormulaCalcUpdateMode() {
    
}

function TaxFormulaCalcAddMode() {
    $("#new-btn").addClass("hidden");
    $("#cancel-btn").removeClass("hidden");
    $("#add-btn").removeClass("hidden");
    $("#tax-formula-calc-parent-form").trigger("reset");
    taxFormulaCalcTable.clear().draw();
    taxFormulaCalcTable.row.add([
            '<input type="text" name="id" readonly />',
            '<input type="number" name="LowerAmount" min="0" />',
            '<input type="number" name="HigherAmount" min="0" />',
            '<input type="number" name="Percentage" min="0" />',
            '<input type="number" name="FixedAmount" min="0" />',
            '<input type="number" name="OtherAmount" min="0" />',
            '<input type="text" name="Remarks" />'
    ]).draw(false);
    insertMode = true;
}

function taxFormulaCalcAddRow(data) {
    taxFormulaCalcTable.row.add([
            '<input type="text" name="id" readonly value="' + data.id + '" />',
            '<input type="number" name="LowerAmount" value="' + data.LowerAmount + '" min="0" />',
            '<input type="number" name="HigherAmount" value="' + data.HigherAmount + '" min="0" />',
            '<input type="number" name="Percentage" value="' + data.Percentage + '" min="0" />',
            '<input type="number" name="FixedAmount" value="' + data.FixedAmount + '" min="0" />',
            '<input type="number" name="OtherAmount" value="' + data.OtherAmount + '" min="0" />',
            '<input type="text" name="Remarks" value="' + data.Remarks + '" />'
    ]).draw(false);
}

function CompanyCreate() {
    var form = $("#company-form");
    var data = formToObject(form);

    $.ajax({
        type: "POST",
        url: "CompanyCreate",
        data: JSON.stringify({ company: data }),
        contentType: "application/json",
        success: function () {
            $('#add-company-modal').modal('hide');
            //$("td").remove();
            //for (let i = 0; i < result.length; i++) {
            //    var tab = $('<tr />');
            //    tab.append("<td>" + result[i].id + "</td>", "<td>" + result[i].PayElementCode + "</td>", "<td>" + result[i].Description + "</td>", "<td>" + result[i].Type + "</td>", "<td>" + result[i].PayElementType + "</td>", "<td>" + result[i].Amount + "</td>", "<td>" + moment(result[i].EffectiveDate).format("YYYY-MM-DD") + "</td>", "<td>" + result[i].Taxable + "</td>");
            //    $('#pay-elements-table').append(tab);
            //}
            Message(addMessage);
            location.reload();
        }
    });
}

function CompanyDelete() {
    DeleteTableRow(CompanyTable, "CompanyDelete");
    location.reload();
}

function CompanySwitch() {
    $.ajax({
        type: "POST",
        url: "CompanySwitch",
        data: JSON.stringify({ id: CompanyTable.$("tr.selected").children().eq(0).text() }),
        contentType: "application/json",
        success: function () {
            location.reload();
        }
    });
}