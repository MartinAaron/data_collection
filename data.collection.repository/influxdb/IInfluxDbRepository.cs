using InfluxData.Net.InfluxDb.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace data.collection.repository.influxdb
{
    public interface IInfluxDbRepository
    {
        Task WriteAsync(Dictionary<string, object> tags, Dictionary<string, object> fields, string measurementName);
        Task<IEnumerable<Serie>> GetAsync(string query);
        void Write(string message);
    }
}
