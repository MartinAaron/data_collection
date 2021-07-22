using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using data.collection.middleware.util.Rabbit;
using data.collection.repository.influxdb;
using data.collection.util.Enums;
using data.collection.util.Extentions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace data.collect.message_consumer.Workers
{
    public abstract class BaseWorker : BackgroundService
    {
        protected BaseWorker(IRabbitMQBusiness rabbitMqBusiness, IInfluxDbRepository influxDbRepository,
            ILogger<BaseWorker> logger)
        {
            _rabbitMqBusiness = rabbitMqBusiness;
            _influxDbRepository = influxDbRepository;
            _logger = logger;
        }

        private readonly IRabbitMQBusiness _rabbitMqBusiness;
        private readonly IInfluxDbRepository _influxDbRepository;
        private readonly ILogger<BaseWorker> _logger;

        protected string exchange => "data_collect";
        protected abstract DeviceType type { get; }

        protected virtual async Task SaveDataAsync(CancellationToken stoppingToken)
        {
            try
            {
                var channel = await _rabbitMqBusiness.GetConnection();
                channel.ExchangeDeclare(exchange, type: ExchangeType.Topic);
                channel.QueueDeclare(queue: $"{exchange}.{type.GetDescription()}", exclusive: false);
                channel.QueueBind(queue: $"{exchange}.{type.GetDescription()}", exchange, routingKey: type.GetDescription());

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("DataCollectWorker running at: {time}", DateTimeOffset.Now);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        var entityString = Encoding.UTF8.GetString(body);
                        _logger.LogInformation($"- received message" + $"：{entityString}");
                        try
                        {
                            if (!entityString.IsNullOrEmpty())
                                _influxDbRepository.Write(entityString);
                        }
                        catch (Exception e)
                        {
                            _logger.LogWarning($"- {e.Message}");
                        }

                        channel.BasicAck(ea.DeliveryTag, false);
                    };

                    channel.BasicConsume(queue: $"{exchange}.{type.GetDescription()}", autoAck: false, consumer: consumer);
                    await Task.Delay(2000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }
}