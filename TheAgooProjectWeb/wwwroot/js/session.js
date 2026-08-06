var font, clazz;
var datatablez;
$(document).ready(function () {
    datatablez = $("#sessiontable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "paging": true,
        responsive: true,
        "ajax": {
            "url": "/Home/SessionData",
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
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="d-flex justify-content-between">
                            <button class="btn btn-primary btn-sm btn-edit" title="Edit Session/Year Name" data-id="${data.id}" data-name="${data.name}" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </button>
                            &nbsp;
                            <a onclick=removeSession("/api/sessionmanager/${data.id}") title="Permanently Remove session" class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="bi bi-trash"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });

    $(document).on("click", ".btn-edit", function () {

        // Get values from data attributes
        var sessionName = $(this).data("name");
        var sessionId = $(this).data("id");

        // Populate modal input fields
        $("#editSessionModal input[name='sessionYear.Name']").val(sessionName);
        $("#editSessionModal input[name='sessionYear.Id']").val(sessionId);

        // Show Bootstrap modal
        var modal = new bootstrap.Modal(document.getElementById('editSessionModal'));
        modal.show();
    });
});


function removeSession(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger me-2'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
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
                            data.message,
                            'success'
                        );
                        datatablez.ajax.reload();
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
                'Session/Year Record NOT Deleted',
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
