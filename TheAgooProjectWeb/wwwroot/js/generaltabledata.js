var datatable;
var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    loadtable();
});
datatable = $("#general").DataTable({
    "processing": true, "serverSide": true,
    "filter": true,
    responsive: true,
    destroy: true,
    "ajax": {
        "url": "/Home/GeneralInfo",
        "type": "POST",
        "datatype": "json"
    },
    "columnDefs": [{ "visible": true, "searchable": true, }],
    "columns": [
        {
            "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
        },
        { "data": "term", "autoWidth": true },
        { "data": "sessionYear.name", "autoWidth": true },
        { "data": "classcchool.name", "autoWidth": true },
        { "data": "subClasses.name", "autoWidth": true },
        {
            "data": "nextTermStart", "autoWidth": true,
            render: function (data, type, row) {
                if (data.length > 10) {
                    return data = data.substring(0, 10);
                } else {
                    return data;
                }
            }
        },
        {
            "data": "termEnd", "autoWidth": true,
            render: function (data, type, row) {
                if (data.length > 10) {
                    return data = data.substring(0, 10);
                } else {
                    return data;
                }
            }
        },
        { "data": "classTeacher", "autoWidth": true },
        {
            //"data": "id",
            "data": null, "render": function (data, type, full) {
                return `
                        <div class="d-flex justify-content-center">
                         <a class=" btn-secondary btn-sm" href="/Compute-Result/General-Class-Info/Upsert?id=${data.id}" style="cursor:pointer" title="Edit general class information">
                                <i class="fa fa-edit"></i>
                            </a> &nbsp;
                            <a onclick=DeleteGeneral("/api/ManageGeneralInfo?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                   <i class="fa fa-trash"></i>
                               </a>
                        </div>
                    `;
            }
        }
    ]
});
function DeleteGeneral(url) {
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
                    datatable.ajax.reload();
                    if (data.success) {
                        swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
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
                'General Information NOT Deleted!',
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

