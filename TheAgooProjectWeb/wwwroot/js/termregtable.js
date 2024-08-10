
var datatable;
var classbtn = ""; var classfa = "";
$(document).ready(function () {
    datatable = $("#termregtable1").DataTable({
        "processing": true, "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/TermRegistration",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    var name = data.studentsData.surName + ' ' + (data?.studentsData.otherName||'') + ' ' + data.studentsData.firstName;
                    return name;
                }
            },
            { "data": "studentsData.studentRegNo", "autoWidth": true },
            { "data": "term", "autoWidth": true },
            { "data": "sessionYear.name", "autoWidth": true },
            { "data": "schoolclasses.name", "autoWidth": true },
            { "data": "subClasses.name", "autoWidth": true },
            {
                "data": null, "render": function (data, type, full) {
                    var output = Object.keys(data.resultTable).length;
                    return output + ' Subjects';
                }

            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                            <div class="d-flex justify-content-center">
                             <a class=" btn-secondary btn-sm" href="/Admin/TermRegistration/Edit?id=${data.id}" style="cursor:pointer" title="Edit">
                                    <i class="fa fa-edit"></i>
                                </a> &nbsp;
                                <a onclick=removeStudent("/api/ManageTermReg?id=${data.id}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                       <i class="fa fa-trash"></i>
                                   </a>
                            </div>`;

                }
            }
        ]
    });
});

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
                        datatable.ajax.reload();
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
                'Student NOT removed from the term!',
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
