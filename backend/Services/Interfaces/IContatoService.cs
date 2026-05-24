using AutoMapper;
using TreinamentoAPI.Models.DTOs;

namespace TreinamentoAPI.Services
{
    /// <summary>
    /// Interface que define o contrato para operações de negócio de Contatos
    /// Princípio: Interface Segregation - interfaces específicas ao invés de genéricas
    /// </summary>
    public interface IContatoService
    {
        Task<IEnumerable<ContatoDto>> ObterTodosAsync();
        Task<ContatoDto> ObterPorIdAsync(int id);
        Task<ContatoDto> CriarAsync(CriarContatoDto criarContatoDto);
        Task<ContatoDto> AtualizarAsync(int id, AtualizarContatoDto atualizarContatoDto);
        Task<bool> DeletarAsync(int id);
    }
}