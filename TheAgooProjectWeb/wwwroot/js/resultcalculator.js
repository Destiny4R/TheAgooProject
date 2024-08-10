$("body").on("click", "#btnAdd", function () {
    //Reference the Name and Country TextBoxes.
    var subjecttext = $("#subject option:selected").text();
    var subjectvalue = $("#subject option:selected").val();//$("#subject").val();
    var assign = $("#assign").val();
    var proj = $("#proj").val();
    var test = $("#test").val();
    var cwork = $("#cwork").val();
    var exams = $("#exams").val();
    var total = $("#total").val();
    var grade = $("#grade").val();
    var remark = $("#remark").val();
    

    if (subject === '' || assign === '' || proj === '' || test === '' || cwork === '' || exams === '') {
        return Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Provide all scores before adding!',
            footer: 'Missing Scores'
        });
    }
    //Get the reference of the Table's TBODY element.
    var tBody = $("#resultcalculatortb > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add subject cell.
    var cell = $(row.insertCell(-1));
    cell.html(subjecttext);
    //Add assign cell.
    cell = $(row.insertCell(-1));
    cell.html(assign);
    //Add cwork cell.
    cell = $(row.insertCell(-1));
    cell.html(cwork);
    //Add second test cell.
    cell = $(row.insertCell(-1));
    cell.html(test);
    //Add first test cell.
    cell = $(row.insertCell(-1));
    cell.html(proj);
    //Add exams cell.
    cell = $(row.insertCell(-1));
    cell.html(exams);
    //Add total cell.
    cell = $(row.insertCell(-1));
    cell.html(total);
    //Add grade cell.
    cell = $(row.insertCell(-1));
    cell.html(grade);
    //Add remark cell.
    cell = $(row.insertCell(-1));
    cell.html(remark);
    //Add Subject code cell.
    cell = $(row.insertCell(-1));
    cell.html(subjectvalue);

    //Add Remove cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<button><i class='fa fa-trash'></i></button>");
    btnRemove.attr("type", "button");
    btnRemove.attr("class", "btn btn-sm btn-danger");
    btnRemove.attr("onclick", "Remove(this);");
    //btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    $("#assign").val("");
    $("#proj").val("");
    $("#test").val("");
    $("#cwork").val("");
    $("#exams").val("");
    $("#total").val("");
    $("#grade").val("");
    $("#remark").val("");
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    //if (confirm("Do you want to delete: " + name)) {
    //    //Get the reference of the Table.
    //    var table = $("#resultcalculatortb")[0];

    //    //Delete the Table row using it's Index.
    //    table.deleteRow(row[0].rowIndex);
    //}
    Swal.fire({
        title: "Do you want to remove "+name+"?",
        showDenyButton: true,
        confirmButtonText: "Yes, Remove!",
        denyButtonText: `Cancel`
    }).then((result) => {
        if (result.isConfirmed) {
            var table = $("#resultcalculatortb")[0];
            table.deleteRow(row[0].rowIndex);
            Swal.fire("Deleted!", name+ " successfully removed", "success");
        } else if (result.isDenied) {
            Swal.fire(name+" not deleted", "", "info");
        }
    });
};

$("body").on("click", "#btnSave", function () {

    //Loop through the Table rows and build a JSON array.
    var resultsExams = new Array();
    $("#resultcalculatortb TBODY TR").each(function () {
        var row = $(this);
        var result = {};
        result.subjectsCode = row.find("TD").eq(9).html();
        result.assignment = row.find("TD").eq(1).html();
        result.test = row.find("TD").eq(2).html();
        result.project = row.find("TD").eq(3).html();
        result.classWork = row.find("TD").eq(4).html();
        result.examination = row.find("TD").eq(5).html();
        result.total = row.find("TD").eq(6).html();
        result.grade = row.find("TD").eq(7).html();
        result.remark = row.find("TD").eq(8).html();
        result.termRegID = $("#termreg_Id").val();

        resultsExams.push(result);
    });
    var JsonData = {
        "resultsExams": resultsExams
    }
    if (Object.keys(JsonData.resultsExams).length < 1) {
        return Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'No data added to be posted!',
            footer: 'Missing Scores'
        });
    }
    //Send the JSON array to Controller using AJAX.

    $.ajax({
        type: "POST",
        url: "/api/SendResult",
        data: JsonData,//JSON.stringify(JsonData),
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            // Show loading indicator
            blockcallback();
        },
        success: function (data) {
            if (data.success) {
                // Handle successful response
                swal.fire('Result Uploading Information!', data.message, 'success');
                $.unblockUI();
                //$("#thedivdata").remove();
            } else {
                // Handle error response
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: data.message,
                    footer: 'Result Uploading Fail'
                });
            }
        },
        error: function () {
            // Handle general error (e.g., network issues)
            $.unblockUI();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                footer: 'Check internet connectivity'
            });
        },
        complete: function () {
            // Hide loading indicator
            $.unblockUI();
        }
    });
});
function sum() {
    var noelect = $("#noelective").val();
    var assign = document.getElementById("assign").value;
    var project = document.getElementById("proj").value;
    var tests = document.getElementById("test").value;
    var cw = document.getElementById("cwork").value;
    var Exams = document.getElementById("exams").value;
    var sum = Number(assign) + Number(project) + Number(tests) + Number(cw) + Number(Exams);
    document.getElementById("total").value = sum;
    if (sum < 40) {
        document.getElementById("grade").value = 'E';
        document.getElementById("remark").value = "Poor";
    }
    else if (sum < 56) {
        document.getElementById("grade").value = 'D';
        document.getElementById("remark").value = "Fair";
    }
    else if (sum < 66) {
        document.getElementById("grade").value = 'C';
        document.getElementById("remark").value = "Good";
    }
    else if (sum < 75) {
        document.getElementById("grade").value = 'B';
        document.getElementById("remark").value = "Very Good";
    }
    if (sum > 74) {
        document.getElementById("grade").value = 'A';
        document.getElementById("remark").value = "Distintction";
    }
    //ASSIGNMENT FOR ELECTIVE AND NON ELECTIVE
    if (noelect == "noelective") {
        if (assign > 10) {
            document.getElementById("assign").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Assignment should not be greater than 10, check and try again',
                footer: 'Over assigned scores'
            });
        }
    } else {
        if (assign > 5) {
            document.getElementById("assign").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Assignment should not be greater than 5, check and try again',
                footer: 'Over assigned scores'
            });
        }
    }
    //CLASS WORK FOR ELECTIVE AND NON ELECTIVE
    if (noelect == "noelective") {
        if (cw > 10) {
            document.getElementById("cwork").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Class work should not be greater than 10, check and try again',
                footer: 'Over assigned scores'
            });
        }
    } else {
        if (cw > 5) {
            document.getElementById("cwork").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Class work should not be greater than 5, check and try again',
                footer: 'Over assigned scores'
            });
        }
    }


    if (tests > 10) {
        document.getElementById("total").value = '';
        document.getElementById("test").value = '';
        return Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Test scores should not be greater than 10, check and try again',
            footer: 'Over assigned scores'
        });
    }
    if (project > 10) {
        document.getElementById("total").value = '';
        document.getElementById("proj").value = '';
        return Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Project scores should not be greater than 10, check and try again',
            footer: 'Over assigned scores'
        });
    }
    //EXAMINATION FOR ELECTIVE AND NON ELECTIVE
    if (noelect == "noelective") {
        if (Exams > 60) {
            document.getElementById("exams").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Examination should not be greater than 60, check and try again',
                footer: 'Over assigned scores'
            });
        }
    } else {
        if (Exams > 70) {
            document.getElementById("exams").value = '';
            document.getElementById("total").value = '';
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Examination should not be greater than 70, check and try again',
                footer: 'Over assigned scores'
            });
        }
    }

    if (sum > 100) {
        document.getElementById("total").value = '';
        document.getElementById("exams").value = '';
        return alert('Total scores can not be morethan 100, check and try again');
    }
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