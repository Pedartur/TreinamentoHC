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
    /// Repositório de Produtos usando SQL puro (sem Entity Framework)
    /// Demonstra como executar scripts SQL diretamente no banco de dados
    /// </summary>
    public interface IProdutoRepositorySql
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorIdAsync(int id);
        Task<Produto> CriarAsync(Produto produto);
        Task<Produto> AtualizarAsync(Produto produto);
        Task<bool> DeletarAsync(int id);
        Task<IEnumerable<Produto>> ObterPorPrecoAsync(decimal precoMinimo, decimal precoMaximo);
        Task<int> ObterTotalProdutosAsync();
    }
}
