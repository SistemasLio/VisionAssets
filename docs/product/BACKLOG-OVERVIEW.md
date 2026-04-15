# Backlog — visão geral (Épicos e PBIs)

Este documento concentra **épicos** e **PBIs** com IDs alinhados a [GOVERNANCE.md](../GOVERNANCE.md). Histórias detalhadas podem viver em `docs/product/stories/US-NNN.md` (criar quando necessário).

---

## EPIC-001 — Fundação do agente e serviço Windows

**Objetivo**: repositório .NET estruturado, serviço instalável, configuração e logging.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-001 | Estrutura de solução .NET (serviço + bibliotecas) | Done | `VisionAssets.slnx`, `src/VisionAssets.Agent` |
| PBI-002 | Configuração (appsettings, variáveis, parâmetros MSI) | Done | `Agent` + `appsettings*.json`; MSI em EPIC-004 |
| PBI-003 | Logging estruturado (Serilog) e rotação | Done | Console + arquivo diário, retenção 14 dias |

**Requisitos**: REQ-003, REQ-007, REQ-NF-004.

---

## EPIC-002 — Persistência SQLite e modelo de dados

**Objetivo**: esquema inicial, migrações, repositórios.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-010 | Esquema SQLite (Machine, Hardware, Software, InventoryRun) | Done | `001_initial.sql`, tabelas + índices |
| PBI-011 | Migrações versionadas | Done | `MigrationRunner`, `schema_migrations`, SQL embutido |
| PBI-012 | Camada de acesso a dados | Done | Dapper, `MachineRepository`, `InventoryRunRepository` |

**Requisitos**: REQ-006, REQ-007.

---

## EPIC-003 — Coleta de inventário (hardware e software)

**Objetivo**: implementar coleta conforme [INVENTORY-COLLECTION.md](../technical/INVENTORY-COLLECTION.md).

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-020 | Coleta de hardware via WMI/CIM | Done | `VisionAssets.Inventory/HardwareCollector.cs` |
| PBI-021 | Coleta de software via Registry (64/32 bit + HKCU opcional) | Done | `SoftwareCollector.cs`; `Agent:IncludeCurrentUserUninstallKeys` |
| PBI-022 | Estratégia anti-duplicação e normalização | Done | Deduplicação por nome+versão+editor no software |
| PBI-023 | Licenças: detecção parcial + modelo para dados manuais | Planned | Tabela `license_record`; sem preenchimento automático no MVP |

**Requisitos**: REQ-004, REQ-005, REQ-008, REQ-009.

---

## EPIC-004 — Empacotamento MSI e implantação

**Objetivo**: MSI reprodutível, parâmetros de instalação silenciosa, documentação para GPO/SCCM/Intune.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-030 | Projeto WiX (ou ferramenta escolhida) e pipeline de build | Done | `installer/VisionAssets.Installer`, WiX 5; workflow `.github/workflows/msi.yml`; ADR-001 |
| PBI-031 | Self-contained vs framework-dependent | Done | ADR-001: MSI atual = framework-dependent (`win-x64`); self-contained como evolução (Q-002) |
| PBI-032 | Documentação de implantação em rede | Done | [DEPLOYMENT.md](../technical/DEPLOYMENT.md) |

**Requisitos**: REQ-001, REQ-002, REQ-NF-001.

---

## EPIC-005 — Exportação e integração leve (Fase B)

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-040 | Exportação CSV/JSON | Planned | REQ-010 |
| PBI-041 | Contrato de dados para API central | In progress | Rascunho OpenAPI em `docs/contracts/`; implementação em EPIC-006 |

---

## EPIC-006 — Backend central e sincronização (Entra ID)

**Objetivo**: API greenfield, autenticação Microsoft Entra ID, envio de snapshots pelo agente (~400 máquinas; 2–3×/semana).

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-050 | Entra ID: app registrations, permissões (app roles), consentimento admin, distribuição de credencial (cert/secret) | Planned | ADR-002 |
| PBI-051 | API ASP.NET Core — `POST /v1/inventory-snapshots`, validação JWT, persistência central | Planned | REQ-012; código em **repositório separado** ([ADR-003](../decisions/ADR-003-api-repository-separate.md)) |
| PBI-052 | Agente: MSAL (client credentials), `HttpClient`, outbox/retry após SQLite | In progress | `VisionAssets.Sync`, `sync_outbox`, `Backend:*`; REQ-012 |
| PBI-053 | Identidade reforçada: `azure_ad_device_id` quando disponível no Windows; `machine_id` + hostname no payload | Done | `EntraDeviceIdReader`, payload OpenAPI |

**Requisitos**: REQ-012 (e alinhamento a REQ-010 se export coexistir).

---

## User stories iniciais (macro)

Detalhar em arquivos `US-NNN.md` com critérios de aceite.

| US ID | Título | Épico | PBI |
|-------|--------|-------|-----|
| US-001 | Como operador de TI, quero instalar o agente silenciosamente para implantar em massa | EPIC-004 | PBI-030 |
| US-002 | Como gestor de ativos, quero ver inventário de hardware atualizado para planejar substituições | EPIC-003 | PBI-020 |
| US-003 | Como gestor de ativos, quero lista de software instalado confiável para auditoria | EPIC-003 | PBI-021 |
| US-004 | Como administrador, quero logs de execução do inventário para diagnosticar falhas | EPIC-001 | PBI-003 |
| US-005 | Como operador de TI, quero que o inventário seja enviado de forma segura ao servidor central (Entra ID) para consolidar ativos | EPIC-006 | PBI-051, PBI-052 |

---

## Como exportar para Azure DevOps / Jira

1. Copiar título: `[US-004] Como administrador...`.
2. Colar descrição e critérios do arquivo da história.
3. Vincular **Parent** ao Épico ou Feature correspondente.
4. Criar **Tasks** com prefixo `TASK-xxx` referenciando o mesmo US.
