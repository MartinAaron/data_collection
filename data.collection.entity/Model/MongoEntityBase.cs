using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace data.collection.entity.Model
{
    public class MongoEntityBase
    {
        /// <summary>
        /// MongoDB系统自带的主键
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("id")] public string Id { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [BsonElement("add_time")]
        public DateTime AddTime { get; set; } = DateTime.Now;
    }
}