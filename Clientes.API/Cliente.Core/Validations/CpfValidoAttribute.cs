using System.ComponentModel.DataAnnotations;

namespace Clientes.Core.Validations
{
    public sealed class CpfValidoAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            if (value is not string cpf)
                return false;

            if (string.IsNullOrWhiteSpace(cpf))
                return true;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            return ValidarDigitos(cpf);
        }

        private static bool ValidarDigitos(string cpf)
        {
            int[] pesos1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] pesos2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = pesos1
                .Select((peso, i) => (cpf[i] - '0') * peso)
                .Sum();

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            soma = pesos2
                .Select((peso, i) => (cpf[i] - '0') * peso)
                .Sum();

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf[9] - '0' == digito1
                && cpf[10] - '0' == digito2;
        }
    }
}
