using System.Threading.Tasks;
using data.collection.util.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace data.collection.middleware.util.Rabbit
{
    public class RabbitMQBusiness : IRabbitMQBusiness, ISingletonDependency
    {
        public RabbitMQBusiness(ILogger<RabbitMQBusiness> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private readonly ILogger<RabbitMQBusiness> _logger;
        private readonly IConfiguration _configuration;

        public async Task<IModel> GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:Connection"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:User"],
                Password = _configuration["RabbitMQ:Password"]
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            _logger.LogInformation("connected to rabbit mq : dc.rabbit.crservice.cn ");
            return channel;
        }
    }
}