using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.entity.DataCollect;

namespace data.collect.Spider.ZLHtmlNews
{
    public interface IZLHtmlNews
    {
        Task<IList<ZJMachineCollectionEntity>> GetZLHtmlData();
    }
}