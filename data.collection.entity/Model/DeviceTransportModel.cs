using System.Collections.Generic;

namespace data.collection.entity.Model
{
    public class DeviceTransportModel
    {
        public string TableName { get; set; }
        public Dictionary<string, object> Fields { get; set; }
        public Dictionary<string, object> Tags { get; set; }
    }
}