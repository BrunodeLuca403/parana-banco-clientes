using System.Text;
using System.Text.Json;
using Clientes.Core.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Clientes.Infrastructure.Messaging
{
    public class ClienteEventPublisher : IClienteEventPublisher
    {
        private readonly IRabbitMqConnection _connection;
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<ClienteEventPublisher> _logger;

        public ClienteEventPublisher(
            IRabbitMqConnection connection,
            IOptions<RabbitMqSettings> options,
            ILogger<ClienteEventPublisher> logger)
        {
            _connection = connection;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task PublicarClienteCadastradoAsync(ClienteCadastradoEvent evento, CancellationToken cancellationToken = default)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: _settings.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString(),
                Type = nameof(ClienteCadastradoEvent)
            };

            await channel.BasicPublishAsync(
                exchange: _settings.Exchange,
                routingKey: _settings.ClienteCadastradoRoutingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Evento ClienteCadastrado publicado. ClienteId={ClienteId}, Exchange={Exchange}, RoutingKey={RoutingKey}",
                evento.ClienteId, _settings.Exchange, _settings.ClienteCadastradoRoutingKey);
        }
    }
}
