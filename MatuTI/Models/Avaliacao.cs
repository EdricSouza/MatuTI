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

    [Display(Name = "Evidência")]
    public string? Evidencia { get; set; }

    [Display(Name = "Providência")]
    public string? Providencia { get; set; }
}
