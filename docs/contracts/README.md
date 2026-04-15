# Contratos de API (VisionAssets)

Ficheiros **OpenAPI** versionados servindo de acordo entre o **agente** e o **backend central**.

| Ficheiro | Estado | Notas |
|----------|--------|-------|
| [inventory-v1.openapi.yaml](inventory-v1.openapi.yaml) | Rascunho v0.1 | Snapshot de inventário; autenticação Bearer (JWT Entra). Ver [ADR-002](../decisions/ADR-002-entra-id-central-api.md). |

Convenções:

- Quebras compatíveis: novo `schema_version` no corpo ou novo path `/v2/...`.
- Autenticação: OAuth 2.0 client credentials → access token no header `Authorization: Bearer`.
