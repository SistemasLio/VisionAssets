# Matriz de rastreabilidade

Ligue **requisitos de negócio** a **épicos/PBIs/US** e, quando o código existir, a **módulos/commits/tags**.

## Legenda

| Coluna | Conteúdo |
|--------|----------|
| REQ | ID em [REQUIREMENTS.md](../business/REQUIREMENTS.md) |
| Épico / PBI | IDs em [BACKLOG-OVERVIEW.md](../product/BACKLOG-OVERVIEW.md) |
| US | User story (quando criada em `docs/product/stories/`) |
| Implementação | Pastas, projetos ou PRs — preencher após código |

## Matriz (MVP)

| REQ | Épico | PBI | US | Implementação |
|-----|-------|-----|----|----------------|
| REQ-001 | EPIC-004 | PBI-030 | US-001 | `installer/VisionAssets.Installer`, `VisionAssets.Agent.msi`, ADR-001 |
| REQ-002 | EPIC-004 | PBI-030, PBI-032 | — | MSI + serviço (WiX `ServiceInstall`); [DEPLOYMENT.md](../technical/DEPLOYMENT.md) |
| REQ-003 | EPIC-001 | PBI-001 | — | `src/VisionAssets.Agent` (`AddWindowsService`, `AgentWorker`) |
| REQ-004 | EPIC-003 | PBI-020 | US-002 | `VisionAssets.Inventory`, `hardware_component` |
| REQ-005 | EPIC-003 | PBI-021 | US-003 | `SoftwareCollector`, `installed_software` |
| REQ-006 | EPIC-002 | PBI-010, PBI-011 | — | `VisionAssets.Persistence`, `Migrations/001_initial.sql` |
| REQ-007 | EPIC-002 | PBI-010 | — | `inventory_run`, `AgentWorker.RecordInventoryRunAsync` |
| REQ-008 | EPIC-003 | PBI-021 | — | Sem `Win32_Product`; só Registry + WMI |
| REQ-009 | EPIC-003 | PBI-023 | — | — |
| REQ-010 | EPIC-005 | PBI-040 | — | — |
| REQ-011 | EPIC-001 | PBI-003 | US-004 | `Program.cs` (Serilog arquivo + console), `Logs/` |
| REQ-012 | EPIC-006 | PBI-051, PBI-052, PBI-053 | US-005 | [ADR-002](../decisions/ADR-002-entra-id-central-api.md), [ADR-003](../decisions/ADR-003-api-repository-separate.md), API em repo externo; [OpenAPI](../contracts/inventory-v1.openapi.yaml) |

## Requisitos não funcionais

| REQ | Como verificar | Evidência |
|-----|----------------|-----------|
| REQ-NF-001 | Teste de instalação em VM domain-joined | Link para runbook ou anexo |
| REQ-NF-002 | Revisão de campos e política de mascaramento | ADR ou doc de segurança |
| REQ-NF-003 | Medição em piloto | Métricas no changelog ou relatório |
| REQ-NF-004 | Esta matriz + PRs com IDs | Este arquivo |

## Manutenção

- Atualizar **uma linha por REQ** quando PBI/US mudarem.
- Para cada release, revisar se todos os REQ **Must** do escopo estão cobertos ou explicitamente adiados.
