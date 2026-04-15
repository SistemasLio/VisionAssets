# Coleta de inventário — fontes e práticas

**Implementação no código:** `src/VisionAssets.Inventory/` (`HardwareCollector`, `SoftwareCollector`, `OperatingSystemCollector`).

## Hardware (WMI/CIM)

Classes típicas (não exaustivo):

| Área | Classe / notas |
|------|----------------|
| CPU | `Win32_Processor` |
| Memória | `Win32_PhysicalMemory` |
| Disco | `Win32_DiskDrive`, `Win32_LogicalDisk` |
| BIOS / placa | `Win32_BIOS`, `Win32_BaseBoard` |
| Rede | `Win32_NetworkAdapter` (filtrar não físicos) |
| GPU | `Win32_VideoController` |

**Prática**: preferir **CIM** (`Get-CimInstance` equivalente em API) e tratar **permissão** e **timeout** em consultas.

## Software — Registry

Chaves principais:

- `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall`
- `HKLM\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall`
- Opcional por usuário: `HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall`

Ler valores como `DisplayName`, `DisplayVersion`, `Publisher`, `InstallDate`, `UninstallString`.

## Software — o que evitar em rotina

- **`Win32_Product`**: pode ser **lenta** e causar **consistência MSI** indesejada em alguns cenários. Reservar para diagnóstico manual ou ferramenta separada, não para inventário horário.

## Microsoft Store / AppX

Se o escopo exigir: complementar com API de pacotes (decisão em ADR se for obrigatório no MVP).

## Licenças

- **Windows / Office**: em muitos casos só **status** ou **tipo**; chaves podem não existir no registro.
- **Plano**: detecção heurística + **LicenseRecord** com `detection_method` e **importação manual** para contratos.

**Decisão D-002**: não depender de `Win32_Product` para inventário padrão — alinhado a REQ-008.

## Performance

- Executar consultas em **paralelo controlado** (limite de concorrência) para não saturar WMI.
- Cache curto de dados que não mudam entre runs (ex.: BIOS) — opcional, avaliar em ADR.
