# MatuTI v2 — Avaliação de Maturidade de TI

Versão atualizada com o roadmap consolidado do grupo.

## Requisitos
- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- EF Core Tools: `dotnet tool install --global dotnet-ef`

## Como rodar (banco do zero)

```bash
cd MatuTI_v3

dotnet restore

# Se nunca rodou antes:
dotnet ef migrations add Inicial
dotnet ef database update

# Se já rodou a v1 e quer aplicar só as novidades:
dotnet ef database update RoadmapV2

dotnet run
```

Acesse: **https://localhost:5001**
Login: **admin@matuti.com / Admin@123**

## O que foi implementado (Roadmap do grupo)

| NC | Não Conformidade           | O que foi feito                                                      |
|----|----------------------------|----------------------------------------------------------------------|
| 1  | Modelo de maturidade pouco detalhado | Escala COBIT 0–5 por indicador no relatório              |
| 2  | Questões não vinculadas a frameworks | Campo DominioCOBIT e PraticaITIL em cada questão         |
| 3  | Melhoria contínua não estruturada    | Plano de ação com Responsável, Prazo e Status            |
| 4  | Diagnóstico sem upload de evidências | Upload de arquivo (PDF, imagem, Word) por resposta        |
| 5  | Ausência de gestão de ativos         | Módulo Inventário de Ativos (hardware, software, cloud)  |
| 6  | Falta avaliação LGPD                 | Indicador "Segurança e LGPD" com 5 questões específicas  |
| 7  | Ambiente moderno não avaliado        | Indicador "Cloud e Ambiente Moderno" com 4 questões      |
| 8  | Sistema não orienta, só diagnostica  | Sugestões automáticas de GAP por indicador               |
| 9  | Ausência de histórico evolutivo      | Gráfico de evolução de maturidade no relatório           |

## Estrutura do projeto

```
MatuTI_v3/
├── Data/
│   ├── AppDbContext.cs
│   └── Seed.cs
├── Migrations/
│   ├── *_Inicial.cs
│   └── *_RoadmapV2.cs      ← nova migration
├── Models/
│   ├── ApplicationUser.cs
│   ├── AtivoTI.cs          ← novo (NC5)
│   ├── Avaliacao.cs        ← atualizado (NC3, NC4, NC6)
│   ├── Empresa.cs
│   └── Questao.cs          ← atualizado (NC2)
├── Pages/
│   ├── Auth/
│   ├── Admin/
│   ├── Avaliacao/          ← Responder e Relatório atualizados
│   ├── Empresas/
│   ├── Inventario/         ← novo (NC5)
│   └── Questoes/           ← Create/Edit com COBIT/ITIL
├── Services/
│   └── ScoreCalculator.cs  ← atualizado (NC1, NC7, NC8)
└── Program.cs
```
