using data.collection.util.DI;
using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using InfluxData.Net.InfluxDb.Models.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using data.collection.entity.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace data.collection.repository.influxdb
{
    public class InfluxDbRepository : IInfluxDbRepository, ISingletonDependency
    {
        public InfluxDbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var url = _configuration["InfluxDb"];
            _client = new InfluxDbClient(_configuration["InfluxDb"], string.Empty, string.Empty, InfluxDbVersion.v_1_3);
        }

        private readonly IConfiguration _configuration;
        private readonly InfluxDbClient _client;

        public async Task<IEnumerable<Serie>> GetAsync(string query)
        {
            return await _client.Client.QueryAsync(query, _configuration["InfluxDbName"],
                epochFormat: "%Y%m%d %H:%M:%S");
        }

        public void Write(string message)
        {
            JsonReader reader = new JsonTextReader(new StringReader(message));

            string entityStr = String.Empty;
            while (reader.Read())
            {
                entityStr = reader.Value.ToString();
            }
            var entity = JsonConvert.DeserializeObject<DeviceTransportModel>(entityStr ?? string.Empty);
            
            var dic = new Dictionary<string, object>();
            
            foreach (var (key, value) in entity.Fields)
            {
                dic.Add(key, value.ToString());
            }
            
            var pointToWrite = new Point()
            {
                Name = entity.TableName,
                Tags = entity.Tags,
                Fields = dic,
                Timestamp = DateTime.UtcNow
            };

            _client.Client.WriteAsync(pointToWrite, _configuration["InfluxDbName"]).Wait();
        }

        public async Task WriteAsync(Dictionary<string, object> tags, Dictionary<string, object> fields,
            string measurementName)
        {
            var dic = new Dictionary<string, object>();

            foreach (var (key, value) in fields)
            {
                dic.Add(key, value.ToString());
            }

            var pointToWrite = new Point()
            {
                Name = measurementName,
                Tags = tags,
                Fields = dic,
                Timestamp = DateTime.UtcNow
            };

            await _client.Client.WriteAsync(pointToWrite, _configuration["InfluxDbName"]);
        }
    }
}