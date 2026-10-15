document.addEventListener("DOMContentLoaded", function () {
    if (!VivaFitApi.requireAuth()) {
        return;
    }

    const table = document.getElementById("planos-table");
    const message = document.getElementById("page-message");

    document
        .querySelectorAll("#logout-button, #mobile-logout")
        .forEach(function (button) {
            button.addEventListener("click", VivaFitApi.logout);
        });

    function escapeHtml(value) {
        return String(value ?? "").replace(/[&<>"']/g, function (char) {
            return {
                "&": "&amp;",
                "<": "&lt;",
                ">": "&gt;",
                '"': "&quot;",
                "'": "&#039;"
            }[char];
        });
    }

    async function load() {
        try {
            const plans = await VivaFitApi.request("/Planos");

            table.innerHTML = plans.length
                ? plans
                      .map(function (plan) {
                          return `<tr><td>${escapeHtml(
                              plan.nome
                          )}</td><td>${escapeHtml(
                              plan.descricao
                          )}</td><td>R$ ${Number(plan.valor || 0)
                              .toFixed(2)
                              .replace(".", ",")}</td><td>${escapeHtml(
                              plan.duracaoMeses
                          )} meses</td><td><span class="status ${
                              plan.ativo === false ? "inactive" : ""
                          }">${
                              plan.ativo === false ? "Inativo" : "Ativo"
                          }</span></td></tr>`;
                      })
                      .join("")
                : '<tr><td colspan="5" class="empty-state">Nenhum plano encontrado.</td></tr>';
        } catch (error) {
            message.textContent = error.message;
            table.innerHTML =
                '<tr><td colspan="5" class="empty-state">Não foi possível carregar os planos.</td></tr>';
        }
    }

    document.getElementById("refresh-planos").addEventListener("click", load);
    load();
});
