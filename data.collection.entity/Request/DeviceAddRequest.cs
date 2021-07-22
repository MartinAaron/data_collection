using System;
using System.Collections.Generic;
using data.collection.util.Enums;
using data.collection.util.Extentions;

namespace data.collection.entity.Request
{
    public class DeviceAddRequest
    {
        public DeviceType device_type { get; set; }
        public string device_name { get; set; }
        public string project_id { get; set; }
        public long complete_time { get; set; } = DateTime.Now.GetTimeStamp();
        public Dictionary<string, object> meta_data { get; set; }
    }
}