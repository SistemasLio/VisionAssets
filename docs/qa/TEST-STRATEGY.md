# Estratégia de testes

## Objetivo

Garantir que cada entrega do VisionAssets é **verificável**, com critérios claros e rastreio entre requisitos, casos de uso e evidências (manual ou automática).

## Níveis (pirâmide alvo)

| Nível | Âmbito no VisionAssets | Estado |
|-------|-------------------------|--------|
| **Unitário** | Funções puras, parsers, normalização de strings de software | A introduzir com projetos `tests/` |
| **Integração** | SQLite in-memory ou ficheiro temporário, migrações, repositórios | A introduzir |
| **Sistema / manual** | Agente a correr em Windows, ficheiros de log, base SQLite real, portal VitePress | **Ativo** — descrito em [HOW-TO-TEST.md](HOW-TO-TEST.md) |
| **Instalação** | MSI, serviço Windows, GPO (EPIC-004) | Planeado |

## Ambientes

| Ambiente | Uso |
|----------|-----|
| **Desenvolvimento** | `DOTNET_ENVIRONMENT=Development`, dados em `Data/` e `Logs/` sob o output do build |
| **CI** | `dotnet build`; workflow de documentação (`docs.yml`); testes automatizados quando existirem |
| **Piloto** | Máquinas-alvo com política interna (domínio, Intune, etc.) — fora do repositório |

## Ferramentas

| Ferramenta | Finalidade |
|--------------|------------|
| `dotnet build` / `dotnet test` | Compilação e (futuros) testes automatizados |
| Navegador | Portal de documentação local (`npm run docs:preview`) e GitHub Pages |
| Leitor SQLite | DB Browser for SQLite, `sqlite3`, extensão VS Code — validar esquema e dados |
| Visualizador de eventos / ficheiros | Logs Serilog em disco |

## Critérios de saída (gate) para release

- [ ] [HOW-TO-TEST.md](HOW-TO-TEST.md) atualizado para a versão
- [ ] [USE-CASES.md](USE-CASES.md) e/ou [RETROSPECTIVE.md](RETROSPECTIVE.md) alinhados ao âmbito
- [ ] `CHANGELOG.md` com notas testáveis quando aplicável
- [ ] Falhas conhecidas registadas (issues ou secção no changelog)
