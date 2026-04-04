using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Empresas;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Empresa Empresa { get; set; } = new();

    public void OnGet()
    {
        ViewData["ActivePage"] = "Empresas";
        ViewData["Title"] = "Nova Empresa";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _db.Empresas.Add(Empresa);
        await _db.SaveChangesAsync();

        TempData["Sucesso"] = $"Empresa '{Empresa.Nome}' cadastrada com sucesso!";
        return RedirectToPage("Index");
    }
}
