$(document).ready(function () {
    function formatDate(value) {
        if (!value) return "";
        var date = new Date(value);
        if (isNaN(date.getTime())) return value;
        var dd = String(date.getDate()).padStart(2, "0");
        var mm = String(date.getMonth() + 1).padStart(2, "0");
        var yyyy = date.getFullYear();
        var hh = String(date.getHours()).padStart(2, "0");
        var min = String(date.getMinutes()).padStart(2, "0");
        return dd + "/" + mm + "/" + yyyy + " " + hh + ":" + min;
    }

    if ($("#paymentTable").length) {
        $("#paymentTable").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "paging": true,
            "responsive": true,
            "ajax": {
                "url": "/students/TermFeesPayment",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0],
                "visible": true,
                "searchable": true,
            }],
            "columns": [
                {
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    "data": "fullName"
                },
                { "data": "fees", "autoWidth": true },
                { "data": "paid", "autoWidth": true },
                { "data": "balance", "autoWidth": true },
                {
                    "data": "bal", "autoWidth": true,
                    render: function (data, type, row) {
                        if (data == 0) {
                            return `<label class="bg-success badge"><i class="bi bi-check2-circle me-2"></i> Completed</label>`;
                        } else {
                            return `<label class="bg-warning badge"><i class="bi bi-x-close me-2"></i> Part Payment</label>`;
                        }
                    }
                },
                { "data": "sessionYear", "autoWidth": true },
                { "data": "term", "autoWidth": true },
                { "data": "className", "autoWidth": true },
                {
                    "data": "createDate", "autoWidth": true,
                    render: function (data, type, row) {
                        return formatDate(data);
                    }
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="d-flex justify-content-between">
                                <a class="btn btn-success btn-sm" href="/ultilities/Fees-Payment-Invoice?id=${data}" style="cursor:pointer" title="Print receipt" target="_blank">
                                    <i class="bi bi-printer"></i>
                                </a>
                            </div>
                        `;
                    }
                },
            ]
        });
    }

    if ($("#fee_payment").length) {
        $("#fee_payment").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            responsive: true,
            destroy: true,
            "ajax": {
                "url": "/students/otherPayment",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{ "visible": true, "searchable": true, }],
            "columns": [
                {
                    "data": "id", render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; }
                },
                {
                    "data": "fullname"
                },
                { "data": "amount", "autoWidth": true },
                { "data": "otherpayitem", "autoWidth": true },
                { "data": "amountpaid", "autoWidth": true },
                {
                    "data": null, "render": function (data, type, full) {
                        if (data.amount2 == data.amountpaid2) {
                            return `<span class="badge h4 bg-success" title="Fees completed">
                        <i class="bi bi-check2-all"></i> Completed
                        </span>`;
                        } else {
                            return `<label title="Part Payment" class="badge h3 bg-warning"><i class="bi bi-x-circle"></i> Part Payment</label>`;
                        }
                    }
                },
                {
                    "data": null, "render": function (data, type, full) {
                        return data.session;
                    }
                },
                {
                    "data": null, "render": function (data, type, full) {
                        return data.term;
                    }
                },
                {
                    "data": null, "render": function (data, type, full) {
                        return data.classes;
                    }
                },
                {
                    "data": "createdate", "autoWidth": true,
                    render: function (data, type, row) {
                        return formatDate(data);
                    }
                }
            ]
        });
    }

    if ($("#ptapayment").length) {
        $("#ptapayment").DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "paging": true,
            "responsive": true,
            "ajax": {
                "url": "/students/PTAFeesPaymentTable",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0],
                "visible": true,
                "searchable": true,
            }],
            "columns": [
                {
                    "data": "id",
                    render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    "data": "fullname"
                },
                { "data": "ptafees", "autoWidth": true },
                { "data": "amount", "autoWidth": true },
                { "data": "balance", "autoWidth": true },
                {
                    "data": "bal", "autoWidth": true,
                    render: function (data, type, row) {
                        if (data == 0) {
                            return `<label class="bg-success badge"><i class="bi bi-check2-circle me-2"></i> Completed</label>`;
                        } else {
                            return `<label class="bg-warning badge">Part Payment</label>`;
                        }
                    }
                },
                { "data": "session", "autoWidth": true },
                { "data": "term", "autoWidth": true },
                { "data": "classname", "autoWidth": true },
                {
                    "data": "createdate", "autoWidth": true,
                    render: function (data, type, row) {
                        return formatDate(data);
                    }
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        <div class="d-flex justify-content-between">
                        <a class="btn btn-primary btn-sm" href="/ultilities/Pta-Payment-Invoice?id=${data}" target="_blank" title="Print receipt">
                                <i class="bi bi-printer"></i>
                            </a>
                        </div>
                    `;
                    }
                },
            ]
        });
    }

});
