import { defineConfig } from 'vitepress'
import { withMermaid } from 'vitepress-plugin-mermaid'

export default withMermaid(
  defineConfig({
    title: 'VisionAssets',
    description: 'Documentação do projeto VisionAssets — inventário de hardware e software no Windows.',
    lang: 'pt-BR',
    base: '/VisionAssets/',
    // GitHub Pages não faz rewrite de URLs “clean”; sem .html pode dar 404.
    cleanUrls: false,
    lastUpdated: true,
    themeConfig: {
      // Caminho relativo ao base (/VisionAssets/) — evita 404 no primeiro paint em Pages.
      logo: 'favicon.svg',
      nav: [
        { text: 'Início', link: '/' },
        { text: 'Índice', link: '/README' },
        {
          text: 'Repositório',
          link: 'https://github.com/SistemasLio/VisionAssets',
        },
      ],
      outline: {
        level: [2, 3],
      },
      search: {
        provider: 'local',
      },
      sidebar: [
        {
          text: 'Visão geral',
          items: [
            { text: 'Início', link: '/' },
            { text: 'Índice da documentação', link: '/README' },
            { text: 'Governança', link: '/GOVERNANCE' },
            { text: 'Contexto do projeto', link: '/overview/CONTEXT' },
            { text: 'Agentes (IA)', link: '/overview/AGENTS' },
            { text: 'Changelog', link: '/overview/CHANGELOG' },
          ],
        },
        {
          text: 'Negócio',
          items: [
            { text: 'Visão', link: '/business/VISION' },
            { text: 'Requisitos', link: '/business/REQUIREMENTS' },
            { text: 'Glossário', link: '/business/GLOSSARY' },
            { text: 'Questões em aberto', link: '/business/OPEN-QUESTIONS' },
          ],
        },
        {
          text: 'Produto',
          items: [
            { text: 'Roadmap', link: '/product/ROADMAP' },
            { text: 'Backlog', link: '/product/BACKLOG-OVERVIEW' },
            { text: 'Template de história', link: '/product/templates/USER-STORY' },
            { text: 'Histórias', link: '/product/stories/README' },
          ],
        },
        {
          text: 'Técnico',
          items: [
            { text: 'Arquitetura', link: '/technical/ARCHITECTURE' },
            { text: 'Stack', link: '/technical/STACK' },
            { text: 'Modelo de dados', link: '/technical/DATA-MODEL' },
            { text: 'Coleta de inventário', link: '/technical/INVENTORY-COLLECTION' },
          ],
        },
        {
          text: 'Rastreio e decisões',
          items: [
            { text: 'Matriz de rastreabilidade', link: '/traceability/TRACEABILITY-MATRIX' },
            { text: 'ADRs', link: '/decisions/README' },
            { text: 'Template ADR', link: '/decisions/ADR-000-template' },
            { text: 'Como escrever changelog', link: '/changelog/README' },
          ],
        },
      ],
      socialLinks: [
        {
          icon: 'github',
          link: 'https://github.com/SistemasLio/VisionAssets',
        },
      ],
      footer: {
        message: 'Documentação do repositório VisionAssets.',
        copyright: 'Conteúdo em Markdown versionado no Git.',
      },
    },
  }),
  {
    mermaid: {
      theme: 'neutral',
      themeVariables: {
        primaryColor: '#e8eef4',
        primaryTextColor: '#1a1a1a',
        lineColor: '#5c6b7a',
      },
    },
  },
)
