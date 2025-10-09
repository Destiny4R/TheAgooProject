if ($("#flot-area-2").length) {
    var o = [[0, 2], [1, 3], [2, 6], [3, 5], [4, 7], [5, 8], [6, 10]];
    var e = [[0, 1], [1, 2], [2, 5], [3, 3], [4, 5], [5, 6], [6, 9]];

    $.plot($("#flot-area-2"), [
        { data: o, label: "New Customer", color: "#11c46e" },
        { data: e, label: "Returning Customer", color: "#38414a" }
    ], {
        series: {
            lines: { show: true, lineWidth: 0, fill: 0 },
            splines: { show: true, tension: 0.4, lineWidth: 0, fill: 0.8 },
            shadowSize: 0
        },
        points: { show: false },
        legend: {
            position: "ne",
            margin: [0, -32],
            noColumns: 0,
            labelBoxBorderColor: "transparent",
            labelFormatter: function (o, e) { return o + "&nbsp;&nbsp;"; },
            width: 30,
            height: 2
        },
        grid: {
            hoverable: true,
            clickable: true,
            borderColor: "rgba(123, 145, 158, 0.1)",
            borderWidth: 0,
            labelMargin: 5,
            backgroundColor: "transparent"
        },
        yaxis: {
            min: 0,
            max: 15,
            tickColor: "rgba(123, 145, 158, 0.1)",
            font: { color: "#7b919e", size: 10 }
        },
        xaxis: {
            tickColor: "rgba(123, 145, 158, 0.1)",
            font: { color: "#7b919e", size: 10 }
        },
        tooltip: true,
        tooltipOpts: { content: "%s : %y.0", shifts: { x: -30, y: -50 } }
    });
}
if ($("#barChart").length) {
    var ctx = $("#barChart").get(0).getContext("2d");
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep"],
            datasets: [
                {
                    label: "Apple",
                    data: [46, 57, 59, 54, 62, 58, 64, 60, 66],
                    backgroundColor: "#eff2f7"
                },
                {
                    label: "Samsung",
                    data: [74, 83, 102, 97, 86, 106, 93, 114, 94],
                    backgroundColor: "#3d8ef8"
                },
                {
                    label: "Oppo",
                    data: [37, 42, 38, 26, 47, 50, 54, 55, 43],
                    backgroundColor: "#11c46e"
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            scales: {
                yAxes: [
                    {
                        display: false,
                        gridLines: {
                            drawBorder: false
                        },
                        ticks: {
                            fontColor: "#686868"
                        }
                    }
                ],
                xAxes: [
                    {
                        barPercentage: 0.7,
                        categoryPercentage: 0.5,
                        ticks: {
                            fontColor: "#7b919e"
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        }
                    }
                ]
            },
            elements: {
                point: {
                    radius: 0
                }
            }
        }
    });
}


if ($("#spline_area").length) {
    var options = {
        chart: {
            height: 350,
            type: "area"
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: "smooth",
            width: 3
        },
        series: [
            {
                name: "series1",
                data: [34, 40, 28, 52, 42, 109, 100]
            },
            {
                name: "series2",
                data: [32, 60, 34, 46, 34, 52, 41]
            }
        ],
        colors: ["#eff2f7", "#3d8ef8"],
        xaxis: {
            type: "datetime",
            categories: [
                "2018-09-19T00:00:00",
                "2018-09-19T01:30:00",
                "2018-09-19T02:30:00",
                "2018-09-19T03:30:00",
                "2018-09-19T04:30:00",
                "2018-09-19T05:30:00",
                "2018-09-19T06:30:00"
            ]
        },
        grid: {
            borderColor: "#f1f1f1"
        },
        tooltip: {
            x: {
                format: "dd/MM/yy HH:mm"
            }
        }
    };
    (chart = new ApexCharts(document.querySelector("#spline_area"), options)).render();
}

if ($("#column_chart").length) {
    var options = {
        chart: {
            height: 350,
            type: "bar",
            toolbar: {
                show: false
            }
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: "45%",
                endingShape: "rounded"
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 2,
            colors: ["transparent"]
        },
        series: [
            {
                name: "Net Profit",
                data: [46, 57, 59, 54, 62, 58, 64, 60, 66]
            },
            {
                name: "Revenue",
                data: [74, 83, 102, 97, 86, 106, 93, 114, 94]
            },
            {
                name: "Free Cash Flow",
                data: [37, 42, 38, 26, 47, 50, 54, 55, 43]
            }
        ],
        colors: ["#eff2f7", "#3d8ef8", "#11c46e"],
        xaxis: {
            categories: ["Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct"]
        },
        yaxis: {
            title: {
                text: "$ (thousands)"
            }
        },
        grid: {
            borderColor: "#f1f1f1"
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "$ " + val + " thousands";
                }
            }
        }
    };
    (chart = new ApexCharts(document.querySelector("#column_chart"), options)).render();
}

if ($("#lineChart").length) {
    var e = $("#lineChart").get(0).getContext("2d");
    new Chart(e, {
        type: "line",
        data: {
            labels: ["2013", "2014", "2014", "2015", "2016", "2017", "2018", "2019"],
            datasets: [
                {
                    label: "Apple",
                    data: [120, 180, 140, 210, 160, 240, 180, 210],
                    borderColor: ["#0db4d6"],
                    borderWidth: 3,
                    fill: false,
                    pointBackgroundColor: "#ffffff",
                    pointBorderColor: "#0db4d6"
                },
                {
                    label: "Samsung",
                    data: [80, 140, 100, 170, 120, 200, 140, 170],
                    borderColor: ["#7c8a96"],
                    borderWidth: 3,
                    fill: false,
                    pointBackgroundColor: "#ffffff",
                    pointBorderColor: "#7c8a96"
                }
            ]
        },
        options: {
            scales: {
                yAxes: [
                    {
                        gridLines: {
                            drawBorder: false,
                            borderDash: [3, 3],
                            zeroLineColor: "#7b919e"
                        },
                        ticks: {
                            min: 0,
                            color: "#7b919e"
                        }
                    }
                ],
                xAxes: [
                    {
                        gridLines: {
                            display: false,
                            drawBorder: false,
                            color: "#ffffff"
                        }
                    }
                ]
            },
            elements: {
                line: {
                    tension: 0.2
                },
                point: {
                    radius: 4
                }
            },
            stepsize: 1
        }
    });
}

if ($("#flot-line-4").length)
    $.plot($("#flot-line-4"), [
        { data: [[0, 10], [1, 7], [2, 8], [3, 9], [4, 6], [5, 5], [6, 7]], label: "New Customer", color: "#008080" },
        { data: [[0, 8], [1, 5], [2, 6], [3, 8], [4, 4], [5, 3], [6, 6]], label: "Returning Customer", color: "#564ab1" }
    ], {
        series: {
            lines: { show: !1 },
            splines: { show: !0, tension: .4, lineWidth: 2 },
            shadowSize: 0
        },
        points: { show: !0 },
        legend: {
            position: "ne",
            margin: [0, -32],
            noColumns: 0,
            labelBoxBorderColor: null,
            labelFormatter: function (o, e) { return o + "&nbsp;&nbsp;"; },
            width: 30,
            height: 2
        },
        grid: {
            hoverable: !0,
            clickable: !0,
            borderColor: "rgba(123, 145, 158,0.1)",
            borderWidth: 0,
            labelMargin: 5,
            backgroundColor: "transparent"
        },
        yaxis: {
            min: 0,
            max: 15,
            tickColor: "rgba(123, 145, 158,0.1)",
            font: { color: "#7b919e", size: 10 }
        },
        xaxis: {
            tickColor: "rgba(123, 145, 158,0.1)",
            font: { color: "#7b919e", size: 10 }
        },
        tooltip: !0,
        tooltipOpts: { content: "%s : %y.0", shifts: { x: -30, y: -50 } }
    });

$(function () {
    "use strict";
    if ($("#flot-bar-1").length) {
        $.plot($("#flot-bar-1"), [{
            data: [[0, 3], [2, 8], [4, 5], [6, 13], [8, 5], [10, 7], [12, 4], [14, 6]]
        }], {
            series: {
                bars: {
                    show: true,
                    lineWidth: 0,
                    barWidth: 0.3,
                    fillColor: "#0db4d6"
                }
            },
            grid: {
                borderWidth: 1,
                borderColor: "rgba(123, 145, 158,0.1)",
                labelMargin: 15
            },
            yaxis: {
                tickColor: "rgba(123, 145, 158,0.1)",
                font: {
                    color: "#7b919e",
                    size: 10
                }
            },
            xaxis: {
                tickColor: "rgba(123, 145, 158,0.1)",
                font: {
                    color: "#7b919e",
                    size: 10
                }
            },
            tooltip: true,
            tooltipOpts: {
                content: "%s : %y.0",
                shifts: {
                    x: -30,
                    y: -50
                }
            }
        });
    }
});


