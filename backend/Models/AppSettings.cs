namespace TreinamentoAPI.Models
{
    /// <summary>
    /// Classe de configurações centralizada para a aplicação
    /// </summary>
    public class AppSettings
    {
        public string NomeAplicacao { get; set; }
        public string Versao { get; set; }
        public string Ambiente { get; set; }
    }
}