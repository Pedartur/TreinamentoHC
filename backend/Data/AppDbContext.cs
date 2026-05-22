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

        // DbSet que mapeia a entidade Produto para a tabela "Produtos"
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(500);

                entity.Property(e => e.Preco)
                    .HasPrecision(10, 2);

                entity.Property(e => e.Estoque)
                    .IsRequired();

                entity.Property(e => e.DataCriacao)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}