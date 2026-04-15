# ADR-003 — Repositório da API central separado do agente

## Status

Aceite

## Contexto

O backend central (EPIC-006, [ADR-002](ADR-002-entra-id-central-api.md)) deve ser implementado pela equipa. Surge a questão de **monorepo** (agente + API no mesmo repositório) versus **repositório dedicado** só para a API.

## Decisão

- A **API HTTP** (ASP.NET Core ou stack acordada) e a **persistência central** residem num **repositório Git separado** do repositório **VisionAssets** (agente, MSI, documentação do produto).
- O repositório VisionAssets mantém:
  - Contrato público de integração em [inventory-v1.openapi.yaml](../contracts/inventory-v1.openapi.yaml) (fonte de verdade do **contrato**; a equipa da API pode duplicar ou gerar código a partir deste ficheiro).
  - Documentação de sincronização ([API-SYNC.md](../technical/API-SYNC.md)) e ADRs relacionados.
- Versionamento e pipelines CI/CD da API são **independentes** do agente (ciclos de release desacoplados).

## Consequências

- Duplicação aceitável de convenções (naming, versão de `schema_version`) — mitigada pelo OpenAPI versionado e comunicação entre equipas.
- O repositório da API deve referenciar este contrato (submódulo, cópia em build, ou pacote NuGet gerado a partir do OpenAPI — decisão local à API).
- Issues e PRs de backend não aparecem no repositório do agente; rastreabilidade REQ-012 continua na matriz apontando para implementação “repositório API (externo)”.

## Alternativas

| Alternativa | Motivo de não adoção |
|-------------|----------------------|
| Monorepo (`src/VisionAssets.Api` neste repo) | Equipa prefere ciclo de vida e permissões de repositório distintos para o serviço servidor. |
