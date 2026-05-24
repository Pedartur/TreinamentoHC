using Microsoft.EntityFrameworkCore;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Data
{
    /// <summary>
    /// DbContext que representa o banco de dados e mapeia as entidades
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet que mapeia a entidade Contato para a tabela "Contatos"
        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Contato
            modelBuilder.Entity<Contato>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Telefone)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cargo)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}