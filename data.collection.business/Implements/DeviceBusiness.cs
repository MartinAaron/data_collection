using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using data.collection.business.Interface;
using data.collection.entity.Model;
using data.collection.entity.Request;
using data.collection.entity.Result;
using data.collection.middleware.util.Rabbit;
using data.collection.repository.influxdb;
using data.collection.util.DI;
using data.collection.util.Enums;
using data.collection.util.Extentions;
using data.collection.util.IdWorker;
using InfluxData.Net.InfluxDb.Models.Responses;
using Newtonsoft.Json;
using ZR.DataHunter.Util;

namespace data.collection.business.Implements
{
    public class DeviceBusiness : BaseBusiness, IDeviceBusiness, IScopedDependency
    {
        public DeviceBusiness(IInfluxDbRepository influxDbRepository, SnowWorker snowWorker,
            DataCollectionPushHandler dataCollectionPushHandler)
        {
            _influxDbRepository = influxDbRepository;
            _snowWorker = snowWorker;
            _dataCollectionPushHandler = dataCollectionPushHandler;
        }

        private readonly IInfluxDbRepository _influxDbRepository;
        private readonly SnowWorker _snowWorker;
        private readonly DataCollectionPushHandler _dataCollectionPushHandler;

        public async Task<IEnumerable<SelectOption>> GetDeviceTypeAsync()
        {
            return await base.GetEnumSelectOption(type: typeof(DeviceType));
        }

        public async Task AddAsync(DeviceAddRequest request)
        {
            var deviceTransportModel = new DeviceTransportModel()
            {
                TableName = request.device_type.GetDescription(),
            };

            var field = new Dictionary<string, object>();

            field.Add("Device_name", request.device_name);
            field.Add("Project_id", request.project_id);
            field.Add("Create_time", request.complete_time);
            foreach (var data in request.meta_data)
            {
                field.Add(data.Key,data.Value.ToString());
            }
            
            deviceTransportModel.Fields = field;
            deviceTransportModel.Tags = new Dictionary<string, object>()
            {
                {"DeviceSerial", _snowWorker.GetId()}
            };

            await _dataCollectionPushHandler.SendMessage(new Command()
            {
                ExchangeName = "data_collect", Message = JsonConvert.SerializeObject(deviceTransportModel),
                RoutingKey = request.device_type.GetDescription()
            });
        }

        public async Task<DeviceDataResult> GetByConditionAsync(CommonRequest request, bool isSingle)
        {
            var tableName = request.type.GetType()
                .GetMember(request.type.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description;

            var sqlStringBuilder =
                new StringBuilder($"select * from {tableName} where Device_name != '0'   ");
            if (!request.device_name.IsNullOrEmpty())
            {
                sqlStringBuilder.Append($" and Device_name='{request.device_name}' ");
            }

            if (!request.project_id.IsNullOrEmpty())
            {
                sqlStringBuilder.Append($" and Project_id='{request.project_id}' ");
            }

            sqlStringBuilder.Append(" order by time desc ");

            var resList = await _influxDbRepository.GetAsync(sqlStringBuilder.ToString());
            if (resList.Any())
            {
                return await GetDeviceDataResult(resList, request.start_time, request.end_time, isSingle, request.type);
            }
            else return new DeviceDataResult();
        }

        private async Task<DeviceDataResult> GetDeviceDataResult(IEnumerable<Serie> resList, string start_time,
            string end_time, bool isSingle, DeviceType type)
        {
            var res = new DeviceDataResult();
            var t = resList.FirstOrDefault() ?? new Serie();
            res.Colums = t.Columns;
            var tValue = t.Values;
            var temp = t.Values.ToList();
            if (!start_time.IsNullOrEmpty() && !end_time.IsNullOrEmpty())
            {
                temp.Clear();
                foreach (var obj in tValue)
                {
                    var createTimeText = string.Empty;
                    if (type == DeviceType.张拉机)
                        createTimeText = obj[15].ToString();
                    else
                        createTimeText = obj[1].ToString();

                    var createTime = createTimeText.StampToDateTime();
                    if (createTime >= Convert.ToDateTime(start_time) &&
                        createTime <= Convert.ToDateTime(end_time))
                    {
                        temp.Add(obj);
                    }
                }
            }

            res.Values = isSingle ? temp.Take(1).ToList() : temp;
            await Task.CompletedTask;
            return res;
        }
    }
}