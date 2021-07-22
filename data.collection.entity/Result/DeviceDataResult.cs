using System.Collections.Generic;

namespace data.collection.entity.Result
{
    public class DeviceDataResult
    {
        public IList<string>? Colums { get; set; }
        public List<IList<object>>? Values { get; set; }
    }
}