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
    public DbSet<AtivoTI> AtivosIT { get; set; }         // NC5 — Inventário
    public DbSet<AvaliacaoLGPD> AvaliacoesLGPD { get; set; } // NC6 — LGPD

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

        builder.Entity<AtivoTI>()
            .HasOne(a => a.Empresa)
            .WithMany()
            .HasForeignKey(a => a.EmpresaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AvaliacaoLGPD>()
            .HasOne(a => a.Avaliacao)
            .WithMany()
            .HasForeignKey(a => a.AvaliacaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
