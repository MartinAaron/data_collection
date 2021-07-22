using System.Threading.Tasks;
using data.collection.middleware.util.Rabbit;
using data.collection.util.Enums;
using data.collection.util.Extentions;
using data.collection.util.IdWorker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace data.collection.api.Controllers.RabbitTest
{
    [AllowAnonymous]
    [Route("rabbit")]
    public class RabbitMQTestController : BaseController
    {
        public RabbitMQTestController(SnowWorker snowWorker, DataCollectionPushHandler dataCollectionPushHandler) : base(snowWorker)
        {
            _dataCollectionPushHandler = dataCollectionPushHandler;
        }
        private readonly DataCollectionPushHandler _dataCollectionPushHandler;

        /// <summary>
        /// 发送消息到RabbitMQ
        /// </summary>
        /// <param name="message"></param>
        [Route("send_message")]
        [HttpGet]
        public async Task SendMessage(string message)
        {
            var command = new Command()
            {
                Message = "Test Message",
                RoutingKey = DeviceType.人员定位.GetDescription(),
                ExchangeName = "data_collect"
            };
            await _dataCollectionPushHandler.SendMessage(command);
        }
    }
}