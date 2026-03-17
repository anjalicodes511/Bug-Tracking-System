//$(document).ready(function () {

//    setTimer();

//    $("#otpForm").submit(function (e) {
//        alert("hi1");
//        e.preventDefault();

//        var form = $(this);
//        var btn = form.find("button[type='submit']");

//        btn.prop("disabled", true);

//        var otp = $("#otpId").val();

//        if (!otp) {
//            showWarning("Please Enter OTP")
//            btn.prop("disabled", false);
//            return;
//        }

//        $.ajax({
//            url: "/Account/VerifyOtp",
//            type: "POST",
//            contentType: "application/json",
//            dataType: "json",
//            data: JSON.stringify({ otp: otp }),

//            success: function (res) {
//                alert(res);
//                if (res.success) {
//                    showSuccess(res.message);
//                    window.location.href = res.redirectUrl;
//                }
//                else {
//                    showError(res.message || "OTP Verification failed");
//                }
//            },

//            error: function () {
//                showError("Unexpected error occurred");
//            },

//            complete: function () {
//                btn.prop("disabled", false);
//            }
//        });
//    });


//    $("#resendBtn").on("click", function () {

//        $("#resendBtn").hide();
//        $("#resendTimer").show();

//        setTimer();

//        $.ajax({
//            url: "/Account/ResendOtp",
//            type: "POST",

//            success: function (res) {

//                if (res.success) {
//                    showSuccess("OTP resent successfully");


//                    alert("OTP resent successfully");
//                    window.location.href = res.redirectUrl;
//                }
//                else {
//                    showError(res.message || "Failed to resend OTP");
//                }
//            },

//            error: function () {
//                showError("Error while resending OTP");
//            }
//        });

//    });

//});


//function setTimer() {

//    var timeLeft = 60;

//    $("#timer").text(timeLeft);

//    var timer = setInterval(function () {

//        timeLeft--;

//        $("#timer").text(timeLeft);

//        if (timeLeft <= 0) {

//            clearInterval(timer);

//            $("#resendTimer").hide();
//            $("#resendBtn").show();
//        }

//    }, 1000);
//}