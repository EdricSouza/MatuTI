using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MatuTI.Data;
using MatuTI.Models;
using MatuTI.Services;

namespace MatuTI.Pages.Questoes;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Questao Questao { get; set; } = new();

    public SelectList IndicadoresList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Questoes";
        ViewData["Title"] = "Editar Questão";

        var questao = await _db.Questoes.FindAsync(id);
        if (questao == null) return NotFound();
        Questao = questao;
        IndicadoresList = new SelectList(ScoreCalculator.Indicadores);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            IndicadoresList = new SelectList(ScoreCalculator.Indicadores);
            return Page();
        }

        _db.Questoes.Update(Questao);
        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Questão atualizada!";
        return RedirectToPage("Index");
    }
}
