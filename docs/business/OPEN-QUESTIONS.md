# Questões em aberto (para decisão)

Registro vivo de **decisões pendentes**; ao fechar, mover resumo para [../decisions/](../decisions/) (ADR) e remover ou marcar como resolvido aqui.

| ID | Tema | Opções | Impacto | Responsável sugerido |
|----|------|--------|---------|----------------------|
| Q-001 | Versões mínimas do Windows | Apenas Win10 22H2+ vs incluir Server | Testes e matriz de suporte | Produto + Eng |
| Q-002 | Self-contained vs framework-dependent MSI | Tamanho (~50–100MB+) vs dependência de runtime .NET | Deploy e atualizações | Eng |
| Q-003 | Coleta por usuário (HKCU) | Habilitado por padrão vs opt-in | Privacidade e volume de dados | Segurança + Produto |
| Q-004 | Frequência padrão do inventário | Diária / semanal / só boot | Carga WMI e frescor dos dados | Operações |
| Q-005 | Armazenar serial de hardware | Sim completo vs mascarado vs só hash | LGPD / auditoria física | Jurídico + Segurança |
| Q-006 | Ferramenta de MSI | WiX vs comercial | CI e custo de licença | Eng |

## Como usar este arquivo

1. Discutir em reunião ou assíncrono.
2. Registrar decisão em **ADR** com alternativas.
3. Atualizar **REQUIREMENTS.md** ou **STACK.md** se algo virar requisito não negociável.
