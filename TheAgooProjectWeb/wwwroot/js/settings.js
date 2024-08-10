$(document).ready(function () {
    $('#fileInputHead').change(handleFileSelectionhead);
    $('#fileInputAccount').change(handleFileSelectionaccount);


        $('#notifyOnCompletion').change(function () {
            const isChecked = $(this).is(':checked');

            var idz = $('#idz').val();
            const formData = new FormData();
            formData.append("cando", isChecked);
            formData.append("id", idz);

            $.ajax({
                type: "POST",
                url: '/api/CanprintResult',
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        Swal.fire({
                            title: "Successful!",
                            text: data.message,
                            icon: "success",
                            showCancelButton: !0,
                            confirmButtonColor: "#3d8ef8"
                        });
                    } else {
                        swal.fire(
                            'Error!',
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
        });
});

function handleFileSelectionhead(event) {
    const selectedFile = event.target.files[0];

    if (!selectedFile) {
        return; 
    }
    var idz = $('#idz').val();
    var head = "Head";
    const formData = new FormData();
    formData.append('file', selectedFile);

    formData.append("id", idz);
    formData.append("head", head);
    $.ajax({
        type: "POST",
        url: '/api/SettingsSignature',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                Swal.fire({
                    title: "Successful!",
                    text: data.message,
                    icon: "success",
                    showCancelButton: !0,
                    confirmButtonColor: "#3d8ef8"
                });
            } else {
                swal.fire(
                    'Error!',
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

}


function handleFileSelectionaccount(event) {
    const selectedFile = event.target.files[0];

    if (!selectedFile) {
        return;
    }
    var idz = $('#idz').val();
    var head = "Account";
    const formData = new FormData();
    formData.append('file', selectedFile);

    formData.append("id", idz);
    formData.append("head", head);
    $.ajax({
        type: "POST",
        url: '/api/SettingsSignature',
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                Swal.fire({
                    title: "Successful!",
                    text: data.message,
                    icon: "success",
                    showCancelButton: !0,
                    confirmButtonColor: "#3d8ef8"
                });
            } else {
                swal.fire(
                    'Error!',
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