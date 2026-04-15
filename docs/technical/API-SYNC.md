# Sincronização com API central (Entra ID)

Visão de conjunto para **REQ-012**: agente envia snapshots de inventário a um **backend greenfield**, autenticado com **Microsoft Entra ID**.

## Repositório do código

- **Agente (Windows):** este repositório VisionAssets.
- **API central:** repositório **separado** — [ADR-003](../decisions/ADR-003-api-repository-separate.md), detalhes em [API-REPOSITORY.md](API-REPOSITORY.md). O contrato e os ADRs permanecem no repositório do agente como referência entre equipas.

## Documentos de referência

| Documento | Conteúdo |
|-----------|----------|
| [ADR-002](../decisions/ADR-002-entra-id-central-api.md) | Decisão: client credentials, identidade em camadas, idempotência. |
| [ADR-003](../decisions/ADR-003-api-repository-separate.md) | API em repo separado do agente. |
| [Contrato OpenAPI v0.1](../contracts/inventory-v1.openapi.yaml) | Esquema do corpo JSON e endpoint `POST /v1/inventory-snapshots`. |
| [Matriz de rastreabilidade](../traceability/TRACEABILITY-MATRIX.md) | REQ-012 → EPIC-006. |

## Fluxo resumido

1. O agente obtém um **access token** (MSAL, fluxo client credentials) com credenciais da aplicação registada no Entra.
2. Após gravar o inventário em **SQLite**, envia um **snapshot JSON** (hardware + software + SO + identificadores) via **HTTPS**.
3. A API valida o JWT, persiste ou atualiza o estado da máquina e devolve confirmação.

Escala prevista (~400 máquinas, 2–3 execuções por semana): sem requisito de compressão no primeiro incremento; pode ser acrescentada depois.

## Implementação no agente (repositório VisionAssets)

- Biblioteca **`VisionAssets.Sync`**: MSAL (`MsalAccessTokenProvider`), `InventoryHttpSyncClient`, `InventorySyncCoordinator`, `EntraDeviceIdReader`, DTOs alinhados ao OpenAPI.
- Configuração **`Backend`** em `appsettings.json` (`Enabled` por defeito `false`). Secretos: `dotnet user-secrets` ou variáveis de ambiente em produção.
- Tabela SQLite **`sync_outbox`** (migração `002`): retry com `OutboxMaxAttempts`.

## Implementação no servidor (repositório VisionAssets.Api)

- Projeto **ASP.NET Core 8** com **Microsoft.Identity.Web** (validação JWT Entra).
- Endpoints: `POST /v1/inventory-snapshots` (autenticado), `GET /health` (público).
- Armazenamento MVP **em memória** — evoluir para SQL Server / PostgreSQL conforme política da equipa.

## Próximos passos

- **Entra** (PBI-050): app registrations, permissões e consentimento em ambiente real.
- Persistência central e observabilidade na API.

Ver **EPIC-006** em [BACKLOG-OVERVIEW.md](../product/BACKLOG-OVERVIEW.md).
