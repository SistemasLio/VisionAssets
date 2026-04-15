# VisionAssets — Contexto do projeto

**Repositório Git**: [https://github.com/SistemasLio/VisionAssets](https://github.com/SistemasLio/VisionAssets)

Este arquivo é a **porta de entrada** para contexto humano e ferramentas (IDE, agentes). Mantenha-o atualizado quando o escopo ou as decisões mudarem. Detalhes vivem em `docs/`; aqui ficam resumo e links.

## O que é

Aplicação **Windows** instalável via **MSI**, pensada para **implantação em rede** (GPO, SCCM, Intune, etc.), para **inventariar e apoiar o controle** de ativos de **hardware** e **software**, com **backend .NET**, **SQLite local** e coleta periódica ou sob demanda.

## Objetivos de negócio (resumo)

- Visibilidade do parque: hardware, software instalado e (quando detectável) informações de licenciamento.
- Implantação padronizada via MSI e rastreabilidade de mudanças (changelog, backlog).
- Base para evolução futura (sincronização com servidor central, políticas de compliance), sem obrigar isso no MVP.

## Objetivos técnicos (resumo)

- Agente/serviço .NET (Worker/Windows Service) + biblioteca de coleta (WMI/CIM, Registry).
- Persistência em SQLite com esquema versionado (migrações).
- Empacotamento MSI (ex.: WiX), preferência por runtime **self-contained** se não houver .NET pré-instalizado nos alvos.

## Documentação

| Área | Onde |
|------|------|
| Índice geral | [docs/README.md](docs/README.md) |
| Governança e IDs | [docs/GOVERNANCE.md](docs/GOVERNANCE.md) |
| Visão e requisitos de negócio | [docs/business/](docs/business/) |
| Arquitetura e stack | [docs/technical/](docs/technical/) |
| Épicos / backlog / rastreabilidade | [docs/product/](docs/product/) |
| Decisões arquiteturais (ADRs) | [docs/decisions/](docs/decisions/) |
| Changelog | [CHANGELOG.md](CHANGELOG.md) |

## Decisões já registradas (alto nível)

| ID | Decisão | Onde detalhar |
|----|---------|----------------|
| D-001 | SQLite como BD local no MVP | [docs/technical/DATA-MODEL.md](docs/technical/DATA-MODEL.md) |
| D-002 | Coleta WMI/CIM + Registry; evitar `Win32_Product` em rotina | [docs/technical/INVENTORY-COLLECTION.md](docs/technical/INVENTORY-COLLECTION.md) |
| D-003 | Licenças: detecção parcial + campos manuais/importação | [docs/business/REQUIREMENTS.md](docs/business/REQUIREMENTS.md) |

## Como atualizar este arquivo

- Após cada release relevante: uma linha no resumo + link para entrada no `CHANGELOG.md`.
- Quando nascer um servidor central ou novo canal de sync: atualizar objetivos e link para ADR.
