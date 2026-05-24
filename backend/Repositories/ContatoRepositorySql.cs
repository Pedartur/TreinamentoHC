using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{

    public class ContatoRepositorySql : IContatoRepositorySql
    {
        private readonly string _connectionString;

        public ContatoRepositorySql(IConfiguration configuration)
        {
            // Obtém a connection string do appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtém todos os contatos do banco de dados
        /// Script SQL: SELECT simples
        /// </summary>
        public async Task<IEnumerable<Contato>> ObterTodosAsync()
        {
            var contatos = new List<Contato>();

            // Script SQL pré-definido
            string sqlScript = @"
                SELECT 
                    Id,
                    Nome,
                    Telefone,
                    Email,
                    Cargo
                FROM Contatos
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
                                var contato = new Contato
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Telefone = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Cargo = reader.GetString(4)
                                };

                                contatos.Add(contato);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao buscar contatos do banco de dados: {ex.Message}", ex);
            }

            return contatos;
        }

        /// <summary>
        /// Obtém um contato específico por ID
        /// Script SQL: SELECT com WHERE
        /// </summary>
        public async Task<Contato> ObterPorIdAsync(int id)
        {
            // Script SQL pré-definido com parâmetro
            string sqlScript = @"
                SELECT 
                    Id,
                    Nome,
                    Telefone,
                    Email,
                    Cargo
                FROM Contatos
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
                                return new Contato
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Telefone = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Cargo = reader.GetString(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao buscar contato com ID {id}: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Cria um novo contato no banco de dados
        /// Script SQL: INSERT com OUTPUT para retornar o ID gerado
        /// </summary>
        public async Task<Contato> CriarAsync(Contato contato)
        {
            // Script SQL pré-definido com OUTPUT para retornar ID gerado
            string sqlScript = @"
                INSERT INTO Contatos 
                    (Nome, Telefone, Email, Cargo)
                VALUES 
                    (@Nome, @Telefone, @Email, @Cargo);
                
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
                        command.Parameters.AddWithValue("@Nome", contato.Nome ?? "");
                        command.Parameters.AddWithValue("@Telefone", contato.Telefone);
                        command.Parameters.AddWithValue("@Email", contato.Email);
                        command.Parameters.AddWithValue("@Cargo", contato.Cargo);

                        // ExecuteScalar retorna o primeiro valor da primeira linha (ID)
                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int novoId))
                        {
                            contato.Id = novoId;
                            return contato;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao criar contato no banco de dados: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Atualiza um contato existente
        /// Script SQL: UPDATE com WHERE
        /// </summary>
        public async Task<Contato> AtualizarAsync(Contato contato)
        {
            // Script SQL pré-definido para atualização
            string sqlScript = @"
                UPDATE Contatos
                SET 
                    Nome = @Nome,
                    Telefone = @Telefone,
                    Email = @Email,
                    Cargo = @Cargo
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
                        command.Parameters.AddWithValue("@Id", contato.Id);
                        command.Parameters.AddWithValue("@Nome", contato.Nome ?? "");
                        command.Parameters.AddWithValue("@Telefone", contato.Telefone ?? "");
                        command.Parameters.AddWithValue("@Email", contato.Email);
                        command.Parameters.AddWithValue("@Cargo", contato.Cargo);

                        // ExecuteNonQuery retorna o número de linhas afetadas
                        int linhasAfetadas = await command.ExecuteNonQueryAsync();

                        if (linhasAfetadas > 0)
                        {
                            return contato;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Erro ao atualizar contato com ID {contato.Id}: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Deleta um contato do banco de dados
        /// Script SQL: DELETE com WHERE
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            // Script SQL pré-definido para deleção
            string sqlScript = @"
                DELETE FROM Contatos
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
                throw new Exception($"Erro ao deletar contato com ID {id}: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Obtém o total de contatos usando agregação
        /// Script SQL: SELECT com COUNT
        /// </summary>
        public async Task<int> ObterTotalContatosAsync()
        {
            // Script SQL pré-definido com agregação
            string sqlScript = @"
                SELECT COUNT(*) AS Total
                FROM Contatos";

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
                throw new Exception($"Erro ao obter total de contatos: {ex.Message}", ex);
            }

            return 0;
        }
    }
}