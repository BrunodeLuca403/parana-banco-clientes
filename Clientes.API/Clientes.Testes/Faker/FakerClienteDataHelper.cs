using Bogus;
using Bogus.Extensions.Brazil;
using Clientes.Application.DTOs;

namespace Clientes.Testes.Faker
{
    public static class FakerClienteDataHelper
    {
        private static readonly Faker<CadastrarClienteRequest> _fakerRequest =
            new Faker<CadastrarClienteRequest>("pt_BR")
                .RuleFor(c => c.Nome, f => f.Person.FullName)
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Cpf, f => f.Person.Cpf())      
                .RuleFor(c => c.RendaMensal, f => f.Finance.Amount(1000, 20000))
                .RuleFor(c => c.DataNascimento, f => f.Date.Between(
                    DateTime.Today.AddYears(-80),   
                    DateTime.Today.AddYears(-18))); 

        public static CadastrarClienteRequest CreateFakerInsertClient() => _fakerRequest.Generate();
    }
}
