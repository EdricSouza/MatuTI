using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MatuTI.Models;

namespace MatuTI.Pages.Admin;

[Authorize(Roles = "Admin")]
public class UsuariosModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuariosModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<UsuarioViewModel> Usuarios { get; set; } = new();

    public class UsuarioViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        ViewData["ActivePage"] = "Usuarios";
        ViewData["Title"] = "Usuários";

        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Usuarios.Add(new UsuarioViewModel
            {
                Id = user.Id,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email ?? "",
                Perfil = roles.FirstOrDefault() ?? "Sem perfil"
            });
        }
    }

    public async Task<IActionResult> OnPostAlterarPerfilAsync(string userId, string perfil)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var rolesAtuais = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, rolesAtuais);
        await _userManager.AddToRoleAsync(user, perfil);

        TempData["Sucesso"] = $"Perfil de {user.NomeCompleto} atualizado para {perfil}.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostExcluirAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
            await _userManager.DeleteAsync(user);

        TempData["Sucesso"] = "Usuário removido.";
        return RedirectToPage();
    }
}
