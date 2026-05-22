namespace TreinamentoAPI.Models.DTOs
{
    /// <summary>
    /// DTO para criação de um novo Produto (não expõe Id, DataCriacao é automática)
    /// </summary>
    public class CriarProdutoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }

    /// <summary>
    /// DTO para atualização de um Produto (parcial)
    /// </summary>
    public class AtualizarProdutoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
    }

    /// <summary>
    /// DTO para resposta ao listar ou obter um Produto (inclui Id)
    /// </summary>
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}