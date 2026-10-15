(function () {
    const API_BASE_URL = "http://localhost:5000/api";

    window.VivaFitApi = {
        getToken: function () {
            return localStorage.getItem("vivafit_token");
        },

        requireAuth: function () {
            if (!this.getToken()) {
                window.location.href = "index.html";
                return false;
            }

            return true;
        },

        logout: function () {
            localStorage.removeItem("vivafit_token");
            window.location.href = "index.html";
        },

        request: async function (path, options) {
            if (!this.getToken()) {
                this.requireAuth();
                throw new Error("Sessão expirada.");
            }

            const config = options || {};
            config.headers = Object.assign(
                {
                    "Content-Type": "application/json",
                    Authorization: "Bearer " + this.getToken()
                },
                config.headers || {}
            );

            const response = await fetch(API_BASE_URL + path, config);

            if (response.status === 401) {
                this.logout();
                throw new Error("Sua sessão expirou. Faça login novamente.");
            }

            let data = null;
            const text = await response.text();

            if (text) {
                try {
                    data = JSON.parse(text);
                } catch (_) {
                    data = text;
                }
            }

            if (!response.ok) {
                const detail =
                    data && data.title
                        ? data.title
                        : typeof data === "string"
                            ? data
                            : "Não foi possível concluir a solicitação.";

                throw new Error(detail);
            }

            return data;
        }
    };
})();
