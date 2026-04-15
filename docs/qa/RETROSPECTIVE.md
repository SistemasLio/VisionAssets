# Retrospectiva de entregas e validação

Registo **retroativo** do que já foi entregue até à data indicada e **como** foi (ou deve ser) validado. Atualizar em marcos (release, fim de épico).

**Última atualização:** 2026-04-15

## Sumário

| Marco | Conteúdo | Evidência de QA |
|-------|----------|------------------|
| Documentação + governança | Estrutura `docs/`, IDs, matriz de rastreio, changelog | Revisão manual dos ficheiros; links internos |
| Portal VitePress | Site estático, sidebar, Mermaid, GitHub Pages | TC-DOC-01, TC-DOC-02 |
| EPIC-001 — Agente base | Worker, Serilog, serviço Windows, heartbeat | TC-AGENT-01, TC-AGENT-02 |
| EPIC-002 — Persistência | SQLite, migrações, `machine`, `inventory_run` | TC-INV-01 |

## Detalhe por épico

### Documentação e portal (sem código de produto)

- **Entregue:** Markdown em `docs/`, `docs/overview/`, portal VitePress, workflow GitHub Actions, `.nojekyll` no build, `cleanUrls: false` para Pages.
- **Validação:** leitura no repositório; `npm run docs:build` sem erros; site público após deploy (quando CI ativo).
- **Riscos mitigados:** 404 no Pages (Jekyll, base URL) documentados no README.

### EPIC-001 — Fundação do agente

- **Entregue:** `src/VisionAssets.Agent`, `AddWindowsService`, `AgentOptions` (heartbeat, logs), Serilog consola + ficheiro rotativo, `ContentRoot` = pasta do executável.
- **Validação:** TC-AGENT-01; logs em `Logs/`; versão `0.3.0` no assembly para rastreio.
- **Pendências de teste automático:** nenhum teste de unidade ainda (ver [TEST-STRATEGY.md](TEST-STRATEGY.md)).

### EPIC-002 — SQLite e modelo de dados

- **Entregue:** `src/VisionAssets.Persistence`, `001_initial.sql`, `MigrationRunner`, repositórios, integração no `Program.cs` e `AgentWorker` (registo de execuções sem coleta WMI ainda).
- **Validação:** TC-INV-01; inspeção da base; migração aplicada uma vez por versão.
- **Pendências:** EPIC-003 (preencher `hardware_component` e `installed_software`); testes de integração sobre SQLite in-memory.

## Lições e melhorias

- Fechar o processo do agente antes de `dotnet build` evita bloqueio do `.exe` no Windows.
- Manter [HOW-TO-TEST.md](HOW-TO-TEST.md) sincronizado com o comportamento real reduz regressões silenciosas.
