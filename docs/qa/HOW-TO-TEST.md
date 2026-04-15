# Como testar

Guias práticos e **casos de teste (TC-xxx)** reproduzíveis. Atualizar este ficheiro sempre que mudar comportamento visível ou fluxo de instalação.

## Pré-requisitos gerais

- Windows 10/11 para o agente
- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) (ou compatível com a solução)
- Node.js 20+ apenas para o portal de documentação
- Opcional: [DB Browser for SQLite](https://sqlitebrowser.org/) para inspecionar a base

---

## TC-DOC-01 — Portal de documentação (local)

**UC:** UC-DOC-001  

1. Na raiz do repositório: `npm install` (primeira vez).
2. `npm run docs:build` — deve concluir sem erros; pasta `docs/.vitepress/dist` gerada (ignorada pelo Git).
3. `npm run docs:preview` e abrir a URL indicada no terminal (incluir o path `/VisionAssets/` se aplicável).
4. Confirmar: página inicial carrega, barra lateral com secções, busca (`Ctrl+K`) devolve resultados, links internos abrem.

**Resultado esperado:** site estático servido sem 404 nos fluxos principais.

---

## TC-DOC-02 — Portal (GitHub Pages)

**UC:** UC-DOC-002  

1. Confirmar que **Settings → Pages → Source: GitHub Actions** está ativo.
2. Após push em `main`, verificar workflow **Deploy documentation** concluído com sucesso.
3. Abrir `https://sistemaslio.github.io/VisionAssets/` (ajustar org/repo se diferente).

**Resultado esperado:** mesmo conteúdo essencial que em pré-visualização local.

---

## TC-AGENT-01 — Agente em Development

**UC:** UC-AGENT-001  

1. Fechar qualquer instância anterior do `VisionAssets.Agent.exe` (evita lock ao compilar).
2. `dotnet build` na raiz da solução.
3. `dotnet run --project src/VisionAssets.Agent`
4. Verificar no console: mensagem de arranque com caminho dos **logs** e do **ficheiro SQLite**.
5. Confirmar criação de pasta `Logs/` e ficheiros `visionassets-YYYYMMDD.log` junto ao executável (output típico `bin/Debug/net8.0-windows`).
6. Parar com `Ctrl+C`.

**Resultado esperado:** arranque limpo, logs Serilog em disco e consola, encerramento sem excepção.

---

## TC-AGENT-02 — Registo como serviço Windows (manual)

**UC:** UC-AGENT-002  

1. `dotnet publish src/VisionAssets.Agent -c Release -o publish/agent`
2. Como administrador: criar serviço com `sc create` apontando para `VisionAssets.Agent.exe` publicado (nome do serviço com espaço: `sc create "VisionAssets Agent" binPath= "..."` — ver [README](https://github.com/SistemasLio/VisionAssets/blob/main/README.md)).
3. `sc start "VisionAssets Agent"` e verificar logs em `%ProgramData%\VisionAssets\Logs`.
4. `sc stop "VisionAssets Agent"` / `sc delete "VisionAssets Agent"` para limpeza de teste.

**Resultado esperado:** serviço arranca e escreve logs no caminho de produção.

---

## TC-INV-02 — Inventário WMI + Registry no SQLite

**UC:** UC-INV-001  

1. Garantir **TC-INV-01** já executado ou base válida.
2. `dotnet run --project src/VisionAssets.Agent` e aguardar pelo menos um ciclo (em Development, 1 minuto entre ciclos ou reiniciar para forçar nova execução imediata ao subir).
3. Abrir `visionassets.db` no caminho do output **net8.0-windows** (ver nota em [TC-INV-01](#tc-inv-01--sqlite-migrações-e-inventory_run)) e verificar:
   - `machine.os_name` / `os_version` preenchidos (caption do Windows).
   - Várias linhas em `hardware_component` com `category` (CPU, RAM, DISK, GPU, …).
   - Várias linhas em `installed_software` com `name` não vazio.
4. Opcional: `Agent:IncludeCurrentUserUninstallKeys` = `true` em `appsettings.Development.json` e repetir — contagem em `installed_software` pode aumentar.

**Resultado esperado:** snapshot completo por execução; cada ciclo substitui hardware/software da máquina (sem histórico incremental entre ciclos).

---

## TC-INV-01 — SQLite: migrações e `inventory_run`

**UC:** UC-INV-002  

1. Executar **TC-AGENT-01** pelo menos até criar a base (ou apagar `Data/visionassets.db` no output para forçar migração limpa).
2. Abrir `visionassets.db` na pasta `Data/` junto ao executável (tipicamente `bin/Debug/net8.0-windows/Data/visionassets.db`; evitar confundir com outro output, por exemplo `net8.0/Data/`).
3. Confirmar tabelas: `schema_migrations`, `machine`, `inventory_run`, etc.
4. Confirmar pelo menos uma linha em `machine` (hostname) e linhas em `inventory_run` com `status` `success` após um ciclo de heartbeat (intervalo pode ser 1 min em Development).
5. Confirmar `agent_version` preenchido em `inventory_run`.

**Resultado esperado:** esquema alinhado a [DATA-MODEL.md](../technical/DATA-MODEL.md); execuções registadas.

---

## TC-MSI-01 — Instalação pelo MSI

**UC:** UC-AGENT-002 (variante instalador)

**Pré-requisitos:** VM ou posto de teste x64 com Windows 10/11; **.NET 8 Runtime x64** instalado ([transferência](https://dotnet.microsoft.com/download/dotnet/8.0)).

1. Obter `VisionAssets.Agent.msi` (build local do projeto WiX ou artefato do workflow **Build MSI** no GitHub Actions).
2. Como administrador: `msiexec /i "C:\caminho\VisionAssets.Agent.msi" /qn /norestart` (opcional: `/L*v %TEMP%\va-msi.log` para diagnóstico).
3. Confirmar em `services.msc` que o serviço **VisionAssets Agent** existe e está **Em execução** (ou iniciar manualmente após política de reboot).
4. Confirmar pastas `%ProgramFiles%\VisionAssets\Agent\` (binários) e, após um ciclo, `%ProgramData%\VisionAssets\Logs\` e `%ProgramData%\VisionAssets\Data\visionassets.db`.
5. Limpeza de teste: desinstalar pela aplicação *Aplicações* do Windows ou `msiexec /x {ProductCode} /qn` (ProductCode conforme versão instalada).

**Resultado esperado:** instalação silenciosa sem erros; serviço registado e dados de inventário possíveis na base local. Detalhes e cenários GPO/SCCM/Intune: [DEPLOYMENT.md](../technical/DEPLOYMENT.md).

---

## TC-BUILD-01 — Compilação da solução

1. Na raiz: `dotnet build`
2. Opcional: `dotnet build -c Release`

**Resultado esperado:** 0 erros; projetos `VisionAssets.Agent` e `VisionAssets.Persistence` compilados.

---

## Regressão rápida antes de merge

- [ ] TC-BUILD-01
- [ ] TC-AGENT-01 (se alterou agente ou persistência)
- [ ] TC-INV-01 (se alterou SQLite ou migrações)
- [ ] TC-INV-02 (se alterou coleta WMI/Registry ou snapshots)
- [ ] TC-DOC-01 (se alterou `docs/` ou VitePress)
- [ ] TC-MSI-01 (se alterou WiX, MSI ou [DEPLOYMENT.md](../technical/DEPLOYMENT.md))
