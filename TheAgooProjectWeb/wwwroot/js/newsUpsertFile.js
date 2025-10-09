$(document).ready(function () {
    $("#formnewsdata").on("submit", function (event) {
        event.preventDefault();

        let isValid = true;

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
            url: "https://aggocollegemkd.com.ng/api/NewsManager",
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