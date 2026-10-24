using Clientes.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clientes.Core.Repository
{
    public interface IClienteRepository
    {
        Task CadastrarClienteAsync(Cliente cliente);
        Task<Cliente?> BuscarClienteIdAsync(Guid clienteId);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}
