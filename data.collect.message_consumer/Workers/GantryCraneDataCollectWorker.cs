using System.Threading;
using System.Threading.Tasks;
using data.collection.middleware.util.Rabbit;
using data.collection.repository.influxdb;
using data.collection.util.Enums;
using Microsoft.Extensions.Logging;

namespace data.collect.message_consumer.Workers
{
    public class GantryCraneDataCollectWorker : BaseWorker
    {
        public GantryCraneDataCollectWorker(IRabbitMQBusiness rabbitMqBusiness, IInfluxDbRepository influxDbRepository,
            ILogger<GantryCraneDataCollectWorker> logger) : base(rabbitMqBusiness, influxDbRepository, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SaveDataAsync(stoppingToken);
        }

        protected override DeviceType type => DeviceType.龙门吊;
    }
}