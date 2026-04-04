using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Avaliacao;

[Authorize]
public class ResponderModel : PageModel
{
    private readonly AppDbContext _db;
    public ResponderModel(AppDbContext db) => _db = db;

    public Models.Avaliacao Avaliacao { get; set; } = null!;
    public List<IGrouping<string, Resposta>> RespostasPorIndicador { get; set; } = new();

    [BindProperty]
    public List<RespostaInput> Inputs { get; set; } = new();

    public class RespostaInput
    {
        public int RespostaId { get; set; }
        public bool Sim { get; set; }
        public string? Evidencia { get; set; }
        public string? Providencia { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Avaliacoes";
        ViewData["Title"] = "Responder Avaliação";

        var avaliacao = await _db.Avaliacoes
            .Include(a => a.Empresa)
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avaliacao == null) return NotFound();
        if (avaliacao.Concluida) return RedirectToPage("Relatorio", new { id });

        Avaliacao = avaliacao;
        RespostasPorIndicador = avaliacao.Respostas
            .OrderBy(r => r.Questao.Indicador)
            .GroupBy(r => r.Questao.Indicador)
            .ToList();

        Inputs = avaliacao.Respostas.Select(r => new RespostaInput
        {
            RespostaId = r.Id,
            Sim = r.Sim,
            Evidencia = r.Evidencia,
            Providencia = r.Providencia
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostSalvarAsync(int id, bool concluir = false)
    {
        var avaliacao = await _db.Avaliacoes
            .Include(a => a.Respostas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avaliacao == null) return NotFound();

        foreach (var input in Inputs)
        {
            var resposta = avaliacao.Respostas.FirstOrDefault(r => r.Id == input.RespostaId);
            if (resposta == null) continue;

            resposta.Sim = input.Sim;
            resposta.Evidencia = input.Sim ? input.Evidencia : null;
            resposta.Providencia = !input.Sim ? input.Providencia : null;
        }

        if (concluir)
        {
            avaliacao.Concluida = true;
            await _db.SaveChangesAsync();
            return RedirectToPage("Relatorio", new { id });
        }

        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Respostas salvas com sucesso!";
        return RedirectToPage("Responder", new { id });
    }
}
