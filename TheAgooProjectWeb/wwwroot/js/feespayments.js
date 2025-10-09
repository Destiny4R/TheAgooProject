var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    loadtable();
});
function loadtable() {
    datatable = $("#fee_payment").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        responsive: true,
        destroy: true,
        "ajax": {
            "url": "/Home/FeesPayemtData",
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
                    var name = data.termregistrations.studentsData.surName + ' ' + (data?.termregistrations.studentsData.otherName || '') + ' ' + data.termregistrations.studentsData.firstName;
                    return name;
                }
            },
            { "data": "termregistrations.studentsData.studentRegNo", "autoWidth": true },
            { "data": "termlyFees.amount", "autoWidth": true },
            { "data": "paid", "autoWidth": true },
            { "data": "balance", "autoWidth": true },
            {
                "data": null, "render": function (data, type, full) {
                    if (data.status =="Completed") {
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
                    return data.termregistrations.sessionYear.name;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return data.termregistrations.term;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return data.termregistrations.schoolclasses.name + " - " + data.termregistrations.subClasses.name;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="d-flex justify-content-center">
                         <button type="button" onclick="viewpaymentifo(this)" data-id="${data.id}" title="View payment details" class="btn btn-primary btn-sm waves-effect waves-light">
                         <i class="fa fa-eye"></i>
                         </button>
                            &nbsp;
                            <a target="_blank" class="btn btn-success btn-sm waves-effect waves-light" href="/ultilities/Fees-Payment-Invoice?id=${data.id}" style="cursor:pointer" title="Print receipt">
                                <i class="fas fa-print"></i>
                            </a> &nbsp;
                            <a onclick=FeesPaymentD("/api/FeesPaymentManager?id=${data.id}") class="btn btn-danger btn-sm waves-effect waves-light" href="#" style="cursor:pointer">
                                   <i class="fas fa-trash-alt"></i>
                               </a>
                        </div>
                    `;
                }
            }
        ]
    });
}
function FeesPaymentD(url) {
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
                'Fees Payment NOT Deleted!',
                'error'
            );
        }
    });
}

function test() {
    $(".bs-example-modal-center").hide();
}
function viewpaymentifo(dat) {
    var dataId = $(dat).data('id');
    let id = Number(dataId);
    if (id < 1) {
        return Swal.fire("Ooop!", "Invalid data", "erro");
    }
    $.ajax({
        type: "GET",
        url: "/webapi/Veiwreceipt?id="+id,
        success: function (data) {
            $.unblockUI();
            if (data) {
                if (data != null) {
                    $("#receipt").val(data.receiptNumber);
                    $("#name").val(data.name);
                    $("#studentreg").val(data.studentReg);
                    $("#session").val(data.sessionyear);
                    $("#term").val(data.term);
                    $("#classes").val(data.classes);
                    $("#fees").val(data.fees);
                    $("#amount").val(data.amount);
                    $("#balance").val(data.balance);
                    $("#status").val(data.status);
                    $("#description").val(data.description);
                    $("#cash").val(data.cashier); 
                    $("#create").val(data.createdate);
                    $(".bs-example-modal-center").show();
                }else {
                swal.fire(
                    'Ooop!',
                    'Unknow fees payment',
                    'error'
                );
            }
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
}
function editPayment() {
    swal.fire(
        'Ooop!',
        'Sorry, payment made are none editable, kindly remove your payment and re-add for error corrections',
        'success'
    );
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

