# Repositório da API central (VisionAssets.Api)

O código do **servidor HTTP** que recebe os snapshots de inventário vive num repositório **separado** do agente — [ADR-003](../decisions/ADR-003-api-repository-separate.md).

## Onde está o código

- **Nome do projeto:** `VisionAssets.Api`
- **Repositório remoto:** [github.com/SistemasLio/VisionAssets.Api](https://github.com/SistemasLio/VisionAssets.Api)
- **Agente (relacionado):** [github.com/SistemasLio/VisionAssets](https://github.com/SistemasLio/VisionAssets)
- **Clone local típico:** pasta irmã do agente (ex.: `Labs/VisionAssets.Api` junto a `Labs/VisionAssets`).

## Documentação no repositório da API

Ver o `README.md` na raiz de **VisionAssets.Api** (execução local, Entra ID, Swagger).

## Contrato partilhado

O ficheiro canónico do contrato HTTP/JSON permanece no repositório do agente: [inventory-v1.openapi.yaml](../contracts/inventory-v1.openapi.yaml).
