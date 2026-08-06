var font, clazz;
var datatablez;
$(document).ready(function () {
    datatablez = $("#subjectTablez").DataTable({
        dom: "<'row mb-2'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row mt-2'<'col-sm-4'l><'col-sm-4 text-center'i><'col-sm-4'p>>",
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="bi bi-file-earmark-excel me-1"></i> Export to Excel',
                className: 'btn btn-success btn-sm',
                title: 'Terminal_Master_Sheet_@Model.MasterSheet.ClassName.Replace(" ", "_")_@Model.MasterSheet.TermName',
                exportOptions: {
                    columns: ':visible',
                    format: {
                        body: function (data, row, col, node) {
                            var html = $(node).html();
                            var text = $('<div>').html(html).text();
                            return text.trim();
                        }
                    }
                }
            },
            {
                extend: 'print',
                text: '<i class="bi bi-printer me-1"></i> Print',
                className: 'btn btn-secondary btn-sm',
                title: 'Terminal Master Sheet - @Model.MasterSheet.ClassName',
                exportOptions: {
                    columns: ':visible',
                    format: {
                        body: function (data, row, col, node) {
                            var html = $(node).html();
                            var text = $('<div>').html(html).text();
                            return text.trim();
                        }
                    }
                }
            }
        ],
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
                        font = "bi-lock";
                        clazz = "Deactivate";
                        return '<span class="badge text-bg-success"><i class="bi bi-lock me-2"></i>Active</span>';
                    } else {
                        font = "bi-unlock";
                        clazz = "Activate";
                        return '<span class="badge text-bg-warning"><i class="bi bi-unlock me-2"></i>Inactive</span>';
                    }
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="dropdown">
                          <button class="btn btn-light btn-sm" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="bi bi-three-dots-vertical"></i>
                          </button>
                          <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="/Admin/Admission/Upsert?id=${data.id}"><i class="bi bi-pencil-square me-2"></i> Edit</a></li>
                            <li><a class="dropdown-item" href="/Account/ChangeUserPassword?id=${data.applicationUserId}&url=/Admin/System-User/Index"><i class="bi bi-arrow-clockwise me-2"></i> Change Password</a></li>
                            <li><a class="dropdown-item" onclick=AccountStudent("/api/LockUnlockAccount?id=${data.applicationUserId}") href="#"><i class="bi ${font} me-2"></i> ${clazz}</a></li>
                            <li><a class="dropdown-item" onclick=removeStudent("/api/StudentDelete/${data.applicationUserId}") href="#"><i class="bi bi-trash3 me-2"></i> Delete</a></li>
                          </ul>
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
            cancelButton: 'btn btn-danger me-2'
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
            cancelButton: 'btn btn-danger me-2'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Warning?',
        text: "Are you sure you want to delete this student, this can not be undone!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete!',
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
