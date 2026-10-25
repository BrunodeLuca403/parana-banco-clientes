using Clientes.Application.DTOs;
using Clientes.Application.Services.Implementations;
using Clientes.Core.Entitys;
using Clientes.Core.Events;
using Clientes.Core.Repository;
using Clientes.Testes.Faker;
using Moq;

namespace Clientes.Testes
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _repositoryMock = new();
        private readonly Mock<IClienteEventPublisher> _publisherMock = new();
        private readonly ClienteService _sut;

        public ClienteServiceTests()
        {
            _sut = new ClienteService(_repositoryMock.Object, _publisherMock.Object);
        }

        private static string SomenteDigitos(string valor) =>
            new(valor.Where(char.IsDigit).ToArray());

        [Fact]
        public async Task CadastrarCliente_QuandoCpfComMascara_DevePersistirSomenteDigitos()
        {
            // Arrange
            var request = FakerClienteDataHelper.CreateFakerInsertClient();
            request.Cpf = "620.575.440-10";
            _repositoryMock.Setup(r => r.ExisteCpfAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var resultado = await _sut.CadastrarClienteAsync(request);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal("62057544010", resultado.Value!.Cpf);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoDadosValidos_DeveRetornarSucesso()
        {
            // Arrange
            var request = FakerClienteDataHelper.CreateFakerInsertClient();
            var cpfNormalizado = SomenteDigitos(request.Cpf);
            _repositoryMock.Setup(r => r.ExisteCpfAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var resultado = await _sut.CadastrarClienteAsync(request);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(request.Nome, resultado.Value!.Nome);
            Assert.Equal(cpfNormalizado, resultado.Value.Cpf);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoDadosValidos_DevePersistirEPublicarEvento()
        {
            // Arrange
            var request = FakerClienteDataHelper.CreateFakerInsertClient();
            var cpfNormalizado = SomenteDigitos(request.Cpf);
            _repositoryMock.Setup(r => r.ExisteCpfAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            await _sut.CadastrarClienteAsync(request);

            // Assert
            _repositoryMock.Verify(r => r.CadastrarClienteAsync(It.IsAny<Cliente>()), Times.Once);

            _publisherMock.Verify(
                p => p.PublicarClienteCadastradoAsync(
                    It.Is<ClienteCadastradoEvent>(e => e.Cpf == cpfNormalizado),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoCpfJaExiste_DeveFalharENaoPublicarEvento()
        {
            // Arrange
            var request = FakerClienteDataHelper.CreateFakerInsertClient();
            _repositoryMock.Setup(r => r.ExisteCpfAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var resultado = await _sut.CadastrarClienteAsync(request);

            // Assert
            Assert.False(resultado.IsSuccess);

            _repositoryMock.Verify(r => r.CadastrarClienteAsync(It.IsAny<Cliente>()), Times.Never);
            _publisherMock.Verify(
                p => p.PublicarClienteCadastradoAsync(
                    It.IsAny<ClienteCadastradoEvent>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task CadastrarCliente_QuandoMenorDeIdade_DeveFalhar()
        {
            // Arrange
            var request = FakerClienteDataHelper.CreateFakerInsertClient();
            request.DataNascimento = DateTime.Today.AddYears(-17);
            _repositoryMock.Setup(r => r.ExisteCpfAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var resultado = await _sut.CadastrarClienteAsync(request);

            // Assert
            Assert.False(resultado.IsSuccess);
            _repositoryMock.Verify(r => r.CadastrarClienteAsync(It.IsAny<Cliente>()), Times.Never);
        }
    }
}
