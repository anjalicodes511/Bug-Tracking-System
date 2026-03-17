$(function () {

    $("#forgotPasswordForm").on("submit", function (e) {

        e.preventDefault();

        var form = $(this);
        var btn = form.find("button[type='submit']");
        btn.prop("disabled", true);

        var email = $("#emailId").val();

        if (!email) {
            showError("Email is required");
            btn.prop("disabled", false);
            return;
        }

        $.ajax({
            url: "/Account/ForgotPassword",
            type: "POST",
            data: { email: email },

            success: function (res) {
                //console.log(res);
                if (res.success) {
                    showSuccess(res.message);

                    setTimeout(function () {
                        //alert(res.redirectUrl);
                        window.location.href = res.redirectUrl;
                    }, 1500);
                }
                else {
                    showError(res.message || "Failed to send OTP");
                }
            },

            error: function (xhr) {
                var response = xhr.responseJSON;

                if (response && response.message) {
                    showError(response.message);
                } else {
                    showError("Unexpected error occurred");
                }
            },

            complete: function () {
                btn.prop("disabled", false);
            }

        });

    });

});