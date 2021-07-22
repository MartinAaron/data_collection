using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data.collect.message_consumer.Workers;
using data.collection.util.Extentions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace data.collect.message_consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFxServicesForHostedService();

                    services.AddHostedService<PersonnelDataCollectWorker>();
                    services.AddHostedService<BridgeDataCollectWorker>();
                    services.AddHostedService<GantryCraneDataCollectWorker>();
                    services.AddHostedService<TransportDataCollectWorker>();
                });
    }
}