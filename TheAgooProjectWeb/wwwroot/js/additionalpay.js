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
            { "data": "specialPay.otherPayTables.name", "autoWidth": true},
            { "data": "amount", "autoWidth": true },
            {
                "data": null, "render": function (data, type, full) {
                    if (data.amount == data.specialPay.amount) {
                        return `<span class="badge h4 bg-success" title="Fees completed">
                        <i class="fas fa-check-double"></i> Completed
                        </span>`;
                    } else {
                        return `<label title="Part Payment" class="badge h3 bg-warning"><i class="fas fa-times-circle"></i> Part Payment</label>`;
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
                        <div class="d-flex justify-content-center">
                            <a target="_blank" class="btn btn-primary btn-sm waves-effect waves-light" disabled href="#" style="cursor:pointer" title="Edit Payment">
                                <i class="fas fa-edit"></i>
                            </a> &nbsp;
                            <a onclick=FeesPaymentSpecial("/api/AdditionaPayMnanager?id=${data.id}") class="btn btn-danger btn-sm waves-effect waves-light" href="#" style="cursor:pointer">
                                   <i class="fa fa-trash"></i>
                               </a>
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