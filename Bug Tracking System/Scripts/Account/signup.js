$(function () {

    $("#signupForm").on("submit", function (e) {

        e.preventDefault();

        var form = $(this);
        var btn = form.find("input[type='submit']");
        btn.prop("disabled", true);

        var user = {
            Name: $("#nameId").val(),
            Email: $("#emailId").val(),
            Password: $("#passwordId").val()
        };

        if (!user.Name || !user.Email || !user.Password) {
            showError("All fields are required");
            //alert("All fields are required");
            btn.prop("disabled", false);
            return;
        }
        $.ajax({
            url: "/Account/Signup",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(user),

            success: function (res) {
                if (res.success) {
                    showSuccess(res.message);
                    //alert(res.message);
                    setTimeout(function () {
                        window.location.href = res.redirectUrl;
                    }, 1500);
                }
                else {
                    showError(res.message || "Signup failed");
                    //alert(res.message || "Signup failed");
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