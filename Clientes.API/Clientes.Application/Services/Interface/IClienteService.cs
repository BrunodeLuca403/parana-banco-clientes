using Clientes.Application.Common;
using Clientes.Application.DTOs;

namespace Clientes.Application.Services.Interface
{
    public interface IClienteService
    {
        Task<ResultViewModel<ClienteResponse>> CadastrarClienteAsync(CadastrarClienteRequest request);
        Task<ResultViewModel<ClienteResponse?>> BuscarClientePorIdAsync(Guid clienteId);
    }
}
