(function () {
    'use strict';

    var modalElement = document.getElementById('bulkImportModal');
    if (!modalElement) {
        return;
    }

    var fileInput = document.getElementById('bulkExcelFile');
    var sessionSelect = document.getElementById('bulkSessionYear');
    var previewButton = document.getElementById('bulkImportPreviewBtn');
    var commitButton = document.getElementById('bulkImportCommitBtn');
    var previewBody = document.getElementById('bulkImportPreviewBody');
    var previewWrapper = document.getElementById('bulkImportPreviewWrap');
    var alertBox = document.getElementById('bulkImportAlert');
    var summaryBox = document.getElementById('bulkImportSummary');
    var footerNote = document.getElementById('bulkImportFooterNote');

    var maxFileSize = 5 * 1024 * 1024;
    var previewButtonHtml = '<i class="bi bi-search me-1"></i> Upload &amp; Preview';
    var commitButtonHtml = '<i class="bi bi-check2-circle me-1"></i> Confirm Import';
    var state = {
        busy: false,
        hasPreview: false,
        errorCount: 0,
        successCount: 0
    };

    function escapeHtml(value) {
        if (value === null || value === undefined) {
            return '';
        }

        return String(value).replace(/[&<>"']/g, function (character) {
            return {
                '&': '&amp;',
                '<': '&lt;',
                '>': '&gt;',
                '"': '&quot;',
                "'": '&#39;'
            }[character];
        });
    }

    function showAlert(type, html) {
        alertBox.className = 'alert alert-' + type + ' mt-3';
        alertBox.innerHTML = html;
        alertBox.classList.remove('d-none');
    }

    function hideAlert() {
        alertBox.className = 'alert d-none mt-3';
        alertBox.innerHTML = '';
    }

    function blockUi() {
        if (typeof blockcallback === 'function') {
            blockcallback();
        }
    }

    function unblockUi() {
        if (window.jQuery && typeof window.jQuery.unblockUI === 'function') {
            window.jQuery.unblockUI();
        }
    }

    function updateButtons() {
        previewButton.disabled = state.busy;
        commitButton.disabled = state.busy || !state.hasPreview || state.errorCount > 0;
    }

    function setBusy(isBusy, activeButton) {
        state.busy = isBusy;

        previewButton.innerHTML = isBusy && activeButton === 'preview'
            ? '<span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Checking...'
            : previewButtonHtml;

        commitButton.innerHTML = isBusy && activeButton === 'commit'
            ? '<span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span> Importing...'
            : commitButtonHtml;

        updateButtons();
    }

    function resetPreview() {
        state.hasPreview = false;
        state.errorCount = 0;
        state.successCount = 0;

        previewBody.innerHTML = '';
        previewWrapper.classList.add('d-none');
        summaryBox.className = 'd-none mt-3';
        summaryBox.innerHTML = '';
        footerNote.textContent = '';
        hideAlert();
        updateButtons();
    }

    function fieldClass(row, field, isError) {
        if (!isError || !Array.isArray(row.errorFields)) {
            return '';
        }

        return row.errorFields.indexOf(field) >= 0 ? 'text-danger fw-semibold' : '';
    }

    function renderPreview(data) {
        var rows = Array.isArray(data.rows) ? data.rows : [];
        previewBody.innerHTML = '';

        if (rows.length === 0) {
            state.hasPreview = false;
            state.errorCount = 0;
            state.successCount = 0;
            previewWrapper.classList.add('d-none');
            summaryBox.className = 'd-none mt-3';
            showAlert('warning', 'No student rows were found in the uploaded file.');
            updateButtons();
            return;
        }

        var fragment = document.createDocumentFragment();
        rows.forEach(function (row) {
            var isError = row.status === 'ERROR' || (Array.isArray(row.errors) && row.errors.length > 0);

            var mainRow = document.createElement('tr');
            if (isError) {
                mainRow.classList.add('table-danger');
            }

            mainRow.innerHTML =
                '<td>' + escapeHtml(row.rowNumber) + '</td>' +
                '<td class="' + fieldClass(row, 'surName', isError) + '">' + escapeHtml(row.surName) + '</td>' +
                '<td class="' + fieldClass(row, 'firstName', isError) + '">' + escapeHtml(row.firstName) + '</td>' +
                '<td class="' + fieldClass(row, 'otherName', isError) + '">' + escapeHtml(row.otherName) + '</td>' +
                '<td class="' + fieldClass(row, 'gender', isError) + '">' + escapeHtml(row.gender) + '</td>' +
                '<td class="' + fieldClass(row, 'dateOfBirth', isError) + '">' + escapeHtml(row.dateOfBirth) + '</td>' +
                '<td class="' + fieldClass(row, 'state', isError) + '">' + escapeHtml(row.state) + '</td>' +
                '<td class="' + fieldClass(row, 'localGov', isError) + '">' + escapeHtml(row.localGov) + '</td>' +
                '<td class="' + fieldClass(row, 'address', isError) + '">' + escapeHtml(row.address) + '</td>' +
                '<td>' + (isError
                    ? '<span class="badge text-bg-danger">ERROR</span>'
                    : '<span class="badge text-bg-success">OK</span>') + '</td>';

            fragment.appendChild(mainRow);

            if (isError) {
                var detailRow = document.createElement('tr');
                detailRow.classList.add('table-danger');
                var errorItems = (row.errors || []).map(function (error) {
                    return '<li>' + escapeHtml(error) + '</li>';
                }).join('');
                detailRow.innerHTML =
                    '<td></td>' +
                    '<td colspan="8" class="small">' +
                    '<ul class="mb-0 ps-3">' + errorItems + '</ul>' +
                    '</td>';
                fragment.appendChild(detailRow);
            }
        });

        previewBody.appendChild(fragment);
        previewWrapper.classList.remove('d-none');

        state.errorCount = rows.filter(function (row) {
            return row.status === 'ERROR' || (Array.isArray(row.errors) && row.errors.length > 0);
        }).length;
        state.successCount = rows.length - state.errorCount;
        state.hasPreview = true;

        if (state.errorCount > 0) {
            summaryBox.className = 'alert alert-warning mt-3';
            summaryBox.innerHTML =
                '<i class="bi bi-exclamation-triangle me-1"></i> <strong>' + state.errorCount + '</strong> row(s) have errors. ' +
                'Fix your Excel file and re-upload. ' +
                '<span class="ms-1">' + state.successCount + ' row(s) are ready.</span>';
            footerNote.textContent = 'Fix the highlighted rows before importing.';
        } else {
            summaryBox.className = 'alert alert-success mt-3';
            summaryBox.innerHTML =
                '<i class="bi bi-check2-circle me-1"></i> All <strong>' + state.successCount + '</strong> row(s) passed validation. ' +
                'Click <strong>Confirm Import</strong> to save them.';
            footerNote.textContent = data.sessionName
                ? 'Session: ' + data.sessionName
                : 'Ready to import.';
        }

        updateButtons();
    }

    async function postImportForm(url) {
        var file = fileInput.files && fileInput.files[0];
        var formData = new FormData();
        formData.append('excelFile', file);
        formData.append('sessionYearId', sessionSelect.value);

        var response = await fetch(url, {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        var text = await response.text();
        try {
            return JSON.parse(text);
        } catch (error) {
            throw new Error('The server returned an unexpected response. Refresh the page and try again.');
        }
    }

    function validateSelectedFile() {
        var file = fileInput.files && fileInput.files[0];
        if (!file) {
            showAlert('warning', 'Please choose an Excel file (.xlsx) to import.');
            return false;
        }

        if (!/\.xlsx$/i.test(file.name)) {
            showAlert('warning', 'Only Excel .xlsx files are accepted. Save the workbook as .xlsx and try again.');
            fileInput.value = '';
            return false;
        }

        if (file.size > maxFileSize) {
            showAlert('warning', 'The selected file is larger than the 5MB limit.');
            fileInput.value = '';
            return false;
        }

        return true;
    }

    async function previewFile() {
        if (state.busy) {
            return;
        }

        hideAlert();
        if (!validateSelectedFile()) {
            return;
        }

        if (!sessionSelect.value) {
            showAlert('warning', 'Please select the session/year that these students should be admitted into.');
            sessionSelect.focus();
            return;
        }

        resetPreview();
        setBusy(true, 'preview');
        blockUi();

        try {
            var data = await postImportForm('/api/BulkImportPreview');
            if (!data.success) {
                showAlert('danger', escapeHtml(data.message || 'The file could not be previewed.'));
                return;
            }

            renderPreview(data);
        } catch (error) {
            showAlert('danger', escapeHtml(error.message || 'Something went wrong while reading the file.'));
        } finally {
            setBusy(false, 'preview');
            unblockUi();
        }
    }

    async function commitImport() {
        if (state.busy || commitButton.disabled) {
            return;
        }

        if (!validateSelectedFile() || !state.hasPreview || state.errorCount > 0) {
            return;
        }

        if (typeof Swal !== 'undefined') {
            var confirmation = await Swal.fire({
                title: 'Import students?',
                text: 'This will create ' + state.successCount + ' student account(s) and admission record(s).',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Yes, import',
                cancelButtonText: 'Cancel',
                reverseButtons: true
            });

            if (!confirmation.isConfirmed) {
                return;
            }
        }

        hideAlert();
        setBusy(true, 'commit');
        blockUi();

        try {
            var data = await postImportForm('/api/BulkImportCommit');
            if (data.success) {
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'success',
                        title: 'Import Complete',
                        text: data.message || (data.importedCount + ' student(s) imported.'),
                        confirmButtonText: 'OK'
                    });
                }

                if (typeof datatablez !== 'undefined' && datatablez) {
                    datatablez.ajax.reload(null, false);
                }

                if (typeof bootstrap !== 'undefined') {
                    var modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
                    modalInstance.hide();
                }
            } else {
                showAlert('danger', escapeHtml(data.message || 'The import failed. No records were saved.'));
                if (Array.isArray(data.rows)) {
                    renderPreview({
                        rows: data.rows,
                        errorCount: typeof data.errorCount === 'number' ? data.errorCount : 0,
                        successCount: 0,
                        sessionName: ''
                    });
                }
            }
        } catch (error) {
            showAlert('danger', escapeHtml(error.message || 'Something went wrong while importing. No records were saved.'));
        } finally {
            setBusy(false, 'commit');
            unblockUi();
        }
    }

    fileInput.addEventListener('change', function () {
        resetPreview();
        if (fileInput.files && fileInput.files.length > 0) {
            validateSelectedFile();
        }
    });

    sessionSelect.addEventListener('change', function () {
        if (state.hasPreview) {
            resetPreview();
            showAlert('info', 'Session changed. Preview the file again to refresh the import summary.');
        }
    });

    previewButton.addEventListener('click', previewFile);
    commitButton.addEventListener('click', commitImport);

    modalElement.addEventListener('hidden.bs.modal', function () {
        fileInput.value = '';
        resetPreview();
    });
})();
