$(document).ready(function () {
    $("#loginForm").on("submit", function (e) {
        e.preventDefault();

        var form = $(this);
        var btn = form.find("input[type='submit']");
        btn.prop("disabled", true);

        var user = {
            Email: $("#emailId").val(),
            Password:$("#passwordId").val()
        }

        if (!user.Email || !user.Password) {
            $("#errorMessage").text("Please Enter All Fields");
            btn.prop("disabled", false);
            return;
        }

        $.ajax({
            url: "/Account/Login",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(user),
            success: function (res) {
                if (res.success) {
                    showSuccess(res.message);
                    setTimeout(function () {
                        window.location.href = res.redirectUrl;
                    }, 1500);
                }
                else {
                    showError(res.message || "Something Went Wrong");
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