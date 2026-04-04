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

    public static List<string> Indicadores => new()
    {
        "Maturidade dos Processos de TI",
        "Uso de Ferramentas de TI",
        "Nível de Serviço",
        "Alinhamento Estratégico",
        "Governança de TI",
        "Gestão de Riscos",
        "Cultura de TI"
    };
}
