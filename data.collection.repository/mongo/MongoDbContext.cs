using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.entity.Model;
using MongoDB.Driver;

namespace data.collection.repository.mongo
{
    public class MongoDbContext<T> where T : MongoEntityBase
    {
        protected MongoDbContext(IMongoDatabase db)
        {
            _db = db;
        }

        private readonly IMongoDatabase _db;
        private IMongoCollection<T> Entities => _db.GetCollection<T>(typeof(T).Name.ToLower());

        public async Task<IMongoCollection<T>> GetList()
        {
            await Task.CompletedTask;
            return Entities;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await Entities.DeleteOneAsync(x => x.Id == id);
            return deleteResult.DeletedCount != 0;
        }

        public async Task<List<T>> GetListPaged(int skip = 0, int count = 0)
        {
            var result = await Entities.Find(x => true)
                .Skip(skip)
                .Limit(count)
                .ToListAsync();
            return result;
        }

        public async Task<T> GetById(string id)
        {
            return await Entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(T t)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, t.Id);
            var replaceOneResult = await Entities.ReplaceOneAsync(doc => doc.Id == t.Id, t);
            return replaceOneResult.ModifiedCount != 0;
        }

        public async Task<string> Create(T t)
        {
            await Entities.InsertOneAsync(t);
            return t._id.ToString();
        }

        public async Task<List<T>> GetListByField(string fieldName, string fieldValue)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, fieldValue);
            var result = await Entities.Find(filter).ToListAsync();
            return result;
        }
    }
}