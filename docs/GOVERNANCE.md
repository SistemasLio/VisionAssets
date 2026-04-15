# Governança documental e rastreabilidade

Este projeto usa **Markdown** como fonte da verdade para negócio, produto e técnico, preparado para exportar ou espelhar em ferramentas (Azure DevOps, Jira, GitHub Issues).

## Hierarquia de trabalho (referência)

```
Épico (EPIC)
  └── PBI / Feature (PBI) — opcional se o time usar só histórias grandes
        └── User Story (US)
              └── Task (TASK)
```

- **Épico**: entrega de valor grande, várias sprints ou meses.
- **PBI (Product Backlog Item)**: incremento priorizável alinhado a um épico (termo comum no Azure Boards).
- **User Story**: valor para usuário/operador, estimável.
- **Task**: trabalho técnico (implementação, teste, doc).

## Convenção de IDs (prefixos)

Use IDs **estáveis** e **únicos** no repositório. Formato sugerido:

| Tipo | Formato | Exemplo |
|------|---------|---------|
| Épico | `EPIC-NNN` | `EPIC-001` |
| PBI | `PBI-NNN` | `PBI-001` |
| User Story | `US-NNN` | `US-001` |
| Task | `TASK-NNN` | `TASK-001` |
| Requisito | `REQ-NNN` | `REQ-001` |
| Decisão (ADR) | `ADR-NNN` | `ADR-001` |
| Risco | `RISK-NNN` | `RISK-001` |

- `NNN` é numérico com zeros à esquerda (três dígitos até 999; depois aumentar dígitos de forma consistente).
- Ao criar um item em ferramenta externa, colocar o ID do repo no **título ou descrição** (ex.: `[US-014] Inventário de software via Registry`).

## Onde registrar o quê

| Necessidade | Arquivo principal |
|-------------|-------------------|
| Escopo e prioridade de negócio | `docs/business/REQUIREMENTS.md` (`REQ-xxx`) |
| Épicos e PBIs | `docs/product/BACKLOG-OVERVIEW.md` |
| Histórias | `docs/product/stories/` (criar pasta quando a primeira US for detalhada) ou corpo do PBI com link |
| Ligação req → código | `docs/traceability/TRACEABILITY-MATRIX.md` |
| Decisão com alternativas | `docs/decisions/ADR-NNN-*.md` |
| Release e mudanças | `CHANGELOG.md` + tag Git |

## User Story — campos mínimos

Ver [product/templates/USER-STORY.md](product/templates/USER-STORY.md). Todo US deve ter:

- ID (`US-xxx`)
- Vínculo a épico/PBI (`EPIC-xxx`, `PBI-xxx`)
- Critérios de aceite testáveis
- Rastreio a requisitos (`REQ-xxx`) quando aplicável

## Changelog

- Seguir o formato descrito em [changelog/README.md](changelog/README.md) e no cabeçalho de [CHANGELOG.md](../CHANGELOG.md).
- Versões alinhadas a tags Git (`v0.1.0`, `v1.0.0`).

## Pull requests / revisão

- Referenciar IDs (`Closes US-012`, `Related REQ-005`).
- Atualizar `TRACEABILITY-MATRIX.md` quando o PR encerrar um requisito ou história.

## Privacidade e dados sensíveis

- Não colocar em documentação: chaves de licença completas, credenciais, dados pessoais reais de produção.
- Usar exemplos fictícios ou mascaramento.
