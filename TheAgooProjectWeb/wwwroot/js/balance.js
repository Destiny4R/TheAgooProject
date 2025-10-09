$(document).ready(function () {
    var mylipro = $('#balpro');
    var mylipro2 = $('#balpro2');
        $.ajax({
            type: "GET",
            url: "/webapi/Balance",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (data) {
                const obj = JSON.parse(data);
                if (obj != null) {
                    mylipro.append('<a href="https://portal.nigeriabulksms.com/" target="_blank" title="Login to Bulk SMS portal for more information" class="notification text-dark mt-3">SMS Credit (' + obj.currency + obj.balance + ') </a > '); 
                    mylipro2 = mylipro2.append('<span>'+ obj.currency + obj.balance +'</span>');
                }
            },
            complete: function () {
            },
            error: function () {
                mylipro.append('<a href="javascript:;" class="notification text-dark">SMS Credit.(N/A) </a > ');
            }
        });
});

