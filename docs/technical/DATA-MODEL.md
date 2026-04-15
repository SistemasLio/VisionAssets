# Modelo de dados (SQLite) — esboço

Esquema evolui por **migrações**; este documento descreve a intenção. Ajustar nomes de colunas ao implementar.

## Entidades principais

### Machine

Identifica o endpoint.

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | TEXT PK | UUID |
| hostname | TEXT | |
| domain | TEXT | opcional |
| os_name | TEXT | |
| os_version | TEXT | build |
| last_seen_at | TEXT ISO8601 | |

### HardwareComponent

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | TEXT PK | |
| machine_id | TEXT FK | |
| category | TEXT | CPU, RAM, DISK, GPU, NET, BIOS, … |
| manufacturer | TEXT | |
| model | TEXT | |
| serial | TEXT | cuidado com LGPD |
| details_json | TEXT | flexível |

### InstalledSoftware

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | TEXT PK | |
| machine_id | TEXT FK | |
| name | TEXT | |
| version | TEXT | |
| publisher | TEXT | |
| install_date | TEXT | se disponível |
| source | TEXT | Registry path / outro |
| evidence_json | TEXT | opcional |

### LicenseRecord (opcional no MVP)

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | TEXT PK | |
| machine_id | TEXT FK | |
| product | TEXT | |
| license_type | TEXT | |
| key_last4 | TEXT | nunca chave completa sem política |
| detection_method | TEXT | auto / manual / import |
| confidence | TEXT | high / medium / low |

### InventoryRun

| Campo | Tipo | Descrição |
|-------|------|-----------|
| id | TEXT PK | |
| machine_id | TEXT FK | |
| started_at | TEXT | |
| finished_at | TEXT | |
| status | TEXT | `running` (em curso), `success`, `failed` |
| agent_version | TEXT | |
| error_message | TEXT | sanitizado |

## Índices sugeridos

- `InstalledSoftware(machine_id, name, version, publisher)`
- `HardwareComponent(machine_id, category)`
- `InventoryRun(machine_id, started_at DESC)`

## Decisão registrada

- **D-001**: SQLite como store local no MVP — ver [CONTEXT.md](../overview/CONTEXT.md).
