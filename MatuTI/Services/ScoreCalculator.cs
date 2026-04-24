using MatuTI.Models;

namespace MatuTI.Services;

public static class ScoreCalculator
{
    public static double Calcular(IEnumerable<Resposta> respostas)
    {
        var lista = respostas.ToList();
        if (!lista.Any()) return 0;
        var total = lista.Sum(r => r.Questao?.Peso ?? 1);
        var obtido = lista.Where(r => r.Sim).Sum(r => r.Questao?.Peso ?? 1);
        return total == 0 ? 0 : (double)obtido / total * 100;
    }

    public static double CalcularPorIndicador(IEnumerable<Resposta> respostas, string indicador)
    {
        var filtradas = respostas.Where(r => r.Questao?.Indicador == indicador).ToList();
        return Calcular(filtradas);
    }

    // NC1 — Escala 0-5 por domínio COBIT (CMMI-based)
    public static int CapacidadeCOBIT(double scorePercentual) => scorePercentual switch
    {
        >= 95 => 5,
        >= 80 => 4,
        >= 65 => 3,
        >= 45 => 2,
        >= 20 => 1,
        _ => 0
    };

    public static string DescricaoCapacidade(int nivel) => nivel switch
    {
        5 => "Otimizado — Processo em melhoria contínua e inovação permanente",
        4 => "Previsível — Processo medido e controlado dentro de limites definidos",
        3 => "Estabelecido — Processo padronizado e documentado na organização",
        2 => "Gerenciado — Processo planejado, monitorado e ajustado",
        1 => "Realizado — Processo atinge seus objetivos, ainda que informalmente",
        _ => "Incompleto — Processo não implementado ou não atinge seus objetivos"
    };

    public static string NivelMaturidade(double score) => score switch
    {
        >= 90 => "Estratégico",
        >= 80 => "Eficaz",
        >= 60 => "Eficiente",
        _ => "Artesanal"
    };

    public static string DescricaoNivel(string nivel) => nivel switch
    {
        "Estratégico" => "A TI é vista como parceiro estratégico do negócio. Processos altamente automatizados e integrados, infraestrutura resiliente e escalável, uso inovador de tecnologias e forte cultura de colaboração.",
        "Eficaz" => "Processos de TI otimizados e alinhados aos objetivos estratégicos. Forte governança de TI, gestão de riscos consolidada, monitoramento contínuo e cultura de melhoria contínua.",
        "Eficiente" => "Implementação de processos básicos de ITIL, maior foco na prevenção de problemas e padronização. Início da integração entre sistemas e crescente consciência sobre segurança da informação.",
        "Artesanal" => "TI reativa com foco em apagar incêndios. Falta de planejamento estratégico, processos manuais e ineficientes, alta dependência de indivíduos e baixo nível de automação.",
        _ => string.Empty
    };

    public static string CorNivel(string nivel) => nivel switch
    {
        "Estratégico" => "success",
        "Eficaz" => "info",
        "Eficiente" => "warning",
        "Artesanal" => "danger",
        _ => "secondary"
    };

    // NC8 — Sugestões automáticas de plano de ação por indicador
    public static string SugestaoGAP(string indicador) => indicador switch
    {
        "Maturidade dos Processos de TI" =>
            "Documente os processos de TI utilizando um framework como ITIL 4 ou COBIT 2019. Realize workshops internos para mapear e padronizar os fluxos de trabalho existentes.",
        "Uso de Ferramentas de TI" =>
            "Avalie e implemente ferramentas de monitoramento (ex: Zabbix, Grafana) e uma plataforma de service desk (ex: GLPI, Freshservice). Priorize a integração entre sistemas.",
        "Nível de Serviço" =>
            "Formalize os SLAs (Acordos de Nível de Serviço) com as áreas de negócio. Crie um catálogo de serviços e implante métricas de disponibilidade e tempo de resposta.",
        "Alinhamento Estratégico" =>
            "Desenvolva um Plano Diretor de TI (PDTI) alinhado ao planejamento estratégico da organização. Envolva a diretoria nas decisões tecnológicas de maior impacto.",
        "Governança de TI" =>
            "Crie um Comitê de TI com representantes de todas as áreas. Defina políticas formais, KPIs de TI e realize reuniões periódicas de acompanhamento.",
        "Gestão de Riscos" =>
            "Implante uma matriz de riscos de TI e um processo formal de avaliação e tratamento. Elabore ou atualize o Plano de Continuidade de Negócios (PCN/BCP) e o Plano de Recuperação de Desastres (DRP).",
        "Cultura de TI" =>
            "Invista em treinamentos e certificações para a equipe de TI. Crie um programa de gestão do conhecimento e incentive a participação em comunidades e eventos da área.",
        "Segurança e LGPD" =>
            "Nomeie um DPO (Encarregado de Dados), realize o mapeamento dos dados pessoais tratados, implante política de privacidade e conduza treinamentos de conscientização sobre a LGPD.",
        "Cloud e Ambiente Moderno" =>
            "Mapeie os serviços de Cloud utilizados formalmente e identifique Shadow IT. Defina uma política de uso de nuvem, governança de custos e controles de segurança para ambientes cloud.",
        _ => "Identifique as lacunas específicas, defina responsáveis e estabeleça prazos para cada ação corretiva."
    };

    public static List<string> Indicadores => new()
    {
        "Maturidade dos Processos de TI",
        "Uso de Ferramentas de TI",
        "Nível de Serviço",
        "Alinhamento Estratégico",
        "Governança de TI",
        "Gestão de Riscos",
        "Cultura de TI",
        "Segurança e LGPD",       // NC6
        "Cloud e Ambiente Moderno" // NC7
    };

    public static List<string> DominiosCOBIT => new()
    {
        "EDM — Avaliar, Dirigir e Monitorar",
        "APO — Alinhar, Planejar e Organizar",
        "BAI — Construir, Adquirir e Implementar",
        "DSS — Entregar, Servir e Suportar",
        "MEA — Monitorar, Avaliar e Verificar"
    };

    public static List<string> PraticasITIL => new()
    {
        "Gerenciamento de Incidentes",
        "Gerenciamento de Problemas",
        "Gerenciamento de Mudanças",
        "Gerenciamento de Nível de Serviço",
        "Central de Serviços",
        "Gerenciamento de Ativos de TI",
        "Gerenciamento de Configuração",
        "Gerenciamento de Disponibilidade",
        "Gerenciamento de Capacidade",
        "Gerenciamento de Continuidade",
        "Gerenciamento de Segurança",
        "Melhoria Contínua"
    };
}
