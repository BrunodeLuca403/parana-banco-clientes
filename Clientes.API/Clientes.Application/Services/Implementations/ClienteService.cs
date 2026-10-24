using Clientes.Application.Common;
using Clientes.Application.DTOs;
using Clientes.Application.Services.Interface;
using Clientes.Core.Entitys;
using Clientes.Core.Repository;

namespace Clientes.Application.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ResultViewModel<ClienteResponse>> CadastrarClienteAsync(CadastrarClienteRequest request)
        {
            try
            {
                if (await _clienteRepository.ExisteCpfAsync(request.Cpf))
                    return ResultViewModel<ClienteResponse>.Failure(Error.Validation("Clientes.Application.Services.Implementations.CadastrarCliente", "Já existe um cliente cadastrado com este CPF."));

                var cliente = new Cliente(
                    request.Nome.Trim(),
                    request.DataNascimento,
                    request.Cpf,
                    request.Email.Trim());

                await _clienteRepository.CadastrarClienteAsync(cliente);

                var response = ClienteResponse.FromEntity(cliente);

                return ResultViewModel<ClienteResponse>.Success(response);

            } catch (Exception ex) {

                return ResultViewModel<ClienteResponse>.Failure(Error.Failure("Clientes.Application.Services.Implementations.CadastrarCliente", ex.Message));
            }
            
        }

        public async Task<ResultViewModel<ClienteResponse?>> BuscarClientePorIdAsync(Guid clienteId)
        {
            var cliente = await _clienteRepository.BuscarClienteIdAsync(clienteId);

            var DtoCliente = new ClienteResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Cpf = cliente.Cpf,
                DataNascimento = cliente.DataNascimento
            };

            if(cliente is null)
            {
                return ResultViewModel<ClienteResponse?>.Failure(Error.NotFound("Clientes.Application.Services.Implementations.BuscarClientePorIdAsync", $"Cliente com Id {clienteId} não encontrado"));
            }

            return ResultViewModel<ClienteResponse?>.Success(DtoCliente);

        }
    }
}
