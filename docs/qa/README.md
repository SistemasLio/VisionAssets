# Qualidade (QA) e validação

Esta secção é **obrigatória** para o processo do VisionAssets: descreve como validamos entregas, casos de uso verificáveis e evidências de teste. Deve evoluir **junto** com o código e o backlog.

## Documentos

| Documento | Conteúdo |
|-----------|----------|
| [TEST-STRATEGY.md](TEST-STRATEGY.md) | Níveis de teste, âmbito, ferramentas, ambientes |
| [USE-CASES.md](USE-CASES.md) | Casos de uso (UC-xxx) e rastreio a requisitos / épicos |
| [HOW-TO-TEST.md](HOW-TO-TEST.md) | Passos práticos: portal, agente, SQLite, CI |
| [RETROSPECTIVE.md](RETROSPECTIVE.md) | Entregas já feitas e como foram validadas (atualizar por marco) |

## Diretriz do projeto

1. **Cada incremento** que altere comportamento ou dados deve ter pelo menos: atualização em [HOW-TO-TEST.md](HOW-TO-TEST.md) (como reproduzir) e, quando fizer sentido, novo ou alterado caso de uso em [USE-CASES.md](USE-CASES.md).
2. **Histórias de utilizador** devem manter critérios de aceite **testáveis**; o QA documental descreve a verificação manual ou automática esperada.
3. **Regressão**: antes de fechar uma release, revisar a lista de cenários em [HOW-TO-TEST.md](HOW-TO-TEST.md) aplicável ao âmbito da versão.
4. **Testes automatizados** (quando existirem): mencionar em [TEST-STRATEGY.md](TEST-STRATEGY.md) e na pipeline de CI; não substituem a atualização desta pasta quando há passos só manuais.

## IDs sugeridos

| Prefixo | Uso |
|---------|-----|
| **UC-xxx** | Caso de uso (objetivo do utilizador/operador) |
| **TC-xxx** | Cenário de teste / caso de teste (passos concretos) — ver HOW-TO-TEST |

## Ligação com o resto da documentação

- Requisitos: [REQUIREMENTS.md](../business/REQUIREMENTS.md)
- Governança e PRs: [GOVERNANCE.md](../GOVERNANCE.md)
- Changelog: [CHANGELOG.md](../overview/CHANGELOG.md)
