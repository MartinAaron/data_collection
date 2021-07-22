using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.business.Interface;
using data.collection.entity.Model;
using data.collection.entity.Request;
using data.collection.entity.Result;
using data.collection.util.IdWorker;
using Microsoft.AspNetCore.Mvc;

namespace data.collection.api.Controllers.DeviceManage
{
    /// <summary>
    /// 设备控制器
    /// </summary>
    [Route("api/device")]
    public class DeviceController : BaseController
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DeviceController(SnowWorker snowWorker, IDeviceBusiness deviceBusiness) : base(snowWorker)
        {
            _deviceBusiness = deviceBusiness;
        }

        private readonly IDeviceBusiness _deviceBusiness;

        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("type/get")]
        public async Task<IEnumerable<SelectOption>> GetDeviceType()
        {
            return await _deviceBusiness.GetDeviceTypeAsync();
        }

        /// <summary>
        /// 存入设备数据
        /// </summary>
        [HttpPut]
        [Route("save")]
        public async Task Add([FromBody] DeviceAddRequest request)
        {
            await _deviceBusiness.AddAsync(request);
        }

        /// <summary>
        /// 根据类型获取设备最新数据
        /// </summary>
        /// <returns></returns>
        [Route("get")]
        [HttpPost]
        public async Task<DeviceDataResult> Get([FromBody] CommonRequest request)
        {
            return await _deviceBusiness.GetByConditionAsync(request, true);
        }

        /// <summary>
        /// 根据类型/时间/名称获取列表
        /// </summary>
        /// <returns></returns>
        [Route("list/get")]
        [HttpPost]
        public async Task<DeviceDataResult> GetByCondition([FromBody] CommonRequest request)
        {
            return await _deviceBusiness.GetByConditionAsync(request);
        }
    }
}