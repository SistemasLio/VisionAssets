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
5. Confirmar criação de pasta `Logs/` e ficheiros `visionassets-YYYYMMDD.log` junto ao executável (output `bin/Debug/net8.0` ou equivalente).
6. Parar com `Ctrl+C`.

**Resultado esperado:** arranque limpo, logs Serilog em disco e consola, encerramento sem excepção.

---

## TC-AGENT-02 — Registo como serviço Windows (manual)

**UC:** UC-AGENT-002  

1. `dotnet publish src/VisionAssets.Agent -c Release -o publish/agent`
2. Como administrador: criar serviço com `sc create` apontando para `VisionAssets.Agent.exe` publicado (instruções na raiz do repositório, ficheiro `README.md`).
3. `sc start <nomeDoServiço>` e verificar logs em `%ProgramData%\VisionAssets\Logs`.
4. `sc stop` / `sc delete` para limpeza de teste.

**Resultado esperado:** serviço arranca e escreve logs no caminho de produção.

---

## TC-INV-02 — Inventário WMI + Registry no SQLite

**UC:** UC-INV-001  

1. Garantir **TC-INV-01** já executado ou base válida.
2. `dotnet run --project src/VisionAssets.Agent` e aguardar pelo menos um ciclo (em Development, 1 minuto entre ciclos ou reiniciar para forçar nova execução imediata ao subir).
3. Abrir `visionassets.db` e verificar:
   - `machine.os_name` / `os_version` preenchidos (caption do Windows).
   - Várias linhas em `hardware_component` com `category` (CPU, RAM, DISK, GPU, …).
   - Várias linhas em `installed_software` com `name` não vazio.
4. Opcional: `Agent:IncludeCurrentUserUninstallKeys` = `true` em `appsettings.Development.json` e repetir — contagem em `installed_software` pode aumentar.

**Resultado esperado:** snapshot completo por execução; cada ciclo substitui hardware/software da máquina (sem histórico incremental entre ciclos).

---

## TC-INV-01 — SQLite: migrações e `inventory_run`

**UC:** UC-INV-002  

1. Executar **TC-AGENT-01** pelo menos até criar a base (ou apagar `Data/visionassets.db` no output para forçar migração limpa).
2. Abrir `Data/visionassets.db` com ferramenta SQLite.
3. Confirmar tabelas: `schema_migrations`, `machine`, `inventory_run`, etc.
4. Confirmar pelo menos uma linha em `machine` (hostname) e linhas em `inventory_run` com `status` `success` após um ciclo de heartbeat (intervalo pode ser 1 min em Development).
5. Confirmar `agent_version` preenchido em `inventory_run`.

**Resultado esperado:** esquema alinhado a [DATA-MODEL.md](../technical/DATA-MODEL.md); execuções registadas.

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
