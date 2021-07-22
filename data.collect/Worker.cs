using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using data.collect.Collect;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace data.collect
{
    public class Worker : BackgroundService
    {
        public Worker(IDataScanBusiness dataScanBusiness, ILogger<Worker> logger)
        {
            _dataScanBusiness = dataScanBusiness;
            _logger = logger;
        }

        private readonly IDataScanBusiness _dataScanBusiness;
        private readonly ILogger<Worker> _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var cts = new CancellationTokenSource();
                Task.Factory.StartNew(state => _dataScanBusiness.ScanYJ(stoppingToken),
                    cts, cts.Token);
                Task.Factory.StartNew(state => _dataScanBusiness.ScanZL(stoppingToken),
                    cts, cts.Token);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }
}