import fs from 'node:fs'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

const root = path.dirname(fileURLToPath(import.meta.url))
const dist = path.join(root, '..', 'docs', '.vitepress', 'dist', '.nojekyll')
fs.writeFileSync(
  dist,
  '# Desativa o processamento Jekyll no GitHub Pages.\n',
  'utf8',
)
