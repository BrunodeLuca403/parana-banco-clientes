using Clientes.Application.Common;
using Clientes.Application.DTOs;
using Clientes.Application.Services.Interface;
using Clientes.Core.Entitys;
using Clientes.Core.Events;
using Clientes.Core.Repository;

namespace Clientes.Application.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteEventPublisher _eventPublisher;

        public ClienteService(
            IClienteRepository clienteRepository,
            IClienteEventPublisher eventPublisher)
        {
            _clienteRepository = clienteRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<ResultViewModel<ClienteResponse>> CadastrarClienteAsync(CadastrarClienteRequest request)
        {
            try
            {

                var cpfNormalizado = new string(request.Cpf.Where(char.IsDigit).ToArray());

                if (await _clienteRepository.ExisteCpfAsync(cpfNormalizado))
                    return ResultViewModel<ClienteResponse>.Failure(Error.Validation("Clientes.Application.Services.Implementations.CadastrarCliente", "Já existe um cliente cadastrado com este CPF."));

                if (request.DataNascimento > DateTime.Today.AddYears(-18))
                    return ResultViewModel<ClienteResponse>.Failure(Error.Validation("Clientes.Application.Services.Implementations.CadastrarCliente", "O cliente deve ter pelo menos 18 anos."));

                    var cliente = new Cliente(
                    request.Nome.Trim(),
                    request.DataNascimento,
                    cpfNormalizado,
                    request.Email.Trim(),
                    request.RendaMensal);

                await _clienteRepository.CadastrarClienteAsync(cliente);

                var evento = new ClienteCadastradoEvent(
                    cliente.Id,
                    cliente.Nome,
                    cliente.Cpf,
                    cliente.Email,
                    cliente.RendaMensal,
                    cliente.DataNascimento,
                    DateTime.UtcNow);

                await _eventPublisher.PublicarClienteCadastradoAsync(evento);

                var response = ClienteResponse.FromEntity(cliente);

                return ResultViewModel<ClienteResponse>.Success(response);

            } catch (Exception ex) {

                return ResultViewModel<ClienteResponse>.Failure(Error.Failure("Clientes.Application.Services.Implementations.CadastrarCliente", "Erro interno ao cadastrar o cliente."));
            }
            
        }

        public async Task<ResultViewModel<ClienteResponse?>> BuscarClientePorIdAsync(Guid clienteId)
        {
            var cliente = await _clienteRepository.BuscarClienteIdAsync(clienteId);

            if(cliente is null)
            {
                return ResultViewModel<ClienteResponse?>.Failure(Error.NotFound("Clientes.Application.Services.Implementations.BuscarClientePorIdAsync", $"Cliente com Id {clienteId} não encontrado"));
            }
            var DtoCliente = new ClienteResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Cpf = cliente.Cpf,
                DataNascimento = cliente.DataNascimento
            };

            

            return ResultViewModel<ClienteResponse?>.Success(DtoCliente);

        }
    }
}
