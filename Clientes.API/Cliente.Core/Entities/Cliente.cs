using Clientes.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clientes.Core.Entitys
{
    public class Cliente : BaseEntity
    {
        public Cliente(string nome, DateTime dataNascimento, string cpf, string email, decimal rendaMensal)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            Cpf = cpf;
            Email = email;
            RendaMensal = rendaMensal;

        }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Cpf { get; private set; }
        public string Email { get; private set; }
        public decimal RendaMensal { get; private set; }
    }
}
