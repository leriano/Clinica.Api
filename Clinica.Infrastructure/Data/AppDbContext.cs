using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Consulta> Consultas => Set<Consulta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração da tabela Pessoa
        modelBuilder.Entity<Pessoa>(e =>
        {
            e.ToTable("Pessoa");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(100);
            e.Property(x => x.Email).IsRequired().HasMaxLength(100);
            e.Property(x => x.Telefone).HasMaxLength(20);
            e.HasIndex(x => x.Email).IsUnique();
        });

        // Configuração da tabela Consulta
        modelBuilder.Entity<Consulta>(e =>
        {
            e.ToTable("Consulta");
            e.HasKey(x => x.Id);
            e.Property(x => x.DataConsulta).IsRequired();
            e.Property(x => x.Descricao).HasMaxLength(255);
            e.HasOne(x => x.Pessoa)
                .WithMany()
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
