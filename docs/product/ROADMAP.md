# Roadmap

Datas são **orientativas**; ajustar conforme capacidade do time.

## Fase A — MVP (agente + inventário + SQLite + MSI)

**Marco**: instalação corporativa funcional e primeira coleta completa em piloto.

Entregas esperadas:

- Serviço .NET com coleta agendada e/ou disparo manual.
- Modelo de dados SQLite + migrações.
- Coleta de hardware (WMI/CIM) e software (Registry).
- MSI com documentação de parâmetros silenciosos e implantação em rede ([DEPLOYMENT.md](../technical/DEPLOYMENT.md)).
- Logs e documentação de operação.

**Épicos**: [EPIC-001](BACKLOG-OVERVIEW.md#epic-001), [EPIC-002](BACKLOG-OVERVIEW.md#epic-002), [EPIC-003](BACKLOG-OVERVIEW.md#epic-003), [EPIC-004](BACKLOG-OVERVIEW.md#epic-004) (MSI; documentação de rede em evolução).

## Fase B — Relatórios e operação

**Marco**: exportação estável e comparativos básicos.

- Exportação CSV/JSON agendada ou por comando.
- Melhorias de deduplicação e normalização de nomes de software.
- UI mínima opcional (bandeja / status).

**Épicos**: [EPIC-005](BACKLOG-OVERVIEW.md#epic-005); documentação de implantação em rede ([EPIC-004](BACKLOG-OVERVIEW.md#epic-004) PBI-032).

## Fase C — Centralização

**Marco**: inventário consolidado no servidor com identidade corporativa (Entra ID).

- API greenfield + **Microsoft Entra ID** (client credentials); contrato [OpenAPI](../contracts/inventory-v1.openapi.yaml); [ADR-002](../decisions/ADR-002-entra-id-central-api.md).
- Sincronização do agente (snapshots após SQLite); fila/retry; escala ~400 máquinas, 2–3×/semana.
- Evolução: dashboards, políticas de compliance, export CSV/JSON ([EPIC-005](BACKLOG-OVERVIEW.md#epic-005)) em paralelo se necessário.

**Épico**: [EPIC-006](BACKLOG-OVERVIEW.md#epic-006--backend-central-e-sincronização-entra-id).
