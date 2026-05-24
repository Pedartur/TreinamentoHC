using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreinamentoAPI.Models
{
    /// <summary>
    /// Entidade que representa um Contato no banco de dados
    /// </summary>
    public class Contato
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }
}