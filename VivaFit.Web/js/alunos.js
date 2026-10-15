document.addEventListener("DOMContentLoaded", function () {
    if (!VivaFitApi.requireAuth()) {
        return;
    }

    const table = document.getElementById("alunos-table");
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

    function render(items) {
        if (!items.length) {
            table.innerHTML =
                '<tr><td colspan="6" class="empty-state">Nenhum aluno encontrado.</td></tr>';
            return;
        }

        table.innerHTML = items
            .map(function (aluno) {
                const active = aluno.ativo !== false;

                return `<tr><td>${escapeHtml(aluno.nome)}</td><td>${escapeHtml(
                    aluno.cpf
                )}</td><td>${escapeHtml(aluno.email)}</td><td>${escapeHtml(
                    aluno.telefone
                )}</td><td><span class="status ${
                    active ? "" : "inactive"
                }">${active ? "Ativo" : "Inativo"}</span></td><td><button class="delete-button" data-id="${
                    aluno.id
                }">Excluir</button></td></tr>`;
            })
            .join("");

        table
            .querySelectorAll(".delete-button")
            .forEach(function (button) {
                button.addEventListener("click", async function () {
                    if (!window.confirm("Deseja excluir este aluno?")) {
                        return;
                    }

                    try {
                        await VivaFitApi.request(
                            "/Alunos/" + button.dataset.id,
                            {
                                method: "DELETE"
                            }
                        );

                        load();
                    } catch (error) {
                        message.textContent = error.message;
                    }
                });
            });
    }

    async function load() {
        message.textContent = "";
        table.innerHTML =
            '<tr><td colspan="6" class="empty-state">Carregando alunos...</td></tr>';

        try {
            render(await VivaFitApi.request("/Alunos"));
        } catch (error) {
            message.textContent = error.message;
            table.innerHTML =
                '<tr><td colspan="6" class="empty-state">Não foi possível carregar os alunos.</td></tr>';
        }
    }

    document
        .getElementById("aluno-form")
        .addEventListener("submit", async function (event) {
            event.preventDefault();

            const form = event.currentTarget;
            const payload = {
                nome: form.nome.value.trim(),
                cpf: form.cpf.value.trim(),
                email: form.email.value.trim(),
                telefone: form.telefone.value.trim(),
                dataNascimento: form.dataNascimento.value
            };

            try {
                await VivaFitApi.request("/Alunos", {
                    method: "POST",
                    body: JSON.stringify(payload)
                });

                form.reset();
                message.textContent = "Aluno cadastrado com sucesso.";
                load();
            } catch (error) {
                message.textContent = error.message;
            }
        });

    document.getElementById("refresh-alunos").addEventListener("click", load);
    load();
});
