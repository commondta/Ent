$(document).ready(function () {
    $('#address-detail-dataTable').DataTable({
        scrollX: true,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'excel', 'print'
        ]
    });
    $('#bank-detail-dataTable').DataTable({
        scrollX: true,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'excel', 'print'
        ]
    });
    $('#job-detail-dataTable').DataTable({
        scrollX: true,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'excel', 'print'
        ]
    });
    $('#personal-detail-dataTable').DataTable({
        scrollX: true,
        searching: false,
        dom: 'Bfrtip',
        buttons: [
            'excel', 'print'
        ]
    });
});