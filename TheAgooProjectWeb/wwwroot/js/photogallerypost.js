
$(document).ready(function () {
    $(document).ready(function () {
        $("#senddata").on("click", function (event) {
            event.preventDefault();

            var formData = new FormData($("#formgallery")[0]);
            $.ajax({
                type: "POST",
                url: "https://localhost:44327/api/GalleryPhotos",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swal.fire('Information!', data.message, 'success');
                        $("#formgallery")[0].reset();
                    } else {
                        swal.fire('Information!', data.message, 'error');
                    }
                },
                beforeSend: function () {
                    blockcallback();
                },
                error: function (error) {

                    function handleError() {
                        $.unblockUI();
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong!',
                            footer: 'Check internet connectivity'
                        });
                    }
                    handleError();
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        });
    });
});

function photoGalleryRemover(button) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })
    var dataId = $(button).data("id");

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
                url: "https://localhost:44348/api/SinglePhotoManager/" + dataId,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        var divremove = 'gallery_' + dataId;
                        $("#" + divremove).remove();
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
                'Photo Gallery NOT Deleted',
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