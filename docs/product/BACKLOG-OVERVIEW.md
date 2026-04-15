# Backlog — visão geral (Épicos e PBIs)

Este documento concentra **épicos** e **PBIs** com IDs alinhados a [GOVERNANCE.md](../GOVERNANCE.md). Histórias detalhadas podem viver em `docs/product/stories/US-NNN.md` (criar quando necessário).

---

## EPIC-001 — Fundação do agente e serviço Windows

**Objetivo**: repositório .NET estruturado, serviço instalável, configuração e logging.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-001 | Estrutura de solução .NET (serviço + bibliotecas) | Planned | Worker Service / Windows Service |
| PBI-002 | Configuração (appsettings, variáveis, parâmetros MSI) | Planned | |
| PBI-003 | Logging estruturado (Serilog) e rotação | Planned | |

**Requisitos**: REQ-003, REQ-007, REQ-NF-004.

---

## EPIC-002 — Persistência SQLite e modelo de dados

**Objetivo**: esquema inicial, migrações, repositórios.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-010 | Esquema SQLite (Machine, Hardware, Software, InventoryRun) | Planned | Ver [DATA-MODEL.md](../technical/DATA-MODEL.md) |
| PBI-011 | Migrações versionadas | Planned | |
| PBI-012 | Camada de acesso a dados | Planned | |

**Requisitos**: REQ-006, REQ-007.

---

## EPIC-003 — Coleta de inventário (hardware e software)

**Objetivo**: implementar coleta conforme [INVENTORY-COLLECTION.md](../technical/INVENTORY-COLLECTION.md).

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-020 | Coleta de hardware via WMI/CIM | Planned | |
| PBI-021 | Coleta de software via Registry (64/32 bit + HKCU opcional) | Planned | |
| PBI-022 | Estratégia anti-duplicação e normalização | Planned | |
| PBI-023 | Licenças: detecção parcial + modelo para dados manuais | Planned | REQ-009 |

**Requisitos**: REQ-004, REQ-005, REQ-008, REQ-009.

---

## EPIC-004 — Empacotamento MSI e implantação

**Objetivo**: MSI reprodutível, parâmetros de instalação silenciosa, documentação para GPO/SCCM/Intune.

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-030 | Projeto WiX (ou ferramenta escolhida) e pipeline de build | Planned | ADR para escolha |
| PBI-031 | Self-contained vs framework-dependent | Planned | ADR |
| PBI-032 | Documentação de implantação em rede | Planned | |

**Requisitos**: REQ-001, REQ-002, REQ-NF-001.

---

## EPIC-005 — Exportação e integração leve (Fase B)

| PBI ID | Título | Status | Notas |
|--------|--------|--------|-------|
| PBI-040 | Exportação CSV/JSON | Planned | REQ-010 |
| PBI-041 | Contrato de dados para futura API central | Planned | REQ-012 |

---

## User stories iniciais (macro)

Detalhar em arquivos `US-NNN.md` com critérios de aceite.

| US ID | Título | Épico | PBI |
|-------|--------|-------|-----|
| US-001 | Como operador de TI, quero instalar o agente silenciosamente para implantar em massa | EPIC-004 | PBI-030 |
| US-002 | Como gestor de ativos, quero ver inventário de hardware atualizado para planejar substituições | EPIC-003 | PBI-020 |
| US-003 | Como gestor de ativos, quero lista de software instalado confiável para auditoria | EPIC-003 | PBI-021 |
| US-004 | Como administrador, quero logs de execução do inventário para diagnosticar falhas | EPIC-001 | PBI-003 |

---

## Como exportar para Azure DevOps / Jira

1. Copiar título: `[US-004] Como administrador...`.
2. Colar descrição e critérios do arquivo da história.
3. Vincular **Parent** ao Épico ou Feature correspondente.
4. Criar **Tasks** com prefixo `TASK-xxx` referenciando o mesmo US.
