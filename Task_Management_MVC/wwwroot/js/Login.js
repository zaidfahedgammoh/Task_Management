document.addEventListener("DOMContentLoaded", function () {

    const languageButton = document.getElementById("languageButton");
    const themeButton = document.getElementById("themeButton");

    languageButton.addEventListener("click", function () {

        const currentLanguage = document.documentElement.lang;

        const newLanguage = currentLanguage === "ar"
            ? "en"
            : "ar";

        const returnUrl =
            window.location.pathname + window.location.search;

        fetch("/Auth/SetLanguage", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
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

    const savedTheme = localStorage.getItem("theme");

    if (savedTheme === "dark") {
        document.body.classList.add("dark-mode");
        themeButton.textContent = "☀️";
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
            isDark ? "☀️" : "🌙";

    });

});
const loginForm = document.getElementById("loginForm");
const loginMessage = document.getElementById("loginMessage");

loginForm.addEventListener("submit", async function (event) {

    event.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const response = await fetch("/Auth/Login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
    });
    const responseText = await response.text();

    console.log("Status:", response.status);
    console.log("Response:", responseText);

    if (response.ok) {

        loginMessage.textContent = "Login successful!";
        loginMessage.className = "mt-3 text-center text-success";

    } else {

        loginMessage.textContent = "Invalid email or password.";
        loginMessage.className = "mt-3 text-center text-danger";

    }
});