using Clientes.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Clientes.Core.Entitys
{
    public class Cliente : BaseEntity
    {
        public Cliente(string nome, DateTime dataNascimento, string cpf, string email)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            Cpf = cpf;
            Email = email;
        }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3)]
        public string Nome { get; private set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; private set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [CpfValido(ErrorMessage = "O CPF informado é inválido.")]
        public string Cpf { get; private set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado é inválido.")]
        public string Email { get; private set; }
    }
}
