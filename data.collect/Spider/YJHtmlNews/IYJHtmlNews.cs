using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.entity.DataCollect;

namespace data.collect.Spider.YJHtmlNews
{
    public interface IYJHtmlNews
    {
        Task<IList<YJMachineCollectEntity>> GetYJHtmlData();
    }
}
