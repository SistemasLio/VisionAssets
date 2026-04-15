-- VisionAssets esquema inicial (EPIC-002). SQLite: FKs exigem PRAGMA foreign_keys=ON na conexão.

CREATE TABLE IF NOT EXISTS machine (
  id TEXT NOT NULL PRIMARY KEY,
  hostname TEXT NOT NULL,
  domain TEXT,
  os_name TEXT,
  os_version TEXT,
  last_seen_at TEXT
);

CREATE TABLE IF NOT EXISTS hardware_component (
  id TEXT NOT NULL PRIMARY KEY,
  machine_id TEXT NOT NULL,
  category TEXT NOT NULL,
  manufacturer TEXT,
  model TEXT,
  serial TEXT,
  details_json TEXT,
  FOREIGN KEY (machine_id) REFERENCES machine(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_hardware_machine_category ON hardware_component(machine_id, category);

CREATE TABLE IF NOT EXISTS installed_software (
  id TEXT NOT NULL PRIMARY KEY,
  machine_id TEXT NOT NULL,
  name TEXT,
  version TEXT,
  publisher TEXT,
  install_date TEXT,
  source TEXT,
  evidence_json TEXT,
  FOREIGN KEY (machine_id) REFERENCES machine(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_software_dedup ON installed_software(machine_id, name, version, publisher);

CREATE TABLE IF NOT EXISTS license_record (
  id TEXT NOT NULL PRIMARY KEY,
  machine_id TEXT NOT NULL,
  product TEXT,
  license_type TEXT,
  key_last4 TEXT,
  detection_method TEXT,
  confidence TEXT,
  FOREIGN KEY (machine_id) REFERENCES machine(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS inventory_run (
  id TEXT NOT NULL PRIMARY KEY,
  machine_id TEXT NOT NULL,
  started_at TEXT NOT NULL,
  finished_at TEXT,
  status TEXT NOT NULL,
  agent_version TEXT,
  error_message TEXT,
  FOREIGN KEY (machine_id) REFERENCES machine(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_run_machine_started ON inventory_run(machine_id, started_at DESC);
