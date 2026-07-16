# Desafio Prático - Sistema de Controle de Gastos Residenciais

Sistema para gerenciamento de pessoas e suas transações financeiras (receitas e despesas), com consulta de totais individuais e gerais.

## Tecnologias

- **Backend:** .NET 10 (C#), ASP.NET Core Web API, Entity Framework Core, SQLite
- **Frontend:** React + TypeScript, Vite

## Estrutura do projeto

## Como rodar o Backend


```bash
cd Backend
dotnet restore
dotnet ef database update   # cria o banco SQLite e aplica as migrations
dotnet watch run
```

A API sobe em `http://localhost:5129` (configurável em `Properties/launchSettings.json`).

## Como rodar o Frontend

```bash
cd Frontend
npm install
npm run dev
```

O frontend sobe em `http://localhost:5173` e já está configurado para consumir a API em `http://localhost:5129`.

**Importante:** o backend precisa estar rodando antes do frontend, senão as requisições vão falhar.

## Funcionalidades

### Pessoas
- Criar, listar e deletar pessoas (identificador, nome, idade gerados/geridos automaticamente).
- Ao deletar uma pessoa, todas as transações associadas são deletadas em cascata.

### Transações
- Criar e listar transações (descrição, valor, tipo, pessoa associada).
- Pessoas menores de 18 anos só podem registrar transações do tipo **Despesa**.

### Totais
- Listagem de receitas, despesas e saldo por pessoa.
- Total geral consolidado de todas as pessoas.

