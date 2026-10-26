using Clientes.Application.Common;
using Clientes.Application.DTOs;
using Clientes.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.API.Controllers
{
    [ApiController]
    [Route("api/v1/Clientes/")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<ActionResult<ResultViewModel<ClienteResponse>>> CadastrarCliente([FromBody] CadastrarClienteRequest request)
        {
            var resultado = await _clienteService.CadastrarClienteAsync(request);

            if (!resultado.IsSuccess)
            {
                return BadRequest(ResultViewModel<ClienteResponse>.Failure(resultado.Error));
            }

            var cliente = resultado.Value;

            return CreatedAtAction(
                nameof(GetByIdCliente),
                new { id = cliente.Id },
                ResultViewModel<ClienteResponse>.Success(cliente));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResultViewModel<ClienteResponse>>> GetByIdCliente([FromRoute] Guid id)
        {
            var resultado = await _clienteService.BuscarClientePorIdAsync(id);

            if (!resultado.IsSuccess)
            {
                return NotFound(ResultViewModel<ClienteResponse>.Failure(resultado.Error));
            }

            return Ok(ResultViewModel<ClienteResponse>.Success(resultado.Value));
        }

    }
}
