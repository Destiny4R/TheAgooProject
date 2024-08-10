var datatable;
var classbtn = ""; var classfa = "";
var ap = "";
$(document).ready(function () {
    datatable = $("#tech-companies-1").DataTable({
        "processing": true,
        "serverSide": false,
        "filter": false,
        "responsive": false,
        "destroy": true,
        "info": false,
        "paging": false,
        "pageLength": 100,
        "ajax": {
            "url": "/Home/ResultComputation",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{ "visible": false, "searchable": false, }],
        "columns": [
            {
                "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
            },
            { "data": "fullname", "autoWidth": true },
            { "data": "studentRegNo", "autoWidth": true },
            { "data": "term", "autoWidth": true },
            { "data": "sessionYear", "autoWidth": true },
            { "data": "classes", "autoWidth": true },
            { "data": "subClass", "autoWidth": true }, 
            {
                "data": null, "render": function (data, type, full) {
                    if (data.status) {
                        return `<span class="badge h4 bg-success" title="Results have been successfully uploaded">${data.subjectsState}</span>`;
                    } else {
                        return `<label title="Results are yet to be uploaded or added" class="badge h3 bg-danger">${data.subjectsState}</label>`;
                    }
                }
            },
            {
                "data": null, "render": function (data, type, full) {
                    if (data.attendance) {
                        return `<span class="badge h4 bg-success" title="Attendance not added"><i class="fas fa-check-double"></i></span>`;
                    } else {
                        return `<label title="Attendance yet to be added" class="badge bg-danger"><i class="far fa-times-circle"></i></label>`;
                    }
                }
            },
            {
                "data": null, "render": function (data) {
                    if (data.remarkStatus) {
                        return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-info btn-sm" title="View results in detail" href="/Compute-Result/Detail-Result?id=${data.termRegId}" style="cursor:pointer">
                                <i class="fa fa-eye"></i>
                            </a>&nbsp;
                            <a class="btn btn-success btn-sm" href="/Compute-Result/Remark-Comment?id=${data.termRegId}" style="cursor:pointer" title="Edit student remark">
                                <i class="fas fa-check-double"></i> &nbsp; Remark
                            </a>
                        </div>
                    `;
                    } else {
                        return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-info btn-sm" title="View results in detail" href="/Compute-Result/Detail-Result?id=${data.termRegId}" style="cursor:pointer">
                                <i class="fa fa-eye"></i>
                            </a>&nbsp;
                            <a class="btn btn-danger btn-sm" href="/Compute-Result/Remark-Comment?id=${data.termRegId}" style="cursor:pointer" title="Add student remark">
                                <i class="fas fa-window-close"></i> &nbsp; Remark
                            </a>
                        </div>
                    `;
                    }
                }
            }
        ]
    });
});

//PUBLISHING THE RESULT
$("body").on("click", "#btnpublish", function () {
    var term = $('#theterm').find(":selected").val();
    var session = $('#thesession').find(":selected").val();
    if (term != "" && session != "") {
        $.ajax({
            type: "POST",
            url: "/api/PublistResult",
            data: { "input.term": term, "input.session": session },
            dataType: "json",
            beforeSend: function () {
                blockcallback();
            },
            success: function (data) {
                $.unblockUI();
                if (data.success) {
                    swal.fire(
                        'Result Publishing!',
                        data.message,
                        'success'
                    );
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data.message,
                        footer: 'Result Publication Error!'
                    });
                }
            },
            complete: function (data) {
                $.unblockUI();
            },
            error: function () {
                $.unblockUI();
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong!',
                    footer: 'Check internet connectivity'
                });
            }
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Select Session/Year & Term before publishing the result',
            footer: 'Result Publication Failed!'
        });
    }
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
