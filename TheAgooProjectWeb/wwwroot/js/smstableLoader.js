var dataTable;
$(document).ready(function () {
    dataTable = $("#subjectTable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Home/LoadSmsData",
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
                    let output = (data.applicationUser.fullname);
                    return output;
                }
            },
            { "data": "sender", "autoWidth": true },
            { "data": "subject", "autoWidth": true },
            {
                "data": "message",
                render: function (data, type, row) {
                    if (data.length > 30) {
                        return data = data.substring(0, 25)+'.....';
                    } else {
                        return data;
                    }
                }
            },
            { "data": "cost", "autoWidth": true },
            {
                "data": "createdDate", "autoWidth": true,
                render: function (data, type, row) {
                    var hours = new Date(data).getHours()
                    let ap = hours >= 12 ? 'pm' : 'am';
                    return data = data.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <button type="button" class="btn btn-success btn-sm" onclick="return viewReport(${data});" id="buttonsms"  data-id="${data}" style="cursor:pointer">
                            <i class="fa fa-spin fa-spinner" id="smsloader${data}" style="display:none;"></i>
                                <i class="fa fa-eye"  id="nosmsloader${data}"></i>
                            </button>
                        </div>
                    `;
                }
            }
        ]
    });
});


function viewReport(id) {
    $.ajax({
        type: "POST",
        url: '/webapi/SmsDetails/',
        data: { id: id},
        dataType: "json",
        success: successFunc,
        error: errorFunc,
        beforeSend: function () {
            blockcallback();
            $('#smsloader' + id).show();
            $('#nosmsloader' + id).hide();
        },
        complete: function () {
            $.unblockUI();
            $('#smsloader' + id).hide();
            $('#nosmsloader' + id).show();
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
            $('#paymentmodal').modal('show');
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

function test() {
    $('#paymentmodal').modal('hide');
}

function blockcallback() {
    $.blockUI({
        message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div> ',
        fadeIn: 800,
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