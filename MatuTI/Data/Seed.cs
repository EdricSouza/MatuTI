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

        // Criar roles
        string[] roles = { "Admin", "Avaliador" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Criar usuário admin padrão
        if (await userManager.FindByEmailAsync("admin@matuti.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@matuti.com",
                Email = "admin@matuti.com",
                NomeCompleto = "Administrador",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Criar questões padrão se não houver nenhuma
        if (!db.Questoes.Any())
        {
            var questoes = new List<Questao>
            {
                // Maturidade dos Processos de TI
                new() { Indicador = "Maturidade dos Processos de TI", Peso = 2, Texto = "Os processos de TI estão documentados e formalizados?" },
                new() { Indicador = "Maturidade dos Processos de TI", Peso = 2, Texto = "Existe um processo definido para gestão de mudanças em TI?" },
                new() { Indicador = "Maturidade dos Processos de TI", Peso = 1, Texto = "Os processos de TI são revisados e atualizados periodicamente?" },
                new() { Indicador = "Maturidade dos Processos de TI", Peso = 3, Texto = "Os processos de TI estão alinhados com frameworks como ITIL ou COBIT?" },

                // Uso de Ferramentas de TI
                new() { Indicador = "Uso de Ferramentas de TI", Peso = 2, Texto = "A empresa utiliza ferramentas de monitoramento de infraestrutura?" },
                new() { Indicador = "Uso de Ferramentas de TI", Peso = 2, Texto = "Existe uma ferramenta de gestão de chamados (help desk/service desk)?" },
                new() { Indicador = "Uso de Ferramentas de TI", Peso = 1, Texto = "As ferramentas de TI estão integradas entre si?" },
                new() { Indicador = "Uso de Ferramentas de TI", Peso = 2, Texto = "Existe inventário automatizado de ativos de TI?" },

                // Nível de Serviço
                new() { Indicador = "Nível de Serviço", Peso = 3, Texto = "Existem SLAs (Acordos de Nível de Serviço) formalizados?" },
                new() { Indicador = "Nível de Serviço", Peso = 2, Texto = "Os SLAs são monitorados e reportados regularmente?" },
                new() { Indicador = "Nível de Serviço", Peso = 2, Texto = "Existe catálogo de serviços de TI disponível para os usuários?" },
                new() { Indicador = "Nível de Serviço", Peso = 1, Texto = "A satisfação dos usuários com os serviços de TI é medida?" },

                // Alinhamento Estratégico
                new() { Indicador = "Alinhamento Estratégico", Peso = 3, Texto = "O planejamento de TI está alinhado ao planejamento estratégico da empresa?" },
                new() { Indicador = "Alinhamento Estratégico", Peso = 2, Texto = "O orçamento de TI é definido com base nos objetivos do negócio?" },
                new() { Indicador = "Alinhamento Estratégico", Peso = 2, Texto = "A diretoria participa das decisões estratégicas de TI?" },
                new() { Indicador = "Alinhamento Estratégico", Peso = 1, Texto = "Existe um roadmap tecnológico definido para os próximos anos?" },

                // Governança de TI
                new() { Indicador = "Governança de TI", Peso = 3, Texto = "Existe um comitê ou estrutura formal de governança de TI?" },
                new() { Indicador = "Governança de TI", Peso = 2, Texto = "As políticas de TI estão documentadas e comunicadas?" },
                new() { Indicador = "Governança de TI", Peso = 2, Texto = "Existem métricas e KPIs para avaliar o desempenho da TI?" },
                new() { Indicador = "Governança de TI", Peso = 2, Texto = "São realizadas auditorias periódicas dos processos de TI?" },

                // Gestão de Riscos
                new() { Indicador = "Gestão de Riscos", Peso = 3, Texto = "Existe um processo formal de identificação e avaliação de riscos de TI?" },
                new() { Indicador = "Gestão de Riscos", Peso = 3, Texto = "Existe plano de continuidade de negócios (BCP) ou de recuperação de desastres (DRP)?" },
                new() { Indicador = "Gestão de Riscos", Peso = 2, Texto = "São realizados backups regulares e testados periodicamente?" },
                new() { Indicador = "Gestão de Riscos", Peso = 2, Texto = "A segurança da informação segue uma política formal (ex: ISO 27001)?" },

                // Cultura de TI
                new() { Indicador = "Cultura de TI", Peso = 2, Texto = "Os colaboradores de TI recebem treinamentos e capacitações regulares?" },
                new() { Indicador = "Cultura de TI", Peso = 1, Texto = "Existe incentivo à inovação e melhoria contínua dentro da equipe de TI?" },
                new() { Indicador = "Cultura de TI", Peso = 2, Texto = "O conhecimento técnico é documentado e compartilhado entre a equipe?" },
                new() { Indicador = "Cultura de TI", Peso = 1, Texto = "A equipe de TI tem clareza sobre os objetivos e metas da área?" },
            };

            db.Questoes.AddRange(questoes);
            await db.SaveChangesAsync();
        }
    }
}
