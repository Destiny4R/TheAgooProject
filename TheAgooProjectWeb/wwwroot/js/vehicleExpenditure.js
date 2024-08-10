
var datatable;
var classbtn = ""; var classfa = "";
var ap = "";
var statusx = 0;
statusx = $('#status').val();
$(document).ready(function () {
    loadtable();
});
function loadtable() {
    datatable = $("#tablevehicleExpendi").DataTable({
        "processing": true, "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/ExpeditureVehicle",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            { "data": "vehicleRegister.vehicleRegNumber", "autoWidth": true },
            { "data": "amount", "autoWidth": true },
            { "data": "partnershipReg.name", "autoWidth": true },
            { "data": "driversTable.name", "autoWidth": true },
            {
                //"data": "systemUserTable.station", "autoWidth": true
                "data": null, "render": function (data, type, full) {
                    if (data.systemUserTable == null) {
                        return data = "Head Office";
                    } else {
                        return data.systemUserTable.station;
                    }
                }
            },
            {
                "data": "status", "autoWidth": true,
                render: function (data, type, row) {
                    if (data == 0) {
                        return '<span class="badge badge-warning"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Not Approve </span>';
                    } else if (data == 1) {
                        return '<span class="badge badge-success"> <i class="mdi mdi-check-all mr-1"></i> Approve</span>';
                    } else {
                        return '<span class="badge badge-danger">Report Rejected <i class="fas fa-thumbs-down" aria-hidden="true"></i></span>';
                    }
                }
            },
            {
                "data": "expenditureDate", "autoWidth": true,
                render: function (data, type, row) {
                    if (data.length > 10) {
                        return data = data.substring(0, 10);
                    } else {
                        return data;
                    }
                }
            },
            { "data": "expenditureType", "autoWidth": true },
            {
                //"data": "id",
                "data": null, "render": function (data, type, full) {
                    if (statusx == 1) {
                        if (data.systemUserTable != null) {
                            return `
                            <div class="d-flex justify-content-left ml-2">
                                <a class=" btn-dark btn-sm" href="/Expenditure/VehicleExpenditurez/ViewVehicleExpenseDatails/?id=${data.id}" style="cursor:pointer" title="">
                                <i class="fa fa-eye"></i>
                            </a> 
                            </div>`;
                        } else {
                            return `
                            <div class="d-flex justify-content-center">
                         <a class=" btn-secondary btn-sm" href="/Expenditure/VehicleExpenditurez/Upsert?id=${data.id}" style="cursor:pointer" title="Edit">
                                <i class="fa fa-edit"></i>
                            </a> &nbsp;
                            <a class=" btn-dark btn-sm" href="/Expenditure/VehicleExpenditurez/ViewVehicleExpenseDatails/?id=${data.id}" style="cursor:pointer" title="">
                                <i class="fa fa-eye"></i>
                            </a> &nbsp;
                            <a onclick=dropVehicleexpendi("/api/DropVehicleExpenditure?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                   <i class="fa fa-trash"></i>
                               </a>
                        </div>`;
                        }
                    } else {

                        return `
                        <div class="d-flex justify-content-center">
                         <a class=" btn-secondary btn-sm" href="/Expenditure/VehicleExpenditurez/Upsert?id=${data.id}" style="cursor:pointer" title="Edit vehicle expenditure">
                                <i class="fa fa-edit"></i>
                            </a> &nbsp;
                            <a class=" btn-dark btn-sm" href="/Expenditure/VehicleExpenditurez/ViewVehicleExpenseDatails/?id=${data.id}" style="cursor:pointer" title="View vehicle expenditure">
                                <i class="fa fa-eye"></i>
                            </a> &nbsp;
                            <a onclick=dropVehicleexpendi("/api/DropVehicleExpenditure?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer" title="Delete vehicle expenditure">
                                   <i class="fa fa-trash"></i>
                               </a>
                        </div>
                    `;
                    }
                }
            }
        ]
    });
}
function dropVehicleexpendi(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to recover this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        loadtable();
                    } else {
                        swal.fire(
                            'Deleted!',
                            data.message,
                            'error'
                        );
                    }
                },
                beforeSend: function () {
                    blockcallback();
                },
                error: function () {
                    $.unblockUI();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity!'
                    });
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swal.fire(
                'Cancelled',
                'Vehicle Expenditure NOT Deleted!',
                'error'
            );
        }
    });
}

function manage(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You want to carry out this action?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, Execute!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swal.fire(
                            'Action Message!',
                            data.message,
                            'success'
                        );
                        loadtable();
                    } else {
                        swal.fire(
                            'Action Message!',
                            data.message,
                            'error'
                        );
                    }
                },
                beforeSend: function () {
                    blockcallback();
                },
                error: function () {
                    $.unblockUI();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity!'
                    });
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swal.fire(
                'Cancelled',
                'Action aborted!',
                'error'
            );
        }
    });
}

function blockcallback() {
    $.blockUI({
        message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div>',
        fadeIn: 800,
        //timeout: 2000, unblock after 2 seconds
        overlayCSS: {
            backgroundColor: '#1b2024',
            opacity: 0.8,
            zIndex: 1200,
            cursor: 'wait'
        },
        css: {
            border: 0,
            color: '#fff',
            zIndex: 1201,
            padding: 0,
            backgroundColor: 'transparent'
        },
        onBlock: function () {

        }
    });
}
