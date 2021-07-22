using System.Threading;
using System.Threading.Tasks;
using data.collection.middleware.util.Rabbit;
using data.collection.repository.influxdb;
using data.collection.util.Enums;
using Microsoft.Extensions.Logging;

namespace data.collect.message_consumer.Workers
{
    public class PersonnelDataCollectWorker : BaseWorker
    {
        protected override DeviceType type => DeviceType.人员定位;

        public PersonnelDataCollectWorker(IRabbitMQBusiness rabbitMqBusiness, IInfluxDbRepository influxDbRepository,
            ILogger<PersonnelDataCollectWorker> logger) : base(rabbitMqBusiness, influxDbRepository, logger)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SaveDataAsync(stoppingToken);
        }
    }
}