namespace TreinamentoAPI.Models.DTOs
{
    /// <summary>
    /// DTO para criação de um novo Contato (não expõe Id)
    /// </summary>
    public class CriarContatoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de um Contato (parcial)
    /// </summary>
    public class AtualizarContatoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta ao listar ou obter um Contato (inclui Id)
    /// </summary>
    public class ContatoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }
}