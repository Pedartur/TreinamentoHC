using Microsoft.EntityFrameworkCore;
using TreinamentoAPI.Data;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{
    /// <summary>
    /// Implementação do Repository Pattern para a entidade Produto
    /// Responsabilidade: Acesso a dados e persistência
    /// </summary>
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método GET - Safe e Idempotente
        /// Obtém todos os produtos do banco de dados
        /// </summary>
        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        /// <summary>
        /// Método GET - Safe e Idempotente
        /// Obtém um produto específico pelo Id
        /// </summary>
        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Método POST - Não idempotente
        /// Cria um novo produto no banco de dados
        /// </summary>
        public async Task<Produto> CriarAsync(Produto produto)
        {
            produto.DataCriacao = DateTime.UtcNow;
            _context.Produtos.Add(produto);
            await SalvarAsync();
            return produto;
        }

        /// <summary>
        /// Método PUT - Idempotente
        /// Atualiza um produto existente
        /// </summary>
        public async Task<Produto> AtualizarAsync(Produto produto)
        {
            var produtoExistente = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);

            if (produtoExistente == null)
                return null;

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Preco = produto.Preco;
            produtoExistente.Estoque = produto.Estoque;

            _context.Produtos.Update(produtoExistente);
            await SalvarAsync();
            return produtoExistente;
        }

        /// <summary>
        /// Método DELETE - Idempotente
        /// Deleta um produto do banco de dados
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return false;

            _context.Produtos.Remove(produto);
            await SalvarAsync();
            return true;
        }

        /// <summary>
        /// Salva todas as mudanças no contexto
        /// </summary>
        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}