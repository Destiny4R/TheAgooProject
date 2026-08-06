var font, clazz;
var datatablez;

$(document).ready(function () {

    if ($("#newsdatatable").length) {
        datatablez = $("#newsdatatable").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "paging": true,
            responsive: true,
            "ajax": {
                "url": "/Home/GetnewsInformation",
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
                { "data": "reporter", "autoWidth": true },
                { "data": "headline", "autoWidth": true },
                { "data": "category", "autoWidth": true },
                {
                    "data": "createdDate", "autoWidth": true,
                    render: function (data, type, row) {
                        var hours = new Date(data).getHours()
                        let ap = hours >= 12 ? 'pm' : 'am';
                        return data = data.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                    }
                },
                {
                    "data": null, "render": function (data, type, full) {
                        return `
                        <div class="d-flex justify-content-between">
                            <a class="btn btn-primary btn-sm" title="Edit news" href="/WebsiteManager/WebNews/Upsert?id=${data.id}" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            &nbsp;
                            <a onclick=manageNews("/api/RemoveSchoolNews/${data.id}") title="Permanently Remove News" class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="bi bi-trash"></i>
                            </a>
                        </div>
                    `;
                    }
                }
            ]
        });
    }

    $("#formnewsdata").on("submit", function (event) {
        event.preventDefault();

        let isValid = true;
        let baseurl = $("#baseUrl").val();
        $("#formnewsdata input, #formnewsdata select, #formnewsdata textarea").each(function () {
            if ($(this).val() === "" && $(this).attr("required")) {
                $(this).addClass("is-invalid");
                isValid = false;
            } else {
                $(this).removeClass("is-invalid");
                $(this).addClass("is-valid");
            }
        });

        if (!isValid) {
            return swal.fire('Warning Message!', 'Please fill in all required fields.', 'error');
        }
        var newsBodyContent = tinymce.get("newsdata_NewBody").getContent();
        var formData = new FormData(this);
        formData.append("newsdata.NewBody", newsBodyContent);

        $.ajax({
            type: "POST",
            url: `${baseurl}/api/NewsManager`,
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                $.unblockUI();
                if (data.success) {
                    swal.fire('Information!', data.message, 'success');
                    $("#formnewsdata")[0].reset();
                } else {
                    swal.fire('Information!', data.message, 'error');
                }
            },
            beforeSend: function () {
                blockcallback();
            },
            error: function (error) {
                // This function now has a name: handleError
                function handleError() {
                    $.unblockUI();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity'
                    });
                }
                handleError(); // Call the named function
            },
            complete: function () {
                $.unblockUI();
            }
        });
    });



    0 < $("#newsdata_NewBody").length && tinymce.init({
        selector: "textarea#newsdata_NewBody",
        height: 300,
        plugins: ["advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            "save table contextmenu directionality emoticons template paste textcolor"],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | l      ink image | print preview media fullpage | forecolor backcolor emoticons",
        style_formats: [{ title: "Bold text", inline: "b" },
        {
            title: "Red text", inline: "span",
            styles: { color: "#ff0000" }
        },
        {
            title: "Red header", block: "h1",
            styles: { color: "#ff0000" }
        },
        {
            title: "Example 1", inline: "span",
            classes: "example1"
        },
        { title: "Example 2", inline: "span", classes: "example2" },
        { title: "Table styles" },
        { title: "Table row 1", selector: "tr", classes: "tablerow1" }]
    });
});

function manageNews(url) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    let baseurl = $("#baseUrl").val();

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
                url: baseurl + url,//https://localhost:44327
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
                'News Information NOT Deleted',
                'error'
            );
        }
    });

}
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