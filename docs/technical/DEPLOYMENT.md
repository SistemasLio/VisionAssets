# Implantação em rede — MSI do agente

Guia para administradores: pré-requisitos, instalação silenciosa e referência para **GPO**, **Microsoft Configuration Manager (SCCM)** e **Microsoft Intune**. Complementa [ADR-001](../decisions/ADR-001-wix-msi-framework-dependent.md) e o [README do repositório](https://github.com/SistemasLio/VisionAssets/blob/main/README.md).

## Pré-requisitos nos postos de trabalho

| Requisito | Detalhe |
|-----------|---------|
| SO | Windows 10 ou 11 **x64** (alinhado ao RID `win-x64` do pacote). |
| Runtime .NET | **.NET 8 Runtime (x64)** — o MSI atual é *framework-dependent*; sem o runtime, o serviço não arranca. [Transferência](https://dotnet.microsoft.com/download/dotnet/8.0). |
| Permissões | Instalação e desinstalação do MSI exigem **elevação** (conta com direitos de administrador local ou equivalente). |

**Pastas em produção** (após instalação pelo MSI, com `DOTNET_ENVIRONMENT` por defeito não-Development):

- **Logs:** `%ProgramData%\VisionAssets\Logs\`
- **SQLite:** `%ProgramData%\VisionAssets\Data\visionassets.db`

**Serviço Windows:** nome interno **VisionAssets Agent**; tipo de arranque **Automático** (definido no instalador WiX).

**Binários instalados:** `%ProgramFiles%\VisionAssets\Agent\` (ou `Program Files (x86)` conforme redirecionamento do sistema — o pacote é x64).

---

## Obter o MSI

1. **Build local (Windows):** na raiz do repositório  
   `dotnet build installer/VisionAssets.Installer/VisionAssets.Installer.wixproj -c Release`  
   Saída: `installer/VisionAssets.Installer/bin/Release/VisionAssets.Agent.msi`.

2. **CI:** no GitHub Actions, workflow **Build MSI** — artefato `VisionAssets.Agent-msi` com o ficheiro `.msi`.

Garanta que a **versão** do MSI (`ProductVersion` no pacote) corresponde à política de release da organização antes de distribuir em massa.

---

## Instalação silenciosa (`msiexec`)

Comandos típicos (executar como administrador; ajustar caminho do MSI):

```text
msiexec /i "C:\caminho\VisionAssets.Agent.msi" /qn /norestart
```

| Parâmetro | Significado |
|-----------|-------------|
| `/i` | Instalar. |
| `/qn` | Sem interface (silencioso). |
| `/norestart` | Não reiniciar automaticamente (recomendado em pacotes; combinar com política de reboot do ambiente). |

**Log de diagnóstico (recomendado em piloto):**

```text
msiexec /i "C:\caminho\VisionAssets.Agent.msi" /qn /norestart /L*v "%TEMP%\VisionAssets-Agent-install.log"
```

**Códigos de saída:** consulte a documentação da Microsoft para `msiexec` (0 = sucesso comum).

**Propriedades públicas:** o pacote WiX atual não expõe propriedades MSI personalizadas para caminhos ou intervalo de heartbeat; configuração pós-instalação pode evoluir (ficheiros `appsettings.json`, variáveis de ambiente, ou políticas futuras).

---

## Desinstalação silenciosa

Preferir **desinstalar pelo mesmo identificador de produto** registado no sistema, ou pela entrada em *Aplicações* do Windows.

Exemplo genérico com `msiexec` (substitua o GUID pelo **ProductCode** do produto instalado — pode variar entre versões do MSI):

```text
msiexec /x {XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX} /qn /norestart
```

Para localizar o ProductCode numa máquina de teste: PowerShell `Get-WmiObject Win32_Product` (uso moderado em produção) ou ferramentas de inventário do próprio Windows Installer / registo.

---

## Política de grupo (GPO) — atribuição de pacote

Visão geral (terminologia pode variar conforme versão do Windows Server / consola):

1. Criar ou editar um GPO ligado a **Computadores** (instalação por máquina).
2. **Configuração do computador → Políticas → Configurações do software → Instalação de software.**
3. **Novo → Pacote** — apontar para o ficheiro `.msi` numa partilha de rede (**UNC**), com permissões de leitura para contas de computador ou utilizadores conforme o modelo.
4. Tipo de implementação: **Atribuído** (instalação na próxima política ou arranque, conforme opções).
5. Forçar `gpupdate /force` em teste ou aguardar ciclo de política; pode ser necessário **reinício** para concluir a instalação atribuída.

Validar sempre num OU de piloto antes do anel completo.

---

## Microsoft Configuration Manager (SCCM / MECM)

Fluxo típico:

1. **Aplicações:** criar uma aplicação com o tipo **Windows Installer (MSI)** e apontar para `VisionAssets.Agent.msi`.
2. Preencher metadados (nome, versão, fabricante) alinhados ao inventário de software da organização.
3. **Tipo de implementação:** instalação para dispositivos Windows; comando de instalação pode ser o `msiexec` silencioso acima se precisar de personalização além do detetado automaticamente.
4. **Dependência:** garantir que o **.NET 8 Runtime x64** está implantado antes (ou como dependência da aplicação), por exemplo via outra aplicação ou baseline.
5. Implementar para uma **coleção** de piloto, monitorizar **Monitoring → Deployments**, e expandir.

---

## Microsoft Intune (Win32 app)

1. Empacotar o MSI num **Win32 Content Prep Tool** (`IntuneWin`) se a política interna exigir um único ficheiro para upload, **ou** distribuir o MSI conforme as opções disponíveis na subscrição.
2. **Instalar comando de exemplo:**  
   `msiexec /i "VisionAssets.Agent.msi" /qn /norestart`  
   (caminho relativo ao diretório extraído no cliente Intune.)
3. **Desinstalar:** usar o **ProductCode** da versão implantada ou o comando que o Intune gerar a partir do MSI.
4. **Requisitos:** SO Windows 10/11 64 bits; regra de deteção (por exemplo versão de ficheiro em `Program Files\VisionAssets\Agent\VisionAssets.Agent.exe` ou chave de registo do MSI).
5. Dependência explícita do **.NET 8 Runtime** como aplicação pré-requisito ou script de verificação.

---

## Atualização entre versões

O instalador WiX inclui regra de **Major Upgrade** (mesmo `UpgradeCode`, versão superior). Em geral, instalar o MSI mais recente substitui a versão anterior. Em ambientes muito restritivos, teste o caminho de upgrade numa VM antes do anel de produção.

---

## Resolução de problemas (resumo)

| Sintoma | Verificar |
|---------|-----------|
| Serviço não inicia | .NET 8 Runtime x64 instalado; permissões em `%ProgramData%\VisionAssets`; ficheiros em `Program Files\VisionAssets\Agent` intactos. |
| Sem logs | Pasta `%ProgramData%\VisionAssets\Logs` criada após primeiro arranque; permissões de escrita do contexto do serviço (normalmente **Local System**). |
| MSI falha | Log `/L*v`; espaço em disco; conflito com instalação antiga manual (`sc delete` apenas após desinstalação correta ou orientação de suporte). |

Eventos da aplicação e do serviço: **Visualizador de Eventos do Windows** (origens relacionadas com .NET e Serviço).

---

## Referências externas

- [msiexec](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/msiexec) (Microsoft Learn)
- [Implantação de aplicações Win32 na Microsoft Intune](https://learn.microsoft.com/en-us/mem/intune/apps/apps-win32-app-management) (Microsoft Learn)
