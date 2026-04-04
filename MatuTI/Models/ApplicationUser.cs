using Microsoft.AspNetCore.Identity;

namespace MatuTI.Models;

public class ApplicationUser : IdentityUser
{
    public string NomeCompleto { get; set; } = string.Empty;
}
