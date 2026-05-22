using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{

    public class ProdutoRepositorySql : IProdutoRepositorySql
    {
        private readonly string _connectionString;

        public ProdutoRepositorySql(IConfiguration configuration)
        {
            // Obtém a connection string do appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtém todos os produtos do banco de dados
        /// Script SQL: SELECT simples
        /// </summary>
        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            var produtos = new List<Produto>();

            // Script SQL pré-definido
            string sqlScript = @"
                SELECT 
                    Id,
                    Nome,
                    Descricao,
                    Preco,
                    Estoque,
                    DataCriacao
                FROM Produtos
                ORDER BY Nome ASC";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        // Define o tipo de comando como Text (script SQL)
                        command.CommandType = CommandType.Text;
                        // Define timeout de 30 segundos
                        command.CommandTimeout = 30;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var produto = new Produto
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Descricao = reader.GetString(2),
                                    Preco = reader.GetDecimal(3),
                                    Estoque = reader.GetInt32(4),
                                    DataCriacao = reader.GetDateTime(5)
                                };

                                produtos.Add(produto);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao buscar produtos do banco de dados: {ex.Message}", ex);
            }

            return produtos;
        }

        /// <summary>
        /// Obtém um produto específico por ID
        /// Script SQL: SELECT com WHERE
        /// </summary>
        public async Task<Produto> ObterPorIdAsync(int id)
        {
            // Script SQL pré-definido com parâmetro
            string sqlScript = @"
                SELECT 
                    Id,
                    Nome,
                    Descricao,
                    Preco,
                    Estoque,
                    DataCriacao
                FROM Produtos
                WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        // Adiciona parâmetro para prevenir SQL Injection
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Produto
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Descricao = reader.GetString(2),
                                    Preco = reader.GetDecimal(3),
                                    Estoque = reader.GetInt32(4),
                                    DataCriacao = reader.GetDateTime(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao buscar produto com ID {id}: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Cria um novo produto no banco de dados
        /// Script SQL: INSERT com OUTPUT para retornar o ID gerado
        /// </summary>
        public async Task<Produto> CriarAsync(Produto produto)
        {
            // Script SQL pré-definido com OUTPUT para retornar ID gerado
            string sqlScript = @"
                INSERT INTO Produtos 
                    (Nome, Descricao, Preco, Estoque, DataCriacao)
                VALUES 
                    (@Nome, @Descricao, @Preco, @Estoque, @DataCriacao);
                
                -- Retorna o ID gerado
                SELECT SCOPE_IDENTITY() AS Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        // Adiciona parâmetros com segurança
                        command.Parameters.AddWithValue("@Nome", produto.Nome ?? "");
                        command.Parameters.AddWithValue("@Descricao", produto.Descricao ?? "");
                        command.Parameters.AddWithValue("@Preco", produto.Preco);
                        command.Parameters.AddWithValue("@Estoque", produto.Estoque);
                        command.Parameters.AddWithValue("@DataCriacao", DateTime.Now);

                        // ExecuteScalar retorna o primeiro valor da primeira linha (ID)
                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int novoId))
                        {
                            produto.Id = novoId;
                            produto.DataCriacao = DateTime.Now;
                            return produto;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao criar produto no banco de dados: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Atualiza um produto existente
        /// Script SQL: UPDATE com WHERE
        /// </summary>
        public async Task<Produto> AtualizarAsync(Produto produto)
        {
            // Script SQL pré-definido para atualização
            string sqlScript = @"
                UPDATE Produtos
                SET 
                    Nome = @Nome,
                    Descricao = @Descricao,
                    Preco = @Preco,
                    Estoque = @Estoque
                WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        // Adiciona parâmetros
                        command.Parameters.AddWithValue("@Id", produto.Id);
                        command.Parameters.AddWithValue("@Nome", produto.Nome ?? "");
                        command.Parameters.AddWithValue("@Descricao", produto.Descricao ?? "");
                        command.Parameters.AddWithValue("@Preco", produto.Preco);
                        command.Parameters.AddWithValue("@Estoque", produto.Estoque);

                        // ExecuteNonQuery retorna o número de linhas afetadas
                        int linhasAfetadas = await command.ExecuteNonQueryAsync();

                        if (linhasAfetadas > 0)
                        {
                            return produto;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao atualizar produto com ID {produto.Id}: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Deleta um produto do banco de dados
        /// Script SQL: DELETE com WHERE
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            // Script SQL pré-definido para deleção
            string sqlScript = @"
                DELETE FROM Produtos
                WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@Id", id);

                        int linhasAfetadas = await command.ExecuteNonQueryAsync();

                        return linhasAfetadas > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao deletar produto com ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca produtos por faixa de preço
        /// Script SQL: SELECT com WHERE complexo
        /// </summary>
        public async Task<IEnumerable<Produto>> ObterPorPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            var produtos = new List<Produto>();

            // Script SQL pré-definido com múltiplos parâmetros
            string sqlScript = @"
                SELECT 
                    Id,
                    Nome,
                    Descricao,
                    Preco,
                    Estoque,
                    DataCriacao
                FROM Produtos
                WHERE Preco BETWEEN @PrecoMinimo AND @PrecoMaximo
                ORDER BY Preco ASC";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@PrecoMinimo", precoMinimo);
                        command.Parameters.AddWithValue("@PrecoMaximo", precoMaximo);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var produto = new Produto
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Descricao = reader.GetString(2),
                                    Preco = reader.GetDecimal(3),
                                    Estoque = reader.GetInt32(4),
                                    DataCriacao = reader.GetDateTime(5)
                                };

                                produtos.Add(produto);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao buscar produtos por preço: {ex.Message}", ex);
            }

            return produtos;
        }

        /// <summary>
        /// Obtém o total de produtos usando agregação
        /// Script SQL: SELECT com COUNT
        /// </summary>
        public async Task<int> ObterTotalProdutosAsync()
        {
            // Script SQL pré-definido com agregação
            string sqlScript = @"
                SELECT COUNT(*) AS Total
                FROM Produtos";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.CommandType = CommandType.Text;

                        // ExecuteScalar retorna um valor único
                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int total))
                        {
                            return total;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao obter total de produtos: {ex.Message}", ex);
            }

            return 0;
        }
    }
}