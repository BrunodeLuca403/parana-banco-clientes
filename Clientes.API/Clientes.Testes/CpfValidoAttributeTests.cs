using Clientes.Core.Validations;

namespace Clientes.Testes
{
    public class CpfValidoAttributeTests
    {
        private readonly CpfValidoAttribute _sut = new();

        [Theory]
        [InlineData("11144477735")]   // CPF válido (só dígitos)
        [InlineData("111.444.777-35")] // mesmo CPF, com máscara
        public void IsValid_QuandoCpfValido_DeveRetornarTrue(string cpf)
        {
            // Act
            var resultado = _sut.IsValid(cpf);

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("12345678900")]   
        [InlineData("11111111111")]  
        [InlineData("123")]          
        [InlineData("abcdefghijk")]   
        public void IsValid_QuandoCpfInvalido_DeveRetornarFalse(string cpf)
        {
            // Act
            var resultado = _sut.IsValid(cpf);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void IsValid_QuandoValorNulo_DeveRetornarTrue()
        {
            var resultado = _sut.IsValid(null);
            Assert.True(resultado);
        }
    }
}
