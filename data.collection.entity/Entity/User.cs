using data.collection.entity.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace data.collection.entity.Entity
{
    
    [BsonIgnoreExtraElements]
    public class User : MongoEntityBase
    {
        [BsonElement("account")] public string Account { get; set; }
        [BsonElement("password")] public string Password { get; set; }
        [BsonElement("role_id")] public string? RoleId { get; set; }
        [BsonElement("department_id")] public string? DepartmentId { get; set; }
        [BsonElement("real_name")] public string RealName { get; set; }
        [BsonElement("avatar")] public string Avatar { get; set; }
    }
}