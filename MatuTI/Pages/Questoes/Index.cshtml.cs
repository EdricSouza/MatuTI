using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Questoes;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<IGrouping<string, Questao>> QuestoesPorIndicador { get; set; } = new();

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Questoes";
        ViewData["Title"] = "Banco de Questões";

        var questoes = await _db.Questoes.OrderBy(q => q.Indicador).ThenBy(q => q.Id).ToListAsync();
        QuestoesPorIndicador = questoes.GroupBy(q => q.Indicador).ToList();
    }

    public async Task<IActionResult> OnPostExcluirAsync(int id)
    {
        var questao = await _db.Questoes.FindAsync(id);
        if (questao != null)
        {
            questao.Ativo = !questao.Ativo;
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
