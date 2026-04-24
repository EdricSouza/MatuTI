using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Inventario;

[Authorize]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty] public AtivoTI Ativo { get; set; } = new();
    public SelectList EmpresasList { get; set; } = default!;

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Inventario";
        ViewData["Title"] = "Novo Ativo";
        EmpresasList = new SelectList(await _db.Empresas.OrderBy(e => e.Nome).ToListAsync(), "Id", "Nome");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            EmpresasList = new SelectList(await _db.Empresas.OrderBy(e => e.Nome).ToListAsync(), "Id", "Nome");
            return Page();
        }
        _db.AtivosIT.Add(Ativo);
        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Ativo cadastrado!";
        return RedirectToPage("Index");
    }
}
