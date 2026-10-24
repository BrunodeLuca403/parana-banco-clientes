using System.ComponentModel.DataAnnotations;
using Clientes.Core.Validations;

namespace Clientes.Application.DTOs
{
    public class CadastrarClienteRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [CpfValido(ErrorMessage = "CPF inválido.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(254, ErrorMessage = "O e-mail deve ter no máximo 254 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A renda mensal é obrigatória.")]
        [Range(0.01, 999999999.99, ErrorMessage = "A renda mensal deve ser maior que zero.")]
        public decimal RendaMensal { get; private set; }
    }
}
