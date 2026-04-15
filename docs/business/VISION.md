# Visão de produto

## Problema

Organizações precisam **saber o que existe** no parque de máquinas Windows (hardware e software) para suporte, compras, auditoria e conformidade. Planilhas e inventários manuais ficam desatualizados; ferramentas genéricas nem sempre atendem políticas internas ou integração futura.

## Visão

Um **agente leve** instalado via **MSI**, distribuível pela rede corporativa, que **coleta de forma padronizada** informações de hardware e software, persiste em **SQLite local** e oferece base para relatórios e evolução (exportação, sincronização central).

## Stakeholders (típicos)

| Papel | Interesse |
|-------|-----------|
| TI / Operações | Implantação silenciosa, logs, estabilidade |
| Gestão de ativos / compras | Inventário confiável, ciclo de vida |
| Segurança / Compliance | Rastreio de software, redução de shadow IT (fase posterior) |
| Usuário final | Impacto mínimo (serviço em background) |

## Fora de escopo (MVP explícito)

- Painel web central obrigatório (pode ser fase posterior).
- Inventário de **licenças completas** para todos os fornecedores (limitação técnica do ecossistema Windows — ver [REQUIREMENTS.md](REQUIREMENTS.md)).
- Suporte a Linux/macOS no MVP (decisão futura via ADR).

## Indicadores de sucesso (sugestão)

- MSI instalável em massa com parâmetros documentados.
- Primeira coleta completa em >95% das máquinas piloto sem intervenção manual.
- Base SQLite consultável e exportável para auditoria interna.
