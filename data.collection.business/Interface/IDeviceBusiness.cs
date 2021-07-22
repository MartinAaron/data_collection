using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.entity.Model;
using data.collection.entity.Request;
using data.collection.entity.Result;
using InfluxData.Net.InfluxDb.Models.Responses;

namespace data.collection.business.Interface
{
    public interface IDeviceBusiness
    {
        Task<IEnumerable<SelectOption>> GetDeviceTypeAsync();
        Task AddAsync(DeviceAddRequest request);
        Task<DeviceDataResult> GetByConditionAsync(CommonRequest request, bool isSingle = false);
    }
}