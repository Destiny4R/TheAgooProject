$(document).ready(function () {
    dataTable = $("#tablenews").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Home/GetFeedBack",
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
            { "data": "name", "autoWidth": true },
            { "data": "email", "autoWidth": true },
            {
                "data": "subject",
                render: function (data, type, row) {
                    if (data.length > 30) {
                        return data = data.substring(0, 30) + '.....';
                    } else {
                        return data;
                    }
                }
            },
            {
                "data": "message",
                render: function (data, type, row) {
                    if (data.length > 50) {
                        return data = data.substring(0, 50) + '.....';
                    } else {
                        return data;
                    }
                }
            },
            {
                "data": "createdDate", "autoWidth": true,
                render: function (data, type, row) {
                    var hours = new Date(data).getHours()
                    let ap = hours >= 12 ? 'pm' : 'am';
                    return data = data.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                }
            },
            {
                "data": "isRead", "autoWidth": true, className: 'text-center',
                render: function (data, type, row) {
                    if (data == true) {
                        return `<label class="bg-warning badge"><i class="fa fa-envelope-open" aria-hidden="true"></i></label>`;
                    } else {
                        return `<label class="bg-success badge"><i class="fa fa-envelope" aria-hidden="true"></i></label>`;
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-success btn-sm" title="View feed back from customer" href="/Messaging/Feed-Back/Message-Detail?id=${data}" style="cursor:pointer">
                                <i class="bi bi-eye"></i>
                            </a>
                            <a onclick=Delete("/api/DeleteFeedback/${data}") class="btn btn-danger btn-sm pl-2" href="#" style="cursor:pointer">
                                <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
});

function Delete(url) {
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
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: data.message,
                            footer: 'Feed back message'
                        });
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
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Action aborted!',
                'error'
            );
        }
    });
}

function blockcallback() {
    $.blockUI({
        message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div> ',
        fadeIn: 800,
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