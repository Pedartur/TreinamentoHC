using Microsoft.AspNetCore.Mvc;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Services;

namespace ProdutoAPI.Controllers
{
    /// <summary>
    /// Controller que define os endpoints da API REST para Produtos
    /// Responsabilidade: Receber requisições HTTP e retornar respostas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoService produtoService, ILogger<ProdutosController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/produtos
        /// Método HTTP: GET
        /// Características: Safe (read-only), Idempotente
        /// Retorna: Lista de todos os produtos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> ObterTodos()
        {
            _logger.LogInformation("GET /api/produtos - Listando todos os produtos");
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }

        /// <summary>
        /// GET /api/produtos/{id}
        /// Método HTTP: GET
        /// Características: Safe (read-only), Idempotente
        /// Retorna: Um produto específico pelo Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> ObterPorId(int id)
        {
            _logger.LogInformation($"GET /api/produtos/{id} - Obtendo produto específico");

            var produto = await _produtoService.ObterPorIdAsync(id);

            if (produto == null)
            {
                _logger.LogWarning($"Produto com Id {id} não encontrado");
                return NotFound(new { mensagem = $"Produto com Id {id} não encontrado" });
            }

            return Ok(produto);
        }

        /// <summary>
        /// POST /api/produtos
        /// Método HTTP: POST
        /// Características: Não idempotente (cada chamada cria um novo recurso)
        /// Entrada: CriarProdutoDto com dados do novo produto
        /// Retorna: O produto criado com seu Id gerado
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> Criar([FromBody] CriarProdutoDto criarProdutoDto)
        {
            _logger.LogInformation($"POST /api/produtos - Criando novo produto");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produtoCriado = await _produtoService.CriarAsync(criarProdutoDto);

                // Retorna 201 Created com a localização do novo recurso
                return CreatedAtAction(nameof(ObterPorId), new { id = produtoCriado.Id }, produtoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/produtos/{id}
        /// Método HTTP: PUT
        /// Características: Idempotente (múltiplas chamadas têm o mesmo resultado)
        /// Entrada: Id do produto e AtualizarProdutoDto com dados atualizados
        /// Retorna: O produto atualizado
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoDto>> Atualizar(int id, [FromBody] AtualizarProdutoDto atualizarProdutoDto)
        {
            _logger.LogInformation($"PUT /api/produtos/{id} - Atualizando produto");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produtoAtualizado = await _produtoService.AtualizarAsync(id, atualizarProdutoDto);

                if (produtoAtualizado == null)
                {
                    return NotFound(new { mensagem = $"Produto com Id {id} não encontrado" });
                }

                return Ok(produtoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/produtos/{id}
        /// Método HTTP: DELETE
        /// Características: Idempotente (chamar novamente retorna 404, mesmo resultado esperado)
        /// Entrada: Id do produto a ser deletado
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            _logger.LogInformation($"DELETE /api/produtos/{id} - Deletando produto");

            var resultado = await _produtoService.DeletarAsync(id);

            if (!resultado)
            {
                return NotFound(new { mensagem = $"Produto com Id {id} não encontrado" });
            }

            // 204 No Content - indica sucesso mas sem corpo na resposta
            return NoContent();
        }
    }
}