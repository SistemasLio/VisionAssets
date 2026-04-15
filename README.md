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
- **SQLite (EPIC-002)**: ficheiro local (`Agent:DatabasePath` ou padrão `Data/visionassets.db` em Development e `%ProgramData%\VisionAssets\Data\visionassets.db` em produção). Migrações em `src/VisionAssets.Persistence/Migrations/`. Cada heartbeat regista uma linha em `inventory_run`.
- **Inventário (EPIC-003)**: projeto `src/VisionAssets.Inventory` — hardware via WMI/CIM (sem `Win32_Product`), software via chaves Uninstall (HKLM 64/32, HKCU opcional com `Agent:IncludeCurrentUserUninstallKeys`). Snapshot completo em `hardware_component` e `installed_software` a cada execução.
- **Build:** o projeto do agente usa `net8.0-windows`; compile na sua máquina Windows (ou agente CI `windows-latest`). O projeto `VisionAssets.Persistence` compila em qualquer SO.

```bash
dotnet build
dotnet run --project src/VisionAssets.Agent
```

Variáveis de ambiente: `DOTNET_ENVIRONMENT=Development` (já definida em `launchSettings.json` ao depurar).

### MSI (EPIC-004)

Projeto WiX: [installer/VisionAssets.Installer](installer/VisionAssets.Installer). Gera `VisionAssets.Agent.msi` (x64, **framework-dependent** — requer [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) x64 no alvo, conforme [ADR-001](docs/decisions/ADR-001-wix-msi-framework-dependent.md)).

Na raiz, em Windows:

```bash
dotnet build installer/VisionAssets.Installer/VisionAssets.Installer.wixproj -c Release
```

Saída: `installer/VisionAssets.Installer/bin/Release/VisionAssets.Agent.msi`. O `dotnet build` da `VisionAssets.slnx` pode não invocar o projeto WiX em todas as versões da CLI; use o comando acima para o MSI.

Instalação silenciosa típica (administrador): `msiexec /i VisionAssets.Agent.msi /qn` (ajuste conforme política da organização).

Instalação manual do serviço **sem** MSI (administrador, após `dotnet publish`), se necessário:

```text
sc create "VisionAssets Agent" binPath= "C:\caminho\completo\VisionAssets.Agent.exe"
sc start "VisionAssets Agent"
```

(O nome do serviço inclui espaço, alinhado ao instalador MSI.)

## Estado do repositório

Agente com SQLite, inventário WMI/Registry e **MSI WiX**; portal VitePress e documentação em `docs/`. Ver [docs/overview/CHANGELOG.md](docs/overview/CHANGELOG.md).
