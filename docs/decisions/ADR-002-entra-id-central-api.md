# ADR-002 — Backend central greenfield, Microsoft Entra ID e identidade de máquina

## Status

Aceite

## Contexto

- **REQ-012**: evolução para sincronização com servidor central.
- Backend **greenfield** (equipa implementa API e persistência do zero).
- A organização utiliza **Microsoft Entra ID** (anteriormente Azure AD) — autenticação corporativa deve integrar-se a este IdP.
- Escala alvo: **~400 estações**; inventário **2–3 vezes por semana** por máquina (baixa frequência; payloads completos são aceitáveis sem otimização agressiva de compressão no primeiro incremento).
- Política de dados: **enviar o máximo possível** do inventário (hardware, software, identificadores úteis) **sem restrições adicionais** nesta fase (alinhado a auditoria e CMDB interno).

## Decisão

### 1. Autenticação e autorização (Entra ID)

- **Fluxo OAuth 2.0 [client credentials](https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-client-creds-grant-flow)** entre o agente (cliente confidencial) e a API protegida.
- **App registration** no Entra ID (modo **single-tenant** com o `tenant_id` da organização, salvo decisão futura de multi-tenant):
  - Uma aplicação **“VisionAssets Agent”** (daemon) com credenciais **certificado X.509** (preferido) ou **client secret** (aceitável em piloto, com rotação documentada).
  - Uma aplicação **“VisionAssets API”** (recurso) que **expõe scopes** / **app roles**; a primeira solicita permissão de aplicação à segunda (**admin consent** uma vez no tenant).
- Tokens emitidos por `login.microsoftonline.com/{tenant}`; a API valida **issuer**, **audience** e **lifetime** (JWT Bearer via middleware padrão ASP.NET Core / Microsoft.Identity.Web).
- Tráfego **HTTPS apenas** (TLS 1.2+); URL base da API configurável (`Backend:BaseUrl`).

### 2. Identidade da máquina (“blindagem” em camadas)

Nenhum identificador sozinho cobre todos os cenários; usar **modelo em camadas**:

| Campo | Origem | Uso |
|--------|--------|-----|
| `machine_id` | GUID estável já persistido no SQLite local (`MachineRepository`) | **Chave primária lógica** no servidor; estável entre reinstalações do agente se o ficheiro de dados for preservado. |
| `machine_fingerprint` (opcional, fase posterior) | Hash derivado de atributos de hardware (ex.: serial de placa + disco sistema) | Desambiguação se `machine_id` for regenerado. |
| `entra_tenant_id` | Configuração (`Backend:TenantId`) | Correlação explícita ao tenant. |
| `azure_ad_device_id` | Quando disponível: leitura do identificador de dispositivo Azure AD / Entra no Windows (ex.: estado de junção híbrida / Entra join — APIs ou registo conforme [documentação Microsoft](https://learn.microsoft.com/en-us/azure/active-directory/devices/overview)) | **Correlação forte** com o objeto dispositivo no Entra ID; recomendado para CMDB e relatórios de conformidade. |
| `device_name` / `dns_hostname` | WMI / já coletado | Exibição e cruzamento com Intune / SCCM. |

O servidor **persiste** `machine_id` como chave principal; preenche `azure_ad_device_id` quando o cliente envia e valida consistência ao longo do tempo (alertas se mudar sem explicação).

**Nota:** com **client credentials**, o token **não** identifica o PC — a identificação é sempre o **payload** assinado por TLS + política de rede. Reforços futuros (ADR filho): **certificado por máquina**, **Azure Arc** / **managed identity** em VMs Azure, ou **mTLS** com PKI interna.

### 3. Formato de integração

- **Snapshots JSON** versionados (`schema_version`), alinhados ao [contrato OpenAPI](../contracts/inventory-v1.openapi.yaml) (evolução de PBI-041).
- **Idempotência:** cabeçalho `Idempotency-Key` recomendado (`{machine_id}:{inventory_run_id}`) para repetir envios sem duplicar linhas de negócio.
- **Frequência:** 2–3×/semana — fila local com **retry exponencial** suficiente; compressão **gzip** opcional em iteração posterior (baixa prioridade).

### 4. Backend greenfield

- API **nova** (ex.: ASP.NET Core 8), hospedagem à escolha da equipa (Azure App Service, AKS, VM, etc.) — fora do âmbito deste ADR.
- Persistência central (SQL Server, PostgreSQL, …) com esquema derivado do [DATA-MODEL.md](../technical/DATA-MODEL.md) + extensões (`azure_ad_device_id`, timestamps de receção).

### 5. Agente (incrementos futuros)

- Biblioteca **MSAL.NET** para obter tokens; cache em disco protegido quando aplicável.
- Após sucesso na persistência local, **outbox** (tabela ou colunas em `inventory_run`) para envios falhados / offline.

## Consequências

- Dependência de **consentimento de administrador** no Entra e de **pipeline seguro** para distribuir certificado ou segredo do cliente (GPO, Intune, Credential Guard).
- Operação de **rotação** de segredos/certificados documentada em [DEPLOYMENT.md](../technical/DEPLOYMENT.md) (secção futura “Backend / Entra”).
- REQ-012 passa a ter **implementação alvo** explícita (ver EPIC-006 no backlog).

## Alternativas consideradas

| Alternativa | Motivo de não adoção no primeiro incremento |
|-------------|---------------------------------------------|
| Apenas API key estática | Menos integração com políticas Entra e auditoria centralizada. |
| Utilizador interativo (authorization code) | Inadequado a serviço Windows sem utilizador. |
| Managed Identity em todas as estações | Aplicável sobretudo a cargas Azure; postos genéricos Windows usam client credentials + identificadores no payload. |
