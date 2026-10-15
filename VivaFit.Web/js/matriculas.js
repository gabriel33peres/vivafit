document.addEventListener("DOMContentLoaded", function () {
    if (!VivaFitApi.requireAuth()) {
        return;
    }

    const table = document.getElementById("matriculas-table");
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

    function date(value) {
        if (!value) {
            return "—";
        }

        return new Date(value).toLocaleDateString("pt-BR");
    }

    async function load() {
        try {
            const rows = await VivaFitApi.request("/Matriculas");

            table.innerHTML = rows.length
                ? rows
                      .map(function (row) {
                          return `<tr><td>Aluno #${escapeHtml(
                              row.alunoId
                          )}</td><td>Plano #${escapeHtml(
                              row.planoId
                          )}</td><td>${date(
                              row.dataInicio
                          )}</td><td>${date(
                              row.dataFim
                          )}</td><td><span class="status">${escapeHtml(
                              row.status
                          )}</span></td></tr>`;
                      })
                      .join("")
                : '<tr><td colspan="5" class="empty-state">Nenhuma matrícula encontrada.</td></tr>';
        } catch (error) {
            message.textContent = error.message;
            table.innerHTML =
                '<tr><td colspan="5" class="empty-state">Não foi possível carregar as matrículas.</td></tr>';
        }
    }

    document
        .getElementById("refresh-matriculas")
        .addEventListener("click", load);

    load();
});
