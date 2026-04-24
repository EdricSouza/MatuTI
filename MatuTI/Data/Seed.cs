using Microsoft.AspNetCore.Identity;
using MatuTI.Models;

namespace MatuTI.Data;

public static class Seed
{
    public static async Task InicializarAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var db = services.GetRequiredService<AppDbContext>();

        string[] roles = { "Admin", "Avaliador" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        if (await userManager.FindByEmailAsync("admin@matuti.com") == null)
        {
            var admin = new ApplicationUser { UserName = "admin@matuti.com", Email = "admin@matuti.com", NomeCompleto = "Administrador", EmailConfirmed = true };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!db.Questoes.Any())
        {
            var questoes = new List<Questao>
            {
                // Maturidade dos Processos de TI
                new() { Indicador="Maturidade dos Processos de TI", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Mudanças", Texto="Os processos de TI estão documentados e formalizados?" },
                new() { Indicador="Maturidade dos Processos de TI", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Mudanças", Texto="Existe um processo definido para gestão de mudanças em TI?" },
                new() { Indicador="Maturidade dos Processos de TI", Peso=2, DominioCOBIT="MEA — Monitorar, Avaliar e Verificar", PraticaITIL="Melhoria Contínua", Texto="Os processos de TI são revisados e atualizados periodicamente?" },
                new() { Indicador="Maturidade dos Processos de TI", Peso=4, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="Os processos de TI estão alinhados com frameworks como ITIL ou COBIT?" },

                // Uso de Ferramentas de TI
                new() { Indicador="Uso de Ferramentas de TI", Peso=3, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Disponibilidade", Texto="A empresa utiliza ferramentas de monitoramento de infraestrutura?" },
                new() { Indicador="Uso de Ferramentas de TI", Peso=3, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Central de Serviços", Texto="Existe uma ferramenta de gestão de chamados (help desk/service desk)?" },
                new() { Indicador="Uso de Ferramentas de TI", Peso=2, DominioCOBIT="BAI — Construir, Adquirir e Implementar", PraticaITIL="Gerenciamento de Configuração", Texto="As ferramentas de TI estão integradas entre si?" },
                new() { Indicador="Uso de Ferramentas de TI", Peso=3, DominioCOBIT="BAI — Construir, Adquirir e Implementar", PraticaITIL="Gerenciamento de Ativos de TI", Texto="Existe inventário automatizado de ativos de TI?" },

                // Nível de Serviço
                new() { Indicador="Nível de Serviço", Peso=4, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Nível de Serviço", Texto="Existem SLAs (Acordos de Nível de Serviço) formalizados?" },
                new() { Indicador="Nível de Serviço", Peso=3, DominioCOBIT="MEA — Monitorar, Avaliar e Verificar", PraticaITIL="Gerenciamento de Nível de Serviço", Texto="Os SLAs são monitorados e reportados regularmente?" },
                new() { Indicador="Nível de Serviço", Peso=2, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Central de Serviços", Texto="Existe catálogo de serviços de TI disponível para os usuários?" },
                new() { Indicador="Nível de Serviço", Peso=2, DominioCOBIT="MEA — Monitorar, Avaliar e Verificar", PraticaITIL="Melhoria Contínua", Texto="A satisfação dos usuários com os serviços de TI é medida?" },

                // Alinhamento Estratégico
                new() { Indicador="Alinhamento Estratégico", Peso=5, DominioCOBIT="EDM — Avaliar, Dirigir e Monitorar", PraticaITIL="Melhoria Contínua", Texto="O planejamento de TI está alinhado ao planejamento estratégico da empresa?" },
                new() { Indicador="Alinhamento Estratégico", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="O orçamento de TI é definido com base nos objetivos do negócio?" },
                new() { Indicador="Alinhamento Estratégico", Peso=3, DominioCOBIT="EDM — Avaliar, Dirigir e Monitorar", PraticaITIL="Melhoria Contínua", Texto="A diretoria participa das decisões estratégicas de TI?" },
                new() { Indicador="Alinhamento Estratégico", Peso=2, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="Existe um roadmap tecnológico definido para os próximos anos?" },

                // Governança de TI
                new() { Indicador="Governança de TI", Peso=5, DominioCOBIT="EDM — Avaliar, Dirigir e Monitorar", PraticaITIL="Melhoria Contínua", Texto="Existe um comitê ou estrutura formal de governança de TI?" },
                new() { Indicador="Governança de TI", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Segurança", Texto="As políticas de TI estão documentadas e comunicadas?" },
                new() { Indicador="Governança de TI", Peso=3, DominioCOBIT="MEA — Monitorar, Avaliar e Verificar", PraticaITIL="Melhoria Contínua", Texto="Existem métricas e KPIs para avaliar o desempenho da TI?" },
                new() { Indicador="Governança de TI", Peso=3, DominioCOBIT="MEA — Monitorar, Avaliar e Verificar", PraticaITIL="Melhoria Contínua", Texto="São realizadas auditorias periódicas dos processos de TI?" },

                // Gestão de Riscos
                new() { Indicador="Gestão de Riscos", Peso=5, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Continuidade", Texto="Existe um processo formal de identificação e avaliação de riscos de TI?" },
                new() { Indicador="Gestão de Riscos", Peso=5, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Continuidade", Texto="Existe plano de continuidade de negócios (BCP) ou de recuperação de desastres (DRP)?" },
                new() { Indicador="Gestão de Riscos", Peso=3, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Disponibilidade", Texto="São realizados backups regulares e testados periodicamente?" },
                new() { Indicador="Gestão de Riscos", Peso=4, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Segurança", Texto="A segurança da informação segue uma política formal (ex: ISO 27001)?" },

                // Cultura de TI
                new() { Indicador="Cultura de TI", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="Os colaboradores de TI recebem treinamentos e capacitações regulares?" },
                new() { Indicador="Cultura de TI", Peso=2, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="Existe incentivo à inovação e melhoria contínua dentro da equipe de TI?" },
                new() { Indicador="Cultura de TI", Peso=2, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Melhoria Contínua", Texto="O conhecimento técnico é documentado e compartilhado entre a equipe?" },
                new() { Indicador="Cultura de TI", Peso=2, DominioCOBIT="EDM — Avaliar, Dirigir e Monitorar", PraticaITIL="Melhoria Contínua", Texto="A equipe de TI tem clareza sobre os objetivos e metas da área?" },

                // NC6 — Segurança e LGPD
                new() { Indicador="Segurança e LGPD", Peso=5, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Segurança", Texto="A empresa possui um encarregado de dados (DPO) nomeado conforme a LGPD?" },
                new() { Indicador="Segurança e LGPD", Peso=5, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Segurança", Texto="Existe mapeamento dos dados pessoais tratados pela empresa (Registro de Operações)?" },
                new() { Indicador="Segurança e LGPD", Peso=4, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Segurança", Texto="Os colaboradores recebem treinamento sobre LGPD e privacidade de dados?" },
                new() { Indicador="Segurança e LGPD", Peso=4, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Segurança", Texto="A empresa possui política de privacidade e termos de uso adequados à LGPD?" },
                new() { Indicador="Segurança e LGPD", Peso=3, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Segurança", Texto="Há controles de acesso lógico implementados (MFA, senhas fortes, princípio do menor privilégio)?" },

                // NC7 — Cloud e Ambiente Moderno
                new() { Indicador="Cloud e Ambiente Moderno", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Ativos de TI", Texto="A empresa possui política formal para uso de serviços em nuvem?" },
                new() { Indicador="Cloud e Ambiente Moderno", Peso=4, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Ativos de TI", Texto="Todos os serviços de Cloud utilizados são conhecidos e homologados pela TI (controle de Shadow IT)?" },
                new() { Indicador="Cloud e Ambiente Moderno", Peso=3, DominioCOBIT="APO — Alinhar, Planejar e Organizar", PraticaITIL="Gerenciamento de Disponibilidade", Texto="Existe governança e controle de custos para os ambientes de Cloud?" },
                new() { Indicador="Cloud e Ambiente Moderno", Peso=3, DominioCOBIT="DSS — Entregar, Servir e Suportar", PraticaITIL="Gerenciamento de Segurança", Texto="Os contratos com fornecedores de Cloud incluem cláusulas de segurança e conformidade com LGPD?" },
            };
            db.Questoes.AddRange(questoes);
            await db.SaveChangesAsync();
        }
    }
}
