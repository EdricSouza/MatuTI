using System.ComponentModel.DataAnnotations;

namespace MatuTI.Models;

public class Empresa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [Display(Name = "Nome da Empresa")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "CNPJ é obrigatório")]
    [Display(Name = "CNPJ")]
    public string CNPJ { get; set; } = string.Empty;

    [Display(Name = "Setor")]
    public string? Setor { get; set; }

    [Display(Name = "Responsável")]
    public string? Responsavel { get; set; }

    [Display(Name = "E-mail de contato")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string? Email { get; set; }

    [Display(Name = "Telefone")]
    public string? Telefone { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
}
