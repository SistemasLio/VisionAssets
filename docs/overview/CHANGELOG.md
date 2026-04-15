# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [Unreleased]

### Added

- **EPIC-004 (MVP parcial)**: projeto WiX 5 `installer/VisionAssets.Installer` — MSI `VisionAssets.Agent.msi` (Program Files, serviço `VisionAssets Agent`); publish `win-x64` framework-dependent; [ADR-001](../decisions/ADR-001-wix-msi-framework-dependent.md); workflow GitHub Actions `Build MSI` (artefato).
- **EPIC-003**: projeto `VisionAssets.Inventory` (`net8.0-windows`), coleta hardware (Win32_* via CIM), software (Uninstall Registry, 64/32/HKCU opcional), deduplicação de pacotes; `IInventoryDataRepository` + snapshots; `InventoryOrchestrator`; agente `net8.0-windows` v0.4.0; `MachineRepository.UpdateOperatingSystemAsync`; opções `IncludeCurrentUserUninstallKeys`, `CimQueryTimeoutMs`.
- Documentação de **QA**: pasta `docs/qa/` (estratégia, casos de uso UC-xxx, como testar TC-xxx, retrospectiva); diretriz em [GOVERNANCE.md](../GOVERNANCE.md); portal VitePress e índices atualizados.
- **EPIC-002**: biblioteca `src/VisionAssets.Persistence` (SQLite + Dapper), migração `001_initial.sql` (tabelas `machine`, `hardware_component`, `installed_software`, `license_record`, `inventory_run`), `IMigrationRunner`, `MachineRepository`, `InventoryRunRepository`; arranque aplica migrações pendentes; `Agent:DatabasePath` e ficheiro por defeito em `Data/` ou `%ProgramData%\VisionAssets\Data\`.
- **EPIC-001**: projeto `src/VisionAssets.Agent` (.NET 8 Worker), `AddWindowsService` (“VisionAssets Agent”), Serilog (console + arquivo com rotação diária), `Agent:HeartbeatIntervalMinutes` e `Agent:LogsDirectory`, `ContentRoot` = pasta do executável.
- Solução `VisionAssets.slnx` na raiz do repositório.
- Site de documentação com **VitePress** (`npm run docs:dev` / `docs:build`), tema sóbrio, busca local, diagramas Mermaid e sidebar por seção.
- Workflow **GitHub Actions** para publicar em **GitHub Pages** (`.github/workflows/docs.yml`).
- Pasta `docs/overview/`: contexto do projeto, instruções para agentes de IA e changelog (fonte única; na raiz permanece `README.md` com links).

### Changed

- `CONTEXT.md`, `AGENTS.md` e `CHANGELOG.md` movidos da raiz para `docs/overview/`; referências internas atualizadas.
- `cleanUrls: false` no VitePress para compatibilidade com o servidor estático do GitHub Pages.
- Após o build, script `scripts/ensure-nojekyll.mjs` grava `.nojekyll` em `dist` (Jekyll do Pages não deve processar o site).

### Fixed

- 404 no GitHub Pages causado pela ausência de `.nojekyll` no artefato publicado (arquivos em `public/` começando com ponto não eram copiados).

## [0.0.0] — 2026-04-15

### Added

- Documentação inicial do plano: contexto, governança, negócio, produto (épicos/PBIs/US), técnico, rastreabilidade, template de ADR e changelog.
- Repositório remoto GitHub, branch `main`, tag `v0.0.0`.
- Estrutura de documentação do repositório (sem código de aplicação ainda).

[Unreleased]: https://github.com/SistemasLio/VisionAssets/compare/v0.0.0...HEAD
[0.0.0]: https://github.com/SistemasLio/VisionAssets/releases/tag/v0.0.0
