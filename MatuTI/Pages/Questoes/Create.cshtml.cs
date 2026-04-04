using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MatuTI.Data;
using MatuTI.Models;
using MatuTI.Services;

namespace MatuTI.Pages.Questoes;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Questao Questao { get; set; } = new();

    public SelectList IndicadoresList { get; set; } = default!;

    public void OnGet()
    {
        ViewData["ActivePage"] = "Questoes";
        ViewData["Title"] = "Nova Questão";
        IndicadoresList = new SelectList(ScoreCalculator.Indicadores);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            IndicadoresList = new SelectList(ScoreCalculator.Indicadores);
            return Page();
        }

        _db.Questoes.Add(Questao);
        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Questão cadastrada com sucesso!";
        return RedirectToPage("Index");
    }
}
