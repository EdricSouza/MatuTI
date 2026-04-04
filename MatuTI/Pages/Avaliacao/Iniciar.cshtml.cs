using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Avaliacao;

[Authorize]
public class IniciarModel : PageModel
{
    private readonly AppDbContext _db;
    public IniciarModel(AppDbContext db) => _db = db;

    public SelectList EmpresasList { get; set; } = default!;

    [BindProperty]
    public int EmpresaId { get; set; }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "NovaAvaliacao";
        ViewData["Title"] = "Iniciar Avaliação";
        await CarregarEmpresas();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var empresa = await _db.Empresas.FindAsync(EmpresaId);
        if (empresa == null)
        {
            ModelState.AddModelError(string.Empty, "Empresa não encontrada.");
            await CarregarEmpresas();
            return Page();
        }

        var questoes = await _db.Questoes.Where(q => q.Ativo).ToListAsync();
        if (!questoes.Any())
        {
            ModelState.AddModelError(string.Empty, "Não há questões cadastradas para iniciar a avaliação.");
            await CarregarEmpresas();
            return Page();
        }

        var avaliacao = new Models.Avaliacao
        {
            EmpresaId = EmpresaId,
            Data = DateTime.Now,
            AplicadaPorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "",
            AplicadaPorNome = User.Identity?.Name ?? ""
        };

        _db.Avaliacoes.Add(avaliacao);
        await _db.SaveChangesAsync();

        // Criar respostas em branco para cada questão
        var respostas = questoes.Select(q => new Resposta
        {
            AvaliacaoId = avaliacao.Id,
            QuestaoId = q.Id,
            Sim = false
        }).ToList();

        _db.Respostas.AddRange(respostas);
        await _db.SaveChangesAsync();

        return RedirectToPage("Responder", new { id = avaliacao.Id });
    }

    private async Task CarregarEmpresas()
    {
        var empresas = await _db.Empresas.OrderBy(e => e.Nome).ToListAsync();
        EmpresasList = new SelectList(empresas, "Id", "Nome");
    }
}
