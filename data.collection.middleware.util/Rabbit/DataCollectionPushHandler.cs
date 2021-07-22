using System.Text;
using System.Threading.Tasks;
using data.collection.util.DI;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace data.collection.middleware.util.Rabbit
{
    public class DataCollectionPushHandler : ISingletonDependency
    {
        public DataCollectionPushHandler(IRabbitMQBusiness business)
        {
            _business = business;
        }

        private readonly IRabbitMQBusiness _business;

        public async Task SendMessage(Command command)
        {
            using var channel = await _business.GetConnection();
            channel.ExchangeDeclare(command.ExchangeName, type: ExchangeType.Topic);
            channel.BasicPublish(command.ExchangeName, routingKey: command.RoutingKey, basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command.Message)));
        }
    }
}