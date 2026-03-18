$(function () {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;

    $("#emailId").on("input", function () {
        const email = $(this).val();

        if (email.length === 0) {
            $(this).removeClass("is-valid is-invalid");
            return;
        }

        if (emailRegex.test(email)) {
            $(this).removeClass("is-invalid").addClass("is-valid");
        } else {
            $(this).removeClass("is-valid").addClass("is-invalid");
        }
    });

    $("#passwordId").on("input", function () {
        const password = $(this).val();

        if (password.length === 0) {
            $(this).removeClass("is-valid is-invalid");
            return;
        }

        if (passwordRegex.test(password)) {
            $(this).removeClass("is-invalid").addClass("is-valid");
        } else {
            $(this).removeClass("is-valid").addClass("is-invalid");
        }
    });

    $("#signupForm").on("submit", function (e) {
        //alert("Hello");
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
        if (!emailRegex.test(user.Email)) {
            showError("Enter a valid email");
            btn.prop("disabled", false);
            return;
        }

        if (!passwordRegex.test(user.Password)) {
            showError("Password must contain uppercase, lowercase and number (min 6 chars)");
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