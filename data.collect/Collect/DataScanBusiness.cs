using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using data.collect.Spider.YJHtmlNews;
using data.collect.Spider.ZLHtmlNews;
using data.collection.business.Interface;
using data.collection.entity.Request;
using data.collection.util.Enums;
using data.collection.util.Extentions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace data.collect.Collect
{
    public class DataScanBusiness : IDataScanBusiness
    {
        public DataScanBusiness(ILogger<DataScanBusiness> logger, IYJHtmlNews yJHtmlNews,
            IDeviceBusiness deviceBusiness, IZLHtmlNews zlHtmlNews)
        {
            _logger = logger;
            _yJHtmlNews = yJHtmlNews;
            _deviceBusiness = deviceBusiness;
            _zlHtmlNews = zlHtmlNews;
        }

        private readonly ILogger<DataScanBusiness> _logger;
        private readonly IYJHtmlNews _yJHtmlNews;
        private readonly IDeviceBusiness _deviceBusiness;
        private readonly IZLHtmlNews _zlHtmlNews;

        public async Task ScanYJ(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning("Worker running at: {time}", DateTimeOffset.Now);

                var res = await _yJHtmlNews.GetYJHtmlData();
                foreach (var model in res)
                {
                    var entity = await _deviceBusiness.GetByConditionAsync(new CommonRequest()
                    {
                        type = DeviceType.压浆机,
                        start_time = model.CompleteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        end_time = model.CompleteTime.ToString("yyyy-MM-dd HH:mm:ss")
                    }, false);
                    if (entity.Values == null || (entity.Values != null && !entity.Values.Any()))
                    {
                        //更新到Db
                        var saveEntity = new DeviceAddRequest();
                        saveEntity.device_type = DeviceType.压浆机;
                        saveEntity.project_id = "321";
                        saveEntity.complete_time = model.CompleteTime.GetTimeStamp();
                        saveEntity.device_name = "一局临金项目梁场";
                        saveEntity.meta_data = model.ObjectToDictionary();
                        await _deviceBusiness.AddAsync(saveEntity);
                        _logger.LogInformation($"{JsonConvert.SerializeObject(saveEntity)} 保存成功");
                    }
                    else
                    {
                        _logger.LogInformation($"{JsonConvert.SerializeObject(model)} 数据重复，忽略");
                    }
                }

                await Task.Delay(1000 * 60 * 21, stoppingToken);
            }
        }

        public async Task ScanZL(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogWarning("Worker running at: {time}", DateTimeOffset.Now);
                    var res = await _zlHtmlNews.GetZLHtmlData();
                    _logger.LogInformation(res.Count.ToString());
                    foreach (var model in res)
                    {
                        var entity = await _deviceBusiness.GetByConditionAsync(new CommonRequest()
                        {
                            type = DeviceType.张拉机,
                            start_time = model.CompleteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            end_time = model.CompleteTime.ToString("yyyy-MM-dd HH:mm:ss")
                        }, false);
                        if (entity.Values == null || (entity.Values != null && !entity.Values.Any()))
                        {
                            //更新到Db
                            var saveEntity = new DeviceAddRequest();
                            saveEntity.device_type = DeviceType.张拉机;
                            saveEntity.project_id = "320";
                            saveEntity.complete_time = model.CompleteTime.GetTimeStamp();
                            saveEntity.device_name = "一局临金项目蒋家桥";
                            saveEntity.meta_data = model.ObjectToDictionary();
                            await _deviceBusiness.AddAsync(saveEntity);
                            _logger.LogInformation($"{JsonConvert.SerializeObject(saveEntity)} 保存成功");
                        }
                        else
                        {
                            _logger.LogInformation($"{JsonConvert.SerializeObject(model)} 数据重复，忽略");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }

                await Task.Delay(1000 * 60 * 20, stoppingToken);
            }
        }
    }
}