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
    public bool SemEmpresas { get; set; }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Inventario";
        ViewData["Title"] = "Novo Ativo";
        await CarregarEmpresasAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await CarregarEmpresasAsync();
        if (SemEmpresas)
        {
            ModelState.AddModelError(string.Empty, "Nenhuma empresa cadastrada. Cadastre uma empresa antes de registrar ativos.");
            return Page();
        }

        if (!await _db.Empresas.AnyAsync(e => e.Id == Ativo.EmpresaId))
            ModelState.AddModelError("Ativo.EmpresaId", "Selecione uma empresa válida.");

        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                    System.Console.WriteLine($"ModelState erro em '{entry.Key}': {error.ErrorMessage}");
            }
            return Page();
        }

        Ativo.CriadoEm = DateTime.Now;

        try
        {
            _db.AtivosIT.Add(Ativo);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível salvar o ativo. Verifique os dados e tente novamente.");
            return Page();
        }

        TempData["Sucesso"] = "Ativo cadastrado!";
        return RedirectToPage("Index");
    }

    private async Task CarregarEmpresasAsync()
    {
        var empresas = await _db.Empresas.OrderBy(e => e.Nome).ToListAsync();
        SemEmpresas = !empresas.Any();
        EmpresasList = new SelectList(
            empresas,
            "Id",
            "Nome",
            Ativo.EmpresaId);
    }
}
