using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MatuTI.Models;

public class AtivoTI
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Empresa é obrigatória")]
    [Display(Name = "Empresa")]
    public int EmpresaId { get; set; }

    [ValidateNever]
    public Empresa Empresa { get; set; } = null!;

    [Required(ErrorMessage = "Nome é obrigatório")]
    [Display(Name = "Nome / Identificação")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo é obrigatório")]
    [Display(Name = "Tipo")]
    public string Tipo { get; set; } = string.Empty; // Hardware, Software, Cloud

    [Display(Name = "Fabricante / Fornecedor")]
    public string? Fabricante { get; set; }

    [Display(Name = "Versão / Modelo")]
    public string? Versao { get; set; }

    [Display(Name = "Localização / Ambiente")]
    public string? Localizacao { get; set; } // On-premise, Cloud, Híbrido

    [Display(Name = "Responsável")]
    public string? Responsavel { get; set; }

    [Display(Name = "Data de aquisição")]
    public DateTime? DataAquisicao { get; set; }

    [Display(Name = "Fim do suporte")]
    public DateTime? FimSuporte { get; set; }

    [Display(Name = "Crítico para o negócio")]
    public bool Critico { get; set; } = false;

    [Display(Name = "Observações")]
    public string? Observacoes { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;
}
