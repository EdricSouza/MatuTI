using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Inventario;

[Authorize]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty] public AtivoTI Ativo { get; set; } = new();
    public SelectList EmpresasList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Inventario";
        ViewData["Title"] = "Editar Ativo";
        var ativo = await _db.AtivosIT.FindAsync(id);
        if (ativo == null) return NotFound();
        Ativo = ativo;
        EmpresasList = new SelectList(await _db.Empresas.OrderBy(e => e.Nome).ToListAsync(), "Id", "Nome");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            EmpresasList = new SelectList(await _db.Empresas.OrderBy(e => e.Nome).ToListAsync(), "Id", "Nome");
            return Page();
        }
        _db.AtivosIT.Update(Ativo);
        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Ativo atualizado!";
        return RedirectToPage("Index");
    }
}
