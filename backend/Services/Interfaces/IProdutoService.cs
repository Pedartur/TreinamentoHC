using AutoMapper;
using TreinamentoAPI.Models.DTOs;

namespace TreinamentoAPI.Services
{
    /// <summary>
    /// Interface que define o contrato para operações de negócio de Produtos
    /// Princípio: Interface Segregation - interfaces específicas ao invés de genéricas
    /// </summary>
    public interface IProdutoService
    {
        Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
        Task<ProdutoDto> ObterPorIdAsync(int id);
        Task<ProdutoDto> CriarAsync(CriarProdutoDto criarProdutoDto);
        Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto atualizarProdutoDto);
        Task<bool> DeletarAsync(int id);
    }
}