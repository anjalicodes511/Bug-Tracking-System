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
            alert("All fields are required");
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
                    alert(res.message);
                    window.location.href = "/Account/VerifyOtp";
                }
                else {
                    alert(res.message || "Signup failed");
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

});