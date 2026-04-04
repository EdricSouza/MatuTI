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

    [Display(Name = "Peso")]
    [Range(1, 3, ErrorMessage = "Peso deve ser entre 1 e 3")]
    public int Peso { get; set; } = 1;

    [Display(Name = "Ativo")]
    public bool Ativo { get; set; } = true;

    public ICollection<Resposta> Respostas { get; set; } = new List<Resposta>();
}
