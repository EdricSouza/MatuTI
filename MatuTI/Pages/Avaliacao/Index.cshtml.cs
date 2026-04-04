using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Avaliacao;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<Models.Avaliacao> Avaliacoes { get; set; } = new();

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Avaliacoes";
        ViewData["Title"] = "Avaliações";

        Avaliacoes = await _db.Avaliacoes
            .Include(a => a.Empresa)
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .OrderByDescending(a => a.Data)
            .ToListAsync();
    }
}
