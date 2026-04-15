# Requisitos

Cada requisito tem ID **`REQ-xxx`** para rastreio em épicos, histórias e matriz técnica.

## Funcionais

| ID | Descrição | Prioridade |
|----|-----------|------------|
| REQ-001 | O sistema deve ser instalável no Windows via pacote **MSI** | Must |
| REQ-002 | O sistema deve suportar **instalação silenciosa** e parâmetros documentados (caminho do banco, intervalo de coleta, etc.) | Must |
| REQ-003 | O sistema deve executar como **serviço** (ou Worker em modo serviço) em segundo plano | Must |
| REQ-004 | O sistema deve **inventariar propriedades de hardware** relevantes (CPU, memória, discos, BIOS, rede, GPU, etc.) | Must |
| REQ-005 | O sistema deve **listar software instalado** a partir de fontes confiáveis (Registry Uninstall, complementos conforme [INVENTORY-COLLECTION.md](../technical/INVENTORY-COLLECTION.md)) | Must |
| REQ-006 | O sistema deve **persistir** inventários em **SQLite** local com esquema versionado | Must |
| REQ-007 | O sistema deve registrar **execuções de inventário** (sucesso/falha, timestamp, versão do agente) | Must |
| REQ-008 | O sistema deve **evitar** rotinas de coleta que degradem performance (ex.: uso sistemático de `Win32_Product`) | Must |
| REQ-009 | O sistema deve permitir **detecção parcial** de licenciamento (Office/Windows quando aplicável) e **campos para dados manuais ou importados** | Should |
| REQ-010 | O sistema deve permitir **exportação** dos dados (CSV/JSON) para uso externo | Should |
| REQ-011 | O sistema deve manter **logs** operacionais rotativos | Should |
| REQ-012 | O sistema deve preparar evolução para **sincronização** com servidor central (contrato de dados, sem implementação obrigatória no MVP) | Could |

## Não funcionais

| ID | Descrição | Prioridade |
|----|-----------|------------|
| REQ-NF-001 | Instalação deve funcionar em ambiente **domain-joined** típico | Must |
| REQ-NF-002 | Dados sensíveis (ex.: chaves) não devem ser armazenados em texto claro sem **política explícita**; preferir mascaramento | Must |
| REQ-NF-003 | Consumo de CPU/memória do agente deve ser **compatível com desktop** (metas numéricas a definir em ADR) | Should |
| REQ-NF-004 | Documentação e backlog devem permitir **rastreabilidade** requisito → entrega | Must |

## MoSCoW (resumo)

- **Must**: REQ-001 a REQ-008, REQ-NF-001, REQ-NF-002, REQ-NF-004.
- **Should**: REQ-009 a REQ-011, REQ-NF-003.
- **Could**: REQ-012.

## Suposições

- Alvo principal: **Windows 10/11** (builds suportadas pela equipe — detalhar em ADR quando fixado).
- Agente roda com privilégios suficientes para WMI/Registry (conta de serviço padrão ou documentada).

## Riscos de negócio

| ID | Risco | Mitigação |
|----|-------|-----------|
| RISK-001 | Expectativa de “todas as licenças” automáticas | Comunicar limites; REQ-009 + treinamento |
| RISK-002 | Resistência a agente em endpoint | Política de privacidade; minimizar dados (REQ-NF-002) |
