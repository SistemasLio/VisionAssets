# ADR-001 — WiX 5 para MSI e payload framework-dependent

## Status

Aceite

## Contexto

REQ-001 exige instalação via MSI; EPIC-004 prevê empacotamento reprodutível e documentação de implantação.

## Decisão

- **Ferramenta:** [WiX Toolset](https://wixtoolset.org/) v5 (`WixToolset.Sdk` no MSBuild), projeto em `installer/VisionAssets.Installer/`.
- **Payload:** `dotnet publish` do agente com `-r win-x64 --self-contained false` (requer **.NET 8 Runtime** no alvo).
- **Instalação:** ficheiros em `Program Files\VisionAssets\Agent\`; registo do serviço Windows **VisionAssets Agent** com arranque automático (componente MSI com `ServiceInstall` / `ServiceControl`).

## Consequências

- MSI mais pequeno do que um pacote self-contained; em ambientes sem runtime .NET, será necessário pré-instalar o runtime ou evoluir para variante self-contained (Q-002 no `OPEN-QUESTIONS.md`).
- O build do MSI deve ser feito em **Windows** (WiX); em CI usar `runs-on: windows-latest`.

## Alternativas consideradas

- **Self-contained:** maior tamanho; reservado para iteração futura ou segundo artefato.
- **Outras ferramentas de MSI:** Advanced Installer, etc. — custo/licenciamento; WiX mantém alinhamento com STACK.md.
