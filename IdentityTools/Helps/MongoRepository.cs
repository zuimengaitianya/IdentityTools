using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTools.Helps
{
    public class MongoRepository //: IRepository
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        //public MongoRepository(IOptions<ConfigurationOptions> optionsAccessor)
        //{
        //    _client = new MongoClient(configurationOptions.MongoConnection);
        //    _database = _client.GetDatabase(configurationOptions.MongoDatabaseName);
        //}

        public MongoRepository()
        {
            _client = new MongoClient(new MongoClientSettings()
            {
                Servers = new List<MongoServerAddress>
                {
                    new MongoServerAddress("127.0.0.1",12701)
                }
            });
            _database = _client.GetDatabase("IdentityServer");
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return _database.GetCollection<T>(typeof(T).Name).AsQueryable();
        }

        public IQueryable<T> Where<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression);
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var result = _database.GetCollection<T>(typeof(T).Name).DeleteMany(predicate);

        }
        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public bool CollectionExists<T>() where T : class, new()
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);
            var filter = new BsonDocument();
            var totalCount = collection.CountDocuments(filter);
            return (totalCount > 0) ? true : false;

        }

        public void Add<T>(T item) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertOne(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            _database.GetCollection<T>(typeof(T).Name).InsertMany(items);
        }
    }
}
