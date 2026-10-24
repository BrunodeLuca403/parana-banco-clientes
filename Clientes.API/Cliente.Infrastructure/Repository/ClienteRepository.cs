using Clientes.Core.Entitys;
using Clientes.Core.Repository;
using Clientes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clientes.Infrastructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteDbContext _Repository;

        public ClienteRepository(ClienteDbContext repository)
        {
            _Repository = repository;
        }

        public async Task<Cliente?> BuscarClienteIdAsync(Guid clienteId)
        {
            return await _Repository.Clientes
                .SingleOrDefaultAsync(c => c.Id == clienteId && !c.IsDelete);
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _Repository.Clientes
                .AnyAsync(c => c.Cpf == cpf && !c.IsDelete);
        }

        public async Task CadastrarClienteAsync(Cliente cliente)
        {
            await _Repository.Clientes.AddAsync(cliente);
            await _Repository.SaveChangesAsync();
        }
    }
}
