using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatuTI.Services;

namespace MatuTI.Models;

public class Avaliacao
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; } = null!;

    [Display(Name = "Data")]
    public DateTime Data { get; set; } = DateTime.Now;

    [Display(Name = "Aplicada por")]
    public string AplicadaPorId { get; set; } = string.Empty;

    [Display(Name = "Aplicada por")]
    public string AplicadaPorNome { get; set; } = string.Empty;

    [Display(Name = "Concluída")]
    public bool Concluida { get; set; } = false;

    public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();

    [NotMapped]
    public double ScoreGeral => CalcularScore(Respostas);

    [NotMapped]
    public string NivelMaturidade => ScoreCalculator.NivelMaturidade(ScoreGeral);

    public static double CalcularScore(ICollection<Resposta> respostas)
    {
        if (!respostas.Any()) return 0;
        var total = respostas.Sum(r => r.Questao?.Peso ?? 1);
        var obtido = respostas.Where(r => r.Sim).Sum(r => r.Questao?.Peso ?? 1);
        return total == 0 ? 0 : (double)obtido / total * 100;
    }
}

public class Resposta
{
    public int Id { get; set; }

    public int AvaliacaoId { get; set; }
    public Avaliacao Avaliacao { get; set; } = null!;

    public int QuestaoId { get; set; }
    public Questao Questao { get; set; } = null!;

    [Display(Name = "Resposta")]
    public bool Sim { get; set; }

    [Display(Name = "Evidência (texto)")]
    public string? Evidencia { get; set; }

    // NC4 — upload de arquivo de evidência
    [Display(Name = "Arquivo de evidência")]
    public string? EvidenciaArquivo { get; set; }

    // NC3 — plano de ação estruturado
    [Display(Name = "Providência / Plano de ação")]
    public string? Providencia { get; set; }

    [Display(Name = "Responsável")]
    public string? PlanoResponsavel { get; set; }

    [Display(Name = "Prazo")]
    public DateTime? PlanoPrazo { get; set; }

    [Display(Name = "Status")]
    public string PlanoStatus { get; set; } = "Pendente";
}

// NC6 — Indicador LGPD
public class AvaliacaoLGPD
{
    public int Id { get; set; }
    public int AvaliacaoId { get; set; }
    public Avaliacao Avaliacao { get; set; } = null!;

    [Display(Name = "Questão")]
    public string Questao { get; set; } = string.Empty;

    [Display(Name = "Resposta")]
    public bool Conforme { get; set; }

    [Display(Name = "Observação")]
    public string? Observacao { get; set; }
}
