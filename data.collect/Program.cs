using data.collect.Collect;
using data.collect.Spider;
using data.collect.Spider.YJHtmlNews;
using data.collect.Spider.ZLHtmlNews;
using data.collection.util.ApiClient;
using data.collection.util.Extentions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace data.collect
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
                    services.AddSingleton<IYJHtmlNews, YJHtmlNews>();
                    services.AddSingleton<IZLHtmlNews, ZLHtmlNews>();
                    //Add Http Client
                    services.AddHttpClient();
                    services.AddSingleton(typeof(ApiClient));
                    services.AddSingleton<IDataScanBusiness, DataScanBusiness>();
                    services.AddHostedService<Worker>();
                });
    }
}