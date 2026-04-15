# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [Unreleased]

### Added

- Site de documentação com **VitePress** (`npm run docs:dev` / `docs:build`), tema sóbrio, busca local, diagramas Mermaid e sidebar por seção.
- Workflow **GitHub Actions** para publicar em **GitHub Pages** (`.github/workflows/docs.yml`).
- Pasta `docs/overview/`: contexto do projeto, instruções para agentes de IA e changelog (fonte única; na raiz permanece `README.md` com links).

### Changed

- `CONTEXT.md`, `AGENTS.md` e `CHANGELOG.md` movidos da raiz para `docs/overview/`; referências internas atualizadas.

## [0.0.0] — 2026-04-15

### Added

- Documentação inicial do plano: contexto, governança, negócio, produto (épicos/PBIs/US), técnico, rastreabilidade, template de ADR e changelog.
- Repositório remoto GitHub, branch `main`, tag `v0.0.0`.
- Estrutura de documentação do repositório (sem código de aplicação ainda).

[Unreleased]: https://github.com/SistemasLio/VisionAssets/compare/v0.0.0...HEAD
[0.0.0]: https://github.com/SistemasLio/VisionAssets/releases/tag/v0.0.0
