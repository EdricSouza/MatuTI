using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Empresas;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<Empresa> Empresas { get; set; } = new();

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Empresas";
        ViewData["Title"] = "Empresas";
        Empresas = await _db.Empresas
            .Include(e => e.Avaliacoes)
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostExcluirAsync(int id)
    {
        var empresa = await _db.Empresas.FindAsync(id);
        if (empresa != null)
        {
            _db.Empresas.Remove(empresa);
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
