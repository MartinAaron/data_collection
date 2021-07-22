using data.collection.util.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace data.collection.identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddFxServices();
                    services.AddMongoClient();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}