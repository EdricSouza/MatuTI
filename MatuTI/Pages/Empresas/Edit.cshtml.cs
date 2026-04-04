using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Empresas;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Empresa Empresa { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Empresas";
        ViewData["Title"] = "Editar Empresa";

        var empresa = await _db.Empresas.FindAsync(id);
        if (empresa == null) return NotFound();
        Empresa = empresa;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _db.Empresas.Update(Empresa);
        await _db.SaveChangesAsync();

        TempData["Sucesso"] = "Empresa atualizada com sucesso!";
        return RedirectToPage("Index");
    }
}
