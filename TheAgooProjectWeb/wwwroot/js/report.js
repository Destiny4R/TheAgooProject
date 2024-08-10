//ADDING AND CALCULATING FARE IN THE TABLE
function adddata() {
    var destination = $("#Daily_DestinationId").val();
    var fare = $("#fare").val();
    var noofperson = $("#noofperson").val();
    var total = $("#totalamount").val();
    var textx = $("#Daily_DestinationId option:selected").text();
    var btnz = '<button type="button" onclick="remove(this);" class="btn btn-danger btn-sm who"><i class="fa fa-trash"></i></button>';

    if (total == "") {
        return swal.fire(
            'Cancelled',
            'Provide all required fields before adding!',
            'error'
        );
    }
    if (destination == null || textx =="Select Station") {
        return swal.fire(
            'Cancelled',
            'Select destination before adding!',
            'error'
        );
    }
    $("#dailyreporter > tbody").append("<tr><td title=" + textx +">" + destination + "</td><td>" + fare + "</td><td>" + noofperson + "</td><td>" + total + "</td><td>" + btnz + "</td></tr>");
    totalbal += Number(total);

    $("#totalincomebal").val(totalbal + additionincome);
    $("#lodgetotalincome").val(totalbal + additionincome);
    $("#netbalance").val(totalbal + additionincome - expenses);

    var inco = $("#totalincomebal").val();

    $("#Daily_Commission").val((percent / 100) * inco);
    
    //netbalan += totalbal - expenses;
    $("#fare").val('');
    $("#noofperson").val('');
    $("#totalamount").val('');
    $("#Daily_DestinationId").focus();
}
//FARE CALCULATOR additionalincome
var netbalan = 0;
var totalbal = 0;
var additionincome = 0;
var expenses = 0;

var percent = $("#Comm_Percent").val();
function sum() {
    var fare = $("#fare").val();
    var noofperson = $("#noofperson").val();
    $("#totalamount").val(fare * noofperson);

};
function additionalincome() {
    var waybill = $("#Daily_LoadWayBill").val();
    var lodgement = $("#Daily_Lodgement").val();
    
    var total = Number(waybill) + Number(lodgement);
    additionincome = total;
    $("#totalincomebal").val(totalbal + total);
    $("#netbalance").val(totalbal + total - expenses);

    var inco = $("#totalincomebal").val();
    

}
function expense() {
    if (totalbal > 0) {
        var fuel = $("#Daily_FuelExpenditure").val();
        var commiz = $("#Daily_Commission").val();
        var repair = $("#Daily_Repairs").val();
        var other = $("#Daily_OtherExpenditure").val();
        var overith = $("#Daily_Overnith").val();

        var inc = Number($("#totalincomebal").val());
        var incomewithoutload = $("#lodgetotalincome").val();
        //CALCULATE DRIVERS SAVINGS IN PERCENTAGE
        var driversaving = Number($("#Driver_Percent").val());
        var driversavingResult = (driversaving / 100) * incomewithoutload;
        $("#Daily_DriverSavings").val(driversavingResult);

        //CALCULATE PARK ALLOWANCE PERCENTAGE
        var driverallow = Number($("#Park_Percent").val());
        var driverallowResult = (driverallow / 100) * incomewithoutload;
        $("#Daily_ParkOperation").val(driverallowResult);


        //ADDING DRIVER'S SAVING AND PARK ALLOWANCES TO EXPENDITURES
        var oldDriver = Number($("#Daily_DriverSavings").val());
        var oldpark = Number($("#Daily_ParkOperation").val());


        expenses = Number(fuel) + Number(repair) + Number(other) + Number(overith) + Number(commiz) + oldDriver + oldpark;


        $("#expenditure").val(expenses);

        netbalan = inc - expenses;
        $("#netbalance").val(inc - expenses);
    }
    else {
        swal.fire(
            'Cancelled',
            'Provide car income before adding expenditures!',
            'error'
        );
        $("#Daily_FuelExpenditure").val(0);
        $("#Daily_Repairs").val(0);
        $("#Daily_OtherExpenditure").val(0);
        $("#Daily_Overnith").val(0);
    }
};

 function postreport () {
    var destinationRoots = new Array();

    var dailyreport = {
        dailycomments: {},
        destinationRoots:[]
    };
    $("#dailyreporter TBODY TR").each(function () {
        var row = $(this);
        var destiny = {};
        destiny.rootDestination = row.find("TD").eq(0).html();
        destiny.Fare = row.find("TD").eq(1).html();
        destiny.PassengerNumber = row.find("TD").eq(2).html();
        destiny.TotalAmount = row.find("TD").eq(3).html();
        dailyreport.destinationRoots.push(destiny);
    });

    dailyreport.NoOfVehLoaded = $("#Daily_NoOfVehLoaded").val();
    dailyreport.Overnith = $("#Daily_Overnith").val();
    dailyreport.Trip = $("#Daily_Trip").val();
    dailyreport.VehicleRegId = $("#VehicleRegId").val();
    dailyreport.NoCarried = $("#Daily_NoCarried").val();
    dailyreport.ReportedDate = $('#Daily_ReportedDate').val();
    dailyreport.Lodgement = $("#Daily_Lodgement").val();
    dailyreport.LoadWayBill = $("#Daily_LoadWayBill").val();
    dailyreport.Commission = $("#Daily_Commission").val();
    dailyreport.FuelExpenditure = $("#Daily_FuelExpenditure").val();
    dailyreport.Repairs = $("#Daily_Repairs").val();
    dailyreport.OtherExpenditure = $("#Daily_OtherExpenditure").val();
    dailyreport.DriversId = $("#DriversId").val();
    dailyreport.TotalIncome = $("#totalincomebal").val(); 
    dailyreport.NetIncome = $("#netbalance").val();
    dailyreport.ParkOperation = $("#Daily_ParkOperation").val();
    dailyreport.DriverSavings = $("#Daily_DriverSavings").val();


    dailyreport.dailycomments.fuelcomment = $("#Daily_DailyComments_FuelComment").val();
    dailyreport.dailycomments.OtherComments = $("#Daily_DailyComments_OtherComments").val();
    dailyreport.dailycomments.repairscomment = $("#Daily_DailyComments_RepairsComment").val();

     var h = 0;
     h = $("#totalincomebal").val();
    if ( h > 0) {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success btn-sm',
                cancelButton: 'btn btn-danger btn-sm'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({
            title: "Submit This Report!",
            text: "Confirm every information before submitting this report!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, Submit!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: 'POST',
                    url: '/api/DataReport',
                   contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: dailyreport,
                    cache: false,
                    success: function (data) {
                        $.unblockUI();
                        if (data.success) {
                            swalWithBootstrapButtons.fire(
                                'Report Information!',
                                data.message,
                                'success'
                            );
                            window.location.replace("/DailyReports/Index");
                           // location.reload('/DailyReports/Add-Report');
                            
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: data.message,
                                footer: 'Outcome Server Report'
                            });
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
                    'Report Not Submitted!',
                    'Information'
                );
            }
        });
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'All selected fields are required!',
            footer: 'Select and try again!'
        });
    }
    
};



//REMOVING ALREADY ADDED FARE IF NEED BE
function remove(button) {
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    var income = Number($("TD", row).eq(3).html());
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to delete " + name+"?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '$success',
        cancelButtonColor: '$danger',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            //When remove an already calculated destination, subtract it from the net income too.
            var netb = 0, totalic = 0;

            totalic = $("#totalincomebal").val();
            netb = $("#netbalance").val();
            totalic -= income;
            netb -= income;
            //global val..
            totalbal -= income;
            netbalan -= income;


            var driver = 0;
            var park = 0;

            var driversaving = Number($("#Driver_Percent").val());
            var driverallow = Number($("#Park_Percent").val());

            
            driver = (driversaving / 100) * income;


            park = (driverallow / 100) * income;

            if (driver > 0 && park > 0) {
                var oldDriver = $("#Daily_DriverSavings").val();
                var oldpark = $("#Daily_ParkOperation").val();

                $("#Daily_DriverSavings").val(oldDriver - driver);
                $("#Daily_ParkOperation").val(oldpark - park);

                var loads = $("#lodgetotalincome").val();
                $("#lodgetotalincome").val(loads - income);
            }
            $("#netbalance").val(netb);
            $("#totalincomebal").val(totalic);

            var table = $("#dailyreporter")[0];
            table.deleteRow(row[0].rowIndex);
            Swal.fire(
                'Deleted!',
                name +' destination removed!',
                'success'
            )
        }
    });
};
//MANAGING FORM WIZARD ACTIVITIES
$(function () {
    "use strict";

    $("#form-horizontal").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide",
        onStepChanged: function (event, currentIndex, priorIndex) {
            if (currentIndex === 3) {
                $('#form-horizontal').find('a[href="#finish"]').remove();
                
                $('#form-horizontal .actions li:last-child').append('<button type="button" onclick="postreport();" id="submitreport" class="btn btn-primary btn-sm"><i class="far fa-paper-plane"></i> Submit Report</button>');
            }
            if (currentIndex === 2) { 
                $("#submitreport").remove();
            }
        }
    });

    $('#Daily_ReportedDate').bootstrapMaterialDatePicker({
        weekStart: 0, time: false
    });
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
