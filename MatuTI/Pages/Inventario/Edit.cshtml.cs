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
        await CarregarEmpresasAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await _db.Empresas.AnyAsync(e => e.Id == Ativo.EmpresaId))
            ModelState.AddModelError("Ativo.EmpresaId", "Selecione uma empresa válida.");

        if (!ModelState.IsValid)
        {
            await CarregarEmpresasAsync();
            return Page();
        }

        var ativoExistente = await _db.AtivosIT.FindAsync(Ativo.Id);
        if (ativoExistente == null) return NotFound();

        ativoExistente.EmpresaId = Ativo.EmpresaId;
        ativoExistente.Nome = Ativo.Nome;
        ativoExistente.Tipo = Ativo.Tipo;
        ativoExistente.Fabricante = Ativo.Fabricante;
        ativoExistente.Versao = Ativo.Versao;
        ativoExistente.Localizacao = Ativo.Localizacao;
        ativoExistente.Responsavel = Ativo.Responsavel;
        ativoExistente.DataAquisicao = Ativo.DataAquisicao;
        ativoExistente.FimSuporte = Ativo.FimSuporte;
        ativoExistente.Critico = Ativo.Critico;
        ativoExistente.Observacoes = Ativo.Observacoes;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível atualizar o ativo. Verifique os dados e tente novamente.");
            await CarregarEmpresasAsync();
            return Page();
        }

        TempData["Sucesso"] = "Ativo atualizado!";
        return RedirectToPage("Index");
    }

    private async Task CarregarEmpresasAsync()
    {
        EmpresasList = new SelectList(
            await _db.Empresas.OrderBy(e => e.Nome).ToListAsync(),
            "Id",
            "Nome",
            Ativo.EmpresaId);
    }
}
