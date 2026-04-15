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

### GitHub Pages (publicação)

1. No repositório: **Settings → Pages → Build and deployment → Source: GitHub Actions** (não use “Deploy from a branch” para este fluxo).
2. Faça push na `main` ou rode o workflow manualmente: **Actions → Deploy documentation → Run workflow**.
3. Aguarde os jobs **build** e **deploy** concluírem (1–3 minutos na primeira vez).
4. Abra: **https://sistemaslio.github.io/VisionAssets/** (caminho com o **nome exato** do repositório).

O comando `npm run docs:build` também cria `docs/.vitepress/dist/.nojekyll` para o GitHub Pages **não** processar o site com Jekyll (evita 404 com VitePress).

#### Se ainda aparecer 404

- A URL do projeto nunca é a raiz `github.io` sozinha; use sempre `https://sistemaslio.github.io/VisionAssets/`.
- Em **Actions**, confira se o workflow terminou com sucesso.
- Em **Settings → Pages**, veja o aviso “Your site is live at …”.
- Teste em aba anônima após o deploy.

## Estado do repositório

Documentação e estrutura de rastreabilidade; código da aplicação ainda não iniciado. Ver [docs/overview/CHANGELOG.md](docs/overview/CHANGELOG.md).
