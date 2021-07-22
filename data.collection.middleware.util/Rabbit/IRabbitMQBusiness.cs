using System.Threading.Tasks;
using RabbitMQ.Client;

namespace data.collection.middleware.util.Rabbit
{
    public interface IRabbitMQBusiness
    {
        Task<IModel> GetConnection();
    }
}