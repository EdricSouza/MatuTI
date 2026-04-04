using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MatuTI.Models;

namespace MatuTI.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Questao> Questoes { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }
    public DbSet<Resposta> Respostas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Resposta>()
            .HasOne(r => r.Avaliacao)
            .WithMany(a => a.Respostas)
            .HasForeignKey(r => r.AvaliacaoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Resposta>()
            .HasOne(r => r.Questao)
            .WithMany(q => q.Respostas)
            .HasForeignKey(r => r.QuestaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
