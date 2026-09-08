document.addEventListener("DOMContentLoaded", function () {

    const languageButton =
        document.getElementById("languageButton");

    const themeButton =
        document.getElementById("themeButton");

    const passwordToggle =
        document.getElementById("passwordToggle");

    const passwordInput =
        document.getElementById("password");

    const loginForm =
        document.getElementById("loginForm");

    const loginMessage =
        document.getElementById("loginMessage");


    // Language

    languageButton.addEventListener("click", function () {

        const currentLanguage =
            document.documentElement.lang;

        const newLanguage =
            currentLanguage === "ar"
                ? "en"
                : "ar";

        const returnUrl =
            window.location.pathname +
            window.location.search;

        fetch("/Auth/SetLanguage", {
            method: "POST",

            headers: {
                "Content-Type":
                    "application/x-www-form-urlencoded"
            },

            body: new URLSearchParams({
                culture: newLanguage,
                returnUrl: returnUrl
            })
        })
            .then(response => {

                if (response.ok) {
                    window.location.reload();
                }

            });

    });


    // Dark mode

    const savedTheme =
        localStorage.getItem("theme");

    if (savedTheme === "dark") {

        document.body.classList.add("dark-mode");

        themeButton.textContent = "Light Mode";
    }


    themeButton.addEventListener("click", function () {

        document.body.classList.toggle("dark-mode");

        const isDark =
            document.body.classList.contains("dark-mode");

        localStorage.setItem(
            "theme",
            isDark ? "dark" : "light"
        );

        themeButton.textContent =
            isDark
                ? "Light Mode"
                : "Dark Mode";

    });


    // Show or hide password

    passwordToggle.addEventListener("click", function () {

        if (passwordInput.type === "password") {

            passwordInput.type = "text";

            passwordToggle.textContent = "Hide";

        }
        else {

            passwordInput.type = "password";

            passwordToggle.textContent = "Show";

        }

    });


    // Login

    loginForm.addEventListener("submit", async function (event) {

        event.preventDefault();

        const email =
            document.getElementById("email").value;

        const password =
            document.getElementById("password").value;


        const response =
            await fetch("/Auth/Login", {

                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify({
                    email: email,
                    password: password
                })

            });


        const responseText =
            await response.text();


        console.log("Status:", response.status);
        console.log("Response:", responseText);


        if (response.ok) {

            loginMessage.textContent =
                "Login successful!";

            loginMessage.className =
                "success-message";

        }
        else {

            loginMessage.textContent =
                "Invalid email or password.";

            loginMessage.className =
                "error-message";

        }

    });

});