$("#otpForm").submit(function (e) {
    e.preventDefault();

    var btn = form.find("input[type='submit']");
    btn.prop("disabled", true);

    var otp = $("#otpId").val();
    if (!otp) {
        alert("Please Enter OTP");
        btn.prop("disabled", false);
        return;
    }
    $.ajax({
        url: "/Account/VerifyOtp",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(otp),
        success: function (res) {
            if (res.success) {
                alert(res.message);
                window.location.href = "/Account/Login";
            }
            else {
                alert(res.message || "OTP Verification failed");
            }
        },
        error: function (xhr) {
            console.error(xhr.responseText);
            alert("Unexpected error occurred");
        },
        complete: function () {
            btn.prop("disabled", false);
        }
    });
});