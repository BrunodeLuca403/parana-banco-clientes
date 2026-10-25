using Clientes.Core.Entitys;

namespace Clientes.Application.DTOs
{
    public class ClienteResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatAt { get; set; }
        public decimal RendaMensal { get; set; }

        public static ClienteResponse FromEntity(Cliente cliente) => new()
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            DataNascimento = cliente.DataNascimento,
            Cpf = cliente.Cpf,
            Email = cliente.Email,
            CreatAt = cliente.CreatAt,
            RendaMensal = cliente.RendaMensal
        };
    }
}
