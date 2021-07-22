using System;
using data.collection.util.Enums;

namespace data.collection.entity.Request
{
    public class CommonRequest
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        public DeviceType type { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string device_name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public string project_id { get; set; }
    }
}