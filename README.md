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
| QA (testes, UC, como testar) | [docs/qa/README.md](docs/qa/README.md) |

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

## Agente .NET (EPIC-001 — primeiro entregável)

Solução em [VisionAssets.slnx](VisionAssets.slnx), projeto [src/VisionAssets.Agent](src/VisionAssets.Agent):

- Worker (`AgentWorker`) com heartbeat configurável.
- Pronto para instalação como **serviço Windows** (`AddWindowsService`, nome `VisionAssets Agent`).
- **Serilog**: console + arquivo com rotação diária; em Development os logs ficam em `Logs/` sob o diretório do executável; em produção, `%ProgramData%\VisionAssets\Logs` (sobrescreva com `Agent:LogsDirectory`).
- **SQLite (EPIC-002)**: ficheiro local (`Agent:DatabasePath` ou padrão `Data/visionassets.db` em Development e `%ProgramData%\VisionAssets\Data\visionassets.db` em produção). Migrações em `src/VisionAssets.Persistence/Migrations/`. Cada heartbeat regista uma linha em `inventory_run` (sem dados de hardware/software até EPIC-003).

```bash
dotnet build
dotnet run --project src/VisionAssets.Agent
```

Variáveis de ambiente: `DOTNET_ENVIRONMENT=Development` (já definida em `launchSettings.json` ao depurar).

Instalação manual do serviço (administrador, após `dotnet publish`):

```text
sc create VisionAssetsAgent binPath= "C:\caminho\completo\VisionAssets.Agent.exe"
sc start VisionAssetsAgent
```

## Estado do repositório

Documentação, portal VitePress e **agente base** (sem SQLite nem coleta WMI ainda). Ver [docs/overview/CHANGELOG.md](docs/overview/CHANGELOG.md).
