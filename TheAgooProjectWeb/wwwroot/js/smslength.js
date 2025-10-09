
function countincrement() {
    $("#smsData_Message").keyup(function () {
        var counter = $("#smscharcount").text($(this).val().length);
        var moreCharater = document.getElementById("smsData_Message");
        var length = moreCharater.value.length;

        if (length > 800) {
            document.getElementById("smspage").innerHTML = "MMS";
        } else {
            if (length > 640) {
                document.getElementById("smspage").innerHTML = "page/5";
            } else {
                if (length > 480) {
                    document.getElementById("smspage").innerHTML = "page/4";
                } else {
                    if (length > 320) {
                        document.getElementById("smspage").innerHTML = "page/3";
                    } else {
                        if (length > 160) {
                            document.getElementById("smspage").innerHTML = "page/2";
                        } else {
                            if (length < 160 || length == 160) {
                                document.getElementById("smspage").innerHTML = "page/1";
                            }
                        }
                    }
                }
            }
        }
    });
}