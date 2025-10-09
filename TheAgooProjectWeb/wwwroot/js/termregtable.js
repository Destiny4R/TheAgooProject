
var datatable;
var classbtn = ""; var classfa = "";
$(document).ready(function () {
    $('#checkAll').on('change', function () {
        $('.check-class').prop('checked', this.checked);
        if (!this.checked) {
            var el = $('#checkAll').get(0);
            if (el && el.checked && ('indeterminate' in el)) {
                // Set visual state of "Select all" control
                // as 'indeterminate'
                el.indeterminate = true;
            }
        }
    });
    let no = 0;


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
                    //<input type="checkbox" class="check-class" name="datas[${no}].ischeck" id="checkAll_${no}" value="false" />
                    return `
                        <input type="checkbox" class="check-class" name="datas[${no}].ischeck" id="checkAll_${no}" value="true">
                    `;
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
                    no++;
                    return `
                            <div class="d-flex justify-content-center">
                             <a class=" btn-secondary btn-sm" href="/Admin/TermRegistration/Edit?id=${data.id}" style="cursor:pointer" title="Edit">
                                    <i class="fa fa-edit"></i>
                                </a> &nbsp;
                                <input type="hidden" name="datas[${no - 1}].id"  value="${data.id}" hidden />
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

document.getElementById('thecheckhouse').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent the default form submission
    var checkedCheckboxes = document.querySelectorAll('#termregtable1 tbody input[type="checkbox"]:checked');

    if (checkedCheckboxes.length === 0) {
        return Swal.fire({
            icon: 'error',
            //title: 'Oops...',
            text: 'Please check one or more rows',
            footer: 'NO DATA CHECKED'
        });
    }

    var rc = checkedCheckboxes.length > 1 ? "recocrds" : "recocrd";
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: `Are you sure want to delete ${checkedCheckboxes.length} ${rc}?`,
        text: "You won't be able to recover this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            blockcallback();
            // If the user confirms, submit the form
            this.submit();
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Student(s) NOT Deleted',
                'error'
            );
        }
    });
});


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
