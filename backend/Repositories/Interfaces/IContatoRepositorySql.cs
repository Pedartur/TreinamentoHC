using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{
    /// <summary>
    /// Repositório de Contatos usando SQL puro (sem Entity Framework)
    /// Demonstra como executar scripts SQL diretamente no banco de dados
    /// </summary>
    public interface IContatoRepositorySql
    {
        Task<IEnumerable<Contato>> ObterTodosAsync();
        Task<Contato> ObterPorIdAsync(int id);
        Task<Contato> CriarAsync(Contato contato);
        Task<Contato> AtualizarAsync(Contato contato);
        Task<bool> DeletarAsync(int id);
        Task<int> ObterTotalContatosAsync();
    }
}
