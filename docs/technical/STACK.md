# Stack técnica (proposta)

Valores abaixo são **padrão sugerido** até fixação por ADR.

| Área | Escolha | Observação |
|------|---------|------------|
| Linguagem | C# / **.NET 8** (LTS) | Ajustar se a política da empresa fixar outra LTS. |
| Runtime no cliente | **Self-contained** (preferência) ou framework-dependent | Trade-off: tamanho do MSI vs pré-requisito .NET. |
| Agente | **Worker Service** hospedado como **Windows Service** | |
| WMI | **Microsoft.Management.Infrastructure** (CIM) | Preferir a `System.Management` legada quando necessário para compatibilidade. |
| Dados | **SQLite** + **Microsoft.Data.Sqlite** | |
| ORM / acesso | **Dapper** ou ADO direto | Evitar EF Core se o time quiser binário menor e SQL explícito. |
| Migrações | Scripts versionados ou **FluentMigrator** | |
| Logs | **Serilog** (arquivo + opcional Event Log) | |
| MSI | **WiX Toolset** | Alternativa: Advanced Installer, etc. (ADR). |
| Testes | xUnit + testes de integração com SQLite in-memory | |

## Build e CI

- `dotnet build` na raiz (solução `VisionAssets.slnx`); `dotnet test` quando houver projetos de teste.
- Artefato: MSI em pasta `artifacts/`
- Versionamento alinhado a `docs/overview/CHANGELOG.md` e tags `v*`
