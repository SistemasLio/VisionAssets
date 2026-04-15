# Casos de uso

Cada caso de uso (**UC-xxx**) liga-se a requisitos (**REQ-xxx**) e é validado por cenários em [HOW-TO-TEST.md](HOW-TO-TEST.md) (**TC-xxx**).

## Documentação e repositório

| ID | Actor | Objetivo | REQ / épico | TC |
|----|-------|----------|-------------|-----|
| UC-DOC-001 | Leitor / developer | Consultar toda a documentação Markdown no portal com navegação e busca | REQ-NF-004, docs | TC-DOC-01 |
| UC-DOC-002 | Maintainer | Gerar o site localmente e confirmar build antes de publicar | REQ-NF-004 | TC-DOC-02 |

## Agente Windows (operador / TI)

| ID | Actor | Objetivo | REQ / épico | TC |
|----|-------|----------|-------------|-----|
| UC-AGENT-001 | Operador | Executar o agente em modo consola e ver logs estruturados | REQ-003, REQ-011, EPIC-001 | TC-AGENT-01 |
| UC-AGENT-002 | Operador | Garantir que o processo pode ser instalado como serviço Windows (futuro MSI) | REQ-003 | TC-AGENT-02 |
| UC-INV-001 | Gestor de ativos | Ter inventário de hardware e software na base local | REQ-004, REQ-005, REQ-006, EPIC-003 | TC-INV-02 |
| UC-INV-002 | Auditor | Ver histórico de execuções de inventário (sucesso/falha, versão do agente) | REQ-007, EPIC-002 | TC-INV-01 |

## Formato sugerido para novos casos de uso

```markdown
## UC-XXX — Título curto

**Actor:** …  
**Pré-condições:** …  
**Fluxo principal:** …  
**Pós-condições:** …  
**REQ:** REQ-xxx  
**TC associados:** TC-xxx
```
