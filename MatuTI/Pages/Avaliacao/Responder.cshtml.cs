using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

namespace MatuTI.Pages.Avaliacao;

[Authorize]
public class ResponderModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ResponderModel(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public Models.Avaliacao Avaliacao { get; set; } = null!;
    public List<IGrouping<string, Resposta>> RespostasPorIndicador { get; set; } = new();

    [BindProperty]
    public List<RespostaInput> Inputs { get; set; } = new();

    public class RespostaInput
    {
        public int RespostaId { get; set; }
        public bool Sim { get; set; }
        public string? Evidencia { get; set; }
        public IFormFile? EvidenciaArquivo { get; set; }
        public string? Providencia { get; set; }
        public string? PlanoResponsavel { get; set; }
        public DateTime? PlanoPrazo { get; set; }
        public string PlanoStatus { get; set; } = "Pendente";
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ViewData["ActivePage"] = "Avaliacoes";
        ViewData["Title"] = "Responder Avaliação";

        var avaliacao = await _db.Avaliacoes
            .Include(a => a.Empresa)
            .Include(a => a.Respostas).ThenInclude(r => r.Questao)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avaliacao == null) return NotFound();
        if (avaliacao.Concluida) return RedirectToPage("Relatorio", new { id });

        Avaliacao = avaliacao;
        RespostasPorIndicador = avaliacao.Respostas
            .OrderBy(r => r.Questao.Indicador)
            .GroupBy(r => r.Questao.Indicador)
            .ToList();

        Inputs = avaliacao.Respostas.Select(r => new RespostaInput
        {
            RespostaId = r.Id,
            Sim = r.Sim,
            Evidencia = r.Evidencia,
            Providencia = r.Providencia,
            PlanoResponsavel = r.PlanoResponsavel,
            PlanoPrazo = r.PlanoPrazo,
            PlanoStatus = r.PlanoStatus
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostSalvarAsync(int id, bool concluir = false)
    {
        var avaliacao = await _db.Avaliacoes
            .Include(a => a.Respostas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (avaliacao == null) return NotFound();

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        foreach (var input in Inputs)
        {
            var resposta = avaliacao.Respostas.FirstOrDefault(r => r.Id == input.RespostaId);
            if (resposta == null) continue;

            resposta.Sim = input.Sim;
            resposta.Evidencia = input.Sim ? input.Evidencia : null;
            resposta.Providencia = !input.Sim ? input.Providencia : null;
            resposta.PlanoResponsavel = !input.Sim ? input.PlanoResponsavel : null;
            resposta.PlanoPrazo = !input.Sim ? input.PlanoPrazo : null;
            resposta.PlanoStatus = !input.Sim ? input.PlanoStatus : "Pendente";

            // NC4 — Upload de arquivo de evidência
            if (input.Sim && input.EvidenciaArquivo != null && input.EvidenciaArquivo.Length > 0)
            {
                var ext = Path.GetExtension(input.EvidenciaArquivo.FileName);
                var fileName = $"ev_{resposta.Id}_{DateTime.Now.Ticks}{ext}";
                var filePath = Path.Combine(uploadsDir, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await input.EvidenciaArquivo.CopyToAsync(stream);
                resposta.EvidenciaArquivo = fileName;
            }
        }

        if (concluir)
        {
            avaliacao.Concluida = true;
            await _db.SaveChangesAsync();
            return RedirectToPage("Relatorio", new { id });
        }

        await _db.SaveChangesAsync();
        TempData["Sucesso"] = "Respostas salvas com sucesso!";
        return RedirectToPage("Responder", new { id });
    }
}
