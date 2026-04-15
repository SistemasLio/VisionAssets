# Como manter o changelog

## Princípios

- Uma entrada em **[Unreleased]** durante o trabalho; ao **tagar** uma versão, renomear a seção para o número (`[1.0.0]`) e data.
- Tipos de mudança: **Added**, **Changed**, **Deprecated**, **Removed**, **Fixed**, **Security** (conforme [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/)).
- Linguagem: **português** para descrições voltadas ao time; termos técnicos em inglês quando forem nomes de API ou ferramentas.

## Ligação com backlog

- Referenciar IDs quando fizer sentido: `US-014`, `PBI-020`.
- Releases importantes: alinhar com marcos do [ROADMAP.md](../product/ROADMAP.md).

## Versionamento (SemVer)

- **MAJOR**: incompatibilidade de contrato ou remoção de suporte.
- **MINOR**: funcionalidade nova compatível.
- **PATCH**: correções compatíveis.

Para o **primeiro executável/MSI**, sugerir `0.1.0` ou `1.0.0` conforme política do produto (documentar em ADR se houver debate).
