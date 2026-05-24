using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{
    /// <summary>
    /// Interface que define o contrato para operações de acesso a dados de Contatos
    /// Princípio: Dependency Inversion - dependa de abstrações
    /// </summary>
    public interface IContatoRepository
    {
        Task<IEnumerable<Contato>> ObterTodosAsync();
        Task<Contato> ObterPorIdAsync(int id);
        Task<Contato> CriarAsync(Contato contato);
        Task<Contato> AtualizarAsync(Contato contato);
        Task<bool> DeletarAsync(int id);
        Task<bool> SalvarAsync();
    }
}