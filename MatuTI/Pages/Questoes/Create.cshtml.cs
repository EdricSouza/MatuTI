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

    [BindProperty] public Questao Questao { get; set; } = new();
    public SelectList IndicadoresList { get; set; } = default!;
    public SelectList DominiosCOBITList { get; set; } = default!;
    public SelectList PraticasITILList { get; set; } = default!;

    public void OnGet()
    {
        ViewData["ActivePage"] = "Questoes";
        ViewData["Title"] = "Nova Questão";
        CarregarListas();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) { CarregarListas(); return Page(); }
        _db.Questoes.Add(Questao);
        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Questão cadastrada!";
        return RedirectToPage("Index");
    }

    private void CarregarListas()
    {
        IndicadoresList = new SelectList(ScoreCalculator.Indicadores);
        DominiosCOBITList = new SelectList(ScoreCalculator.DominiosCOBIT);
        PraticasITILList = new SelectList(ScoreCalculator.PraticasITIL);
    }
}
