var font, clazz;
var datatablez;
$(document).ready(function () {
    datatablez = $("#subjectTablez").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "paging": true,
        responsive: true,
        "ajax": {
            "url": "/Home/StudentData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true,
            "searchable": true,
        }],
        "columns": [
            {
                "data": "id",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    var name = data.surName + ' ' + (data?.otherName||'')+  ' ' + (data.firstName);
                    return name;
                }
            },
            { "data": "studentRegNo", "autoWidth": true },
            { "data": "gender", "autoWidth": true },
            {
                "data": "dateOfBirth",
                render: function (data, type, row) {
                    if (data.length > 10) {
                        return data = data.substring(0, 10);
                    } else {
                        return data;
                    }
                }
            },
            { "data": "state", "autoWidth": true },
            { "data": "localGov", "autoWidth": true },
            {
                "data": "applicationUser.active", "render": function (data, type, full) {
                    if (data == true) {
                        font = "fa-unlock";
                        clazz = "btn-success";
                        return data;
                    } else {
                        font = "fa-lock";
                        clazz = "btn-warning";
                        return data;
                    }
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-primary btn-sm" title="Edit Student Record" href="/Admin/Admission/Upsert?id=${data.id}" style="cursor:pointer">
                                <i class="fa fa-edit"></i>
                            </a>&nbsp;
                            <a class="btn btn-info btn-sm" href="/Account/ChangeUserPassword?id=${data.applicationUserId}&_returnUrl=/Admin/Admission/Index" style="cursor:pointer" title="Reset student/pupil password">
                                <i class="fas fa-bolt"></i>
                            </a>&nbsp;
                            <a onclick=AccountStudent("/api/LockUnlockAccount/${data.applicationUserId}") title="Suspend Student Account" class="btn ${clazz} btn-sm" href="#" style="cursor:pointer">
                                <i class="fa ${font}"></i>
                            </a>
                            &nbsp;
                            <a onclick=removeStudent("/api/StudentDelete/${data.applicationUserId}") title="Permanently Remove Student Record" class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="fa fa-trash"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
});
//AccountStudent
function AccountStudent(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You want to Block/ Unblock student account!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, execute!',
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
                        swalWithBootstrapButtons.fire(
                            'BLOCK/UNBLOCK!',
                            data.message,
                            'success'
                        );
                        datatablez.ajax.reload();
                        //datatableloader();
                    } else {
                        swalWithBootstrapButtons.fire(
                            'BLOCK/UNBLOCK!',
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
                        footer: 'Check internet connectivity'
                    });
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Action Aborted',
                'error'
            );
        }
    });
}

function removeStudent(url) {
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
                            'Record and Account Successfully deleted.',
                            'success'
                        );
                        datatablez.ajax.reload();
                    } else {
                        toastr.error(data.message);
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
                        footer: 'Check internet connectivity'
                    });
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Student Record NOT Deleted',
                'error'
            );
        }
    });

}
//function datatableloader() {

//}
function blockcallback() {
    $.blockUI({
        message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div> ',
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
