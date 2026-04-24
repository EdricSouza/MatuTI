using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;
using MatuTI.Services;

namespace MatuTI.Pages.Avaliacao;

[Authorize]
public class RelatorioModel : PageModel
{
    private readonly AppDbContext _db;
    public RelatorioModel(AppDbContext db) => _db = db;

    public Models.Avaliacao Avaliacao { get; set; } = null!;
    public double ScoreGeral { get; set; }
    public string NivelMaturidade { get; set; } = string.Empty;
    public string DescricaoNivel { get; set; } = string.Empty;
    public string CorNivel { get; set; } = string.Empty;
    public Dictionary<string, double> ScoresPorIndicador { get; set; } = new();
    public Dictionary<string, int> CapacidadePorIndicador { get; set; } = new(); // NC1
    public List<Resposta> Pontos { get; set; } = new();
    public List<Resposta> Lacunas { get; set; } = new();

    // NC9 — histórico de avaliações da mesma empresa
    public List<Models.Avaliacao> HistoricoAvaliacoes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Avaliacoes";
        ViewData["Title"] = "Relatório de Maturidade";

        var avaliacao = await _db.Avaliacoes
            .Include(a => a.Empresa)
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avaliacao == null) return NotFound();
        if (!avaliacao.Concluida) return RedirectToPage("Responder", new { id });

        Avaliacao = avaliacao;
        ScoreGeral = ScoreCalculator.Calcular(avaliacao.Respostas);
        NivelMaturidade = ScoreCalculator.NivelMaturidade(ScoreGeral);
        DescricaoNivel = ScoreCalculator.DescricaoNivel(NivelMaturidade);
        CorNivel = ScoreCalculator.CorNivel(NivelMaturidade);

        foreach (var indicador in ScoreCalculator.Indicadores)
        {
            var score = ScoreCalculator.CalcularPorIndicador(avaliacao.Respostas, indicador);
            ScoresPorIndicador[indicador] = score;
            CapacidadePorIndicador[indicador] = ScoreCalculator.CapacidadeCOBIT(score); // NC1
        }

        Pontos = avaliacao.Respostas.Where(r => r.Sim).OrderBy(r => r.Questao.Indicador).ToList();
        Lacunas = avaliacao.Respostas.Where(r => !r.Sim).OrderBy(r => r.Questao.Indicador).ToList();

        // NC9 — Buscar histórico de avaliações concluídas da mesma empresa
        HistoricoAvaliacoes = await _db.Avaliacoes
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .Where(a => a.EmpresaId == avaliacao.EmpresaId && a.Concluida && a.Id != id)
            .OrderBy(a => a.Data)
            .ToListAsync();

        return Page();
    }
}
