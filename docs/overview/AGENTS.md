# Instruções para agentes (Cursor / IA) — VisionAssets

Use este arquivo junto com [CONTEXT.md](CONTEXT.md) para alinhar comportamento no repositório.

## Prioridades

1. **Rastreabilidade**: toda mudança de escopo ou técnica relevante deve refletir em documentação (`docs/`, [CHANGELOG.md](CHANGELOG.md)) conforme [GOVERNANCE.md](../GOVERNANCE.md).
2. **Escopo mínimo**: não expandir o MVP sem registro em backlog (épico/PBI/US) ou decisão (ADR).
3. **Consistência**: IDs de trabalho (`EPIC-xxx`, `US-xxx`, `TASK-xxx`) e links entre documentos.

## O que criar ou atualizar ao implementar

| Situação | Ação |
|----------|------|
| Nova funcionalidade | User story ou task referenciada; atualizar [TRACEABILITY-MATRIX.md](../traceability/TRACEABILITY-MATRIX.md) |
| Decisão técnica com trade-offs | Novo ADR em `docs/decisions/` |
| Mudança visível ao usuário ou operação | Entrada em [CHANGELOG.md](CHANGELOG.md) |
| Alteração de requisito de negócio | [REQUIREMENTS.md](../business/REQUIREMENTS.md) + matriz de rastreabilidade |

## Estrutura de pastas de código (quando existir)

- Manter espelho lógico: `src/`, `tests/`, `build/` — documentar em [ARCHITECTURE.md](../technical/ARCHITECTURE.md) quando a primeira árvore for criada.

## Idioma

- Documentação de produto/negócio: **português (Brasil)**.
- Código e nomes técnicos: **inglês** é aceitável; comentários podem seguir o padrão do time.

## Não fazer

- Apagar histórico do changelog (apenas corrigir entradas com erro factual).
- Introduzir dependências sem mencionar em ADR ou README técnico quando houver impacto de licença ou deploy.
