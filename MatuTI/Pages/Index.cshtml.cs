using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public int TotalEmpresas { get; set; }
    public int TotalAvaliacoes { get; set; }
    public int TotalQuestoes { get; set; }
    public int TotalAtivos { get; set; }
    public List<MatuTI.Models.Avaliacao> UltimasAvaliacoes { get; set; } = new();
    
    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Dashboard";
        ViewData["Title"] = "Dashboard";

        TotalEmpresas  = await _db.Empresas.CountAsync();
        TotalAvaliacoes = await _db.Avaliacoes.CountAsync(a => a.Concluida);
        TotalQuestoes  = await _db.Questoes.CountAsync(q => q.Ativo);
        TotalAtivos    = await _db.AtivosIT.CountAsync();

        UltimasAvaliacoes = await _db.Avaliacoes
            .Include(a => a.Empresa)
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .Where(a => a.Concluida)
            .OrderByDescending(a => a.Data)
            .Take(5)
            .ToListAsync();
    }
}
