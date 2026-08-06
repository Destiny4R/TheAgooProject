var font, clazz;
var datatablez;
$(document).ready(function () {
    if ($("#teammanager").length) {
        datatablez = $("#teammanager").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "paging": true,
            responsive: true,
            "ajax": {
                "url": "/Home/TeamManagers",
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
                { "data": "fullname", "autoWidth": true },
                { "data": "position", "autoWidth": true },
                { "data": "email", "autoWidth": true },
                { "data": "phone", "autoWidth": true },
                { "data": "hierarchy", "autoWidth": true },
                {
                    "data": "createdDate",
                    "render": function (data) {
                        if (!data) return 'Null';
                        var date = new Date(data);
                        var formattedDate = date.toISOString().slice(0, 19).replace('T', ' ');
                        var hours = date.getHours();
                        var ap = hours >= 12 ? 'pm' : 'am';
                        return formattedDate + ' ' + ap;
                    }
                },
                {
                    "data": null, "render": function (data, type, full) {
                        return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-primary btn-sm" title="Edit Team data" href="/WebsiteManager/TeamManagers/Upsert?id=${data.id}" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            &nbsp;
                            <a onclick=removeteam("/api/RemoveTeamManagement/${data.id}") title="Permanently Remove team member" class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `;
                    }
                }
            ]
        });
    }

    $("#senddata").on("click", function (event) {
        event.preventDefault();

        var formData = new FormData($("#formteam")[0]);
        let baseurl = $("#baseUrl").val();
        $.ajax({
            type: "POST",
            url: `${baseurl}/api/TeamManagements`,//https://localhost:44327
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                $.unblockUI();
                if (data.success) {
                    swal.fire('Information!', data.message, 'success');
                    $("#formteam")[0].reset();
                } else {
                    swal.fire('Information!', data.message, 'error');
                }
            },
            beforeSend: function () {
                blockcallback();
            },
            error: function (error) {
                // This function now has a name: handleError
                function handleError() {
                    $.unblockUI();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity'
                    });
                }
                handleError(); // Call the named function
            },
            complete: function () {
                $.unblockUI();
            }
        });
    });
});
function removeteam(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })
    let baseurl = $("#baseUrl").val();
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
                type: "DELETE",//Add url
                url: baseurl + url,
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
                'Team Member Record NOT Deleted',
                'error'
            );
        }
    });

}
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