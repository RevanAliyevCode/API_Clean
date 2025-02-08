using System;
using System.Text;
using API.Application.Abstraction.Services.Producer;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace API.Infrastructure.Services.Producer;

public class ProducerService : IProducerService
{
    readonly string _hostName = "localhost";
    readonly string _queueName = "product-queue";

    public async Task ProduceAsync(string action, object message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _queueName, exclusive: false, autoDelete: false);

        var value = new
        {
            Action = action,
            Message = message
        };

        var json = JsonConvert.SerializeObject(value);

        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
    }
}
