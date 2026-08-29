
$(document).ready(function () {
    $("#daily-attendance-sheet-dataTable").DataTable({ scrollX: true });
    $("#monthly-attendance-sheet-dataTable").DataTable({ scrollX: true });
    $("#ot-process-dataTable").DataTable({ scrollX: true });
    $("#loan-application-dataTable").DataTable();
    $('#leave-settlement-dataTable').DataTable({
        scrollX: true,
        paging: false,
        searching: false
    });
    $('#monthly-addition-dataTable').DataTable({
        scrollX: true,
        paging: false,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'colvis'
        ]
    });
    $('#monthly-deduction-dataTable').DataTable({
        scrollX: true,
        paging: false,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'colvis'
        ]
    });
});