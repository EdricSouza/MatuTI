using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Inventario;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<AtivoTI> Ativos { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? EmpresaId { get; set; }

    public List<Empresa> Empresas { get; set; } = new();

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Inventario";
        ViewData["Title"] = "Inventário de Ativos de TI";
        Empresas = await _db.Empresas.OrderBy(e => e.Nome).ToListAsync();
        var query = _db.AtivosIT.Include(a => a.Empresa).AsQueryable();
        if (EmpresaId.HasValue)
            query = query.Where(a => a.EmpresaId == EmpresaId.Value);
        Ativos = await query.OrderBy(a => a.Empresa.Nome).ThenBy(a => a.Tipo).ThenBy(a => a.Nome).ToListAsync();
    }

    public async Task<IActionResult> OnPostExcluirAsync(int id)
    {
        var ativo = await _db.AtivosIT.FindAsync(id);
        if (ativo != null) { _db.AtivosIT.Remove(ativo); await _db.SaveChangesAsync(); }
        return RedirectToPage();
    }
}
