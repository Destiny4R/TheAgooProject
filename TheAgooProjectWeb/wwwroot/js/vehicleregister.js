var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    loadtable();
    
});
function loadtable() {
    datatable = $("#tableregisteredcars").DataTable({
        "processing": true, "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/VehicleRegistered",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            { "data": "partnershipReg.applicationUser.userName", "autoWidth": true },
            { "data": "partnershipReg.name", "autoWidth": true },
            { "data": "name", "autoWidth": true },
            { "data": "vehicleRegNumber", "autoWidth": true },
            { "data": "companyNumber", "autoWidth": true },
            { "data": "carType", "autoWidth": true },
            { "data": "carMake", "autoWidth": true },
            { "data": "seater", "autoWidth": true },
            {
                "data": "status",
                render: function (data, type, row) {
                    if (data == true) {
                        classfa = "fa-unlock";
                        classbtn = "btn-success";
                        return data;
                    } else {
                        classfa = "fa-lock";
                        classbtn = "btn-warning";
                        return data;
                    }
                }
            },
            {
                //"data": "id",
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="d-flex justify-content-center">
                         <a class=" btn-secondary btn-sm" href="/CarData/Upsert?id=${data.id}" style="cursor:pointer" title="Edit Vehicle Information">
                                <i class="fa fa-edit"></i>
                            </a> &nbsp;
                            <a onclick=managevehiclez("/api/VehicleManager?id=${data.id}") class="btn ${classbtn} btn-sm" href="#" style="cursor:pointer" title="Lock/Unlock Vehicle">
                                <i class="fa ${classfa}"></i>
                            </a> &nbsp;
                            <a onclick=Deletevehicle("/api/ManageVehicle?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                   <i class="fa fa-trash"></i>
                               </a>
                        </div>
                    `;
                }
            }
        ]
    });
}
function Deletevehicle(url) {
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
                'Vehicle NOT Deleted!',
                'error'
            );
        }
    });
}

function managevehiclez(url) {
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
