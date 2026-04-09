# MatuTI — Avaliação de Maturidade de TI

Sistema web para avaliação de maturidade de TI em empresas, desenvolvido em ASP.NET Core Razor Pages.

## Requisitos

- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- EF Core Tools: `dotnet tool install --global dotnet-ef`

## Como rodar

```bash
# 1. Entre na pasta do projeto
cd MatuTI

# 2. Restaure os pacotes
dotnet restore

# 3. Crie a migration inicial
dotnet ef migrations add Inicial

# 4. Aplique a migration (cria o banco SQLite)
dotnet ef database update

# 5. Rode a aplicação
dotnet run
```

Acesse: **https://localhost:5001** ou **http://localhost:5000**

## Deploy no Render com Docker

Este projeto já vem com `Dockerfile` para publicação como container no Render.

### Passo a passo

1. Suba este repositório para o GitHub.
2. No Render, crie um **Web Service** e escolha a opção **Docker**.
3. Aponte para o repositório e deixe o Render usar o `Dockerfile` da raiz.
4. Crie um **Persistent Disk** e monte em `/var/data`.
5. Configure a variável de ambiente `DB_PATH` como `/var/data/matuti.db`.
6. Faça o deploy.

### Observações importantes

- O app já escuta a variável `PORT`, então funciona no Render sem ajustes extras de porta.
- O banco usa SQLite. Sem disco persistente, os dados podem ser perdidos em novos deploys.
- Na primeira inicialização, o app executa as migrations e o seed automaticamente.

### Variáveis úteis

| Variável | Valor sugerido |
|----------|----------------|
| `DB_PATH` | `/var/data/matuti.db` |

### `Dockerfile`

O container compila o projeto com o SDK .NET 8 e executa a aplicação com o runtime ASP.NET 8.

## Credenciais padrão

| Campo  | Valor              |
|--------|--------------------|
| E-mail | admin@matuti.com   |
| Senha  | Admin@123          |

## Funcionalidades

### Perfis
- **Admin** — acesso total: empresas, questões, usuários, avaliações
- **Avaliador** — inicia e responde avaliações, visualiza relatórios

### Módulos
1. **Login / Registro** — autenticação com perfis
2. **Empresas** — CRUD de empresas clientes
3. **Questões** — banco de perguntas por indicador com pesos
4. **Avaliações** — fluxo completo: iniciar → responder → relatório
5. **Relatório** — score geral, score por indicador, nível de maturidade, pontos fortes e lacunas com planos de ação

### Níveis de Maturidade
| Score     | Nível        |
|-----------|--------------|
| 0 – 59%   | Artesanal    |
| 60 – 79%  | Eficiente    |
| 80 – 89%  | Eficaz       |
| 90 – 100% | Estratégico  |

### Indicadores avaliados
1. Maturidade dos Processos de TI
2. Uso de Ferramentas de TI
3. Nível de Serviço
4. Alinhamento Estratégico
5. Governança de TI
6. Gestão de Riscos
7. Cultura de TI

## Estrutura do projeto

```
MatuTI/
├── Data/
│   ├── AppDbContext.cs       # EF Core + Identity
│   └── Seed.cs               # Dados iniciais (admin + questões)
├── Models/
│   ├── ApplicationUser.cs
│   ├── Empresa.cs
│   ├── Avaliacao.cs          # Inclui Resposta
│   └── Questao.cs
├── Services/
│   └── ScoreCalculator.cs    # Cálculo de score e níveis
├── Pages/
│   ├── Auth/                 # Login, Registro, Logout
│   ├── Empresas/             # CRUD de empresas
│   ├── Questoes/             # CRUD de questões
│   ├── Avaliacao/            # Iniciar, Responder, Relatório
│   └── Admin/                # Gestão de usuários
└── Program.cs
```
