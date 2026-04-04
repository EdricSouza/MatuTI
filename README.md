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
