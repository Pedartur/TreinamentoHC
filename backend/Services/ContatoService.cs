using AutoMapper;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Models;
using TreinamentoAPI.Repositories;

namespace TreinamentoAPI.Services
{
    /// <summary>
    /// Implementação do Service Pattern para Contatos
    /// Responsabilidade: Encapsular regras de negócio
    /// Princípio: Single Responsibility - apenas lógica de negócio
    /// </summary>
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContatoService> _logger;

        public ContatoService(IContatoRepository contatoRepository, IMapper mapper, ILogger<ContatoService> logger)
        {
            _contatoRepository = contatoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os contatos e mapeia para DTOs
        /// </summary>
        public async Task<IEnumerable<ContatoDto>> ObterTodosAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todos os contatos");
                var contatos = await _contatoRepository.ObterTodosAsync();
                return _mapper.Map<IEnumerable<ContatoDto>>(contatos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter contatos: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtém um contato específico pelo Id
        /// </summary>
        public async Task<ContatoDto> ObterPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obtendo contato com Id: {id}");
                var contato = await _contatoRepository.ObterPorIdAsync(id);

                if (contato == null)
                {
                    _logger.LogWarning($"Contato com Id {id} não encontrado");
                    return null;
                }

                return _mapper.Map<ContatoDto>(contato);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter contato {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cria um novo contato com validações de negócio
        /// </summary>
        public async Task<ContatoDto> CriarAsync(CriarContatoDto criarContatoDto)
        {
            try
            {
                _logger.LogInformation($"Criando novo contato: {criarContatoDto.Nome}");

                var contato = _mapper.Map<Contato>(criarContatoDto);
                var contatoCriado = await _contatoRepository.CriarAsync(contato);

                _logger.LogInformation($"Contato criado com sucesso. Id: {contatoCriado.Id}");
                return _mapper.Map<ContatoDto>(contatoCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar contato: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Atualiza um contato existente
        /// Método PUT - Idempotente
        /// </summary>
        public async Task<ContatoDto> AtualizarAsync(int id, AtualizarContatoDto atualizarContatoDto)
        {
            try
            {
                _logger.LogInformation($"Atualizando contato com Id: {id}");

                var contatoExistente = await _contatoRepository.ObterPorIdAsync(id);

                if (contatoExistente == null)
                {
                    _logger.LogWarning($"Contato com Id {id} não encontrado para atualização");
                    return null;
                }

                _mapper.Map(atualizarContatoDto, contatoExistente);
                var contatoAtualizado = await _contatoRepository.AtualizarAsync(contatoExistente);

                _logger.LogInformation($"Contato {id} atualizado com sucesso");
                return _mapper.Map<ContatoDto>(contatoAtualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar contato {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deleta um contato
        /// Método DELETE - Idempotente
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deletando contato com Id: {id}");
                var resultado = await _contatoRepository.DeletarAsync(id);

                if (!resultado)
                {
                    _logger.LogWarning($"Contato com Id {id} não encontrado para deleção");
                    return false;
                }

                _logger.LogInformation($"Contato {id} deletado com sucesso");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar contato {id}: {ex.Message}");
                throw;
            }
        }
    }
}