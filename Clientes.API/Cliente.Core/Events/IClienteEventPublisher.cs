namespace Clientes.Core.Events
{
    public interface IClienteEventPublisher
    {
        Task PublicarClienteCadastradoAsync(ClienteCadastradoEvent evento, CancellationToken cancellationToken = default);
    }
}
