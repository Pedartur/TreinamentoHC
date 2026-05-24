using Microsoft.AspNetCore.Mvc;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Services;

namespace ContatoAPI.Controllers
{
    /// <summary>
    /// Controller que define os endpoints da API REST para Contatos
    /// Responsabilidade: Receber requisições HTTP e retornar respostas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContatosController : ControllerBase
    {
        private readonly IContatoService _contatoService;
        private readonly ILogger<ContatosController> _logger;

        public ContatosController(IContatoService contatoService, ILogger<ContatosController> logger)
        {
            _contatoService = contatoService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/contatos
        /// Método HTTP: GET
        /// Características: Safe (read-only), Idempotente
        /// Retorna: Lista de todos os contatos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContatoDto>>> ObterTodos()
        {
            _logger.LogInformation("GET /api/contatos - Listando todos os contatos");
            var contatos = await _contatoService.ObterTodosAsync();
            return Ok(contatos);
        }

        /// <summary>
        /// GET /api/contatos/{id}
        /// Método HTTP: GET
        /// Características: Safe (read-only), Idempotente
        /// Retorna: Um contato específico pelo Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContatoDto>> ObterPorId(int id)
        {
            _logger.LogInformation($"GET /api/contatos/{id} - Obtendo contato específico");

            var contato = await _contatoService.ObterPorIdAsync(id);

            if (contato == null)
            {
                _logger.LogWarning($"Contato com Id {id} não encontrado");
                return NotFound(new { mensagem = $"Contato com Id {id} não encontrado" });
            }

            return Ok(contato);
        }

        /// <summary>
        /// POST /api/contatos
        /// Método HTTP: POST
        /// Características: Não idempotente (cada chamada cria um novo recurso)
        /// Entrada: CriarContatoDto com dados do novo contato
        /// Retorna: O contato criado com seu Id gerado
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ContatoDto>> Criar([FromBody] CriarContatoDto criarContatoDto)
        {
            _logger.LogInformation($"POST /api/contatos - Criando novo contato");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var contatoCriado = await _contatoService.CriarAsync(criarContatoDto);

                // Retorna 201 Created com a localização do novo recurso
                return CreatedAtAction(nameof(ObterPorId), new { id = contatoCriado.Id }, contatoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/contatos/{id}
        /// Método HTTP: PUT
        /// Características: Idempotente (múltiplas chamadas têm o mesmo resultado)
        /// Entrada: Id do contato e AtualizarContatoDto com dados atualizados
        /// Retorna: O contato atualizado
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ContatoDto>> Atualizar(int id, [FromBody] AtualizarContatoDto atualizarContatoDto)
        {
            _logger.LogInformation($"PUT /api/contatos/{id} - Atualizando contato");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var contatoAtualizado = await _contatoService.AtualizarAsync(id, atualizarContatoDto);

                if (contatoAtualizado == null)
                {
                    return NotFound(new { mensagem = $"Contato com Id {id} não encontrado" });
                }

                return Ok(contatoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/contatos/{id}
        /// Método HTTP: DELETE
        /// Características: Idempotente (chamar novamente retorna 404, mesmo resultado esperado)
        /// Entrada: Id do contato a ser deletado
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            _logger.LogInformation($"DELETE /api/contatos/{id} - Deletando contato");

            var resultado = await _contatoService.DeletarAsync(id);

            if (!resultado)
            {
                return NotFound(new { mensagem = $"Contato com Id {id} não encontrado" });
            }

            // 204 No Content - indica sucesso mas sem corpo na resposta
            return NoContent();
        }
    }
}