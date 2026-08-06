var font, clazz;
var datatablez;
$(document).ready(function () {
    datatablez = $("#studentdata").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "paging": true,
        responsive: true,
        "ajax": {
            "url": "/Home/PTAFeesPayemtData",
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
            {
                "data": null, "render": function (data, type, full) {
                    var name = data.termregistration.studentsData.surName + ' ' + (data?.termregistration.studentsData.otherName || '') + ' ' + data.termregistration.studentsData.firstName;
                    return name;
                }
            },
            { "data": "termregistration.studentsData.studentRegNo", "autoWidth": true },
            { "data": "ptaFee.fees", "autoWidth": true },
            { "data": "amount", "autoWidth": true },
            { "data": "balance", "autoWidth": true },
            { "data": "termregistration.term", "autoWidth": true },
            { "data": "termregistration.sessionYear.name", "autoWidth": true },
            {
                "data": null, "render": function (data, type, full) {
                    var name = data.termregistration.schoolclasses.name + ' - ' + (data.termregistration.subClasses.name || '');
                    return name;
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    if (data.createdDate.length > 10) {
                        return data.createdDate.substring(0, 10);
                    }
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-primary btn-sm" target="_blank" title="View Payment Invoice" href="/ultilities/Pta-Payment-Invoice?id=${data.id}"  style="cursor:pointer">
                                <i class="bi bi-printer"></i>
                            </a>
                            &nbsp;
                            <a onclick=removeSession("/api/PTAFeesPaymentManager/${data.id}") title="Permanently Remove this Expenditure" class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
});


function removeSession(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success p-2',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

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
                'PTA Fees Payment Record NOT Deleted',
                'error'
            );
        }
    });

}

function viewPayment(url) {
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        success: successFunc,
        error: errorFunc,
        beforeSend: function () {
            blockcallback();
        },
        complete: function () {
            $.unblockUI();
        }
    });

    function successFunc(data) {
        $.unblockUI();
        if (data.success) {
            $('#employee1').val(data.data.applicationUser.fullname);
            $('#subject1').val(data.data.subject);
            $('#sender1').val(data.data.sender);
            $('#message1').val(data.data.message);
            $('#cash').val(data.data.cost);
            var hours = new Date(data.data.createdDate).getHours()
            let ap = hours >= 12 ? 'pm' : 'am';
            let data2 = data.data.createdDate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
            $('#createdate1').val(data2);
            $('#staticBackdrop').modal('show');
        } else {
            $.unblockUI();
            swal.fire(
                'Ooop!',
                'An error occured while generating the report',
                'error'
            );
        }
    }

    function errorFunc() {
        $('#smsloader' + id).hide();
        $('#nosmsloader' + id).show();
        $.unblockUI();
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: 'Check internet connectivity!'
        });
    }

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
