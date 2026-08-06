var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    loadtable();
});
function loadtable() {
    datatable = $("#specialpayment").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/AditionalPaymentData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": true, "searchable": true, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            {
                "data": null, "render": function (data, type, full) {
                    var name = data.termregistration.studentsData.surName + ' ' + (data?.termregistration.studentsData.otherName || '') + ' ' + data.termregistration.studentsData.firstName;
                    return name;
                }
            },
            { "data": "termregistration.studentsData.studentRegNo", "autoWidth": true },
            { "data": "specialPay.amount", "autoWidth": true },
            { "data": "specialPay.otherPayTables.name", "autoWidth": true },
            { "data": "amount", "autoWidth": true },
            {
                "data": null, "render": function (data, type, full) {
                    if (data.amount == data.specialPay.amount) {
                        return `<span class="badge h4 bg-success" title="Fees completed">
                        <i class="bi bi-check2-all me-2"></i> Completed
                        </span>`;
                    } else {
                        return `<label title="Part Payment" class="badge h3 bg-warning"><i class="bi bi-star-half me-2"></i> Part Payment</label>`;
                    }
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return data.termregistration.sessionYear.name;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return data.termregistration.term;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return data.termregistration.schoolclasses.name + " - " + data.termregistration.subClasses.name;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="dropdown">
                            <button class="btn btn-secondary" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="bi bi-three-dots-vertical"></i>
                            </button>
                            <ul class="dropdown-menu">
                                <li>
                                    <a target="_blank" class="dropdown-item" disabled href="#" style="cursor:pointer" title="Edit Payment">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                                </li>
                                <li>
                                    <a onclick=FeesPaymentSpecial("/api/AdditionaPayMnanager?id=${data.id}") class="dropdown-item" href="#" style="cursor:pointer">
                                       <i class="bi bi-trash3"></i>
                                   </a>
                                </li>
                            </ul>
                        </div>
                    `;
                }
            }
        ]
    });
}
function FeesPaymentSpecial(url) {
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
                'Special Payment NOT Deleted!',
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