# VisionAssets

Agente Windows (planejado) para inventário de hardware e software, com backend .NET, SQLite local e instalação via MSI.

**Repositório**: [github.com/SistemasLio/VisionAssets](https://github.com/SistemasLio/VisionAssets)

**Site de documentação** (GitHub Pages): [https://sistemaslio.github.io/VisionAssets/](https://sistemaslio.github.io/VisionAssets/)

## Documentação no repositório

| | |
|--|--|
| Contexto do projeto | [docs/overview/CONTEXT.md](docs/overview/CONTEXT.md) |
| Índice completo | [docs/README.md](docs/README.md) |
| Governança (IDs, backlog, changelog) | [docs/GOVERNANCE.md](docs/GOVERNANCE.md) |
| Agentes de IA (Cursor) | [docs/overview/AGENTS.md](docs/overview/AGENTS.md) |
| Changelog | [docs/overview/CHANGELOG.md](docs/overview/CHANGELOG.md) |

## Desenvolvimento do site de docs

Na raiz do repositório, com Node.js 20+:

```bash
npm install
npm run docs:dev
```

Build estático: `npm run docs:build` (saída em `docs/.vitepress/dist`, ignorada pelo Git).

### GitHub Pages

No repositório GitHub: **Settings → Pages → Build and deployment → Source: GitHub Actions**. O workflow [`.github/workflows/docs.yml`](.github/workflows/docs.yml) publica o site a cada push na branch `main`. A URL pública usa o prefixo `/VisionAssets/` ([site](https://sistemaslio.github.io/VisionAssets/)).

## Estado do repositório

Documentação e estrutura de rastreabilidade; código da aplicação ainda não iniciado. Ver [docs/overview/CHANGELOG.md](docs/overview/CHANGELOG.md).
