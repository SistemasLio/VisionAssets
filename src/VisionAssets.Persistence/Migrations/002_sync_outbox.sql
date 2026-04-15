-- EPIC-006: fila local de envio ao API central (retry).

CREATE TABLE IF NOT EXISTS sync_outbox (
  id TEXT NOT NULL PRIMARY KEY,
  machine_id TEXT NOT NULL,
  inventory_run_id TEXT NOT NULL,
  payload_json TEXT NOT NULL,
  status TEXT NOT NULL,
  created_at TEXT NOT NULL,
  last_attempt_at TEXT,
  attempt_count INTEGER NOT NULL DEFAULT 0,
  last_error TEXT,
  FOREIGN KEY (machine_id) REFERENCES machine(id) ON DELETE CASCADE,
  UNIQUE (machine_id, inventory_run_id)
);

CREATE INDEX IF NOT EXISTS idx_sync_outbox_pending ON sync_outbox(status, created_at);
