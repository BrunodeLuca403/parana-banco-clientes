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
        public async Task<ActionResult<ClienteResponse>> CadastrarCliente([FromBody] CadastrarClienteRequest request)
        {
            var cadastrarCliente = await _clienteService.CadastrarClienteAsync(request);

            if (!cadastrarCliente.IsSuccess)
            {
                return BadRequest(cadastrarCliente.Error);
            }
            return Ok(cadastrarCliente);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClienteResponse>> GetByIdCares([FromRoute] Guid id)
        {
            var cliente = await _clienteService.BuscarClientePorIdAsync(id);
            if (!cliente.IsSuccess)
            {
                return NotFound(cliente.Error);
            }
            return Ok(cliente);
        }

    }
}
