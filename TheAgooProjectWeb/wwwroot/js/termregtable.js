
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
    datatable = $("#termregtable1").DataTable({
        "processing": true, "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/TermRegistration",
            "type": "POST",
            "datatype": "json",
            data: function (d) {
                var f = collectFormData();
                d.term = f.term;
                d.classes = f.classes;
                d.subclass = f.subclass;
                d.sessionYear = f.sessionYear;
                return d;
            }
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                "data": null, "render": function (data, type, row, meta) {
                    return `
                        <input type="checkbox" class="check-class" name="datas[${meta.row}].ischeck" value="true">
                    `;
                }
            },
            {
                "data": null, "render": function (data, type, row, meta) {
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
                "data": null, "render": function (data, type, row, meta) {
                    var output = Object.keys(data.resultTable).length;
                    return output + ' Subjects';
                }

            },
            {
                "data": null, "render": function (data, type, row, meta) {
                    return `
                        <div class="dropdown">
                          <button class="btn btn-light btn-sm" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="bi bi-three-dots-vertical"></i>
                          </button>
                          <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="/Admin/TermRegistration/Edit?id=${data.id}"><i class="bi bi-pencil-square me-2"></i> Edit</a></li>
                            <li><button class="dropdown-item" data-id="${data.id}" id="btnDelete" type="button"><i class="bi bi-trash3 me-2"></i> Delete</button></li>
                            <input type="hidden" name="datas[${meta.row}].id"  value="${data.id}" />
                          </ul>
                        </div>
                        `;

                }
            }
        ]
    });


    $(document).on('click', '#btnDelete', function () {
        var id = $(this).data('id');
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success ',
                cancelButton: 'btn btn-danger me-3'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({
            title: 'Are you sure?',
            text: "You want to remove this sudent from this term?!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: `/api/ManageTermReg/${id}`,
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
            }
        });
    });

    function collectFormData() {
        return {
            term: $('select[name="CTSSClass.Term"]').val(),
            sessionYear: $('select[name="CTSSClass.SessionYear"]').val(),
            classes: $('select[name="CTSSClass.Classes"]').val(),
            subclass: $('select[name="CTSSClass.Subclass"]').val()
        };
    }

    // Handle changes and send AJAX request
    $('select[name^="CTSSClass"]').on('change', function () {
        var formData = collectFormData();

        // Example: Only proceed if all fields are selected
        if (formData.term && formData.sessionYear && formData.classes && formData.subclass) {
            //console.log('All fields selected:', formData);
            datatable.ajax.reload();
        }
    });
});

function removeStudent(url) {
    
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
