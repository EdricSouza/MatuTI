using System.ComponentModel.DataAnnotations;

namespace MatuTI.Models;

public class Questao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Texto é obrigatório")]
    [Display(Name = "Pergunta")]
    public string Texto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indicador é obrigatório")]
    [Display(Name = "Indicador")]
    public string Indicador { get; set; } = string.Empty;

    // NC2 — Mapeamento a frameworks
    [Display(Name = "Domínio COBIT")]
    public string? DominioCOBIT { get; set; }

    [Display(Name = "Prática ITIL 4")]
    public string? PraticaITIL { get; set; }

    // NC1 — Peso ampliado 1-5 para escala mais granular
    [Display(Name = "Peso")]
    [Range(1, 5, ErrorMessage = "Peso deve ser entre 1 e 5")]
    public int Peso { get; set; } = 1;

    [Display(Name = "Ativo")]
    public bool Ativo { get; set; } = true;

    public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
}
