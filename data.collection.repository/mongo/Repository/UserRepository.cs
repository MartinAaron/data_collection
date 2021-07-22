using data.collection.entity.Entity;
using data.collection.util.DI;
using MongoDB.Driver;

namespace data.collection.repository.mongo.Repository
{
    public class UserRepository : MongoDbContext<User>, IScopedDependency
    {
        public UserRepository(IMongoDatabase db) : base(db)
        {
        }
    }
}