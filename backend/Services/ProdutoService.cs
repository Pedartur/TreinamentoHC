using AutoMapper;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Models;
using TreinamentoAPI.Repositories;

namespace TreinamentoAPI.Services
{
    /// <summary>
    /// Implementação do Service Pattern para Produtos
    /// Responsabilidade: Encapsular regras de negócio
    /// Princípio: Single Responsibility - apenas lógica de negócio
    /// </summary>
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper, ILogger<ProdutoService> logger)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os produtos e mapeia para DTOs
        /// </summary>
        public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todos os produtos");
                var produtos = await _produtoRepository.ObterTodosAsync();
                return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter produtos: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtém um produto específico pelo Id
        /// </summary>
        public async Task<ProdutoDto> ObterPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obtendo produto com Id: {id}");
                var produto = await _produtoRepository.ObterPorIdAsync(id);

                if (produto == null)
                {
                    _logger.LogWarning($"Produto com Id {id} não encontrado");
                    return null;
                }

                return _mapper.Map<ProdutoDto>(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter produto {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cria um novo produto com validações de negócio
        /// </summary>
        public async Task<ProdutoDto> CriarAsync(CriarProdutoDto criarProdutoDto)
        {
            try
            {
                // Validações de negócio
                if (criarProdutoDto.Preco <= 0)
                    throw new ArgumentException("Preço deve ser maior que zero");

                if (criarProdutoDto.Estoque < 0)
                    throw new ArgumentException("Estoque não pode ser negativo");

                _logger.LogInformation($"Criando novo produto: {criarProdutoDto.Nome}");

                var produto = _mapper.Map<Produto>(criarProdutoDto);
                var produtoCriado = await _produtoRepository.CriarAsync(produto);

                _logger.LogInformation($"Produto criado com sucesso. Id: {produtoCriado.Id}");
                return _mapper.Map<ProdutoDto>(produtoCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar produto: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Atualiza um produto existente
        /// Método PUT - Idempotente
        /// </summary>
        public async Task<ProdutoDto> AtualizarAsync(int id, AtualizarProdutoDto atualizarProdutoDto)
        {
            try
            {
                // Validações de negócio
                if (atualizarProdutoDto.Preco <= 0)
                    throw new ArgumentException("Preço deve ser maior que zero");

                if (atualizarProdutoDto.Estoque < 0)
                    throw new ArgumentException("Estoque não pode ser negativo");

                _logger.LogInformation($"Atualizando produto com Id: {id}");

                var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);

                if (produtoExistente == null)
                {
                    _logger.LogWarning($"Produto com Id {id} não encontrado para atualização");
                    return null;
                }

                _mapper.Map(atualizarProdutoDto, produtoExistente);
                var produtoAtualizado = await _produtoRepository.AtualizarAsync(produtoExistente);

                _logger.LogInformation($"Produto {id} atualizado com sucesso");
                return _mapper.Map<ProdutoDto>(produtoAtualizado);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar produto {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deleta um produto
        /// Método DELETE - Idempotente
        /// </summary>
        public async Task<bool> DeletarAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deletando produto com Id: {id}");
                var resultado = await _produtoRepository.DeletarAsync(id);

                if (!resultado)
                {
                    _logger.LogWarning($"Produto com Id {id} não encontrado para deleção");
                    return false;
                }

                _logger.LogInformation($"Produto {id} deletado com sucesso");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar produto {id}: {ex.Message}");
                throw;
            }
        }
    }
}