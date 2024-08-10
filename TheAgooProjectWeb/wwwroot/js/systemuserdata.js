var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    loadtable();
});
function loadtable() {
    $("#tablesystemusers").DataTable({
        "processing": true, "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/SystemUserz",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            { "data": "fullname", "autoWidth": true },
            { "data": "userName", "autoWidth": true },
            {
                "data": "active",
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
            { "data": "position", "autoWidth": true },
            {
                "data": null,
                "render": function (data, type, full) {
                    return `
                    <div class="d-flex justify-content-center">
                         <a class=" btn-secondary btn-sm" href="/Admin/System-User/Upsert?id=${data.id}" style="cursor:pointer" title="Edit System User Account">
                                <i class="fa fa-edit"></i>
                            </a> &nbsp;
                            <a class=" btn-dark btn-sm" href="/Account/ChangeUserPassword?id=${data.id}&url=/Admin/System-User/Index" style="cursor:pointer" title="Change System User Account Password">
                                <i class="fa fa-redo"></i>
                            </a> &nbsp;
                            <a onclick=Systemusermanger("/api/LockUnlockAccount?id=${data.id}") class="btn ${classbtn} btn-sm" href="#" style="cursor:pointer" title="Lock/Unlock branch Account">
                                <i class="fa ${classfa}"></i>
                            </a> &nbsp;
                            <a onclick=DeleteSystemUser("/api/ManageSystemUser?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                   <i class="fa fa-trash"></i>
                               </a>
                        </div>
                    `;
                }
            }
        ]
    });
}
function DeleteSystemUser(url) {
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
                'System User Account NOT Deleted!',
                'error'
            );
        }
    });
}
function Systemusermanger(url) {
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
