$(document).ready(function () {
    $(document).ready(function () {
        $("#senddata").on("click", function (event) {
            event.preventDefault();

            var formData = new FormData($("#formteam")[0]);
            $.ajax({
                type: "POST",
                url: "https://aggocollegemkd.com.ng/api/TeamManagements",//https://localhost:44327
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swal.fire('Information!', data.message, 'success');
                        $("#formteam")[0].reset();
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