# Repositório da API central (VisionAssets.Api)

O código do **servidor HTTP** que recebe os snapshots de inventário vive num repositório **separado** do agente — [ADR-003](../decisions/ADR-003-api-repository-separate.md).

## Onde está o código

- **Nome do projeto:** `VisionAssets.Api`
- **Localização de desenvolvimento (máquina do autor):** pasta irmã do clone do agente, por exemplo `Labs/VisionAssets.Api` junto a `Labs/VisionAssets`.
- **Git:** criar um repositório remoto (ex.: `github.com/SistemasLio/VisionAssets.Api`) e fazer push a partir dessa pasta; o agente continua em `github.com/SistemasLio/VisionAssets`.

## Documentação no repositório da API

Ver o `README.md` na raiz de **VisionAssets.Api** (execução local, Entra ID, Swagger).

## Contrato partilhado

O ficheiro canónico do contrato HTTP/JSON permanece no repositório do agente: [inventory-v1.openapi.yaml](../contracts/inventory-v1.openapi.yaml).
