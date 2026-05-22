using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{
    /// <summary>
    /// Interface que define o contrato para operações de acesso a dados de Produtos
    /// Princípio: Dependency Inversion - dependa de abstrações
    /// </summary>
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorIdAsync(int id);
        Task<Produto> CriarAsync(Produto produto);
        Task<Produto> AtualizarAsync(Produto produto);
        Task<bool> DeletarAsync(int id);
        Task<bool> SalvarAsync();
    }
}