document.addEventListener("DOMContentLoaded", async function () {
    if (!VivaFitApi.requireAuth()) {
        return;
    }

    document
        .querySelectorAll("#logout-button, #mobile-logout")
        .forEach(function (button) {
            button.addEventListener("click", VivaFitApi.logout);
        });

    const resources = [
        ["/Alunos", "total-alunos"],
        ["/Planos", "total-planos"],
        ["/Matriculas", "total-matriculas"],
        ["/Pagamentos", "total-pagamentos"]
    ];

    await Promise.all(
        resources.map(async function (item) {
            try {
                const data = await VivaFitApi.request(item[0]);
                document.getElementById(item[1]).textContent = Array.isArray(
                    data
                )
                    ? data.length
                    : "—";
            } catch (_) {
                document.getElementById(item[1]).textContent = "—";
            }
        })
    );
});
