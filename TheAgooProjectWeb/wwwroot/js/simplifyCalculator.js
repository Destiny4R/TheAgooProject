$(document).ready(function () {
    $("#resultcalculatorSm tbody tr").on("keyup", "input[type='number']", function () {
        var noelect = $("#noelective").val();
        // Get the current row's "Test", "Project", and "Exams" input elements
        const assign = $(this).closest("tr").find("#assign");
        const project = $(this).closest("tr").find("#proj");
        const classWork = $(this).closest("tr").find("#cwork");
        const test = $(this).closest("tr").find("#test");
        const exam = $(this).closest("tr").find("#exams");

        // Get the values from the inputs and convert them to numbers
        const assignValue = parseFloat(assign.val()) || 0;
        const projectValue = parseFloat(project.val()) || 0;
        const testValue = parseFloat(test.val()) || 0;
        const classworkValue = parseFloat(classWork.val()) || 0;
        const examsValue = parseFloat(exam.val()) || 0;

        // Calculate the total
        const total = assignValue + testValue + projectValue + classworkValue + examsValue;

        // Update the "Total" input with the calculated value
        $(this).closest("tr").find("#total").val(total);
        $(this).closest("tr").find("#grade").val(getGrade(total));
        $(this).closest("tr").find("#remark").val(getRemark(total));



        //ASSIGNMENT FOR ELECTIVE AND NON ELECTIVE
        if (noelect == "noelective") {
            if (assignValue > 10) {
                $(this).closest("tr").find("#assign").val(0);
                $(this).closest("tr").find("#total").val(0);
                return Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Assignment should not be greater than 10, check and try again',
                    footer: 'Over assigned scores'
                });
            }
        } else {
            if (assignValue > 5) {
                $(this).closest("tr").find("#assign").val(0);
                $(this).closest("tr").find("#total").val(0);
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
            if (classworkValue > 10) {
                $(this).closest("tr").find("#cwork").val(0);
                $(this).closest("tr").find("#total").val(0);
                return Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Class work should not be greater than 10, check and try again',
                    footer: 'Over assigned scores'
                });
            }
        } else {
            if (classworkValue > 5) {
                $(this).closest("tr").find("#cwork").val(0);
                $(this).closest("tr").find("#total").val(0);
                return Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Class work should not be greater than 5, check and try again',
                    footer: 'Over assigned scores'
                });
            }
        }



        if (testValue > 10) {
            $(this).closest("tr").find("#test").val(0);
            $(this).closest("tr").find("#total").val(0);
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Test scores should not be greater than 10, check and try again',
                footer: 'Over assigned scores'
            });
        }
        if (projectValue > 10) {
            $(this).closest("tr").find("#proj").val(0);
            $(this).closest("tr").find("#total").val(0);
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Project scores should not be greater than 10, check and try again',
                footer: 'Over assigned scores'
            });
        }
        //EXAMINATION FOR ELECTIVE AND NON ELECTIVE
        if (noelect == "noelective") {
            if (examsValue > 60) {
                $(this).closest("tr").find("#exams").val(0);
                $(this).closest("tr").find("#total").val(0);
                return Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Examination should not be greater than 60, check and try again',
                    footer: 'Over assigned scores'
                });
            }
        } else {
            if (examsValue > 70) {
                $(this).closest("tr").find("#exams").val(0);
                $(this).closest("tr").find("#total").val(0);
                return Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Examination should not be greater than 70, check and try again',
                    footer: 'Over assigned scores'
                });
            }
        }


        if (total > 100) {
            $(this).closest("tr").find("#exams").val(0);
            $(this).closest("tr").find("#total").val(0);
            return alert('Total scores can not be morethan 100, check and try again');
        }
    });
    function getGrade(total) {
        if (total >= 75) {
            return "A";
        } else if (total >= 65) {
            return "B";
        } else if (total >= 55) {
            return "C";
        } else if (total >= 40) {
            return "D";
        } else {
            return "E";
        }
    }
    function getRemark(total) {
        if (total >= 75) {
            return "Distinction";
        } else if (total >= 65) {
            return "Very Good";
        } else if (total >= 55) {
            return "Good";
        } else if (total >= 40) {
            return "Fair";
        } else {
            return "Poor";
        }
    }

    $("#btnSave").click(function () { 
        var result = new Array();
        $("#resultcalculatorSm tbody tr").each(function () {
            var results = {};
                results.Assignment = $(this).find("#assign").val();
                results.Test = $(this).find("#test").val();
                results.project = $(this).find("#proj").val();
                results.classWork = $(this).find("#cwork").val();
                results.examination = $(this).find("#exams").val();
                results.total = $(this).find("#total").val();
                results.grade = $(this).find("#grade").val();
                results.remark = $(this).find("#remark").val();
                results.termRegId = $("#termregId").val();
                results.subjectsCode = $(this).find("#subjectcode").val();
            //};
            result.push(results);
        });
        if (Object.keys(result).length < 1) {
            return Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'No data added to be posted!',
                footer: 'Missing Scores'
            });
        }
            $.ajax({
                type: "POST",
                url: "/api/SendResult", 
                data: result,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                cache: false,
                beforeSend: function () {
                    // Show image container
                    blockcallback();
                },
                success: function (data) {
                    if (data.success) {
                        swal.fire(
                            'Result Uploading Information!',
                            data.message,
                            'success'
                        );
                        $.unblockUI();
                        $("#thedivdata").remove();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: data.message,
                            footer: 'Result Uploading Fail'
                        });
                        //location.reload();
                    }
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
                    // Hide image container
                    $.unblockUI();
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                        footer: 'Check internet connectivity'
                    });
                }
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
