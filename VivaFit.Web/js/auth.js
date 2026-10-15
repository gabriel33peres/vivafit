document.addEventListener("DOMContentLoaded", function () {
    if (VivaFitApi.getToken()) {
        window.location.href = "dashboard.html";
        return;
    }

    const form = document.getElementById("login-form");
    const message = document.getElementById("login-message");

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        message.textContent = "Entrando...";

        const email = form.email.value.trim();
        const senha = form.senha.value;

        try {
            const response = await fetch(
                "http://localhost:5089/api/Usuarios/login?email=" +
                    encodeURIComponent(email) +
                    "&senha=" +
                    encodeURIComponent(senha),
                {
                    method: "POST"
                }
            );

            const data = await response.json();

            if (!response.ok) {
                throw new Error(
                    typeof data === "string"
                        ? data
                        : data.title || "E-mail ou senha inválidos."
                );
            }

            localStorage.setItem("vivafit_token", data.token);
            window.location.href = "dashboard.html";
        } catch (error) {
            message.textContent =
                error.message || "Não foi possível realizar o login.";
        }
    });
});
