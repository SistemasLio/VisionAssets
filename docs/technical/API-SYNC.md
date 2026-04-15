# Sincronização com API central (Entra ID)

Visão de conjunto para **REQ-012**: agente envia snapshots de inventário a um **backend greenfield**, autenticado com **Microsoft Entra ID**.

## Documentos de referência

| Documento | Conteúdo |
|-----------|----------|
| [ADR-002](../decisions/ADR-002-entra-id-central-api.md) | Decisão: client credentials, identidade em camadas, idempotência. |
| [Contrato OpenAPI v0.1](../contracts/inventory-v1.openapi.yaml) | Esquema do corpo JSON e endpoint `POST /v1/inventory-snapshots`. |
| [Matriz de rastreabilidade](../traceability/TRACEABILITY-MATRIX.md) | REQ-012 → EPIC-006. |

## Fluxo resumido

1. O agente obtém um **access token** (MSAL, fluxo client credentials) com credenciais da aplicação registada no Entra.
2. Após gravar o inventário em **SQLite**, envia um **snapshot JSON** (hardware + software + SO + identificadores) via **HTTPS**.
3. A API valida o JWT, persiste ou atualiza o estado da máquina e devolve confirmação.

Escala prevista (~400 máquinas, 2–3 execuções por semana): sem requisito de compressão no primeiro incremento; pode ser acrescentada depois.

## Próximos passos de implementação

Ver **EPIC-006** em [BACKLOG-OVERVIEW.md](../product/BACKLOG-OVERVIEW.md) (PBIs de API, Entra e agente).
