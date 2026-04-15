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

## Fase C — Centralização (opcional)

**Marco**: visão única do parque.

- API e autenticação.
- Sincronização incremental do SQLite com backend central.
- Dashboards e políticas (compliance).

Itens correspondentes nascerão como novos épicos (`EPIC-00x`) quando a fase for aprovada.
