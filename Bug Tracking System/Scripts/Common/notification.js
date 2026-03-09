toastr.options = {
    "closeButton": true,
    "progressBar": true,
    "positionClass": "toast-top-center",
    "timeOut": "3000",
    "extendedTimeOut": "1000",
    "preventDuplicates": true
};

function showSuccess(message) {
    toastr.success(message);
}

function showError(message) {
    toastr.error(message);
}

function showWarning(message) {
    toastr.warning(message);
}

function showInfo(message) {
    toastr.info(message);
}