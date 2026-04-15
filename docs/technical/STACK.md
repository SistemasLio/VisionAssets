# Stack técnica (proposta)

Valores abaixo são **padrão sugerido** até fixação por ADR.

| Área | Escolha | Observação |
|------|---------|------------|
| Linguagem | C# / **.NET 8** (LTS) | Ajustar se a política da empresa fixar outra LTS. |
| Runtime no cliente | **Framework-dependent** no MSI atual (ADR-001); self-contained como evolução | Trade-off: tamanho do MSI vs pré-requisito .NET. |
| Agente | **Worker Service** hospedado como **Windows Service** | |
| WMI | **Microsoft.Management.Infrastructure** (CIM) | Preferir a `System.Management` legada quando necessário para compatibilidade. |
| Dados | **SQLite** + **Microsoft.Data.Sqlite** | Implementado em `VisionAssets.Persistence`. |
| ORM / acesso | **Dapper** | Repositórios em `VisionAssets.Persistence`. |
| Migrações | SQL embutido + `schema_migrations` | `MigrationRunner`, ficheiros `Migrations/00N_*.sql`. |
| Logs | **Serilog** (arquivo + opcional Event Log) | |
| MSI | **WiX Toolset** v5 (`installer/VisionAssets.Installer`) | [ADR-001](../decisions/ADR-001-wix-msi-framework-dependent.md). |
| Sync / Entra ID | **MSAL.NET** + HTTPS (`VisionAssets.Sync`) | [ADR-002](../decisions/ADR-002-entra-id-central-api.md); API noutro repositório ([ADR-003](../decisions/ADR-003-api-repository-separate.md)). |
| Testes | xUnit + testes de integração com SQLite in-memory | |

## Build e CI

- `dotnet build` na raiz (solução `VisionAssets.slnx`); `dotnet test` quando houver projetos de teste.
- Artefato MSI: `installer/VisionAssets.Installer/bin/Release/VisionAssets.Agent.msi` (build explícito do `.wixproj`); `dotnet publish` intermédio em `artifacts/agent-publish/` (ignorado pelo Git).
- Versionamento alinhado a `docs/overview/CHANGELOG.md` e tags `v*`
