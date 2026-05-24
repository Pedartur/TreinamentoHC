using Microsoft.EntityFrameworkCore;
using TreinamentoAPI.Data;
using TreinamentoAPI.Models;

namespace TreinamentoAPI.Repositories
{
    /// <summary>
    /// Implementação do Repository Pattern para a entidade Contato
    /// Responsabilidade: Acesso a dados e persistência
    /// </summary>
    public class ContatoRepository : IContatoRepository
    {
        private readonly AppDbContext _context;

        public ContatoRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método GET - Safe e Idempotente
        /// Obtém todos os contatos do banco de dados
        /// </summary>
        public async Task<IEnumerable<Contato>> ObterTodosAsync()
        {
            return await _context.Contatos.ToListAsync();
        }

        /// <summary>
        /// Método GET - Safe e Idempotente
        /// Obtém um contato específico pelo Id
        /// </summary>
        public async Task<Contato> ObterPorIdAsync(int id)
        {
            return await _context.Contatos.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Método POST - Não idempotente
        /// Cria um novo contato no banco de dados
        /// </summary>
        public async Task<Contato> CriarAsync(Contato contato)
        {
            _context.Contatos.Add(contato);
            await SalvarAsync();
            return contato;
        }

        /// <summary>
        /// Método PUT - Idempotente
        /// Atualiza um contato existente
        /// </summary>
        public async Task<Contato> AtualizarAsync(Contato contato)
        {
            var contatoExistente = await _context.Contatos.FirstOrDefaultAsync(p => p.Id == contato.Id);

            if (contatoExistente == null)
                return null;

            contatoExistente.Nome = contato.Nome;
            contatoExistente.Telefone = contato.Telefone;
            contatoExistente.Email = contato.Email;
            contatoExistente.Cargo = contato.Cargo;

            _context.Contatos.Update(contatoExistente);
            await SalvarAsync();
            return contatoExistente;
        }

        /// <summary>
        /// Método DELETE - Idempotente
        /// Deleta um contato do banco de dados
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            var contato = await _context.Contatos.FirstOrDefaultAsync(p => p.Id == id);

            if (contato == null)
                return false;

            _context.Contatos.Remove(contato);
            await SalvarAsync();
            return true;
        }

        /// <summary>
        /// Salva todas as mudanças no contexto
        /// </summary>
        public async Task<bool> SalvarAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}